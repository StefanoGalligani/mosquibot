using System.Collections.Generic;
using UnityEngine;

public enum PieceType {
    ghost,
    queen,
    ant,
    spider,
    beetle,
    grasshopper,
    mosquito,
    ladybug,
    pillbug
}

public abstract class Piece {
    protected Vector3Int _coords = Vector3Int.zero;
    private Piece[] _neighbors = {null, null, null, null, null, null};
    private PieceType _type;
    private static Dictionary<Vector2Int, Stack<Piece>> _posToPiece = new Dictionary<Vector2Int, Stack<Piece>>();

    // TYPE //

    protected void SetPieceType(PieceType type) { _type = type; }

    public PieceType GetPieceType() { return _type; }

    // POS2PIECE //

    private void AddToDictionary()
    {
        Vector2Int pos = new Vector2Int(_coords.x, _coords.y);

        if (!_posToPiece.TryGetValue(pos, out Stack<Piece> stack)) {
            stack = new Stack<Piece>();
            _posToPiece.Add(pos, stack);
        }
        stack.Push(this);
    }

    // COORDS //
    protected abstract void PlacementLogic(Vector3Int coords);
    public void Place(Vector3Int coords) { PlacementLogic(coords); AddToDictionary(); }
    public Vector3Int GetCoords() { return _coords; }

    // NEIGHBORS //

    public static Vector3Int CoordsFromIndex(Vector3Int origCoords, int i)
    {
        switch (i){
            case 0: return new Vector3Int (origCoords.x + 0, origCoords.y + 1, 0);
            case 1: return new Vector3Int (origCoords.x + 1, origCoords.y + 1, 0);
            case 2: return new Vector3Int (origCoords.x + 1, origCoords.y + 0, 0);
            case 3: return new Vector3Int (origCoords.x - 0, origCoords.y - 1, 0);
            case 4: return new Vector3Int (origCoords.x - 1, origCoords.y - 1, 0);
            case 5: return new Vector3Int (origCoords.x - 1, origCoords.y - 0, 0);
            default: return new Vector3Int (0, 0, 0);
        }
    }

    private void CheckForNewNeighbors()
    {
        for (byte i = 0; i < 6; i++)
        {
            if (_neighbors[i] != null) continue;
            Vector3Int nPos = CoordsFromIndex(_coords, i);
            if (_posToPiece.TryGetValue((Vector2Int)nPos, out Stack<Piece> stack))
            {
                JoinNeighbors(this, stack.Peek(), i);
            }
        }
    }

    protected abstract void UpdateNeighborInfo(Piece newNeighbor, byte dir);

    public static void JoinNeighbors (Piece p1, Piece p2, byte dir)
    {
        if (p1 != null) {
            p1._neighbors[dir] = p2; p1.UpdateNeighborInfo(p2, dir); }
        if (p2 != null) { byte d1 = (byte)((dir + 3) % 6);
            p2._neighbors[d1] = p1; p2.UpdateNeighborInfo(p1, d1); }
    }

    public void SetNeighbors(Piece[] neighbors) {
        for (byte i = 0; i < 6; i++)
        {
            JoinNeighbors(this, neighbors[i], i);
        }
    }

    public Piece[] GetNeighbors() { return _neighbors; }
    public List<GhostPiece> CreateGhostNeighbors()
    {
        List<GhostPiece> newPieces = new List<GhostPiece>();
        for (byte i=0; i<6; ++i) {
            if (_neighbors[i] == null) {
                GhostPiece p = new GhostPiece(); //TODO: non istanziare ma prenderli da una pool
                p.Place(CoordsFromIndex(_coords, i));
                newPieces.Add(p);
                JoinNeighbors(this, p, i);
                JoinNeighbors(_neighbors[(i + 5) % 6], _neighbors[i], (byte)((i + 1) % 6));
            }
        }

        for (int i=0; i<6; ++i) {
            _neighbors[i].CheckForNewNeighbors();
        }

        return newPieces;
    }
}

public abstract class PlayablePiece : Piece
{
    private byte _owner = 0;
    private bool _placed = false;
    private Piece _coveredPiece;

    // OWNER //
    public PlayablePiece SetOwner(byte owner) { _owner = owner; return this; }
    public byte GetOwner() { return _owner; }

    // PLACEMENT //
    protected override void PlacementLogic(Vector3Int coords) {
        _coords = coords;
        _coords.z ++;
        _placed = true;
    }

    public void SetCoveredPiece(Piece p) { _coveredPiece = p; }

    public bool IsPlaced() { return _placed; }

    // NEIGHBORS

    protected override void UpdateNeighborInfo(Piece newNeighbor, byte dir)
    {
        // TODO implementarlo nelle sottoclassi
    }
}

public sealed class GhostPiece : Piece {
    private byte[] _neighborColorsCount = {0, 0, 6};
    private byte[] _neighbourColors = {2, 2, 2, 2, 2, 2};
    private byte _spawnColor = 2;
    private byte _index;
    private GameState _state;

    public GhostPiece()
    {
        SetPieceType(PieceType.ghost);
    }

    public GhostPiece SetIndexAndState(byte index, GameState state)
    {
        _index = index;
        _state = state;
        return this;
    }

    protected override void PlacementLogic(Vector3Int coords) {
        _coords = coords;
    }

    // NEIGHBORS

    protected override void UpdateNeighborInfo(Piece newNeighbor, byte dir)
    {
        byte newColor = newNeighbor != null && newNeighbor is PlayablePiece ?
            ((PlayablePiece)newNeighbor).GetOwner() : (byte)2;
        if (_neighbourColors[dir] != newColor)
        {
            _neighborColorsCount[_neighbourColors[dir]] --;
            _neighborColorsCount[newColor] ++;
            _neighbourColors[dir] = newColor;

            byte prevColor = _spawnColor;

            if      (_neighborColorsCount[0] != 0 && _neighborColorsCount[1] == 0) _spawnColor = 0;
            else if (_neighborColorsCount[0] == 0 && _neighborColorsCount[1] != 0) _spawnColor = 1;
            else _spawnColor = 2;

            if (_state != null && prevColor != _spawnColor)
                _state.UpdateSpawnInfo(_index, prevColor, _spawnColor, this);
        }
    }

    public byte GetSpawnColor() { return _spawnColor; }

}

public sealed class QueenPiece : PlayablePiece {
    public QueenPiece()
    {
        SetPieceType(PieceType.queen);
    }
}

public sealed class AntPiece : PlayablePiece {
    public AntPiece()
    {
        SetPieceType(PieceType.ant);
    }
}

public sealed class SpiderPiece : PlayablePiece {
    public SpiderPiece()
    {
        SetPieceType(PieceType.spider);
    }
}

public sealed class BeetlePiece : PlayablePiece {
    public BeetlePiece()
    {
        SetPieceType(PieceType.beetle);
    }
}

public sealed class GrasshopperPiece : PlayablePiece {
    public GrasshopperPiece()
    {
        SetPieceType(PieceType.grasshopper);
    }
}

public sealed class MosquitoPiece : PlayablePiece {
    public MosquitoPiece()
    {
        SetPieceType(PieceType.mosquito);
    }
}

public sealed class LadybugPiece : PlayablePiece {
    public LadybugPiece()
    {
        SetPieceType(PieceType.ladybug);
    }
}

public sealed class PillbugPiece : PlayablePiece {
    public PillbugPiece()
    {
        SetPieceType(PieceType.pillbug);
    }
}