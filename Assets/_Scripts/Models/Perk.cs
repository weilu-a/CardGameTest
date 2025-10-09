using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk
{
    public Sprite Image => data.Image;
    private readonly PerkData data;// 构造静态数据
    private readonly PerkCondition condition;
    private readonly AutoTargetEffect effect;
    public Perk(PerkData perkData)
    {
        data = perkData;
        condition = data.perkCondition;
        effect = data.autoTarget;
    }

    public void OnAdd()
    {
        condition.SubscribeCondition(Reaction);
    }
    public void OnRemove()
    {
        condition.UnSubscribeCondition(Reaction);
    }
    private void Reaction(GameAction gameAction)
    {
        if (condition.SubConditionIsMet(gameAction))
        {
            List<CombatantView> target = new();
            //检测这个游戏动作有没有也实现Caster接口
            if(data.UseActionCasterAsTarget && gameAction is IHaveCaster haveCaster)
            {
                target.Add(haveCaster.Caster);
            }
            if (data.UseAutoTarget)
            {
                target.AddRange(effect.TargetMode.GetTargets());
            }
            GameAction perkEffectAction = effect.Effect.GetGameAction(target, HeroSystem.Instance.HeroView);
            ActionSystem.Instance.AddReaction(perkEffectAction);
        }
    }
}
