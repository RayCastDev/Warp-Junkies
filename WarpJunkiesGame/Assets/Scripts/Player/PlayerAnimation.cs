using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator),typeof(PlayerEngine))]
public class PlayerAnimation : MonoBehaviour
{
    private PlayerEngine playerEngine;
    private Animator animator;

    bool isSLiding = false;
    void Awake()
    {
        playerEngine = GetComponent<PlayerEngine>();
        animator = GetComponent<Animator>();

        playerEngine.Runned   += EnableRunAnimation;
        playerEngine.Grounded += EnableGroundedAnimation;
        playerEngine.Jumped   += EnableJumpAnimation;
        playerEngine.Slided   += EnableSlideAnimation;
        playerEngine.Falled   += EnableFallAnimation;
       // playerEngine.Dead     += EnableDeathAnimation;
    }
    public void EnableRunAnimation()
    {
        animator.SetTrigger("StartRunning");
    }

    private void EnableGroundedAnimation(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);
    }
    private void EnableJumpAnimation()
    {
        animator.SetTrigger("Jump");
    }

    private void EnableSlideAnimation(CharacterController characterController)
    {
        if (!isSLiding)
        {
            //animator.SetTrigger("Slide");
            StartSliding(characterController);
            StartCoroutine(StopSliding(characterController));
        }
    }

    private void StartSliding(CharacterController characterController)
    {
        isSLiding = true;
        animator.SetBool("Sliding1", isSLiding);    
        characterController.height /= 3;
        characterController.center = new Vector3(characterController.center.x, characterController.center.y / 3, characterController.center.z);
    }

    private IEnumerator StopSliding(CharacterController characterController)
    {
        yield return new WaitForSeconds(0.6f);
        isSLiding = false;
        animator.SetBool("Sliding1", isSLiding);
        characterController.height *= 3;
        characterController.center = new Vector3(characterController.center.x, characterController.center.y * 3, characterController.center.z);
    }

    private void EnableFallAnimation()
    {
        animator.SetTrigger("FastDown");
    }

    public void EnableDeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    private void OnDestroy()
    {
        playerEngine.Runned   -= EnableRunAnimation;
        playerEngine.Grounded -= EnableGroundedAnimation;
        playerEngine.Jumped   -= EnableJumpAnimation;
        playerEngine.Slided   -= EnableSlideAnimation;
        playerEngine.Falled   -= EnableFallAnimation;
       // playerEngine.Dead     -= EnableDeathAnimation;
    }

}
