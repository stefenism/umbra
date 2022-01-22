using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : InteractableObject
{

    public LightObject lightObject;

    public override void Interact(Combatant cmb) {
        lightObject.ToggleLight();
        currentState = !currentState;
    }
}
