using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// µþ¼ÓÐ§¹û
/// </summary>
public class EffectSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<PerformEffectGA>(PerformEffectPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<PerformEffectGA>();

    }

    private IEnumerator PerformEffectPerformer(PerformEffectGA performEffectGA)
    {
        GameAction effectAction = performEffectGA.effect.GetGameAction(performEffectGA.Targets);
        ActionSystem.Instance.AddReaction(effectAction);
        yield return null;
    }
}
