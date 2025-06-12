using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeBackTo2DWhenLeavingChess : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        DontDestroyOnLoad(gameObject);
    }
    bool chessIsLoaded = false;
    // Update is called once per frame
    void Update() {
        if (SceneManager.GetActiveScene().name == "Chess") {
            chessIsLoaded = true;
        }
        else if (chessIsLoaded) {
            SwapRenderPipeline.UltraAnd2D();
        }
    }
}
