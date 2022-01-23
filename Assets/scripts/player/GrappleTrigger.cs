using UnityEngine;

public class GrappleTrigger : MonoBehaviour {
    
    private BoxCollider2D trigger;
    private Player playerBrain;

    private void Start() {
        playerBrain = GameManager.gameDaddy.playerBrain;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.TryGetComponent<Enemy>(out Enemy enemy)){
            Enemy grappledEnemy = enemy;
            grappledEnemy.getGrappled(playerBrain);
            playerBrain.doGrapple(enemy);
        }
    }
}