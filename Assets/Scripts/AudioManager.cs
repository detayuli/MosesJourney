using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class audiomanager : MonoBehaviour
{
    public static audiomanager Instance { get; private set; }

    [Header("---------- Audio Sources ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource walkSource; // Slot baru untuk suara jalan looping

    [Header("---------- Audio Clips ----------")]
    public AudioClip buttonPressSound;
    public AudioClip walkSound;
    public AudioClip WinSound;
    public AudioClip JumpSound;
    public AudioClip SaberDeaths;
    public AudioClip SaberLand;
    public AudioClip typingSound;
    public AudioClip BGMMusic;
    public AudioClip PresspSound;
    public AudioClip SaberPadoru;
    

    [Header("---------- Scene Music Mapping ----------")]
    [SerializeField] private List<SceneMusicMapping> sceneMusicMappings;

    private Dictionary<string, AudioClip> sceneMusicDict = new Dictionary<string, AudioClip>();
    private bool isWalking = false;
    private bool sfxEnabled = true;
    private bool musicEnabled = true;

    [System.Serializable]
    public class SceneMusicMapping
    {
        public List<string> sceneNames;
        public AudioClip musicClip;
    }

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
            return;
        }

        foreach (var mapping in sceneMusicMappings)
        {
            foreach (var sceneName in mapping.sceneNames)
            {
                sceneMusicDict[sceneName] = mapping.musicClip;
            }
        }

        SceneManager.activeSceneChanged += OnSceneChanged;
        PlayMusicInCurrentScene();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        PlayMusicInCurrentScene();
    }

    public void PlayMusicInCurrentScene()
    {
        if (!musicEnabled || musicSource == null) return;
        string currentScene = SceneManager.GetActiveScene().name;
        if (sceneMusicDict.TryGetValue(currentScene, out AudioClip targetClip))
        {
            if (musicSource.clip == targetClip && musicSource.isPlaying) return;
            musicSource.clip = targetClip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null && sfxEnabled)
            SFXSource.PlayOneShot(clip);
    }

    // Perbaikan HandleWalking untuk looping yang benar
    public void HandleWalking(bool walking)
    {
        if (walkSource == null || walkSound == null || !sfxEnabled) return;

        if (walking && !isWalking)
        {
            walkSource.clip = walkSound;
            walkSource.loop = true;
            walkSource.Play();
            isWalking = true;
        }
        else if (!walking && isWalking)
        {
            walkSource.Stop();
            isWalking = false;
        }
    }

    public void HandleButtonPress() => PlaySFX(JumpSound);
}