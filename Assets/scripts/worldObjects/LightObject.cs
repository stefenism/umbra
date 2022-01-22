using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObject : MonoBehaviour
{

    private Light light;

    private void Awake() {
        light = GetComponent<Light>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public double GetInnerRaidus() {
        return 0;
        //return light.point
    }

    public double GetOuterRadius() {
        return 0;
        //return light.GetOuterRadius();
    }

    public double GetDistanceFromGameObject(GameObject player) {
        return Vector2.Distance(light.gameObject.transform.position, player.transform.position);
    }

    public bool IsGameObjectInOuterLight(GameObject player) {
        var distance = GetDistanceFromGameObject(player);
        return  distance - GetOuterRadius() < 0 && distance - GetInnerRaidus() > 0;
    }

    public bool IsGameObjectInInnerLight(GameObject player) {
        return GetDistanceFromGameObject(player) - GetInnerRaidus() < 0;
    }

    public bool IsGameObjectInLight(GameObject player) {
        return IsGameObjectInOuterLight(player) || IsGameObjectInInnerLight(player);
    }

    //Returns true if ray cast could hit the player
    public bool RaytraceToGameObject(GameObject player) {
        float maxRange = 5;
        RaycastHit2D hit;
        if (GetDistanceFromGameObject(player) < GetOuterRadius()) {
            //if (Physics.Raycast(light.gameObject.transform.position, (player.transform.position - light.gameObject.transform.position), out hit, 5f)) {

            //}
            return true;
        }

        return false;
    }

}
