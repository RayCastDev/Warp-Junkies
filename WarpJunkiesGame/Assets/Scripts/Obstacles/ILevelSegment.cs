using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Obstacles
{
    public interface ILevelSegment
    {
        ISegmentObstaclesGenerator segmentObstaclesGenerator { get; set; }
        string LevelOwner { get; set; }
        void SetSegmentLayer(int layerMask);
    }
}
