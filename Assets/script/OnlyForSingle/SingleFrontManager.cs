using System;
using lln.ChuDaDi_MainLogic;
using lln.ChuDaDi_MainLogic.cardLogic;
using lln.ChuDaDi_MainLogic.Utils;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using lln.ChuDaDi_MainLogic.player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleFrontManager : MonoBehaviour
{
    public GameObject backEnd;

    public GameObject selfPlayer;
    public GameObject left;
    public GameObject up;
    public GameObject right;

    public GameObject showBtn;
    public GameObject skipBtn;
    public GameObject ErrText;
    public GameObject startBtn;
    public GameObject finishCanv;

    public GameObject cardPrefab;
    


    //游戏开始，启用玩家物体
    public void WakeUpPlayer()
    {
        startBtn.SetActive(false);
        PlayerInit();  
        backEnd.GetComponent<SingleBackEnd>().startGame();
    }

    private void PlayerInit()
    {
        
        right.GetComponent<OtherPlayerControl>().GetIP = "right";
        up.GetComponent<OtherPlayerControl>().GetIP = "up";
        left.GetComponent<OtherPlayerControl>().GetIP = "left";
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

    public void UpRound()
    {
        Player[] players = Game.instance.players;
        int[] cards = {players[1].cards.cardsOfPlayer.Count,players[2].cards.cardsOfPlayer.Count,players[3].cards.cardsOfPlayer.Count};

        for (int i = 0; i < 3; i++){
            Debug.Log("牌数 " + cards[i]);
        }
        right.GetComponent<OtherPlayerControl>().TrueSetNum(cards[0]);
        up.GetComponent<OtherPlayerControl>().TrueSetNum(cards[1]);
        left.GetComponent<OtherPlayerControl>().TrueSetNum(cards[2]);

        showBtn.GetComponent<Button>().interactable = true;
        skipBtn.GetComponent<Button>().interactable = true;
    }

    public void DownRound()
    {
        showBtn.GetComponent<Button>().interactable = false;
        skipBtn.GetComponent<Button>().interactable = false;
    }

    public void FailTxt()
    {
        ErrText.SetActive(true);
        Invoke("HideFailTxt", 1.5f);
    }
    private void HideFailTxt()
    {
        ErrText.SetActive(false);
    }

    //出牌函数
    public void ShowCard()
    {
        CardsToShow toShow = selfPlayer.GetComponent<CardsToShow>();
        if (toShow.GetCardGroup == null)
        {
            return;
        }
        selfPlayer.GetComponent<ViewCards>().SaveTemp();
        CardGroup group = toShow.Convert("local");
        string groupJson = Newtonsoft.Json.JsonConvert.SerializeObject(group);
        string recv = backEnd.GetComponent<SingleBackEnd>().showCard(groupJson);
        if (recv.Equals("{}"))
        {
            return;
        }
        else if (recv.StartsWith("a"))
        {
            FailTxt();
            return;
        }
        else
        {
            SeccessfulShow(recv);
            SingleBackEnd.NextN = true;
            //后端提醒下一个人出牌
        }

        DownRound();
    }

    public void SeccessfulShow(string json)
    {
        float delMutiplier = 4f;
        float delMutiplierUp = 2.3f;

        CardGroup group = JsonConvert.DeserializeObject<CardGroup>(json);
        string ip = group.ip;
        
        
        if (ip.Equals("local"))
        {
            List<ViewCard> temp = selfPlayer.GetComponent<ViewCards>().removeCal_Show();
            selfPlayer.GetComponent<CardsToShow>().clearCards();
            gameObject.GetComponent<CardsOnTable>().ShowOut(temp);
        }
        else if (right.GetComponent<OtherPlayerControl>().GetIP == ip)
        {
            right.GetComponent<OtherPlayerControl>().ShowOut(group.cards, Vector3.left * delMutiplier);
        }
        else if (up.GetComponent<OtherPlayerControl>().GetIP == ip)
        {
            up.GetComponent<OtherPlayerControl>().ShowOut(group.cards, Vector3.down * delMutiplierUp);
        }
        else if (left.GetComponent<OtherPlayerControl>().GetIP == ip)
        {
            left.GetComponent<OtherPlayerControl>().ShowOut(group.cards, Vector3.right * delMutiplier);
        }
    }

    public void Skip()
    {
        string recv = backEnd.GetComponent<SingleBackEnd>().doNothing();
        if (recv.StartsWith("a")){
            FailTxt();
            return;
        }
        else
        {
            SeccessfulSkip(recv);
            SingleBackEnd.NextN = true;
            //通知下一个人出牌
        }

        DownRound();
    }

    public void SeccessfulSkip(string ip)
    {
        if ("local".Equals(ip))
        {
            //删上次牌
            foreach (Transform child in transform)
            {
                GameObject chi = child.gameObject;
                // 对每个子物体进行操作
                if (chi.GetComponent<ViewCard>() != null)
                {
                    GameObject.Destroy(chi);
                }
            }
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (right.GetComponent<OtherPlayerControl>().GetIP == ip)
        {
            right.GetComponent<OtherPlayerControl>().Skiptxt();
        }
        else if (up.GetComponent<OtherPlayerControl>().GetIP == ip)
        {
            up.GetComponent<OtherPlayerControl>().Skiptxt();
        }
        else if (left.GetComponent<OtherPlayerControl>().GetIP == ip)
        {
            left.GetComponent<OtherPlayerControl>().Skiptxt();
        }
    }

    public void SeccessfulFinish(string json)
    {
        finishCanv.SetActive(true);
        FinishPlayer[] players = JsonConvert.DeserializeObject<FinishPlayer[]>(json);

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < players.Length; i++)
        {
            sb.Append(players[i].name + " :\t" + players[i].score + "\n");
        }
        finishCanv.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = sb.ToString();
    }

    public void JumpOut()
    {
        SceneManager.LoadScene("001_login");
    }


}
