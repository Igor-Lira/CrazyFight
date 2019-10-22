using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPlayer : Photon.MonoBehaviour
{
    public GameObject player;
    public float Life = 100;

    public int countWins = 0;

    public GameObject infoSaved;

    public string nick;

    void Update()
    {
        if (photonView.isMine)
        {
            photonView.RPC("MyNAmeIs", PhotonTargets.All);
        }
    }

    [PunRPC]
    public void MyNAmeIs()
    {
        //print(photonView.owner.NickName);
        print("i'm the " + PhotonNetwork.player.ID + ", nick1: " + PlayerPrefs.GetString("nick"));
    }
}
