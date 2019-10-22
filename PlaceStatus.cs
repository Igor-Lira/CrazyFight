using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceStatus : Photon.MonoBehaviour
{
    public GameObject place;
    public bool isOccupied = false;
    void Update()
    {
        if (isOccupied)
        {
            photonView.RPC("PlaceOneOccupied", PhotonTargets.All);
        }
    }

    [PunRPC]
    public void PlaceOneOccupied()
    {
        isOccupied = true;
        print(this.place.name);
    }
}
