using UnityEngine;

public class PlayerStateManager : MonoBehaviour {

    private enum PlayerState {
        LIGHT,
        SHADOW,
        GROUND,
        DEAD
    }

    public enum UsingState {
        BALL,
        TALLBOY
    }

    public bool HasEnemeyGrappled;

    private PlayerState playerState = PlayerState.LIGHT;
    public UsingState usingState = UsingState.BALL;
    private PlayerMovement mover;

    private void Awake() {
        mover = GetComponent<PlayerMovement>();
    }

    public void InitializeState(PlayerMovement currentPlayer) {
        mover = currentPlayer;
    }

    public void toggleMode() {
        if (IsPlayerInLight()) {
            SetPlayerOnGround();
        } else if (IsPlayerInDark()) {
            SetPlayerInLight();
        } else if (IsPlayerOnGround()) {
            SetPlayerInDark();
        }
    }

    public void SetPlayerInDark(){
        playerState = PlayerState.SHADOW;
        mover.setDarknessMode();
    }

    public void SetPlayerInLight() {
        playerState = PlayerState.LIGHT;
        mover.setLightMode();
    }

    public void SetPlayerOnGround() {
        playerState = PlayerState.GROUND;
        mover.setGroundMode();
    }

    public bool IsPlayerInLight() { return playerState == PlayerState.LIGHT; }
    public bool IsPlayerInDark() { return playerState == PlayerState.SHADOW; }
    public bool IsPlayerOnGround() { return playerState == PlayerState.GROUND; }
    public bool IsPlayerDead() { return playerState == PlayerState.DEAD; }

}