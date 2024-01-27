using UnityEngine;
using UnityEngine.InputSystem;

/* Be careful and get sure the thowing object it's not colliding with the player collider */

[RequireComponent(typeof(ThrowFoV))]
public class ThrowForward : MonoBehaviour
{
    [SerializeField] float _Impulse = 12.0f;

    ThrowFoV _FieldOfView;


    private void Start()
    {
        _FieldOfView = GetComponent<ThrowFoV>();
    }


    public void ThrowObject(Collider collider, Vector3 direction)
    {
        collider.enabled = true;

        collider.transform.parent = null;

        Rigidbody rigidbody = collider.attachedRigidbody;

        

        // Reset forces
        rigidbody.isKinematic = false;
        rigidbody.velocity = Vector3.zero;

        rigidbody.useGravity = true;

        

        Transform targetInView = _FieldOfView.GetTargetInFoV();

        if (targetInView != null) 
        {
            print("Al target");
            direction =  targetInView.position - transform.position;
        }

        // Add impulse
        rigidbody.AddForce(direction.normalized * _Impulse, ForceMode.Impulse); // To exclude the mass -> ForceMode.VelocityChange



        if (rigidbody.CompareTag("Player"))
        {
            PlayerInput pi = rigidbody.GetComponent<PlayerInput>();
            CharacterMovement cm = rigidbody.GetComponent<CharacterMovement>();

            if (pi == null || cm == null)
            {
                Debug.LogWarning("Something went wrong");
            }
            else
            {
                pi.enabled = false;
                cm.IsMovementAllowed = true;
            }
        }
    }


}
