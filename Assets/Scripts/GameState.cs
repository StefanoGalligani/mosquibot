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
    private byte _moveCount = 0;
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
        _ghostPieces.Add(_nextIndex, (GhostPiece)firstPiece.SetIndexAndState(_nextIndex, this));
        _whiteSpawns.Add(_nextIndex++, firstPiece);

        _whiteQueenOnlyReserve.Add(_wqIndex = _nextIndex++, new QueenPiece().SetOwner(0));
        _whiteReserve.Add(_nextIndex++,
            (PlayablePiece)new AntPiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++,
            (PlayablePiece)new AntPiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++, 
            (PlayablePiece)new AntPiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++, 
            (PlayablePiece)new SpiderPiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++, 
            (PlayablePiece)new SpiderPiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++, 
            (PlayablePiece)new BeetlePiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++, 
            (PlayablePiece)new BeetlePiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++, 
            (PlayablePiece)new GrasshopperPiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++, 
            (PlayablePiece)new GrasshopperPiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++, 
            (PlayablePiece)new GrasshopperPiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++, 
            (PlayablePiece)new MosquitoPiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++, 
            (PlayablePiece)new LadybugPiece().SetOwner(0).SetIndexAndState(_nextIndex, this));
        _whiteReserve.Add(_nextIndex++, 
            (PlayablePiece)new PillbugPiece().SetOwner(0).SetIndexAndState(_nextIndex, this));

        _blackQueenOnlyReserve.Add(_bqIndex = _nextIndex++, new QueenPiece().SetOwner(1));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new AntPiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new AntPiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new AntPiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new SpiderPiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new SpiderPiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new BeetlePiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new BeetlePiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new GrasshopperPiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new GrasshopperPiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new GrasshopperPiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new MosquitoPiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new LadybugPiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
        _blackReserve.Add(_nextIndex++, 
            (PlayablePiece)new PillbugPiece().SetOwner(1).SetIndexAndState(_nextIndex, this));
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
        return (_moveCount == 6 && !_whiteQueenPlaced) ? _whiteQueenOnlyReserve : _whiteReserve;
    }

    public Dictionary<byte, PlayablePiece> GetBlackReserve()
    {
        return (_moveCount == 7 && !_blackQueenPlaced) ? _blackQueenOnlyReserve : _blackReserve;
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
    public int GetMoveCount() { return _moveCount; }

    public void ApplyMove(Move m)
    {
        m.PerformMove(this);
    }

    public void Spawn(byte newPieceIndex, byte oldPieceIndex)
    {
        var reserve =     _turn == 0 ?              _whiteReserve : _blackReserve;
        var startSpawns = _turn == 0 || _moveCount == 1? _whiteSpawns : _blackSpawns;
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
            _ghostPieces.Add(_nextIndex, (GhostPiece)p.SetIndexAndState(_nextIndex, this));
            newSpawns.Add(_nextIndex++, p);
        }

        _turn = (byte)(1 - _turn);
        _moveCount++;

        if      (newPieceIndex == _wqIndex) _whiteQueenPlaced = true;
        else if (newPieceIndex == _bqIndex) _blackQueenPlaced = true;
        if      (_moveCount == 1) _whiteReserve.Add(_wqIndex, _whiteQueenOnlyReserve.GetValueOrDefault(_wqIndex));
        else if (_moveCount == 2) _blackReserve.Add(_bqIndex, _blackQueenOnlyReserve.GetValueOrDefault(_bqIndex));
    }

    public void MovePiece(byte pieceIndex, byte positionIndex, bool isOnTopOfPiece)
    {
        PlayablePiece p = _playablePieces.GetValueOrDefault(pieceIndex);
        Piece destination = isOnTopOfPiece ? _playablePieces.GetValueOrDefault(positionIndex) :
                                             _ghostPieces.GetValueOrDefault(positionIndex);
        Piece previouslyCovered = p.GetCoveredPiece();

        Vector3Int newCoords = destination.GetCoords();
        newCoords.z++;
        p.Move(newCoords);
        //PRENDERE NUOVI VICINI DENTRO IL METODO, OVVERO COPIARE QUELLI DEL PEZZO COPERTO
        //SE UNO DEI VICINI è IL PEZZO CHE SI è APPENA MOSSO,
        //SOSTITUIRLO CON IL PEZZO PRECEDENTEMENTE COPERTO, SIA PER Sé STESSO CHE PER IL NUOVO PEZZO COPERTO
            p.SetCoveredPiece(destination); //METTERLO NEL METODO MOVE
            previouslyCovered.SetCovered(false); //METTERLO NEL METODO MOVE
        //GENERARE NUOVI GHOSTPIECE SE NECESSARIO

        //AGGIORNARE MOVIMENTI DI TUTTI I PEZZI COINVOLTI UNA VOLTA SOLA (creare hashset e scorrerlo?)
            //PRECEDENTI VICINI
            //NUOVI VICINI
            //NUOVO PEZZO COPERTO
            //PROPRI MOVIMENTI

        //Todo: controllo anelli
            //struttura che rappresenta un anello, una per ogni anello in gioco
            //quando viene creato un anello, creare la struttura e aggiornare i movimenti dei pezzi coinvolti
            //quando viene distrutto un anello, aggiornare i movimenti dei pezzi coinvolti e distruggere struttura

        _turn = (byte)(1 - _turn);
        _moveCount++;
    }

    public void UpdateSpawnInfo (byte ghostIndex, byte previousOwner, byte newOwner, GhostPiece p)
    {
        if      (previousOwner == 0) { _whiteSpawns.Remove(ghostIndex); }
        else if (previousOwner == 1) { _blackSpawns.Remove(ghostIndex); }
        if      (newOwner == 0)      { _whiteSpawns.Add(ghostIndex, p); }
        else if (newOwner == 1)      { _blackSpawns.Add(ghostIndex, p); }
    }
}
