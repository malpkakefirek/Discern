using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnomalyRoom : MonoBehaviour
{
    private string preAnomalyTag = "AnomalyObjectPre";
    private string postAnomalyTag = "AnomalyObjectPost";
    private GameObject[] anomalyObjects;
    public Dictionary<Transform, List<Transform>> anomalies;
    public Transform activeAnomaly = null;
    public string activeAnomalyName = null;

    public float spawnPointX;
    public float spawnPointY;
    public float spawnPointZ;

    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> roomObjects = new();
        foreach (Transform roomTransforms in transform.GetComponentsInChildren<Transform>())
        {
            if (roomTransforms.gameObject.tag == preAnomalyTag)
            {
                roomObjects.Add(roomTransforms.gameObject);
            }
        }
        anomalyObjects = roomObjects.ToArray();
        anomalies = new Dictionary<Transform, List<Transform>>();
        if (anomalyObjects.Length <= 0)
        {
            Debug.LogWarning("No Anomaly Objects found in room: " + gameObject.name);
            return;
        }
        Transform postFolder = anomalyObjects[0].transform.parent.parent.GetChild(1);
        foreach (GameObject anomaly in anomalyObjects)
        {
            foreach (Transform anomaliesPostFolder in postFolder.GetComponentsInChildren<Transform>())
            {
                // Only get folders for anomalies, inside the Post folder
                if (anomaliesPostFolder.name != anomaly.name)
                {
                    continue;
                }

                List<Transform> postAnomalies = new List<Transform>();
                foreach (Transform anomalyPost in anomaliesPostFolder.GetComponentsInChildren<Transform>(true))
                {
                    // Only get objects that are anomalies
                    if (anomalyPost.tag == postAnomalyTag)
                    {
                        postAnomalies.Add(anomalyPost);
                    }
                }
                if (postAnomalies.Count > 0)
                {
                    anomalies[anomaly.transform] = postAnomalies;
                }

                // Hide all anomalies
                foreach (Transform anomalyPost in anomalies[anomaly.transform])
                {
                    anomalyPost.gameObject.SetActive(false);
                }
            }
        }
        Debug.Log("Objects in " + gameObject.name + " : "+ anomalies.Count);
    }

    private bool IsObjectInRoom(GameObject obj)
    {
        //Collider roomCollider = GetComponent<Collider>();
        Collider[] allColliders = gameObject.GetComponents<Collider>();
        if (allColliders != null)
        {
            foreach (Collider collider in allColliders)
            {
                if (collider.bounds.Contains(obj.transform.position))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            Debug.LogWarning("No collider found on the room GameObject: " + gameObject.name);
            return false;
        }
    }

    void Update()
    {
    }

    public bool spawnRandomAnomaly()
    {
        if (anomalies.Count > 0)
        {
            (Transform randomAnomalyPre, List<Transform> anomalyPost) = anomalies.ElementAt(Random.Range(0, anomalies.Count));
            Transform randomAnomalyPost = anomalyPost[Random.Range(0, anomalyPost.Count)];
            activeAnomaly = randomAnomalyPre;
            activeAnomalyName = randomAnomalyPost.name;
            randomAnomalyPre.gameObject.SetActive(false);
            randomAnomalyPost.gameObject.SetActive(true);
            Debug.Log("Summoned anomaly " + randomAnomalyPost.name + " on " + randomAnomalyPre.name + " in room " + name);
            return true;
        }
        else
        {
            Debug.LogWarning("No Anomalies in the " + gameObject.name + " are present!");
            return false;
        }
    }

    public bool fixAnomaly(string anomalyName)
    {
        if (anomalies.Count == 0)
        {
            Debug.LogWarning("No Anomalies in the " + gameObject.name + " are present!");
            return false;  
        }

        if (!activeAnomaly)
        {
            Debug.Log("There were no anomalies active in the " + gameObject.name + "!");
            return false;
        }

        if (anomalyName != activeAnomalyName)
        {
            Debug.Log("The anomaly in the " + gameObject.name + " was incorrect! ("+ anomalyName+" != "+ activeAnomalyName + ")");
            return false;
        }

        activeAnomalyName = null;
        activeAnomaly.gameObject.SetActive(true);
        foreach (Transform activeAnomaliesPost in anomalies[activeAnomaly])
        {
            activeAnomaliesPost.gameObject.SetActive(false);
        }
        activeAnomaly = null;
        Debug.Log("The anomaly in the " + gameObject.name + " was correct!");
        return true;
    }
}
