using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 展示与隐藏悬停视图
/// </summary>
public class CardViewHoverSystem : Singleton<CardViewHoverSystem>
{
    [SerializeField] private CardView card;

    public void Show(Card cnum, Vector3 position)
    {
        card.gameObject.SetActive(true);
        card.Setup(cnum);
        card.transform.position = position;
    }
    public void Hide()
    {
        card.gameObject.SetActive(false);
    }
}

