using lln.Network.client;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using lln.Bluetooth.bluetoothDriverWrapper;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Chosing : MonoBehaviour{
    public GameObject Main;
    public GameObject MainNET;
    public GameObject HostNET;
    public GameObject GuestNET;
    public GameObject MainBT;

    private GameObject driver;

    public GameObject BTdriver;

    public void ChangeToNet(){
        Main.SetActive(false);
        MainNET.SetActive(true);
    }

    public void ChangeToBT(){
        driver = Instantiate(BTdriver);
        driver.name = "BTDriver";

        Main.SetActive(false);
        MainBT.SetActive(true);
        
        driver.GetComponent<Wrapper>().makeBluetoothDiscoverable();
    }
    
    public void ChangeToHost()
    {
        Debug.Log("我是主机");
        MainNET.SetActive(false);
        HostNET.SetActive(true);
        HostToDo();
    }

    public void ChangeToGuest()
    {
        MainNET.SetActive(false);
        GuestNET.SetActive(true);
    }

    public void HostToDo()
    {
        Transform IPtxt = HostNET.transform.GetChild(1);
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
        Transform IPtxt = GuestNET.transform.GetChild(1).GetChild(1);
        ClientMain.ip = IPtxt.gameObject.GetComponent<Text>().text;
        FrontManager.isHost = false;
        
        SceneManager.LoadScene("003_Mutiplayer");
    }
    
    public void HostGetNextBT()
    {
        FrontManager.isHost = true;
        FrontManagerBT.isHost = true;
        driver.GetComponent<Wrapper>().startScanViable();
        lln.Bluetooth.server.Main.pluginObjNoGC = driver.GetComponent<Wrapper>().pluginObj;
        SceneManager.LoadScene("009_BTtest");
    }
    
    public void GuestGetNextBT()
    {
        FrontManager.isHost = false;
        FrontManagerBT.isHost = false;
        driver.GetComponent<Wrapper>().seekForBinded();
        lln.Bluetooth.server.Main.pluginObjNoGC = driver.GetComponent<Wrapper>().pluginObj;
        SceneManager.LoadScene("009_BTtest");
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
