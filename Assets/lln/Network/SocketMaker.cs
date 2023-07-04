using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace lln.Network{
    public class SocketMaker : MonoBehaviour{
        public Socket socket;
        public GameObject obj;
        
        private void Start(){

        }

        public void Init()
        {
            int port = 11096;
            string ip = obj.GetComponent<IPTaker>().ip;
            Debug.Log(ip);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(IPAddress.Parse(ip), port);
        }
    }
}