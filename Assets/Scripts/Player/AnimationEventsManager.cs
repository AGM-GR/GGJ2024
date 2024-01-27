using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsManager : MonoBehaviour
{
    public Hitter Hitter;
    public ToothHitter ToothHitter;

    public void Hit()
    {
        Hitter.TryHit();
    }

    public void ToothHit()
    {
        ToothHitter.TryToothHit();
    }
}
