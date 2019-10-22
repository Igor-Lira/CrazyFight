using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaguettePosition : Photon.MonoBehaviour
{
    public Transform baguettePosition;
    public GameObject owner;
    public SpriteRenderer BaguetteSprite;
    private Vector3 selfPos, selScale;
    private float xPos;
    public int scaleRate;
    // Start is called before the first frame update
    void Start()
    {
         if (owner != null)
        {
            transform.localScale = 50*owner.transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (owner != null)
            {
            if (owner.GetComponent<Move>().flyingAnimator)
            {
                xPos = 1.3f;
            }
            else
            {
                xPos = 0.9f;
            }
                if (owner.transform.localScale.x < 0)
                {
                    transform.position = owner.transform.position + new Vector3(-xPos, -1.0f, 0);
                    transform.rotation = owner.transform.rotation;
                }
                else
                {
                    transform.position = owner.transform.position + new Vector3(xPos, -1.0f, 0);
                    transform.rotation = owner.transform.rotation;
                }
            }
            else Destroy(gameObject);
  
    }
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo Info)
    {
        if (stream.isWriting)
        {
          //  stream.SendNext(transform.localScale);
        }
        else
        {
           // selScale = (Vector3)stream.ReceiveNext();
        }
    }

}
   
  

