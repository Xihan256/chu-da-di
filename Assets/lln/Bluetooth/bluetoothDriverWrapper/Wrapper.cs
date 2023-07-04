using System;
using UnityEngine;

namespace lln.Bluetooth.bluetoothDriverWrapper{
    public class Wrapper : MonoBehaviour{
        public AndroidJavaClass pluginClass;
        public AndroidJavaObject pluginObj;

        private void Awake(){
            DontDestroyOnLoad(gameObject);
            InitializePlugin();
        }
        
        private void InitializePlugin() {
            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

            pluginClass = new AndroidJavaClass("scut.why.Main");
            pluginObj = pluginClass.CallStatic<AndroidJavaObject>("newInstance");
            Debug.LogWarning("当前OBJECT为空吗 " + (pluginObj == null));

            int i = pluginObj.Get<int>("REQ_PERMISSION_CODE");
            Debug.Log("获取到属性吗? " + i);
            pluginObj.Set("activity",currentActivity);
            
        }

        public void hostSendMsg(string msg){
            pluginObj.Call("hostSendMsg" , msg);
        }


        public void sendMsg(string msg){
            Debug.LogWarning("发出去的消息是 " + msg);
            pluginObj.Call("sendMsg" , msg);
        }

        public void makeBluetoothDiscoverable(){
            Debug.LogWarning("对象是null吗?" + (pluginObj == null));
            pluginObj.Call("makeBluetoothDiscoverable");
        }

        public void startScanViable(){
            pluginObj.Call("startScanViable");
        }
        
        public void doServer(){
            pluginObj.Call("doServer");
        }

        public void seekForBinded(){
            pluginObj.Call("seekForBinded","Xihan的Mi 11");
        }

        public void doClient(){
            pluginObj.Call("doClient");
        }
        
        
    }
}