using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public sealed class GameState {
    private Dictionary<byte, PlayablePiece> _playablePieces;
    private Dictionary<byte, GhostPiece> _ghostPieces;
    private Dictionary<byte, PlayablePiece> _whiteReserve;
    private Dictionary<byte, PlayablePiece> _blackReserve;
    private Dictionary<byte, PlayablePiece> _whiteQueenOnlyReserve;
    private Dictionary<byte, PlayablePiece> _blackQueenOnlyReserve;
    private Dictionary<byte, GhostPiece> _whiteSpawns;
    private Dictionary<byte, GhostPiece> _blackSpawns;
    private byte _nextIndex = 0;
    private byte _turn = 0;
    private byte _move = 0;
    private bool _whiteQueenPlaced = false;
    private bool _blackQueenPlaced = false;
    private byte _wqIndex;
    private byte _bqIndex;
    public GameState()
    {
        _playablePieces =        new Dictionary<byte, PlayablePiece>();
        _ghostPieces =           new Dictionary<byte, GhostPiece>();
        _whiteReserve =          new Dictionary<byte, PlayablePiece>();
        _blackReserve =          new Dictionary<byte, PlayablePiece>();
        _whiteQueenOnlyReserve = new Dictionary<byte, PlayablePiece>();
        _blackQueenOnlyReserve = new Dictionary<byte, PlayablePiece>();
        _whiteSpawns =           new Dictionary<byte, GhostPiece>();
        _blackSpawns =           new Dictionary<byte, GhostPiece>();

        GhostPiece firstPiece = new GhostPiece();
        firstPiece.Place(new Vector3Int(0, 0, 0));
        _ghostPieces.Add(_nextIndex, firstPiece.SetIndexAndState(_nextIndex, this));
        _whiteSpawns.Add(_nextIndex++, firstPiece);

        _whiteQueenOnlyReserve.Add(_wqIndex = _nextIndex++, new QueenPiece().SetOwner(0));
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

        _blackQueenOnlyReserve.Add(_bqIndex = _nextIndex++, new QueenPiece().SetOwner(1));
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

    public Dictionary<byte, PlayablePiece> GetPlayablePieces()
    {
        return _playablePieces;
    }

    public Dictionary<byte, GhostPiece> GetGhostPieces()
    {
        return _ghostPieces;
    }

    public Dictionary<byte, PlayablePiece> GetWhiteReserve()
    {
        return (_move == 6 && !_whiteQueenPlaced) ? _whiteQueenOnlyReserve : _whiteReserve;
    }

    public Dictionary<byte, PlayablePiece> GetBlackReserve()
    {
        return (_move == 7 && !_blackQueenPlaced) ? _blackQueenOnlyReserve : _blackReserve;
    }

    public Dictionary<byte, GhostPiece> GetWhiteSpawns()
    {
        return _whiteSpawns;
    }

    public Dictionary<byte, GhostPiece> GetBlackSpawns()
    {
        return _blackSpawns;
    }

    public byte GetTurn() { return _turn; }
    public int GetMove() { return _move; }

    public void Spawn(byte newPieceIndex, byte oldPieceIndex)
    {
        var reserve =     _turn == 0 ?              _whiteReserve : _blackReserve;
        var startSpawns = _turn == 0 || _move == 1? _whiteSpawns : _blackSpawns;
        var newSpawns =   _turn == 0 ?              _whiteSpawns : _blackSpawns;
        PlayablePiece newPiece = reserve.GetValueOrDefault(newPieceIndex);
        GhostPiece oldPiece = startSpawns.GetValueOrDefault(oldPieceIndex);

        reserve.Remove(newPieceIndex);
        startSpawns.Remove(oldPieceIndex);

        _playablePieces.Add(_nextIndex++, newPiece);
        newPiece.Place(oldPiece.GetCoords());
        newPiece.SetNeighbors(oldPiece.GetNeighbors());
        newPiece.SetCoveredPiece(oldPiece);
        foreach (GhostPiece p in newPiece.CreateGhostNeighbors()) {
            _ghostPieces.Add(_nextIndex, p.SetIndexAndState(_nextIndex, this));
            newSpawns.Add(_nextIndex++, p);
        }

        _turn = (byte)(1 - _turn); //REMINDER: va fatto dopo qualsiasi mossa
        _move++;

        if      (newPieceIndex == _wqIndex) _whiteQueenPlaced = true;
        else if (newPieceIndex == _bqIndex) _blackQueenPlaced = true;
        if      (_move == 1) _whiteReserve.Add(_wqIndex, _whiteQueenOnlyReserve.GetValueOrDefault(_wqIndex));
        else if (_move == 2) _blackReserve.Add(_bqIndex, _blackQueenOnlyReserve.GetValueOrDefault(_bqIndex));
    }

    public void UpdateSpawnInfo (byte ghostIndex, byte previousOwner, byte newOwner, GhostPiece p)
    {
        if      (previousOwner == 0) { _whiteSpawns.Remove(ghostIndex); }
        else if (previousOwner == 1) { _blackSpawns.Remove(ghostIndex); }
        if      (newOwner == 0)      { _whiteSpawns.Add(ghostIndex, p); }
        else if (newOwner == 1)      { _blackSpawns.Add(ghostIndex, p); }
    }
}
