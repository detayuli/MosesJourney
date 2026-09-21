using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Pengaturan Target & Waktu")]
    public Transform targetTujuan;
    public float durasiGeser = 2f;
    public float delayVoiceOver = 0.5f; // Atur jeda sebelum VO bersuara (detik)
    public string voName;

    [Header("Referensi UI")]
    public GameObject skipButtonUI;

    private void Start()
    {
        if (skipButtonUI) skipButtonUI.SetActive(false);
        if (!string.IsNullOrEmpty(voName)) StartCoroutine(PlayVODelayed());
        if (targetTujuan) StartCoroutine(PanCameraRoutine());
    }

    private IEnumerator PlayVODelayed()
    {
        if (delayVoiceOver > 0f) yield return new WaitForSeconds(delayVoiceOver);
        AudioManager.Instance?.PlayVoiceOver(voName);
    }

    private IEnumerator PanCameraRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(targetTujuan.position.x, startPos.y, startPos.z);

        for (float t = 0f; t < durasiGeser; t += Time.deltaTime)
        {
            float step = Mathf.SmoothStep(0f, 1f, t / durasiGeser);
            transform.position = Vector3.Lerp(startPos, endPos, step);
            yield return null;
        }

        transform.position = endPos;
        if (skipButtonUI) skipButtonUI.SetActive(true);
    }
}