using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DestroyShapes : MonoBehaviour
{
    public int countDelete;
    private GameObject obj;
    private static bool DestroyNext = true;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        countDelete = DrawController.countDelete;
        obj = GameObject.Find("Shape " + countDelete);
        if (DestroyNext && obj)
        {
            DestroyNext = false; // We need to wait a time to destroy the next
            DestroyAfterTime();
        }


    }

    public void DestroyAfterTime()
    {
        StartCoroutine(ExecuteAfterTime(3));
      
        
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
        DestroyNext = true; 
        
    }
}
