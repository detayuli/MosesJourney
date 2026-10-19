using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    [SerializeField] private PieceType acceptedType = PieceType.BeigeBrick; // Properti ini yang dipanggil oleh DraggablePiece

    // Properti ini yang dipanggil oleh DraggablePiece
    public PieceType AcceptedType => acceptedType;

    public bool IsPlaced { get; private set; } = false;

    public void Placed()
    {
        IsPlaced = true;
        // audiomanager.Instance?.SFXSource.PlayOneShot(audiomanager.Instance.PressSound);
    }
}