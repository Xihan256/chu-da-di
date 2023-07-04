using System.Collections.Generic;
using System.Linq;
using System.Threading;
using lln.ChuDaDi_MainLogic.cardLogic;
using lln.ChuDaDi_MainLogic.rules;
using lln.ChuDaDi_MainLogic.Utils;
using Newtonsoft.Json;
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

        public void DoSingle(CardGroup group,Rule rule){
            
            Game game = Game.instance;
            
            List<Card> selfCards = this.cards.cardsOfPlayer;
            if (rule.GetType() == typeof(Everything)){
                int index = selfCards.Count - 1;
                Card cardMin = selfCards[index];

                List<Card> indexs = new List<Card>();

                for (int i = 0; i < selfCards.Count; i++){
                    if (selfCards[i].point == cardMin.point){
                        indexs.Add(selfCards[i]);
                    }
                }

                if (indexs.Count == 1){
                    CardGroup group1 = new CardGroup(1,CardGroup.SINGLE , this.ip);
                    group1.cards = indexs;
                    string json = JsonConvert.SerializeObject(group1);
                    for (int i = 0; i < indexs.Count; i++){
                        selfCards.Remove(indexs[i]);
                    }

                    GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulShow(json);
                    game.numOfDoNothing = 0;
                    game.currentRule = game.rulesMap[CardGroup.SINGLE];
                    game.currentGroup = group1;
                    game.changeCurrentPlayer();
                    SingleBackEnd.NextN = true;

                }else if (indexs.Count == 2){
                    CardGroup group1 = new CardGroup(2,CardGroup.PAIR , this.ip);
                    indexs.Sort();
                    group1.cards = indexs;
                    string json = JsonConvert.SerializeObject(group1);
                    GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulShow(json);
                    for (int i = 0; i < indexs.Count; i++){
                        selfCards.Remove(indexs[i]);
                    }
                    
                    game.numOfDoNothing = 0;
                    game.currentRule = game.rulesMap[CardGroup.PAIR];
                    game.currentGroup = group1;
                    game.changeCurrentPlayer();
                    SingleBackEnd.NextN = true;
                }else if (indexs.Count == 3){
                    List<Card> pairList = new List<Card>();
                    for (int i = selfCards.Count - 1 - 3; i >= 1; i--){
                        int point = selfCards[i].point;
                        for (int j = i - 1; j >= 0 ;j--){
                            if (point == selfCards[j].point){
                                pairList.Add(selfCards[j]);
                                pairList.Add(selfCards[i]);
                                break;
                            } 
                        }

                        if (pairList.Count == 2){
                            break;
                        }
                    }

                    if (pairList.Count == 2){
                        indexs.Add(pairList[0]);
                        indexs.Add(pairList[1]);
                        
                        CardGroup group1 = new CardGroup(5,CardGroup.THREE_WITH_PAIR , this.ip);
                        indexs.Sort();
                        group1.cards = indexs;
                        
                        string json = JsonConvert.SerializeObject(group1);
                        GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulShow(json);
                        for (int i = 0; i < indexs.Count; i++){
                            selfCards.Remove(indexs[i]);
                        }
                        
                        game.numOfDoNothing = 0;
                        game.currentRule = game.rulesMap[CardGroup.THREE_WITH_PAIR];
                        game.currentGroup = group1;
                        game.changeCurrentPlayer();
                        SingleBackEnd.NextN = true;
                    } else{
                        CardGroup group1 = new CardGroup(3,CardGroup.THREE , this.ip);
                        indexs.Sort();
                        group1.cards = indexs;
                        string json = JsonConvert.SerializeObject(group1);
                        GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulShow(json);
                        for (int i = 0; i < indexs.Count; i++){
                            selfCards.Remove(indexs[i]);
                        }
                        
                        game.numOfDoNothing = 0;
                        game.currentRule = game.rulesMap[CardGroup.THREE];
                        game.currentGroup = group1;
                        game.changeCurrentPlayer();
                        SingleBackEnd.NextN = true;
                    }
                } else if(indexs.Count == 4){
                    if (selfCards.Count == 4){
                        CardGroup group1 = new CardGroup(4,CardGroup.FOUR , this.ip);
                        indexs.Sort();
                        group1.cards = indexs;
                        string json = JsonConvert.SerializeObject(group1);
                        GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulShow(json);
                        for (int i = 0; i < indexs.Count; i++){
                            selfCards.Remove(indexs[i]);
                        }
                        
                        game.numOfDoNothing = 0;
                        game.currentRule = game.rulesMap[CardGroup.FOUR];
                        game.currentGroup = group1;
                        game.changeCurrentPlayer();
                        SingleBackEnd.NextN = true;
                    } else{
                        int ix = selfCards.Count - 1 - 4;
                        indexs.Add(selfCards[ix]);
                    
                        CardGroup group1 = new CardGroup(5,CardGroup.FOUR_WITH_SINGLE , this.ip);
                        indexs.Sort();
                        group1.cards = indexs;
                        string json = JsonConvert.SerializeObject(group1);
                        GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulShow(json);
                        for (int i = 0; i < indexs.Count; i++){
                            selfCards.Remove(indexs[i]);
                        }
                        
                        game.numOfDoNothing = 0;
                        game.currentRule = game.rulesMap[CardGroup.FOUR_WITH_SINGLE];
                        game.currentGroup = group1;
                        game.changeCurrentPlayer();
                        SingleBackEnd.NextN = true;
                    }

                    
                }
                return;
            }
            
            
            string type = group.type;

            List<Card> cardToShow = new List<Card>();
            if (type.Equals(CardGroup.SINGLE)){
                int _point = -1;
                for (int i = selfCards.Count - 1; i >= 0 ; i--){
                    if (!selfCards[i].greaterThan(group.cards[0])){
                        continue;
                    }
                    
                    _point = selfCards[i].point;
                    for (int j = i - 1; j >= 1 ; j--){
                        if (selfCards[j].point != _point){
                            cardToShow.Add(selfCards[i]);
                            break;
                        }
                    }

                    if (cardToShow.Count == 1){
                        break;
                    }
                }
                if (cardToShow.Count != 1){
                    game.doNothing();
                    GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulSkip(this.ip);
                    SingleBackEnd.NextN = true;
                } else{
                    CardGroup group1 = new CardGroup(1,CardGroup.SINGLE , this.ip);
                    cardToShow.Sort();
                    group1.cards = cardToShow;
                    string json = JsonConvert.SerializeObject(group1);
                    GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulShow(json);
                    for (int i = 0; i < cardToShow.Count; i++){
                        selfCards.Remove(cardToShow[i]);
                    }

                    game.numOfDoNothing = 0;
                    game.currentRule = game.rulesMap[CardGroup.SINGLE];
                    game.currentGroup = group1;
                    game.changeCurrentPlayer();
                    SingleBackEnd.NextN = true;
                }
            }else if (type.Equals(CardGroup.PAIR)){
                int _point = -1;
                for (int i = selfCards.Count - 1; i >= 0 ; i--){
                    if (!selfCards[i].greaterThan(group.cards[0])){
                        continue;
                    }
                    
                    _point = selfCards[i].point;
                    for (int j = i - 1; j >= 1 ; j--){
                        if (selfCards[j].point == _point){
                            if (selfCards[j].point != selfCards[j - 1].point){
                                cardToShow.Add(selfCards[i]);
                                cardToShow.Add(selfCards[j]);
                                break;
                            }
                            
                        }
                    }

                    if (cardToShow.Count == 2){
                        break;
                    }
                }
                if (cardToShow.Count != 2){
                    game.doNothing();
                    GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulSkip(this.ip);
                    SingleBackEnd.NextN = true;
                } else{
                    CardGroup group1 = new CardGroup(2,CardGroup.PAIR , this.ip);
                    cardToShow.Sort();
                    group1.cards = cardToShow;
                    string json = JsonConvert.SerializeObject(group1);
                    GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulShow(json);
                    for (int i = 0; i < cardToShow.Count; i++){
                        selfCards.Remove(cardToShow[i]);
                    }

                    game.numOfDoNothing = 0;
                    game.currentRule = game.rulesMap[CardGroup.PAIR];
                    game.currentGroup = group1;
                    game.changeCurrentPlayer();
                    SingleBackEnd.NextN = true;
                }
            }else if (type.Equals(CardGroup.THREE)){
                int _point = -1;
                for (int i = selfCards.Count - 1; i >= 0 ; i--){
                    if (!selfCards[i].greaterThan(group.cards[0])){
                        continue;
                    }
                    
                    _point = selfCards[i].point;
                    int count = 0;
                    for (int j = i - 1; j >= 1 ; j--){
                        if (selfCards[j].point == _point){
                            count++;
                        } else{
                            break;
                        }

                        if (count == 2){
                            if (selfCards[j].point != selfCards[j - 1].point){
                                cardToShow.Add(selfCards[i]);
                                cardToShow.Add(selfCards[j]);
                                cardToShow.Add(selfCards[j-1]);
                                break;
                            }
                        }
                    }

                    if (cardToShow.Count == 3){
                        break;
                    }
                }
                if (cardToShow.Count != 3){
                    game.doNothing();
                    GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulSkip(this.ip);
                    SingleBackEnd.NextN = true;
                } else{
                    CardGroup group1 = new CardGroup(3,CardGroup.THREE , this.ip);
                    cardToShow.Sort();
                    group1.cards = cardToShow;
                    string json = JsonConvert.SerializeObject(group1);
                    GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulShow(json);
                    for (int i = 0; i < cardToShow.Count; i++){
                        selfCards.Remove(cardToShow[i]);
                    }

                    game.numOfDoNothing = 0;
                    game.currentRule = game.rulesMap[CardGroup.THREE];
                    game.currentGroup = group1;
                    game.changeCurrentPlayer();
                    SingleBackEnd.NextN = true;
                }
            }else if (type.Equals(CardGroup.FOUR)){
                
                int _point = -1;
                for (int i = selfCards.Count - 1; i >= 0 ; i--){
                    if (!selfCards[i].greaterThan(group.cards[0])){
                        continue;
                    }
                    
                    _point = selfCards[i].point;
                    int count = 0;
                    for (int j = i - 1; j >= 1 ; j--){
                        if (selfCards[j].point == _point){
                            count++;
                        } else{
                            break;
                        }

                        if (count == 3){
                            if (selfCards[j].point != selfCards[j - 1].point){
                                cardToShow.Add(selfCards[i]);
                                cardToShow.Add(selfCards[j]);
                                cardToShow.Add(selfCards[j-1]);
                                cardToShow.Add(selfCards[j-2]);
                                break;
                            }
                        }
                    }

                    if (cardToShow.Count == 4){
                        break;
                    }
                }
                if (cardToShow.Count != 4){
                    game.doNothing();
                    GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulSkip(this.ip);
                    SingleBackEnd.NextN = true;
                } else{
                    CardGroup group1 = new CardGroup(4,CardGroup.FOUR , this.ip);
                    cardToShow.Sort();
                    group1.cards = cardToShow;
                    string json = JsonConvert.SerializeObject(group1);
                    GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulShow(json);
                    for (int i = 0; i < cardToShow.Count; i++){
                        selfCards.Remove(cardToShow[i]);
                    }

                    game.numOfDoNothing = 0;
                    game.currentRule = game.rulesMap[CardGroup.FOUR];
                    game.currentGroup = group1;
                    game.changeCurrentPlayer();
                    SingleBackEnd.NextN = true;
                }
            } else{
                game.doNothing();
                GameObject.Find("CoreManager").GetComponent<SingleFrontManager>().SeccessfulSkip(this.ip);
                SingleBackEnd.NextN = true;
            }
            
            
        }
    }
}