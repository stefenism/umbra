using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour {
    
    private enum EnemyGunState {
        IDLE,
        SHOOTING
    }

    EnemyGunState gunState = EnemyGunState.IDLE;

    Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
        setGunAtRandomAngle();
    }

    public void shootGun() {
        // anim.SetBool("Shooting", true);
        setGunShooting();
    }

    public void stopShootingGun() {
        //anim.SetBool("Shooting", false);
        setGunIdle();
        setGunAtRandomAngle();
    }

    public void setGunAtRandomAngle() {
        float angle = Random.Range(-45, 45);
        Quaternion q = Quaternion.AngleAxis( angle, Vector3.forward);
        transform.localRotation = q;
    }

    void setGunIdle(){gunState = EnemyGunState.IDLE;}
    void setGunShooting(){gunState = EnemyGunState.SHOOTING;}
    
    public bool isGunShooting(){return gunState == EnemyGunState.SHOOTING;}
}