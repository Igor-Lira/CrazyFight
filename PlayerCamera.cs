using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject camera;
    public GameObject target;
    private Vector3 offset;
    private Vector3 HeightPos = new Vector3((float)0, (float)0, (float)0);

    // Use this for initialization
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.transform.position + offset + HeightPos;
        }
    }
}

