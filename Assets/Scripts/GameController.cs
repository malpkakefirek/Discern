using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    private int roomsCount;


    [SerializeField] int anomalySpawnTimeMin = 15;
    [SerializeField] int anomalySpawnTimeMax = 45;
    [SerializeField] float failCondition = 0.5f;
    [SerializeField] GameObject player;


    private int timeToNextAnomaly = 30;
    private int totalAnomalies = 0;
    private int foundAnomalies = 0;
    private List<GameObject> availableRooms;
    // Start is called before the first frame update
    void Start()
    {
        availableRooms = GameObject.FindGameObjectsWithTag("AnomalyRoom").ToList();
        roomsCount = availableRooms.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeToNextAnomaly)
        {
            spawnAnomaly();
            timeToNextAnomaly += Random.Range(anomalySpawnTimeMin, anomalySpawnTimeMax);
        }
    }

    private void spawnAnomaly()
    {
        if (availableRooms.Count > 0)
        {
            int randomIndex = Random.Range(0, availableRooms.Count);
            GameObject room = availableRooms[randomIndex];
            Debug.Log("Tried spawning anomaly in: " + room.name);
            if (room.GetComponent<AnomalyRoom>().spawnRandomAnomaly(player.transform.position))
            {
                availableRooms.Remove(room);
                totalAnomalies++;

                float ratio = (float)totalAnomalies / roomsCount;
                if (ratio > failCondition)
                {
                    Debug.LogError("Player died after new anomaly spawned");
                }
            }
        }
    }
    public void attempAnomalyFix(GameObject room, string name)
    {
        Debug.Log("Attempted anomaly fix in: " + room.name +" | type: "+ name);
        if (room.GetComponent<AnomalyRoom>().fixAnomaly(name))
        {
            totalAnomalies--;
            foundAnomalies++;
            availableRooms.Add(room);
        }
    }
    public bool attempAnomalyFixBool(GameObject room, string name)
    {
        Debug.Log("Attempted anomaly fix in: " + room.name + " | type: " + name);
        if (room.GetComponent<AnomalyRoom>().fixAnomaly(name))
        {
            totalAnomalies--;
            foundAnomalies++;
            availableRooms.Add(room);
            return true;
        }
        return false;
    }
}
