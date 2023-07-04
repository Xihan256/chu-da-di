using System;
using System.Threading.Tasks;
using lln.Bluetooth.BTTask;
using lln.ChuDaDi_MainLogic.player;
using lln.ChuDaDi_MainLogic.Utils;
using lln.Network;
using lln.Network.netTasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace lln.ChuDaDi_MainLogic
{
    public class TheBackEnd : MonoBehaviour
    {
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
            game.wakeUp();
            string res = game.giveMsgToFrontEnd();
            GameObject initObj = Instantiate(initSender);
            initObj.GetComponent<InitCardOut>().WorkToDo(res);
            GameObject.Destroy(initObj, 2.0f);
            
            string currIp = game.getCurrIp();
            GameObject roundObj = Instantiate(roundSender);
            roundObj.GetComponent<InformToShow>().WorkToDo(currIp);
            GameObject.Destroy(roundObj, 2.0f);
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


        public void addPlayer(string name, string ip)
        {
            Player player = new Player(name, ip);
            game.addPlayer(player);
        }
    }
}