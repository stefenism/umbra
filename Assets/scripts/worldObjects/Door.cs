using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{

    public bool RequiresSwitch = false;
    public GameObject doorObj;
    private Animator animator;

    private void Awake() {
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
        doorObj.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        UpdateAnimation(true);
    }

    public void CloseDoor() {
        currentState = false;
        doorObj.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        UpdateAnimation(false);
    }

    public bool ToggleDoor() {
        currentState = !currentState;
        doorObj.gameObject.GetComponent<BoxCollider2D>().enabled = !currentState;
        UpdateAnimation(currentState);
        return currentState;
    }

    public bool IsDoorOpen() {
        return currentState;
    }

    private void UpdateAnimation(bool b) {
        animator.SetBool("Open", b);
    }
}
