using System.Collections.Generic;
using UnityEngine;

public class SackManager : MonoBehaviour
{
    public static SackManager Instance { get; private set; }

    [Header("Level Configuration")]
    [SerializeField] private LevelPuzzleData levelData;

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnLocation;

    [Header("Scene References")]
    [Tooltip("Kumpulkan semua slot di level ini (baik merah maupun hijau)")]
    [SerializeField] private List<PuzzleSlot> allSlots; 
    [SerializeField] private GameObject trashBinObject;
    [SerializeField] private float snapDistance = 1.2f;
    [SerializeField] private GameObject UIWin;

    private int totalBricksNeeded;
    private int currentBricksPlaced = 0;
    private bool hasActivePiece = false;

    public Transform TrashBinTransform => trashBinObject != null ? trashBinObject.transform : null;
    public float SnapDistance => snapDistance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SetupLevel();
    }

    private void SetupLevel()
    {
        if (levelData == null)
        {
            Debug.LogError("Level Data belum dimasukkan ke SackManager!");
            return;
        }

        totalBricksNeeded = levelData.totalBricksNeeded;
        currentBricksPlaced = 0;

        // Nyalakan / matikan tong sampah berdasarkan data level
        if (trashBinObject != null)
        {
            trashBinObject.SetActive(levelData.enableTrashBin);
        }

        if (UIWin != null) UIWin.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (hasActivePiece || currentBricksPlaced >= totalBricksNeeded)
            return;

        SpawnPiece();
    }

    private void SpawnPiece()
    {
        if (levelData == null) return;

        DraggablePiece prefabToSpawn = levelData.GetRandomPrefab();
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Pool prefab kosong di Level Data!");
            return;
        }

        Vector3 spawnPos = spawnLocation != null ? spawnLocation.position : transform.position + Vector3.up * 1.5f;
        spawnPos.z = 0;

        DraggablePiece spawned = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        spawned.Init(allSlots);

        hasActivePiece = true;
    }

    public void OnPieceCleared()
    {
        hasActivePiece = false;
    }

    public void OnBrickPlaced()
    {
        currentBricksPlaced++;
        Debug.Log($"Progres Bata: {currentBricksPlaced}/{totalBricksNeeded}");

        if (currentBricksPlaced >= totalBricksNeeded)
        {
            if (UIWin != null) UIWin.SetActive(true);
        }
    }
}