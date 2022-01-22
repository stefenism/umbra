using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour {

	public int MaxHealth = 1;
	public int CurrentHealth = 1;
	public bool Dead;

	public void Damage(int amt) {
		CurrentHealth -= amt;
		if (CurrentHealth <= 0)
			Kill();
    }

	public void Kill() {
		Dead = true;
    }

	public void InteractWithObject(InteractableObject obj) {
		obj.Interact(this);
    }
}
