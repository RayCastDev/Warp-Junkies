using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public PieceType type;
    private Piece currentPiece;

    public void Spawn()
    {
        int amtObj = 0;
        switch (type)
        {
            case PieceType.barrier:
                amtObj = ObstaclesManager.Instance.barriers.Count;
                break;
            case PieceType.jump:
                amtObj = ObstaclesManager.Instance.jumps.Count;
                break;
            case PieceType.slide:
                amtObj = ObstaclesManager.Instance.slides.Count;
                break;
            case PieceType.wall:
                amtObj = ObstaclesManager.Instance.walls.Count;
                break;
        }


        currentPiece = ObstaclesManager.Instance.GetPiece(type, UnityEngine.Random.Range(0, amtObj));
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }

    public void DeSpawn()
    {
        currentPiece.gameObject.SetActive(false);
    }
}
