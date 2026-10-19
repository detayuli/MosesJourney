using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Panggil fungsi ini saat player MENANG / Selesai Membaca Story
    public static void UnlockNextLevel(string nextLevelID)
    {
        PlayerPrefs.SetInt("UNLOCKED_" + nextLevelID, 1);
        PlayerPrefs.Save();
    }

    // Panggil ini jika mau mereset progress (misal untuk testing/cheat)
    [ContextMenu("Reset All Progress")]
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Progress Level Berhasil Direset!");
    }
}