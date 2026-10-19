using System.Collections;
using UnityEngine;

public class PunchScaleEffect : MonoBehaviour
{
    public float punchMultiplier = 1.25f;
    public float duration = 0.2f;

    private Vector3 originalScale;
    private Coroutine punchRoutine;

    private void Awake() => originalScale = transform.localScale;
    private void OnDisable() => transform.localScale = originalScale;
    private void OnMouseDown() => PlayPunch();

    public void PlayPunch()
    {
        if (punchRoutine != null) StopCoroutine(punchRoutine);
        punchRoutine = StartCoroutine(AnimatePunch());
    }

    private IEnumerator AnimatePunch()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Sinus 0 sampai PI menghasilkan kurva melengkung naik ke 1 lalu balik ke 0
            float wave = Mathf.Sin((elapsed / duration) * Mathf.PI);
            transform.localScale = Vector3.Lerp(originalScale, originalScale * punchMultiplier, wave);
            yield return null;
        }

        transform.localScale = originalScale;
        punchRoutine = null;
    }
}