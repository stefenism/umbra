using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : InteractableObject {

    public LightObject lightObject;
    public Sprite onSprite;
    public Sprite offSprite;
    private SpriteRenderer renderer;

    public AudioClip SwitchSound;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
        currentState = lightObject.enabled;
        UpdateSprite(lightObject.enabled);
    }

    public override void Interact(Combatant cmb) {
        lightObject.ToggleLight();
        currentState = !currentState;
        UpdateSprite(currentState);
    }

    public void UpdateSprite(bool b) {
            renderer.sprite = b ? onSprite : offSprite;
    }
}
