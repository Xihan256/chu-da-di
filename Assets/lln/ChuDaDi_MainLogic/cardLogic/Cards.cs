using System;
using System.Collections.Generic;
using System.Linq;
using lln.ChuDaDi_MainLogic.Utils;


namespace lln.ChuDaDi_MainLogic.cardLogic{
    public class Cards{
        
        public List<Card> cardsOfPlayer;

        public Cards(){
            cardsOfPlayer = new List<Card>();
        }

        public static void shuffle(List<Card> list){
            Random r = new System.Random();
            for (int i = 0; i < list.Count; i++){
                int seed = r.Next() % 52;
                if (seed == i){
                    seed = r.Next() % 52;
                }
                (list[seed], list[i]) = (list[i], list[seed]);
            }
        }

        public List<Card> getCardsOfPlayer(){
            return this.cardsOfPlayer;
        }

        public void addCard(Card card){
            cardsOfPlayer.Add(card);
        }

        public void sortCards(){
            cardsOfPlayer.Sort();
        }

        public static Card findMax(List<Card> cards){
            int index = -1;
            int peekPoint = 0,peekSuit = 0;
            for (int i = 0; i < cards.Count; i++){
                if (cards[i].point > peekPoint){
                    peekPoint = cards[i].point;
                    peekSuit = cards[i].suit;
                    index = i;
                }else if (cards[i].point == peekPoint){
                    if (cards[i].suit > peekSuit){
                        peekSuit = cards[i].suit;
                        index = i;
                    }
                }
            }

            return cards[index];
        }

        public void drop(CardGroup group){
            List<Card>list = group.cards;
            for (int i = 0; i < list.Count; i++){
                for (int j = 0; j < cardsOfPlayer.Count; j++){
                    if (list[i].Equals(cardsOfPlayer[j])){
                        cardsOfPlayer.Remove(cardsOfPlayer[j]);
                    }
                }
            }
        }

        public void setCardsOfPlayer(List<Card> c){
            this.cardsOfPlayer = c;
        }
    }
}