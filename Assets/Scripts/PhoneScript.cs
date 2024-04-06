using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhoneScript : MonoBehaviour
{
    [SerializeField] GameObject phoneMenu;
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject anomalyPanel;
    [SerializeField] GameController gameController;
    private GameObject selectedRoom;
    private bool phoneUp = false;
    private List<GameObject> availableRooms;
    private Dictionary<KeyCode, GameObject> roomKeyBindings = new Dictionary<KeyCode, GameObject>();
    private Dictionary<KeyCode, string> anomalyKeyBindings = new Dictionary<KeyCode, string>();

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
    private string listOfAnomalies;
    private string selectedAnomaly;

    void Start()
    {
        availableRooms = new List<GameObject>(GameObject.FindGameObjectsWithTag("AnomalyRoom"));

        roomPanelText = roomPanel.GetComponentInChildren<TextMeshProUGUI>();

        anomalyPanelText = anomalyPanel.GetComponentInChildren<TextMeshProUGUI>();

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

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (phoneUp)
            {
                phoneUp = false;
                phoneMenu.SetActive(false);
                Debug.Log("Phone down.");
            }
            else
            {
                phoneUp = true;
                phoneMenu.SetActive(true);
                Debug.Log("Phone up.");
            }
        }

        if (!phoneUp)
        {
            return;
        }

        if (!selectedRoom)
        {
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
        }
        else
        {   

            if (Input.GetKeyDown(KeyCode.Q))
            {
                selectedRoom = null;

                anomalyPanel.SetActive(false);

                roomPanel.SetActive(true);

            }

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


        }


    }

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
    }
}

