using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIMenumanager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string SkipSceneName;

    [Header("Transition Settings")]
    [SerializeField] private float defaultTransitionDelay = 0.35f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SkipScene();
        }
    }

    // Hanya 1 parameter string -> Sekarang muncul di Inspector!
    public virtual void MoveToScene(string sceneName)
    {
        TriggerFeedback();
        StartCoroutine(LoadSceneWithDelay(sceneName));
    }

    public virtual void QuitGame()
    {
        TriggerFeedback();
        StartCoroutine(QuitWithDelay());
    }

    public virtual void RestartGame()
    {
        TriggerFeedback();
        StartCoroutine(LoadSceneWithDelay(SceneManager.GetActiveScene().name));
    }

    public virtual void SkipScene()
    {
        SceneManager.LoadScene(SkipSceneName);
    }

    public virtual void OpenPanel(GameObject targetPanel)
    {
        if (targetPanel != null)
        { 
            TriggerFeedback();
            targetPanel.SetActive(true);
        }
    }

    public virtual void ClosePanel(GameObject targetPanel)
    {
        if (targetPanel != null)
        {
            TriggerFeedback();
            targetPanel.SetActive(false);
        }
    }

    public virtual void TriggerFeedback()
    {
        ClickSound();

        // Otomatis mencari tombol yang baru saja diklik
        GameObject currentButton = EventSystem.current != null ? EventSystem.current.currentSelectedGameObject : null;

        if (currentButton != null)
        {
            // Spawn partikel tepat di posisi tombol dan jadikan tombol sebagai parent
            VFXManager.Instance?.PlayVFX(VFXType.Sparkle, currentButton.transform.position, currentButton.transform);
        }
        else
        {
            // Fallback jika diklik pakai touch/mouse langsung
            VFXManager.Instance?.PlayVFX(VFXType.Sparkle, Input.mousePosition);
        }
    }

    public virtual void ClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("event:/SFX/UISFX/ButtonClick");
        }
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(defaultTransitionDelay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator QuitWithDelay()
    {
        yield return new WaitForSeconds(defaultTransitionDelay);
        Application.Quit();
    }
}