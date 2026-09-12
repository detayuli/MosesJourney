using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    Brick,
    Stone
}

[RequireComponent(typeof(Collider2D))]
public class DraggablePiece : MonoBehaviour
{
    [SerializeField] private PieceType pieceType;

    private bool isDragging = false;
    private bool isPlaced = false;
    private Vector2 offset;
    private List<PuzzleSlot> availableSlots;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void Init(List<PuzzleSlot> slots)
    {
        availableSlots = slots;
    }

    private void Update()
    {
        if (isDragging && !isPlaced)
        {
            Vector2 mousePos = GetMousePos();
            transform.position = mousePos - offset;
        }
    }

    private void OnMouseDown()
    {
        if (isPlaced) return;

        isDragging = true;
        offset = GetMousePos() - (Vector2)transform.position;
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

        // 1. Cek Tong Sampah (Khusus Batu)
        if (trashBin != null && Vector2.Distance(transform.position, trashBin.position) < snapDist)
        {
            if (pieceType == PieceType.Stone)
            {
                Debug.Log("Batu berhasil dibuang!");
                SackManager.Instance.OnPieceCleared();
                Destroy(gameObject);
                return;
            }
            else
            {
                Debug.Log("Bata salah ditaruh di tong sampah!");
                FailAndDestroy();
                return;
            }
        }

        // 2. Cek Slot Bata (Khusus Bata)
        if (pieceType == PieceType.Brick && availableSlots != null)
        {
            PuzzleSlot closestSlot = null;
            float minDistance = float.MaxValue;

            foreach (var slot in availableSlots)
            {
                if (slot != null && !slot.IsPlaced)
                {
                    float dist = Vector2.Distance(transform.position, slot.transform.position);
                    if (dist < minDistance && dist < snapDist)
                    {
                        minDistance = dist;
                        closestSlot = slot;
                    }
                }
            }

            if (closestSlot != null)
            {
                // Snap posisi bata langsung ke titik slot
                transform.position = closestSlot.transform.position;
                isPlaced = true;

                // Matikan collider agar bata yang tertempel tidak bisa di-klik/drag lagi
                if (col != null) col.enabled = false;

                closestSlot.Placed();
                SackManager.Instance.OnBrickPlaced();
                SackManager.Instance.OnPieceCleared();
                return;
            }
        }

        // 3. Jika dilepas di tempat yang salah / tidak pas pada slot
        FailAndDestroy();
    }

    private void FailAndDestroy()
    {
        SackManager.Instance.OnPieceCleared();
        Destroy(gameObject);
    }

    private Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}