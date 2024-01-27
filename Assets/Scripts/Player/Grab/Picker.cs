using System.Collections;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
public class Picker : MonoBehaviour
{
    const float TRIGGER_ENABLED_TIME = 0.5f;

    [SerializeField] Transform _PickSlot;
    [SerializeField] float _PickTime = 2f;
    [SerializeField] float _DropImpulse = 2f;
    [SerializeField] LayerMask _PickLayerMask;

    [Space]
    public UnityEvent<Collider> OnObjectPicked;
    public UnityEvent<Collider> OnObjectDropped;

    Collider _PickTriggerCollider;
    Coroutine _EnablerCoroutine;
    Coroutine _PickCoroutine;

    private void Start()
    {
        _PickTriggerCollider = GetComponent<Collider>();

        _PickTriggerCollider.isTrigger = true;
        _PickTriggerCollider.enabled = false;
    }

    public void DropPicked(Collider picked)
    {

        if (_PickCoroutine != null)
        {
            Debug.LogWarning("The player is still picking the object.");
            return;
        }

        if (picked)
        {
            picked.enabled = true;
            picked.attachedRigidbody.useGravity = true;

            if (picked.attachedRigidbody.isKinematic)
            {
                picked.attachedRigidbody.isKinematic = false;
                picked.attachedRigidbody.velocity = Vector3.zero;
                picked.attachedRigidbody.angularVelocity = Vector3.zero;
            }

            if (picked.transform.parent == _PickSlot)
            {
                picked.transform.parent = null;
            }


            Vector3 dropDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 1f, UnityEngine.Random.Range(-1f, 1f));
            picked.attachedRigidbody.AddForce(dropDirection.normalized * _DropImpulse, ForceMode.Impulse);

            // Asignar que se destruya o que no haga daño?
            OnObjectDropped.Invoke(picked);
        }
        else
        {
            Debug.Log("Nothing to drop");
        }
    }

    public void TryPick()
    {
        if (_EnablerCoroutine != null)
        {
            Debug.LogWarning("Pick time already enabled");
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

    private void OnTriggerEnter(Collider objectToPick)
    {
        if (_PickCoroutine != null)
        {
            Debug.LogWarning($"Already picking animation: {objectToPick.name}");
            return;
        }

        if (!IsInLayerMasks(objectToPick.gameObject, _PickLayerMask))
        {
            Debug.LogWarning($"Not pickable: {objectToPick.name}");
            return;
        }


        if (objectToPick.CompareTag("Player"))
        {
            Debug.LogWarning($"Pickear personajes esta restringido demomento");
            return;
        }

        // Comprobar que es un objeto
        // If layyer or tag...

        if (objectToPick.attachedRigidbody)
        {
            // Clear forces
            objectToPick.attachedRigidbody.velocity = Vector3.zero;
            objectToPick.attachedRigidbody.angularVelocity = Vector3.zero;
            objectToPick.attachedRigidbody.useGravity = false;
            objectToPick.attachedRigidbody.isKinematic = true; // Sino constrain la posición
            objectToPick.enabled = false;
        }

        _PickCoroutine = StartCoroutine(PickingAnimationCoroutine(objectToPick));
    }

    IEnumerator PickingAnimationCoroutine(Collider picked)
    {
        Transform pickedTransform = picked.transform;
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
        OnObjectPicked.Invoke(picked);

        _PickCoroutine = null;
    }

    private bool IsInLayerMasks(GameObject gameObject, int layerMasks)
    {
        return layerMasks == (layerMasks | (1 << gameObject.layer));
    }
}

