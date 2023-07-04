using lln.Bluetooth.bluetoothDriverWrapper;
using lln.Bluetooth.server;
using UnityEngine;

namespace lln.Bluetooth.BTTask{
    public class BTInformToShow : MonoBehaviour{
        public Wrapper wrapeer;

        public void WorkToDo(string currentIP){
            wrapeer = GameObject.Find("Main").GetComponent<Main>().wrapper;
            wrapeer.hostSendMsg("w" + currentIP);
        }
    }
}