using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public int CharacterIndex;

    public GameObject[] CharacterGOs;

    private GameObject _currentCharacterGO;

    public void Start()
    {
        _currentCharacterGO = CharacterGOs[CharacterIndex];
        _currentCharacterGO.SetActive(true);
        Animator = _currentCharacterGO.GetComponentInChildren<Animator>();
    }


}

