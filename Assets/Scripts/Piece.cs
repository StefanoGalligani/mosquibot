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
        for (int i = 0; i < 6; i++)
        {
            if (_neighbors[i] != null) continue;
            Vector3Int nPos = CoordsFromIndex(_coords, i);
            if (_posToPiece.TryGetValue((Vector2Int)nPos, out Stack<Piece> stack))
            {
                JoinNeighbors(this, stack.Peek(), i);
            }
        }
    }


    public static void JoinNeighbors (Piece p1, Piece p2, int dir)
    {
        p1._neighbors[dir] = p2;
        if (p2 != null) p2._neighbors[(dir + 3) % 6] = p1;
    }

    public void SetNeighbors(Piece[] neighbors) {
        for (int i = 0; i < 6; i++)
        {
            JoinNeighbors(this, neighbors[i], i);
        }
    }

    public Piece[] GetNeighbors() { return _neighbors; }
    public List<GhostPiece> CreateGhostNeighbors()
    {
        List<GhostPiece> newPieces = new List<GhostPiece>();
        for (int i=0; i<6; ++i) {
            if (_neighbors[i] == null) {
                GhostPiece p = new GhostPiece(); //TODO: non istanziare ma prenderli da una pool
                p.Place(CoordsFromIndex(_coords, i));
                newPieces.Add(p);
                JoinNeighbors(this, p, i);
                JoinNeighbors(_neighbors[(i + 5) % 6], _neighbors[i], (i + 1) % 6);
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
    private int _owner = 0;
    private bool _placed = false;

    // OWNER //
    public PlayablePiece SetOwner(int owner) { _owner = owner; return this; }
    public int GetOwner() { return _owner; }

    // PLACEMENT //
    protected override void PlacementLogic(Vector3Int coords) {
        _coords = coords;
        _coords.y ++;
        _placed = true;
    }

    public bool IsPlaced() { return _placed; }
}

public sealed class GhostPiece : Piece {
    public GhostPiece()
    {
        SetPieceType(PieceType.ghost);
    }
    protected override void PlacementLogic(Vector3Int coords) {
        _coords = coords;
    }
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