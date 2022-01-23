using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Combatant
{

    private enum EnemyState {
        PATROLLING,
        SHOOTING,
        DEAD
    }

    EnemyState enemyState = EnemyState.PATROLLING;

    EnemyGun enemyGun;
    Animator anim;
    Animator gunAnim;

    public Transform bulletStartPosition;

    public bool facingRight = true;

    [Header("movement")]
    public float speed = 3;
    private bool arrived = false;
    private bool moving = true;

    [Header("shootin")]
    public Bullet bullet;
    public float aimSpeed = .5f;
    private LineRenderer bulletTracer;
    private Vector3 shootStartPoint;
    private RaycastHit2D bulletHit;
    private Vector3 target;
    public float groundCheckDistance = .5f;
    public LayerMask groundLayer;

    [Header("coroutines")]
    Coroutine doInstruction = null;

    private Rigidbody2D rb;

    public float MaxWanderRight;
    public float MaxWanderLeft;
    public bool LastWalkDistance; //True for left, false for right
    private Vector2 startingPosition;


    private float currentIdleTime = 0f;
    public float MaxIdleTime = 1f;
    public float MinIdleTime = .5f;
    private bool currentlyIdle = false;

    public void Start() {
        startingPosition = gameObject.transform.position;
        player = GameManager.gameDaddy.player.gameObject.transform;
        playerScript = GameManager.gameDaddy.player;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyGun = transform.GetChild(0).GetComponent<EnemyGun>();

        bulletTracer = GetComponentInChildren<LineRenderer>();
        bulletTracer.positionCount = 2;
        bulletTracer.enabled = false;
    }

    public override void Update() {
        Scan();
        setAnims();

        if(inRange) {
            checkGroundAngle();

            // if(enemyGun.isGunShooting()) {
                Shoot();
            // }
            target = playerScript.currentEnemyTarget();
            StopAllCoroutines();
            ShootTransition();
        } else {
            setEnemyPatrolling();
        }

        if(!(enemyState == EnemyState.SHOOTING)){
            checkFlip();
        }
    }

    void setAnims() {
        // set anims here
    }

    void checkFlip() {
        if(rb.velocity.x > 0 && !facingRight ) {
            Flip();
        } else if ( rb.velocity.x < 0 && facingRight ) {
            Flip();
        } else if ( rb.velocity.x == 0) {
            return;
        }
    }

    void checkFacing() {
        Vector3 theScale = transform.localScale;
        if( player.position.x > transform.position.x && !facingRight) {
            Flip();
        } else if ( player.position.x < transform.position.x && facingRight ) {
            Flip();
        }
    }

    public override void playerDetected() {
        if( enemyState == EnemyState.SHOOTING ) {
            return; 
        }

        // probably shoot right?
        // Shoot();
    }

    public override void Scan() {
        // base.Scan();

        Vector2 ray = new Vector2(transform.position.x, transform.position.y);

        playerHitPositions = playerScript.getPlayerHitPositions();

        foreach(Vector3 hitPosition in playerHitPositions){

            Vector3 lineOfSightVector = (hitPosition - (Mathf.Sign(transform.localRotation.x) * transform.position ));
            float faceOffset = (Mathf.Sign(transform.localScale.x) > 0) ? 0 : 180;
            float radAngle = Mathf.Sign(transform.localScale.x) * Mathf.Atan2(lineOfSightVector.y, lineOfSightVector.x);
            float lineOfSightAngle = radAngle * Mathf.Rad2Deg + (Mathf.Sign(transform.localRotation.x) * (Mathf.Sign(lineOfSightVector.y) * faceOffset));
            bool acceptableLineOfSight = lineOfSightAngle >= -45 && lineOfSightAngle <= 45;

            hit = Physics2D.Raycast(transform.position, hitPosition - transform.position, range, playerLayer);

            Ray2D hitRay = new Ray2D(transform.position, hitPosition - transform.position);

            Debug.DrawRay(transform.position, (hitPosition - transform.position).normalized * range, Color.red);

            if(hit.collider != null){
                if(hit.collider.gameObject.tag == "Player"){
                    if (GameManager.gameDaddy.player.playerState.IsPlayerInLight() || GameManager.gameDaddy.player.playerState.IsPlayerOnGround()){
                        if(acceptableLineOfSight){
                            if(!acceptableLineOfSight){
                                StopAllCoroutines();
                            }
                            inRange = true;
                            player = hit.collider.gameObject.transform;
                            break;
                        }
                    } else {
                        inRange = false;
                    }
                }

                else{
                    inRange = false;
                    }
            }
            else
            {
                inRange = false;
            }

                //testing shit
                // inRange = true;
        }


        cooldown += Time.deltaTime;

        if(cooldown <= fireRate){
            if(cooldown * 5f >= fireRate){
                bulletTracer.enabled = false;
            }
        }
    }

    void ShootTransition() {
        Debug.Log("gonna shoot");
        setEnemyShooting();
        //anim.SetBool("Shoot", true);
        checkFacing();
    }

    void check_hit() {
        if(bulletHit.collider == null) {
            return;
        }

        if( bulletHit.collider.gameObject.tag == "Enemy") {}
    }

    Vector2 find_end_point() {
        bulletHit = Physics2D.Raycast(enemyGun.transform.position, (enemyGun.transform.right * this.transform.localScale.x), range * 2, playerLayer);

        return bulletHit.point;
    }

    void checkGroundAngle(){
        float groundAngle = 0;
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, -transform.up, groundCheckDistance, groundLayer);
        Debug.DrawRay(transform.position, -transform.up,Color.red, 2);

        if(groundHit.collider != null){
            //get direction the ground is facing
            Vector3 groundFacingDirection = groundHit.normal;
            Debug.DrawRay(transform.position, -groundHit.normal, Color.yellow, 2f);
            //y axis up
            Vector3 verticalDirection = Vector3.up;

            //find angle between two directions
            //returns acute angle (0 to 180 degrees)
            groundAngle = Vector3.Angle(verticalDirection, groundFacingDirection);

            if(groundAngle < 60 && groundAngle > 20){
                rb.velocity = new Vector2(rb.velocity.x + (-groundHit.normal.x * .65f), rb.velocity.y);
                // rb.AddForce(-groundHit.normal * Physics.gravity.magnitude * 2, ForceMode2D.Impulse);
            }

        }
    }

    public override void Shoot() {

        moving = false;

        if (cooldown > fireRate) {
            cooldown = 0;

            shootStartPoint = bulletStartPosition.position - ((this.transform.right * this.transform.localScale.x )/2);
            bulletTracer.SetPosition(0, shootStartPoint);
            Vector2 endPosition = find_end_point();
            bulletTracer.SetPosition(1, endPosition);
            bulletTracer.enabled = true;

            check_hit();
        }

        Vector2 vectorToTarget = target - (Mathf.Sign(transform.localRotation.x) * enemyGun.gameObject.transform.position);
        float faceOffset = (Mathf.Sign(transform.localScale.x) > 0) ? 0 : 180;
        float radAngle = Mathf.Sign(transform.localScale.x) * Mathf.Atan2(vectorToTarget.y, vectorToTarget.x);
        float angle = radAngle * Mathf.Rad2Deg + (Mathf.Sign(transform.localRotation.x) * (Mathf.Sign(vectorToTarget.y) * faceOffset));
        angle = Mathf.Clamp(angle, -45f, 45f);

        Debug.Log("gonna rotate gun");

        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion rotation = Quaternion.Slerp(enemyGun.gameObject.transform.localRotation, q, Time.deltaTime * aimSpeed);
        Debug.Log("rotation is: " + rotation);
        enemyGun.transform.localRotation = rotation;
    }

    void Flip() {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }

    // public bool isEnemyShooting(){enemyState == EnemyState.SHOOTING;}

    public void setEnemyPatrolling(){enemyState = EnemyState.PATROLLING;}
    public void setEnemyShooting(){enemyState = EnemyState.SHOOTING;}
    public void setEnemyDead(){enemyState = EnemyState.DEAD;}

    public void AimAtPlayer() { //PLEASE FIX ME, I'M RELLY DUMBY
        //Vector2 direction = this.gameObject.transform.position - player.gameObject.transform.position;
        //this.gameObject.transform.rotation = Quaternion.LookRotation(direction);

        float orientTransform = transform.position.x;
        float orientTarget = playerScript.currentEnemyTarget().x;
        Quaternion newRotation;

        if (orientTransform > orientTarget) 
            newRotation = Quaternion.LookRotation(transform.position - playerScript.currentEnemyTarget(), -Vector3.up);
        else
            newRotation = Quaternion.LookRotation(transform.position - playerScript.currentEnemyTarget(), Vector3.up);

        newRotation.x = 0.0f;
        newRotation.y = 0.0f;

        transform.rotation = newRotation; // Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime);
        // transform.rotation = Quaternion.RotateTowards(transform.rotation,newRotation, Time.deltaTime * enemyAimSpeed);


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
        return Vector2.Distance(gameObject.transform.position, playerScript.currentEnemyTarget());
    }


    // public void UpdateLogic() {
    //     //AI Logic
    //     if (DistanceToPlayer() > EyeDistance) {
    //         Wander();
    //         return;
    //     }
    //     if (player.IsInLight()) {
    //         if (currentAimTIme < TimeToAim) {
    //             AimAtPlayer();
    //             currentAimTIme += Time.deltaTime;
    //             return;
    //         }
    //         Shoot();
    //         return;
    //     }
    //     Wander();
    // }


}
