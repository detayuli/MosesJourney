using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [Header("Configuration")]
    [SerializeField] private VFXConfigSO vfxConfig;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

public void PlayVFX(VFXType type, Vector3 position, Transform parent = null)
{
    if (vfxConfig == null) return;

    GameObject prefab = vfxConfig.GetVFXPrefab(type);
    if (prefab == null) return;

    GameObject vfxInstance = Instantiate(prefab, position, Quaternion.identity);

    if (parent != null)
    {
        // worldPositionStays = true menjaga partikel tetap di posisi tombol, bukan loncat ke tengah
        vfxInstance.transform.SetParent(parent, true);
    }

    Destroy(vfxInstance, vfxConfig.destroyDelay);
}
}