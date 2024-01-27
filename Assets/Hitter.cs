using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitter : MonoBehaviour
{
    public Collider HitTrigger;
    public float ForcePower = 100f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            TryHit();
        }
    }

    private void TryHit()
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
        Vector3 direction = transform.forward;
        //other.GetComponent<Rigidbody>().AddForce(direction * ForcePower, ForceMode.Impulse);
        StartCoroutine(Push(other.transform, direction));
    }

    IEnumerator Push(Transform transform, Vector3 dir)
    {
        float delta = 0;
        while (delta <= 0.5f)
        {
            transform.position += dir * ForcePower * Time.deltaTime;
            delta += Time.deltaTime;
            yield return null;
        }
    }
}
