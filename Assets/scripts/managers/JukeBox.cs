using UnityEngine;

public class JukeBox : MonoBehaviour {
    
    public static JukeBox jukebox = null;

    private void Awake() {
        if(jukebox == null) {
            jukebox = this;
        } else if (jukebox == this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}