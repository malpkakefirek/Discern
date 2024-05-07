using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameFinished : MonoBehaviour
{
    [SerializeField] GameObject FinishScreen;
    [SerializeField] MonoBehaviour pauseMenu;
    [SerializeField] MonoBehaviour phoneMenu;
    [SerializeField] TextMeshProUGUI bigHeader;
    [SerializeField] TextMeshProUGUI smallHeader;



    public void FinishGame(string causeMessage){
        
        FinishScreen.SetActive(true);

        Time.timeScale = 0;
        pauseMenu.enabled = false;
        phoneMenu.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        Debug.Log("Player lost | Cause: " + causeMessage);
    }

    public void LoseGame(string causeMessage){

        FinishGame(causeMessage);

        bigHeader.text = "You Lose";
        smallHeader.text = causeMessage;
    }
}
