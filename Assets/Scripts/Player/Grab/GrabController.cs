using UnityEngine;

public class GrabController : MonoBehaviour
{
    [SerializeField] Picker _Picker;
    [SerializeField] ThrowForward _Thrower;

    Collider _ObjectGrabbed = null;

    public Collider ObjectGrabbed
    {
        get { return _ObjectGrabbed; }
    }

    private void Start()
    {
        _Picker.OnObjectPicked.AddListener((Collider col)=> { _ObjectGrabbed = col; }) ;
        _Picker.OnObjectDropped.AddListener((Collider col) => 
        {
            if (col != _ObjectGrabbed) 
            {
                Debug.LogWarning("Algo raro pasa");
            }

            _ObjectGrabbed = null; 
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //Como si lo llamara desde la animación 
        {
            Pick();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Drop();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Throw();
        }
    }

    public void Pick()
    {
        if (_ObjectGrabbed)
        {
            Debug.LogWarning("The player has already an object grabbed.");
            return;
        }

        _Picker.TryPick();
    }

    public void Throw()
    {
        if (_ObjectGrabbed)
        {
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
