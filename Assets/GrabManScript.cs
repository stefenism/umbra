using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabManScript : MonoBehaviour
{

    public Vector3 manPosition;
    public bool flipMan;
    public Vector3 deathPoint;


    public GameObject fakeMan;
    public GameObject trail;

    public GameObject deadGuyPrefab;

    bool travelingToMan = true;
    float alpha = 0;
    bool done = false;

    // Start is called before the first frame update
    void Start()
    {
        fakeMan.transform.position = manPosition;
        if (flipMan)
        {
            fakeMan.GetComponent<SpriteRenderer>().flipX = true;
        }
        trail.transform.position = deathPoint;
        trail.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (done)
        {
            return;
        }
        if (travelingToMan)
        {
            alpha += Time.deltaTime*3;
            trail.transform.position = Vector3.Lerp(deathPoint, manPosition, alpha);
            if (alpha >= 1)
            {
                travelingToMan = false;
            }
        }
        else
        {
            alpha -= Time.deltaTime*2;
            trail.transform.position = Vector3.Lerp(deathPoint, manPosition, alpha);
            fakeMan.transform.position = Vector3.Lerp(deathPoint, manPosition, alpha);
            if (alpha <= 0)
            {
                Instantiate(deadGuyPrefab, fakeMan.transform.position, Quaternion.identity, null);
                done = true;
                Destroy(fakeMan);
                Invoke("DestroySelf", 0.5f);
            }
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
