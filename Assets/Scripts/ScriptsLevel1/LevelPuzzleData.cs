using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PieceSpawnWeight
{
    public DraggablePiece prefab;
    [Tooltip("Semakin besar angkanya, semakin sering item ini keluar")]
    [Range(1, 100)] public int weight = 10;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Puzzle/Level Data")]
public class LevelPuzzleData : ScriptableObject
{
    [Header("Level Info")]
    public string levelName = "Level 1-1";

    [Header("Win Condition")]
    [Tooltip("Total gabungan semua slot bata yang harus terisi agar menang")]
    public int totalBricksNeeded = 6;

    [Header("Mechanic Toggle")]
    [Tooltip("Centang jika level ini membutuhkan tong sampah")]
    public bool enableTrashBin = false;

    [Header("Spawn Pool")]
    [Tooltip("Daftar item yang bisa keluar dari karung beserta bobot peluangnya")]
    public List<PieceSpawnWeight> spawnPool = new List<PieceSpawnWeight>();

    /// <summary>
    /// Mengambil prefab acak menggunakan Weighted Random
    /// </summary>
    public DraggablePiece GetRandomPrefab()
    {
        if (spawnPool == null || spawnPool.Count == 0) return null;

        int totalWeight = 0;
        foreach (var entry in spawnPool)
        {
            if (entry.prefab != null) totalWeight += entry.weight;
        }

        if (totalWeight <= 0) return null;

        int roll = UnityEngine.Random.Range(0, totalWeight);
        int accumulated = 0;

        foreach (var entry in spawnPool)
        {
            if (entry.prefab == null) continue;

            accumulated += entry.weight;
            if (roll < accumulated)
            {
                return entry.prefab;
            }
        }

        return spawnPool[0].prefab;
    }
}