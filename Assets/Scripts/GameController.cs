using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using OccaSoftware.SuperSimpleSkybox.Runtime;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Experimental.GlobalIllumination;

public class GameController : MonoBehaviour
{
    private int roomsCount;


    [SerializeField] int anomalySpawnTimeMin = 15;
    [SerializeField] int anomalySpawnTimeMax = 45;
    [SerializeField] float failCondition = 0.5f;
    [SerializeField] GameFinished gameFinished;
    [SerializeField] GameObject player;
    [SerializeField] GameObject Torch;

    [SerializeField] GameObject moon;
    [SerializeField] GameObject sun;
    [SerializeField] GameObject sky;


    private int timeToNextAnomaly;
    private int totalAnomalies;
    private int foundAnomalies;
    private List<GameObject> availableRooms;

    //Time
    [System.Serializable]
    public class GameTime
    {
        [Header("Run Time Editables"), Tooltip("Increase to move time forward")]
        public float raw;

        [Header("Editable Options"), Space, Tooltip("Start time in Minutes"), Range(0, 59)]
        public int startMinutes;
        [Tooltip("Start time in Hours"), Range(0, 23)]
        public int startHours;
        [Tooltip("Real Time in seconds per Hour in Game")]
        public int timePerHour;

        [Header("NON Editable Options")]
        [Tooltip("Calculated time in Minutes")]
        public int minutes;
        [Tooltip("Calculated time in Hours")]
        public int hours;

        public void Initialize()
        {
            raw = 0;
            UpdateTime();
        }
        public void UpdateTime()
        {
            raw += Time.deltaTime;

            float totalGameMinutes = (raw / timePerHour) * 60;
            minutes = startMinutes + (int)totalGameMinutes;
            hours = startHours + minutes / 60;
            minutes %= 60;
            hours %= 24;
            //SetRotationX(sun, (hours * 60 + minutes) / (60 * 24));
        }
    }
    //Object to call to get Time
    [SerializeField]
    public GameTime gameTime;
    // Start is called before the first frame update
    void Start()
    {
        availableRooms = GameObject.FindGameObjectsWithTag("AnomalyRoom").ToList();
        roomsCount = availableRooms.Count;
        totalAnomalies = 0;
        foundAnomalies = 0;
        timeToNextAnomaly = 60;
        gameTime.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTime.raw > timeToNextAnomaly)
        {
            spawnAnomaly();
            timeToNextAnomaly += Random.Range(anomalySpawnTimeMin, anomalySpawnTimeMax);
        }

        gameTime.UpdateTime();
        SetSkyRotation();

        if(Input.GetKeyDown(KeyCode.F))
        {
            Torch.SetActive(!Torch.activeSelf);
        }
    }
    public void SetSkyRotation()
    {
        float timeFraction = (float)(gameTime.hours * 60 + gameTime.minutes) / (float)(60 * 24);
        float angle = 360f * timeFraction;
        angle -= 90f;

        if (angle < 0f)
            angle += 360f;
        //Debug.Log("timeFract: "+timeFraction+" | Angle: " + angle);

        Quaternion currentRotation = sky.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(angle, 0f, 0f);
        Quaternion newRotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime);

        sky.transform.rotation = newRotation;
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
                    gameFinished.LoseGame("Anomalies have taken over the office");
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
