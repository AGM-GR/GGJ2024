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
            _isHitting = true;
            _character.Animator.SetTrigger("Hit");
        }
    }

    public void TryHit()
    {
        StartCoroutine(HitEnabler());
    }

    IEnumerator HitEnabler()
    {
        HitTrigger.enabled = true;
        yield return new WaitForSeconds(0.5f);
        HitTrigger.enabled = false;
        _isHitting = false;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isHitting) return;
        if (other.attachedRigidbody == null) return;

        HitTrigger.enabled = false;
        _isHitting = false;

        Vector3 direction = transform.forward;
        StartCoroutine(Push(other.attachedRigidbody, direction));
    }

    IEnumerator Push(Rigidbody pushedRigidbody, Vector3 dir)
    {
        Character pushedPlayer = pushedRigidbody.GetComponent<Character>();
        if (pushedPlayer != null)
        {
            pushedPlayer.ClearPlayer();
            pushedPlayer.SetPlayerInput(false);
        }


        pushedRigidbody.GetComponent<TeethManager>().DropTooth();

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

            if (Vector3.Distance(pushedRigidbody.position, lastPosition) + 0.01f < Vector3.Distance(pushedRigidbody.position, nextPosition))
            {
                Debug.Log(name + " Hitted something!!");
                
                if (pushedPlayer != null)
                {
                    //player pega contra algo
                    pushedPlayer.SetStunnedPlayer();
                    pushedPlayer.GetComponent<TeethManager>().DropTooth();

                }

                break;
            }

        }
        
        if (pushedPlayer != null)
        {
            pushedPlayer.SetPlayerInput(true);
        }
    }

    public void Clear()
    {
        _isHitting = false;
        HitTrigger.enabled = false;
    }
}

