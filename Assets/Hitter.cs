using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hitter : MonoBehaviour
{
    public Collider HitTrigger;
    public float ForcePower = 100f;
    public Animator Animator;

    public bool _isHitting;

    public float PushDistance = 2f;
    public float PushVelocity = 10f;

    private void OnFirstAttack(InputValue value)
    {
        if (value.isPressed)
        {
            Animator.SetTrigger("Hit");
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
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isHitting) return;

        _isHitting = true;
        HitTrigger.enabled = false;

        Vector3 direction = transform.forward;
        //other.GetComponent<Rigidbody>().AddForce(direction * ForcePower, ForceMode.Impulse);
        StartCoroutine(Push(other.transform, direction));        
    }

    IEnumerator Push(Transform transform, Vector3 dir)
    {
        float pushDuration = PushDistance / PushVelocity;

        float delta = 0;
        while (delta <= pushDuration)
        {
            transform.position += dir * PushVelocity * Time.deltaTime;
            delta += Time.deltaTime;
            _isHitting = false;
            yield return null;
        }
    }
}

