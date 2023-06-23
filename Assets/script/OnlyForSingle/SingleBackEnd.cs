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

    private void Start()
    {
        game = new Game();
    }

    public void startGame()
    {
        game.wakeUp();
        GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().WakeUpPlayer();
        string res = game.giveMsgToFrontEnd();
        Player self = JsonConvert.DeserializeObject<Player[]>(res)[0];
        List<Card> cards = self.cards.cardsOfPlayer;
        string s = Newtonsoft.Json.JsonConvert.SerializeObject(cards);
        GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().CardInit(s);
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

    public void regist(string name)
    {
        //todo
    }

    public void addAchievement(string achievement)
    {
        //todo
    }



    public void addPlayer(string name, string ip)
    {
        Player player = new Player(name, ip);
        game.addPlayer(player);
    }
}
