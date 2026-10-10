using Unity.VisualScripting;
using UnityEngine;

public class PieceObject : MonoBehaviour
{
    [SerializeField] private Sprite[] _pieceSprites;
    [SerializeField] private SpriteRenderer _colorRenderer;
    [SerializeField] private SpriteRenderer _pieceRenderer;

    private Piece _referencePiece;
    private static Vector3 _xDir = new Vector3(0.88f, 0, 0);
    private static Vector3 _yDir = new Vector3(-0.44f, 0.76f, 0);
    private static Vector3 _zDir = new Vector3(0, 0, -0.2f);

    public void UpdatePosition()
    {
        int[] coords = _referencePiece.GetCoords();
        transform.position = _xDir * coords[0] + _yDir * coords[1] + _zDir * coords[2];
    }

    public void AddReference(Piece piece)
    {
        _referencePiece = piece;
        UpdatePosition();

        if (piece is PlayablePiece) {
            _colorRenderer.color = (((PlayablePiece)piece).GetOwner() == 0) ? Color.white : Color.black;

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
            _colorRenderer.color = Color.gray;
        }
    }

    public Piece GetReference() { return _referencePiece; }
}
