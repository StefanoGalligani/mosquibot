using System.Linq;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private bool[] _players = {true, true};
    private GameState _currentState;
    [SerializeField] private GameStateVisualizer _visualizer;

    void Start()
    {
        _currentState = new GameState();
        _visualizer.ShowGameState(_currentState);
    }

    public GameState GetCurrentState() { return _currentState; }
    
    public void PlacePiece (byte newPiece, byte oldPiece)
    {
        Move m = new SpawnMove(newPiece, oldPiece);
        _currentState.ApplyMove(m);
        _visualizer.ShowGameState(_currentState);
    }

    public void MovePiece (byte piece, byte newPosition, byte pieceOwner, bool isOnTopOfPiece)
    {
        Move m = new MovementMove(piece, newPosition, pieceOwner, isOnTopOfPiece);
        _currentState.ApplyMove(m);
        _visualizer.ShowGameState(_currentState);
    }

}
