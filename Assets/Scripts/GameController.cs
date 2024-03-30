using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    private GameObject[] rooms;
    private List<GameObject> roomsWithAnomalies = new List<GameObject>();


    [SerializeField] int anomalySpawnTimeMin = 15;
    [SerializeField] int anomalySpawnTimeMax = 45;
    [SerializeField] float failCondition = 0.5f;


    private int timeToNextAnomaly = 30;
    private int totalAnomalies = 0;
    private int foundAnomalies = 0;
    private bool phoneOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        rooms = GameObject.FindGameObjectsWithTag("AnomalyRoom");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeToNextAnomaly)
        {
            spawnAnomaly();
            totalAnomalies++;
            timeToNextAnomaly += Random.Range(anomalySpawnTimeMin, anomalySpawnTimeMax);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            phoneOpened = !phoneOpened;
            Debug.Log("is phone opened: " + phoneOpened);
            if (phoneOpened)
            {
                openPhone();
            }
        }
    }

    private void spawnAnomaly()
    {
        List<GameObject> availableRooms = rooms.Except(roomsWithAnomalies).ToList();

        if (availableRooms.Count != 0)
        {
            int randomIndex = Random.Range(0, availableRooms.Count);
            GameObject room = availableRooms[randomIndex];
            Debug.Log("Tried spawning anomaly in: " + room.name);
            //room.GetComponent<AnomalyRoom>.spawnAnomaly();
            roomsWithAnomalies.Add(room);


            float ratio = (float)roomsWithAnomalies.Count / rooms.Length;
            if (ratio > failCondition)
            {
                Debug.LogError("Player died after new anomaly spawned");
            }
        }


    }
    private void openPhone()
    {
        // to be implemented
        Debug.Log("Anomalies found : " + foundAnomalies + "/" + totalAnomalies);
    }
}
