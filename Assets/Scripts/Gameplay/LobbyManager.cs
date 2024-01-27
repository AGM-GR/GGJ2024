using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }

    private PlayerInputManager _inputManager;

    public int MinPlayerAmount = 2;
    public int ConnectedPlayersAmount;
    public Button PlayGameButton;

    public List<GameObject> characterBanners;

    //public MusicController musicController;
    //public List<Animator> characterBanners;
    //public Animator joinTextAnimator;

    [Header("Lobby Input Actions")]
    [SerializeField] InputAction startGame = null;

    public bool GameStarted { get; set; }

    private void Awake()
    {
        Instance = this;

        PlayGameButton.interactable = false;
        PlayGameButton.onClick.AddListener(StartGame);

        _inputManager = FindObjectOfType<PlayerInputManager>();
        _inputManager.onPlayerJoined += (p) => OnPlayerJoined(p);
    }

    private void Start()
    {
        foreach (GameObject go in characterBanners)
        {
            go.SetActive(false);
        }
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
        Debug.Log("Control scheme: " + player.currentControlScheme);
        PlayBannerAnimation(player);
        InitializeCharacter(player);
        ConnectedPlayersAmount++;
        RefreshPlayButton();
    }

    private void PlayBannerAnimation(PlayerInput player)
    {
        characterBanners[player.playerIndex].SetActive(true);

        //characterBanners[player.playerIndex].SetTrigger("PlayerEntry");
        //characterBanners[player.playerIndex].Play("Loop", 1, GetPreviousAnimatorsNormalizedTime(player.playerIndex));
    }

    private static void InitializeCharacter(PlayerInput player)
    {
        //var character = player.GetComponent<Character>();
        //character.Initialize(player.playerIndex, player.currentControlScheme);
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


    public void StartGame()
    {
        AllowPlayersMovement();
        _inputManager.enabled = false;
        this.gameObject.SetActive(false);

        //musicController.StartMusicAndGames();
        GameStarted = true;
    }

    private static void AllowPlayersMovement()
    {
        /*var characterMovements = FindObjectsOfType<CharacterMovement>().ToList();
        characterMovements.ForEach(o =>
        {
            o.GetComponent<CharacterInfluenceAction>().CanInfluence = true;
            o.IsMovementAllowed = true;
        });*/
    }

    private float GetPreviousAnimatorsNormalizedTime(int index)
    {
        return 1f;
        /*if (index == 0) {
            return joinTextAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        } else {
            return characterBanners[index - 1].GetCurrentAnimatorStateInfo(1).normalizedTime;
        }*/
    }
}
