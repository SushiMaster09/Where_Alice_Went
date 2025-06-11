using UnityEngine;
namespace TC {
    public class MovementCircles : MonoBehaviour {
        public GameObject OriginalObject;
        public Vector2Int offset;

        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * OriginCube.MaxSizeOfBoard * 4, Color.red, 5);
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit Info)) {
                    if (Info.collider.gameObject == gameObject) {
                        CheckIfTaking();
                        if (OriginalObject.GetComponent<UnderlyingPiece>().playersTeam) {
                            OriginalObject.GetComponent<UnderlyingPiece>().previousPosition = new Vector3(transform.position.x, transform.position.y + 0.9f + OriginalObject.GetComponent<UnderlyingPiece>().thisPiece.playersTeamVerticalOffset, transform.position.z);
                        }
                        else {
                            OriginalObject.GetComponent<UnderlyingPiece>().previousPosition = new Vector3(transform.position.x, transform.position.y + 0.9f + OriginalObject.GetComponent<UnderlyingPiece>().thisPiece.AITeamVerticalOffset, transform.position.z);
                        }
                        OriginalObject.GetComponent<UnderlyingPiece>().selected = false;
                        OriginalObject.GetComponent<UnderlyingPiece>().hasMoved = true;
                        OriginalObject.GetComponent<UnderlyingPiece>().DeactivateVisibility();
                        Player.player.GetComponent<Player>().numberOfMoves -= 1;
                    }
                }
            }
        }
        void CheckIfTaking() {
            if (OriginalObject.GetComponent<UnderlyingPiece>().PieceInDirection(offset.x, offset.y) != null) {
                OriginalObject.GetComponent<UnderlyingPiece>().capturedPieces += 1;
                AI.ai.AITeam.Remove(OriginalObject.GetComponent<UnderlyingPiece>().PieceInDirection(offset.x, offset.y).GetComponent<UnderlyingPiece>().thisPiece);
                Destroy(OriginalObject.GetComponent<UnderlyingPiece>().PieceInDirection(offset.x, offset.y));
            }
        }
    }
}