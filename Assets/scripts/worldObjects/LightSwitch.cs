using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : InteractableObject {

    public LightObject lightObject;
    public Sprite onSprite;
    public Sprite offSprite;
    private SpriteRenderer renderer;

    private AudioSource audioSource;
    public AudioClip SwitchSound;
    public float VolumeScale = 1f;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<SpriteRenderer>();
        currentState = lightObject.enabled;
        UpdateSprite(lightObject.enabled);
    }

    public override void Interact(Combatant cmb) {
        lightObject.ToggleLight();
        currentState = !currentState;
        UpdateSprite(currentState);
        PlaySound();
    }

    public void UpdateSprite(bool b) {
            renderer.sprite = b ? onSprite : offSprite;
    }

    public void PlaySound() {
        audioSource.PlayOneShot(SwitchSound, VolumeScale);
    }
}
