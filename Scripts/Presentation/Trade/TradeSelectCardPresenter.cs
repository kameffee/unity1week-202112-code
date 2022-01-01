using System;
using Cysharp.Threading.Tasks;
using Kameffee.AudioPlayer;
using UniRx;
using Unity1week202112.Domain.Command;
using Unity1week202112.Presentation.CommandCard;
using Unity1week202112.Presentation.EffectDetail;
using VContainer;

namespace Unity1week202112.Presentation.Trade
{
    /// <summary>
    /// コマンドカード
    /// </summary>
    public sealed class TradeSelectCardPresenter : IDisposable
    {
        [Inject]
        private AudioPlayer _audioPlayer;
        
        private readonly TradeSelectCardModel _model;
        private readonly CommandCardView _view;
        private readonly CommandEffectIconProvider _commandEffectIconProvider;
        private readonly Func<int, CommandEffectDetailModel> _dialogFactory;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public TradeSelectCardPresenter(
            TradeSelectCardModel model,
            CommandCardView view,
            CommandEffectIconProvider commandEffectIconProvider,
            Func<int, CommandEffectDetailModel> dialogFactory)
        {
            _model = model;
            _view = view;
            _commandEffectIconProvider = commandEffectIconProvider;
            _dialogFactory = dialogFactory;
        }

        public void Initialize()
        {
            _view.SetNum(_model.CommandCardModel.EffectNum);

            // アイコン
            _commandEffectIconProvider.GetIcon((int)_model.CommandCardModel.CommandEffect.EffectType)
                .ToObservable()
                .Subscribe(sprite => _view.SetImage(sprite))
                .AddTo(_disposable);
            
            _model.CommandCardModel.Visible
                .Where(visible => visible)
                .Subscribe(visible => _view.Show())
                .AddTo(_disposable);

            _model.CommandCardModel.Selected
                .SkipLatestValueOnSubscribe()
                .Subscribe(isSelected =>
                {
                    _view.SetEnableFeedBack(!isSelected);
                    _view.SetSelectedState(isSelected, false);
                })
                .AddTo(_disposable);

            // 選択可能状態
            _model.CommandCardModel.Selectable
                .Subscribe(selectable =>
                {
                    _view.SetEnableFeedBack(selectable);
                    _view.SetSelectableState(selectable);
                })
                .AddTo(_disposable);

            // 使用制限状態
            _model.CommandCardModel.LockUseStatus
                .Subscribe(state => _view.SetLockCount(state.RemainingCount))
                .AddTo(_disposable);

            _model.CommandCardModel.Selectable
                .Subscribe(selectable => _view.SetEnableFeedBack(selectable))
                .AddTo(_disposable);

            // 選択フェーズ中 & クリックせれた時
            _view.OnClick
                .Subscribe(_ =>
                {
                    _model.CommandCardModel.SetSelected(true);
                    _model.Select();
                })
                .AddTo(_disposable);
            
            // 消失させる
            _model.CommandCardModel.OnUse
                .Subscribe(async _ =>
                {
                    _view.SetEnableFeedBack(false);
                    await _view.PlayUseAnimation(default);
                })
                .AddTo(_disposable);

            // _view.OnLongPress
            //     .Merge(_view.OnRightClick)
            //     .Subscribe(_ => _dialogFactory.Invoke((int)_model.CommandCardModel.CommandEffect.EffectType))
            //     .AddTo(_disposable);
        }
        
        public async UniTask PlayInAnimation(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            await _view.PlayInAnimation();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _view?.Delete();
        }
    }
}
