using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GameAction
{
    public List<GameAction> PreReactions { get; private set; } = new();// 执行前反应
    public List<GameAction> PerformReactions { get; private set; } = new();// 执行中反应
    public List<GameAction> PostReactions { get; private set; } = new();//执行后反应
}

