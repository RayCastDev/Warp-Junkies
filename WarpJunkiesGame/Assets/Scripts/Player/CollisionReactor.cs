using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionReactor : MonoBehaviour
{
    [SerializeField] private string _obstaclesTag = "Obstacle";

    public GameEvent gameOver;
    CharacterController characterController;
    private bool isGodMode = false;
    public Material hologram;
    public Material defaulte;
    public List<SkinnedMeshRenderer> MeshRenderer;

    public PlayerEngine playerEngine;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerEngine = GetComponent<PlayerEngine>();
    }
    //public event Action Faced = delegate { };
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if(hit.gameObject.tag == _obstaclesTag)
        {
            if (isGodMode)
            {
                Physics.IgnoreCollision(hit.collider, characterController);
            }
            else
            {
                gameOver.Raise();
                //Faced();
            }
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "God")
        {
            Destroy(other.gameObject);
            StartCoroutine("ACtivateGodMode");
            Debug.Log("GodMode Activate");
        }
    }



    IEnumerator ACtivateGodMode()
    {
        playerEngine.MultiplySpeed(2f);
        foreach (SkinnedMeshRenderer smr in MeshRenderer)
        {
            smr.material = hologram;
           
        }
        isGodMode = true;
        yield return new WaitForSeconds(6);
       
        playerEngine.DivideSpeed(2f);
       
        yield return new WaitForSeconds(1);
        foreach (SkinnedMeshRenderer smr in MeshRenderer)
        {

            smr.material = defaulte;
        }
        isGodMode = false;
    }
}
