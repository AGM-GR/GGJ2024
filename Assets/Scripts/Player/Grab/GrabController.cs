using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabController : MonoBehaviour
{
    [SerializeField] Picker _Picker;
    [SerializeField] ThrowForward _Thrower;

    Collider _ObjectGrabbed = null;

    private Character _character;
    PlayerInput _PlayerInput;

    private bool canGrab = true;

    Coroutine _CheckGrabCoroutine;

    public Collider ObjectGrabbed
    {
        get { return _ObjectGrabbed; }
    }

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _PlayerInput = GetComponentInParent<PlayerInput>();
    }

    private void Start()
    {
        _Picker.OnObjectPicked.AddListener((Collider col) => 
        { 
            _ObjectGrabbed = col;
            _character.Animator.SetTrigger("PickUp");
            _character.Animator.SetFloat("HangingObject", 1);
            _character.CharacterMovement.isMovingSlow = true;

            foreach (InputAction action in _PlayerInput.actions)
            {
                action.Disable();
            }
             _PlayerInput.actions.FindAction("Grab").Enable();
             _PlayerInput.actions.FindAction("Move").Enable();
             _PlayerInput.actions.FindAction("Jump").Enable();
        });

        _Picker.OnObjectDropped.AddListener((Collider col) => 
        {
            if (col != _ObjectGrabbed)
            {
                Debug.LogWarning("Algo raro pasaba ... digievolucionaban !!! En tama�o y color, ellos son los DIGIMOOOOON :D");
            }

            _ObjectGrabbed = null;

            foreach (InputAction action in _PlayerInput.actions)
            {
                action.Enable();
            }
        });

        _Thrower.OnObjectThrowed.AddListener((Collider col) =>
        {
            if (col != _ObjectGrabbed)
            {
                Debug.LogWarning("Raro");
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

        if (_CheckGrabCoroutine != null)
        {
            Debug.LogError("Cuidadin");
        }

        _CheckGrabCoroutine = StartCoroutine(CheckCanGrab());
    }

    public void Throw()
    {
        // Esto?
        _character.Animator.SetFloat("HangingObject", 0);
        _character.CharacterMovement.isMovingSlow = false;

        if (_ObjectGrabbed)
        {
            _character.Animator.SetTrigger("ThrowObject");
            canGrab = false;
   
        }
        else
        {
            Debug.Log("Nothing to throw");
        }
    }

    

    public void DoThrow(){
        print("Trhowe " + _Thrower);
        print("_ObjectGrabbed " + _ObjectGrabbed);
        print("transform" + transform);
        _Thrower.ThrowObject(_ObjectGrabbed, transform.forward);
        //_ObjectGrabbed = null;

        foreach (InputAction action in _PlayerInput.actions)
        {
            action.Enable();
        }

        if (_CheckGrabCoroutine != null)
        {
            Debug.LogError("Cuidadin");
        }

        _CheckGrabCoroutine = StartCoroutine(CheckCanGrab());

    }

    private IEnumerator CheckCanGrab()
    {
        canGrab = false;
         _character.CharacterMovement.IsMovementAllowed = false;
        yield return Utils.WaitAnimStateToChange(_character.Animator);
        yield return new WaitForSeconds(0.1f);
        canGrab = true;
        _character.CharacterMovement.IsMovementAllowed = true;

        _CheckGrabCoroutine = null;
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
            //Debug.Log("Nothing to drop");
        }
    }
}
