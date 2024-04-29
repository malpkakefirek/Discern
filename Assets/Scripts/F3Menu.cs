using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class F3Menu : MonoBehaviour
{
    private GameObject[] rooms;
    [SerializeField] GameObject gameController;
    [SerializeField] TextMeshProUGUI menuText;

    private bool isMenuOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        rooms = GameObject.FindGameObjectsWithTag("AnomalyRoom");
        menuText.text = "";
        InvokeRepeating("RefreshFunction", 0f, 1f); //refresh every 1 sec
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)){
            if (isMenuOpen)
            {
                isMenuOpen = false;
                menuText.text = "";
            }
            else
            {
                isMenuOpen = true;
                menuText.text = "Time = " + gameController.GetComponent<GameController>().gameTime.hours + ":" + (gameController.GetComponent<GameController>().gameTime.minutes<10?"0"+ gameController.GetComponent<GameController>().gameTime.minutes: gameController.GetComponent<GameController>().gameTime.minutes) + "\n";
                menuText.text += "Infographics:\n";
                getRoomsInfo();
            }
        }
    }

    private void getRoomsInfo()
    {
        foreach (GameObject room in rooms)
        {
            if(room.GetComponent<AnomalyRoom>().activeAnomaly != null)
            {
                menuText.text += room.name + "  |  object: " + room.GetComponent<AnomalyRoom>().activeAnomaly.gameObject.name + "  |  anomaly: " + room.GetComponent<AnomalyRoom>().activeAnomalyName + "\n";
            }
        }
    }

    void RefreshFunction()
    {
        if (isMenuOpen)
        {
            menuText.text = "Time = " + gameController.GetComponent<GameController>().gameTime.hours + ":" + (gameController.GetComponent<GameController>().gameTime.minutes < 10 ? "0" + gameController.GetComponent<GameController>().gameTime.minutes : gameController.GetComponent<GameController>().gameTime.minutes) + "\n";
            menuText.text += "Infographics:\n";
            getRoomsInfo();
        }
    }
}
