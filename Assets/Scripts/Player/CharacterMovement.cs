using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    // Exposed variables
    public float MoveSpeed = 3f;
    public float MoveSpeedSlow = 2f;
    public bool isMovingSlow = false;
    public float MinInputAmount = 0.5f;
    public float SpeedDampTime = 0.2f;
    public float RotateSpeed = 6f;
    public float FloorOffsetY = 0.75f;
    [Header("Speed Up")]
    public float speedUpMultiplier = 1.5f;
    public float speedUpDuration = 12f;
    [Space]
    [Header("Debug")]
    public bool startWithMovement;

    // Private fields
    private float _horizontalAxis;
    private float _verticalAxis;
    private float _inputAmount;

    private Vector3 _moveDirection = Vector3.zero;

    private float _currentSpeedMultiplier = 1f;

    // Component dependences
    private Camera _mainCamera;
    private Rigidbody _rb;
    private Character _character;
    public Jumper Jumper;

    public bool IsMovementAllowed
    {
        get { return _isMovementAllowed; }
        set
        {
            if (value == false)
            {
                _inputAmount = 0;
                _rb.velocity = Vector3.zero;
            }
            _isMovementAllowed = value;
        }
    }

    public bool _isMovementAllowed;

    private void Awake()
    {
        _character = GetComponent<Character>();

        if (startWithMovement)
        {
            IsMovementAllowed = true;
        }
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        Rotate();

        if (!IsMovementAllowed || (LobbyManager.Instance != null && !LobbyManager.Instance.GameStarted)) return;

        Move();
        _character.Animator.SetFloat("Speed", _inputAmount);
        Vector3 velocity = _moveDirection * GetSpeed() * _inputAmount * _currentSpeedMultiplier;
        _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);


    }
    private float GetSpeed()
    {
        if (isMovingSlow)
        {
            return MoveSpeedSlow;
        }
        else
        {
            return MoveSpeed;
        }
    }


    public void OnMove(InputValue value)
    {
        _verticalAxis = value.Get<Vector2>().y;
        _horizontalAxis = value.Get<Vector2>().x;
    }


    private void Move()
    {
        _moveDirection = Vector3.zero; // reset movement

        Vector3 correctedVertical = _verticalAxis * Vector3.forward;
        Vector3 correctedHorizontal = _horizontalAxis * Vector3.right;

        // Transformar las direcciones de entrada en el espacio de la cámara
        correctedVertical = _mainCamera.transform.TransformDirection(correctedVertical);
        correctedHorizontal = _mainCamera.transform.TransformDirection(correctedHorizontal);

        Vector3 combinedInput = correctedVertical + correctedHorizontal;
        _moveDirection = new Vector3(combinedInput.normalized.x, 0, combinedInput.normalized.z);

        float inputMagnitude = Mathf.Abs(_horizontalAxis) + Mathf.Abs(_verticalAxis);
        _inputAmount = Mathf.Clamp01(inputMagnitude);
        if (_inputAmount <= MinInputAmount) _inputAmount = 0;
    }



    private void Rotate()
    {
        if (_moveDirection == Vector3.zero) return;
        Quaternion rot = Quaternion.LookRotation(_moveDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * _inputAmount * RotateSpeed);
        transform.rotation = targetRotation;
    }
}
