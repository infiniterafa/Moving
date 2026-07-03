using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;

    public GameObject controllerMenu; 

    public static bool isPaused; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame(); 
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        controllerMenu.SetActive(true);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; 

        isPaused = true;
    }

    public void ResumeGame()
    {
        controllerMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToStartMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGameController()
    {
        controllerMenu.SetActive(true);
        Time.timeScale = 0f;

        isPaused = true;
    }


}
