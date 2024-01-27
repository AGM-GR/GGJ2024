using System;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterType Name => _characterData.Name;
    public Animator Animator { get; private set; }
    public int CharacterIndex;
    public string ControlScheme;

    public GameObject[] CharacterGOs;

    private GameObject _currentCharacterGO;

    public bool IsInit;

    private CharacterData _characterData;


    public void Initialize(int index,
                           string controlScheme,
                           CharacterData characterData,
                           Vector3 spawningPosition)
    {
        CharacterIndex = index;
        ControlScheme = controlScheme;
        _characterData = characterData;

        SetupModelDependences();
        IsInit = true;
        transform.position = spawningPosition;

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
}

