using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Combatant {

    private List<InteractableObject> nearObjects;
    private Enemy enemyInRange;
    private LightObject[] lights;
    private PlayerStateManager stateManager;
    private Enemy grappledEnemy;

    private void Awake() {
        lights = FindObjectsOfType<LightObject>();
        stateManager = GetComponent<PlayerStateManager>();
        nearObjects = new List<InteractableObject>();
    }

    // Update is called once per frame
    void Update() {
        if (IsInLight()) {
            if (!stateManager.IsPlayerInLight()){
                stateManager.SetPlayerInLight();
            }
        } else {
            if (!stateManager.IsPlayerInDark()) {
                stateManager.SetPlayerInDark();
                if (stateManager.HasEnemeyGrappled) {
                    grappledEnemy.Kill();
                    grappledEnemy.transform.parent = null;
                    Destroy(grappledEnemy.gameObject);
                    stateManager.HasEnemeyGrappled = false;
                    grappledEnemy = null;
                }
            }
        }

        if (Input.GetKeyDown("e")) {
            Interact();
        }
        if (Input.GetMouseButtonDown(0)) {
            GrappleEnemy();
        }
        //Check for interact key press
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision) {
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

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObject)) {
            if (nearObjects.Contains(interactableObject))
                nearObjects.Remove(interactableObject);
        }
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
            enemyInRange = null;
        }
    }

    private void Interact() {
        if (nearObjects.Count > 0)
            nearObjects[0].Interact(this);
    }

    public void GrappleEnemy() {
        if (enemyInRange == null) {
            //Play miss/grabble animation?
            return;
        }

        if (stateManager.IsPlayerInLight() && grappledEnemy == null) {
            //Play big boy grapple
            stateManager.HasEnemeyGrappled = true;
            grappledEnemy = enemyInRange;
            grappledEnemy.transform.parent = transform;
        }else if (stateManager.IsPlayerInDark() && grappledEnemy == null) {
            //Just kill the baddy
            enemyInRange.Kill();
        }
    }

    public bool IsInLight() {
        foreach (LightObject light in lights) {
            if (light.IsGameObjectWithinLight(this.gameObject))
                return true;
        }
        return false;
    }

}
