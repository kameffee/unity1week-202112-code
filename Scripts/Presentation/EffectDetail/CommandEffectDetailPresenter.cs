using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Unity1week202112.Presentation.EffectDetail
{
    public class CommandEffectDetailPresenter : IDisposable
    {
        private readonly CommandEffectDetailModel _model;
        private readonly CommandEffectDetailView _view;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public CommandEffectDetailPresenter(CommandEffectDetailModel model, CommandEffectDetailView view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize()
        {
            _model.Initialize();

            // 初期化
            _view.SetEffectName(_model.EffectName);
            _view.SetDescription(_model.Description);

            _model.Icon
                .Subscribe(icon => _view.SetImage(icon))
                .AddTo(_disposable);

            _view.OnCloseEvent
                .Merge(_view.OnClickBackground)
                .Subscribe(_ => _model.Close())
                .AddTo(_disposable);

            _model.OnClose
                .Subscribe(async _ =>
                {
                    await _view.Close();
                    Dispose();
                })
                .AddTo(_disposable);

            _view.Open().Forget();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _model?.Dispose();
        }
    }
}
