using lln.Bluetooth.bluetoothDriverWrapper;
using lln.Bluetooth.server;
using UnityEngine;

namespace lln.Bluetooth.BTTask{
    public class BTFinishGame : MonoBehaviour{
        public Wrapper wrapeer;

        public void WorkToDo(){
            wrapeer = GameObject.Find("Main").GetComponent<Main>().wrapper;
            wrapeer.sendMsg("finish");
        }
    }
}