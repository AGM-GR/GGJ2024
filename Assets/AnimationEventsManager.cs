using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsManager : MonoBehaviour
{
    public Hitter Hitter;

    public void Hit()
    {
        Hitter.TryHit();
    }
}
