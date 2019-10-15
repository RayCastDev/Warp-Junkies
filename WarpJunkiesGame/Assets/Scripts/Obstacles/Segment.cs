using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public const int MaxDispersionLength = 25;
    public const int MinDispersionLength = 12;
    public int SegId { get; set; }
    public bool transition;

    public int length;
    public int beginY1, beginY2, beginY3;
    public int endY1, endY2, endY3;

    private List<Piece> pieces;
    private Piece allLanePiece;
    private void Awake()
    {
       
    }


    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void DeSpawn()
    {
        Destroy(gameObject);
    }

    public void Construct(DefaultObstaclesGenerator generator)
    {

        bool AllLane = Random.Range(0f, 1f) < 0.1;

        if (AllLane && generator.AllLaneJump.Count !=0)
        {
            allLanePiece = generator.AllLaneJump[Random.Range(0, generator.AllLaneJump.Count)];
            GameObject go = Instantiate(allLanePiece.gameObject);
            go.transform.SetParent(transform, false);
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
                        pieces.Add(generator.walls[Random.Range(0, generator.walls.Count)]);
                        countOfWall++;
                        break;
                    case 2:
                        pieces.Add(generator.barriers[0]);

                        break;
                    case 3:
                        pieces.Add(generator.slides[0]);
                        break;
                    case 4:
                        pieces.Add(generator.jumps[0]);
                        break;
                    case 5:
                        pieces.Add(null);
                        break;
                }
            }

            length = Random.Range(MinDispersionLength, MaxDispersionLength);

            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i] != null)
                {
                    GameObject go = Instantiate(pieces[i].gameObject);
                    go.transform.SetParent(transform, false);

                    switch (i)
                    {
                        case 0:
                            go.transform.position = new Vector3(-3, 0, Random.Range(-4, 4));
                            break;
                        case 1:
                            go.transform.position = new Vector3(0, 0, 0);
                            break;
                        case 2:
                            go.transform.position = new Vector3(3, 0, Random.Range(-4, 4));
                            break;
                    }
                }
            }
        }


    
            
        
    }
}
