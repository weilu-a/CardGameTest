using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏动作系统（ActionSystem）
/// - 基于命令模式的异步动作执行框架
/// - 管理游戏动作的生命周期：预执行 → 主执行 → 后执行
/// - 支持动作的前后置订阅（Reaction）
/// - 单例模式，确保全局唯一访问
/// </summary>
public class ActionSystem : Singleton<ActionSystem>
{
    // 当前执行的反应动作列表
    private List<GameAction> reactions;

    /// <summary>
    /// 系统是否正在执行动作（防止并发）
    /// </summary>
    public bool IsPerforming { get; private set; } = false;

    // 动作类型对应的前置订阅回调
    private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();

    // 动作类型对应的后置订阅回调
    private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();

    // 动作类型对应的执行器函数（协程）
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();

    #region 核心方法

    /// <summary>
    /// 执行一个游戏动作
    /// </summary>
    /// <param name="action">动作实例</param>
    /// <param name="OnPerformFinished">动作执行完成后的回调（可选）</param>
    public void Perform(GameAction action, Action OnPerformFinished = null)
    {
        // 如果系统正在执行其他动作，则忽略本次请求
        if (IsPerforming) return;

        IsPerforming = true;

        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            OnPerformFinished?.Invoke();
        }));
    }

    /// <summary>
    /// 将一个反应动作加入当前反应列表
    /// </summary>
    /// <param name="gameAction">反应动作</param>
    public void AddReaction(GameAction gameAction)
    {
        reactions?.Add(gameAction);
    }

    #endregion

    #region 动作执行流程

    /// <summary>
    /// 动作完整执行流程协程：
    /// 1. 执行预反应（Pre）和订阅事件
    /// 2. 执行动作主逻辑（Perform）及中间反应
    /// 3. 执行后置反应（Post）和订阅事件
    /// </summary>
    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        // --- 预执行阶段 ---
        reactions = action.PreReactions;
        PreformSubscribers(action, preSubs);
        yield return PerformReactions();

        // --- 主执行阶段 ---
        reactions = action.PerformReactions;
        yield return PerformPerformer(action);
        yield return PerformReactions();

        // --- 后执行阶段 ---
        reactions = action.PostReactions;
        PreformSubscribers(action, postSubs);
        yield return PerformReactions();

        // 整个流程结束
        OnFlowFinished?.Invoke();
    }

    /// <summary>
    /// 执行动作主逻辑（通过注册的执行器）
    /// </summary>
    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();

        if (performers.ContainsKey(type))
            yield return performers[type](action);
    }

    /// <summary>
    /// 调用订阅者回调（PRE 或 POST）
    /// </summary>
    private void PreformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type type = action.GetType();

        if (subs.ContainsKey(type))
        {
            foreach (var sub in subs[type])
                sub(action);
        }
    }

    /// <summary>
    /// 执行当前反应列表中的所有动作
    /// </summary>
    private IEnumerator PerformReactions()
    {
        foreach (var reaction in reactions)
            yield return Flow(reaction);
    }

    #endregion

    #region 注册/取消动作执行器

    /// <summary>
    /// 注册特定类型动作的执行器（协程）
    /// </summary>
    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);

        if (performers.ContainsKey(type))
            performers[type] = wrappedPerformer;
        else
            performers.Add(type, wrappedPerformer);
    }

    /// <summary>
    /// 移除特定类型动作的执行器
    /// </summary>
    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (performers.ContainsKey(type))
            performers.Remove(type);
    }

    #endregion

    #region 订阅/取消动作事件

    /// <summary>
    /// 订阅特定类型动作的响应事件（PRE 或 POST）
    /// </summary>
    public static void SubscribeReactin<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        void wrappedReaction(GameAction action) => reaction((T)action);

        if (subs.ContainsKey(typeof(T)))
            subs[typeof(T)].Add(wrappedReaction);
        else
        {
            subs.Add(typeof(T), new List<Action<GameAction>> { wrappedReaction });
        }
    }

    /// <summary>
    /// 取消订阅特定类型动作的响应事件
    /// </summary>
    public static void UnSubscribeReactin<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;

        if (subs.ContainsKey(typeof(T)))
        {
            void wrappedReaction(GameAction action) => reaction((T)action);
            subs[typeof(T)].Remove(wrappedReaction);
        }
    }

    #endregion
}
