using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public class GameStateVisualizer : MonoBehaviour
{
    [SerializeField] private PieceObject _piecePrefab;
    [SerializeField] private GameObject _pieceIconPrefab;
    [SerializeField] private RectTransform _whiteReserve;
    [SerializeField] private RectTransform _blackReserve;
    private List<PieceObject> _pieceObjs;
    private List<GameObject> _pieceIcons;

    public GameStateVisualizer()
    {
        _pieceObjs = new List<PieceObject>();
    }

    //TODO: farlo con la mossa senza l'intero stato
    public void ShowGameState(GameState state)
    {
        Dictionary<int, PlayablePiece> playedPieces = state.GetPlayablePieces();
        Dictionary<int, GhostPiece> ghostPieces = state.GetGhostPieces();
        HashSet<Piece> remainingPieces = new HashSet<Piece>();
        foreach (KeyValuePair<int, PlayablePiece> p in playedPieces) remainingPieces.Add(p.Value);
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
