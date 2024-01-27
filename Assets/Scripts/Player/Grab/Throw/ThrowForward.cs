using UnityEngine;

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


    public void ThrowObject(Rigidbody rigidbody, Vector3 direction)
    {
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
    }


}
