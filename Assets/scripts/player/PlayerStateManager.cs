using UnityEngine;

public class PlayerStateManager : MonoBehaviour {
    
    private enum PlayerState {
        LIGHT,
        SHADOW,
        DEAD
    }

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
            SetPlayerInDark();
        } else if(IsPlayerInDark()){
            SetPlayerInLight();
        }
    }

    public void SetPlayerInDark(){
        playerState = PlayerState.SHADOW;
        mover.setDarknessMode();
    }

    public void SetPlayerInLight(){
        playerState = PlayerState.LIGHT;
        mover.setLightMode();
    }

    public bool IsPlayerInLight(){return playerState == PlayerState.LIGHT;}
    public bool IsPlayerInDark(){return playerState == PlayerState.SHADOW;}
    public bool IsPlayerDead(){return playerState == PlayerState.DEAD;}
}