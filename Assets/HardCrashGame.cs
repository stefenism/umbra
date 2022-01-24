using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardCrashGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("HardCrash", 20f);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HardCrash()
    {
        Application.Quit();
    }
}
