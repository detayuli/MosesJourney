using System.Collections;
using UnityEngine;
using FMODUnity;

public class CameraController : MonoBehaviour
{
    [Header("Pengaturan Target & Waktu")]
    public Transform targetTujuan;
    public float durasiGeser = 2f;
    public float delayVoiceOver = 0.5f;
    public EventReference voName; // Nama VO yang akan diputar saat kamera mulai bergerak
    public string nextLevelToUnlockID = "Stage1_Lvl1"; // ID level berikutnya yang akan dibuka setelah kamera selesai bergerak

    [Header("Pengaturan Zoom")]
    public bool gunakanZoom = true;
    public float targetZoomSize = 3.5f; // Ukuran target saat zoom in/out (2D: orthographicSize, 3D: Field of View)

    [Header("Referensi UI")]
    public GameObject skipButtonUI;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;
    }

    private void Start()
    {
        if (skipButtonUI) skipButtonUI.SetActive(false);
        StartCoroutine(PlayVODelayed());
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

        // Simpan nilai zoom awal
        float startZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView;

        for (float t = 0f; t < durasiGeser; t += Time.deltaTime)
        {
            float step = Mathf.SmoothStep(0f, 1f, t / durasiGeser);
            
            // Pergerakan Posisi
            transform.position = Vector3.Lerp(startPos, endPos, step);

            // Perubahan Zoom (Interpolasi mulus)
            if (gunakanZoom)
            {
                float currentZoom = Mathf.Lerp(startZoom, targetZoomSize, step);
                if (cam.orthographic)
                    cam.orthographicSize = currentZoom; // Untuk 2D
                else
                    cam.fieldOfView = currentZoom;      // Untuk 3D
            }

            yield return null;
        }

        transform.position = endPos;

        if (gunakanZoom)
        {
            if (cam.orthographic) cam.orthographicSize = targetZoomSize;
            else cam.fieldOfView = targetZoomSize;
        }

        if (skipButtonUI) skipButtonUI.SetActive(true);
        LevelManager.UnlockNextLevel(nextLevelToUnlockID);
    }
}