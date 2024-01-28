using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LobbyIntro : MonoBehaviour
{
    private PlayerInputManager _inputManager;

    public int MinPlayerAmount = 2;
    public int ConnectedPlayersAmount;
    public Button PlayGameButton;

    [Space]
    [Tooltip("Assign on the final character indexes order")]
    public List<CharacterData> CharacterDatas;
    public List<Animator> Animators;


    [Header("Lobby Input Actions")]
    [SerializeField] InputAction startGame = null;

    public void Start()
    {
        PlayGameButton.interactable = false;
        PlayGameButton.onClick.AddListener(StartGame);

        _inputManager = FindObjectOfType<PlayerInputManager>();
        _inputManager.onPlayerJoined += (p) => OnPlayerJoined(p);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    private void Update()
    {
        if (startGame.WasPressedThisFrame())
        {
            PlayGameButton.OnSubmit(null);
        }
    }

    private void OnEnable()
    {
        startGame.Enable();
    }

    private void OnDisable()
    {
        startGame.Disable();
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        player.gameObject.transform.position = new Vector3(666, 666, 666);
        Debug.Log("Control scheme: " + player.currentControlScheme);
        PlayCharacterAnimation(player.playerIndex);
        ConnectedPlayersAmount++;
        RefreshPlayButton();
    }

    private void PlayCharacterAnimation(int index)
    {
        //Animators[index].SetTrigger("Hover");
        Debug.Log("Selected " + index);
    }

    private void RefreshPlayButton()
    {
        bool canPlay = ConnectedPlayersAmount >= MinPlayerAmount;
        PlayGameButton.interactable = canPlay;
        if (canPlay)
        {
            //PlayGameButton.GetComponent<Animator>().Play("Normal", 0, GetPreviousAnimatorsNormalizedTime(ConnectedPlayersAmount));
        }
    }

}
