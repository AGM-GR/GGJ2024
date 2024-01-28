using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tuto -> https://www.youtube.com/watch?v=EgNV0PWVaS8

public class Destructible : MonoBehaviour
{

    [SerializeField] GameObject _HoleGO; // Child
    [SerializeField] GameObject _ShatteredGO; // Child
    [SerializeField] Transform _RespawnPoint; // Child
    [Space]
    [SerializeField] bool _InheritForces;

    Rigidbody _HoleRigidbody;

    Rigidbody[] _ShatteredRigidbodies;

    bool _IsShattered = false;

    bool _CanDestruct = false;

    public bool CanDestruct 
    { 
        get { return _CanDestruct; } 
        set { _CanDestruct = value; }   
    }



    void Start()
    {
        _HoleRigidbody = _HoleGO.GetComponentInChildren<Rigidbody>();
        _ShatteredRigidbodies = _ShatteredGO.GetComponentsInChildren<Rigidbody>();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {

    //    }
    //}

    public void Shatter()
    {
        if (_IsShattered)
        {
            return;
        }

        _IsShattered = true;

        _ShatteredGO.transform.position = _HoleGO.transform.position;
        _ShatteredGO.transform.rotation = _HoleGO.transform.rotation;

        _HoleGO.SetActive(false);
        _ShatteredGO.SetActive(true);

        if (_InheritForces)
        {
            foreach (Rigidbody rb in _ShatteredRigidbodies)
            {
                rb.velocity = _HoleRigidbody.velocity;
                rb.angularVelocity = _HoleRigidbody.angularVelocity;
            }
        }

        Invoke("Respawn", 3f);
    }

    

    private void Respawn()
    {
        // Reset variables
        _IsShattered = false;
        _CanDestruct = false;

        _HoleGO.transform.localPosition = Vector3.zero;
        _HoleGO.transform.localRotation = Quaternion.identity;
        _HoleRigidbody.velocity = Vector3.zero;
        _HoleRigidbody.angularVelocity = Vector3.zero;

        _ShatteredGO.transform.localPosition = Vector3.zero;
        _ShatteredGO.transform.localRotation = Quaternion.identity;

        foreach (Rigidbody shatterRB in _ShatteredRigidbodies)
        {
            shatterRB.velocity = Vector3.zero;
            shatterRB.angularVelocity = Vector3.zero;
            shatterRB.transform.localPosition = Vector3.zero;
            shatterRB.transform.localRotation = Quaternion.identity;
        }

        

        transform.position = _RespawnPoint.position;
        transform.rotation = _RespawnPoint.rotation;


        _ShatteredGO.SetActive(false);
        _HoleGO.SetActive(true);
    }


}
