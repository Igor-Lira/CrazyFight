using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenager : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject myPlayer;
    private GameObject camera;
    public bool gameOver = true;
    private Vector3 spwanPosition;

    public GameObject healthBarPreFab;
    private GameObject healthBar;

    public GameObject background;

    public GameObject infoSaved;

    void Start() //Using Awake here was causing a lot of BUG'S 
    {
        infoSaved = GameObject.FindWithTag("info");
        //PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.isMasterClient)
        {
            spwanPosition = new Vector3(-3, 5, 0);
        }
        else
        {
            spwanPosition = new Vector3(3, 5, 0);
        }
        myPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spwanPosition, playerPrefab.transform.rotation, 0);

        camera = GameObject.FindWithTag("MainCamera");
        if (camera != null)
        {
            camera.GetComponent<PlayerCamera>().target = myPlayer;
        }

        healthBar = PhotonNetwork.Instantiate(healthBarPreFab.name, spwanPosition, healthBarPreFab.transform.rotation, 0);
      //  healthBar.transform.parent = camera.transform;
        healthBar.GetComponent<HealthbarPosition>().background = background;
        healthBar.GetComponent<HealthbarPosition>().player = myPlayer;
        myPlayer.GetComponent<Move>().playerBar = healthBar;
        myPlayer.GetComponent<InfoPlayer>().infoSaved = infoSaved;


    }
    void Update()
    {
        healthBar.GetComponent<HealthbarPosition>().life = myPlayer.GetComponent<InfoPlayer>().Life;

    }
    IEnumerator OnLeftRoom()
    {
        while (PhotonNetwork.room != null || PhotonNetwork.connected == false)
        {
            yield return 0;
        }

    }

}

