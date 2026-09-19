using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Pengaturan Gerakan")]
    public float jarakGeser = 10f;       // Berapa jauh kamera geser ke kanan
    public float durasiGeser = 2f;       // Durasi waktu pergeseran (detik)

    [Header("Referensi UI")]
    public GameObject skipButtonUI;      // GameObject tombol skip/next

    private void Start()
    {
        // Pastikan UI skip mati dulu di awal jika belum selesai geser
        if (skipButtonUI != null)
        {
            skipButtonUI.SetActive(false);
        }

        // Jalankan pergeseran kamera
        MulaiGeserKamera();
    }

    public void MulaiGeserKamera()
    {
        StartCoroutine(PanCameraRoutine());
    }

    private IEnumerator PanCameraRoutine()
    {
        Vector3 posisiAwal = transform.position;
        // Hanya mengubah sumbu X ke kanan, sumbu Y dan Z tetap sama
        Vector3 posisiTarget = new Vector3(posisiAwal.x + jarakGeser, posisiAwal.y, posisiAwal.z);

        float waktuBerjalan = 0f;

        while (waktuBerjalan < durasiGeser)
        {
            waktuBerjalan += Time.deltaTime;
            float t = waktuBerjalan / durasiGeser;

            // Menggunakan smooth step agar gerakan melambat halus saat mendekati target
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(posisiAwal, posisiTarget, t);
            yield return null;
        }

        // Pastikan posisi persis di target akhir
        transform.position = posisiTarget;

        // Munculkan UI tombol skip setelah kamera berhenti
        if (skipButtonUI != null)
        {
            skipButtonUI.SetActive(true);
        }
    }
}