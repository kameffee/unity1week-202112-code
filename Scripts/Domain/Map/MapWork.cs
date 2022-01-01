using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Unity1week202112.Domain.Map
{
    /// <summary>
    /// マップの歩き方
    /// </summary>
    public sealed class MapWork
    {
        public IObservable<MapPoint> OnMove => _onMove;
        private readonly Subject<MapPoint> _onMove = new Subject<MapPoint>();

        public IObservable<Unit> OnArrived => _onArrived;
        private readonly Subject<Unit> _onArrived = new Subject<Unit>();

        // 現在の位置
        public IReadOnlyReactiveProperty<MapPoint> CurrentPoint => _currentPoint;
        private readonly ReactiveProperty<MapPoint> _currentPoint = new ReactiveProperty<MapPoint>();

        public void Initialize(MapPoint initialPoint)
        {
            _currentPoint.Value = initialPoint;
        }

        public async UniTask Move(MapPoint mapPointId)
        {
            _onMove.OnNext(mapPointId);

            // 着くまで待つ
            await _onArrived.ToUniTask(true);
        }

        /// <summary>
        /// 次へ行けるルート
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MapPoint> GetNextRoot()
        {
            // 1本道の7エリアまで
            if (_currentPoint.Value.PointId + 1 > 6)
            {
                return Array.Empty<MapPoint>();
            }

            // Todo: 分岐が実装できたらする
            return new List<MapPoint>()
            {
                new MapPoint(_currentPoint.Value.PointId + 1),
            };
        }

        public bool HasNext()
        {
            return GetNextRoot().Any();
        }

        /// <summary>
        /// 終了判定
        /// </summary>
        public bool IsEnd()
        {
            return _currentPoint.Value.PointId > 6;
        }

        /// <summary>
        /// 到着
        /// </summary>
        public void Arrived(MapPoint mapPoint)
        {
            _currentPoint.Value = mapPoint;
            _onArrived.OnNext(Unit.Default);
        }
    }
}
