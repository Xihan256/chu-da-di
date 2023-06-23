using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsOnTable : MonoBehaviour
{
    public GameObject cardPrefab;

    public void ShowOut(List<ViewCard> cards)
    {
        //删上次牌
        foreach (Transform child in transform){
            GameObject chi = child.gameObject;
            // 对每个子物体进行操作
            if (chi.GetComponent<ViewCard>() != null){
                GameObject.Destroy(chi);
            }
        }
        //删掉“不出”
        if (transform.GetChild(0).gameObject.activeSelf){
            transform.GetChild(0).gameObject.SetActive(false);
        }
        GameObject vcard;
        Transform pTransform = gameObject.transform;
        int cardNum = cards.Count;
        Vector3 deltaPos;
        for (int i = 0; i < cardNum; i++)
        {
            if (cardNum % 2 == 0)
            {
                deltaPos = new Vector3(((1) * (i - cardNum / 2) - 0.5f), 0, 0);
                vcard = Instantiate(cardPrefab);
                vcard.name = "Card_" + i;
                vcard.transform.SetParent(pTransform);
                vcard.gameObject.GetComponent<RectTransform>().position = gameObject.transform.position + deltaPos;
                vcard.gameObject.GetComponent<RectTransform>().rotation = Quaternion.identity;
                vcard.gameObject.GetComponent<RectTransform>().localScale = Vector3.one/2;
                vcard.GetComponent<ViewCard>().GetTargetPos = gameObject.transform.position + deltaPos;
                vcard.GetComponent<ViewCard>().GetCardType = cards[i].GetCardType;
                vcard.GetComponent<ViewCard>().GetCardValue = cards[i].GetCardValue;
                vcard.GetComponent<ViewCard>().SetImage("Pokers" + "/" + cards[i].GetCardType + cards[i].GetCardValue);
            }
            else if (cardNum % 2 != 0)
            {
                deltaPos = new Vector3((1) * (i - Mathf.Ceil(cardNum / 2)), 0, 0);
                vcard = Instantiate(cardPrefab);
                vcard.name = "Card_" + i;
                vcard.transform.SetParent(pTransform);
                vcard.gameObject.GetComponent<RectTransform>().position = gameObject.transform.position + deltaPos;
                vcard.gameObject.GetComponent<RectTransform>().rotation = Quaternion.identity;
                vcard.gameObject.GetComponent<RectTransform>().localScale = Vector3.one/2;
                vcard.GetComponent<ViewCard>().GetTargetPos = gameObject.transform.position + deltaPos;
                vcard.GetComponent<ViewCard>().GetCardType = cards[i].GetCardType;
                vcard.GetComponent<ViewCard>().GetCardValue = cards[i].GetCardValue;
                vcard.GetComponent<ViewCard>().SetImage("Pokers" + "/" + cards[i].GetCardType + cards[i].GetCardValue);
            }
        }
    }
}
