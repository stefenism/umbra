using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventPasser : MonoBehaviour
{
    private Player player;

    private void Awake() {
        player = transform.parent.GetComponent<Player>();    
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision) {
        player.PassedPlayerCollisionEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        player.PassedPlayerCollisionExit(collision);
    }

    public void TestMethod() {
        player.GetPlayerMovement().FinishSwitchToBall();
    }

    public void StartMovement() {
        Debug.Log("---------------RESTARTING MOVEMENT");
        player.GetPlayerMovement().ContinuePlayerInput();
    }

    public void FinishTurn() {
        player.GetPlayerMovement().FinishFlip();
    }
}
