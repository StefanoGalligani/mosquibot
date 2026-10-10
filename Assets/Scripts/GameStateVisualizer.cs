using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStateVisualizer : MonoBehaviour
{
    [SerializeField] private PieceObject _piecePrefab;
    private List<PieceObject> _pieceObjs;

    public GameStateVisualizer()
    {
        _pieceObjs = new List<PieceObject>();
    }

    //TODO: farlo con la mossa senza l'intero stato
    public void ShowGameState(GameState state)
    {
        Dictionary<int, PlayablePiece> playablePieces = state.GetPlayablePieces();
        Dictionary<int, GhostPiece> ghostPieces = state.GetGhostPieces();
        HashSet<Piece> remainingPieces = new HashSet<Piece>();
        foreach (KeyValuePair<int, PlayablePiece> p in playablePieces) remainingPieces.Add(p.Value);
        foreach (KeyValuePair<int, GhostPiece> p in ghostPieces) remainingPieces.Add(p.Value);

        foreach (PieceObject po in _pieceObjs)
        {
            remainingPieces.Remove(po.GetReference());
            po.UpdatePosition();
        }

        foreach (Piece p in remainingPieces)
        {
            PieceObject newPObj = Instantiate(_piecePrefab);
            newPObj.AddReference(p);
            newPObj.transform.SetParent(transform);
            _pieceObjs.Add(newPObj);
        }
    }
}
