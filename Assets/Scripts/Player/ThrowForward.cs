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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sphere.transform.position = transform.position;

            Rigidbody rb = sphere.AddComponent<Rigidbody>();

            rb.useGravity = true;
            rb.mass = 1f;

            ThrowObject(rb, transform.forward);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sphere.transform.position = transform.position;

            Rigidbody rb = sphere.AddComponent<Rigidbody>();

            rb.useGravity = true;
            rb.mass = 2f;

            ThrowObject(rb, transform.forward);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sphere.transform.position = transform.position;

            Rigidbody rb = sphere.AddComponent<Rigidbody>();

            rb.useGravity = true;
            rb.mass = 5f;

            ThrowObject(rb, transform.forward);
        }
    }

    public void ThrowObject(Rigidbody rigidbody, Vector3 direction)
    {
        // Reset forces
        rigidbody.velocity = Vector3.zero;

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
