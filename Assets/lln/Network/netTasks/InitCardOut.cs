using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using lln.ChuDaDi_MainLogic.player;
using lln.Network.client;
using UnityEngine;
using Object = System.Object;

namespace lln.Network.netTasks{
    public class InitCardOut : MonoBehaviour{
        public Socket socket;

        public void WorkToDo(string players){
            socket = GameObject.Find("Client").GetComponent<ClientMain>().socket;
            processOutput('i' + players,socket);
            
            Debug.Log("初始牌发送完成了");
            Debug.Log(socket.ToString());
        }

        

        private void processOutput(string outstr,Socket socket){
            byte[] bytes = Encoding.UTF8.GetBytes(outstr);
            socket.Send(bytes);
        }
    }
}