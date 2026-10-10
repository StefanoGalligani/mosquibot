using System;
using System.Collections.Generic;

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
    private int[] _coords = {0, 0, 0};
    private Piece[] _neighbors = {null, null, null, null, null, null};
    private PieceType _type;

    // TYPE //

    protected void SetPieceType(PieceType type) { _type = type; }

    public PieceType GetPieceType() { return _type; }

    // COORDS //
    public void SetCoords(int[] coords) { _coords[0] = coords[0]; _coords[1] = coords[1]; _coords[2] = coords[2];}
    public void SetCoordsOnTop(int[] coords) {
        _coords[0] = coords[0]; _coords[1] = coords[1]; _coords[2] = coords[2] + 1;
    }
    public int[] GetCoords() { return _coords; }

    // NEIGHBORS //

    public static int[] CoordsFromIndex(int[] origCoords, int i)
    {
        switch (i){
            case 0: return new int[] {origCoords[0] + 0, origCoords[1] + 1, 0};
            case 1: return new int[] {origCoords[0] + 1, origCoords[1] + 1, 0};
            case 2: return new int[] {origCoords[0] + 1, origCoords[1] + 0, 0};
            case 3: return new int[] {origCoords[0] - 0, origCoords[1] - 1, 0};
            case 4: return new int[] {origCoords[0] - 1, origCoords[1] - 1, 0};
            case 5: return new int[] {origCoords[0] - 1, origCoords[1] - 0, 0};
            default: return new int[] {0, 0, 0};
        }
    }

    public static void JoinNeighbors (Piece p1, Piece p2, int dir)
    {
        p1._neighbors[dir] = p2;
        if (p2 != null) p2._neighbors[(dir + 3) % 6] = p2;
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
        for (int i=0; i<6; ++i)
        {
            if (_neighbors[i] == null)
            {
                GhostPiece p = new GhostPiece(); //TODO: non istanziare ma prenderli da una pool
                _neighbors[i] = p;
                p.SetCoords(CoordsFromIndex(_coords, i));
                newPieces.Add(p);
                JoinNeighbors(this, p, i);
            }
        }
        JoinNeighbors(_neighbors[0], _neighbors[1], 2);
        JoinNeighbors(_neighbors[1], _neighbors[2], 3);
        JoinNeighbors(_neighbors[2], _neighbors[3], 4);
        JoinNeighbors(_neighbors[3], _neighbors[4], 5);
        JoinNeighbors(_neighbors[4], _neighbors[5], 0);
        JoinNeighbors(_neighbors[5], _neighbors[0], 1);
        return newPieces;
    }
}

public abstract class PlayablePiece : Piece
{
    private int _owner = 0;

    // OWNER //
    public void SetOwner(int owner) { _owner = owner; }
    public int GetOwner() { return _owner; }

}

public sealed class GhostPiece : Piece {
    public GhostPiece()
    {
        SetPieceType(PieceType.ghost);
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