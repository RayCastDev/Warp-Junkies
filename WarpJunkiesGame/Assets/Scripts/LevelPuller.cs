using Assets.Scripts.Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPuller : MonoBehaviour
{   
    [System.Serializable]
    public struct Level
    {
        public string levelName;
        public LevelSegment[] levelSegments;
        public int levelPoolSize;
    }

    public List<Level> levels;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public static LevelPuller Instance;

    public GameObject Portal;
    void Awake()
    {
        Instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Level level in levels)
        {
            Queue<GameObject> levelPool = new Queue<GameObject>();
            for (int i = 0; i < level.levelPoolSize; i++)
            {
                GameObject obj = Instantiate(level.levelSegments[Random.Range(0,level.levelSegments.Length)].gameObject);
                obj.SetActive(false);
                levelPool.Enqueue(obj);
            }
            poolDictionary.Add(level.levelName, levelPool);
        }
    }

    public LevelSegment SpawnFromPool (string tag, Vector3 position)
    {
   
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " not found!");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        poolDictionary[tag].Enqueue(objectToSpawn);
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;

        LevelSegment levelSegment = objectToSpawn.GetComponent<LevelSegment>();
        levelSegment.LevelOwner = tag;
        return levelSegment;
    }

    public void HideAllLevel(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " not found!");
            return;
        }

        foreach (GameObject levelBlock in poolDictionary[tag])
        {
            levelBlock.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            foreach (GameObject levelBlock in poolDictionary["Desert"])
            {
                levelBlock.GetComponent<ISegmentObstaclesGenerator>().HideObstacles();
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            bool first = true;
            foreach (GameObject levelBlock in poolDictionary["Desert"])
            {
                if(first)
                {
                    first = false;
                    continue;

                }
                levelBlock.GetComponent<ISegmentObstaclesGenerator>().ShowObstacles();
            }

        }
    }

    public void SpawnPortal(Vector3 position)
    {
        Portal.SetActive(true);
        Portal.transform.position = position;
    }

    public void DisablePortal()
    {
        Portal.SetActive(false);
    }

    public List<string> GetLevelsNames()
    {
        List<string> levelsName = new List<string>();

        foreach(Level level in levels)
        {
            levelsName.Add(level.levelName);
        }
        return levelsName;
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
