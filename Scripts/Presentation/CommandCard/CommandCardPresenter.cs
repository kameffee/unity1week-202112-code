using System;
using Cysharp.Threading.Tasks;
using Kameffee.AudioPlayer;
using UniRx;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Command;
using Unity1week202112.Presentation.EffectDetail;
using VContainer;

namespace Unity1week202112.Presentation.CommandCard
{
    public interface ICommandCardPresenter
    {
        UniTaskVoid Initialize();

        UniTask PlayInAnimation();
    }

    /// <summary>
    /// 1コマンドカード
    /// </summary>
    public class CommandCardPresenter : ICommandCardPresenter, IDisposable
    {
        private readonly CommandEffectIconProvider _commandEffectIconProvider;
        private readonly CommandCardModel _model;
        private readonly ICommandCardView _view;
        private readonly IWaitCardSelect _waitCardSelect;
        private readonly ICardSelectPhaseHandler _cardSelectPhaseHandler;
        private readonly FieldCardsModel _fieldCardsModel;
        private readonly CardSlotList _cardSlotList;
        private readonly Func<int, CommandEffectDetailModel> _dialogFactory;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public CommandCardPresenter(
            CommandCardModel model,
            ICommandCardView view,
            IWaitCardSelect waitCardSelect,
            CommandEffectIconProvider commandEffectIconProvider,
            ICardSelectPhaseHandler cardSelectPhaseHandler,
            FieldCardsModel fieldCardsModel,
            CardSlotList cardSlotList,
            Func<int, CommandEffectDetailModel> dialogFactory)
        {
            _commandEffectIconProvider = commandEffectIconProvider;
            _model = model;
            _view = view;
            _waitCardSelect = waitCardSelect;
            _cardSelectPhaseHandler = cardSelectPhaseHandler;
            _fieldCardsModel = fieldCardsModel;
            _cardSlotList = cardSlotList;
            _dialogFactory = dialogFactory;
        }

        public async UniTaskVoid Initialize()
        {
            _view.SetNum(_model.EffectNum);

            _model.Position
                .SkipLatestValueOnSubscribe()
                .Subscribe(pos =>
                {
                    var toPos = _cardSlotList.GetHolder(pos).anchoredPosition;
                    _view.Move(toPos);
                })
                .AddTo(_disposable);

            _model.Visible
                .Where(visible => visible)
                .Subscribe(visible => _view.Show())
                .AddTo(_disposable);

            _model.Selected
                .SkipLatestValueOnSubscribe()
                .Subscribe(isSelected =>
                {
                    _view.SetEnableFeedBack(!isSelected);
                    _view.SetSelectedState(isSelected);
                })
                .AddTo(_disposable);

            // 選択可能状態
            _model.Selectable
                .Subscribe(selectable =>
                {
                    _view.SetEnableFeedBack(selectable);
                    _view.SetSelectableState(selectable);
                })
                .AddTo(_disposable);

            // 使用制限状態
            _model.LockUseStatus
                .Subscribe(state => _view.SetLockCount(state.RemainingCount))
                .AddTo(_disposable);

            // 使用された時
            _model.OnUse
                .Subscribe(async _ =>
                {
                    _view.SetEnableFeedBack(false);
                    await _view.PlayUseAnimation(default);
                    _fieldCardsModel.RemoveCard(_model);
                    _view.Delete();

                    _fieldCardsModel.UpdateCardsStatus();
                    Dispose();
                })
                .AddTo(_disposable);

            _cardSelectPhaseHandler.OnPhase
                .Select(onPhase => onPhase && _model.Selectable.Value)
                .Subscribe(onPhase => _view.SetEnableFeedBack(onPhase))
                .AddTo(_disposable);

            // 選択フェーズ中 & クリックせれた時
            _view.OnClick
                .Where(_ => _cardSelectPhaseHandler.OnPhase.Value)
                .Where(_ => _model.Selectable.Value)
                .Where(_ => !_model.LockUseStatus.Value.Avairable)
                .Subscribe(_ => _waitCardSelect.Select(_model))
                .AddTo(_disposable);

            _view.OnLongPress
                .Merge(_view.OnRightClick)
                .Subscribe(_ => _dialogFactory.Invoke((int)_model.CommandEffect.EffectType))
                .AddTo(_disposable);

            _commandEffectIconProvider.GetIcon((int)_model.CommandEffect.EffectType)
                .ToObservable()
                .Subscribe(sprite => _view.SetImage(sprite))
                .AddTo(_disposable);
        }

        public async UniTask PlayInAnimation()
        {
            await _view.PlayInAnimation();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
