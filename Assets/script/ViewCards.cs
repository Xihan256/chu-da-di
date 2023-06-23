using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCards : MonoBehaviour
{
    private List<ViewCard> CardsInHand = new List<ViewCard>();//����
    private List<ViewCard> tempCards = new List<ViewCard>();//�����ݴ�
    
    public List<ViewCard> GetCards
    {
        get { return CardsInHand; }
    }

    public List<ViewCard> GetTemp
    {
        get { return tempCards; }
    }

    public void addInHand(GameObject obj)
    {
        ViewCard card = obj.GetComponent<ViewCard>();
        if (!CardsInHand.Contains(card))
        {
            CardsInHand.Add(card);
            //CardsInHand.Sort();
            PosCalculate();
        }
    }

    public void removeFromHand(GameObject obj)
    {
        ViewCard card = obj.GetComponent<ViewCard>();
        if (CardsInHand.Contains(card))
        {
            CardsInHand.Remove(card);
            CardsInHand.Sort();
            PosCalculate();
        }
    }

    public void SaveTemp()
    {
        tempCards.Clear();
        foreach (ViewCard c in CardsInHand)
        {
            if (c.onSelect == true)
            {
                tempCards.Add(c);
            }
        }
    }

    public List<ViewCard> removeCal_Show()
    {
        GameObject card;
        foreach (ViewCard c in tempCards)
        {
            if (CardsInHand.Contains(c))
            {
                card = c.gameObject;
                removeFromHand(card);
                GameObject.Destroy(card, 1f);
            }
        }
        return tempCards;
    }

    private void PosCalculate()
    {
        int cardNum = CardsInHand.Count;
        Vector3 deltaPos;
        for(int i=0; i<cardNum; i++)
        {
            if (cardNum % 2 == 0)
            {
                deltaPos = new Vector3(((1) * (i - cardNum / 2)-0.5f), 0, 0);
                CardsInHand[i].GetTargetPos = GameObject.Find("SelfPlayer").transform.position + deltaPos;
            }
            else if (cardNum % 2 != 0)
            {
                deltaPos = new Vector3((1) * (i - Mathf.Ceil(cardNum / 2)), 0, 0);
                CardsInHand[i].GetTargetPos = GameObject.Find("SelfPlayer").transform.position + deltaPos;
            }
        }
    }

}
