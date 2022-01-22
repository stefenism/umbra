using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightObject : MonoBehaviour {

    private Light2D lightObj;
    public bool lightOn = true;

    void Awake() {
        lightObj = GetComponent<Light2D>();
        if (!lightOn) {
            lightObj.enabled = false;
        }
    }

    public double GetInnerRaidus() {
        return lightObj.pointLightInnerRadius;
    }

    public double GetOuterRadius() {
        return lightObj.pointLightOuterRadius;
    }

    public double GetDistanceFromGameObject(GameObject player) {
        return Vector2.Distance(lightObj.gameObject.transform.position, player.transform.position);
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
        if (!lightOn) 
            return false;
        RaycastHit2D hit;
        if (GetDistanceFromGameObject(player) < GetOuterRadius()) {
            float angle = Vector2.Angle(lightObj.gameObject.transform.up, player.gameObject.transform.position);
           // if (Input.GetKeyDown("q")) {
                //Debug.Log(angle);
            //}
            hit = Physics2D.Raycast(lightObj.gameObject.transform.position, (player.transform.position - lightObj.gameObject.transform.position), (float)GetOuterRadius());
            if (hit.collider != null && hit.collider.gameObject == player) { //Hit GameObject
                return true;
            }
        }
        return false;
    }


    public Vector2 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
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
        lightObj.enabled = lightOn;
    }

}
