using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    RedBrick,
    GreenBrick,
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

    public PieceType Type => pieceType;

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

        // 1. Cek Tong Sampah (Khusus Batu dan Tong Sampah sedang aktif di level ini)
        if (trashBin != null && trashBin.gameObject.activeInHierarchy && 
            Vector2.Distance(transform.position, trashBin.position) < snapDist)
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
                Debug.Log("Bata tidak boleh dibuang ke tong sampah!");
                FailAndDestroy();
                return;
            }
        }

        // 2. Cek Slot Bata (Cocokkan Tipe Bata dengan Tipe Slot)
        if ((pieceType == PieceType.RedBrick || pieceType == PieceType.GreenBrick) && availableSlots != null)
        {
            PuzzleSlot closestSlot = null;
            float minDistance = float.MaxValue;

            foreach (var slot in availableSlots)
            {
                // Slot harus belum terisi dan tipenya cocok (Merah ke Merah, Hijau ke Hijau)
                if (slot != null && !slot.IsPlaced && slot.AcceptedType == pieceType)
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
                transform.position = closestSlot.transform.position;
                isPlaced = true;

                if (col != null) col.enabled = false;

                closestSlot.Placed();
                SackManager.Instance.OnBrickPlaced();
                SackManager.Instance.OnPieceCleared();
                return;
            }
        }

        // 3. Jika salah tempat / dilepas di sembarang area
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