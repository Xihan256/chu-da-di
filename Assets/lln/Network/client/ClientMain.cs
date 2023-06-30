using lln.ChuDaDi_MainLogic;
using lln.ChuDaDi_MainLogic.Utils;
using lln.Network.netTasks;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using lln.ChuDaDi_MainLogic.cardLogic;
using lln.ChuDaDi_MainLogic.player;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using Object = System.Object;

namespace lln.Network.client{
    public class ClientMain : MonoBehaviour{
        public Socket socket;
        public static string ip;
        public static string selfname;
        public int port = 11096;
        public FrontManager front;
        public TheBackEnd back;
        public OtherPlayerControl[] others;
        private void Start(){
            Debug.Log(ip);
        }

        private void addName(Socket s){
            
            byte[] bytes = Encoding.UTF8.GetBytes('r' +  selfname + "$" + GetLocalIPAddress());
            socket.Send(bytes);
        }

        public void clientInit(){

            others = new OtherPlayerControl[3];
            front = GameObject.Find("CoreManager").GetComponent<FrontManager>();
            back = GameObject.Find("BackEnd").GetComponent<TheBackEnd>();

            others[0] = front.right.GetComponent<OtherPlayerControl>();
            others[1] = front.up.GetComponent<OtherPlayerControl>();
            others[2] = front.left.GetComponent<OtherPlayerControl>();

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(IPAddress.Parse(ip), port);

            
            Thread thread = new Thread(runAccept);
            thread.Start(socket);
        }

        
        public void runAccept(Object o){
            Socket s = (Socket)o;
            Thread.Sleep(200);
            addName(s);
            while(true){
                String receive = processInput(s);
                Debug.Log(receive);
                if (receive.StartsWith("i")){
                    receive = receive.Substring(1);
                    Player[] players = JsonConvert.DeserializeObject<Player []>(receive);

                    int myIndex = -1;
                    for (int i = 0; i < players.Length; i++){
                        if (players[i].ip == GetLocalIPAddress()){
                            myIndex = i;
                            List<Card> cards = players[i].cards.cardsOfPlayer;
                            //cards.Sort();
                            string ss = JsonConvert.SerializeObject(cards);
                            
                            //front.CardInit(ss);
                            front.initJson = ss;
                            front.initN = true;
                        }
                    }
                    
                    others[0].GetName = players[(myIndex + 1) % 4].name;
                    others[0].GetIP = players[(myIndex + 1) % 4].ip;
                    
                    others[1].GetName = players[(myIndex + 2) % 4].name;
                    others[1].GetIP = players[(myIndex + 2) % 4].ip;
                    
                    others[2].GetName = players[(myIndex + 3) % 4].name;
                    others[2].GetIP = players[(myIndex + 3) % 4].ip;


                }else if (receive.StartsWith("s"))
                {
                    TheBackEnd backEnd = back;
                    string recv = backEnd.showCard(receive.Substring(1));
                    if (recv.Equals("{}"))
                    {
                        Debug.LogError("出现了非常严重的错误，但是无法解决，不行重开吧");
                    }else if (recv.StartsWith("a"))
                    {
                        recv = recv.Substring(1);
                        byte[] bytes = Encoding.UTF8.GetBytes("err" + recv);
                        socket.Send(bytes);
                        
                    }
                    else
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(recv);
                        socket.Send(bytes);
                        
                        byte[] bytes1 = Encoding.UTF8.GetBytes("w" + backEnd.getCurrIp());
                        socket.Send(bytes1);
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
                    front.skipIp = receive;
                    front.skipN = true;
                    
                    if (substr != null){
                        Thread.Sleep(200);
                        front.roundN = true;
                    }

                }else if (receive.StartsWith("r")){
                    receive = receive.Substring(1);
                    //back.addplayerName(receive);
                }
                else if (receive.StartsWith("o"))
                {
                    receive = receive.Substring(1);
                    front.finishJson = receive;
                    //todo 前端展示分
                    front.finishN = true;
                }else if (receive.Equals("go")){
                    front.roundN = true;
                    //front.UpRound();
                }
                else if (receive.Equals("no"))
                {
                    TheBackEnd backEnd = back;
                    string recv = backEnd.doNothing();
                    if (recv.StartsWith("a"))
                    {
                        recv = recv.Substring(1);
                        byte[] bytes = Encoding.UTF8.GetBytes("err" + recv);
                        socket.Send(bytes);
                    }
                    else
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes("q" + recv);
                        socket.Send(bytes);
                        int i = 0;
                        //todo
                        // while (i < 100000000){
                        //     i++;
                        // }
                        byte[] bytes1 = Encoding.UTF8.GetBytes("w" + backEnd.getCurrIp());
                        socket.Send(bytes1);
                    }
                }
                else if (receive.Equals("error"))
                {
                    //todo 处理错误
                    front.roundN = true;
                    //front.UpRound();
                }
                else if (receive.Equals("finish"))
                {
                    string rtn = back.gameFinish();
                    byte[] bytes = Encoding.UTF8.GetBytes('o' + rtn);
                    socket.Send(bytes);
                    
                }
                else
                {
                    string substr = null;
                    if (receive.Contains('g')){
                        int index = receive.IndexOf('g');
                        substr = receive.Substring(index);
                        receive = receive.Substring(0, index);

                    }
                    
                    //前端调用，渲染牌 receive，处理手牌
                    front.showJson = receive;
                    front.showN = true;
                    //front.SeccessfulShow(receive);
                    
                    if (substr != null){
                        Thread.Sleep(200);
                        front.roundN = true;
                    }

                }

            }
        }

        private string processInput(Socket clientSocket){
            string rtn = null;
            byte[] buffer = new byte[8192];
            int bytesRead = clientSocket.Receive(buffer);

            // 处理接收到的数据
            rtn = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            return rtn;
        }

        private void processOutput(string outstr,Socket socket){
            byte[] bytes = Encoding.UTF8.GetBytes(outstr);
            socket.Send(bytes);
        }
        
        private string GetLocalIPAddress()
        {
            string ipAddress = string.Empty;

            try
            {
                // 获取本机主机名
                string hostName = Dns.GetHostName();

                // 根据主机名获取主机信息
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

                // 遍历主机信息中的IP地址，找到本机的IP地址
                foreach (IPAddress address in hostEntry.AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = address.ToString();
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error getting local IP address: " + ex.Message);
            }

            return ipAddress;
        }
    }
}