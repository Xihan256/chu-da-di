using lln.Network.client;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class SimpleShowMsg : MonoBehaviour
{
    public Socket socket;

    public void workToDo(string json)
    {
        
        socket = GameObject.Find("Client").GetComponent<ClientMain>().socket;

        processOutput(json, socket);
    }

    private void processOutput(string outstr, Socket socket)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(outstr);
        socket.Send(bytes);
    }
}
