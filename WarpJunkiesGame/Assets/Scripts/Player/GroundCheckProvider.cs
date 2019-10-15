using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckProvider : MonoBehaviour, IGroundChecker
{
    [SerializeField] private float _distanceToGround = 0.3f;
    [SerializeField] private bool _showDebug = true;
    public bool CheckIsGrounded(CharacterController controller)
    {
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x,
                                           (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,
                                           controller.bounds.center.z),
                                Vector3.down);
        if (_showDebug)
        {
            Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);
        }

        //controller.Col
        RaycastHit hit;
        if(Physics.Raycast(groundRay,out hit ,_distanceToGround))
        {
            if(hit.collider.tag == "Ground")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
