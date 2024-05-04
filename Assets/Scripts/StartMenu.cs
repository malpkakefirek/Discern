using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private const int LOADING_SCENE = 3;

    [SerializeField] GameObject ControlPanel;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(LOADING_SCENE);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleControls(){

        ControlPanel.SetActive(!ControlPanel.activeSelf);
    }

}
