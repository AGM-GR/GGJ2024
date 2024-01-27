using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsManager : MonoBehaviour
{
    public Hitter Hitter;
    public ToothHitter ToothHitter;

    public GrabController GrabController;

    public void Hit()
    {
        Hitter.TryHit();
    }

    public void ToothHit()
    {
        ToothHitter.TryToothHit();
    }


    public void ThrowEvent(){
        GrabController.DoThrow();
    }
}
