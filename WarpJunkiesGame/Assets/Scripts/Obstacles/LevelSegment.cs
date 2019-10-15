using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public class LevelSegment : MonoBehaviour
    {
        public ISegmentObstaclesGenerator segmentObstaclesGenerator { get; set; }
        [SerializeField] private float _levelSegmentLength = 0f;
        public string LevelOwner { get; set; }
        public float LevelSgmentLenguh
        {
            get { return _levelSegmentLength; }
        }
        public void Awake()
        {
            segmentObstaclesGenerator = GetComponent<ISegmentObstaclesGenerator>();
        }

        public void SetSegmentLayer(int layerMask)
        {
            SetLayerRecursively(this.gameObject, layerMask);
        }

        public void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (null == obj)
            {
                return;
            }

            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                if (null == child)
                {
                    continue;
                }
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}
