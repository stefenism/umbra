using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Combatant
{
    public Bullet bullet;
    private Player player;
    public double EyeDistance = 10;
    public float TimeToAim = 5f;
    private float currentAimTIme = 0f;

    public float MaxWanderRight;
    public float MaxWanderLeft;
    public bool LastWalkDistance; //True for left, false for right
    private Vector2 startingPosition;

    public float WalkSpeed;

    private float currentIdleTime = 0f;
    public float MaxIdleTime = 1f;
    public float MinIdleTime = .5f;
    private bool currentlyIdle = false;

    private void Awake() {
        startingPosition = gameObject.transform.position;
        player = FindObjectOfType<Player>();
    }

    private void Update() {
        UpdateLogic();
    }

    public void AimAtPlayer() { //PLEASE FIX ME, I'M RELLY DUMBY
        //Vector2 direction = this.gameObject.transform.position - player.gameObject.transform.position;
        //this.gameObject.transform.rotation = Quaternion.LookRotation(direction);

        float orientTransform = transform.position.x;
        float orientTarget = player.transform.position.x;
        Quaternion newRotation;

        if (orientTransform > orientTarget) 
            newRotation = Quaternion.LookRotation(transform.position - player.transform.position, -Vector3.up);
        else
            newRotation = Quaternion.LookRotation(transform.position - player.transform.position, Vector3.up);

        newRotation.x = 0.0f;
        newRotation.y = 0.0f;

        transform.rotation = newRotation; // Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime);
        // transform.rotation = Quaternion.RotateTowards(transform.rotation,newRotation, Time.deltaTime * enemyAimSpeed);


    }

    public void Shoot() {
        Bullet bull = Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
    }

    public void Wander() {
        if (LastWalkDistance) {
            if ((startingPosition.x - gameObject.transform.position.x) >= MaxWanderLeft) {
                LastWalkDistance = false; //Go the other direction next frame
            } else {
                //Move forward by speed
            }
        } else {

        }
    }

    public void Idle() {

    }

    public double DistanceToPlayer() {
        return Vector2.Distance(gameObject.transform.position, player.transform.position);
    }


    public void UpdateLogic() {
        //AI Logic
        if (DistanceToPlayer() > EyeDistance) {
            Wander();
            return;
        }
        if (player.IsInLight()) {
            if (currentAimTIme < TimeToAim) {
                AimAtPlayer();
                currentAimTIme += Time.deltaTime;
                return;
            }
            Shoot();
            return;
        }
        Wander();
    }


}
