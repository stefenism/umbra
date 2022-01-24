using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{

    bool RequiresSwitch = true;
    public GameObject doorObj;
    private Animator animator;

    public AudioClip DoorOpenSound;
    private AudioSource audioSource;
    public float VolumeScale = 1f;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
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
        doorObj.gameObject.SetActive(false);
        UpdateAnimation(true);
    }

    public void CloseDoor() {
        currentState = false;
        doorObj.gameObject.SetActive(true);
        UpdateAnimation(false);
    }

    public bool ToggleDoor() {
        currentState = !currentState;
        doorObj.gameObject.SetActive(!currentState);
        UpdateAnimation(currentState);
        PlayAudio();
        return currentState;
    }

    public bool IsDoorOpen() {
        return currentState;
    }

    private void UpdateAnimation(bool b) {
        animator.SetBool("Open", b);
    }

    private void PlayAudio() {
        audioSource.PlayOneShot(DoorOpenSound, VolumeScale);
    }
}
