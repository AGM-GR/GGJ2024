using System.Collections;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
    public UnityEvent<Collider> OnObjectPlacedOnHead;
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
            StopCoroutine(_PickCoroutine);
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

            Destroy(picked.GetComponent<ParentConstraint>());


            Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
            picked.attachedRigidbody.AddForce(dropDirection.normalized * _DropImpulse, ForceMode.Impulse);

            if (picked.CompareTag("Player"))
            {
                PlayerInput pi = picked.GetComponent<PlayerInput>();
                CharacterMovement cm = picked.GetComponent<CharacterMovement>();
                //Character ch = picked.GetComponent<Character>();

                if (pi == null || cm == null) //|| ch == null)
                {
                    Debug.LogWarning("Something went wrong");
                }
                else
                {
                    print("Clear");
                    pi.enabled = true;
                    cm.IsMovementAllowed = true;
                    //ch.ClearPlayer();
                }
            }

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

        // Comprobar que es un objeto
        // If layyer or tag...

        if (objectToPick.CompareTag("Player"))
        {
            PlayerInput pi = objectToPick.GetComponent<PlayerInput>();
            CharacterMovement cm = objectToPick.GetComponent<CharacterMovement>();
            Character ch = objectToPick.GetComponent<Character>();

            if (pi == null || ch == null || cm == null)
            {
                Debug.LogWarning("Something went wrong");
            }
            else
            {
                pi.enabled = false;
                cm.IsMovementAllowed = false;
                ch.ClearPlayer();
            }

            //Debug.LogWarning($"Pickear personajes esta restringido demomento");
            //return;
        }


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
        OnObjectPicked.Invoke(picked);
        
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

        //pickedTransform.parent = _PickSlot;

        //ParentConstraint constraint = pickedTransform.gameObject.AddComponent<ParentConstraint>();
        PositionConstraint posConstraint = pickedTransform.gameObject.AddComponent<PositionConstraint>();

        ConstraintSource constraintSource = new ConstraintSource();
        constraintSource.weight = 1f;
        constraintSource.sourceTransform = _PickSlot;

        //constraint.AddSource(constraintSource);
        posConstraint.AddSource(constraintSource);

        //constraint.constraintActive = true;
        posConstraint.constraintActive = true;

        OnObjectPlacedOnHead.Invoke(picked);

        _PickCoroutine = null;
    }

    private bool IsInLayerMasks(GameObject gameObject, int layerMasks)
    {
        return layerMasks == (layerMasks | (1 << gameObject.layer));
    }
}

