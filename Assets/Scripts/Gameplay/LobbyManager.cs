using Cinemachine;
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
    [Space]
    public List<Transform> SpawningPoints;
    [Space]
    [Tooltip("Assign on the final character indexes order")]
    public List<CharacterData> CharacterDatas;

    public CinemachineTargetGroup TargetGroup;

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
        AddToCameraTargetGroup(player.transform);
        ConnectedPlayersAmount++;
        RefreshPlayButton();
    }

    private void AddToCameraTargetGroup(Transform transform)
    {
        TargetGroup.AddMember(transform, 1, 1);
    }

    private void PlayBannerAnimation(PlayerInput player)
    {
        characterBanners[player.playerIndex].SetActive(true);

        //characterBanners[player.playerIndex].SetTrigger("PlayerEntry");
        //characterBanners[player.playerIndex].Play("Loop", 1, GetPreviousAnimatorsNormalizedTime(player.playerIndex));
    }

    private void InitializeCharacter(PlayerInput player)
    {
        var character = player.GetComponent<Character>();

        int randomIndex = Random.Range(0, SpawningPoints.Count);
        Vector3 randomSpawningPoint = SpawningPoints[randomIndex].position;
        SpawningPoints.RemoveAt(randomIndex);

        //character.Initialize(player.playerIndex, player.currentControlScheme, CharacterDatas[player.playerIndex], randomSpawningPoint);
        character.SetPlayerInput(false);
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
        var characterMovements = FindObjectsOfType<Character>().ToList();
        characterMovements.ForEach(o =>
        {
            o.SetPlayerInput(true);
        });
    }
}
