using lln.ChuDaDi_MainLogic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Object = System.Object;

namespace Assets.lln.Network.client
{
    public class Client_client : MonoBehaviour
    {
        public Socket socket;
        public string ip;
        public string localIP;

        public GameObject simpleShowPrefab;
        public GameObject errSender;
        public GameObject finSender;

        private void Start()
        {
            localIP = GetLocalIPAddress();
            //ip?
            int port = 11096;
            Debug.Log(ip);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(IPAddress.Parse(ip), port);

            Thread thread = new Thread(runAccept);
            thread.Start(socket);
        }

        public void runAccept(Object o)
        {
            Socket s = (Socket)o;

            while (true)
            {
                String receive = processInput(s);
                Debug.Log(receive);
                //todo 更改收到消息时的行动
                if (receive.StartsWith("i"))
                {
                    receive = receive.Substring(1);
                    GameObject.Find("CoreManager").GetComponent<FrontManager>().CardInit(receive);
                    //todo 前端写逻辑
                }
                else if (receive.StartsWith("s"))
                {
                    TheBackEnd backEnd = GameObject.Find("BackEnd").GetComponent<TheBackEnd>();
                    string recv = backEnd.showCard(receive.Substring(1));
                    if (recv.Equals("{}"))
                    {
                        //报错
                    }
                    else if (recv.StartsWith("a"))
                    {
                        recv = recv.Substring(1);
                        GameObject showObj = Instantiate(errSender);
                        showObj.GetComponent<InformError>().WorkToDo(recv);
                        GameObject.Destroy(showObj, 2.0f);
                    }
                    else
                    {
                        GameObject showObj = Instantiate(simpleShowPrefab);
                        showObj.GetComponent<SimpleShowMsg>().workToDo(recv);
                        GameObject.Destroy(showObj, 2.0f);
                    }
                }
                else if (receive.StartsWith("o"))
                {
                    receive = receive.Substring(1);
                    //前端展示分
                }
                else if (receive.Equals("error"))
                {
                    //发给那个人有错误的信息
                }
                else if (receive.Equals("finish"))
                {
                    string rtn = GameObject.Find("BackEnd").GetComponent<TheBackEnd>().gameFinish();
                    //FinishPlayer[] players = Newtonsoft.Json.JsonConvert.DeserializeObject<FinishPlayer[]>(rtn);
                    GameObject showObj = Instantiate(finSender);
                    showObj.GetComponent<FinishMsg>().WorkToDo(rtn);
                    GameObject.Destroy(showObj, 2.0f);
                }
                else
                {
                    //前端调用，渲染牌 receive
                }

            }
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

        private string processInput(Socket clientSocket)
        {
            string rtn = null;
            byte[] buffer = new byte[1024];
            int bytesRead = clientSocket.Receive(buffer);

            // 处理接收到的数据
            rtn = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            return rtn;
        }

        private void processOutput(string outstr, Socket socket)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(outstr);
            socket.Send(bytes);
        }
    }
}

