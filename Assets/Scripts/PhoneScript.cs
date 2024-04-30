using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class PhoneScript : MonoBehaviour
{
    [SerializeField] GameObject phoneMenu;
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject anomalyPanel;
    [SerializeField] Button defaultRoomButton;
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

    private bool phoneUp = false;
    private bool selectedRoom = false;
    private bool isReporting = false;
    private bool cooldownPassed = false; // temp fix for immediate anomaly selection
    private int index;
    private int selectedRoomIndex = 0;
    private string selectedAnomaly;
    private TextMeshProUGUI anomalyPanelText;
    private TextMeshProUGUI reportingPanelText;
    private List<GameObject> rooms;
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

    void Start()
    {
        anomalyPanelText = anomalyPanel.GetComponentInChildren<TextMeshProUGUI>();
        reportingPanelText = reportingPanel.GetComponentInChildren<TextMeshProUGUI>();

        rooms = new List<GameObject>(GameObject.FindGameObjectsWithTag("AnomalyRoom").OrderBy(p => p.name).ToList());
        refreshTextAnomalies();
        anomalyPanel.SetActive(false);
        roomPanel.SetActive(true);
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

    public void SelectRoom(int roomNumber)
    {
        if (selectedRoom)
        {
            return;
        }

        index = 0;
        selectedRoomIndex = roomNumber;
        anomalyPanel.SetActive(true);
        roomPanel.SetActive(false);
        selectedRoom = true;
        refreshTextAnomalies();
        Debug.Log("Selected room: " + rooms[selectedRoomIndex].name);
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
                roomPanel.SetActive(!isReporting);
                reportingPanel.SetActive(isReporting);
                defaultRoomButton.Select();
                index = 0;
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

        // Anomaly Selection
        if (selectedRoom)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                selectedRoom = false;
                anomalyPanel.SetActive(false);
                roomPanel.SetActive(true);
                index = 0;
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

            if (Input.GetKeyDown(KeyCode.Return) & cooldownPassed)
            {
                isReporting = true;
                selectedAnomaly = anomalies[index];

                //gameController.attempAnomalyFix(rooms[selectedRoomIndex], selectedAnomaly);

                selectedRoom = false;
                anomalyPanel.SetActive(false);
                roomPanel.SetActive(false);

                showReportingPanel();

                index = 0;
                cooldownPassed = false;
            }
            else
            {
                cooldownPassed = true;
            }
        }
    }

    private void showReportingPanel()
    {
        reportingPanelText.color = reportingColor;
        reportingPanelText.text = reporting;
        reportingPanel.SetActive(true);
        //Debug.Log(reporting);
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
        //Debug.Log(reportingSuccess);
        reportingPanelText.color = reportingSuccessColor;
        reportingPanelText.text = reportingSuccess;
        Invoke("closeReporting", reportingSuccessTime);
    }
    private void showReportingAfterFail()
    {
        //Debug.Log(reportingFail);
        reportingPanelText.color = reportingFailColor;
        reportingPanelText.text = reportingFail;
        Invoke("closeReporting", reportingFailTime);
    }
    private void closeReporting()
    {
        //Debug.Log("Reporting Finished");
        reportingPanel.SetActive(false);
        roomPanel.SetActive(true);
        isReporting = false;
    }
}

