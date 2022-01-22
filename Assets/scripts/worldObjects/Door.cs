using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{

    public bool RequiresSwitch = false;

    public override void Interact(Combatant cmb) {
        if (!RequiresSwitch)
            ToggleDoor();
    }

    public bool OpenDoor() {
        if (!currentState)
            currentState = true;
        return currentState;
    }

    public bool ToggleDoor() {
        currentState = !currentState;
        return currentState;
    }

    public bool IsDoorOpen() {
        return currentState;
    }
}
