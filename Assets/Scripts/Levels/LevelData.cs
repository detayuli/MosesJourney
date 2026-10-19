using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level System/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelID;       // Unique ID (misal: "Bab1_Story", "Bab1_Level1", "Bab1_Level2")
    public string sceneToLoad;   // Nama Scene Unity yang dipanggil
    public bool isUnlockedByDefault; // Centang TRUE khusus untuk "Story" (level pertama)
}