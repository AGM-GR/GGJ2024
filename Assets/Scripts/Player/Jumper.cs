using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumper : MonoBehaviour
{
    public float jumpForce = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float fallingVelocityThreshold = -2f;

    public Transform checkFloor;

    public bool IsJumping = false;
    private Rigidbody rb;
    private Character _character;


    [Space]
    public float ReleaseTime = 0.5f;
    public float LandingTime = 0.5f;

    public bool IsGrounded {  get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _character = GetComponentInParent<Character>();
    }

    public void OnJump(InputValue value)
    {
        if (!_character.IsInit) return;

        if (value.isPressed && !IsJumping && IsGround())
        {
            ResetAllTriggers();
            StartCoroutine(Jump());
        }
    }

    private void ResetAllTriggers()
    {
        List<string> triggers = new List<string>{ "JumpRelease", "JumpAir", "JumpLanding", "JumpLanded" };
        triggers.ForEach(trigger => _character.Animator.ResetTrigger(trigger));
    }

    void Update()
    {
        if (!_character.IsInit) return;


        IsGrounded = IsGround();
        if (rb.velocity.y < fallingVelocityThreshold && !IsGrounded)
        {
            Fall();
        }

        _character.Animator.SetBool("IsGrounded", IsGrounded);

        if(IsGrounded)
        {
             StartCoroutine(Land());
        }
    }

    bool IsGround(){
        return Physics.Raycast(checkFloor.position, Vector3.down, 0.5f, Physics.AllLayers);
    }
    

    IEnumerator Jump()
    {
        IsJumping = true;
        _character.Animator.SetTrigger("JumpRelease");
        yield return new WaitForSeconds(ReleaseTime);
        
        Vector3 velocityY = Vector3.up * jumpForce;
        rb.velocity = new Vector3(rb.velocity.x,velocityY.y,rb.velocity.z);
        _character.Animator.SetTrigger("JumpAir");
        yield return null;
    }


    IEnumerator Land()
    {
        _character.Animator.SetTrigger("JumpLanded");
        yield return new WaitForSeconds(LandingTime);
        IsJumping = false;
        yield return null;
    }

    private void Fall()
    {
        _character.Animator.SetTrigger("JumpLanding");
        Vector3 velocityY = Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + velocityY.y, rb.velocity.z);
    }
}

