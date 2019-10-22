using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyButtons : MonoBehaviour
{
    public GameObject playerLobby;
    public bool playerIsReady; // When all players are ready we start the game
    // Start is called before the first frame update
    public void OnClickedReady()
    {
      //  playerLobby.GetComponent<LobbyPlayerScenes>().NotReady.SetActive(false);
       // playerLobby.GetComponent<LobbyPlayerScenes>().Ready.SetActive(true);
        playerLobby.GetComponent<LobbyPlayerScenes>().OnCLickedReady();
    }
    public void OnClickedLeave()
    {
        //  playerLobby.GetComponent<LobbyPlayerScenes>().NotReady.SetActive(false);
        // playerLobby.GetComponent<LobbyPlayerScenes>().Ready.SetActive(true);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MenuInicialization");
    }
}
