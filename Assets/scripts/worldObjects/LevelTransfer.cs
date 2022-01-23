using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransfer : MonoBehaviour {


    public string NextLeveName = "";
    private PlayerMovement mover;

    private void Awake() {
        mover = FindObjectOfType<PlayerMovement>();
    }

    public void GotoNextLevel() {
        SceneManager.LoadScene(NextLeveName);
    }


    private void OnTriggerEnter2D(UnityEngine.Collider2D collision) {
        Debug.Log("HIT");
        if (collision.gameObject == mover.tallBoy || collision.gameObject == mover.ballBoy)
            GotoNextLevel();
    }


}