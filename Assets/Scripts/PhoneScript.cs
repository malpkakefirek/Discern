using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class PhoneScript : MonoBehaviour
{
    [SerializeField] GameObject phoneMenu;
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject anomalyPanel;
    [SerializeField] private TextMeshProUGUI anomalyPanelRoomText;
    [SerializeField] Button defaultRoomButton;
    [SerializeField] Button defaultAnomalyButton;
    [SerializeField] GameController gameController;
    [SerializeField] private string reporting = "Reporting";
    [SerializeField] Color reportingColor;
    [SerializeField] float reportingTime = 5f;
    [SerializeField] private string reportingSuccess = "Anomaly removed!";
    [SerializeField] Color reportingSuccessColor;
    [SerializeField] float reportingSuccessTime = 2f;
    [SerializeField] private string reportingFail = "Anomaly NOT found!";
    [SerializeField] Color reportingFailColor;
    [SerializeField] float reportingFailTime = 2f;
    [SerializeField] GameObject reportingPanel;
    [SerializeField] private TextMeshProUGUI clock;

    private bool phoneUp = false;
    private bool selectedRoom = false;
    private bool isReporting = false;
    private int selectedRoomIndex = 0;
    private string selectedAnomaly;
    private TextMeshProUGUI reportingPanelText;
    private GameObject selectedRoomButton;
    private GameObject selectedAnomalyButton;
    private List<GameObject> rooms;
    private string[] anomalies = {
        "size change",
        "disappearance",
        "extra object",
        "appearance change",
        "room anomaly",
        "SCP"
    };

    void Start()
    {
        reportingPanelText = reportingPanel.GetComponentInChildren<TextMeshProUGUI>();
        rooms = new List<GameObject>(GameObject.FindGameObjectsWithTag("AnomalyRoom").OrderBy(p => p.name).ToList());

        anomalyPanel.SetActive(false);
        roomPanel.SetActive(true);
        phoneMenu.SetActive(false);

        selectedRoomButton = defaultRoomButton.gameObject;
        selectedAnomalyButton = defaultAnomalyButton.gameObject;
    }

    public void SelectRoom(int roomNumber)
    {
        if (selectedRoom)
        {
            return;
        }
        selectedRoomButton = EventSystem.current.currentSelectedGameObject;

        selectedRoomIndex = roomNumber;
        anomalyPanelRoomText.text = "Room: " + selectedRoomButton.name;
        Debug.Log("Selected room: " + rooms[selectedRoomIndex].name);

        selectedRoom = true;
        roomPanel.SetActive(false);
        anomalyPanel.SetActive(true);
        defaultAnomalyButton.Select();
    }

    public void SelectAnomaly(int anomalyNumber)
    {
        if (!selectedRoom | isReporting)
        {
            return;
        }

        selectedRoomButton = defaultRoomButton.gameObject;
        selectedAnomalyButton = defaultAnomalyButton.gameObject;

        isReporting = true;
        selectedAnomaly = anomalies[anomalyNumber];
        
        selectedRoom = false;
        anomalyPanel.SetActive(false);
        showReportingPanel();
    }

    string ParseTime(int time)
    {
        string parsedTime = "";

        if (time < 10)
        {
            parsedTime += "0";
        }

        parsedTime += time;
        return parsedTime;
    }

    void Update()
    {
        clock.text = ParseTime(gameController.gameTime.hours) + ":" + ParseTime(gameController.gameTime.minutes);

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (phoneUp)    // Close the phone
            {
                phoneUp = false;
                phoneMenu.SetActive(false);

                if (!isReporting)
                {
                    if (selectedRoom)
                    {
                        selectedAnomalyButton = EventSystem.current.currentSelectedGameObject;
                    }
                    else
                    {
                        selectedRoomButton = EventSystem.current.currentSelectedGameObject;
                    }
                }

                Debug.Log("Phone down.");
            }
            else    // Open the phone
            {
                phoneUp = true;
                phoneMenu.SetActive(true);

                // Bring back button focus
                if (!isReporting)
                {
                    if (selectedRoom)    // Anomaly selection screen
                    {
                        selectedAnomalyButton.GetComponent<Button>().Select();
                    }
                    else    // Room selection screen
                    {
                        selectedRoomButton.GetComponent<Button>().Select();
                    }
                }

                Debug.Log("Phone up.");
            }
        }

        if (isReporting)
        {
            return;
        }

        if (!phoneUp)
        {
            return;
        }

        // Go back from Anomaly Selection
        if (selectedRoom & Input.GetKeyDown(KeyCode.Q))
        {
            selectedRoom = false;
            anomalyPanel.SetActive(false);
            roomPanel.SetActive(true);
            selectedRoomButton.GetComponent<Button>().Select();
            selectedAnomalyButton = defaultAnomalyButton.gameObject;
        }
    }

    private void showReportingPanel()
    {
        reportingPanelText.color = reportingColor;
        reportingPanelText.text = reporting;
        reportingPanel.SetActive(true);

        Invoke("addDot", reportingTime * 1/4f);
        Invoke("addDot", reportingTime * 2/4f);
        Invoke("addDot", reportingTime * 3/4f);
        Invoke("showReportingAfter", reportingTime);
    }

    private void addDot()
    {
        reportingPanelText.text += ".";
    }

    private void showReportingAfter()
    {
        bool isSuccess = gameController.attempAnomalyFixBool(rooms[selectedRoomIndex], selectedAnomaly);
        if (isSuccess)
        {
            showReportingAfterSuccess();
        }
        else
        {
            showReportingAfterFail();
        }
    }

    private void showReportingAfterSuccess()
    {
        reportingPanelText.color = reportingSuccessColor;
        reportingPanelText.text = reportingSuccess;
        Invoke("closeReporting", reportingSuccessTime);
    }

    private void showReportingAfterFail()
    {
        reportingPanelText.color = reportingFailColor;
        reportingPanelText.text = reportingFail;
        Invoke("closeReporting", reportingFailTime);
    }

    private void closeReporting()
    {
        reportingPanel.SetActive(false);
        roomPanel.SetActive(true);
        isReporting = false;

        phoneMenu.SetActive(true);
        defaultRoomButton.Select();
        phoneMenu.SetActive(phoneUp);
    }
}

