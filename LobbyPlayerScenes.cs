using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyPlayerScenes : Photon.MonoBehaviour
{


    ExitGames.Client.Photon.Hashtable playerCustomSettings = new ExitGames.Client.Photon.Hashtable();
    public int playersReady = 0;

    public GameObject background;
    public GameObject textNick, textScore;
    public string nickToShow;

    static int  WinsPlayer1 = 0, WinsPlayer2 = 0;
    static bool modified;

    private bool done = false;

    public Sprite ready;


    void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
        playerCustomSettings.Add("Ready", false);
        playerCustomSettings.Add("Done", false);

        PhotonNetwork.player.SetCustomProperties(playerCustomSettings);
    }

    void Start()
    {

        CountVictories();
        if (photonView.isMine)
        {
            // GetComponent<nickSave>().CallBackLobby();
            photonView.RPC("ShowNick", PhotonTargets.All);
        }

        modified = false;

    }




    public void OnCLickedReady()
    {
        // Shows to others players that you are ready 
        if (photonView.isMine)
        {
            photonView.RPC("ShowReady", PhotonTargets.All);
        }

        if (photonView.isMine)
        {
            // GetComponent<nickSave>().CallBackLobby();
            photonView.RPC("ShowNick", PhotonTargets.All);
        }


        background.GetComponent<Image>().color = Color.green;
        background.GetComponent<LobbyPosition>().ready = true;

        playerCustomSettings["Ready"] = true;

        PhotonNetwork.player.SetCustomProperties(playerCustomSettings);
        // print("I'm the " + photonView.viewID + " and players ready is " + playersReady);

        if (playersReady < 2)
        {
            if (!done && (bool)PhotonNetwork.playerList[0].CustomProperties["Ready"] == true && (bool)PhotonNetwork.playerList[1].CustomProperties["Ready"] == true)
            {
                playersReady = 2;
                done = true;
            }
        }
        if (playersReady == 2 && PhotonNetwork.isMasterClient)
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.LoadLevel("Game");
            }
            else
            {
                Debug.Log("um outro tentou sem ser o master");
            }
        }
    }

    void CountVictories()
    {

            if (!modified && PhotonNetwork.isMasterClient && photonView.isMine)
            {
               
                    if (PlayerPrefs.GetString("wholost")[0].ToString() == "1")
                    {
                        WinsPlayer2++;
                     modified = true;
                     }
                    if (PlayerPrefs.GetString("wholost")[0].ToString() == "2")
                    {
                        WinsPlayer1++;
                modified = true; 
            }
                
            photonView.RPC("ShowScore", PhotonTargets.AllBuffered);

        }

    }

    [PunRPC]
    public void ShowReady ()
    {
        background.GetComponent<Image>().color = Color.green;
        background.GetComponent<Image>().sprite = ready;
        background.GetComponent<LobbyPosition>().ready = true;

    }

    [PunRPC]
    public void ShowNick()
    {
        textNick.GetComponent<Text>().text = "player " + photonView.viewID.ToString()[0];
        // textNick.GetComponent<Text>().text = photonView.owner.NickName;
      
            // textNick.GetComponent<Text>().text = PlayerPrefs.GetString("nick");
   
    }

    [PunRPC]
    public void ShowScore()
    {
      //  textScore.GetComponent<Text>().text = "Player1 Wins: " + WinsPlayer1 + "   Player2 Wins: " + WinsPlayer2;
    }

    void OnPlayerEnteredRoom()
    {
        if (photonView.isMine)
        {
            photonView.RPC("ShowNick", PhotonTargets.All);
        }
    }
}

