using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class PhoneScript : MonoBehaviour
{
    [SerializeField] GameObject phoneMenu;
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject anomalyPanel;
    [SerializeField] GameController gameController;
    private bool phoneUp = false;

    private GameObject[] rooms;
    private int index;

    //private List<GameObject> availableRooms;
    //private Dictionary<KeyCode, GameObject> roomKeyBindings = new Dictionary<KeyCode, GameObject>();
    //private Dictionary<KeyCode, string> anomalyKeyBindings = new Dictionary<KeyCode, string>();

    private string[] anomalies = {
        "increased size",
        "decreased size",
        "moving objects",
        "disappearance",
        "painting anomaly",
        "lighting anomaly",
        "SCP",
        "model change",
        "texture change",
        "sound anomaly",
        "extra object",
        "room anomaly"
    };

    private TextMeshProUGUI roomPanelText;
    private TextMeshProUGUI anomalyPanelText;
    //private string listOfAnomalies;
    private bool selectedRoom = false;
    private int selectedRoomIndex = 0;
    private string selectedAnomaly;

    void Start()
    {
        //availableRooms = new List<GameObject>(GameObject.FindGameObjectsWithTag("AnomalyRoom"));

        roomPanelText = roomPanel.GetComponentInChildren<TextMeshProUGUI>();

        anomalyPanelText = anomalyPanel.GetComponentInChildren<TextMeshProUGUI>();

        rooms = GameObject.FindGameObjectsWithTag("AnomalyRoom");
        refreshTextRooms();
        refreshTextAnomalies();
        anomalyPanel.SetActive(false);
        roomPanel.SetActive(true);

        /*
        for (int i = 0; i < availableRooms.Count; i++)
        {
            KeyCode roomKey = GetObjectKeyCode(i);
            roomKeyBindings.Add(roomKey, availableRooms[i]);
        }

        foreach (var room in roomKeyBindings)
        {
            roomPanelText.text += room.Key + " " + room.Value.name + "\n";
        }

        for (int i = 0; i < anomalies.Length; i++)
        {
            KeyCode anomalyKey = GetObjectKeyCode(i);
            anomalyKeyBindings.Add(anomalyKey, anomalies[i]);
        }

        foreach (var anomaly in anomalyKeyBindings)
        {
            listOfAnomalies += anomaly.Key + " " + anomaly.Value + "\n";
        }
        */

    }

    private void refreshTextRooms()
    {
        roomPanelText.text = "";
        for (int i = 0; i < rooms.Length; i++)
        {
            if (i == index)
            {
                roomPanelText.text += "<color=yellow>" + rooms[i].name + "</color>\n";
            }
            else
            {
                roomPanelText.text += rooms[i].name + "\n";
            }
        }
    }
    private void refreshTextAnomalies()
    {
        anomalyPanelText.text = "Press Q to go back. \n\nSelected Room: " + rooms[selectedRoomIndex].name + "\n\nAnomalies: \n";
        for (int i = 0; i < anomalies.Length; i++)
        {
            if (i == index)
            {
                anomalyPanelText.text += "<color=yellow>" + anomalies[i] + "</color>\n";
            }
            else
            {
                anomalyPanelText.text += anomalies[i] + "\n";
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (phoneUp)
            {
                phoneUp = false;
                phoneMenu.SetActive(false);
                anomalyPanel.SetActive(false);
                roomPanel.SetActive(false);
                Debug.Log("Phone down.");
            }
            else
            {
                phoneUp = true;
                phoneMenu.SetActive(true);
                anomalyPanel.SetActive(false);
                roomPanel.SetActive(true);
                index = 0;
                Debug.Log("Phone up.");
            }
        }

        if (!phoneUp)
        {
            return;
        }

        if (!selectedRoom)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow) && index != 0)
            {
                index -= 1;
                refreshTextRooms();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && index != rooms.Length - 1)
            {
                index += 1;
                refreshTextRooms();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                selectedRoomIndex = index;
                index = 0;
                anomalyPanel.SetActive(true);
                roomPanel.SetActive(false);
                selectedRoom = true;
                refreshTextAnomalies();
                Debug.Log("Selected room: " + rooms[selectedRoomIndex].name);
            }
            /*
            foreach (var kvp in roomKeyBindings)
            {
                if (Input.GetKeyDown(kvp.Key))
                {
                    selectedRoom = kvp.Value;

                    anomalyPanel.SetActive(true);

                    roomPanel.SetActive(false);

                    anomalyPanelText.text = "Press Q to go back. \n\nSelected Room: " + selectedRoom.name
                     + "\n\nAnomalies: \n" + listOfAnomalies;

                    Debug.Log("Selected room: " + selectedRoom.name);
                }
            }
            */
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Q))
            {
                selectedRoom = false;

                anomalyPanel.SetActive(false);

                roomPanel.SetActive(true);

                index = 0;

                refreshTextRooms();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && index != 0)
            {
                index -= 1;
                refreshTextAnomalies();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && index != anomalies.Length - 1)
            {
                index += 1;
                refreshTextAnomalies();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                selectedAnomaly = anomalies[index];

                gameController.attempAnomalyFix(rooms[selectedRoomIndex], selectedAnomaly);

                selectedRoom = false;

                anomalyPanel.SetActive(false);

                roomPanel.SetActive(true);

                index = 0;

                refreshTextRooms();
            }

            /*
            foreach (var kvp in anomalyKeyBindings)
            {
                if (Input.GetKeyDown(kvp.Key))
                {
                    selectedAnomaly = kvp.Value;

                    gameController.attempAnomalyFix(selectedRoom, selectedAnomaly);

                    selectedRoom = null;

                    anomalyPanel.SetActive(false);

                    roomPanel.SetActive(true);

                }
            }
            */


        }


    }
/*
    KeyCode GetObjectKeyCode(int roomIndex)
    {
        if (roomIndex < 9)
        {
            return KeyCode.Alpha1 + roomIndex;
        }
        else
        {
            return KeyCode.F1 + (roomIndex - 9);
        }
    }*/
}

