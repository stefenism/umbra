using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody2D rb;
    public GameObject tallBoy;
    public GameObject ballBoy;
    public bool StartAsBall;
    private float horizontalMovement;
    private float verticalMovement;
    public PlayerStateManager playerState;

    [Header("shadow flight")]
    public float flightSpeed = 1.5f;
    public float flightAccel = .1f;
    public float flightDecel = .075f;
    public float maxFlightSpeed = 3;

    [Header("glide (light mode) ")]
    public float verticalGlideSpeed = .5f;
    public float horizontalGlideSpeed = 2;
    public float glideAccel = .075f;
    public float glideDecel = 0.075f;
    public float maxVerticalGlideSpeed = 1;
    public float maxHorizontalGlideSpeed = 2;
    public float glideGravityModifier = .25f;

    [Header("horizontal movement")]
    public float runSpeed = 2;

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

    private void Awake() {
        if (StartAsBall) {
            rb = ballBoy.GetComponent<Rigidbody2D>();
            //tallBoy.SetActive(false);
        } else {
            rb = tallBoy.GetComponent<Rigidbody2D>();
            ballBoy.SetActive(false);
        }

        playerState = GetComponent<PlayerStateManager>();
    }

    private void Update() {
        checkInput();

        checkLightGrounded();

        if (Input.GetKeyDown("b")) {
            playerState.toggleMode();
        }
    }

    private void FixedUpdate() {
        if (playerState.IsPlayerOnGround()) {
            run();

            if (jumping) {
                Jump();
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

        JumpButton();
    }

    private void checkLightGrounded() {
        if (playerState.IsPlayerInLight() && grounded) {
            playerState.SetPlayerOnGround();
        }
    }

    private void run() {
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = horizontalMovement * runSpeed;
        if (!playerState.HasEnemeyGrappled) { //Once a enemy has been grappled, don't flip sprite anymore
            if (newVelocity.x < 0) {
                //Moving left, flip sprite
            } else if (newVelocity.x > 0) { //Don't fiip when 0
                //Moving 
            }
        }
        rb.velocity = newVelocity;
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
    }

    public void setLightMode() {
        rb.gravityScale = 1;
    }

    public void setGroundMode() {

    }

    public void setGravityScale(float newScale) { rb.gravityScale = newScale; }

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
    }

    void Jump() {
        if (playerState.HasEnemeyGrappled) { //Can't jump if enemy in hands
            return;
        }
        Debug.Log("jumping now");
        Vector2 jumpVector = rb.velocity;
        jumpVector.y = jumpForce;

        if (jumpDuration < jumpTime) {
            rb.velocity = jumpVector;
        }
    }

    public void SwitchToTallBoy() {
        rb = tallBoy.GetComponent<Rigidbody2D>();
        ballBoy.SetActive(false);
        playerState.usingState = PlayerStateManager.UsingState.TALLBOY;
    }

    public void SwitchToBall() {
        rb = ballBoy.GetComponent<Rigidbody2D>();
        tallBoy.SetActive(false);
        playerState.usingState = PlayerStateManager.UsingState.BALL;
    }

    public GameObject GetUsedStateObject() {
        if (playerState.usingState == PlayerStateManager.UsingState.BALL) {
            return ballBoy;
        } else {
            return tallBoy;
        }
    }
}