using lln.Bluetooth.BTTask;
using lln.Bluetooth.server;
using lln.ChuDaDi_MainLogic.player;
using lln.ChuDaDi_MainLogic.Utils;
using lln.Network;
using lln.Network.netTasks;
using Newtonsoft.Json;
using UnityEngine;

namespace lln.ChuDaDi_MainLogic{
    public class TheBackEndBT : MonoBehaviour{
        private Game game;
        public GameObject initSender;
        public GameObject roundSender;

        private void Start()
        {
            game = new Game();
            Game.instance = game;
        }

        

        public string getCurrIp()
        {
            return game.getCurrIp();
        }

        public void startGame()
        {
            game.addPlayer(new Player(Main.selfname,Main.selfname));
            game.wakeUp();
            string res = game.giveMsgToFrontEnd();
            GameObject initObj = Instantiate(initSender);
            initObj.GetComponent<BTInitCardOut>().WorkToDo(res);
            GameObject.Destroy(initObj, 2.0f);
            GameObject.Find("Main").GetComponent<Main>().processMsg('i'+res);

            string currIp = game.getCurrIp();
            GameObject roundObj = Instantiate(roundSender);
            roundObj.GetComponent<BTInformToShow>().WorkToDo(currIp);
            GameObject.Destroy(roundObj, 2.0f);
            GameObject.Find("Main").GetComponent<Main>().processMsg('w'+currIp);
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

        public string doNothing()
        {
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

        public string gameFinish()
        {
            FinishPlayer[] players = game.calculateScore();

            string serializeObject = JsonConvert.SerializeObject(players);
            TheNetwork.scoreToDatabase(serializeObject);//todo 要改的

            return serializeObject;
        }

        public void addplayerName(string receive){
            game.addPlayer(new Player(receive,receive));
        }
    }
}