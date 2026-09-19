using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenumanager : MonoBehaviour
{
    public string SkipSceneName;

    private void Update()
    {
        // Tekan tombol L untuk skip scene
        if (Input.GetKeyDown(KeyCode.L))
        {
            SkipScene();
        }
    }

    public virtual void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public virtual void QuitGame()
    {
        Application.Quit();
    }

    public virtual void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public virtual void SkipScene()
    {
        MoveToScene(SkipSceneName);
    }

    // Mengatur aktif/tidaknya GameObject secara spesifik (true/false)
    public virtual void SetActivePanel(GameObject targetPanel, bool isActive)
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(isActive);
        }
    }
    // Untuk tombol Buka Panel
    public virtual void OpenPanel(GameObject targetPanel)
    {
        if (targetPanel != null)
        {   
            targetPanel.SetActive(true);
        }
    }

    // Untuk tombol Tutup Panel
    public virtual void ClosePanel(GameObject targetPanel)
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
        }
    }

    public virtual void ClickSound()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
    }
}