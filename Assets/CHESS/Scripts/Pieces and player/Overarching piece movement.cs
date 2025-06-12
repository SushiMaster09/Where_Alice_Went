using System.Collections.Generic;
using UnityEngine;

namespace TC{
    public class OverarchingPieceMovement : MonoBehaviour {
        public static OverarchingPieceMovement Instance;
        public List<PieceMovement> allPieceMovement;
        public int usingIndex = 0;
        public bool usePlayersTeam = true;

        void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}