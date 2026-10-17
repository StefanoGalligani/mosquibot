public abstract class Move
{
    public abstract void PerformMove(GameState state);
}

public sealed class SpawnMove : Move
{
    private byte _newPieceIndex;
    private byte _oldPieceIndex;

    public SpawnMove(byte newPieceIndex, byte oldPieceIndex)
    {
        _newPieceIndex = newPieceIndex;
        _oldPieceIndex = oldPieceIndex;
    }

    public override void PerformMove(GameState state)
    {
        state.Spawn(_newPieceIndex, _oldPieceIndex);
    }
}

public sealed class MovementMove : Move
{
    private byte _pieceIndex;
    private byte _positionIndex;
    private bool _isOnTopOfPiece;

    public MovementMove(byte pieceIndex, byte positionIndex, bool isOnTopOfPiece)
    {
        _pieceIndex = pieceIndex;
        _positionIndex = positionIndex;
        _isOnTopOfPiece = isOnTopOfPiece;
    }

    public override void PerformMove(GameState state)
    {
        state.MovePiece(_pieceIndex, _positionIndex, _isOnTopOfPiece);
    }
}