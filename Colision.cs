using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision : MonoBehaviour
{ 
    public bool _TargetFounded = false;

    public GameObject Baguette;
    public GameObject player;
    public GameObject playeratacked;


    void OnCollisionEnter2D(Collision2D col)
    {
        var comp = col.gameObject.GetComponent<Move>();
        if (comp && col.gameObject != player)
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
}
