using System;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity1week202112.Domain.Map;
using VContainer.Unity;

namespace Unity1week202112.Presentation.Map
{
    /// <summary>
    /// フィール上のアリ
    /// </summary>
    public class FieldPlayerPresenter : IInitializable, IDisposable
    {
        private readonly MapWork _work;
        private readonly IFieldPlayerView _view;
        private readonly MapFieldBattlePointList _fieldBattlePointList;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public FieldPlayerPresenter(MapWork work,
            IFieldPlayerView view,
            MapFieldBattlePointList fieldBattlePointList)
        {
            _work = work;
            _view = view;
            _fieldBattlePointList = fieldBattlePointList;
        }

        public void Initialize()
        {
            _work.OnMove
                .Subscribe(async point => Move(point).Forget())
                .AddTo(_disposable);
        }

        private async UniTaskVoid Move(MapPoint point)
        {
            var battlePoint = _fieldBattlePointList.Get(point);
            if (battlePoint != null)
            {
                await _view.Move(battlePoint.WorldPosition);
            }

            // 到着
            _work.Arrived(point);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
