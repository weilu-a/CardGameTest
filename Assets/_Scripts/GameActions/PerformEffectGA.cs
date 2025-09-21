using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformEffectGA : GameAction
{
    public Effect effect { get; set; }
    public List<CombatantView> Targets { get; set; }

    public PerformEffectGA(Effect effect,List<CombatantView> targets)
    {
        this.effect = effect;
        Targets = targets == null ? null : new(targets);
    }
}
