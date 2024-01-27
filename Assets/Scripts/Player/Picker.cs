using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TODO: Desactivar fisicas */
// Drop objetc

[RequireComponent(typeof(Collider))]
public class Picker : MonoBehaviour
{
    const float TRIGGER_ENABLED_TIME = 0.5f;

    [SerializeField] Transform _PickSlot;
    [SerializeField] float _PickTime = 2f;
    [SerializeField] float _DropImpulse = 2f;

    Collider _PickTriggerCollider;
    Coroutine _EnablerCoroutine;
    Coroutine _PickCoroutine;
    Collider _ObjectPicked = null;

    public bool ObjectPicked 
    { 
        get { return _ObjectPicked; }
    }

    private void Start()
    {
        _PickTriggerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //Como si lo llamara desde la animación 
        {
            TryPick();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            DropPicked();
        }
    }

    public void DropPicked()
    {
        if (_PickCoroutine != null)
        {
            Debug.LogWarning("The player is still picking the object.");
            return;
        }

        if (_ObjectPicked)
        {
            _ObjectPicked.enabled = true;
            _ObjectPicked.attachedRigidbody.velocity = Vector3.zero;
            _ObjectPicked.attachedRigidbody.angularVelocity = Vector3.zero;
            _ObjectPicked.attachedRigidbody.useGravity = true;

            if (_ObjectPicked.transform.parent == _PickSlot)
            {
                _ObjectPicked.transform.parent = null;
            }
             

            Vector3 dropDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 1f, UnityEngine.Random.Range(-1f, 1f));
            _ObjectPicked.attachedRigidbody.AddForce(dropDirection.normalized * _DropImpulse, ForceMode.Impulse);

            // Asignar que se destruya o que no haga daño?

            _ObjectPicked = null;
        }
        else
        {
            Debug.Log("Nothing to drop");
        }
    }

    public void TryPick()
    {
        if (_ObjectPicked)
        {
            Debug.LogWarning("The player has already an object picked.");
            return;
        }

        if (_EnablerCoroutine != null)
        {
            Debug.LogWarning("Already picking");
            return;
        }

        _EnablerCoroutine = StartCoroutine(PickEnablerCoroutine());
    }

    IEnumerator PickEnablerCoroutine() // Periodo de tiempo que esta la collider activada para detectar si esta cogiendo un objeto
    {
        _PickTriggerCollider.enabled = true;
        yield return new WaitForSeconds(TRIGGER_ENABLED_TIME);
        _PickTriggerCollider.enabled = false;
        yield return null;

        _EnablerCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_PickCoroutine != null)
        {
            Debug.LogWarning("Already picking animation");
            return;
        }

        print("Picking");

        // Comprobar que es un objeto
        // If layyer or tag...

        if (other.attachedRigidbody)
        {
            // Clear forces
            other.attachedRigidbody.velocity = Vector3.zero;
            other.attachedRigidbody.angularVelocity = Vector3.zero;
            other.attachedRigidbody.useGravity = false;
            other.enabled = false;
        }


        Vector3 direction = transform.forward;

        _ObjectPicked = other;

        _PickCoroutine = StartCoroutine(PickingAnimationCoroutine(other.transform));
    }

    IEnumerator PickingAnimationCoroutine(Transform pickedTransform)
    {
        Vector3 originalPos = pickedTransform.position;

        float progress = 0f;
        float step;
        Vector3 newPos;

        do
        {
            // Add the new tiny extra amount
            progress += Time.deltaTime / _PickTime;
            progress = Mathf.Clamp01(progress);

            // Apply changes
            
            step = 1 - Mathf.Pow(1 - progress, 5); // easeOutQuint
            newPos = Vector3.Lerp(originalPos, _PickSlot.position, step);

            pickedTransform.position = newPos;


            // Wait for the next frame
            yield return null;
        }
        while (progress < 1f); // Keep moving while we don't reach any goal

        pickedTransform.parent = _PickSlot;

        _PickCoroutine = null;
    }


    //private static IEnumerator RelocateCoroutine<T>(T unityObject, Vector3 targetPosition, float moveTime, Interpolation interpolation) where T : UnityEngine.Object
    //{
    //    Vector3 originalPos = GetPosition(unityObject);

    //    float progress = 0f;
    //    float step;
    //    Vector3 newPos;

    //    do
    //    {
    //        // Add the new tiny extra amount
    //        progress += Time.deltaTime / moveTime;
    //        progress = Mathf.Clamp01(progress);

    //        // Apply changes
    //        step = InterpolationCurve.GetInterpolatedStep(progress, interpolation);
    //        newPos = Vector3.Lerp(originalPos, targetPosition, step);

    //        SetPosition(unityObject, newPos);


    //        // Wait for the next frame
    //        yield return null;
    //    }
    //    while (progress < 1f); // Keep moving while we don't reach any goal
    //}
}

