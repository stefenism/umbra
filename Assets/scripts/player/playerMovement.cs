using UnityEngine;

public class playerMovement : MonoBehaviour {
    
    private Rigidbody2D rb;
    private float horizontalMovement;

    public float runSpeed = 2;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        checkInput();
    }

    private void FixedUpdate() {
        run();
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