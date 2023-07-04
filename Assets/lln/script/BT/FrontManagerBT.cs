using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using lln.Bluetooth.BTTask;
using lln.Bluetooth.server;
using lln.ChuDaDi_MainLogic;
using lln.ChuDaDi_MainLogic.cardLogic;
using lln.ChuDaDi_MainLogic.Utils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FrontManagerBT : MonoBehaviour
{
    public static bool isHost;

    private string selfIP;
    //??????
    public GameObject selfPlayer;
    public GameObject left;
    public GameObject up;
    public GameObject right;
    //????UI
    public GameObject showBtn;
    public GameObject skipBtn;
    public GameObject ErrText;
    public GameObject startBtn;
    public GameObject finishCanv;
    //task???
    public GameObject cardPrefab;
    public GameObject showPrefab;
    public GameObject skipPrefab;
    public GameObject finPrefab;
    
    //调函数参数
    public bool initN=false;
    public bool skipN=false;
    public bool showN=false;
    public bool roundN = false;
    public bool finishN = false;
    public string initJson;
    public string skipIp;
    public string showJson;
    public string finishJson;
    

    private void Start()
    {
        selfIP = Main.selfname;
        isHost = FrontManager.isHost;
    }

    private void Update(){
        if (initN){
            initN = false;
            CardInit(initJson);
        }

        if (skipN){
            skipN = false;
            SeccessfulSkip(skipIp);
        }

        if (showN){
            showN = false;
            SeccessfulShow(showJson);
        }

        if (roundN){
            roundN = false;
            UpRound();
        }

        if (finishN){
            finishN = false;
            SeccessfulFinish(finishJson);
        }
    }

    public string GetIP
    {
        set { selfIP = value; }
        get { return selfIP; }
    }

    //?????????????
    public void WakeUpPlayer()
    {
        startBtn.SetActive(false);
        GameObject.Find("Main").GetComponent<Main>().otherPlayerInit();
        if (isHost)
        {
            GameObject.Find("BackEnd").GetComponent<TheBackEndBT>().startGame();
        } else{
            GameObject.Find("Main").GetComponent<Main>().connectToServer();
        }
        
        showBtn.SetActive(true);
        skipBtn.SetActive(true);
        selfPlayer.SetActive(true);
        left.SetActive(true);
        up.SetActive(true);
        right.SetActive(true);
    }


    //?????????
    public void CardInit(string json)
    {
        GameObject vcard;
        ViewCards cards = selfPlayer.GetComponent<ViewCards>();
        Transform pTransform = selfPlayer.transform;
        List<Card> list = JsonConvert.DeserializeObject<List<Card>>(json);
        //list.Sort();
        for(int i = 0; i < list.Count; i++)
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

        initN = false;
    }
    

    //??????
    public void UpRound()
    {
        //?????????
        showBtn.GetComponent<Button>().interactable = true;
        skipBtn.GetComponent<Button>().interactable = true;
        roundN = false;
    }

    //??????
    public void DownRound()
    {
        //????????
        showBtn.GetComponent<Button>().interactable = false;
        skipBtn.GetComponent<Button>().interactable = false;
    }

    //???????
    public void FailTxt()
    {
        //????????????????????????
        ErrText.SetActive(true);
        Invoke("HideFailTxt", 1.5f);
    }
    private void HideFailTxt()
    {
        ErrText.SetActive(false);
    }


    public void ShowCard()
    {
        CardsToShow toShow = selfPlayer.GetComponent<CardsToShow>();
        if(toShow.GetCardGroup == null)
        {
            Debug.LogWarning("传了空的牌组");
            return;
        }
        selfPlayer.GetComponent<ViewCards>().SaveTemp();
        CardGroup group = toShow.Convert(selfIP);
        string groupJson = JsonConvert.SerializeObject(group);

        if (isHost){
            GameObject.Find("Main").GetComponent<Main>().processMsg("s"+groupJson);
        } else{
            GameObject showObj = Instantiate(showPrefab);
            showObj.GetComponent<BTShowCards>().workToDo(groupJson);
            GameObject.Destroy(showObj, 2.0f);
        }

        DownRound();
    }

    //????

    //??????
    public void SeccessfulShow(string json){
        float delMutiplier = 4f;
        float delMutiplierUp = 2.3f;
        
        CardGroup group = JsonConvert.DeserializeObject<CardGroup>(json);
        string ip = group.ip;
        if (selfIP == ip){
            List<ViewCard> temp = selfPlayer.GetComponent<ViewCards>().removeCal_Show();
            selfPlayer.GetComponent<CardsToShow>().clearCards();
            gameObject.GetComponent<CardsOnTable>().ShowOut(temp);
            if (selfPlayer.GetComponent<ViewCards>().GetCards.Count == 0)
            {
                GameObject finObj = Instantiate(finPrefab);
                finObj.GetComponent<BTFinishGame>().WorkToDo();
                GameObject.Destroy(finObj, 2.0f);
            }
        } else if(right.GetComponent<OtherPlayerControl>().GetIP==ip){
            right.GetComponent<OtherPlayerControl>().ShowOut(group.cards,Vector3.left * delMutiplier);
        }
        else if (up.GetComponent<OtherPlayerControl>().GetIP == ip){
            up.GetComponent<OtherPlayerControl>().ShowOut(group.cards,Vector3.down * delMutiplierUp);
        }
        else if (left.GetComponent<OtherPlayerControl>().GetIP == ip){
            left.GetComponent<OtherPlayerControl>().ShowOut(group.cards,Vector3.right * delMutiplier);
        }

        
    }

    //????
    public void Skip()
    {
        if (isHost){
            GameObject.Find("Main").GetComponent<Main>().processMsg("no");
        } else{
            GameObject skipObj = Instantiate(skipPrefab);
            skipObj.GetComponent<BTSkip>().workToDo();
            GameObject.Destroy(skipObj, 2.0f);
        }

        DownRound();
    }

    public void SeccessfulSkip(string ip){
        if (selfIP == ip){
            //删上次牌
            foreach (Transform child in transform){
                GameObject chi = child.gameObject;
                // 对每个子物体进行操作
                if (chi.GetComponent<ViewCard>() != null){
                    GameObject.Destroy(chi);
                }
            }
            transform.GetChild(0).gameObject.SetActive(true);
        } else if(right.GetComponent<OtherPlayerControl>().GetIP==ip){
            right.GetComponent<OtherPlayerControl>().Skiptxt();
        }
        else if (up.GetComponent<OtherPlayerControl>().GetIP == ip){
            up.GetComponent<OtherPlayerControl>().Skiptxt();
        }
        else if (left.GetComponent<OtherPlayerControl>().GetIP == ip){
            left.GetComponent<OtherPlayerControl>().Skiptxt();
        }

        skipN = false;
    }

    public void SeccessfulFinish(string json){
        finishCanv.SetActive(true);
        FinishPlayer[] players = JsonConvert.DeserializeObject<FinishPlayer[]>(json);

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < players.Length; i++){
            sb.Append(players[i].name + " :\t" + players[i].score + "\n");
        }
        finishCanv.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = sb.ToString();
    }

    public void JumpOut(){
        SceneManager.LoadScene("001_login");

        Thread thread = new Thread(addToDatabase);
        thread.Start();
    }

    private void addToDatabase(){
        //finishJson
        FinishPlayer[] players = JsonConvert.DeserializeObject<FinishPlayer[]>(finishJson);

        for (int i = 0; i < players.Length; i++){
            string request = "http://8.134.143.81:8080/put/point?name=" + players[i].name + "&point=" +
                             players[i].score;
            
            using (HttpClient client = new HttpClient()){
                string responseBody = client.GetStringAsync(request).Result;
                Debug.Log(responseBody);
            }
            
        }
    }
}
