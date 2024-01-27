using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitter : MonoBehaviour
{
    public Collider HitTrigger;
    public float ForcePower = 100f;
    public Animator Animator;

    public bool _isHitting;

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
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
        Debug.Log("Push");
        float delta = 0;
        while (delta <= 0.5f)
        {
            transform.position += dir * ForcePower * Time.deltaTime;
            delta += Time.deltaTime;
            _isHitting = false;
            yield return null;
        }
    }
}

