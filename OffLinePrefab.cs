using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffLinePrefab : MonoBehaviour
{
    public GameObject playerOnline;

    void Start()
    {
        
    }
    void Update()
    {
        var myPosition = transform.position;
        var target = playerOnline.transform.position; // Gets the player ransform position !
        if (myPosition.x > target.x)
        {
            //  GetComponent<Move>().
            //if(Math.abs(myPosition.x - target.x) > 5){//try to do a down combo. A, W + 0.2 sec + 0.2 sec W + 0.2 sec W and + SSS
        }
        else
        {

        }
        if (myPosition.y > target.y)
        {

        }
        else
        {

        }
      //always atack when possible
      //Always ComboAtack:

    }
}
