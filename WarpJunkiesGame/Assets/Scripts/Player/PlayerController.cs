using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    int layerMask;
    int layerMask2;
    private const float LANE_DISTANCE = 3.0f;
    private bool isRunning = false;

    private Animator anim;
    private CharacterController controller;

    [SerializeField]
    private float jumpForce = 8.0f;
    public float gravity = 12.0f;
    private float vertivalVelocity;

    private float originalSpeed = 10.0f;
    private float speed;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIncreaseAmount = 0.2f;

    private int desiredLane = 1;

    // Start is called before the first frame update
    void Start()
    {
        speed = originalSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();       
    }


    // Update is called once per frame
    private void Update()
    {     
        if (!isRunning)
        {
            return;
        }
        
        //Increase speed per time
        if(Time.time - speedIncreaseLastTick > speedIncreaseTime  )
        {
            speedIncreaseLastTick = Time.time;
            speed += speedIncreaseAmount;
            GameController.Instance.UpdateModifier(speed - originalSpeed);

            //EXPERIMANTAL!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            gravity += 0.2f ;
            jumpForce += 0.06f;
        }

        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);

        if (MobileInput.Instance.SwipeLeft)
        {
            MoveLane(false);         
        }
        if (MobileInput.Instance.SwipeRight)
        {
            MoveLane(true);
        }

        Vector3 targetPosition = transform.position.z * Vector3.forward;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }

        Vector3 moveVector = Vector3.zero;

        if (isGrounded)
        {         
            vertivalVelocity = -0.1f;
            if (MobileInput.Instance.SwipeUp)
            {
                anim.SetTrigger("Jump");               
                vertivalVelocity = jumpForce;              
            }
            else if(MobileInput.Instance.SwipeDown)
            {
                StartSliding();
                Invoke("StopSliding", 0.8f);
            }
        }
        else
        {
            vertivalVelocity -= (gravity * Time.deltaTime);
            if(MobileInput.Instance.SwipeDown)
            {
                anim.SetTrigger("FastDown");
                vertivalVelocity = -jumpForce;
            }       
        }

        moveVector.z = speed;
        moveVector.y = vertivalVelocity;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;
        controller.Move(moveVector * Time.deltaTime);

        Vector3 dir = controller.velocity;
        if (dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, 0.05f);
        }
    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private void StartSliding()
    {
        int SlidingChoose = UnityEngine.Random.Range(0, 2);
        if (SlidingChoose == 0)
        {
            anim.SetBool("Sliding1", true);
        }
        else
        {
            anim.SetBool("Sliding2", true);
        }

        controller.height /= 3;
        controller.center = new Vector3(controller.center.x, controller.center.y/3,controller.center.z);
    }

    private void StopSliding()
    {
        anim.SetBool("Sliding1", false);
        anim.SetBool("Sliding2", false);
        controller.height *= 3;
        controller.center = new Vector3(controller.center.x, controller.center.y * 3, controller.center.z);
    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(new Vector3(
            controller.bounds.center.x,
            (controller.bounds.center.y-controller.bounds.extents.y)+0.2f,
            controller.bounds.center.z),Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

        return Physics.Raycast(groundRay, 0.2f + 0.1f);
  
    }
    public void StartRunning()
    {
        isRunning = true;
        anim.SetTrigger("StartRunning");
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
                break;
        }
    }
    private void Crash()
    {
        anim.SetTrigger("Death");
        isRunning = false;
        //GameController.Instance.IsDead = true;
    }
}
