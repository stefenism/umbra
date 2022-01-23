using UnityEngine;

public class GrappleTrigger : MonoBehaviour {
    
    private BoxCollider2D trigger;
    private Player playerBrain;

    private void Start() {
        playerBrain = GameManager.gameDaddy.playerBrain;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.TryGetComponent<Enemy>(out Enemy enemy)){
            if(GameManager.gameDaddy.player.playerState.IsPlayerOnGround()){
                Enemy grappledEnemy = enemy;
                grappledEnemy.getGrappled(playerBrain);
                playerBrain.doGrapple(enemy);
            } else if (GameManager.gameDaddy.player.playerState.IsPlayerInDark()){
                if(enemy.gameObject.activeSelf){
                    Enemy grappledEnemy = enemy;
                    playerBrain.setGrappledEnemy(grappledEnemy);
                    GameManager.gameDaddy.player.explodeEnemy(enemy);
                }
            }
        }
    }
}