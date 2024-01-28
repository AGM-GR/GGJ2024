using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/* Be careful and get sure the thowing object it's not colliding with the player collider */

[RequireComponent(typeof(ThrowFoV))]
public class ThrowForward : MonoBehaviour
{
    [SerializeField] float _Impulse = 12.0f;

    [Space]
    public UnityEvent<Collider> OnObjectThrowed;


    ThrowFoV _FieldOfView;


    private void Start()
    {
        _FieldOfView = GetComponent<ThrowFoV>();
    }


    public void ThrowObject(Collider collider, Vector3 direction)
    {
        if (collider == null) 
        {
            Debug.LogWarning("Throw vacio");
            return;
        }

        collider.enabled = true;
        

        ParentConstraint constraint = collider.GetComponent<ParentConstraint>();
        //PositionConstraint constraint = collider.GetComponent<PositionConstraint>();
        constraint.RemoveSource(0);
        constraint.constraintActive = false;
        Destroy(constraint); // Hacer un wait de un par de frames??

        Rigidbody rigidbody = collider.attachedRigidbody;

        ObjectCollisionHelper och = collider.GetComponent<ObjectCollisionHelper>();
        if (och) 
        { 
            och.InvulnerableCollider = GetComponentInParent<Collider>(); // Player collider
            och.HitCollisionsEnabled = true;
        }
        

        // Reset forces
        rigidbody.isKinematic = false;
        rigidbody.velocity = Vector3.zero;

        rigidbody.useGravity = true;


        Transform targetInView = _FieldOfView.GetTargetInFoV();

        if (targetInView != null && targetInView != collider.transform) 
        {
            print("Al target");
            Vector3 newDirection = targetInView.position - transform.position;
            direction =  new Vector3(newDirection.x, direction.y, newDirection.z);
        }

        // Add impulse
        rigidbody.AddForce(direction.normalized * _Impulse * rigidbody.mass, ForceMode.Impulse); // To exclude the mass -> ForceMode.VelocityChange

        OnObjectThrowed.Invoke(collider);
        
        if (rigidbody.CompareTag("Player"))
        {
            StartCoroutine(ThrowCharacter(rigidbody, rigidbody.GetComponent<Character>()));
        }
    }

    private IEnumerator ThrowCharacter(Rigidbody characterRigidbody, Character character)
    {
        character.CharacterMovement.Jumper.enabled = true;
        character.CharacterMovement.IsMovementAllowed = false;
        character.SetPlayerInput(false);

        character.Animator.SetBool("Grabbed", false);
        character.Animator.SetBool("Launched", true);

        yield return new WaitForFixedUpdate();

        bool willStun = false;
        character.NotifiyCollisions(true);
        character.onPlayerCollided += (col) => PlayerCollide(characterRigidbody, character, col, ref willStun);

        yield return null;
        yield return new WaitUntil(() => character.CharacterMovement.Jumper.IsGrounded);

        character.NotifiyCollisions(false);
        character.onPlayerCollided -= (col) => PlayerCollide(characterRigidbody, character, col, ref willStun);
        characterRigidbody.velocity = Vector3.zero;

        character.Animator.SetBool("WallLanding", false);
        character.Animator.SetBool("Launched", false);

        if (willStun)
        {
            character.SetStunnedPlayer();
        }
        else
        {
            character.SetPlayerWaitAnimation();
            character.CharacterMovement.IsMovementAllowed = true;
        }
    }

    private void PlayerCollide(Rigidbody characterRigidbody, Character character, Collision collision, ref bool willStun)
    {
        if (collision.rigidbody == null && collision.gameObject.layer != LayerMask.NameToLayer("Object"))
        {
            if (!character.CharacterMovement.Jumper.IsGrounded) {
                character.Animator.SetBool("WallLanding", true);
                willStun = true;
            }
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("Object"))
        {
            willStun = true;
        }
    }
}
