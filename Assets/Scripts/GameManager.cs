using System.Linq;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private int _turn = 0;
    private bool[] _players = {true, true};
    private GameState _currentState;
    [SerializeField] private GameStateVisualizer _visualizer;

    void Start()
    {
        _currentState = new GameState();
        _visualizer.ShowGameState(_currentState);
        PlacePiece(new QueenPiece(), _currentState.GetGhostPieces().ElementAt(0).Value);
        PlacePiece(new QueenPiece(), _currentState.GetGhostPieces().ElementAt(1).Value);
    }

    public void PlacePiece (PlayablePiece newPiece, GhostPiece oldPiece)
    {
        _currentState.Spawn(_turn, newPiece, oldPiece);
        _turn = 1 - _turn;
        _visualizer.ShowGameState(_currentState);
    }

}
