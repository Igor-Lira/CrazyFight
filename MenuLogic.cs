using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuLogic : MonoBehaviour
{
    public GameObject connectedMenu, nickObj;

    
    public void disableMenuUI ()
    {
        PlayerPrefs.SetString("wholost","0");
        PhotonNetwork.LoadLevel("Game");

    }
}
