using System.Collections.Generic;
using System.Linq;
using Unity1week202112.Domain.Map;

namespace Unity1week202112.Presentation.Map
{
    public sealed class MapFieldBattlePointList
    {
        public IReadOnlyList<IBattlePoint> All => _battlePoints;
        
        private readonly IReadOnlyList<IBattlePoint> _battlePoints;

        public MapFieldBattlePointList(IReadOnlyList<IBattlePoint> battlePoints)
        {
            _battlePoints = battlePoints;
        }

        public IBattlePoint Get(MapPoint mapPoint)
        {
            return _battlePoints.FirstOrDefault(point => point.MapPointId == mapPoint.PointId);
        }
    }
}
