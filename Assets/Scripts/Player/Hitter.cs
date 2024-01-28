using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Hitter : MonoBehaviour
{
    public Collider HitTrigger;
    public Animator Animator;

    public bool _isHitting;

    [Header("Push Settings")]
    public AnimationCurve PushCurve = AnimationCurve.Linear(0,0,1,1);
    public float PushDistance = 2f;
    public float PushVelocity = 10f;

    public float ForcePower = 100f;
    private Character _character;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
    }

    void OnFirstAttack(InputValue value)
    {
        if (!_isHitting && value.isPressed)
        {
          
            _character.Animator.SetTrigger("Hit");
        }
    }

    public void TryHit()
    {
        StartCoroutine(HitEnabler());
    }

    IEnumerator HitEnabler()
    {
        _isHitting = true;
        HitTrigger.enabled = true;
        yield return new WaitForFixedUpdate();
        HitTrigger.enabled = false;
        _isHitting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isHitting) return;
        if (other.isTrigger) return;
        if (other.attachedRigidbody == null) return;

        Vector3 direction = transform.forward;
        StartCoroutine(Push(other.attachedRigidbody, direction));
    }

    IEnumerator Push(Rigidbody pushedRigidbody, Vector3 dir)
    {
        bool stopPushing = false;

        Character pushedPlayer = pushedRigidbody.GetComponent<Character>();
        if (pushedPlayer != null)
        {
            pushedPlayer.CharacterMovement.LookAt(_character.transform.position);
            pushedPlayer.Animator.SetTrigger("HitReceived");
            pushedPlayer.SetPlayerWaitAnimation();

            pushedPlayer.NotifiyCollisions(true);
            pushedPlayer.onPlayerCollided += (col) => PlayerCollide(pushedPlayer, col, ref stopPushing);
        }



        float pushDuration = PushDistance / PushVelocity;
        Vector3 initialPosition = pushedRigidbody.position;
        Vector3 destPosition = pushedRigidbody.position + dir * PushDistance;

        Vector3 nextPosition;
        Vector3 lastPosition;

        float delta = 0;
        while (delta <= 1f)
        {
            lastPosition = pushedRigidbody.position;

            delta += Time.fixedDeltaTime * (1f / pushDuration);

            nextPosition = Vector3.Lerp(initialPosition, destPosition, PushCurve.Evaluate(delta));
            pushedRigidbody.MovePosition(nextPosition);

            yield return new WaitForFixedUpdate();

            if (Vector3.Distance(pushedRigidbody.position, lastPosition) + 0.01f < Vector3.Distance(pushedRigidbody.position, nextPosition) || stopPushing)
            {
                break;
            }

        }
        
        if (pushedPlayer != null)
        {
            pushedPlayer.NotifiyCollisions(false);
            pushedPlayer.onPlayerCollided -= (col) => PlayerCollide(pushedPlayer, col, ref stopPushing);

            if (!stopPushing)
                pushedPlayer.SetPlayerInput(true);
        }
    }

    private void PlayerCollide(Character character, Collision collision, ref bool stopPush)
    {
        if (collision.rigidbody == null && collision.gameObject.layer != LayerMask.NameToLayer("Object"))
        {
            stopPush = true;
            character.SetStunnedPlayer();
            character.GetComponent<TeethManager>().DropTooth();
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("Object"))
        {
            stopPush = true;
            character.SetStunnedPlayer();
        }
    }

    public void Clear()
    {
        _isHitting = false;
        HitTrigger.enabled = false;
    }
}

