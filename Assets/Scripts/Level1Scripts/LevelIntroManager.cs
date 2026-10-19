using System.Collections;
using UnityEngine;

public class LevelIntroManager : MonoBehaviour
{
    public static LevelIntroManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject tutorialUIPanel;
    [SerializeField] private GameObject GuardTutorialUI;
    [SerializeField] private GameObject GuardUI;
    [SerializeField] private GameObject karung;

    [Header("Intro Configuration")]
    [SerializeField] private string introVOName;
    [SerializeField] private float defaultDelayIfNoAudio = 30f; 
    [SerializeField] private float tutorialDelayAfterVO = 0.5f;

    // Status untuk ngecek apakah intro/tutorial masih jalan
    public bool IsIntroActive { get; private set; } = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        IsIntroActive = true;

        // 1. Setup UI di awal
        if (tutorialUIPanel != null) tutorialUIPanel.SetActive(false);
        if (GuardTutorialUI != null) GuardTutorialUI.SetActive(true);
        if (GuardUI != null) GuardUI.SetActive(false);
        if (karung != null) karung.SetActive(false);

        // 2. Putar VO via AudioManager
        if (!string.IsNullOrEmpty(introVOName))
        {
            AudioManager.Instance?.PlayVoiceOver(introVOName);
        }

        // 3. Tunggu fixed 10 detik (+ delay tambahan kalau ada)
        yield return new WaitForSeconds(defaultDelayIfNoAudio + tutorialDelayAfterVO);

        // 4. Munculkan UI Tutorial
        ShowTutorialUI();
    }

    public void ShowTutorialUI()
    {
        if (tutorialUIPanel != null)
        {
            tutorialUIPanel.SetActive(true);
            if (GuardTutorialUI != null) GuardTutorialUI.SetActive(false);
        }
    }

    // Dipanggil OnClick() di tombol '>>' UI Tutorial
    public void CloseTutorialUI()
    {
        if (tutorialUIPanel != null)
        {
            tutorialUIPanel.SetActive(false);
            if (GuardTutorialUI != null) GuardTutorialUI.SetActive(false);
            if (GuardUI != null) GuardUI.SetActive(true);
            if (karung != null) karung.SetActive(true);
            
            AudioManager.Instance?.PlaySFX("ClickButton");
        }

        // Tandai intro selesai, gameplay di-unlock!
        IsIntroActive = false;
    }
}