using Assets.Scripts.Obstacles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBlocksManager : MonoBehaviour
{
    private const int LEVEL_SEGMENT_LENGTH = 50;
    private const int PLAYER_OFFSET_TO_SPAWN = 30;
    private const int SEGMETS_COUNT_ON_SCREEN = 6;

    int Cam1cullingMask1;
    int Cam1cullingMask2;
    int Cam2cullingMask3;
    int Cam2cullingMask4;


    LevelPuller levelPuller;

    List<string> levelsName;
    string currentLevel;
    string nextLevel;


    public Camera cam2;
    public enum Level
    {
        Desert,
        City,
        Ice
    };

    //public Level currentLevel;
    //public Level nextLevel;

    private Transform playerTransform;
    
    public int levelBlockCountToSwitchLevel = 7;
    public bool startSwitching = false;
    bool portalIsSet = false;
    int zOffset = 0;
    
    public static LevelBlocksManager Instance;
    private void Awake()
    {
        Cam1cullingMask1 = ~(1 << LayerMask.NameToLayer("LevelSwitcher1"));
        Cam1cullingMask2 = ~(1 << LayerMask.NameToLayer("LevelSwitcher2"));

        Cam2cullingMask3 = (1 << LayerMask.NameToLayer("LevelSwitcher1")) |
                           (1 << LayerMask.NameToLayer("VisibleAnyWay"));
        Cam2cullingMask4 = (1 << LayerMask.NameToLayer("LevelSwitcher2")) |
                           (1 << LayerMask.NameToLayer("VisibleAnyWay"));

        Camera.main.cullingMask = Cam1cullingMask2;
        cam2.cullingMask = Cam2cullingMask4;
        Instance = this;
    }
    private void Start()
    {     
        levelPuller = LevelPuller.Instance;
        levelsName = levelPuller.GetLevelsNames();
        currentLevel = levelsName.FirstOrDefault(); 
        

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        InitialiseLevel();
    }

    private void InitialiseLevel()
    {
        ConstructSegmentObstacles(levelPuller.SpawnFromPool(currentLevel, new Vector3(0, -1.41f, zOffset)), true, Camera.main.cullingMask == Cam1cullingMask2 ? 15 : 16);
        zOffset += LEVEL_SEGMENT_LENGTH;
        for (int i = 0; i < SEGMETS_COUNT_ON_SCREEN-1; i++)
        {
            ConstructSegmentObstacles(levelPuller.SpawnFromPool(currentLevel, new Vector3(0, -1.41f, zOffset)), false, Camera.main.cullingMask == Cam1cullingMask2 ? 15 : 16);
            zOffset += LEVEL_SEGMENT_LENGTH;
            levelBlockCountToSwitchLevel--;
        }
    }

    private void ConstructSegmentObstacles(LevelSegment levelSegment, bool IsClear, int layerMask)
    {
        levelSegment.segmentObstaclesGenerator.GenerateLevelObstacles(IsClear);
        levelSegment.SetSegmentLayer(layerMask);
    }


    private void Update()
    {

        //    if (playerTransform.position.z > (zOffset - SEGMETS_COUNT_ON_SCREEN * LEVEL_SEGMENT_LENGTH + PLAYER_OFFSET_TO_SPAWN))
        //{
        //    Debug.Log("JOPA");
        //    levelPuller.SpawnFromPool(currentLevel.ToString(), new Vector3(0, -1.41f, zOffset),false);                 
        //    levelBlockCountToSwitchLevel--;
        //    if (levelBlockCountToSwitchLevel == 0 || startSwitching)
        //    {
        //        startSwitching = true;
        //        if (!portalIsSet)
        //        {
        //            SetPortal();
        //        }
        //        levelPuller.SpawnFromPool(nextLevel.ToString(), new Vector3(0, -1.41f, zOffset),false);             
        //    }
        //    zOffset += 50;
        //}

        if (playerTransform.position.z > (zOffset - SEGMETS_COUNT_ON_SCREEN * LEVEL_SEGMENT_LENGTH + PLAYER_OFFSET_TO_SPAWN))
        {
            if (levelBlockCountToSwitchLevel != 0 && !startSwitching)
            {
                ConstructSegmentObstacles(levelPuller.SpawnFromPool(currentLevel, new Vector3(0, -1.41f, zOffset)), false, Camera.main.cullingMask == Cam1cullingMask2 ? 15 : 16);
                levelBlockCountToSwitchLevel--;
            }
            else if (levelBlockCountToSwitchLevel == 0 || startSwitching)
            {
                startSwitching = true;
                if (!portalIsSet)
                {
                    ConstructSegmentObstacles(levelPuller.SpawnFromPool(currentLevel, new Vector3(0, -1.41f, zOffset)), true, Camera.main.cullingMask == Cam1cullingMask2 ? 15 : 16);

                    levelBlockCountToSwitchLevel--;
                    SetPortal();
                    ConstructSegmentObstacles(levelPuller.SpawnFromPool(nextLevel, new Vector3(0, -1.41f, zOffset)), true, Camera.main.cullingMask == Cam1cullingMask2 ? 16 : 15);
                }
                else
                {
                    ConstructSegmentObstacles(levelPuller.SpawnFromPool(currentLevel, new Vector3(0, -1.41f, zOffset)), true, Camera.main.cullingMask == Cam1cullingMask2 ? 15 : 16);
                    ConstructSegmentObstacles(levelPuller.SpawnFromPool(nextLevel, new Vector3(0, -1.41f, zOffset)), false, Camera.main.cullingMask == Cam1cullingMask2 ? 16 : 15);
                    levelBlockCountToSwitchLevel--;
                }
            }
            zOffset += LEVEL_SEGMENT_LENGTH;
        }

    }

    private void SetPortal()
    {
        levelBlockCountToSwitchLevel = 7;
        nextLevel = LevelChooser();
        levelPuller.SpawnPortal(new Vector3(0, 2.4f, zOffset));
        portalIsSet = true;
        //levelPuller.ChangeLevelLayerMask(nextLevel.ToString(), 12);
    }

    public void EnterInPortal()
    {
        levelPuller.HideAllLevel(currentLevel.ToString());
        currentLevel = nextLevel;
        startSwitching = false;
        portalIsSet = false;
        //levelPuller.ChangeLevelLayerMask(currentLevel.ToString(), 8);
        Camera.main.cullingMask = Camera.main.cullingMask == Cam1cullingMask1 ? Cam1cullingMask2:Cam1cullingMask1;
        cam2.cullingMask = cam2.cullingMask == Cam2cullingMask3 ? Cam2cullingMask4 : Cam2cullingMask3;
    }

    private string LevelChooser()
    {
        string levelNext;
        if (levelsName.Count == 1)
        {
            Debug.LogError("Levels Need To Be More than one");
            levelNext = null;
        }
        else
        {
            do
            {
                levelNext = levelsName[UnityEngine.Random.Range(0, levelsName.Count)];
            }
            while (levelNext == currentLevel);
        }
        return levelNext;
    }
}
