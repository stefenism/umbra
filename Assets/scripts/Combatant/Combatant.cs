using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour {

	public int MaxHealth = 1;
	public int CurrentHealth = 1;
	public bool Dead;
    public float range;
    private bool shooting;
    public bool inRange;
    public float fireRate;
    public float cooldown;
    public RaycastHit2D hit;
    public List<Vector3> playerHitPositions = new List<Vector3>();

    private Vector2 ray;
    
    public Transform player;
    public LayerMask playerLayer;

    public void Start() {
        player = GameManager.gameDaddy.player.transform;
    }

    public virtual void Update() {
        Scan();

        if(inRange){
            Shoot();
        }
    }

    public virtual void Shoot(){}

    public virtual void playerDetected(){}
    public virtual void resetPatrol(){}

    public virtual void Scan() {
        ray = new Vector2(transform.position.x, transform.position.y);

        playerHitPositions = GameManager.gameDaddy.player.getPlayerHitPositions();

        foreach(Vector3 hitPosition in playerHitPositions){

            hit = Physics2D.Raycast(transform.position, hitPosition - transform.position, range, playerLayer);

            Ray2D hitRay = new Ray2D(transform.position, hitPosition - transform.position);

            Debug.DrawRay(transform.position, ( hitPosition - transform.position).normalized * range);

            if(hit.collider != null) {
                if ( hit.collider.gameObject.tag == "Player") {
                    if( !GameManager.gameDaddy.player.playerState.IsPlayerInLight()) {
                        inRange = true;
                        player = hit.collider.gameObject.transform;
                        break;
                    }
                } else {
                    inRange = false;
                }
            } else {
                inRange = false;
            }
        }
    }

	public void Damage(int amt) {
		CurrentHealth -= amt;
		if (CurrentHealth <= 0)
			Kill();
    }

	public void Kill() {
		Dead = true;
    }

	public void InteractWithObject(InteractableObject obj) {
		obj.Interact(this);
    }
}
