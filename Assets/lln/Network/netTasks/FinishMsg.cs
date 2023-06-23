using lln.ChuDaDi_MainLogic.Utils;
using lln.Network.client;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class FinishMsg : MonoBehaviour
{
    public Socket socket;

    public void WorkToDo(string json)
    {
        socket = GameObject.Find("Client").GetComponent<ClientMain>().socket;
        FinishPlayer[] players = Newtonsoft.Json.JsonConvert.DeserializeObject<FinishPlayer[]>(json);

        for(int i = 0; i <players.Length; i++)
        {
            string js = Newtonsoft.Json.JsonConvert.SerializeObject(players[i]);
            processOutput('o' + js, socket);
        }
    }

    private void processOutput(string outstr, Socket socket)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(outstr);
        socket.Send(bytes);
    }
}
