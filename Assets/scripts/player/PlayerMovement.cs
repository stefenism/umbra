using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody2D rb;

    public GameObject tallBoy;
    public GameObject ballBoy;
    public GameObject headSetPosition;
    public bool StartAsBall;

    public Animator anim;
    public BoxCollider2D boxCollider;

    private float horizontalMovement;
    private float verticalMovement;
    public PlayerStateManager playerState;
    public Player playerBrain;

    [Header("shadow flight")]
    public float flightSpeed = 3f;
    public float flightAccel = .1f;
    public float flightDecel = .075f;
    public float maxFlightSpeed = 6;

    [Header("glide (light mode) ")]
    public float verticalGlideSpeed = .5f;
    public float horizontalGlideSpeed = 2;
    public float glideAccel = .075f;
    public float glideDecel = 0.075f;
    public float maxVerticalGlideSpeed = 6;
    public float maxHorizontalGlideSpeed = 6;
    public float glideGravityModifier = 1.5f;

    [Header("horizontal movement")]
    public float runSpeed = 2;
    private bool facingRight = true;

    [Header("Jumping")]
    public bool grounded;
    private bool jumping = false;
    private bool jumpAllowed = true;
    private bool canJump = true;

    public float jumpForce = 3;
    private float jumpDuration = 0;
    public float jumpTime = .3f;
    public float airDrag = 1;
    public float gravityDropModifier = 2;
    public float airSpeed = 2f;

    public List<Vector3> hitPositions = new List<Vector3>();
    public Animator ballAnimator;
    public Animator tallAnimator;
    private bool killControls;

    public GameObject deadGuy;
    public GameObject grabMan;

    public AudioSource audioSource;

    public AudioClip GrabEnemySound;
    public AudioClip DeathSound; //Not implemented
    public AudioClip SwitchToTallSound;
    public AudioClip SwitchToBallSound;
    public AudioClip MoveSound; //Not implemented

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        playerState = GetComponent<PlayerStateManager>();
        playerBrain = GetComponent<Player>();
        if (!StartAsBall) {
            rb = tallBoy.GetComponent<Rigidbody2D>();
            playerState.SetPlayerInLight();
            playerState.usingState = PlayerStateManager.UsingState.TALLBOY;
        } else {
            rb = ballBoy.GetComponent<Rigidbody2D>();
            ballBoy.SetActive(true);
            tallBoy.SetActive(false);
        }

        
        anim = GetComponent<Animator>();
        boxCollider = ballBoy.GetComponent<BoxCollider2D>();
    }

    private void Start() {
        GameManager.gameDaddy.player = this;
        GameManager.gameDaddy.playerBrain = playerBrain;
    }

    private void Update() {

        if(playerState.IsPlayerDead()) {
            return;
        }
        
        checkInput();

        checkLightGrounded();
        checkGrappled();

        if(Input.GetKeyDown("r")) {
            GameManager.ReloadCurrentScene();
        }
    }

    private void FixedUpdate() {
        //Debug.Log("kill controls is:" + killControls);
        if (killControls) {
            return;
        }
        //Debug.Log("inside fixed update");
        if (playerState.IsPlayerOnGround() || playerState.IsPlayerGrappling()) {
            Debug.Log("inside fixed update run");
            run();

            if(!playerState.IsPlayerGrappling()){
                if (jumping) {
                    Jump();
                }
            }

            if (!grounded && jumping) {
                jumpDuration += Time.fixedDeltaTime;
            }

            if (grounded) {
                jumpDuration = 0;
            }
        }

        if (playerState.IsPlayerInLight()) {
            glide();
        }

        if (playerState.IsPlayerInDark()) {
            fly();
        }
    }

    private void checkInput() {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        if (grounded) {
            // setGravityScale(1);
            jumping = false;
            canJump = true;
            DetermineJumpButton();
        }

       // Debug.Log("is player grappling: " + playerState.IsPlayerGrappling());

        if(!playerState.IsPlayerGrappling()){
            if(rb.velocity.x > 0.1f && !facingRight) {
                Flip();
            }
            else if(rb.velocity.x < -0.1f && facingRight) {
                Flip();
            }
        }

        JumpButton();
    }

    private void checkLightGrounded() {
        if (playerState.IsPlayerInLight() && grounded && !playerState.IsPlayerGrappling()) {
            Debug.Log("setting player on ground");
            playerState.SetPlayerOnGround();
        }
    }

    private void checkGrappled() {
        if(playerBrain.getGrappledEnemy() != null) {
            playerState.SetPlayerGrappling();
        }
    }

    private void run() {
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = horizontalMovement * runSpeed;
        rb.velocity = newVelocity;
        if (newVelocity.x > 0 || 0 > newVelocity.x) {
            tallAnimator.SetBool("Walking", true);
            if (playerState.HasEnemeyGrappled) {
                tallAnimator.SetBool("WalkingBackward", true);
            }
        } else {
            tallAnimator.SetBool("Walking", false);
            tallAnimator.SetBool("WalkingBackward", false);
        }
    }

    private void fly() {
        Vector2 newVelocity = rb.velocity;

        // if you're moving
        if (horizontalMovement != 0) {

            if (Mathf.Abs(rb.velocity.x) <= maxFlightSpeed) {
                newVelocity.x += (horizontalMovement * (flightSpeed * flightAccel));
            } else {
                newVelocity.x -= rb.velocity.x * flightDecel;
            }
        }

        if (verticalMovement != 0) {

            if (Mathf.Abs(rb.velocity.y) <= maxFlightSpeed) {
                newVelocity.y += (verticalMovement * (flightSpeed * flightAccel));
            } else {
                newVelocity.y -= rb.velocity.y * flightDecel;
            }
        }

        // input has stopped decelerate
        if (horizontalMovement == 0) {
            if (Mathf.Abs(newVelocity.x) > 0) {
                newVelocity.x -= rb.velocity.x * flightDecel;
            }
        }

        if (verticalMovement == 0) {
            if (Mathf.Abs(newVelocity.y) > 0) {
                newVelocity.y -= rb.velocity.y * flightDecel;
            }
        }

        rb.velocity = newVelocity;
    }

    // this could be fly with different values passed in but...game jam
    private void glide() {
        Vector2 newVelocity = rb.velocity;

        if (horizontalMovement != 0) {

            if (Mathf.Abs(rb.velocity.x) <= maxHorizontalGlideSpeed) {
                newVelocity.x += (horizontalMovement * (horizontalGlideSpeed * glideAccel));
            } else {
                newVelocity.x -= rb.velocity.x * glideDecel;
            }
        }

        if (verticalMovement != 0) {
            if (rb.velocity.y <= 0 && Mathf.Abs(rb.velocity.y) < maxVerticalGlideSpeed) {
                newVelocity.y += (verticalMovement * (verticalGlideSpeed * glideAccel));
            } else {
                newVelocity.y -= rb.velocity.y * glideDecel;
            }
        }

        if (horizontalMovement == 0) {
            if (Mathf.Abs(newVelocity.x) > 0) {
                newVelocity.x -= rb.velocity.x * glideDecel;
            }
        }

        rb.velocity = newVelocity;
    }

    public void setDarknessMode() {
        rb.gravityScale = 0;
        SwitchToBall();
    }

    public void setLightMode() {
        rb.gravityScale = 1;
    }

    public void setGroundMode() {
        rb.gravityScale = 1;
        SwitchToTallBoy();
    }

    public void setGravityScale(float newScale) { rb.gravityScale = newScale; }

    public List<Vector3> getPlayerHitPositions() {
        if (playerState.usingState == PlayerStateManager.UsingState.BALL) {
            getHitPositions(ballBoy.transform.position);
            return hitPositions;
        } else {
            getHitPositions(tallBoy.transform.position);
            return hitPositions;
        }
    }

    public void getHitPositions(Vector3 position) {
        hitPositions.Clear();
        hitPositions.Add(new Vector3(position.x + boxCollider.bounds.extents.x, position.y, position.z));
        hitPositions.Add(new Vector3(position.x - boxCollider.bounds.extents.x, position.y, position.z));
        hitPositions.Add(position);
        string result = string.Join(",", hitPositions);
    }

    void DetermineJumpButton() {
        if (grounded && !Input.GetButton("Jump")) {
            jumpAllowed = true;
        }
    }

    void JumpButton() {
        if (!Input.GetButton("Jump")) {
            if (rb.velocity.y >= 0) {
                Vector2 dragForce = rb.velocity;
                dragForce.y = rb.velocity.y * airDrag;

                rb.velocity = dragForce;

                if (!grounded) {
                    jumpDuration = jumpTime;
                }
            }
        }

        if (Input.GetButton("Jump") && jumpAllowed) {
            jumping = true;
            canJump = false;
        }

        if (jumpDuration >= jumpTime) {
            jumping = false;
            jumpAllowed = false;

            if (rb.velocity.y < 0 && !playerState.IsPlayerInDark() && !playerState.IsPlayerInLight()) {
                setGravityScale(gravityDropModifier);
            }

            if (rb.velocity.y < 0 && playerState.IsPlayerInLight()) {
                setGravityScale(glideGravityModifier);
            }
        }

        if(!grounded) {
            jumpAllowed = false;
        }
    }

    void Jump() {
        if (playerState.HasEnemeyGrappled) { //Can't jump if enemy in hands
            return;
        }
        Vector2 jumpVector = rb.velocity;
        jumpVector.y = jumpForce;

        if (jumpDuration < jumpTime) {
            rb.velocity = jumpVector;
        }
    }

    void Flip() {
        facingRight = !facingRight;
        //tallAnimator.SetBool("Turn", true);
        Transform currentObject = GetUsedStateObject().transform;
        Vector3 theScale = currentObject.transform.localScale;
        theScale.x *= -1;
        currentObject.transform.localScale = theScale;
    }

    public void FinishFlip() { //Don't delete
    }

    public void SwitchToTallBoy() {
        if (!tallBoy.activeSelf) {
            rb = tallBoy.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
            ballBoy.SetActive(false);
            tallBoy.transform.position = ballBoy.transform.position;
            tallBoy.SetActive(true);
            playerState.usingState = PlayerStateManager.UsingState.TALLBOY;
            boxCollider = tallBoy.GetComponent<BoxCollider2D>();

            Vector3 theScale = tallBoy.transform.localScale;
            theScale.x = facingRight ? 1 : -1;
            tallBoy.transform.localScale = theScale;

            StopPlayerInput();
            tallAnimator.Rebind();
            tallAnimator.Update(0f);
            PlaySound(SwitchToTallSound, .5f);
        }
    }

    public void SwitchToBall() {
        if (!ballBoy.activeSelf) {
            Vector3 theScale = ballBoy.transform.localScale;
            theScale.x = facingRight ? 1 : -1;
            ballBoy.transform.localScale = theScale;
            tallAnimator.SetBool("Walking", false);
            tallAnimator.SetBool("WalkingBackward", false);
            ballBoy.transform.position = tallBoy.transform.position;
            tallAnimator.SetBool("Fly", true);
            PlaySound(SwitchToBallSound, .5f);
            rb.velocity = Vector3.zero;
            killControls = true;
            if(playerBrain.getGrappledEnemy() != null){
                explodeEnemy(playerBrain.getGrappledEnemy());
            }
        }
    }

    public void explodeEnemy(Enemy enemy) {
        // GameObject grabManObject = Instantiate(grabMan, transform.position, transform.rotation);
        // GrabManScript grabManScript = grabmanObject.GetComponent<GrabManScript>();

        if(tallBoy.activeSelf) {
            ballBoy.transform.position = tallBoy.transform.position;
        }
        GrabManScript grabManScript = Instantiate(grabMan, transform.position, transform.rotation).GetComponent<GrabManScript>();
        grabManScript.manPosition = enemy.transform.position;
        grabManScript.deathPoint = tallBoy.activeSelf ? tallBoy.transform.position : ballBoy.transform.position;
        grabManScript.flipMan = facingRight ? true : false;
        playerBrain.killEnemy(enemy);
    }

    public Vector3 currentEnemyTarget() {
        return GetUsedStateObject().transform.position;
    }

    public void FinishSwitchToBall() {
        killControls = false;
        rb = ballBoy.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        tallBoy.SetActive(false);
        ballBoy.SetActive(true);
        playerState.usingState = PlayerStateManager.UsingState.BALL;
        boxCollider = ballBoy.GetComponent<BoxCollider2D>();
    }

    public GameObject GetUsedStateObject() {
        if (playerState.usingState == PlayerStateManager.UsingState.BALL) {
            return ballBoy;
        } else {
            return tallBoy;
        }
    }

    public void StopPlayerInput() {
        killControls = true;
    }

    public void ContinuePlayerInput() {
        killControls = false;
    }

    public void PlaySound(AudioClip clip, float VolumeScale) {
        audioSource.PlayOneShot(clip, VolumeScale);
    }
}