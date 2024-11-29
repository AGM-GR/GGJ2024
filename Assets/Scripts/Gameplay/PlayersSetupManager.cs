using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayersSetupManager : MonoBehaviour
{
    public static PlayersSetupManager Instance { get; private set; }
    public CinemachineTargetGroup TargetGroup;
    public List<Transform> SpawningPoints;
    public List<CharacterData> CharacterDatas;

    public bool GameStarted { get; set; }


    private void Start()
    {
        // Initialize players
        PlayerInput[] players = FindObjectsOfType<PlayerInput>();
        foreach (PlayerInput player in players)
        {
            InitializeCharacter(player);
            AddToCameraTargetGroup(player.transform);
        }


        // Start game
        //StartGame();
    }

    private void InitializeCharacter(PlayerInput player)
    {
        var character = player.GetComponent<Character>();

        int randomIndex = Random.Range(0, SpawningPoints.Count);
        Transform selectedSpawningPoint = SpawningPoints[randomIndex];
        Debug.Log($"Spawning point: {selectedSpawningPoint.gameObject.name}");
        Vector3 randomSpawningPoint = selectedSpawningPoint.position;
        SpawningPoints.RemoveAt(randomIndex);

        character.Initialize(player.playerIndex, player.currentControlScheme, CharacterDatas[player.playerIndex], randomSpawningPoint);
        character.SetPlayerInput(false);
    }

    private void AddToCameraTargetGroup(Transform transform)
    {
        TargetGroup.AddMember(transform, 1, 1);
    }

    public void StartGame()
    {
        AllowPlayersMovement();
        this.gameObject.SetActive(false);
        GameStarted = true;
    }

    private static void AllowPlayersMovement()
    {
        var characterMovements = FindObjectsOfType<Character>().ToList();
        characterMovements.ForEach(character =>
        {
            character.SetPlayerInput(true);
        });
    }

}
