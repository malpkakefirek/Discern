using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnomalyRoom : MonoBehaviour
{
    private string preAnomalyTag = "AnomalyObjectPre";
    private GameObject[] anomalyObjects;
    public Dictionary<Transform, List<Transform>> anomalies;
    public Transform activeAnomaly = null;

    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> roomObjects = new();
        foreach (Transform roomTransforms in transform.GetComponentsInChildren<Transform>())
        {
            if (roomTransforms.gameObject.tag.Contains(preAnomalyTag))
            {
                roomObjects.Add(roomTransforms.gameObject);
            }
        }
        anomalyObjects = roomObjects.ToArray();
        anomalies = new Dictionary<Transform, List<Transform>>();
        Transform postFolder = anomalyObjects[0].transform.parent.parent.GetChild(1);
        foreach (GameObject anomaly in anomalyObjects)
        {
            foreach (Transform anomalyPostFolder in postFolder.GetComponentsInChildren<Transform>())
            {
                if (anomalyPostFolder.name == anomaly.name)
                {
                    anomalies[anomaly.transform] = new (anomalyPostFolder.GetComponentsInChildren<Transform>());
                    anomalies[anomaly.transform].Remove(anomaly.transform); // GetComponentsInChildren also adds itself, so we need to remove it
                    foreach (Transform anomalyPost in anomalies[anomaly.transform])
                    {
                        anomalyPost.gameObject.SetActive(false);
                    }
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

    private float timeToUpdate = 0f;
    private float updateTimeIncrement = 10f;
    void Update()
    {
        if (Time.time >= timeToUpdate)
        {
            if(!IsObjectInRoom(player))
                DoTestThingy();
            timeToUpdate += updateTimeIncrement;
        }
    }
    private void DoTestThingy()
    {
        spawnRandomAnomaly();
    }



    public void spawnRandomAnomaly()
    {
        if (anomalies.Count > 0)
        {
            (Transform randomAnomalyPre, List<Transform> anomalyPost) = anomalies.ElementAt(Random.Range(0, anomalies.Count));
            Transform randomAnomalyPost = anomalyPost[Random.Range(0, anomalyPost.Count)];
            activeAnomaly = randomAnomalyPre;
            randomAnomalyPre.gameObject.SetActive(false);
            randomAnomalyPost.gameObject.SetActive(true);
            Debug.Log("Summoned anomaly " + randomAnomalyPost.name + " on " + randomAnomalyPre.name + " in room " + name);
        }
        else
        {
            Debug.LogWarning("No Anomalies in the " + gameObject.name + " are present!");
        }
    }
}
