using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Welcome : MonoBehaviour
{

    public void ToLogin(){
        SceneManager.LoadScene("001_login");
    }
}
