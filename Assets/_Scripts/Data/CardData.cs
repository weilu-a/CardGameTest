using System.Collections;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Card")]
/// <summary>
/// 卡牌数值- 包装卡牌数据和效果信息
/// </summary>
public class CardData : ScriptableObject
{
    [field: SerializeField] public int Mana { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }

    [field: SerializeReference, SR] public Effect ManualTargetEffect { get; private set; } = null;

    [field: SerializeField] public List<AutoTargetEffect> OtherEffects { get; private set; }

}
