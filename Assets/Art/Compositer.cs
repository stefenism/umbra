using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Compositer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TilemapCollider2D>().usedByComposite = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
