using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    public Rigidbody2D rb;
    private float horizontalMovement;
    private float verticalMovement;
    public PlayerStateManager playerState;

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
        rb = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerStateManager>();
    }

    private void Update() {
        checkInput();

        if(Input.GetKeyDown("b")){
            playerState.toggleMode();
        }
    }

    private void FixedUpdate() {
        if(playerState.IsPlayerInLight()){
            run();

            if(jumping){
                Jump();
            }

            if(!grounded && jumping){
                jumpDuration += Time.fixedDeltaTime;
            }

            if(grounded){
                jumpDuration = 0;
            }
        }

        if(playerState.IsPlayerInDark()){
            fly();
        }
    }

    private void checkInput() {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        if(grounded){
            // setGravityScale(1);
            jumping = false;
            canJump = true;
            DetermineJumpButton();
        }

        JumpButton();
    }

    private void run() {
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = horizontalMovement * runSpeed;
        rb.velocity = newVelocity;
    }

    private void fly() {
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = horizontalMovement * runSpeed;
        newVelocity.y = verticalMovement * runSpeed;
        rb.velocity = newVelocity;
    }

    public void setDarknessMode(){
        rb.gravityScale = 0;
    }

    public void setLightMode(){
        rb.gravityScale = 1;
    }

    public void setGravityScale(float newScale){rb.gravityScale = newScale;}

    void DetermineJumpButton() {
        if( grounded && !Input.GetButton("Jump")) {
            jumpAllowed = true;
        }
    }

    void JumpButton () {
        if(!Input.GetButton("Jump")) {
            if(rb.velocity.y >= 0){
                Vector2 dragForce = rb.velocity;
                dragForce.y = rb.velocity.y * airDrag;

                rb.velocity = dragForce;

                if(!grounded ){
                    jumpDuration = jumpTime;
                }
            }
        }

        if(Input.GetButton("Jump") && jumpAllowed){
            jumping = true;
            canJump = false;
        }

        if(jumpDuration >= jumpTime){
            jumping = false;
            jumpAllowed = false;

            if(rb.velocity.y < 0 ){
                setGravityScale(gravityDropModifier);
            }
        }
    }

    void Jump() {
        Debug.Log("jumping now");
        Vector2 jumpVector = rb.velocity;
        jumpVector.y = jumpForce;

        if(jumpDuration < jumpTime){
            rb.velocity = jumpVector;
        }
    }
}