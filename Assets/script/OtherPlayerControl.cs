using System.Collections;
using System.Collections.Generic;
using lln.ChuDaDi_MainLogic.cardLogic;
using UnityEngine;
using TMPro;

public class OtherPlayerControl : MonoBehaviour
{
    private Transform handNum;
    private TextMeshProUGUI Text;
    private string name;
    private string Ip;
    private int cardnum = 13;

    public GameObject cardPrefab;

    public string GetIP
    {
        set { Ip = value; }
        get { return Ip; }
    }
    
    public string GetName
    {
        set { name = value; }
        get { return name; }
    }

    private void Start()
    {
        handNum = transform.Find("handNum");
        Text = handNum.GetComponent<TextMeshProUGUI>();
    }

    public void SetNum(int newNum)
    {
        if(handNum != null){
            cardnum = cardnum - newNum;
            Text.text = cardnum.ToString();
            Debug.LogWarning("不要多次调用?");
        }
    }

    public void ShowOut(List<Card> cards , Vector3 del){
        //删上次牌
        foreach (Transform child in transform){
            GameObject chi = child.gameObject;
            // 对每个子物体进行操作
            if (chi.GetComponent<ViewCard>() != null){
                GameObject.Destroy(chi);
            }
        }
        //删掉“不出”
        if (transform.GetChild(1).gameObject.activeSelf){
            transform.GetChild(1).gameObject.SetActive(false);
        }

        GameObject vcard;
        Transform pTransform = gameObject.transform;
        int cardNum = cards.Count;
        SetNum(cardNum);
        Vector3 deltaPos;
        for (int i = 0; i < cardNum; i++)
        {
            if (cardNum % 2 == 0)
            {
                deltaPos = new Vector3(((0.5f) * (i - cardNum / 2) - 0.5f), 0, 0);
                vcard = Instantiate(cardPrefab);
                vcard.name = "Card_" + i;
                vcard.transform.SetParent(pTransform);
                vcard.gameObject.GetComponent<RectTransform>().position = gameObject.transform.position + deltaPos + del;
                vcard.gameObject.GetComponent<RectTransform>().rotation = Quaternion.identity;
                vcard.gameObject.GetComponent<RectTransform>().localScale = Vector3.one/2;
                vcard.GetComponent<ViewCard>().GetTargetPos = gameObject.transform.position + deltaPos+ del;
                vcard.GetComponent<ViewCard>().GetCardType = (CardType)cards[i].suit;
                vcard.GetComponent<ViewCard>().GetCardValue = cards[i].point;
                vcard.GetComponent<ViewCard>().SetImage("Pokers" + "/" + (CardType) cards[i].suit + cards[i].point);
            }
            else if (cardNum % 2 != 0)
            {
                deltaPos = new Vector3((0.5f) * (i - Mathf.Ceil(cardNum / 2)), 0, 0);
                vcard = Instantiate(cardPrefab);
                vcard.name = "Card_" + i;
                vcard.transform.SetParent(pTransform);
                vcard.gameObject.GetComponent<RectTransform>().position = gameObject.transform.position + deltaPos + del;
                vcard.gameObject.GetComponent<RectTransform>().rotation = Quaternion.identity;
                vcard.gameObject.GetComponent<RectTransform>().localScale = Vector3.one/2;
                vcard.GetComponent<ViewCard>().GetTargetPos = gameObject.transform.position + deltaPos+ del;
                vcard.GetComponent<ViewCard>().GetCardType = (CardType)cards[i].suit;
                vcard.GetComponent<ViewCard>().GetCardValue = cards[i].point;
                vcard.GetComponent<ViewCard>().SetImage("Pokers" + "/" + (CardType)cards[i].suit + cards[i].point);
            }
        }
    }

    public void Skiptxt(){
        foreach (Transform child in transform){
            GameObject chi = child.gameObject;
            // 对每个子物体进行操作
            if (chi.GetComponent<ViewCard>() != null){
                GameObject.Destroy(chi);
            }
        }
        transform.GetChild(1).gameObject.SetActive(true);
    }
}


