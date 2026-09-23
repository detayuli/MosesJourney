using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenumanager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string SkipSceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SkipScene();
        }
    }

    public virtual void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        ClickSound();
    }

    public virtual void QuitGame()
    {
        Application.Quit();
        ClickSound();
    }

    public virtual void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ClickSound();
    }

    public virtual void SkipScene()
    {
        SceneManager.LoadScene(SkipSceneName);
    }

    public virtual void SetActivePanel(GameObject targetPanel, bool isActive)
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(isActive);
            ClickSound();
        }
    }

    public virtual void OpenPanel(GameObject targetPanel)
    {
        if (targetPanel != null)
        {   
            targetPanel.SetActive(true);
            ClickSound();
        }
    }

    public virtual void ClosePanel(GameObject targetPanel)
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
            ClickSound();
        }
    }

    public virtual void ClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
        }
    }
}