using System.Collections.Generic;

public sealed class GameState {
    private Dictionary<int, PlayablePiece> _playablePieces;
    private Dictionary<int, GhostPiece> _ghostPieces;
    private int _nextIndex = 0;
    public GameState()
    {
        _playablePieces = new Dictionary<int, PlayablePiece>();
        _ghostPieces = new Dictionary<int, GhostPiece>();
        GhostPiece firstPiece = new GhostPiece();
        _ghostPieces.Add(_nextIndex++, firstPiece);
    }

    public Dictionary<int, PlayablePiece> GetPlayablePieces()
    {
        return _playablePieces;
    }

    public Dictionary<int, GhostPiece> GetGhostPieces()
    {
        return _ghostPieces;
    }


    public void Spawn (int turn, PlayablePiece newPiece, GhostPiece oldPiece)
    {
        _playablePieces.Add(_nextIndex++, newPiece);
        newPiece.SetOwner(turn);
        newPiece.PlaceOnTop(oldPiece.GetCoords());
        newPiece.SetNeighbors(oldPiece.GetNeighbors());
        foreach (GhostPiece p in newPiece.CreateGhostNeighbors()) { _ghostPieces.Add(_nextIndex++, p); }
    }
}
