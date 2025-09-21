using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerCardGA : GameAction
{
    public EnemyView ManualTarget { get; private set; }
    public Card card { get; set; }

    public PlayerCardGA(Card card)
    {
        this.card = card;
        ManualTarget = null;
    }

    public PlayerCardGA(Card card, EnemyView target)
    {
        this.card = card;
        ManualTarget = target;
    }
}
