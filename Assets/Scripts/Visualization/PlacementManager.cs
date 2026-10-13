using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private PieceIcon _candidatePiece;
    private bool _clickReady = false;
    private float _totalMovement = 0;

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

    private byte FindIndexByGhostPiece(GhostPiece piece)
    {
        Dictionary<byte, GhostPiece> dict = _gameManager.GetCurrentState().GetGhostPieces();
        foreach (KeyValuePair<byte, GhostPiece> pair in dict)
        {
            if (pair.Value == piece) return pair.Key;
        }
        return 255;
    }

    private byte FindIndexByPlayablePiece(PlayablePiece piece)
    {
        Dictionary<byte, PlayablePiece> dict = piece.GetOwner() == 0 ?
            _gameManager.GetCurrentState().GetWhiteReserve() :
            _gameManager.GetCurrentState().GetBlackReserve();
        foreach (KeyValuePair<byte, PlayablePiece> pair in dict)
        {
            if (pair.Value == piece) return pair.Key;
        }
        return 255;
    }

    private void TryPlacingPiece(PieceObject ghostObject)
    {
        if (_candidatePiece == null || !ghostObject.IsVisible()) return;
        _gameManager.PlacePiece(
            FindIndexByPlayablePiece(_candidatePiece.GetReference()),
            FindIndexByGhostPiece((GhostPiece)ghostObject.GetReference()));
        _candidatePiece = null;
    }

    private void CheckLeftButton() {
        if (!_clickReady && Mouse.current.leftButton.wasPressedThisFrame) {
            _clickReady = true;
            _totalMovement = 0;
        }
        _totalMovement += Mouse.current.delta.value.SqrMagnitude();
        if (_clickReady &&  _totalMovement > 5) _clickReady = false;
        if (!_clickReady || !Mouse.current.leftButton.wasReleasedThisFrame) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            GameObject clickedObject = hit.collider.gameObject;
            PieceObject pObj = clickedObject.GetComponent<PieceObject>();
            if (pObj == null) return;

            if (pObj.GetReference() is GhostPiece)
            {
                TryPlacingPiece(pObj);
            }
        }
    }
}
