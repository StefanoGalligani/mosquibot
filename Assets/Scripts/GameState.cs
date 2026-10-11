using System.Collections.Generic;
using UnityEngine;

public sealed class GameState {
    private Dictionary<int, PlayablePiece> _playablePieces;
    private Dictionary<int, GhostPiece> _ghostPieces;
    private Dictionary<int, PlayablePiece> _whiteReserve;
    private Dictionary<int, PlayablePiece> _blackReserve;
    private int _nextIndex = 0;
    public GameState()
    {
        _playablePieces = new Dictionary<int, PlayablePiece>();
        _ghostPieces = new Dictionary<int, GhostPiece>();
        _whiteReserve = new Dictionary<int, PlayablePiece>();
        _blackReserve = new Dictionary<int, PlayablePiece>();

        GhostPiece firstPiece = new GhostPiece();
        firstPiece.Place(new Vector3Int(0, 0, 0));
        _ghostPieces.Add(_nextIndex++, firstPiece);

        _whiteReserve.Add(_nextIndex++, new QueenPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new AntPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new AntPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new AntPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new SpiderPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new SpiderPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new BeetlePiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new BeetlePiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new GrasshopperPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new GrasshopperPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new GrasshopperPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new MosquitoPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new LadybugPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++, new PillbugPiece().SetOwner(0));

        _blackReserve.Add(_nextIndex++, new QueenPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new AntPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new AntPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new AntPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new SpiderPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new SpiderPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new BeetlePiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new BeetlePiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new GrasshopperPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new GrasshopperPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new GrasshopperPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new MosquitoPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new LadybugPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, new PillbugPiece().SetOwner(1));
    }

    public Dictionary<int, PlayablePiece> GetPlayablePieces()
    {
        return _playablePieces;
    }

    public Dictionary<int, GhostPiece> GetGhostPieces()
    {
        return _ghostPieces;
    }

    public Dictionary<int, PlayablePiece> GetWhiteReserve()
    {
        return _whiteReserve;
    }

    public Dictionary<int, PlayablePiece> GetBlackReserve()
    {
        return _blackReserve;
    }


    public void Spawn(PlayablePiece newPiece, GhostPiece oldPiece)
    {
        //TODO: rimuovere pezzo dalla riserva
        _playablePieces.Add(_nextIndex++, newPiece);
        newPiece.Place(oldPiece.GetCoords());
        newPiece.SetNeighbors(oldPiece.GetNeighbors());
        foreach (GhostPiece p in newPiece.CreateGhostNeighbors()) { _ghostPieces.Add(_nextIndex++, p); }
    }
}
