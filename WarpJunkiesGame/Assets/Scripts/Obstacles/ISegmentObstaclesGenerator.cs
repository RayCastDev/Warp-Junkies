using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISegmentObstaclesGenerator
{
    /// <summary>
    /// Метод, который гененирует препятсвия в своём сегменте уровня
    /// </summary>
    /// <param name="isClear">Памаретр, который определяет, будет ли сегмент уровня содержать препятствия</param>
     void GenerateLevelObstacles(bool isClear);
     void HideObstacles();
     void ShowObstacles();
     
}
