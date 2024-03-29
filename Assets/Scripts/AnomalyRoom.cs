using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyRoom : MonoBehaviour
{
    private string preAnomalyTag = "AnomalyObjectPre";
    public GameObject[] anomalyObjects;

    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Objects in " + gameObject.name + ":");

        GameObject[] preObjects = GameObject.FindGameObjectsWithTag(preAnomalyTag);
        anomalyObjects = new GameObject[preObjects.Length];
        int count = 0;
        foreach (GameObject preObj in preObjects)
        {
            if (IsObjectInRoom(preObj))
            {
                anomalyObjects[count] = preObj;
                count++;
            }
        }
        if(count > 0 && count != preObjects.Length)
            System.Array.Resize(ref anomalyObjects, count);
        Debug.Log("Objects in " + gameObject.name + " : "+ anomalyObjects.Length);
    }

    private bool IsObjectInRoom(GameObject obj)
    {
        Collider roomCollider = GetComponent<Collider>();
        if (roomCollider != null)
        {
            return roomCollider.bounds.Contains(obj.transform.position);
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
        if (anomalyObjects.Length > 0)
        {
            int randomIndex = Random.Range(0, anomalyObjects.Length);
            Anomaly selectedObject = anomalyObjects[randomIndex].GetComponent<Anomaly>();
            selectedObject.fixAnomaly(); // place holder, we fix and then spawn new one.
            selectedObject.spawnAnomaly();
        }
        else
        {
            Debug.LogWarning("No Anomalies in the "+gameObject.name+" are present!");
        }
    }
}
