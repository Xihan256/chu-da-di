using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class HostIPTaker : MonoBehaviour
{
    public GameObject smaker;
    public string ip;

    private void Start()
    {
        ip = "127.0.0.1";
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
            Debug.LogError("Error getting local IP address: 1" + ex.Message);
        }

        return ipAddress;
    }
}
