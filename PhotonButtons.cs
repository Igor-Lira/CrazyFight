using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonButtons : Photon.MonoBehaviour
{
    public InputField createRoomInput, joinRoomInput;
    public MenuLogic mLogic;
    public GameObject player;

    public GameObject InfoSaveDataPreFab, infoSaver;
    public GameObject nickField;

    public GameObject radomFails;

    public GameObject  menu, rooms;

    public void onClickCreateRoom()
    {
        if (createRoomInput.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions() { MaxPlayers = 4 }, null);

        }

        
    }
    public void onClickJoinRoom()
    {
       //creates an object that will save all the data!!!!

        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    public void onClickJoinRandom()
    {
        menu.SetActive(false);
        rooms.SetActive(true);
        rooms.GetComponent<RoomList>().showList = true;
        // PhotonNetwork.JoinRandomRoom();


    }
    void OnPhotonRandomJoinFailed()
    {
        radomFails.GetComponent<Text>().enabled = true;
        ChangeText();
    }
    public void OnJoinedRoom()
    {
        // photonView.RPC("DefineNick", PhotonTargets.All);
        // PhotonNetwork.player.NickName = nickField.GetComponent<Text>().text;
        infoSaver = PhotonNetwork.Instantiate(InfoSaveDataPreFab.name, new Vector3(10, 0, 0), InfoSaveDataPreFab.transform.rotation, 0);
        mLogic.disableMenuUI();

    }
    [PunRPC]
    public void DefineNick()
    {
            PlayerPrefs.SetString("nick", nickField.GetComponent<Text>().text);

    }
    public void ChangeText()
    {
        StartCoroutine(ExecuteAfterTime(1f));
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        radomFails.GetComponent<Text>().enabled = false;
    }
    public void onClickBackMenu()
    {
        menu.SetActive(true);
        rooms.SetActive(false);
        rooms.GetComponent<RoomList>().showList = true;
        // PhotonNetwork.JoinRandomRoom();
    }
}
