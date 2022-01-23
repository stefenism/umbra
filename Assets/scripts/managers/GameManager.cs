using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager gameDaddy = null;

    public PlayerMovement player;
    public Player playerBrain;

    Transform currentPlayerSpawn;

    void Awake() {
        if(gameDaddy == null){
            gameDaddy = this;
        } else if(gameDaddy == this){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    static public void RespawnPlayer() {
        GameManager.gameDaddy.player.transform.position = GameManager.gameDaddy.currentPlayerSpawn.position;
    }

    static public void ReloadCurrentScene() {
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    
}