using System;
using UniRx;
using Unity1week202112.Domain;
using VContainer.Unity;

namespace Unity1week202112.Presentation
{
    public class GameTurnPresenter : IStartable, IDisposable
    {
        private readonly GameTurn _gameTurn;
        private readonly ITurnView _view;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public GameTurnPresenter(GameTurn gameTurn, ITurnView view)
        {
            _gameTurn = gameTurn;
            _view = view;
        }

        public void Start()
        {
            _gameTurn.TurnCount
                .Subscribe(count => _view.RenderTurn(count))
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
