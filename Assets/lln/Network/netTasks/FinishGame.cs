using lln.Network.client;
using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace lln.Network.netTasks{
    public class FinishGame : MonoBehaviour{
        public Socket socket;

        public void WorkToDo(){
            socket = GameObject.Find("Client").GetComponent<ClientMain>().socket;
            processOutput("finish",socket);
            
        }
        
        private void processOutput(string outstr,Socket socket){
            byte[] bytes = Encoding.UTF8.GetBytes(outstr);
            socket.Send(bytes);
        }
    }
}