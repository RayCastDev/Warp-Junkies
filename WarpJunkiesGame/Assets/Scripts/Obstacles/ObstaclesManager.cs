using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    public static ObstaclesManager Instance { get; set; }

    public bool SHOW_COLLIDER = true;

    private const float DISTANCE_BEFORE_SPAWN = 150;
    private const int INITIAL_SEGMENTS = 8;
    private const int MAX_SEGMENT_ON_SCREEN = 12;
    private Transform cameraContainer;
    private int amountofActiveSegments;
    private int continiousSegments;
    private int currentSpawnZ;
    //private int currentLevel;
    private int y1, y2, y3;


    public List<Piece> walls = new List<Piece>();
    public List<Piece> jumps = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    public List<Piece> barriers = new List<Piece>();
    public List<Piece> pieces = new List<Piece>();


    public Segment availableSegment;
    public List<Segment> segments = new List<Segment>();

    //private bool isMoving = false;


    private void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
        //currentLevel = 0;
    }

    //Generation initial segments
    private void Start()
    {
        for (int i = 0; i < INITIAL_SEGMENTS; i++)
        {
            GenerateSegment();
        }
    }

    private void Update()
    {
        if (currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
        {
            GenerateSegment();
        }

        if (amountofActiveSegments >= MAX_SEGMENT_ON_SCREEN)
        {
            segments[amountofActiveSegments - 1].DeSpawn();
            segments.RemoveAt(amountofActiveSegments - 1);
            amountofActiveSegments--;
        }
    }

    private void GenerateSegment()
    {
        SpawnSegment();

        if (UnityEngine.Random.Range(0f, 1f) < (continiousSegments * 0.1f))
        {
            continiousSegments = 0;
            SpawnTransition();
        }
        else
        {
            continiousSegments++;
        }

    }

    private void SpawnTransition()
    {
        //List<Segment> possibletransition = availableTransitons.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        //int id = UnityEngine.Random.Range(0, possibletransition.Count);

        GameObject segment_Go = Instantiate(availableSegment).gameObject;
        Segment segment = segment_Go.GetComponent<Segment>();

        //segment.Construct(true);

        //y1 = s.endY1;
        //y2 = s.endY2;
        //y3 = s.endY3;

        segment.transform.SetParent(transform);
        segment.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += segment.length;
        amountofActiveSegments++;
        segment.Spawn();
        segments.Insert(0, segment);
    }

    private void SpawnSegment()
    {
        //?
        //List<Segment> possibleSeg = availableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);

        //Take random Segment
        //int id = UnityEngine.Random.Range(0, possibleSeg.Count);

        GameObject segment_Go = Instantiate(availableSegment).gameObject;

        Segment segment = segment_Go.GetComponent<Segment>();


        //segment.Construct(false);
        //Segment s = GetSegment(id, false);

        //y1 = s.endY1;
        //y2 = s.endY2;
        //y3 = s.endY3;

        segment.transform.SetParent(transform);
        segment.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += segment.length;
        amountofActiveSegments++;
        segment.Spawn();
        segments.Insert(0, segment);
    }

    public Segment GetSegment(int id, bool transition)
    {
        Segment s = null;
        s = segments.Find(x => x.SegId == id && x.transition == transition && !x.gameObject.activeSelf);

        if (s == null)
        {
            //GameObject go = Instantiate((transition) ? availableTransitons[id].gameObject : availableSegments[id].gameObject) as GameObject;
            // s = go.GetComponent<Segment>();

            s.SegId = id;
            s.transition = transition;

            segments.Insert(0, s);
        }
        else
        {
            segments.Remove(s);
            segments.Insert(0, s);
        }
        return s;
    }

    public Piece GetPiece(PieceType pt, int visualIndex)
    {
        Piece p = pieces.Find(x => x.type == pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if (p == null)
        {
            GameObject go = null;
            if (pt == PieceType.wall)
            {
                go = walls[visualIndex].gameObject;
            }
            else if ((pt == PieceType.barrier))
            {
                go = barriers[visualIndex].gameObject;
            }
            else if ((pt == PieceType.jump))
            {
                go = jumps[visualIndex].gameObject;
            }
            else if ((pt == PieceType.slide))
            {
                go = slides[visualIndex].gameObject;
            }

            go = Instantiate(go);
            p = go.GetComponent<Piece>();

            pieces.Add(p);
        }


        return p;
    }
}
