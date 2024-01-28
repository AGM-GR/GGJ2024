using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class LobbyEnabler : MonoBehaviour
{
    public LobbyIntro LobbyGo;
    public PlayerInputManager InputManager;
    public PlayableDirector PlayableDirector;

    private bool init;

    private void Update()
    {
        if (PlayableDirector.time >= 28 && !init)
        {
            init = true;

            InputManager.gameObject.SetActive(true);
            LobbyGo.gameObject.SetActive(true);
        }
    }
}
