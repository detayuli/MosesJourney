using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubtitleSystem : MonoBehaviour
{
    [System.Serializable]
    public struct SubtitleLine
    {
        [TextArea(2, 5)]
        public string text;
        public float duration; // Berapa lama teks tampil penuh
    }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private CanvasGroup textCanvasGroup;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Subtitle Content")]
    [SerializeField] private List<SubtitleLine> lines = new List<SubtitleLine>();

    private Coroutine activeSubtitleRoutine;

    private void Start()
    {
        // Pastikan teks tersembunyi di awal
        if (textCanvasGroup != null)
        {
            textCanvasGroup.alpha = 0f;
        }

        // Jalankan antrean dialog saat mulai
        PlaySubtitles();
    }

    public void PlaySubtitles()
    {
        if (activeSubtitleRoutine != null)
        {
            StopCoroutine(activeSubtitleRoutine);
        }
        activeSubtitleRoutine = StartCoroutine(ShowSequenceRoutine());
    }

    private IEnumerator ShowSequenceRoutine()
    {
        foreach (var line in lines)
        {
            subtitleText.text = line.text;

            // Fade In
            yield return StartCoroutine(FadeAlpha(0f, 1f, fadeDuration));

            // Durasi teks terbaca
            yield return new WaitForSeconds(line.duration);

            // Fade Out
            yield return StartCoroutine(FadeAlpha(1f, 0f, fadeDuration));
        }

        subtitleText.text = string.Empty;
        activeSubtitleRoutine = null;
    }

    private IEnumerator FadeAlpha(float startAlpha, float targetAlpha, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            textCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            yield return null;
        }

        textCanvasGroup.alpha = targetAlpha;
    }
}