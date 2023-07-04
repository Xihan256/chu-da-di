using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using lln.ChuDaDi_MainLogic.player;
using UnityEngine;
using Object = System.Object;
using lln.ChuDaDi_MainLogic;

namespace lln.Network.server{
    public class Main : MonoBehaviour{
        public Socket serverSocket;
        public List<Socket> sockets;
        
        private void Awake(){
            try{
                sockets = new List<Socket>();
                string localIP = GetLocalIPAddress();
                int port = 11096;
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse(localIP), port));

                serverSocket.Listen(8);

                Thread thread = new Thread(loop);
                thread.Start(serverSocket);
            } catch (Exception e){
                Debug.LogError("Failed to start server: " + e.Message);
            } 
        }

        private void OnDestroy()
        {
            serverSocket.Close();
        }

        public void loop(Object o){
            Socket server = (Socket)o;
            
            while (true){
                Socket socket = null;
                try{
                    socket = server.Accept();
                } catch (Exception e){
                    Console.WriteLine("socket wrong");
                    throw;
                }
                sockets.Add(socket);
                
                string guestIP = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
                addPlayer(null , guestIP);//todo 不能是null
                Debug.Log(guestIP + "add ok");
                Thread thread = new Thread(this.runThread);
                thread.Start(socket);
            }
        }

        private void addPlayer(string name, string ip){
            //GameObject.Find("BackEnd").GetComponent<TheBackEnd>().addPlayer(name, ip);
            Player player = new Player(name, ip);
            Game.instance.addPlayer(player);
        }
        
        private string GetLocalIPAddress()
        {
            string ipAddress = string.Empty;

            try
            {
                string hostName = Dns.GetHostName();

                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

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
                Debug.Log("出问题了");
                Debug.LogError("Error getting local IP address: 我自己写的");
            }

            return ipAddress;
        }
        
        public string processInput(Socket clientSocket)
        {
            string rtn = null;
            byte[] buffer = new byte[8192];
            int bytesRead = -1;
            try
            {
                bytesRead = clientSocket.Receive(buffer);
            }
            catch(Exception e)
            {
                Debug.LogError(e.GetType() + "错误");
            }
            

            // 处理接收到的数据
            rtn = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            return rtn;
        }

        private void processOutput(string outstr,Socket socket){
            byte[] bytes = Encoding.UTF8.GetBytes(outstr);
            socket.Send(bytes);
        }
        
        public void runThread(Object o){

            Socket socket = (Socket)o;
            try {
                String send = null;
                String receive = null;

                while(true){
                    if(socket.Connected){
                        try
                        {
                            receive = processInput(socket);
                        }
                        catch(Exception e)
                        {
                            Debug.LogError("Error getting local IP address:input " + e.Message);
                        }


                        if(receive.StartsWith("i")){
                            receive = receive.Substring(1);
                            for(int i = 0; i < sockets.Count ; i++){
                                processOutput("i" + receive, sockets[i]);
                            }
                        }else if (receive.StartsWith("s")){
                            receive = receive.Substring(1);
                            for (int i = 0; i < sockets.Count; i++)
                            {
                                IPAddress address = ((IPEndPoint)sockets[i].RemoteEndPoint).Address;
                                try
                                {
                                    if (address.ToString().Equals(GetLocalIPAddress()))//这是主机的ip
                                    {
                                        processOutput("s" + receive, sockets[i]);
                                        break;
                                    }
                                }catch(Exception e)
                                {
                                    Debug.LogError("Error getting local IP address:1 " + e.Message);
                                }
                            }
                        }else if (receive.StartsWith("o"))
                        {
                            // receive = receive.Substring(1);
                            // FinishPlayer player = Newtonsoft.Json.JsonConvert.DeserializeObject<FinishPlayer>(receive);

                            for (int i = 0; i < sockets.Count; i++)
                            {
                                processOutput(receive , sockets[i]);
                            }
                        }
                        else if (receive.StartsWith("w"))
                        {
                            receive = receive.Substring(1);

                            for (int i = 0; i < sockets.Count; i++)
                            {
                                IPAddress address = ((IPEndPoint)sockets[i].RemoteEndPoint).Address;
                                if (address.ToString().Equals(receive))
                                {
                                    processOutput("go", sockets[i]);
                                    break;
                                }
                            }
                        }else if (receive.StartsWith("q")){
                            string substr = null;
                            if (receive.Contains('w')){
                                int index = receive.IndexOf('w');
                                substr = receive.Substring(index);
                                receive = receive.Substring(0, index);

                            }
                            for (int i = 0; i < sockets.Count; i++)
                            {
                                processOutput(receive, sockets[i]);
                            }
                            
                            if (substr != null){
                                substr = substr.Substring(1);
                                for (int i = 0; i < sockets.Count; i++)
                                {
                                    IPAddress address = ((IPEndPoint)sockets[i].RemoteEndPoint).Address;
                                    if (address.ToString().Equals(substr))
                                    {
                                        Thread.Sleep(1000);
                                        processOutput("go", sockets[i]);
                                        break;
                                    }
                                }
                            }
                        }else if (receive.StartsWith("r")){
                            receive = receive.Substring(1);
                            
                            int index = receive.IndexOf("$");

                            string name = receive.Substring(0, index);
                            string ip = receive.Substring(index + 1);
            
                            Game.instance.setPlayerName(name , ip);

                        }
                        else if (receive.Equals("no"))
                        {
                            for (int i = 0; i < sockets.Count; i++)
                            {
                                IPAddress address = ((IPEndPoint)sockets[i].RemoteEndPoint).Address;
                                try
                                {
                                    if (address.ToString().Equals(GetLocalIPAddress()))//这是主机的ip
                                    {
                                        processOutput("no", sockets[i]);
                                        break;
                                    }
                                }
                                catch(Exception e)
                                {
                                    Debug.LogError("Error getting local IP address:2 " + e.Message);
                                }
                                
                            }
                        }
                        else if (receive.Equals("finish")){
                            for (int i = 0; i < sockets.Count; i++)
                            {
                                IPAddress address = ((IPEndPoint)sockets[i].RemoteEndPoint).Address;
                                try
                                {
                                    if (address.ToString().Equals(GetLocalIPAddress()))//这是主机的ip
                                    {
                                        processOutput(receive, sockets[i]);
                                        break;
                                    }
                                }
                                catch(Exception e)
                                {
                                    Debug.LogError("Error getting local IP address:3 " + e.Message);
                                }
                                
                            }
                        }
                        else if (receive.StartsWith("err"))
                        {
                            receive = receive.Substring(3);
                            for (int i = 0; i < sockets.Count; i++)
                            {
                                IPAddress address = ((IPEndPoint)sockets[i].RemoteEndPoint).Address;
                                if (address.ToString().Equals(receive))//这是有错那个人的ip
                                {
                                    processOutput("error", sockets[i]);
                                    break;
                                }
                            }
                        }
                        else{
                            string substr = null;
                            if (receive.Contains('w')){
                                int index = receive.IndexOf('w');
                                substr = receive.Substring(index);
                                receive = receive.Substring(0, index);

                            }
                            for (int i = 0; i < sockets.Count; i++)
                            {
                                processOutput(receive, sockets[i]);
                            }

                            
                            if (substr != null){
                                substr = substr.Substring(1);
                                for (int i = 0; i < sockets.Count; i++)
                                {
                                    IPAddress address = ((IPEndPoint)sockets[i].RemoteEndPoint).Address;
                                    if (address.ToString().Equals(substr))
                                    {
                                        Thread.Sleep(1000);
                                        processOutput("go", sockets[i]);
                                        break;
                                    }
                                }
                            }
                        }


                    }else{
                        break;
                    }
                }
            } catch (Exception e) {
                Debug.LogError("Error getting local IP address:11111222222222 " + e.GetType());
            }
        }
    }
}