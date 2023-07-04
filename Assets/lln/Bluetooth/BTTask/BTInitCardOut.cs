using lln.Bluetooth.bluetoothDriverWrapper;
using lln.Bluetooth.server;
using UnityEngine;

namespace lln.Bluetooth.BTTask{
    public class BTInitCardOut : MonoBehaviour{
        public Wrapper wrapeer;

        public void WorkToDo(string players){
            wrapeer = GameObject.Find("Main").GetComponent<Main>().wrapper;
            wrapeer.hostSendMsg('i' + players);
        }
    }
}