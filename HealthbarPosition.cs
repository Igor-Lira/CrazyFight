using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarPosition : Photon.MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    public Transform barTransform;
    public float life = 100;
    public float SpecialCount;

    public Transform bar; //5.2 Length
    public Transform barReference;

    public Transform barSpecial;
    public Transform barReferenceSpecial;

    public Vector3 offset;
    public GameObject camera;

    private float startTime;
    public bool specialReady = false;

    private Vector3 selfLocalScale, selfPosition, selfLocalScaleSpecial, selfPositionSpecial;

    //animationBaguetteSpecial
    public float size = 10;
    public bool changeChamera = false, changeBack = false, stopchangeCamera = false;
    public GameObject background;
    public int backgroundControler = 0;


    void Awake()
    {
        SpecialCount = 0;
        offset = new Vector3(17, 0, 1);
        camera = GameObject.FindWithTag("MainCamera");
    }
    void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            offset = new Vector3(17, 0, 1);
        }
        else
        {
            offset = new Vector3(17, 0, 1);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (photonView.isMine)
        {
            if (life < 1)
            {
                player.GetComponent<DetectAtack>().iLost = true;
            }
            //Chenge Camera Size if lost a lot of demage:

            if (changeChamera && !stopchangeCamera)
            {
                backgroundControler++;

                if (backgroundControler < 10)
                {
                    background.GetComponent<SpriteRenderer>().color = Color.red;
                }
                else
                {
                    background.GetComponent<SpriteRenderer>().color = Color.white;
                }
                if (backgroundControler > 20)
                {
                    backgroundControler = 0;
                }

                if (size > 5)
                {
                    if (!changeBack)
                    {
                        size -= 0.1f;
                    }
                    else
                    {
                        size += 0.1f;
                    }
                    if (changeBack && size > 10)
                    {

                        changeChamera = false;
                        stopchangeCamera = true;
                    }
                }
                if (size < 5.2f)
                {
                    changeBack = true;
                }

                camera.GetComponent<Camera>().orthographicSize = size;
                GetComponent<Move>().speed = 0.3f*GetComponent<Move>().speedStandart;

            }
            else
            {
                size = 10;
                background.GetComponent<SpriteRenderer>().color = Color.white;
                changeBack = false;
                GetComponent<Move>().speed = GetComponent<Move>().speedStandart;
                camera.GetComponent<Camera>().orthographicSize = size;
            }
            // To Special:
            if (SpecialCount < 99)
            {
                specialReady = false;
            }
            if (Time.time - startTime > 0.1 && !specialReady)
            {
                startTime = Time.time;
                SpecialCount++;
                if (SpecialCount > 99)
                {
                    specialReady = true;
                }
            }

            //To life
            bar.localScale = new Vector3(life / 100, 1f, 1f);


            barReference.rotation = Quaternion.Euler(0, 0, 0);
             barReference.position = camera.transform.position + offset;
            barReference.localScale = new Vector3(2, 0.6f, 5);
            // barReference.position = new Vector3(22,-5,10);
            // barReference.localScale = new Vector3(2.5f, 1, 5);

            //To Special 
            barSpecial.localScale = new Vector3(SpecialCount / 100, 1f, 1f);


            barReferenceSpecial.rotation = Quaternion.Euler(0, 0, 0);
            barReferenceSpecial.position = camera.transform.position + offset;
             barReferenceSpecial.localScale = new Vector3(2, 0.6f, 5);
            //barReferenceSpecial.position = new Vector3(22, -5, 10);
            //barReferenceSpecial.localScale = new Vector3(2.5f, 1, 5);
        }
        else
        {
            //bar.localScale = new Vector3(otherLife / 100, 1f, 1f);
            // barReference.position = camera.GetComponent<Transform>().position + offset;
            barReference.localScale = new Vector3(2, 0.6f, 5);
            bar.localScale = selfLocalScale;
            // barReference.position = selfPosition;
            if (PhotonNetwork.isMasterClient)
            {
                offset = new Vector3(30, 0, 1);
                
            }
            else
            {
                offset = new Vector3(30, 0, 1);
            }
            barReference.position = camera.transform.position + offset; // we only change offset!
            barReference.rotation = Quaternion.Euler(0, 0, 0);

            //To Special 
            barReferenceSpecial.localScale = new Vector3(2, 0.6f, 5);
            barSpecial.localScale = selfLocalScaleSpecial;
            barReferenceSpecial.rotation = Quaternion.Euler(0, 0, 0);

            //barReferenceSpecial.position = selfPositionSpecial;
            barReferenceSpecial.position = camera.transform.position + offset;
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo Info)
    {
        if (stream.isWriting)
        {
            //stream.SendNext(Life);
            stream.SendNext(bar.localScale);
            stream.SendNext(barReference.position);

            stream.SendNext(barSpecial.localScale);
            stream.SendNext(barReferenceSpecial.position);
        }
        else
        {
            //otherLife = (float)stream.ReceiveNext();
            selfLocalScale = (Vector3)stream.ReceiveNext();
            selfPosition = (Vector3)stream.ReceiveNext();

            selfLocalScaleSpecial = (Vector3)stream.ReceiveNext();
            selfPositionSpecial = (Vector3)stream.ReceiveNext();

        }
    }
}
