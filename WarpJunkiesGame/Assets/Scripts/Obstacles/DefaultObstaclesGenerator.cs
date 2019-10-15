using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultObstaclesGenerator : MonoBehaviour , ISegmentObstaclesGenerator
{
    [Header("Obstacles For Level Segment")]
    public List<Piece> walls = new List<Piece>();
    public List<Piece> jumps = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    public List<Piece> barriers = new List<Piece>();
    public List<Piece> AllLaneJump = new List<Piece>();
    // Start is called before the first frame update

    //public const int MaxDispersionLength = 25;
    //public const int MinDispersionLength = 12;
    //public int length;
    private List<Piece> pieces;
    private Piece allLanePiece;

    private bool isHide = false;

    float startZPosition = -0.3f;
    public GameObject segmentContainer;
    public List<GameObject> LevelBlockSegments;

    public void Awake()
    {
        LevelBlockSegments = new List<GameObject>();
    }
    public  void GenerateLevelObstacles(bool isClear)
    {
            foreach (var item in LevelBlockSegments)
            {
                Destroy(item.gameObject);
            }
            LevelBlockSegments.Clear();
            startZPosition = -0.28f;
            if (!isClear)
            {
                for (int i = 0; i < 3; i++)
                {
                    GenerateSegment(startZPosition);
                    startZPosition += 0.28f;
                }
            }       
    }

    public  void HideObstacles()
    {
        isHide = true;
        foreach (var item in LevelBlockSegments)
        {
            item.SetActive(false);
        }
    }

    public  void ShowObstacles()
    {
        isHide = false;
        foreach (var item in LevelBlockSegments)
        {
            item.SetActive(true);
        }
    }


    private void GenerateSegment(float Zoffset)
    {   
        if (Random.Range(0f, 1f) < 0.25)
        {
            SpawnSegment(true, Zoffset );
        }
        else
        {
            SpawnSegment(false, Zoffset);
        }
    }

    private void SpawnSegment(bool isTransition, float zOffset)
    {
            if (isTransition) return;

            GameObject segment_Go = Instantiate(segmentContainer);
            //Segment segment = segment_Go.GetComponent<Segment>();         
            Construct(segment_Go);
            segment_Go.transform.SetParent(transform);

            segment_Go.transform.localPosition = new Vector3(0, 1.41f, zOffset);
            segment_Go.SetActive(!isHide);
            LevelBlockSegments.Add(segment_Go);
            
    }

    public void Construct(GameObject segmentContainer)
    {

        bool AllLane = Random.Range(0f, 1f) < 0.1;

        if (AllLane && AllLaneJump.Count != 0)
        {
            allLanePiece = AllLaneJump[Random.Range(0, AllLaneJump.Count)];
            GameObject go = Instantiate(allLanePiece.gameObject);
            go.transform.SetParent(segmentContainer.transform, false);
            go.transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            int countOfWall = 0;
            pieces = new List<Piece>();
            for (int i = 0; i < 3; i++)
            {
                switch (countOfWall != 2 ? Random.Range(1, 6) : Random.Range(2, 6))
                {
                    case 1:
                        pieces.Add(walls[Random.Range(0, walls.Count)]);
                        countOfWall++;
                        break;
                    case 2:
                        pieces.Add(barriers[0]);

                        break;
                    case 3:
                        pieces.Add(slides[0]);
                        break;
                    case 4:
                        pieces.Add(jumps[0]);
                        break;
                    case 5:
                        pieces.Add(null);
                        break;
                }
            }

            //length = Random.Range(MinDispersionLength, MaxDispersionLength);

            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i] != null)
                {
                    GameObject go = Instantiate(pieces[i].gameObject);
                    go.transform.SetParent(segmentContainer.transform, false);

                    switch (i)
                    {
                        case 0:
                            go.transform.position = new Vector3(-3, 0, Random.Range(-2, 2));
                            break;
                        case 1:
                            go.transform.position = new Vector3(0, 0, 0);
                            break;
                        case 2:
                            go.transform.position = new Vector3(3, 0, Random.Range(-2, 2));
                            break;
                    }
                }
            }
        }
    }

}
