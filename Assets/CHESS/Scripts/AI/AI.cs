using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Data;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Threading;
using Unity.VisualScripting;

namespace TC {
    public class AI : MonoBehaviour {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public static AI ai;
        public List<PieceMovement> PlayersTeam;
        public List<PieceMovement> AITeam;
        bool firstFrame = true;
        public static int difficulty;
        int numOfAI;
        int numOfPlayers;

        void Awake() {
            ai = this;
        }

        void CheckAllPlayersAndAITeamsArePresentAndCorrect() {
            List<PieceMovement> tempList = new();
            foreach (PieceMovement movement in PlayersTeam.Where(piece => piece.thisObject.playersTeam == false)) {
                tempList.Add(movement);
            }
            foreach (PieceMovement movement in tempList) {
                PlayersTeam.Remove(movement);
                AITeam.Add(movement);
            }
            tempList.Clear();
            foreach (PieceMovement movement in AITeam.Where(piece => piece.thisObject.playersTeam)) {
                tempList.Add(movement);
            }
            foreach (PieceMovement movement in tempList) {
                AITeam.Remove(movement);
                PlayersTeam.Add(movement);
            }
        }

        int Evaluation(Gamestate gamestate) {
            int evaluation = 0;
            foreach (PieceMovement piece in gamestate.playersTeam) {
                //WORK ON THIS
                evaluation -= GetAmountOfMaterial(piece);
            }
            foreach (PieceMovement piece in gamestate.AITeam) {
                evaluation += GetAmountOfMaterial(piece);
            }
            return evaluation;
        }

