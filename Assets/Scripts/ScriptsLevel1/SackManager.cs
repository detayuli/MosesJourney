using System.Collections.Generic;
using UnityEngine;

public class SackManager : MonoBehaviour
{
    public static SackManager Instance { get; private set; }

    [Header("Item Prefabs")]
    [SerializeField] private DraggablePiece brickPrefab;
    [SerializeField] private DraggablePiece stonePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnLocation;

    [Header("Target References")]
    [SerializeField] private List<PuzzleSlot> brickSlots; // Masukkan 6 slot bata di inspector
    [SerializeField] private Transform trashBinTransform; // Objek tempat sampah
    [SerializeField] private float snapDistance = 1.2f;

    [Header("Game State")]
    public int totalBricksNeeded = 6;
    private int currentBricksPlaced = 0;
    private bool hasActivePiece = false;

    public Transform TrashBinTransform => trashBinTransform;
    public float SnapDistance => snapDistance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        // Cegah klik kalau masih ada item yang belum selesai ditaruh atau game sudah beres
        if (hasActivePiece || currentBricksPlaced >= totalBricksNeeded)
            return;

        SpawnRandomPiece();
    }

    private void SpawnRandomPiece()
    {
        Vector3 spawnPos = spawnLocation != null ? spawnLocation.position : transform.position + Vector3.up * 1.5f;
        spawnPos.z = 0;

        // Random 50:50 bata atau batu
        DraggablePiece prefabToSpawn = (Random.value > 0.5f) ? brickPrefab : stonePrefab;

        DraggablePiece spawned = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        spawned.Init(brickSlots);

        hasActivePiece = true;
    }

    public void OnPieceCleared()
    {
        hasActivePiece = false;
    }

    public void OnBrickPlaced()
    {
        currentBricksPlaced++;
        Debug.Log($"Bata terpasang: {currentBricksPlaced}/{totalBricksNeeded}");

        if (currentBricksPlaced >= totalBricksNeeded)
        {
            Debug.Log("Level Selesai! Semua susunan bata sudah penuh.");
        }
    }
}