using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace TC{

    public class Player : MonoBehaviour/* : UnderlyingPiece */{
        public static GameObject player;
        public int numberOfMoves;
        [SerializeField]
        RenderPipelineAsset a;
        [SerializeField]
        RenderPipelineAsset b;

        private void Awake() {
                SwapRenderPipeline.pipeline2Dstat = a;
                SwapRenderPipeline.ultraURPstat = b;
            SwapRenderPipeline.UltraAnd2D();
            if (player == null) {
                player = gameObject;
            }
        }
        private void LateUpdate() {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }

        /*private void Start() {
            previousPosition = new Vector3(0, 1, 0);
            playersTeam = true;
            ActualStart();
        }*/

        void FixedUpdate() {
            if (numberOfMoves == 0) {
                bool gameFinished = true;
                foreach (PieceMovement thing in AI.ai.AITeam) {
                    if (thing.name.Contains("King")) {
                        gameFinished = false;
                    }
                }
                if (gameFinished) {
                    AI.difficulty += 1;
                    SceneManager.LoadScene("ChapSelect");
                }
                else {
                    AI.ai.BeginTurn();
                }
            }
            /*if (mode == Mode.levelling) {
                IfNotLevellingReturn();
                selected = true;
            }*/
            //Selected();
            //EnsureCorrectPositions("Hi, little easter egg here for you");
        }
    }
}