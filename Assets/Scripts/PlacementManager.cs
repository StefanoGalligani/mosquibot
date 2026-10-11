using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private PieceIcon _candidatePiece;

    void Update()
    {
        if (Mouse.current == null) return;
        CheckLeftButton();
    }

    public bool TrySetCandidate(PieceIcon i)
    {
        if (i.GetReference().GetOwner() == _gameManager.GetCurrentState().GetTurn()) {
            _candidatePiece = i;
            return true;
        }
        return false;
    }

    private int FindIndexByGhostPiece(GhostPiece piece)
    {
        Dictionary<int, GhostPiece> dict = _gameManager.GetCurrentState().GetGhostPieces();
        foreach (KeyValuePair<int, GhostPiece> pair in dict)
        {
            if (pair.Value == piece) return pair.Key;
        }
        return -1;
    }

    private int FindIndexByPlayablePiece(PlayablePiece piece)
    {
        Dictionary<int, PlayablePiece> dict = piece.GetOwner() == 0 ?
            _gameManager.GetCurrentState().GetWhiteReserve() :
            _gameManager.GetCurrentState().GetBlackReserve();
        foreach (KeyValuePair<int, PlayablePiece> pair in dict)
        {
            if (pair.Value == piece) return pair.Key;
        }
        return -1;
    }

    private void TryPlacingPiece(GhostPiece ghost)
    {
        if (_candidatePiece == null) return;
        _gameManager.PlacePiece(
            FindIndexByPlayablePiece(_candidatePiece.GetReference()),
            FindIndexByGhostPiece(ghost));
        _candidatePiece = null;
    }

    private void CheckLeftButton() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                GameObject clickedObject = hit.collider.gameObject;
                PieceObject pObj = clickedObject.GetComponent<PieceObject>();
                if (pObj == null) return;

                if (pObj.GetReference() is GhostPiece)
                {
                    TryPlacingPiece((GhostPiece)pObj.GetReference());
                }
            }
        }
    }
}
