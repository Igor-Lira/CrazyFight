using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photonHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Awakwe()
    {
        DontDestroyOnLoad(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
