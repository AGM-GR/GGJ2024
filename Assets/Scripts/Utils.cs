using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    #region Animations

    public static IEnumerator WaitAnimTofinish(Animator animator, int layer = 0)
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => animator.isInitialized);
        yield return new WaitWhile(() => animator.IsInTransition(layer));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= 1f);
    }

    public static IEnumerator WaitAnimStateToChange(Animator animator, int layer = 0)
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => animator.isInitialized);
        yield return new WaitWhile(() => animator.IsInTransition(layer));
        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(layer);
        yield return new WaitUntil(() => currentAnimation.shortNameHash != animator.GetCurrentAnimatorStateInfo(layer).shortNameHash || animator.IsInTransition(layer));
    }

    public static IEnumerator WaitAnimToTime(Animator animator, float normalizedTime, int layer = 0)
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => animator.isInitialized);
        yield return new WaitWhile(() => animator.IsInTransition(layer));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= normalizedTime);
    }

    #endregion
}
