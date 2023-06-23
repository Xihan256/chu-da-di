using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum CardType
{
    spade = 4,//黑桃
    heart = 3,//红心
    club = 2,//梅花
    diamond = 1,//方片
}

public class ViewCard : MonoBehaviour , IComparable<ViewCard>
{
    private CardType cardType;
    public int cardValue;
    private bool canMove = false;
    public bool onSelect = false;
    private Vector3 targetPos;//牌的目标位置

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 3);
        if (Vector3.Distance(transform.position, targetPos) <= 0.03f)
        {
            transform.position = targetPos;
        }
    }

    public void SetImage(string location)
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(location);
    }

    public int CompareTo(ViewCard other)//比较函数，大于返回-1,小于返回1
    {
        if (this.cardValue != other.cardValue) {
            if (this.cardValue == 2) {
                return -1;
            } else if (other.cardValue == 2) {
                return 1;
            } else if (this.cardValue == 1) {
                return -1;
            } else if (other.cardValue == 1) {
                return 1;
            } else {
                return (this.cardValue > other.cardValue ? -1 : 1);
            }
        } else {
            return (this.cardType > other.cardType ? -1 : 1);
        }
    }

    public CardType GetCardType
    {
        set { cardType = value; }
        get { return cardType; }
    }

    public int GetCardValue
    {
        set { cardValue = value; }
        get { return cardValue; }
    }

    public bool GetCanMove
    {
        set { canMove = value; }
        get { return canMove; }
    }

    public Vector3 GetTargetPos
    {
        set { targetPos = value; }
        get { return targetPos; }
    }
}
