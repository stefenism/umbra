using UnityEngine;

public class PlayerStateManager : MonoBehaviour {
    
    private enum PlayerState {
        LIGHT,
        SHADOW,
        GROUND,
        DEAD
    }

    public bool HasEnemeyGrappled;

    private PlayerState playerState = PlayerState.LIGHT;
    private PlayerMovement mover;

    private void Awake(){
        mover = GetComponent<PlayerMovement>();
    }

    public void InitializeState(PlayerMovement currentPlayer) {
        mover = currentPlayer;
    }

    public void toggleMode(){
        if(IsPlayerInLight()){
            SetPlayerOnGround();
        } else if(IsPlayerInDark()){
            SetPlayerInLight();
        } else if(IsPlayerOnGround()){
            SetPlayerInDark();
        }
    }

    public void SetPlayerInDark(){
        Debug.Log("Player set to dark mode");
        playerState = PlayerState.SHADOW;
        mover.setDarknessMode();
    }

    public void SetPlayerInLight(){
        Debug.Log("Player set to light mode.");
        playerState = PlayerState.LIGHT;
        mover.setLightMode();
    }

    public void SetPlayerOnGround(){
        playerState = PlayerState.GROUND;
        mover.setGroundMode();
    }

    public bool IsPlayerInLight(){return playerState == PlayerState.LIGHT;}
    public bool IsPlayerInDark(){return playerState == PlayerState.SHADOW;}
    public bool IsPlayerOnGround(){return playerState == PlayerState.GROUND;}
    public bool IsPlayerDead(){return playerState == PlayerState.DEAD;}
}