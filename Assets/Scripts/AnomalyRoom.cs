using System.Collections.Generic;
using UnityEngine;

public class AnomalyRoom : MonoBehaviour
{
    private string preAnomalyTag = "AnomalyObjectPre";
    public GameObject[] anomalyObjects;
    private Anomaly activeAnomaly;

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
        Debug.Log("Objects in " + gameObject.name + " : "+ anomalyObjects.Length);
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

    public Anomaly getActiveAnomaly()
    {
        return activeAnomaly;
    }

    public void spawnRandomAnomaly()
    {
        if (anomalyObjects.Length > 0)
        {
            if (activeAnomaly != null)
                activeAnomaly.fixAnomaly();// place holder, we fix and then spawn new one.
            int randomIndex = Random.Range(0, anomalyObjects.Length);
            activeAnomaly = anomalyObjects[randomIndex].GetComponent<Anomaly>();
            activeAnomaly.spawnAnomaly();
        }
        else
        {
            Debug.LogWarning("No Anomalies in the " + gameObject.name + " are present!");
        }
    }
}
