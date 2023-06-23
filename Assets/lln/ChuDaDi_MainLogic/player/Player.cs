using System.Collections.Generic;
using lln.ChuDaDi_MainLogic.cardLogic;
using lln.ChuDaDi_MainLogic.Utils;
using UnityEngine;

namespace lln.ChuDaDi_MainLogic.player
{

    public class Player
    {
        public Cards cards;
        public string name { get; set; }
        public string ip { get; set; }

        public Player(string name, string ip)
        {
            this.name = name;
            this.ip = ip;
            this.cards = new Cards();
        }

        public string getName()
        {
            return this.name;
        }

        public string getIP()
        {
            return this.ip;
        }

        public Cards getCards()
        {
            return this.cards;
        }

        public void addCard(Card card)
        {
            this.cards.addCard(card);
        }

        public void sortCards()
        {
            this.cards.sortCards();
        }


        public void dropCard(CardGroup group)
        {
            cards.drop(group);
        }

        public int getCardSize()
        {
            return this.cards.cardsOfPlayer.Count;
        }

        public bool hasBlack2()
        {
            List<Card> list = this.cards.cardsOfPlayer;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].point == 2 && list[i].suit == 4)
                {
                    return true;
                }
            }

            return false;
        }

        public bool hasDiamond3()
        {
            List<Card> list = this.cards.cardsOfPlayer;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].point == 3 && list[i].suit == 1)
                {
                    return true;
                }
            }

            return false;
        }

        
    }
}