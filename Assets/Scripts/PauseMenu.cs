using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] MonoBehaviour fpsController;
    [SerializeField] MonoBehaviour phoneScript;
    public bool paused = false;

    void Start()
    {
        unPauseGame();
    }

    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                unPauseGame();
            }
            else
            {
                pauseGame();
            }
        }
        
    }
    
    public void GoStartMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void pauseGame()
    {
        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        fpsController.enabled = false;

        phoneScript.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Game paused.");
    }

    public void unPauseGame()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        fpsController.enabled = true;

        phoneScript.enabled = true;

        
        Debug.Log("Game unpaused.");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
