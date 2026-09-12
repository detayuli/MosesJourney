using System.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenumanager : MonoBehaviour
{

    public virtual void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}