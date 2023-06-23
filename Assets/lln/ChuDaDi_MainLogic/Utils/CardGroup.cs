using System.Collections.Generic;
using lln.ChuDaDi_MainLogic.cardLogic;

namespace lln.ChuDaDi_MainLogic.Utils
{
    public class CardGroup{
        public static string EVERYTHING = "EVERYTHING";
        public static string SINGLE = "SINGLE";
        public static string PAIR = "PAIR";
        public static string THREE = "THREE";
        public static string FOUR = "FOUR";
        public static string SHUNZI = "SHUN_ZI";
        public static string TONGHUA = "TONG_HUA";
        public static string THREE_WITH_PAIR = "3WITH2";
        public static string FOUR_WITH_SINGLE = "4WITH1";
        public static string TONGHUASHUN = "TONG_HUA_SHUN";
        
        public List<Card> cards{ get; set; }
        public int size{ get; set; }
        public string type{ get; set; }
        
        public string ip{ get; set; }

        public CardGroup(int size, string type , string ip){
            this.size = size;
            this.type = type;
            this.ip = ip;
            cards = new List<Card>();
        }
    }
}