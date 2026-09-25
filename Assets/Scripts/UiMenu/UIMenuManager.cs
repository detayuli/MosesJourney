using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIMenumanager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string SkipSceneName;

    [Header("VFX Configuration")]
    [SerializeField] private VFXConfigSO vfxConfig; // Pasang file aset ScriptableObject di sini

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SkipScene();
        }
    }

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

    public virtual void SetActivePanel(GameObject targetPanel, bool isActive)
    {
        if (targetPanel != null)
        {
            TriggerFeedback();
            targetPanel.SetActive(isActive);
        }
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
        SpawnSparkle();
    }

    public virtual void ClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
        }
    }

    private void SpawnSparkle()
    {
        if (vfxConfig == null) return;

        Vector3 spawnPos;
        Transform parentTransform = null;

        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
            spawnPos = selectedObj.transform.position;
            parentTransform = selectedObj.transform.root;
        }
        else
        {
            spawnPos = Input.mousePosition;
        }

        // Panggil method spawn langsung dari ScriptableObject
        vfxConfig.SpawnSparkle(spawnPos, parentTransform);
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        float delay = (vfxConfig != null) ? vfxConfig.sceneTransitionDelay : 0.2f;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator QuitWithDelay()
    {
        float delay = (vfxConfig != null) ? vfxConfig.sceneTransitionDelay : 0.2f;
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }
}