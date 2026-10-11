using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private PlayablePiece _selectedPiece;

    void Update()
    {
        if (Mouse.current == null) return;
        CheckLeftButton();
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
                    _gameManager.PlacePiece(new AntPiece(), (GhostPiece)pObj.GetReference());
                }
            }
        }
    }
}
