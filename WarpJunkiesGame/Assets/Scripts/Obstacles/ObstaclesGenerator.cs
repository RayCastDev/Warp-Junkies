using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstaclesGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public abstract void GenerateLevelObstacles(bool isClear);
    public abstract void HideObstacles();
    public abstract void ShowObstacles();
}
