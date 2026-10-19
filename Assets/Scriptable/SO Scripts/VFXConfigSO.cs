using UnityEngine;

[CreateAssetMenu(fileName = "GameVFXConfig", menuName = "VFX/Game VFX Config")]
public class VFXConfigSO : ScriptableObject
{
    [System.Serializable]
    public struct VFXData
    {
        public VFXType type;
        public GameObject prefab;
    }

    [Header("All Available VFX")]
    public VFXData[] vfxList;

    [Header("Timing")]
    public float destroyDelay = 1.0f;
    public float sceneTransitionDelay = 0.35f;

    // Fungsi pembantu untuk mencari prefab berdasarkan tipe
    public GameObject GetVFXPrefab(VFXType type)
    {
        if (vfxList == null) return null;

        foreach (var vfx in vfxList)
        {
            if (vfx.type == type)
            {
                return vfx.prefab;
            }
        }

        return null;
    }
}