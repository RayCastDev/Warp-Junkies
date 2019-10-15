using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private LevelBlocksManager _levelBlocksManager;
    // Start is called before the first frame update
    void Start()
    {
        _levelBlocksManager = GameObject.FindObjectOfType<LevelBlocksManager>();     
    }

    private void OnBecameInvisible()
    {
        //Debug.Log("Recyrcle");
        //_levelBlocksManager.RecycleBlock(this.gameObject);
        //transform.position += new Vector3(0,-0,150f);
    }
}
