using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnemyAttackCondition : PerkCondition
{

    public override bool SubConditionIsMet(GameAction gameAction)
    {
        return true;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReactin<AttackHeroGA>(reaction, reactionTiming);
    }



    public override void UnSubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnSubscribeReactin<AttackHeroGA>(reaction, reactionTiming);

    }

}

