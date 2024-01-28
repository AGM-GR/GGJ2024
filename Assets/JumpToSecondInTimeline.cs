using UnityEngine;
using UnityEngine.Playables;

public class JumpToSecondInTimeline : MonoBehaviour
{
    public PlayableDirector timeline; // La Timeline a la que saltar
    public double secondToJumpTo; // El segundo al que saltar en la Timeline
    public KeyCode keyToPress; // La tecla que, al presionarla, hará saltar la Timeline

    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            timeline.time = secondToJumpTo;
        }
    }
}
