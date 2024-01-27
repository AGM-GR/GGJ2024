using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tuto -> https://www.youtube.com/watch?v=EgNV0PWVaS8

public class Destructible : MonoBehaviour
{

    //[SerializeField] GameObject _HoleGO;
    [SerializeField] GameObject _ShatteredGO; // Assign prefab here
    [SerializeField] bool _InheritForces;

    Rigidbody _Rigidbody;

    bool _IsShattered = false;

    // Start is called before the first frame update
    void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();  
    }


    public void Shatter() 
    {
        if (_IsShattered) 
        {
            return;
        }

        _IsShattered = true;

        GameObject shatteredVersion = Instantiate(_ShatteredGO, transform.position, transform.rotation);

        if (_InheritForces)
        {
            Rigidbody[] rigidbodies = shatteredVersion.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in rigidbodies)
            {
                rb.velocity = _Rigidbody.velocity;
                rb.angularVelocity = _Rigidbody.angularVelocity;
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Shatter();  
    }


}
