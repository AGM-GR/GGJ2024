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

    public bool IsJumping = false;
    private Rigidbody rb;
    private Character _character;


    [Space]
    public float ReleaseTime = 0.5f;
    public float LandingTime = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _character = GetComponentInParent<Character>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && !IsJumping)
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
        if (rb.velocity.y < fallingVelocityThreshold)
        {
            Fall();
        }

        _character.Animator.SetBool("IsGrounded", !IsJumping);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(Land());
        }
    }

    IEnumerator Jump()
    {
        IsJumping = true;
        _character.Animator.SetTrigger("JumpRelease");
        yield return new WaitForSeconds(ReleaseTime);
        rb.velocity = Vector3.up * jumpForce;
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
        rb.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }
}

