using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Switch : InteractableObject
{

    public Door door;

    public override void Interact(Combatant cmb) {
        if (!currentState)
            currentState = door.OpenDoor();
    }
}
