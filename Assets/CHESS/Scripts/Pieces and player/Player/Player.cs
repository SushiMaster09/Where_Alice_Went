using UnityEngine;

namespace TC{

    public class Player : MonoBehaviour/* : UnderlyingPiece */{
        public static GameObject player;
        public int numberOfMoves;

        private void Awake() {
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
                AI.ai.BeginTurn();
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