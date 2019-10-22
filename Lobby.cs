using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : Photon.MonoBehaviour
{
    public GameObject playerLobby;
    public GameObject playerLobbyPrefab;
    bool readyLobby;
    private Vector3 offset;
    public int readyPlayers = 0 ;


    //To save info
    public GameObject infoSaved;


    // Start is called before the first frame update

    void Awake()
    {
        

        PhotonNetwork.automaticallySyncScene = true;
    }

    void Start()
    {

        infoSaved = GameObject.FindWithTag("info");

        PhotonNetwork.automaticallySyncScene = true;
        playerLobby = PhotonNetwork.Instantiate(playerLobbyPrefab.name, new Vector3(10, 0, 0), playerLobbyPrefab.transform.rotation, 0);
        GetComponent<LobbyButtons>().playerLobby = playerLobby;
        
    }
 void Update()
    {
       // print(PlayerPrefs.GetString("nick1") + " and nick2: " + PlayerPrefs.GetString("nick2"));
    }
}
