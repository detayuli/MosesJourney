using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("---------- Audio Sources ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource voiceOverSource;

    [System.Serializable]
    public class SoundEffect
    {
        public string sfxName;
        public AudioClip clip;
    }

    [System.Serializable]
    public class SceneMusicMapping
    {
        public string musicName;
        public AudioClip musicClip;
        public List<string> sceneNames;
    }

    [Header("---------- SFX List ----------")]
    [SerializeField] private List<SoundEffect> sfxList = new List<SoundEffect>();

    [Header("---------- Scene Music Mapping ----------")]
    [SerializeField] private List<SceneMusicMapping> sceneMusicMappings = new List<SceneMusicMapping>();

    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sceneMusicDict = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionaries();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.activeSceneChanged += OnSceneChanged;
        PlayMusicInCurrentScene();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void InitializeDictionaries()
    {
        // Setup lookup SFX
        foreach (var sfx in sfxList)
        {
            if (!string.IsNullOrEmpty(sfx.sfxName) && sfx.clip != null)
            {
                sfxDict[sfx.sfxName] = sfx.clip;
            }
        }

        // Setup lookup BGM per scene
        foreach (var mapping in sceneMusicMappings)
        {
            if (mapping.musicClip == null) continue;

            foreach (var sceneName in mapping.sceneNames)
            {
                if (!string.IsNullOrEmpty(sceneName))
                {
                    sceneMusicDict[sceneName] = mapping.musicClip;
                }
            }
        }
    }

    private void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        PlayMusicInCurrentScene();
    }

    public void PlayMusicInCurrentScene()
    {
        if (musicSource == null) return;

        string currentScene = SceneManager.GetActiveScene().name;

        if (sceneMusicDict.TryGetValue(currentScene, out AudioClip targetClip))
        {
            // Jika musik yang dimainkan sama persis dengan scene saat ini, lagu tetap lanjut (seamless)
            if (musicSource.clip == targetClip && musicSource.isPlaying) return;

            musicSource.clip = targetClip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
            musicSource.clip = null;
        }
    }

    // Play SFX via ID nama (Inspector)
    public void PlaySFX(string soundName)
    {
        if (sfxSource == null) return;

        if (sfxDict.TryGetValue(soundName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[AudioManager] SFX dengan nama '{soundName}' tidak ditemukan.");
        }
    }

    // Play SFX via Direct Clip
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // VoiceOver Control
    public void PlayVoiceOver(AudioClip voClip)
    {
        if (voiceOverSource == null || voClip == null) return;
        voiceOverSource.Stop();
        voiceOverSource.clip = voClip;
        voiceOverSource.Play();
    }

    public void StopVoiceOver()
    {
        if (voiceOverSource != null)
        {
            voiceOverSource.Stop();
        }
    }
}