        (int, Vector2Int, UnderlyingPiece) Search(int depth, float alpha, float beta, int startingDepth, Gamestate gamestate) {
            //create a new class where the gamestate can be saved - things like a 200 by 200 board state and a piece list for each team
            Vector2Int bestPosition = Vector2Int.zero;
            depth -= 1;
            bool AITurn = (startingDepth - depth) % 2 == 0;
            int bestEvaluation = AITurn ? int.MinValue : int.MaxValue;
            UnderlyingPiece bestPiece = null;
            if (numOfTimesValidMoveCacheRefreshed <= (int)(startingDepth - depth) / 2) {
                numOfTimesValidMoveCacheRefreshed++;
                foreach (PieceMovement pieceMovement in AITeam) {
                    validMoveCache[pieceMovement.name] = ValidMoves(pieceMovement);
                }
                foreach (PieceMovement pieceMovement in PlayersTeam) {
                    validMoveCache[pieceMovement.name] = ValidMoves(pieceMovement);
                }
            }
            int count = 0;
            IEnumerable<PieceMovement> usedMovement = AITurn ? gamestate.AITeam.Where(piece => piece.hasMoved == false) : gamestate.playersTeam;
            if (depth - startingDepth == 0) {
                //one of the three AI turn moves
                Parallel.ForEach(usedMovement, (movement, parallelLoopsState) => {
                    Gamestate usedGamestate = new(gamestate);
                    if (movement.hasMoved) {
                        return;
                    }
                    foreach (Vector2Int move in validMoveCache[movement.name].OrderByDescending(move => usedGamestate.PieceInPosition(movement.AIAccessiblePosition + move) != null ? GetAmountOfMaterial(usedGamestate.PieceInPosition(movement.AIAccessiblePosition + move)) : 0)) {
                        if (movement.infinitelyScalingRange) {
                            //make a new thing in here to check each time it is scaled up
                            for (int l = 1; l <= movement.currentRange; l++) {
                                Vector2Int scalingMove = move * l;
                                if (!Gamestate.DoesPositionExist(movement.AIAccessiblePosition + scalingMove)) {
                                    break;
                                }
                                PieceMovement capturingPiece = ApplyCapture(movement, scalingMove, !AITurn, ref usedGamestate);
                                movement.AIAccessiblePosition = ApplyMovement(movement, move);
                                if (capturingPiece == movement) {
                                    UndoMove(movement, capturingPiece, ref usedGamestate, AITurn);
                                    break;
                                }
                                movement.hasMoved = true;
                                var aSearch = AITurn ? Search(depth, alpha, beta, startingDepth, new Gamestate(usedGamestate)) : Search(depth, alpha, beta, startingDepth, new Gamestate(usedGamestate));
                                UndoMove(movement, capturingPiece, ref usedGamestate, AITurn);
                                movement.hasMoved = false;
                                if (AITurn) {
                                    if (aSearch.Item1 > bestEvaluation) {
                                        bestEvaluation = aSearch.Item1;
                                        bestPiece = movement.thisObject;
                                        bestPosition = scalingMove;
                                    }
                                    alpha = Math.Max(alpha, bestEvaluation);
                                }
                                else {
                                    if (aSearch.Item1 < bestEvaluation) {
                                        bestEvaluation = aSearch.Item1;
                                        bestPiece = movement.thisObject;
                                        bestPosition = scalingMove;
                                    }
                                    beta = Math.Min(beta, bestEvaluation);
                                }
                                if (beta < alpha) {
                                    break;
                                }
                            }
                        }
                        else {
                            if (!Gamestate.DoesPositionExist(movement.AIAccessiblePosition + move)) {
                                continue;
                            }
                            PieceMovement capturingPiece = ApplyCapture(movement, move, !AITurn, ref usedGamestate);
                            movement.AIAccessiblePosition = ApplyMovement(movement, move);
                            if (capturingPiece == movement) {
                                UndoMove(movement, capturingPiece, ref usedGamestate, AITurn);
                                continue;
                            }
                            movement.hasMoved = true;
                            var aSearch = AITurn ? Search(depth, alpha, beta, startingDepth, new Gamestate(usedGamestate)) : Search(depth, alpha, beta, startingDepth, new Gamestate(usedGamestate));
                            UndoMove(movement, capturingPiece, ref usedGamestate, AITurn);
                            movement.hasMoved = false;
                            if (AITurn) {
                                if (aSearch.Item1 > bestEvaluation) {
                                    bestEvaluation = aSearch.Item1;
                                    bestPiece = movement.thisObject;
                                    bestPosition = move;
                                }
                                alpha = Math.Max(alpha, bestEvaluation);
                            }
                            else {
                                if (aSearch.Item1 < bestEvaluation) {
                                    bestEvaluation = aSearch.Item1;
                                    bestPiece = movement.thisObject;
                                    bestPosition = move;
                                }
                                beta = Math.Min(beta, bestEvaluation);
                            }
                            if (beta < alpha) {
                                //Debug.Log("alpha beta pruning success");
                                break;
                            }
                        }
                    }
                    if (beta < alpha) {
                        //Debug.Log("alpha beta pruning success");
                        parallelLoopsState.Stop();
                    }
                });
            }
            else if (depth > 0) {
                foreach (PieceMovement movement in usedMovement) {
                    if (movement.hasMoved) {
                        continue;
                    }
                    count = 0;
                    foreach (Vector2Int move in validMoveCache[movement.name].OrderByDescending(move => gamestate.PieceInPosition(movement.AIAccessiblePosition + move) != null ? GetAmountOfMaterial(gamestate.PieceInPosition(movement.AIAccessiblePosition + move)) : 0)) {
                        if (movement.infinitelyScalingRange) {
                            //make a new thing in here to check each time it is scaled up
                            for (int l = 1; l <= movement.currentRange; l++) {
                                Vector2Int scalingMove = move * l;
                                if (!Gamestate.DoesPositionExist(movement.AIAccessiblePosition + scalingMove)) {
                                    break;
                                }
                                PieceMovement capturingPiece = ApplyCapture(movement, scalingMove, !AITurn, ref gamestate);
                                movement.AIAccessiblePosition = ApplyMovement(movement, move);
                                if (capturingPiece == movement) {
                                    UndoMove(movement, capturingPiece, ref gamestate, AITurn);
                                    break;
                                }
                                movement.hasMoved = true;
                                var aSearch = AITurn ? Search(depth, alpha, beta, startingDepth, new Gamestate(gamestate)) : Search(depth, alpha, beta, startingDepth, new Gamestate(gamestate));
                                UndoMove(movement, capturingPiece, ref gamestate, AITurn);
                                movement.hasMoved = false;
                                if (AITurn) {
                                    if (aSearch.Item1 > bestEvaluation) {
                                        bestEvaluation = aSearch.Item1;
                                        bestPiece = movement.thisObject;
                                        bestPosition = scalingMove;
                                    }
                                    alpha = Math.Max(alpha, bestEvaluation);
                                }
                                else {
                                    if (aSearch.Item1 < bestEvaluation) {
                                        bestEvaluation = aSearch.Item1;
                                        bestPiece = movement.thisObject;
                                        bestPosition = scalingMove;
                                    }
                                    beta = Math.Min(beta, bestEvaluation);
                                }
                                if (beta < alpha) {
                                    break;
                                }
                            }
                        }
                        else {
                            if (!Gamestate.DoesPositionExist(movement.AIAccessiblePosition + move)) {
                                continue;
                            }
                            PieceMovement capturingPiece = ApplyCapture(movement, move, !AITurn, ref gamestate);
                            movement.AIAccessiblePosition = ApplyMovement(movement, move);
                            if (capturingPiece == movement) {
                                UndoMove(movement, capturingPiece, ref gamestate, AITurn);
                                continue;
                            }
                            movement.hasMoved = true;
                            var aSearch = AITurn ? Search(depth, alpha, beta, startingDepth, new Gamestate(gamestate)) : Search(depth, alpha, beta, startingDepth, new Gamestate(gamestate));
                            UndoMove(movement, capturingPiece, ref gamestate, AITurn);
                            movement.hasMoved = false;
                            if (AITurn) {
                                if (aSearch.Item1 > bestEvaluation) {
                                    //Debug.Log(aSearch.Item1 + ", " + bestEvaluation + ", " + "AIturn");
                                    bestEvaluation = aSearch.Item1;
                                    bestPiece = movement.thisObject;
                                    bestPosition = move;
                                }
                                alpha = Math.Max(alpha, bestEvaluation);
                            }
                            else {
                                if (aSearch.Item1 < bestEvaluation) {
                                    //Debug.Log(aSearch.Item1 + ", " + bestEvaluation + ", " + "Playerturn");
                                    bestEvaluation = aSearch.Item1;
                                    bestPiece = movement.thisObject;
                                    bestPosition = move;
                                }
                                beta = Math.Min(beta, bestEvaluation);
                            }
                            //Debug.Log(movement.name + ", " + movement.thisObject.name);
                            if (beta < alpha) {
                                //Debug.Log("alpha beta pruning success" + beta + ", " + alpha);
                                break;
                            }
                            count++;
                        }
                    }
                    if (beta < alpha) {
                        //Debug.Log("alpha beta pruning success" + beta + ", " + alpha);
                        break;
                    }
                }
            }
            else {
                bestEvaluation = Evaluation(gamestate);
            }
            return (bestEvaluation, bestPosition, bestPiece);
        }
        public static List<Vector2Int> ValidMoves(PieceMovement piece) {
            List<Vector2Int> validMovePositions = new();

            for (int i = 0; i < piece.moveableTiles.Length; i++) {
                for (int j = 0; j < piece.moveableTiles.Length; j++) {
                    if (i + piece.AIAccessiblePosition.x <= OriginCube.MaxSizeOfBoard && j + piece.AIAccessiblePosition.y <= OriginCube.MaxSizeOfBoard && i + piece.AIAccessiblePosition.x >= -OriginCube.MaxSizeOfBoard && j + piece.AIAccessiblePosition.y >= -OriginCube.MaxSizeOfBoard && piece.moveableTiles[i][j]) {
                        Vector2Int move = new(i - piece.potentialRange, j - piece.potentialRange);
                        if (Gamestate.DoesPositionExist(move + piece.AIAccessiblePosition)) {
                            validMovePositions.Add(move);
                        }
                    }
                }
            }
            return validMovePositions;
        }

