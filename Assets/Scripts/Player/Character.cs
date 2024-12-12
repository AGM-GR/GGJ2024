using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public CharacterType Name => _characterData.Name;
    public Animator Animator { get; private set; }
    public int CharacterIndex = -1;
    public string ControlScheme;

    public GameObject ModelsContainer;
    public GameObject[] CharacterGOs;

    public float StunnedTime = 1f;

    private GameObject _currentCharacterGO;

    private CharacterData _characterData;

    private CharacterMovement _characterMovement;
    private GrabController _grabController;
    private Hitter _hitter;
    private ToothHitter _toothHitter;
    private PlayerInput _playerInput;
    private TeethManager _teethManager;
    private EffectHandler _effectHandler;

    public CharacterMovement CharacterMovement => _characterMovement;

    public bool IsInit => CharacterIndex != -1;

    private void Awake()
    {
        CharacterIndex = -1;

        _characterMovement = GetComponent<CharacterMovement>();
        _grabController = GetComponentInChildren<GrabController>();
        _hitter = GetComponentInChildren<Hitter>();
        _toothHitter = GetComponentInChildren<ToothHitter>();
        _teethManager = GetComponentInChildren<TeethManager>();

        if (_playerInput != null)
            _playerInput = GetComponent<PlayerInput>();

        DontDestroyOnLoad(this.gameObject);
    }

    public void Initialize(int index,
                           string controlScheme,
                           CharacterData characterData,
                           Vector3 spawningPosition)
    {
        CharacterIndex = index;
        ControlScheme = controlScheme;
        _characterData = characterData;

        SetupModelDependences();
        transform.position = spawningPosition;

        _playerInput = GetComponent<PlayerInput>();

        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        _grabController.Initialize();
        _teethManager.Initialize();
        _characterMovement.Initialize();
    }

    private void SetupModelDependences()
    {
        foreach (GameObject characterModel in CharacterGOs)
        {
            characterModel.SetActive(false);
        }

        _currentCharacterGO = CharacterGOs[CharacterIndex];
        _currentCharacterGO.SetActive(true);
        Animator = _currentCharacterGO.GetComponentInChildren<Animator>();
        _effectHandler = Animator.GetComponent<EffectHandler>();
    }

    public void ClearPlayer()
    {
        _grabController.Drop();
        _hitter.Clear();
        _toothHitter.Clear();
    }

    public void SetPlayerInput(bool enabled)
    {
        if (enabled)
        {
            _playerInput.ActivateInput();
        } 
        else
        {
            _playerInput.DeactivateInput();
        }
    }

    private Coroutine _stunnedCoroutine;

    public void SetStunnedPlayer()
    {
        if (_stunnedCoroutine != null)
        {
            StopCoroutine(_stunnedCoroutine);
        }

        _stunnedCoroutine = StartCoroutine(StunPlayer());
    }

    private IEnumerator StunPlayer()
    {
        ClearPlayer();
        SetPlayerInput(false);
        Animator.SetBool("Stunned", true);

        yield return new WaitForSeconds(StunnedTime);

        Animator.SetBool("Stunned", false);
        SetPlayerInput(true);

        yield return null;
        _effectHandler.Stop("stun");
    }

    private Coroutine _animatorCoroutine;

    public void SetPlayerWaitAnimation()
    {
        if (_animatorCoroutine != null)
        {
            StopCoroutine(_animatorCoroutine);
        }

        _animatorCoroutine = StartCoroutine(PlayerWaitAnimation());
    }

    private IEnumerator PlayerWaitAnimation()
    {
        ClearPlayer();
        SetPlayerInput(false);

        yield return Utils.WaitAnimStateToChange(Animator);

        SetPlayerInput(true);
    }

    private bool NotifyCollisions = false;
    public Action<Collision> onPlayerCollided;

    public void NotifiyCollisions(bool notify)
    {
        NotifyCollisions = notify;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (NotifyCollisions)
        {
            if (onPlayerCollided != null)
            {
                onPlayerCollided.Invoke(collision);
            }
        }
    }
}

