using lln.ChuDaDi_MainLogic.cardLogic;
using lln.ChuDaDi_MainLogic.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFrontManager : MonoBehaviour
{
    public GameObject backEnd;
    public GameObject selfPlayer;
    public GameObject left;
    public GameObject up;
    public GameObject right;
    public GameObject cardPrefab;

    private void Start()
    {
        
    }

    //游戏开始，启用玩家物体
    public void WakeUpPlayer()
    {
        selfPlayer.SetActive(true);
        left.SetActive(true);
        up.SetActive(true);
        right.SetActive(true);
    }

    //初始化手牌
    public void CardInit(string json)
    {
        GameObject vcard;
        ViewCards cards = selfPlayer.GetComponent<ViewCards>();
        Transform pTransform = selfPlayer.transform;
        List<Card> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Card>>(json);
        for (int i = 0; i < list.Count; i++)//添加牌的实例化
        {
            vcard = Instantiate(cardPrefab);
            vcard.name = "Card_" + i;
            vcard.transform.SetParent(pTransform);
            vcard.gameObject.GetComponent<RectTransform>().position = Vector3.forward;
            vcard.gameObject.GetComponent<RectTransform>().rotation = Quaternion.identity;
            vcard.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
            vcard.GetComponent<ViewCard>().GetCardType = (CardType)list[i].suit;
            vcard.GetComponent<ViewCard>().GetCardValue = list[i].point;
            vcard.GetComponent<ViewCard>().SetImage("Pokers" + "/" + (CardType)list[i].suit + list[i].point);
            cards.addInHand(vcard);
        }
    }

    //出牌函数
    public void ShowCard()
    {
        ViewCards inHand = selfPlayer.GetComponent<ViewCards>();
        CardsToShow toShow = selfPlayer.GetComponent<CardsToShow>();
        if (toShow.GetCardGroup == null)
        {
            return;
        }
        CardGroup group = toShow.Convert("local");
        string groupJson = Newtonsoft.Json.JsonConvert.SerializeObject(group);
        string recv = backEnd.GetComponent<SingleBackEnd>().showCard(groupJson);
        if (recv.Equals("{}"))
        {
            return;
        }
        else if (recv.StartsWith("a"))
        {
            return;
        }
        else
        {
            inHand.removeCal_Show();
        }
    }

    
}
