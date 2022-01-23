using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Combatant {

    private List<InteractableObject> nearObjects;
    private Enemy enemyInRange;
    private LightObject[] lights;
    private PlayerStateManager stateManager;
    private PlayerMovement mover;
    private Enemy grappledEnemy;
    public BoxCollider2D grappleTrigger;

    private float timeToChange = .4f;
    private float timeWaited = 0f;
    private bool waiting = false;

    private void Awake() {
        lights = FindObjectsOfType<LightObject>();
        stateManager = GetComponent<PlayerStateManager>();
        nearObjects = new List<InteractableObject>();
        mover = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update() {
        if (waiting) {
            if (timeWaited >= timeToChange) {
                timeWaited = 0f;
                waiting = false;
            } else {
                timeWaited += Time.deltaTime;
                return;
            }
        }
        if (IsInLight()) {
            Debug.Log("checking if is in light");
            if (!stateManager.IsPlayerInLight() && !stateManager.IsPlayerOnGround() && !stateManager.IsPlayerGrappling()) {
                stateManager.SetPlayerInLight();
            } 
        } else {
            if (!stateManager.IsPlayerInDark()) {
                waiting = true;
                stateManager.SetPlayerInDark();
                // if (stateManager.HasEnemeyGrappled) {
                //     grappledEnemy.Kill();
                //     grappledEnemy.transform.parent = null;
                //     stateManager.HasEnemeyGrappled = false;
                //     grappledEnemy = null;
                // }
            }
        }

        if (Input.GetKeyDown("e")) {
            Interact();
        }
        // if (Input.GetMouseButtonDown(0)) {
        //     GrappleEnemy();
        // }
    }

    public void PassedPlayerCollisionEnter(UnityEngine.Collider2D collision) {
        if (collision.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
            Damage(bullet.Damage);
        if (collision.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObject)) {
            if (!nearObjects.Contains(interactableObject)) {
                nearObjects.Add(interactableObject);
            }
        }
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
            enemyInRange = enemy;
        }
    }

    public void PassedPlayerCollisionExit(Collider2D collision) {
        if (collision.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObject)) {
            if (nearObjects.Contains(interactableObject))
                nearObjects.Remove(interactableObject);
        }
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
            enemyInRange = null;
        }
    }

    public void doGrapple(Enemy enemy){
        GrappleEnemy(enemy);
        stateManager.SetPlayerGrappling();
    }

    private void Interact() {
        if (nearObjects.Count > 0)
            nearObjects[0].Interact(this);
    }

    public void GrappleEnemy(Enemy enemy) {
        if (stateManager.IsPlayerOnGround() && grappledEnemy == null) {
            mover.tallAnimator.SetBool("Attack", true);
        }
        // if (enemyInRange == null) {
        //     return;
        // }

        if (stateManager.IsPlayerOnGround() && grappledEnemy == null) {
            //Play big boy grapple
            grappledEnemy = enemy;
            enemy.gameObject.transform.parent = mover.tallBoy.transform;
        }else if (stateManager.IsPlayerInDark() && grappledEnemy == null) {
            //Just kill the baddy
            // enemyInRange.Kill();
        }
    }

    public void killEnemy(Enemy enemy = null) {
        if(!grappledEnemy && enemy == null){
            return;
        }

        if( enemy != null && !grappledEnemy) {
            Destroy(enemy.gameObject);
        } else {
            Debug.Log("gonna destroy enemy");
            grappledEnemy.transform.parent = null;
            Destroy(grappledEnemy.gameObject);
            grappledEnemy = null;
        }

    }

    public bool IsInLight() {
        foreach (LightObject light in lights) {
            if (light.IsGameObjectWithinLight(mover.GetUsedStateObject()))
                return true;
        }
        return false;
    }

    public void setGrappledEnemy(Enemy enemy){
        grappledEnemy = enemy;
    }

    public Enemy getGrappledEnemy() {
        return grappledEnemy;
    }


    public PlayerMovement GetPlayerMovement() {
        return mover;
    }
}
