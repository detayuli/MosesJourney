using System.Collections.Generic;
using UnityEngine;

public enum PieceType { BeigeBrick, RedBrick, Stone }

[RequireComponent(typeof(Collider2D))]
public class DraggablePiece : MonoBehaviour
{
    [SerializeField] private PieceType pieceType;

    private bool isDragging, isPlaced;
    private Vector2 offset, initialPos;
    private List<PuzzleSlot> availableSlots;
    private Collider2D col;
    private Camera mainCam;

    public PieceType Type => pieceType;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        mainCam = Camera.main;
    }

    private void Start() => initialPos = transform.position;

    public void Init(List<PuzzleSlot> slots) => availableSlots = slots;

    private void Update()
    {
        if (isDragging && !isPlaced)
            transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition) - offset;
    }

    private void OnMouseDown()
    {
        if (isPlaced) return;
        isDragging = true;
        offset = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        AudioManager.Instance?.PlaySFX("event:/SFX/UISFX/DragPuzzle");
    }

    private void OnMouseUp()
    {
        if (isPlaced) return;
        isDragging = false;
        CheckPlacement();
    }

private void CheckPlacement()
    {
        float snapDist = SackManager.Instance.SnapDistance;
        Transform trashBin = SackManager.Instance.TrashBinTransform;

        // 1. Cek Tong Sampah menggunakan Collider
        if (trashBin && trashBin.gameObject.activeInHierarchy)
        {
            Collider2D trashCol = trashBin.GetComponent<Collider2D>();

            if (trashCol != null && (col.IsTouching(trashCol) || trashCol.OverlapPoint(transform.position)))
            {
                AudioManager.Instance?.PlaySFX("event:/SFX/UISFX/TrashBin");
                
                GuardManager.Instance?.TriggerGuardReaction(GuardActionType.BuangSampah);

                SackManager.Instance.OnPieceCleared();
                Destroy(gameObject);
                return;
            }
        }

        // 2. Cek Slot Target
        bool nearAnySlot = false;

        if (availableSlots != null)
        {
            foreach (var slot in availableSlots)
            {
                if (!slot) continue;

                float dist = Vector2.Distance(transform.position, slot.transform.position);

                if (dist < snapDist)
                {
                    nearAnySlot = true;

                    // Penempatan Benar
                    if (!slot.IsPlaced && slot.AcceptedType == pieceType)
                    {
                        transform.position = slot.transform.position;
                        isPlaced = true;
                        if (col) col.enabled = false;

                        slot.Placed();
                        AudioManager.Instance?.PlaySFX("event:/SFX/UISFX/RightDrop");

                        // 1. Hitung progres bata terlebih dahulu
                        SackManager.Instance.OnBrickPlaced();
                        SackManager.Instance.OnPieceCleared();

                        // 2. Jika level BELUM selesai, panggil VO Guard biasa.
                        // Jika level SUDAH selesai, skip agar VO Finish di SackManager yang terputar.
                        if (!SackManager.Instance.IsLevelComplete)
                        {
                            GuardManager.Instance?.TriggerGuardReaction(GuardActionType.Benar);
                        }

                        return;
                    }
                }
            }
        }

        // 3. Penentuan SFX & Posisikan Ulang
        transform.position = initialPos;

        if (nearAnySlot)
        {
            AudioManager.Instance?.PlaySFX("event:/SFX/UISFX/WrongDrop");
            GuardManager.Instance?.TriggerGuardReaction(GuardActionType.Salah);
        }
        else
        {
            AudioManager.Instance?.PlaySFX("event:/SFX/UISFX/ResetDrop");
        }
    }
}