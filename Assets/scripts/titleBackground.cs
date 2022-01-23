using UnityEngine;

public class titleBackground : MonoBehaviour {
    
    public Gradient gradient;
    public SpriteRenderer spriteRenderer;

    private float timer;

    private void Start() {
        
    }

    private void Update() {
        timer += .25f;

        if(timer >= 100){
            timer = 0;
        }

        Color color = gradient.Evaluate(timer/100);
        spriteRenderer.color = color;

        if(Input.anyKey){
            GameManager.LoadNextScene();
        }
    }
}