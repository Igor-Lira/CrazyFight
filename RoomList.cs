using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : MonoBehaviour
{
    public GameObject connection;
    public RoomInfo[] roomsList;
    public GameObject roomLobbyPreFab;
    public bool showList = false;
    public GameObject noAvaibleText;
    private GameObject[] objs;
    // Start is called before the first frame update
    void Start()
    {
        roomsList = connection.GetComponent<PhotonConnect>().roomsList;

    }

    // Update is called once per frame
    void Update()
    {
       if (showList)
        {
            //Delete list:
            objs = GameObject.FindGameObjectsWithTag("roomLobby");
            for (int i = 0; i < objs.Length; i++)
            {
                Destroy(objs[i]);
            }

            if (roomsList.Length > 0)
            {
                for (int i = 0; i < roomsList.Length; i++)
                {
                    // var spwanPosition = new Vector3(-140, 280, 0); // this will depend of i
                    var roomIndx = i + 1;                                          // Create a new line text in the lobby:
                    var roomLobby = Instantiate(roomLobbyPreFab);
                    roomLobby.transform.parent = gameObject.transform;
                    roomLobby.GetComponent<textInfo>().text.GetComponent<Text>().text = "Room " + roomIndx + " : " + roomsList[i].Name;
                    roomLobby.GetComponent<textInfo>().textJoin = roomsList[i].Name;
                    roomLobby.GetComponent<RectTransform>().position = new Vector3(200, 400 - 100 * i, 0);
                    Debug.LogFormat(this, "[{0}] - {1}", i, roomsList[i].Name);
                }
                
            }
            else
            {
                noAvaibleText.GetComponent<Text>().text = "NO OTHER PLAYER WAS FOUND !";
                // No rooms avaibles
            }
            showList = false;
        }
    }

    void OnReceivedRoomListUpdate()
    {
         roomsList = PhotonNetwork.GetRoomList();
        showList = true;


    }
}
