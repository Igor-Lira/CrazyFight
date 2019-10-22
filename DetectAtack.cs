using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectAtack : Photon.MonoBehaviour
{
    public GameObject player;
    public GameObject Baguette;
    static bool gameOver;
    public bool isBlocked = false;
    private bool EndGameStatus;
    public int dmg = 2;
    private GameObject targetPlayer;
    public Animator animator;
    public bool teleReady = false;

    public int winsCount = 0;
    public bool iLost = false;

    public Vector3 telPos;

    public bool SpecialBaguetteAtack = false;

    static bool resultScene = false;
    public GameObject sectionWon, sectionGame, sectionLost;

    ExitGames.Client.Photon.Hashtable playerCustomSettings = new ExitGames.Client.Photon.Hashtable();
    void Awake()
    {
        gameOver = false;
    }
    void Start()
    {

        PhotonNetwork.automaticallySyncScene = true;
        resultScene = false;

        //set scenes
        sectionGame = GameObject.FindWithTag("gameScene");
        sectionWon = GameObject.FindWithTag("wonScene");
        sectionLost = GameObject.FindWithTag("lostScene");
        sectionGame.SetActive(true);
        sectionWon.SetActive(false);
        sectionLost.SetActive(false);

    }
    void Update()
    {
        if (gameOver && photonView.isMine)
        {
            ResultScene();
        }
          //  if (gameOver && photonView.isMine) // This means that one player is already dead so we need to restart the Lobby
          if (gameOver && PhotonNetwork.isMasterClient && resultScene)
        {
            //when we load the level we need to know who won and dispay in the lobby.
            PhotonNetwork.LoadLevel("Lobby");
        }
        if (Baguette.GetComponent<Colision>()._TargetFounded && !isBlocked && !gameOver)
        {

            if (player.GetComponent<Move>().process_Atack || player.GetComponent<Move>().RotationAnimation)
            {
                //When in atacked we disable "Is trigged? // We do one transportation stuff to get the enemy back ??



                if (player.GetComponent<Move>().combo1Ultamate) // Check if combo demage is available
                {
                    dmg = 5;
                    player.GetComponent<Move>().combo1Ultamate = false;
                    player.GetComponent<Move>().combo1UltamatReady = false;
                }
                targetPlayer = Baguette.GetComponent<Colision>().playeratacked;

                if (targetPlayer.GetComponent<InfoPlayer>().Life > 0)
                {
                    targetPlayer.GetComponent<DetectAtack>().photonView.RPC("ApplyDamage", PhotonTargets.All);

                    targetPlayer.GetComponent<Move>().photonView.RPC("Block", PhotonTargets.All);
                    dmg = 2; //Restart the demage 



                }
                else
                {
                    if (photonView.isMine)
                    {
                        photonView.RPC("GameOverCall", PhotonTargets.All);// We need to send this variable to all players to alert that the game is alredy finisehd so they disconect.
                        targetPlayer.GetComponent<DetectAtack>().photonView.RPC("GameOverCall", PhotonTargets.All);
                      
                        // player.GetComponent<DetectAtack>().photonView.RPC("CountWinsVictory", PhotonTargets.All);

                    }
                    //    CountWins = (int)PhotonNetwork.playerList[0].CustomProperties["Victories"];
                    //   playerCustomSettings.Add("Victories", CountWins++);
                    //  PhotonNetwork.player.SetCustomProperties(playerCustomSettings);
                }
            }



        }
        

           
       
    }

    [PunRPC]
    public void ApplyDamage()
    {
        photonView.RPC("PlayAtack", PhotonTargets.All);
        player.GetComponent<InfoPlayer>().Life -= dmg;

        if (player.GetComponent<InfoPlayer>().Life < 1)
        {
            gameOver = true;
        }
    }

    [PunRPC]
    public void ChangeCameraBaguette()
    {
        player.GetComponent<Move>().SpecialBaguetteAtack = true;
    }
    [PunRPC]
    public void Block()
    {
        player.GetComponent<Move>().speed = 0;
        isBlocked = true;
        animator.SetBool("isBlocked", isBlocked);
        StopAnimationBlock();
    }
    public void StopAnimationBlock()
    {
        StartCoroutine(ExecuteAfterTime(0.5f));
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isBlocked = false;
        animator.SetBool("isBlocked", isBlocked);
        player.GetComponent<Move>().speed = 500;
        // In this meedle time, the player can teletransportate to behinde the Enemy, so he can have more chances to fight better. 
        if (!teleReady && GetComponent<Move>().PossibleTele)
        {
            GetComponent<Move>().wantTele = true;
            teleReady = true;
        }
        yield return new WaitForSeconds(time/2);
        GetComponent<Move>().wantTele = false;
        teleReady = false;
        GetComponent<Move>().speed = GetComponent<Move>().speedStandart;
    }
    //Result Scene
    public void ResultScene()
    {

        if (iLost)
        {
            sectionGame.SetActive(false);
            sectionLost.SetActive(true);
            sectionWon.SetActive(false);
        }
        else
        {
            sectionGame.SetActive(false);
            sectionLost.SetActive(false);
            sectionWon.SetActive(true);
        }

        StartCoroutine(ResultAfterTime(2f));
    }
    IEnumerator ResultAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        resultScene = true;
    }

    [PunRPC]
    public void GameOverCall()
    {
       // print(" I'm " + photonView.viewID.ToString()[0] + "and I lost times: " + winsCount);
        PlayerPrefs.SetString("wholost", photonView.viewID.ToString());
        gameOver = true;
       // iLost = true;
    }
    [PunRPC]
    public void CountWinsVictory()
    {
        player.GetComponent<InfoPlayer>().countWins++;
    }


    [PunRPC]
    public void PlayAtack()
    {
        GetComponent<Move>().audioSource.clip = GetComponent<Move>().atackSound;
        GetComponent<Move>().audioSource.Play();
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
        }

        else
        {
        }
    }
}