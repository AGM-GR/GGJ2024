using UnityEngine;
using UnityEngine.InputSystem;

public class GrabController : MonoBehaviour
{
    [SerializeField] Picker _Picker;
    [SerializeField] ThrowForward _Thrower;

    Collider _ObjectGrabbed = null;

    private Character _character;

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
        _Picker.OnObjectPicked.AddListener((Collider col)=> 
        { 
            _ObjectGrabbed = col;
            _character.Animator.SetTrigger("PickUp");
            _character.Animator.SetFloat("HangingObject", 1);
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
    }

    void OnGrab(InputValue value)
    {
        if (value.isPressed)
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
    }

    public void Throw()
    {
        _character.Animator.SetFloat("HangingObject", 0);

        if (_ObjectGrabbed)
        {
            _character.Animator.SetTrigger("ThrowObject");
            _Thrower.ThrowObject(_ObjectGrabbed, transform.forward);
            _ObjectGrabbed = null;
        }
        else
        {
            Debug.Log("Nothing to throw");
        }
    }

    public void Drop()
    {
        _character.Animator.SetFloat("HangingObject", 0);

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
