using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Unity1week202112.Presentation
{
    public class SettingsWindowPresenter : IDisposable
    {
        private readonly SettingsWindow _view;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public SettingsWindowPresenter(SettingsWindow view)
        {
            _view = view;
        }

        public async UniTask Open(CancellationToken cancellationToken)
        {
            _view.OnClickClose
                .Subscribe(async _ =>
                {
                    await _view.Close(default);
                    Dispose();
                })
                .AddTo(_disposable);

            await _view.Open(cancellationToken);
        }

        public async UniTask Close(CancellationToken cancellationToken)
        {
            await _view.Close(cancellationToken);
            Dispose();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _view.Dispose();
        }
    }
}
