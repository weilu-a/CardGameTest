using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 卡牌系统 - 管理抽牌、弃牌、手牌与战斗行动的执行
/// </summary>
public class CardSystem : Singleton<CardSystem>
{
    [Header("视图与位置点")]

    [SerializeField] private HandView handView;              // 手牌显示
    [SerializeField] private Transform drawPilePoint;        // 抽牌堆位置
    [SerializeField] private Transform discardwPilePoint;    // 弃牌堆位置

    [Header("牌堆数据")]
    private readonly List<Card> drawPile = new();        // 抽牌堆
    private readonly List<Card> discardPile = new();     // 弃牌堆
    private readonly List<Card> hand = new();            // 手牌

    [SerializeField] private GameObject damageVFX;

    #region 生命周期
    private void OnEnable()
    {
        // 注册执行器（Performer）
        ActionSystem.AttachPerformer<DrawCardGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllGA>(DiscardAllCaorsPerformer);
        ActionSystem.AttachPerformer<PlayerCardGA>(PlayerCardPerforemer);

        // 注册反应（Reaction）
        ActionSystem.SubscribeReactin<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReactin<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        // 取消注册执行器
        ActionSystem.DetachPerformer<DrawCardGA>();
        ActionSystem.DetachPerformer<DiscardAllGA>();
        ActionSystem.DetachPerformer<PlayerCardGA>();

        // 取消注册反应
        ActionSystem.UnSubscribeReactin<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnSubscribeReactin<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }
    #endregion

    /// <summary>
    /// 初始化牌堆（战斗开始时）
    /// </summary>
    public void Setup(List<CardData> deakData)
    {
        foreach (var cardData in deakData)
        {
            Card card = new(cardData);
            drawPile.Add(card);
        }
    }


    /// <summary>
    /// 抽牌执行器
    /// </summary>
    private IEnumerator DrawCardsPerformer(DrawCardGA drawCardGA)
    {
        int actualAmount = Mathf.Min(drawCardGA.Amount, drawPile.Count);
        int notDrawnAmount = drawCardGA.Amount - actualAmount;

        for (int i = 0; i < actualAmount; i++)
            yield return DrawCard();

        if (notDrawnAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < notDrawnAmount; i++)
                yield return DrawCard();
        }
    }

    /// <summary>
    /// 弃掉所有手牌执行器
    /// </summary>
    private IEnumerator DiscardAllCaorsPerformer(DiscardAllGA discardAll)
    {
        foreach (var card in hand)
        {
            
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hand.Clear();
    }

    /// <summary>
    /// 玩家打出一张卡的执行器
    /// </summary>
    private IEnumerator PlayerCardPerforemer(PlayerCardGA playerCardGA)
    {
        // 从手牌移除并丢进弃牌堆
        hand.Remove(playerCardGA.card);
        CardView cardView = handView.RemoveCard(playerCardGA.card);
        yield return DiscardCard(cardView);

        SpendManaGA spendManaGA = new(playerCardGA.card.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);

        if(playerCardGA.card.Effects != null)
        {
            PerformEffectGA performEffectGA = new(playerCardGA.card.Effects, new() { playerCardGA.ManualTarget});
            ActionSystem.Instance.AddReaction(performEffectGA);
            AttackEnemyGA attackEnemyGA = new(playerCardGA.ManualTarget);
            ActionSystem.Instance.AddReaction(attackEnemyGA);
        }

        //执行卡牌自带的效果
        foreach (var effectWrapper in playerCardGA.card.OtherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            if (targets != null)
            {
                foreach (CombatantView target in targets)
                {
                    Instantiate(damageVFX, target.transform.position, Quaternion.identity);
                }
            }
            PerformEffectGA performEffectGA = new(effectWrapper.Effect,targets);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }



    private void EnemyTurnPreReaction(EnemyTurnGA action)
    {
        // 敌人回合前弃掉所有手牌
        DiscardAllGA discardAllGA = new();
        ActionSystem.Instance.AddReaction(discardAllGA);
    }

    private void EnemyTurnPostReaction(EnemyTurnGA action)
    {
        // 敌人回合后抽 5 张新牌
        DrawCardGA drawCardGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardGA);
    }



    /// <summary>
    /// 从抽牌堆抽一张牌
    /// </summary>
    private IEnumerator DrawCard()
    {
        Card card = drawPile.Draw();
        hand.Add(card);

        // 创建卡牌视图并加入手牌 UI
        CardView cardView = CardViewCreator.Instance.CreateCard(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }

    /// <summary>
    /// 将弃牌堆洗回抽牌堆
    /// </summary>
    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }

    /// <summary>
    /// 弃掉一张牌并播放动画
    /// </summary>
    private IEnumerator DiscardCard(CardView cardView)
    {
        discardPile.Add(cardView.card);
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardwPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }

}
