using System.Collections.Generic;
using UnityEngine;

public enum PieceType { RedBrick, GreenBrick, Stone }

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
        AudioManager.Instance?.PlaySFX("DragPuzzle");
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

        // 1. Cek Tong Sampah (Semua item bisa dibuang ke sini)
        if (trashBin && trashBin.gameObject.activeInHierarchy && Vector2.Distance(transform.position, trashBin.position) < snapDist)
        {
            AudioManager.Instance?.PlaySFX("TrashBin"); // <-- Ganti string SFX-nya di sini
            SackManager.Instance.OnPieceCleared();
            Destroy(gameObject);
            return;
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

                    // Jika slot belum terisi dan tipenya cocok -> Benar
                    if (!slot.IsPlaced && slot.AcceptedType == pieceType)
                    {
                        transform.position = slot.transform.position;
                        isPlaced = true;
                        if (col) col.enabled = false;

                        slot.Placed();
                        AudioManager.Instance?.PlaySFX("RightDrop");
                        SackManager.Instance.OnBrickPlaced();
                        SackManager.Instance.OnPieceCleared();
                        return;
                    }
                }
            }
        }

        // 3. Penentuan SFX Balik:
        transform.position = initialPos;

        if (nearAnySlot)
        {
            // Dilepas dekat slot tapi salah jenis atau sudah penuh
            AudioManager.Instance?.PlaySFX("WrongDrop");
        }
        else
        {
            // Dilepas di sembarang tempat/tengah layar
            AudioManager.Instance?.PlaySFX("ResetDrop");
        }
    }
}