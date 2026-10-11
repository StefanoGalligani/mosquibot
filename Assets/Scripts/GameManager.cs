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
        PlacePiece(
            _currentState.GetWhiteReserve().ElementAt(0).Key,
            _currentState.GetGhostPieces().ElementAt(0).Key);
        PlacePiece(
            _currentState.GetBlackReserve().ElementAt(2).Key,
            _currentState.GetGhostPieces().ElementAt(1).Key);
    }

    public GameState GetCurrentState() { return _currentState; }
    
    public void PlacePiece (int newPiece, int oldPiece)
    {
        _currentState.Spawn(newPiece, oldPiece);
        _visualizer.ShowGameState(_currentState);
    }

}
