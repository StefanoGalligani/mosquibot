using System.Collections.Generic;
using UnityEngine;

public class GameStateVisualizer : MonoBehaviour
{
    [SerializeField] private PieceObject _piecePrefab;
    [SerializeField] private PieceIcon _pieceIconPrefab;
    [SerializeField] private RectTransform _whiteReserve;
    [SerializeField] private RectTransform _blackReserve;
    private List<PieceObject> _pieceObjs;
    private List<PieceIcon> _pieceIcons;

    public GameStateVisualizer()
    {
        _pieceObjs = new List<PieceObject>();
    }

    //TODO: farlo con la mossa senza l'intero stato
    public void ShowGameState(GameState state)
    {
        if (_pieceIcons == null) InitIcons(state);
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

            foreach (PieceIcon i in _pieceIcons) if (i.GetReference() == p) Destroy(i.gameObject);
        }
    }

    private PieceIcon InitIcon(PlayablePiece p)
    {
        PieceIcon i = Instantiate(_pieceIconPrefab);
        i.AddReference(p);
        _pieceIcons.Add(i);
        return i;
    }

    private void InitIcons(GameState state)
    {
        _pieceIcons = new List<PieceIcon>();
        Dictionary<int, PlayablePiece> whitePieces = state.GetWhiteReserve();
        Dictionary<int, PlayablePiece> blackPieces = state.GetBlackReserve();

        foreach(PlayablePiece p in whitePieces.Values)
        {
            PieceIcon i = InitIcon(p);
            i.transform.SetParent(_whiteReserve);
            i.transform.localScale = new Vector3(1, 1, 1);
        }
        foreach(PlayablePiece p in blackPieces.Values)
        {
            PieceIcon i = InitIcon(p);
            i.transform.SetParent(_blackReserve);
            i.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