        Vector2Int ApplyMovement(PieceMovement piece, Vector2Int relativePosition) {
            return piece.AIAccessiblePosition + relativePosition;
        }

        PieceMovement ApplyCapture(PieceMovement piece, Vector2Int relativePosition, bool playersTurn, ref Gamestate gamestate) {
            PieceMovement returningPiece;
            //try {
            returningPiece = gamestate.PieceInPosition(piece.AIAccessiblePosition + relativePosition);
            if (returningPiece != null && returningPiece.thisObject.playersTeam == playersTurn) {
                returningPiece = piece;
                if (playersTurn) {
                    gamestate.AITeam.Remove(returningPiece);
                }
                else {
                    gamestate.playersTeam.Remove(returningPiece);
                }
            }
            //}
            //catch { }
            return returningPiece;
        }

        void UndoMove(PieceMovement piece, PieceMovement capturingPiece, ref Gamestate gamestate, bool AITurn) {
            if (capturingPiece != null) {
                if (AITurn) {
                    gamestate.playersTeam.Add(capturingPiece);
                    if (numOfPlayers < PlayersTeam.Count) {
                        throw new Exception(capturingPiece.name + " Added over and above the rest");
                    }
                }
                else {
                    gamestate.AITeam.Add(capturingPiece);
                    if (numOfAI < AITeam.Count) {
                        throw new Exception(capturingPiece.name + " Added over and above the rest");
                    }
                }
            }
            piece.AIAccessiblePosition = new Vector2Int((int)piece.thisObject.previousPosition.x, (int)piece.thisObject.previousPosition.z);
        }

