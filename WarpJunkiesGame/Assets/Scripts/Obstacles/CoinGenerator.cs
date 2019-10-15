using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour, ISegmentObstaclesGenerator
{
    public GameObject coinsLine;
    public List<GameObject> SegmentsCoins;
    public void GenerateLevelObstacles(bool isClear)
    {
        foreach (var item in SegmentsCoins)
        {
            Destroy(item.gameObject);
        }
        SegmentsCoins.Clear();

        if (!isClear)
        {
            float linePos = -0.1f;
            for (int i = 0; i < 3; i++)
            {
                if (Random.Range(0, 1f) > 0.3f)
                {
                    GameObject go = Instantiate(coinsLine);
                    go.transform.SetParent(transform);
                    go.transform.localPosition = new Vector3(linePos, 1.41f, 0);
                    SegmentsCoins.Add(go);
                }
                linePos += 0.1f;
            }
        }
    }

    public void HideObstacles()
    {
        throw new System.NotImplementedException();
    }

    public void ShowObstacles()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
