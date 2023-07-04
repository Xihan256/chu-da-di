using lln.Bluetooth.bluetoothDriverWrapper;
using lln.Bluetooth.server;
using UnityEngine;

namespace lln.Bluetooth.BTTask{
    public class BTShowCards : MonoBehaviour{
        public Wrapper wrapeer;

        public void workToDo(string json){
            wrapeer = GameObject.Find("Main").GetComponent<Main>().wrapper;
            wrapeer.sendMsg('s' + json);
        }
    }
}