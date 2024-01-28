using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollisionHelper : MonoBehaviour
{
    Destructible _Destructible;

    bool _HitCollisionsEnabled = false;

    Collider _InvulnerableCollider;

    public bool HitCollisionsEnabled 
    { 
        get { return _HitCollisionsEnabled; }
        set { _HitCollisionsEnabled = value; }
    }

    public Collider InvulnerableCollider
    {
        get { return _InvulnerableCollider; }
        set { _InvulnerableCollider = value; }
    }

    private void Start()
    {
        _Destructible = GetComponentInParent<Destructible>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_HitCollisionsEnabled)
        {
            if (_InvulnerableCollider == collision.collider)
            {
                Debug.Log("Te has auto hostiado colega");
                return;
            }

            HitCollisionsEnabled = false;
            

            if (_Destructible && _Destructible.CanDestruct)
            {
                _Destructible.Shatter();
            }


            Character charac = collision.gameObject.GetComponent<Character>();
            if (charac)
            {
                charac.ClearPlayer();

            }


            TeethManager teethMan = collision.gameObject.GetComponent<TeethManager>();

            if (teethMan)
            {
                teethMan.DropTooth();
            }


        }
    }
}
