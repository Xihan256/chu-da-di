using System;
using System.Collections.Generic;
using lln.ChuDaDi_MainLogic;
using lln.ChuDaDi_MainLogic.cardLogic;
using lln.ChuDaDi_MainLogic.player;
using lln.ChuDaDi_MainLogic.Utils;
using lln.Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class SingleBackEnd : MonoBehaviour
{
    private Game game;
    private Player[] _players;
    public static bool NextN;

    private void Start()
    {
        game = new Game();
        Game.instance = game;
        _players = game.players;
    }

    private void FixedUpdate(){
        for (int i = 0; i < _players.Length; i++){
            if (_players[i] != null && _players[i].cards.cardsOfPlayer.Count == 0){
                string finish = gameFinish();
                GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulFinish(finish);
                break;
            }
        }

        if (NextN){
            NextN = false;
            wakeUpNext();
        }
    }

    public void startGame()
    {
        Player player1 = new Player("ZY", "local");
        Player player2 = new Player("LGX", "right");
        Player player3 = new Player("LZW", "up");
        Player player4 = new Player("LLN", "left");
        game.addPlayer(player1);
        game.addPlayer(player2);
        game.addPlayer(player3);
        game.addPlayer(player4);
        _players = game.players;

        for (int i = 0; i < _players.Length; i++){
            Debug.Log(i + _players[i].ip);
        }
        game.wakeUp();

        string res = game.giveMsgToFrontEnd();
        Player self = JsonConvert.DeserializeObject<Player[]>(res)[0];
        List<Card> cards = self.cards.cardsOfPlayer;
        string s = Newtonsoft.Json.JsonConvert.SerializeObject(cards);
        GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().CardInit(s);
        wakeUpNext();
    }

    public void wakeUpNext(){
        string nextManIP = game.getCurrIp();
        if (nextManIP.Equals("local")){
            Debug.Log("来到主家的回合");
            GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().UpRound();
        } else{
            game.wakeUpPlayer(nextManIP);
        }
    }

    public string showCard(string json)
    {
        if (json.Length == 0)
        {
            return "{}";
        }
        CardGroup o = JsonConvert.DeserializeObject<CardGroup>(json);

        bool res = game.validation(o);

        if (res)
        {
            return json;
        }
        else
        {
            return "a" + o.ip;
        }

    }

    



    public string gameFinish()
    {
        FinishPlayer[] players = game.calculateScore();

        string serializeObject = JsonConvert.SerializeObject(players);

        return serializeObject;
    }
    



    public void addPlayer(string name, string ip)
    {
        Player player = new Player(name, ip);
        game.addPlayer(player);
    }

    public string doNothing(){
        string lastIP = game.getCurrIp();
        bool res = game.doNothing();

        if (res)
        {
            return lastIP;
        }
        else
        {
            return "a" + game.getCurrIp();
        }
    }
}
