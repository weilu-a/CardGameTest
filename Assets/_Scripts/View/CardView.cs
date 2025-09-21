using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{

    //引用的组件
    [SerializeField] private TMP_Text mana;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask drop;

    //牌组
    public Card card { get; private set; }

    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;


    public void Setup(Card cnum)
    {
        card = cnum;
        if (card.data == null)
        {
            Debug.LogWarning("卡牌数据为空，初始化失败");
            return;
        }
        mana.text = cnum.Mana.ToString();
        imageSR.sprite = cnum.Image;

    }


    /// <summary>
    /// 鼠标悬停事件 - 显示卡牌详细信息
    /// </summary>
    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        wrapper.SetActive(false);
        Vector3 pos = new(transform.position.x, -1.7f, 0);
        CardViewHoverSystem.Instance.Show(card, pos);
    }

    /// <summary>
    /// 鼠标离开事件 - 隐藏卡牌详细信息
    /// </summary>
    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }

    /// <summary>
    /// 鼠标按下事件 - 开始拖拽卡牌
    /// </summary>
    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if(card.Effects != null)
        {
            ManuaTargetSystem.Instance.StartTargeting(transform.position);
        }
        else
        {
            Interactions.Instance.PlayerIsDragging = true;
            wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();

            dragStartPosition = transform.position;
            dragStartRotation = transform.rotation;

            transform.rotation = Quaternion.Euler(0, 0, 0);

            transform.position = MoustUtil.getMousePositionInWorld(-1);
        }
    }

    /// <summary>
    /// 鼠标拖拽事件 - 更新卡牌位置跟随鼠标
    /// </summary>
    private void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if(card.Effects != null) return;
        transform.position = MoustUtil.getMousePositionInWorld(-1);
    }

    /// <summary>
    /// 鼠标释放事件 - 结束拖拽并处理卡牌放置
    /// </summary>
    private void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if(card.Effects != null)
        {
            EnemyView target = ManuaTargetSystem.Instance.EndTargeting(MoustUtil.getMousePositionInWorld());
            if(target != null && ManaSystem.Instance.HasEnoughMana(card.Mana))
            {
                PlayerCardGA playerCardGA = new(card, target);
                ActionSystem.Instance.Perform(playerCardGA);
            }
        }
        else
        {
            if (ManaSystem.Instance.HasEnoughMana(card.Mana) 
                && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, drop)
                )
            {
                PlayerCardGA playerCardGA = new(card);
                ActionSystem.Instance.Perform(playerCardGA);
            }
            else
            {
                transform.position = dragStartPosition;
                transform.rotation = dragStartRotation;
            }

            Interactions.Instance.PlayerIsDragging = false;
        }
    }
}
