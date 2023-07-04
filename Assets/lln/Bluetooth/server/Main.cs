using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using lln.Bluetooth.bluetoothDriverWrapper;
using lln.ChuDaDi_MainLogic;
using lln.ChuDaDi_MainLogic.cardLogic;
using lln.ChuDaDi_MainLogic.player;
using Newtonsoft.Json;
using UnityEngine;

namespace lln.Bluetooth.server{
    public class Main :　MonoBehaviour{
        private AndroidJavaObject pluginObj;
        public static AndroidJavaObject pluginObjNoGC;
        public static string selfname;
        public GameObject front;
        public GameObject back;
        private OtherPlayerControl[] others;
        public Wrapper wrapper;
        private void Awake(){
            others = new OtherPlayerControl[3];
            wrapper = GameObject.Find("BTDriver").GetComponent<Wrapper>();
            pluginObj = pluginObjNoGC;

            wrapper.pluginObj = pluginObjNoGC;
            
            Debug.LogWarning("wrapper的pluginObj空吗?" + (wrapper.pluginObj == null));
            if (FrontManager.isHost){
                wrapper.doServer();
            }
        }

        public void otherPlayerInit(){
            others[0] = front.GetComponent<FrontManagerBT>().right.GetComponent<OtherPlayerControl>();
            others[1] = front.GetComponent<FrontManagerBT>().up.GetComponent<OtherPlayerControl>();
            others[2] = front.GetComponent<FrontManagerBT>().left.GetComponent<OtherPlayerControl>();
        }
        
        private void Update(){
            bool isUpdated = pluginObj.Get<bool>("isMsgUpdated");

            if (isUpdated){
                pluginObj.Set("isMsgUpdated" , false);
                string receive = pluginObj.Get<string>("currentMsg");
                processMsg(receive);
            }
        }

        public void processMsg(string receive){
            Debug.Log(receive);
            bool isHost = FrontManager.isHost;
                if (receive.StartsWith("i")){
                    receive = receive.Substring(1);
                    Player[] players = JsonConvert.DeserializeObject<Player []>(receive);
        
                    int myIndex = -1;
                    for (int i = 0; i < players.Length; i++){
                        if (players[i].ip == selfname){
                            myIndex = i;
                            List<Card> cards = players[i].cards.cardsOfPlayer;
                            string ss = JsonConvert.SerializeObject(cards);
                            
                            front.GetComponent<FrontManagerBT>().initJson = ss;
                            front.GetComponent<FrontManagerBT>().initN = true;
                        }
                    }
                    
                    others[0].GetName = players[(myIndex + 1) % 4].name;
                    others[0].GetIP = players[(myIndex + 1) % 4].ip;
                    
                    others[1].GetName = players[(myIndex + 2) % 4].name;
                    others[1].GetIP = players[(myIndex + 2) % 4].ip;
                    
                    others[2].GetName = players[(myIndex + 3) % 4].name;
                    others[2].GetIP = players[(myIndex + 3) % 4].ip;
        
        
                }else if (isHost && receive.StartsWith("s"))
                {
                    TheBackEndBT backEnd = back.GetComponent<TheBackEndBT>();
                    string recv = backEnd.showCard(receive.Substring(1));
                    if (recv.Equals("{}"))
                    {
                        Debug.LogError("出现了非常严重的错误，但是无法解决，不行重开吧");
                    }else if (recv.StartsWith("a"))
                    {
                        recv = recv.Substring(1);
                        if (recv.Equals(selfname)){
                            processMsg("err"+recv);
                        } else{
                            wrapper.hostSendMsg("err" + recv);
                        }
                    }
                    else
                    {
                        wrapper.hostSendMsg(recv);
                        processMsg(recv);
                        
                        wrapper.hostSendMsg("w" + backEnd.getCurrIp());
                        processMsg("w" + backEnd.getCurrIp());
                    }
                }else if (receive.StartsWith("q")){
                    string substr = null;
                    if (receive.Contains('g')){
                        int index = receive.IndexOf('g');
                        substr = receive.Substring(index);
                        receive = receive.Substring(0, index);
        
                    }
                    receive = receive.Substring(1);
                    
                    //front.SeccessfulSkip(receive);
                    front.GetComponent<FrontManagerBT>().skipIp = receive;
                    front.GetComponent<FrontManagerBT>().skipN = true;
                    
                    if (substr != null){
                        Thread.Sleep(200);
                        front.GetComponent<FrontManagerBT>().roundN = true;
                    }
        
                }else if (isHost && receive.StartsWith("r")){
                    
                    receive = receive.Substring(1);
                    back.GetComponent<TheBackEndBT>().addplayerName(receive);
                }
                else if (receive.StartsWith("o"))
                {
                    receive = receive.Substring(1);
                    front.GetComponent<FrontManagerBT>().finishJson = receive;
                    //todo 前端展示分
                    front.GetComponent<FrontManagerBT>().finishN = true;
                }else if (receive.StartsWith("w")){
                    receive=receive.Substring(1);
                    if (receive == selfname){
                        front.GetComponent<FrontManagerBT>().roundN = true;
                    }
                }
                else if (isHost && receive.Equals("no"))
                {
                    TheBackEndBT backEnd = back.GetComponent<TheBackEndBT>();
                    string recv = backEnd.doNothing();
                    if (recv.StartsWith("a"))
                    {
                        recv = recv.Substring(1);
                        if (recv.Equals(selfname)){
                            processMsg("err"+recv);
                        } else{
                            wrapper.hostSendMsg("err" + recv);
                        }
                    }
                    else
                    {
                        wrapper.hostSendMsg("q"+recv);
                        processMsg("q"+recv);
                        
                        wrapper.hostSendMsg("w"+backEnd.getCurrIp());
                        processMsg("w"+backEnd.getCurrIp());
                    }
                }
                else if (receive.Equals("error"))
                {
                    //todo 处理错误
                    front.GetComponent<FrontManagerBT>().roundN = true;
                    //front.UpRound();
                }
                else if (isHost && receive.Equals("finish"))
                {
                    string rtn = back.GetComponent<TheBackEndBT>().gameFinish();
                    
                    wrapper.hostSendMsg('o' + rtn);
                    processMsg('o' + rtn);
                }
                else
                {
                    if (!receive.StartsWith('{')){
                        return;
                    }
                    string substr = null;
                    if (receive.Contains('g')){
                        int index = receive.IndexOf('g');
                        substr = receive.Substring(index);
                        receive = receive.Substring(0, index);
        
                    }
                    
                    //前端调用，渲染牌 receive，处理手牌
                    front.GetComponent<FrontManagerBT>().showJson = receive;
                    front.GetComponent<FrontManagerBT>().showN = true;
                    
                    if (substr != null){
                        Thread.Sleep(200);
                        front.GetComponent<FrontManagerBT>().roundN = true;
                    }
        
                }
        }
        
        public void connectToServer(){
            
            wrapper.doClient();
            Thread.Sleep(3000);
            wrapper.sendMsg("r" + selfname);
        }
        

    }
}