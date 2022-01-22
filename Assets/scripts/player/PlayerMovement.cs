using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    private Rigidbody2D rb;
    private float horizontalMovement;
    public PlayerStateManager playerState;

    public float runSpeed = 2;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerStateManager>();
    }

    private void Update() {
        checkInput();
    }

    private void FixedUpdate() {
        if(playerState.IsPlayerInLight()){
            run();
        }
    }

    private void checkInput() {
        horizontalMovement = Input.GetAxis("Horizontal");
    }

    private void run() {
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = horizontalMovement * runSpeed;
        rb.velocity = newVelocity;
    }
}