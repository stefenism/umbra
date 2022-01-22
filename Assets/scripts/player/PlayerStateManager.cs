using UnityEngine;

public class PlayerStateManager : MonoBehaviour {
    
    private enum PlayerState {
        LIGHT,
        SHADOW,
        DEAD
    }

    private PlayerState playerState = PlayerState.LIGHT;
    private PlayerMovement mover;

    public void InitializeState(PlayerMovement currentPlayer) {
        mover = currentPlayer;
    }

    public bool IsPlayerInLight(){return playerState == PlayerState.LIGHT;}
    public bool IsPlayerInDark(){return playerState == PlayerState.SHADOW;}
    public bool IsPlayerDead(){return playerState == PlayerState.DEAD;}
}