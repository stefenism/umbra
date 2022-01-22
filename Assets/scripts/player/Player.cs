using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Combatant {

    private List<InteractableObject> nearObjects;
    private LightObject[] lights;
    private PlayerStateManager stateManager;


    private void Awake() {
        lights = FindObjectsOfType<LightObject>();
        stateManager = GetComponent<PlayerStateManager>();
        nearObjects = new List<InteractableObject>();
    }

    // Update is called once per frame
    void Update() {
        if (IsInLight()) {
           // if (!stateManager.IsPlayerInLight())
                //stateManager.SetPlayerInLight();
        } else {
            if (!stateManager.IsPlayerInDark())
                stateManager.SetPlayerInDark();
        }

        if (Input.GetKeyDown("e")) {
            Interact();
        }
        //Check for interact key press
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision) {
        if (collision.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
            Damage(bullet.Damage);
        if (collision.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObject)) {
            if (!nearObjects.Contains(interactableObject)) {
                nearObjects.Add(interactableObject);
                Debug.Log("Found interactable");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObject)) {
            if (nearObjects.Contains(interactableObject))
                nearObjects.Remove(interactableObject);
        }
    }

    private void Interact() {
        if (nearObjects.Count > 0)
            nearObjects[0].Interact(this);
    }

    public void SwipeAttack() {

    }

    public void GrappleEnemy(Enemy cmb) {

    }

    public bool IsInLight() {
        foreach (LightObject light in lights) {
            if (light.IsGameObjectWithinLight(this.gameObject))
                return true;
        }
        return false; //Try building collision around all the lights?
    }

}
