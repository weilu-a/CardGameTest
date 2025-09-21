using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;


/// <summary>
/// 手牌视图管理，管理手牌数据以及视觉位置
/// </summary>
public class HandView : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private SplineContainer SplineContainer;


    private List<CardView> cards = new();

    public IEnumerator AddCard(CardView g)
    {
        cards.Add(g);
        yield return UpdateCardPosistion(0.15f);
    }
    public CardView RemoveCard(Card card)
    {

        CardView cardView = GetCardView(card);
        if (cardView == null)
        {
            return null;
        }
        cards.Remove(cardView);
        StartCoroutine(UpdateCardPosistion(0.15f));
        return cardView;
    }

    private CardView GetCardView(Card card)
    {
        return cards.Where(CardView => CardView.card == card).FirstOrDefault();
    }

    private IEnumerator UpdateCardPosistion(float duration)
    {
        if (cards.Count == 0)
        {
            yield break;
        }
        float cardSpacing = 1f / maxHandSize;
        float firstCardPosistion = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = SplineContainer.Spline;
        for (int i = 0; i < cards.Count; i++)
        {
            float p = firstCardPosistion + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
            cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i *Vector3.back, duration);
            cards[i].transform.DORotate(rotation.eulerAngles, duration);
            //cards[i].transform.DOLocalRotateQuaternion(rotation, duration);
        }
        yield return new WaitForSeconds(duration);
    }
}
