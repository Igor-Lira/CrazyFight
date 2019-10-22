using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plMove : Photon.MonoBehaviour
{
    public bool devTesting = false;
    public PhotonView photonView;
    public float moveSpeed = 10f;
    public float jumpForce = 80f;
    private Vector3 selfPos;
    public static GameObject LocalPlayerInstance;

    void Awake ()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!devTesting)
        {
             if ( photonView.isMine)
            {
              checkInput();
            }
            else
            {
               smoothNetMovement();
            }
        }
       

    }
    private void checkInput()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0);
        transform.position += move * moveSpeed * Time.deltaTime;

    }
    private void smoothNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
    }
    private void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo Info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
        } else
        {
            selfPos = (Vector3)stream.ReceiveNext();
        }
    }

}
