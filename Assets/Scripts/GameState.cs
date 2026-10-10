using System.Collections.Generic;

public sealed class GameState {
    private HashSet<PlayablePiece> _playablePieces;
    private HashSet<GhostPiece> _ghostPieces;
    public GameState()
    {
        _playablePieces = new HashSet<PlayablePiece>();
        _ghostPieces = new HashSet<GhostPiece>();
        GhostPiece firstPiece = new GhostPiece();
        _ghostPieces.Add(firstPiece);
    }

    public HashSet<PlayablePiece> GetPlayablePieces()
    {
        return _playablePieces;
    }

    public HashSet<GhostPiece> GetGhostPieces()
    {
        return _ghostPieces;
    }


    public void Spawn (int turn, PlayablePiece newPiece, GhostPiece oldPiece)
    {
        _playablePieces.Add(newPiece);
        newPiece.SetOwner(turn);
        newPiece.SetCoordsOnTop(oldPiece.GetCoords());
        newPiece.SetNeighbors(oldPiece.GetNeighbors());
        foreach (GhostPiece p in newPiece.CreateGhostNeighbors()) { _ghostPieces.Add(p); }
    }
}
