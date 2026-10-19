using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButtonUI : MonoBehaviour
{
    [Header("Data Config")]
    [SerializeField] private LevelData levelData;

    [Header("UI Component References")]
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockOverlay; // Drag UI Icon Gembok / Overlay Hitam di sini

    private void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (levelData == null) return;

        // Cek apakah level sudah terbuka di PlayerPrefs (1 = Open, 0 = Locked)
        bool isUnlocked = PlayerPrefs.GetInt("UNLOCKED_" + levelData.levelID, levelData.isUnlockedByDefault ? 1 : 0) == 1;

        // Atur interaksi tombol & tampilkan ikon lock jika terkunci
        button.interactable = isUnlocked;
        if (lockOverlay != null) lockOverlay.SetActive(!isUnlocked);

        // Pasang event klik
        button.onClick.RemoveAllListeners();
        if (isUnlocked)
        {
            button.onClick.AddListener(OnLevelClicked);
        }
    }

    private void OnLevelClicked()
    {
        AudioManager.Instance?.PlaySFX("ButtonClick"); // Sesuaikan jika ada SFX
        SceneManager.LoadScene(levelData.sceneToLoad);
    }
}