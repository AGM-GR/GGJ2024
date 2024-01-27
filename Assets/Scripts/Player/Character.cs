using System;
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
        _characterMovement.IsMovementAllowed = true;
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
        _playerInput.enabled = enabled;
    }
}

