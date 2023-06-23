

using System;
using UnityEngine;

namespace lln.ChuDaDi_MainLogic.cardLogic{
    
    public class Card : IComparable<Card> {
        public int suit{ get; set; }
        public int point{ get; set; }

        public Card(int suit, int point){
            this.suit = suit;
            this.point = point;
        }

        public int CompareTo(Card other){
            if (this.point != other.point) {
                if (this.point == 2) {
                    return -1;
                } else if (other.point == 2) {
                    return 1;
                } else if (this.point == 1) {
                    return -1;
                } else if (other.point == 1) {
                    return 1;
                } else {
                    return (this.point > other.point ? -1 : 1);
                }
            } else {
                return (this.suit > other.suit ? -1 : 1);
            }
        }

        public override string ToString(){
            return suit + " " + point;
        }


        public bool greaterThan(Card currentGroupCard){
            int cmp = this.CompareTo(currentGroupCard) * -1;
            return cmp > 0;
        }

        public override bool Equals(object obj){
            Card c = (Card)obj;

            return this.point == c.point && this.suit == c.suit;
        }
    }
}