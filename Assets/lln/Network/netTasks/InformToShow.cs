using lln.Network.client;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace lln.Network.netTasks
{
    public class InformToShow:MonoBehaviour
    {
        public Socket socket;

        public void WorkToDo(string ip)
        {
            //根据ip通知那个人轮到他出牌
            socket = GameObject.Find("Client").GetComponent<ClientMain>().socket;
            processOutput("w" + ip, socket);
            
            Debug.Log("发给ip " + ip + "回合到他了成功");
        }

        private void processOutput(string outstr, Socket socket)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(outstr);
            socket.Send(bytes);
        }
    }
}
