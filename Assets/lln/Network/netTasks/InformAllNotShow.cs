using System.Net.Sockets;
using System.Text;
using lln.Network.client;
using UnityEngine;

namespace lln.Network.netTasks{
    public class InformAllNotShow : MonoBehaviour{
        public Socket socket;

        public void WorkToDo(string ip)
        {
            socket = GameObject.Find("Client").GetComponent<ClientMain>().socket;
            processOutput("q" + ip, socket);

        }

        private void processOutput(string outstr, Socket socket)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(outstr);
            socket.Send(bytes);
        }
    }
}