using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemyGA : GameAction
{
    public CombatantView Targets { get; set; }
    public AttackEnemyGA(CombatantView targets)
    {
        Targets = targets;
    }

}
