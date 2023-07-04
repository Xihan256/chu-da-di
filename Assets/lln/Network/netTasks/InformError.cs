using lln.Network.client;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class InformError : MonoBehaviour
{
    public Socket socket;

    public void WorkToDo(string ip)
    {
        socket = GameObject.Find("Client").GetComponent<ClientMain>().socket;
        processOutput("err" + ip, socket);

    }

    private void processOutput(string outstr, Socket socket)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(outstr);
        socket.Send(bytes);
    }
}
