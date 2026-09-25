using UnityEngine;

[CreateAssetMenu(fileName = "ButtonVFXConfig", menuName = "VFX/Button VFX Config")]
public class VFXConfigSO : ScriptableObject
{
    [Header("Visual Effects")]
    public GameObject sparklePrefab;

    [Header("Timing")]
    [Tooltip("Durasi sebelum GameObject partikel di-destroy")]
    public float destroyDelay = 1.0f;

    [Tooltip("Waktu tunggu agar animasi sparkle sempat terlihat sebelum pindah scene")]
    public float sceneTransitionDelay = 0.35f;

    /// <summary>
    /// Logika instansiasi partikel bisa langsung ditangani di sini
    /// </summary>
    public void SpawnSparkle(Vector3 position, Transform parent = null)
    {
        if (sparklePrefab == null) return;

        GameObject vfxInstance = Instantiate(sparklePrefab, position, Quaternion.identity);

        if (parent != null)
        {
            vfxInstance.transform.SetParent(parent, true);
        }

        Destroy(vfxInstance, destroyDelay);
    }
}