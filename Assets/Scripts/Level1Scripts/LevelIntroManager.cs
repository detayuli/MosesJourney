using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class LevelIntroManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject tutorialUIPanel;
    [SerializeField] private GameObject GuardUI;


    [Header("Intro Configuration")]
    [TextArea(2, 4)]
    [SerializeField] private string introText = "Hei kamu bangsa Israel, buruan kerja! Susunlah batu bata dengan rapi, buang yang nggak penting, dan ingat... jangan sampai salah pasang!";
    [SerializeField] private string introVOName = "VOLvl1"; // Nama VO di AudioManager kamu
    [SerializeField] private float defaultDelayIfNoAudio = 5f;
    [SerializeField] private float tutorialDelayAfterVO = 0.5f; // Delay tambahan sebelum UI Tutorial muncul setelah VO selesai

    private void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        // 1. Pastikan UI Tutorial tidak aktif dulu di awal
        if (tutorialUIPanel != null)
            tutorialUIPanel.SetActive(false);

        // 2. Set teks narasi di UI
        if (dialogueText != null)
            dialogueText.text = introText;

        // 3. Putar VO via AudioManager
        if (!string.IsNullOrEmpty(introVOName))
        {
            AudioManager.Instance?.PlayVoiceOver(introVOName);
        }

        // 4. Cari durasi audio VO agar UI Tutorial muncul persis setelah VO selesai
        float waitTime = defaultDelayIfNoAudio;

        // Jika kamu bisa mengambil durasi clip dari AudioManager, tunggu sepanjang durasi VO
        // Misal tunggu manual atau panggil Coroutine penunggu
        yield return new WaitForSeconds(waitTime);

        // 5. Setelah VO selesai, tampilkan UI Tutorial
        ShowTutorialUI();
    }

    public void ShowTutorialUI()
    {
        if (tutorialUIPanel != null)
        {
            tutorialUIPanel.SetActive(true);
        }
    }

    // Dipanggil melalui OnClick() tombol Next/Close (>>) di UI Tutorial
    public void CloseTutorialUI()
    {
        if (tutorialUIPanel != null)
        {
            tutorialUIPanel.SetActive(false);
            GuardUI.SetActive(true); // Tampilkan UI Guard setelah tutorial ditutup
        }
    }
}