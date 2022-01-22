using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObject : MonoBehaviour {

    private Light light; //Change to Light2D
    private bool lightOn;
    void Awake() {
        light = GetComponent<Light>(); //Needs to be changed to Light2D
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
        return distance - GetOuterRadius() < 0 && distance - GetInnerRaidus() > 0;
    }

    public bool IsGameObjectInInnerLight(GameObject player) {
        return GetDistanceFromGameObject(player) - GetInnerRaidus() < 0;
    }

    //Returns true if ray cast could hit the player
    public bool IsGameObjectWithinLight(GameObject player) {
        RaycastHit2D hit;
        if (GetDistanceFromGameObject(player) < GetOuterRadius()) {
            hit = Physics2D.Raycast(light.gameObject.transform.position, (player.transform.position - light.gameObject.transform.position), (float)GetOuterRadius());
            if (hit.collider != null && hit.collider == player) { //Hit GameObject
                return true;
            }
        }
        return false;
    }

    public bool IsLightOn() {
        return lightOn;
    }

    public bool ToggleLight() {
        lightOn = !lightOn;
        UpdateLight();
        return lightOn;
    }
    

    public void UpdateLight() {
        light.enabled = lightOn;
    }

}
