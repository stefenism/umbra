using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{

    public bool RequiresSwitch = false;
    public GameObject doorObj;

    private void Awake() {
        if (currentState)
            OpenDoor();
        else
            CloseDoor();
    }

    public override void Interact(Combatant cmb) {
        if (!RequiresSwitch)
            ToggleDoor();
    }

    public void OpenDoor() {
        currentState = true;
        doorObj.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void CloseDoor() {
        currentState = false;
        doorObj.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    public bool ToggleDoor() {
        currentState = !currentState;
        doorObj.gameObject.GetComponent<BoxCollider2D>().enabled = !currentState;
        return currentState;
    }

    public bool IsDoorOpen() {
        return currentState;
    }
}
