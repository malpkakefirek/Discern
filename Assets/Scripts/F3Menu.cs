using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class F3Menu : MonoBehaviour
{
    private GameObject[] rooms;
    [SerializeField] TextMeshProUGUI menuText;

    private bool isMenuOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        rooms = GameObject.FindGameObjectsWithTag("AnomalyRoom");
        menuText.text = "";
        InvokeRepeating("RefreshFunction", 0f, 1f);
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
                getRoomsInfo();
            }
        }
    }

    private void getRoomsInfo()
    {
        foreach (GameObject room in rooms)
        {
            menuText.text += room.name+"  |  object: "+ room.GetComponent<AnomalyRoom>().getActiveAnomaly().name + "  |  anomaly: " + room.GetComponent<AnomalyRoom>().getActiveAnomaly().getCurrentAnomalyName()+"\n";
        }
        
    }

    void RefreshFunction()
    {
        if (isMenuOpen)
        {
            menuText.text = "";
            getRoomsInfo();
        }
    }
}
