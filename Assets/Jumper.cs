using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumper : MonoBehaviour
{
    public float jumpForce = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float fallingVelocityThreshold = -2f;

    public bool IsJumping = false;
    public Animator Animator;
    private Rigidbody rb;

    [Space]
    public float ReleaseTime = 0.5f;
    public float LandingTime = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && !IsJumping)
        {
            StartCoroutine(Jump());
        }
    }

    void Update()
    {
        if (rb.velocity.y < fallingVelocityThreshold)
        {
            Fall();
        }

        Animator.SetBool("IsGrounded", !IsJumping);
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
        Animator.SetTrigger("JumpRelease");
        yield return new WaitForSeconds(ReleaseTime);
        rb.velocity = Vector3.up * jumpForce;
        Animator.SetTrigger("JumpAir");
        yield return null;
    }


    IEnumerator Land()
    {
        Animator.SetTrigger("JumpLanded");
        yield return new WaitForSeconds(LandingTime);
        IsJumping = false;
        yield return null;
    }

    private void Fall()
    {
        Animator.SetTrigger("JumpLanding");
        rb.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }
}

