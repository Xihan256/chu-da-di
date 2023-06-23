using lln.Network.client;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace lln.Network.netTasks{
    public class ShowCards : MonoBehaviour{
        public string cardGroup;
        public Socket socket;

        public void workToDo(string json)
        {
            cardGroup = json;
            socket = GameObject.Find("Client").GetComponent<ClientMain>().socket;

            processOutput('s' + cardGroup, socket);
        }

        private void processOutput(string outstr,Socket socket){
            byte[] bytes = Encoding.UTF8.GetBytes(outstr);
            socket.Send(bytes);
        }
    }
}