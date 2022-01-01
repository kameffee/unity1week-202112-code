using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Unity1week202112.Presentation.Tutorial
{
    public class TutorialPresenter : IAsyncStartable
    {
        private readonly ITutorialView _view;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public TutorialPresenter(ITutorialView view)
        {
            _view = view;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _view.StartTutorial(_cancellationTokenSource.Token);
            _view.Dispose();
        }
    }
}
