using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBaguette_Move : Photon.MonoBehaviour
{
    private Vector3 target;
    // Start is called before the first frame update
    // Update is called once per frame
    private float kfac = 1;
    private float startTime;
    public GameObject SpecialBaguette;
    private bool isArrived = false;
    private float speedBaguette = 7;

    //to multiplayer
    private Vector3 selfPos, selScale;

    //To Colision
    public bool _TargetFounded = false;
    public GameObject Baguette;
    public GameObject owner;
    public GameObject playeratacked;

    public bool changeCamera = false;



    void Start ()
    {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GetComponent<Animator>().SetBool("atackSpecial", true);
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

    }
    void Update()
    {
        if (photonView.isMine)
        {
            if (kfac < 1.6)
            {
                kfac += 0.001f;
                transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
                speedBaguette += 0.01f;
            }
            if (transform.position == target && !isArrived)
            {
                isArrived = true;
                startTime = Time.time;
            }
            if (Time.time - startTime > 1 && isArrived)
            {
                PhotonNetwork.Destroy(SpecialBaguette);
            }
            target.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, target, speedBaguette * Time.deltaTime);

            if (_TargetFounded)
            {
                print("teoricamente mudei");
                playeratacked.GetComponent<DetectAtack>().photonView.RPC("ApplyDamage", PhotonTargets.All);
                if (!changeCamera)
                {
                    changeCamera = true;
                    playeratacked.GetComponent<DetectAtack>().photonView.RPC("ChangeCameraBaguette", PhotonTargets.All);
                }
                
            }
        }
        else
        {
            transform.position = selfPos;
            transform.localScale = selScale;
        }
    }



    void OnCollisionEnter2D(Collision2D col)
    {
        var comp = col.gameObject.GetComponent<Move>();
        if (comp && col.gameObject != owner)
        {
            _TargetFounded = true;
            playeratacked = col.gameObject;

        }
        //I'm trying to avoid using tag's because they're strings !
        // if (col.gameObject.tag == "Player" && col.gameObject != player){   _TargetFounded = true;  } 
    }
    void OnCollisionExit2D(Collision2D col)
    {
        var comp = col.gameObject.GetComponent<Move>();
        if (comp)
        {
            _TargetFounded = false;
        }
    }





    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo Info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
            selScale = (Vector3)stream.ReceiveNext();
        }
    }

}
