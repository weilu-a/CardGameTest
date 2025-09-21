using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌数值- 包装卡牌数据和效果信息
/// </summary>
public class Card
{
    //[SerializeField] private ItemData itemData;

    public Sprite Image => data.Image;
    public Effect Effects => data.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => data.OtherEffects;
    public int Mana { get; private set; }

    public readonly CardData data;

    public Card(CardData data)
    {
        this.data = data;
        this.Mana = data.Mana;
    }
}
