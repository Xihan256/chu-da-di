using lln.Network.client;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chosing : MonoBehaviour
{
    public GameObject Main;
    public GameObject Host;
    public GameObject Guest;

    public void ChangeToHost()
    {
        Debug.Log("我是主机");
        Main.SetActive(false);
        Host.SetActive(true);
        HostToDo();
    }

    public void ChangeToGuest()
    {
        Main.SetActive(false);
        Guest.SetActive(true);
    }

    public void HostToDo()
    {
        Transform IPtxt = Host.transform.GetChild(1);
        IPtxt.gameObject.GetComponent<Text>().text = "IP:"+GetLocalIPAddress();
        ClientMain.ip = GetLocalIPAddress();
        FrontManager.isHost = true;
    }

    public void HostGetNext()
    {
        FrontManager.isHost = true;
        
        SceneManager.LoadScene("003_Mutiplayer");
    }

    public void GuestGetNext()
    {
        Transform IPtxt = Guest.transform.GetChild(1).GetChild(1);
        ClientMain.ip = IPtxt.gameObject.GetComponent<Text>().text;
        FrontManager.isHost = false;
        
        SceneManager.LoadScene("003_Mutiplayer");
    }

    private string GetLocalIPAddress()
    {
        string ipAddress = string.Empty;

        try
        {
            // ��ȡ����������
            string hostName = Dns.GetHostName();

            // ������������ȡ������Ϣ
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

            // ����������Ϣ�е�IP��ַ���ҵ�������IP��ַ
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
            Debug.LogError("Error getting local IP address:���Լ�д��2 ");
        }

        return ipAddress;
    }
}
