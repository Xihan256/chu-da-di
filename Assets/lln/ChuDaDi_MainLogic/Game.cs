using System.Collections.Generic;
using lln.ChuDaDi_MainLogic.player;
using lln.ChuDaDi_MainLogic.rules;
using lln.ChuDaDi_MainLogic.cardLogic;
using lln.ChuDaDi_MainLogic.Utils;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

namespace lln.ChuDaDi_MainLogic
{
    public class Game
    {
        public Player[] players; //4
        private Rule currentRule;
        private Dictionary<string, Rule> rulesMap;
        private CardGroup currentGroup;
        private int currentIndex;
        private int numOfDoNothing;

        public static Game instance;


        public Game()
        {
            players = new Player[4];
            
            rulesMap = new Dictionary<string, Rule>();
            currentGroup = null;
            numOfDoNothing = 0;

            rulesMap.Add(CardGroup.EVERYTHING, new Everything());
            rulesMap.Add(CardGroup.SINGLE, new Single());
            rulesMap.Add(CardGroup.PAIR, new Pair());
            rulesMap.Add(CardGroup.THREE, new Three());
            rulesMap.Add(CardGroup.FOUR, new Four());
            rulesMap.Add(CardGroup.SHUNZI, new ShunZi());
            rulesMap.Add(CardGroup.TONGHUA, new TongHua());
            rulesMap.Add(CardGroup.THREE_WITH_PAIR, new ThreeWithTwo());
            rulesMap.Add(CardGroup.FOUR_WITH_SINGLE, new FourWithSingle());
            rulesMap.Add(CardGroup.TONGHUASHUN, new TongHuaShun());
            
            currentRule = rulesMap[CardGroup.EVERYTHING];
        }

        public void setPlayerName(string name, string ip){
            for (int i = 0; i < players.Length; i++){
                if (players[i].ip.Equals(ip)){
                    players[i].name = name;
                    break;
                }
            }
        }

        public void wakeUp()
        {
            List<Card> cards = new List<Card>();
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 13; j++)
                {
                    cards.Add(new Card(i, j));
                }
            }

            Cards.shuffle(cards);

            for (int i = 0; i < 4; i++){
                Debug.Log(players[i].ip);
                Debug.Log(players[i].name);
            }


            for (int i = 0; i < 13; i++)
            {
                players[0].addCard(cards[i]);
            }

            for (int i = 0; i < 13; i++)
            {
                players[1].addCard(cards[i + 13]);
            }
            for (int i = 0; i < 13; i++)
            {
                players[2].addCard(cards[i + 26]);
            }
            for (int i = 0; i < 13; i++)
            {
                players[3].addCard(cards[i + 39]);
            }

            for (int i = 0; i < 4; i++)
            {
                players[i].sortCards();
            }

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].hasDiamond3())
                {
                    currentIndex = i;
                }
            }
        }

        public string giveMsgToFrontEnd()
        {
            string res = JsonConvert.SerializeObject(players);
            return res;
        }

        public void addPlayer(Player player)
        {
            for (int i = 0; i < 4; i++)
            {
                if (players[i] == null)
                {
                    players[i] = player;
                    break;
                }
            }
        }

        public bool doNothing()
        {
            if (numOfDoNothing == 3){
                
                return false;
            }

            changeCurrentPlayer();
            numOfDoNothing++;
            if (numOfDoNothing == 3){
                currentRule = rulesMap[CardGroup.EVERYTHING];
            }
            return true;
        }

        public string getCurrIp()
        {
            return players[currentIndex].ip;
        }

        public bool validation(CardGroup group)
        {
            if (group == null)
            {
                return false;
            }

            bool contains3 = false;
            if (currentGroup == null){
                List<Card> list = group.cards;
                for (int i = 0; i < list.Count; i++){
                    if (list[i].suit == 1 && list[i].point == 3){
                        contains3 = true;
                    }
                }
                if (!contains3){
                    return false;
                }
            }

            
            
            bool res = currentRule.validate(currentGroup, group);
            

            if (res)
            {
                currentGroup = group;
                currentRule = rulesMap[group.type];
                for (int i = 0; i < 4; i++){
                    if (players[i].ip.Equals(group.ip))
                    {
                        players[i].dropCard(group);
                    }
                }
                changeCurrentPlayer();
                numOfDoNothing = 0;
                Debug.LogWarning("yes 能出");
            } else{
                Debug.LogWarning("rule的类型是" + currentRule.GetType());
                Debug.LogWarning("wrong 不能出");
            }
            return res;
        }

        private void changeCurrentPlayer()
        {
            if (this.currentIndex == 3)
            {
                this.currentIndex = 0;
            }
            else
            {
                this.currentIndex++;
            }
        }

        public FinishPlayer[] calculateScore()
        {
            int[] cardScores = new int[4];
            for (int i = 0; i < 4; i++)
            {
                cardScores[i] = players[i].getCardSize();//todo

                if (players[i].getCardSize() >= 8 && players[i].getCardSize() < 10)
                {
                    cardScores[i] *= 2;
                }
                else if (players[i].getCardSize() >= 10 && players[i].getCardSize() < 13)
                {
                    cardScores[i] *= 3;
                }
                else if (players[i].getCardSize() == 13)
                {
                    cardScores[i] *= 4;
                }

                if (players[i].getCardSize() >= 8 && players[i].hasBlack2())
                {
                    cardScores[i] *= 2;
                }
            }

            int[] finScores = new int[4];

            for (int i = 0; i < 4; i++)
            {
                int sum = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (j != i)
                    {
                        sum += cardScores[j];
                    }
                }
                finScores[i] = sum - 3 * cardScores[i];
            }

            FinishPlayer[] fplayers = new FinishPlayer[4];
            for (int i = 0; i < 4; i++)
            {
                fplayers[i] = new FinishPlayer(finScores[i], players[i].getName(), players[i].getIP());
            }

            return fplayers;
        }
    }
}