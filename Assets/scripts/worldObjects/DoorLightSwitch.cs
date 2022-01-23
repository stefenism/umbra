using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLightSwitch : InteractableObject {

    public LightObject[] lightObjects;
    public Door[] doors;
    public Sprite onSprite;
    public Sprite offSprite;
    private SpriteRenderer renderer;

    private AudioSource audioSource;
    public AudioClip SwitchSound;
    public float VolumeScale = 1f;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<SpriteRenderer>();
        //currentState = lightObject.enabled;
        UpdateSprite(currentState);
    }

    public override void Interact(Combatant cmb) {
        //lightObject.ToggleLight();
        foreach (LightObject light in lightObjects) {
            light.ToggleLight();
        }
        foreach (Door door in doors) {
            door.ToggleDoor();
        }
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
