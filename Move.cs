using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Move : Photon.MonoBehaviour
{
    //my player :
    public GameObject player;
    // my health bar:
    public GameObject playerBar;

    private Vector3 selfPos;
    private Vector3 target;
    private Vector2 target2D;
    private Vector2 playerPos2D;
    private Vector3 mousePos;
    private Vector2 direction;
    public float  speedStandart;
    public float speed;
    private float dist;
    private Rigidbody2D rb;
    public static bool drawNow = false;

    private int NbJumps = 0;
    public int NbJumpsMax = 4;
    public bool process_Atack = false;
    private bool process_AtackDelay = false;
    private bool flipSprite;
    public GameObject myBaguette;


    private float startTimeD;
    private float startTimeA;
    private bool holdA, holdB;

    // To know if the player is next to a collider
    private bool Grounded;
    public Transform footPos; // the mesh itself?
    public LayerMask whatIsGround; // Layer of colliders to test if gorunded 

    //To animations:

    public Animator animator;
    public Animator animatorPlayer;
    public bool movingAnimator;
    public bool flyingAnimator;
    private float StartFlyingTime;
    private bool grounded2;

    //Sprites to flip
    private SpriteRenderer mySpriteRenderer;
    public SpriteRenderer BaguetteSprite, selfSprite;
    private Vector3 selfMyBaguetteScale, selfPlayerScale;


    //To combo Move 1:
    int noOfClicks1 = 0;
    //Time when last button was clicked
    float lastClickedTime = 0;
    //Delay between clicks for which clicks will be considered as combo
    float maxComboDelay = 1.0f;
    float maxComboDelayUltmate = 1.3f;
    float ComboDelayBetween = 0.3f;
    public bool combo1Ultamate = false;
    public bool combo1UltamatReady = false;


    //To combo Move 2:
    int noOfClicks2 = 0;
    //Time when last button was clicked
    float lastClickedTime2 = 0;
    //Delay between clicks for which clicks will be considered as combo

    // Combo Down Atack:
    float lastClickedTime3;
    int noOfClicks3 = 0;
    public bool RotationAnimation;
    private Quaternion targetRotation;

    // Special Atack 
    bool process_AtackSpeacial = false;
    public GameObject baguetteSpecial;
    public GameObject baguetteSpecialPreFab;

    //Tele
    public bool PossibleTele = false;
    public bool wantTele = false;
    public bool changeInScale = false;
    public int i = 0;
    public GameObject TeleportSmokePreFab;

    //move camera
    public bool SpecialBaguetteAtack = false;

    //audios
    public AudioSource audioSource;
    public AudioClip teleport, atackSound;

    private void Awake()
    {
        // get a reference to the SpriteRenderer component on this gameObject
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        PhotonView photonView = PhotonView.Get(this);

        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        speedStandart = 190;
        rb = GetComponent<Rigidbody2D>(); // Get the Body Information of the Player to apply forces and others methods 
        Grounded = true;
        speed = speedStandart;
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            if (wantTele)
            {

                if (Input.GetKeyDown(KeyCode.S))
                {
                    wantTele = false;
                    changeInScale = true;
                    TelePosition();
                    //TelePosition();
                    TeleAnimation();
                }
               
            }
            if (changeInScale)
            {
                ChangeScaleTele();
            }
            else
            {
                if (playerBar.GetComponent<HealthbarPosition>().SpecialCount > 50)
                {
                    PossibleTele = true;
                }
                //First of all we need to know if the player is gorunded or not 
                MoveCameraSpecialBaguette();

                Grounded = Physics2D.OverlapCircle(footPos.position + new Vector3(0, -1f, 0), 1.0f, whatIsGround);

                playerPos2D = transform.position; //// Get the PlayerPosition
                if (Input.GetKeyDown(KeyCode.W))
                {
                    drawNow = true;
                    // photonView.RPC("ChatMessage", PhotonTargets.All, "jup", "and jup!");
                }
                if (Input.GetKey(KeyCode.D) || holdB)
                {
                    holdA = false;
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        startTimeD = Time.time;
                        holdB = true;
                    }
                    if (Time.time - startTimeD < 1)
                    {

                        Mouvement_key(new Vector2(1, 0));
                        if (mySpriteRenderer != null)
                        {
                            // flip the sprite
                            if (myBaguette.transform.localScale.x < 0) // flip the baguettte
                            {
                                myBaguette.transform.localScale = new Vector3(-myBaguette.transform.localScale.x, myBaguette.transform.localScale.y, myBaguette.transform.localScale.z);
                            }
                            if (player.transform.localScale.x < 0) // flip the player
                            {
                                player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
                            }
                        }
                        if (!movingAnimator)
                        {
                            movingAnimator = true;
                            animatorPlayer.SetBool("moving", movingAnimator);
                            StopAnimationPlayer();
                        }
                    }
                    else
                    {
                        holdB = false;
                    }
                }
                if (Input.GetKey(KeyCode.A) || holdA)
                {
                    holdB = false;
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        startTimeA = Time.time;
                        holdA = true;
                    }
                    if (Time.time - startTimeA < 1)
                    {
                        Mouvement_key(new Vector2(-1, 0));
                        if (mySpriteRenderer != null)
                        {
                            // flip the sprite
                            if (myBaguette.transform.localScale.x > 0)
                            {
                                //  myBaguette.transform.localScale = new Vector3(-myBaguette.transform.localScale.x, myBaguette.transform.localScale.y, myBaguette.transform.localScale.z);
                            }
                            if (player.transform.localScale.x > 0)
                            {

                                player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
                            }
                        }
                        if (!movingAnimator)
                        {
                            movingAnimator = true;
                            animatorPlayer.SetBool("moving", movingAnimator);
                            StopAnimationPlayer();
                        }
                    }
                    else
                    {
                        holdA = false;
                    }
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    holdB = false;
                    holdA = false;
                    Mouvement_key_Down(new Vector2(0, -1));
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    holdB = false;
                    holdA = false;
                    if (Grounded) // if the object is grounded, it means it'll be the first jump
                    {
                        NbJumps = 1;
                        Mouvement_key_Up(new Vector2(0, 1));
                    }
                    else
                    {
                        NbJumps = NbJumps + 1;
                        if (NbJumps < NbJumpsMax)
                        {
                            Mouvement_key_Up(new Vector2(0, 1));
                        }

                    }

                }


                //variable ground2 to stop animation when close to the floor
                grounded2 = Physics2D.OverlapCircle(footPos.position + new Vector3(0, -1f, 0), 3.0f, whatIsGround);
                if (!grounded2)
                {
                    flyingAnimator = true;
                    movingAnimator = false; // you cant run in the air
                    animatorPlayer.SetBool("moving", movingAnimator); // this stops run animation
                    animatorPlayer.SetBool("flying", flyingAnimator); // this start fluing animation
                    if (Time.time - StartFlyingTime > 0.28f) // we change animation 
                    {
                        animatorPlayer.SetBool("flying2", flyingAnimator);
                    }

                }
                else
                {
                    // we stop animation
                    StartFlyingTime = Time.time;
                    flyingAnimator = false;
                    animatorPlayer.SetBool("flying2", flyingAnimator);
                    animatorPlayer.SetBool("flying", flyingAnimator);
                }
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    // we need to wait some time to proced to another atack
                    if (!process_AtackDelay)
                    {
                        process_Atack = true;
                        process_AtackDelay = true;
                        animator.SetBool("atack", process_Atack);
                        StopAnimation();
                    }
                }

                //----------------------------------COMBO_MOVE_1-----------------------------------------------
                OnClicked_moveCombo1();
                //----------------------------------COMBO_MOVE_2-----------------------------------------------
                OnClicked_moveCombo2();
                //---------------------------------COMBO_
                //------------------------------------SPECIAL_ATACK--------------------------------------------
                SpecialAtackplayer();
                //----------------------------------TELETRANSPORTATE-------------------------------------------
                DownAtack();

                // IMPORTANT: WE NEED TO STUDY THE CASE THAT BOTH PLAYERS ATACK FRONT - FRONT !!!
            }
        }
        else // Reciving information from the server
        {
            smoothNetMovement();
            TurnSprite(selfPlayerScale, selfMyBaguetteScale);
        }
    }
    private void smoothNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 10);
    }
    private void Mouvement_key(Vector2 direction)
    {
        if (!player.GetComponent<DetectAtack>().isBlocked)
        {
            // We increase the move speed only if the click count is 3
            if (noOfClicks1 == 3)
            {
                speed = 3000;
                noOfClicks1 = 0; // reset 
            }
            if (noOfClicks2 == 3)
            {
                speed = 3000;
                noOfClicks2 = 0; // reset
            }
            if (combo1UltamatReady && noOfClicks1 == 4)
            {
                combo1UltamatReady = false;
                combo1Ultamate = false;
                noOfClicks1 = 0;
                speed = 3000;
            }
            if (combo1UltamatReady && noOfClicks2 == 4)
            {
                combo1UltamatReady = false;
                combo1Ultamate = false;
                noOfClicks2 = 0;
                speed = 3000;
            }
            rb.AddForce(direction * 100 * speed * Time.deltaTime);
            speed = speedStandart;
        }
        else
        {
            rb.AddForce(direction * 100 * speed * Time.deltaTime);
            speed = speedStandart;
        }
    }
    private void Mouvement_key_Up(Vector2 direction)
    {
        rb.AddForce(direction * 1000 * speed * Time.deltaTime);
    }
    private void Mouvement_key_Down(Vector2 direction)
    {
        if (RotationAnimation)
        {
            speed = 4000;
            rb.AddForce(direction * 100 * speed * Time.deltaTime);
            speed = speedStandart;
        }
        else
        {
            rb.AddForce(direction * 150 * speed * Time.deltaTime);
            speed = speedStandart;
        }
       
    }
 
    public void StopAnimation()
    {
        StartCoroutine(ExecuteAfterTime(0.15f));
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        process_Atack = false;
        animator.SetBool("atack", process_Atack);
        yield return new WaitForSeconds(time + 0.2f);
        process_AtackDelay = false;

    }
    public void StopAnimationPlayer()
    {
        StartCoroutine(ExecuteAfterTimePlayer(0.45f));
    }
    IEnumerator ExecuteAfterTimePlayer(float time)
    {
        yield return new WaitForSeconds(time);
        movingAnimator = false;
        animatorPlayer.SetBool("moving", movingAnimator);
    }
    //To multipleyer game:
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo Info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(myBaguette.transform.localScale);
            stream.SendNext(player.transform.localScale);
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
            selfMyBaguetteScale = (Vector3)stream.ReceiveNext();
            selfPlayerScale = (Vector3)stream.ReceiveNext();
        }
    }
    void TurnSprite(Vector3 playerScale, Vector3 selfMyBaguetteScale)
    {
        player.transform.localScale = playerScale;
        myBaguette.transform.localScale = selfMyBaguetteScale;

    }
    private void OnClicked_moveCombo1()
    {

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks1 = 0;
        }
        if (noOfClicks1 == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                lastClickedTime = Time.time;
                noOfClicks1++;
            }
            else
            {
                noOfClicks1 = 0;
            }
        }
        if (noOfClicks1 == 1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                lastClickedTime = Time.time;
                noOfClicks1++;
            }
            else
            {
                if (Time.time - lastClickedTime > ComboDelayBetween)
                {
                    noOfClicks1 = 0;
                }

            }
        }
        if (noOfClicks1 == 2)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                lastClickedTime = Time.time;
                noOfClicks1++;
                combo1Ultamate = true;
            }
            else
            {
                if (Time.time - lastClickedTime > ComboDelayBetween)
                {
                    noOfClicks1 = 0;
                }

            }
        }
        //maxComboDelayUltmate
        if (combo1Ultamate)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                lastClickedTime = Time.time;
                combo1UltamatReady = true;
                noOfClicks1 = 4;
            }
            else
            {
                if (Time.time - lastClickedTime > maxComboDelayUltmate)
                {
                    noOfClicks1 = 0;
                    combo1UltamatReady = false;
                    combo1Ultamate = false;
                }

            }
        }
    }

    private void OnClicked_moveCombo2()
    {

        if (Time.time - lastClickedTime2 > maxComboDelay)
        {
            noOfClicks2 = 0;
        }
        if (noOfClicks2 == 0)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                lastClickedTime2 = Time.time;
                noOfClicks2++;
            }
            else
            {
                noOfClicks2 = 0;
            }
        }
        if (noOfClicks2 == 1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                lastClickedTime2 = Time.time;
                noOfClicks2++;
            }
            else
            {
                if (Time.time - lastClickedTime2 > ComboDelayBetween)
                {
                    noOfClicks2 = 0;
                }

            }
        }
        if (noOfClicks2 == 2)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                lastClickedTime2 = Time.time;
                noOfClicks2++;
                combo1Ultamate = true;
            }
            else
            {
                if (Time.time - lastClickedTime2 > ComboDelayBetween)
                {
                    noOfClicks2 = 0;
                }

            }
        }
        //maxComboDelayUltmate
        if (combo1Ultamate)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                noOfClicks2 = 4;
                lastClickedTime2 = Time.time;
                combo1UltamatReady = true;
            }
            else
            {
                if (Time.time - lastClickedTime2 > maxComboDelayUltmate)
                {
                    noOfClicks2 = 0;
                    combo1UltamatReady = false;
                    combo1Ultamate = false;
                }

            }
        }
    }

    private void SpecialAtackplayer()
    {

        if (Input.GetKeyUp(KeyCode.Space) && playerBar.GetComponent<HealthbarPosition>().specialReady)
        {
            process_AtackSpeacial = true;
            //Creates a new Baguette that will survive for 5 seconds 
            baguetteSpecial = PhotonNetwork.Instantiate(baguetteSpecialPreFab.name, transform.position, baguetteSpecialPreFab.transform.rotation, 0);
            baguetteSpecial.GetComponent<SpecialBaguette_Move>().owner = player;
            //GetComponent<InfoPlayer>().specialReady = false; // Send this to health bar!!!!!
            playerBar.GetComponent<HealthbarPosition>().specialReady = false;
            playerBar.GetComponent<HealthbarPosition>().SpecialCount = 0;
            //GetComponent<InfoPlayer>().SpecialCount = 0;

        }
    }

    private void DownAtack()
    {
        // if you are no grounded and you click twice S, you rotate (and increse demage). You became invunerable and finaly push away all . 
        if (!grounded2)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                lastClickedTime3 = Time.time;
                noOfClicks3++;
            }
            if (noOfClicks3 == 3)
            {
                RotationAnimation = true;
                noOfClicks3 = 0;
            }
            if (RotationAnimation)
            {
                if (player.transform.localScale.x > 0)
                {
                    targetRotation = Quaternion.Euler(0, 0, -90);
                }
                else
                {
                    targetRotation = Quaternion.Euler(0, 0, 90);
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                animator.SetBool("downAtack", RotationAnimation);

            }
        }
        else
        {
            if (RotationAnimation)
            {
                RotationAnimation = false;
                animator.SetBool("downAtack", RotationAnimation);
              
            }
            var target = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 3f);
            noOfClicks3 = 0;
        }

    }
    private void MoveCameraSpecialBaguette()
    {
        if (SpecialBaguetteAtack)
        {
            playerBar.GetComponent<HealthbarPosition>().changeChamera = true;
           // SpecialBaguetteAtack = false;
        }
    }
    private void TelePosition()
    {
        photonView.RPC("PlayTeleport", PhotonTargets.All);

        playerBar.GetComponent<HealthbarPosition>().SpecialCount -= 50;
        PossibleTele = false;

        PhotonNetwork.Instantiate(TeleportSmokePreFab.name, transform.position, TeleportSmokePreFab.transform.rotation, 0);
        if (player.transform.localScale.x > 0)
        {
            transform.position += new Vector3(7, 3, 0);
        }
        else
        {
            transform.position += new Vector3(-7, 3, 0);
        }
    }
    private void ChangeScaleTele()
    {


        if (i == 1)
        {
            //we get back to the initial values after 1 second.
            DelayScale();
        }
        else
        {
            GetComponent<AnimatedSpriteCollider>().iStrigger = false;
            player.transform.localScale = new Vector3(0, 0, 0);
            i++;
        }
    }
    private void TeleAnimation()
    {
        StopAnimationTele();
    }
     private void DelayScale()
    {
        StartCoroutine(ExecuteAfterDelayScale(0.3f));
    }
    IEnumerator ExecuteAfterDelayScale(float time)
    {
        yield return new WaitForSeconds(time);
        changeInScale = false;
        player.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        GetComponent<AnimatedSpriteCollider>().iStrigger = true;
        i = 0;
    }

    public void StopAnimationTele()
    {
        StartCoroutine(ExecuteAfterTimeTele(0.45f));
    }
    IEnumerator ExecuteAfterTimeTele(float time)
    {
        yield return new WaitForSeconds(time);
        wantTele = false;
        GetComponent<DetectAtack>().teleReady = false;
    }

    [PunRPC]
    public void PlayTeleport()
    {
        audioSource.clip = teleport;
        audioSource.Play();
    }

}