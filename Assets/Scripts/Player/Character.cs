using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public CharacterType Name => _characterData.Name;
    public Animator Animator { get; private set; }
    public int CharacterIndex;
    public string ControlScheme;

    public GameObject[] CharacterGOs;

    public float StunnedTime = 1f;

    private GameObject _currentCharacterGO;

    private CharacterData _characterData;

    private CharacterMovement _characterMovement;
    private GrabController _grabController;
    private Hitter _hitter;
    private ToothHitter _toothHitter;
    private PlayerInput _playerInput;


    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _grabController = GetComponent<GrabController>();
        _hitter = GetComponentInChildren<Hitter>();
        _toothHitter = GetComponentInChildren<ToothHitter>();

        if (_playerInput != null)
            _playerInput = GetComponent<PlayerInput>();
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

        yield return new WaitForSeconds(StunnedTime);

        SetPlayerInput(true);
    }
}

