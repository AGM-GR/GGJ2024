using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCollisionHelper : MonoBehaviour
{
    Destructible _Destructible;

    private void Start()
    {
        _Destructible = GetComponentInParent<Destructible>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("Collisionado");
        if (_Destructible.CanDestruct)
        {
            _Destructible.Shatter();
        }
    }
}
