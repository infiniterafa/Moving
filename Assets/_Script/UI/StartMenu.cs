using UnityEngine;
using UnityEngine.SceneManagement;

/// Script para activar las escenas de start menu y game over
public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void EndGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}