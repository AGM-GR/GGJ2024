using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabController : MonoBehaviour
{
    [SerializeField] Picker _Picker;
    [SerializeField] ThrowForward _Thrower;

    Collider _ObjectGrabbed = null;

    private Character _character;

    private bool canGrab = true;

    public Collider ObjectGrabbed
    {
        get { return _ObjectGrabbed; }
    }

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
    }

    private void Start()
    {
        _Picker.OnObjectPicked.AddListener((Collider col) => 
        { 
            _ObjectGrabbed = col;
            _character.Animator.SetTrigger("PickUp");
            _character.Animator.SetFloat("HangingObject", 1);
            _character.CharacterMovement.isMovingSlow = true;

        });

        _Picker.OnObjectDropped.AddListener((Collider col) => 
        {
            if (col != _ObjectGrabbed)
            {
                Debug.LogWarning("Algo raro pasaba ... digievolucionaban !!!");
            }

            _ObjectGrabbed = null; 
        });

        _character.Animator.SetFloat("HangingObject", 0);
        _character.CharacterMovement.isMovingSlow = false;
    }

    void OnGrab(InputValue value)
    {
        if (canGrab && value.isPressed)
        {
            if (_ObjectGrabbed == null)
            {
                Pick();
            } 
            else 
            {
                Throw();
            }
        }
    }

    public void Pick()
    {
        if (_ObjectGrabbed)
        {
            Debug.LogWarning("The player has already an object grabbed.");
            return;
        }

        _character.Animator.SetTrigger("TryPickUp");
        _Picker.TryPick();

        StartCoroutine(CheckCanGrab());
    }

    public void Throw()
    {
        _character.Animator.SetFloat("HangingObject", 0);
        _character.CharacterMovement.isMovingSlow = false;

        if (_ObjectGrabbed)
        {
            _character.Animator.SetTrigger("ThrowObject");
            _Thrower.ThrowObject(_ObjectGrabbed, transform.forward);
            _ObjectGrabbed = null;

            StartCoroutine(CheckCanGrab());
        }
        else
        {
            Debug.Log("Nothing to throw");
        }
    }

    private IEnumerator CheckCanGrab()
    {
        canGrab = false;
        yield return Utils.WaitAnimStateToChange(_character.Animator);
        canGrab = true;
    }

    public void Drop()
    {
        _character.Animator.SetFloat("HangingObject", 0);
        _character.CharacterMovement.isMovingSlow = false;

        if (_ObjectGrabbed)
        {
            _Picker.DropPicked(_ObjectGrabbed);
        }
        else
        {
            Debug.Log("Nothing to drop");
        }
    }
}
