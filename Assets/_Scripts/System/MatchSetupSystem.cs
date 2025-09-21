using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 初始化对局设置
/// </summary>
public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private List<EnemyData> enemyDatas;

    private void Start()
    {
        HeroSystem.Instance.Setup(heroData);
        EnemySystem.Instance.Setup(enemyDatas);
        CardSystem.Instance.Setup(heroData.Deck);
        //EnemySystem.Instance.Setup(npcs, dialData);
        DrawCardGA drawCardGA = new(5);
        ActionSystem.Instance.Perform(drawCardGA);
    }
}
