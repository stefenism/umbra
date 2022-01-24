using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStars : MonoBehaviour
{
    public float speed = -0.002f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speed, 0, 0);
    }
}
