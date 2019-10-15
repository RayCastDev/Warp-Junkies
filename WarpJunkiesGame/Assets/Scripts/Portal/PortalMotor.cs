using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMotor : MonoBehaviour
{
    public GameObject Render;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine("RenderVisible");
            Render.SetActive(false);
            Debug.Log("OnTrigger");
            LevelBlocksManager.Instance.EnterInPortal();
        }
    }

    IEnumerator RenderVisible()
    {
        yield return new WaitForSeconds(3);
        Render.SetActive(true);
    }
}
