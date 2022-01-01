using System;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Command;

namespace Unity1week202112.Presentation.CommandCard
{
    /// <summary>
    /// 相手プレイヤーのカード
    /// </summary>
    public class OtherPlayerCommandCardPresenter : ICommandCardPresenter, IDisposable
    {
        private readonly CommandEffectIconProvider _commandEffectIconProvider;
        private readonly CommandCardModel _model;
        private readonly ICommandCardView _view;
        private readonly IWaitCardSelect _waitCardSelect;
        private readonly FieldCardsModel _fieldCardsModel;
        private readonly CardSlotList _cardSlotList;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public OtherPlayerCommandCardPresenter(
            CommandCardModel model,
            ICommandCardView view,
            IWaitCardSelect waitCardSelect,
            CommandEffectIconProvider commandEffectIconProvider,
            FieldCardsModel fieldCardsModel,
            CardSlotList cardSlotList)
        {
            _commandEffectIconProvider = commandEffectIconProvider;
            _model = model;
            _view = view;
            _waitCardSelect = waitCardSelect;
            _fieldCardsModel = fieldCardsModel;
            _cardSlotList = cardSlotList;
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

            // 選択状態
            _model.Selected
                .SkipLatestValueOnSubscribe()
                .Subscribe(isSelected => _view.SetSelectedState(isSelected))
                .AddTo(_disposable);

            // 選択可能状態
            _model.Selectable
                .Subscribe(selectable => _view.SetSelectableState(selectable))
                .AddTo(_disposable);

            // 使用制限状態
            _model.LockUseStatus
                .Subscribe(lockUseState => _view.SetLockCount(lockUseState.RemainingCount))
                .AddTo(_disposable);

            // 使用された時
            _model.OnUse
                .Subscribe(async _ =>
                {
                    _waitCardSelect.Select(_model);

                    await _view.PlayUseAnimation(default);
                    _fieldCardsModel.RemoveCard(_model);
                    _view.Delete();

                    _fieldCardsModel.UpdateCardsStatus();
                    Dispose();
                })
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
