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
        _currentState.Spawn(newPiece, oldPiece);
        _visualizer.ShowGameState(_currentState);
    }

}
