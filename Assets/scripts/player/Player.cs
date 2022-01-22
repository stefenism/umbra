using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Combatant {

    private List<InteractableObject> nearObjects;

    // Start is called before the first frame updateasdf
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //Check for interact key press
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision) {
        if (collision.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
            Damage(bullet.Damage);
        if (collision.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObject)) {
            if (!nearObjects.Contains(interactableObject))
                nearObjects.Add(interactableObject);
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
        return false; //Try building collision around all the lights?
    }


}
