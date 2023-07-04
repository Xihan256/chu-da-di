using lln.Bluetooth.bluetoothDriverWrapper;
using lln.Bluetooth.server;
using UnityEngine;

namespace lln.Bluetooth.BTTask{
    public class BTSkip : MonoBehaviour{
        public Wrapper wrapeer;

        public void workToDo(){
            wrapeer = GameObject.Find("Main").GetComponent<Main>().wrapper;
            wrapeer.sendMsg("no");
        }
    }
}