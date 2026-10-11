using UnityEngine;
using UnityEngine.UI;

public class PieceIcon : MonoBehaviour
{
    [SerializeField] private Sprite[] _pieceSprites;
    [SerializeField] private Image[] _colorRenderers;
    [SerializeField] private Image _pieceRenderer;

    private Piece _referencePiece;
    public void AddReference(PlayablePiece piece)
    {
        _referencePiece = piece;

        foreach (var r in _colorRenderers) r.color = (piece.GetOwner() == 0) ?
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
    }

    public Piece GetReference() { return _referencePiece; }
}