        int GetAmountOfMaterial(PieceMovement movement) {
            string pieceName;
            if (movement == null) {
                return 0;
            }
            try {
                pieceName = movement.inheritingPiece.name[0..movement.inheritingPiece.name.IndexOf(" ")];
            }
            catch {
                pieceName = movement.inheritingPiece.name;
            }
            int returningInt = 0;
            return pieceName switch {
                "Rook" => 500 + returningInt,
                "Bishop" => 300 + returningInt,
                "Knight" => 300 + returningInt,
                "Pawn" => 100 + returningInt,
                "Queen" => 900 + returningInt,
                "King" => 5000 + returningInt,
                _ => throw new KeyNotFoundException("name not found: " + pieceName),
            };
        }

        private void FixedUpdate() {
            if (firstFrame) {
                numOfAI = AITeam.Count;
                numOfPlayers = PlayersTeam.Count;
                CheckAllPlayersAndAITeamsArePresentAndCorrect();
                firstFrame = false;
            }
            if (AITeam.Count < 5) {

                SceneManager.LoadScene("ChapSelect");
            }
        }


        void ExecuteMove(UnderlyingPiece piece, Vector2Int destination) {
            Debug.Log(piece + ", " + destination);
            if (piece.playersTeam == false) {
                if (piece.PieceInDirection(destination) != null) {
                    if (piece.PieceInDirection(destination).GetComponent<UnderlyingPiece>().playersTeam) {
                        PlayersTeam.Remove(piece.PieceInDirection(destination).GetComponent<UnderlyingPiece>().thisPiece);
                        Destroy(piece.PieceInDirection(destination));
                    }
                    else {
                        throw new Exception(piece.thisPiece.name + "tried to capture it's own piece");
                    }
                }
                piece.previousPosition = new Vector3(piece.transform.position.x + destination.x, UnderlyingPiece.pieceHeight, piece.transform.position.z + destination.y);
            }
            else {
                throw new Exception("tried to move one of the Player's pieces: " + piece.thisPiece.name);
            }
        }

        Dictionary<string, List<Vector2Int>> validMoveCache;
        int numOfTimesValidMoveCacheRefreshed;

        public void BeginTurn() {
            validMoveCache = new();
            numOfTimesValidMoveCacheRefreshed = 0;
            Gamestate gamestate = new(PlayersTeam, AITeam);
            foreach (PieceMovement pieceMovement in AITeam) {
                validMoveCache[pieceMovement.name] = ValidMoves(pieceMovement);
            }
            foreach (PieceMovement pieceMovement in PlayersTeam) {
                validMoveCache[pieceMovement.name] = ValidMoves(pieceMovement);
            }
            foreach (PieceMovement piece in AITeam.Where(piece => piece.thisObject.hasMoved)) {
                piece.thisObject.hasMoved = false;
            }

            foreach (PieceMovement piece in AITeam.Where(piece => piece == null)) {
                AITeam.Remove(piece);
            }
            int searchDepth = 1 + difficulty * 2;
            int numberOfMoves = 1;
            //for (int i = numberOfMoves; i >= 1; i--) {
            var evaluatedPieceAndMovement = Search(searchDepth + numberOfMoves, Mathf.NegativeInfinity, Mathf.Infinity, searchDepth + numberOfMoves - 1, new Gamestate(gamestate));
            ExecuteMove(evaluatedPieceAndMovement.Item3, evaluatedPieceAndMovement.Item2);

            foreach (PieceMovement piece in PlayersTeam.Where(piece => piece.thisObject.hasMoved)) {
                piece.thisObject.hasMoved = false;
            }
            Player.player.GetComponent<Player>().numberOfMoves = 1;
        }
    }
}