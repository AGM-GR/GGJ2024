using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class JoinedWidget : MonoBehaviour
{
    public PlayerInputManager _inputManager;

    public CharacterData Data;
    public TextMeshProUGUI text;

    public void Start()
    {
        text.enabled = false;
        _inputManager.onPlayerJoined += (p) => OnPlayerJoined(p);
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        if(player.playerIndex == Data.Index)
        {
            text.text = $"{Data.Name.ToString().ToUpper()}";
            text.color = Data.Color;
            //text.enabled = true;
        }
    }
}
