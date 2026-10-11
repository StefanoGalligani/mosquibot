using Unity.VisualScripting;
using UnityEngine;

public class PieceObject : MonoBehaviour
{
    [SerializeField] private Sprite[] _pieceSprites;
    [SerializeField] private SpriteRenderer[] _colorRenderers;
    [SerializeField] private SpriteRenderer _pieceRenderer;

    private Piece _referencePiece;
    private static Vector3 _xDir = new Vector3(0.88f, 0, 0);
    private static Vector3 _yDir = new Vector3(-0.44f, 0, 0.76f);
    private static Vector3 _zDir = new Vector3(0, 0.2f, 0);

    public void UpdatePosition()
    {
        Vector3Int coords = _referencePiece.GetCoords();
        transform.position = _xDir * coords.x + _yDir * coords.y + _zDir * coords.z;
        foreach (var r in _colorRenderers) r.sortingOrder = coords[2] * 2;
        if (_pieceRenderer) _pieceRenderer.sortingOrder = coords[2] * 2 + 1;
    }

    public void AddReference(Piece piece)
    {
        _referencePiece = piece;
        UpdatePosition();

        if (piece is PlayablePiece) {
            foreach (var r in _colorRenderers) r.color = (((PlayablePiece)piece).GetOwner() == 0) ?
                Color.white : Color.black;

            Sprite sprite;
            switch (piece.GetPieceType())
            {
                case PieceType.queen:       sprite = _pieceSprites[1]; break;
                case PieceType.ant:         sprite = _pieceSprites[2]; break;
                case PieceType.spider:      sprite = _pieceSprites[3]; break;
                case PieceType.beetle:      sprite = _pieceSprites[4]; break;
                case PieceType.grasshopper: sprite = _pieceSprites[5]; break;
                case PieceType.mosquito:    sprite = _pieceSprites[6]; break;
                case PieceType.ladybug:     sprite = _pieceSprites[7]; break;
                case PieceType.pillbug:     sprite = _pieceSprites[8]; break;
                default: sprite = _pieceSprites[0]; break;
            }
            _pieceRenderer.sprite = sprite;
        } else {
            foreach (var r in _colorRenderers) r.color = Color.gray;
        }
    }

    public Piece GetReference() { return _referencePiece; }
}
