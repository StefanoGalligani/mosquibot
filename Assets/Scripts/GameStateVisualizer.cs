using System.Collections.Generic;
using System.Linq;
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

    public void ShowGameState(GameState state)
    {
        InitIcons(state);
        
        Dictionary<byte, PlayablePiece> playedPieces =  state.GetPlayablePieces();
        Dictionary<byte, GhostPiece> ghostPieces =      state.GetGhostPieces();
        HashSet<Piece> remainingPieces = new HashSet<Piece>();
        foreach (KeyValuePair<byte, PlayablePiece> p in playedPieces)   remainingPieces.Add(p.Value);
        foreach (KeyValuePair<byte, GhostPiece> p in ghostPieces)       remainingPieces.Add(p.Value);

        Dictionary<byte, GhostPiece> availableSpawns = state.GetTurn() == 0 || state.GetMove() == 1 ?
            state.GetWhiteSpawns() : state.GetBlackSpawns();

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
        foreach (PieceObject po in _pieceObjs)
        {
            if (po.GetReference() is GhostPiece)
            {
                po.SetVisibility(availableSpawns.Values.Contains((GhostPiece)(po.GetReference())));
            }
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
        if (_pieceIcons != null)
        {
            foreach (PieceIcon i in _pieceIcons) Destroy(i.gameObject);
        }
        _pieceIcons = new List<PieceIcon>();
        Dictionary<byte, PlayablePiece> whitePieces = state.GetWhiteReserve();
        Dictionary<byte, PlayablePiece> blackPieces = state.GetBlackReserve();

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
