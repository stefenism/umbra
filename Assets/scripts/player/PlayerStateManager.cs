using UnityEngine;

public class PlayerStateManager : MonoBehaviour {

    private enum PlayerState {
        LIGHT,
        SHADOW,
        GROUND,
        GRAPPLING,
        DEAD
    }

    public enum UsingState {
        BALL,
        TALLBOY
    }

    public bool HasEnemeyGrappled;

    [SerializeField]
    private PlayerState playerState = PlayerState.LIGHT;
    public UsingState usingState = UsingState.BALL;
    private PlayerMovement mover;
    private GroundDetect groundDetect;

    private void Awake() {
        mover = GetComponent<PlayerMovement>();
        groundDetect = GetComponent<GroundDetect>();
    }

    public void InitializeState(PlayerMovement currentPlayer) {
        mover = currentPlayer;
    }

    public void toggleMode(){
        Debug.Log("toggling mode");
        if(IsPlayerInLight()){
            SetPlayerOnGround();
        } else if (IsPlayerInDark()) {
            SetPlayerInLight();
        } else if (IsPlayerOnGround()) {
            SetPlayerInDark();
        }
    }

    public void SetPlayerInDark(){
        playerState = PlayerState.SHADOW;
        groundDetect.Initalize(mover.ballBoy.GetComponent<BoxCollider2D>(), mover.ballBoy.transform);
        groundDetect.groundDistance = .5f;
        mover.setDarknessMode();
    }

    public void SetPlayerInLight() {
        playerState = PlayerState.LIGHT;
        mover.setLightMode();
    }

    public void SetPlayerOnGround() {
        playerState = PlayerState.GROUND;
        groundDetect.Initalize(mover.tallBoy.GetComponent<BoxCollider2D>(), mover.tallBoy.transform);
        groundDetect.groundDistance = 1.1f;
        mover.setGroundMode();
    }

    public void SetPlayerGrappling() {
        playerState = PlayerState.GRAPPLING;
    }

    public void SetPlayerDead() {
        Debug.Log("player is dead");
        playerState = PlayerState.DEAD;
        mover.tallAnimator.SetBool("Dead", true);
    }

    public bool IsPlayerInLight() { return playerState == PlayerState.LIGHT; }
    public bool IsPlayerInDark() { return playerState == PlayerState.SHADOW; }
    public bool IsPlayerOnGround() { return playerState == PlayerState.GROUND; }
    public bool IsPlayerGrappling() { return playerState == PlayerState.GRAPPLING; }
    public bool IsPlayerDead() { return playerState == PlayerState.DEAD; }

}