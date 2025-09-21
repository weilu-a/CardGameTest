using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 实例化预制体
/// </summary>

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView cardPrefab;

    public CardView CreateCard(Card cnum, Vector3 viewPosition, Quaternion viewRotation)
    {
        CardView card = Instantiate(cardPrefab, viewPosition, viewRotation);
        card.Setup(cnum);
        return card;
    }
}
