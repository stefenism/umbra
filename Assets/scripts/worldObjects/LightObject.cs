using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightObject : MonoBehaviour {

    private Light2D lightObj;
    public bool lightOn = true;
    public LayerMask ignoreMask;

    void Awake() {
        lightObj = GetComponent<Light2D>();
        if (!lightOn) {
            lightObj.enabled = false;
        }
        int enLayer = LayerMask.GetMask("Enemy");
        int lightLayer = LayerMask.GetMask("Glass");
        //var layermask1 = 1 << enLayer;
        //var layermask2 = 1 << lightLayer;
        ignoreMask = enLayer | lightLayer;
        //ignoreMask = LayerMask.GetMask("Enemy");
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
            hit = Physics2D.Raycast(lightObj.gameObject.transform.position, (player.transform.position - lightObj.gameObject.transform.position), (float)GetOuterRadius(), ~ignoreMask);
            if (hit.collider != null && hit.collider.gameObject.tag == "Player") { //Hit GameObject
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
