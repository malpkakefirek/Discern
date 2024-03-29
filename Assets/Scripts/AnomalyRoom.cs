using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyRoom : MonoBehaviour
{
    GameObject[] preObjects;
    GameObject[] postObjects;
    private string preAnomalyTag = "AnomalyObjectPre";
    private string postAnomalyTag = "AnomalyObjectPost";
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Objects in " + gameObject.name + ":");
        GameObject[] preObjects = GameObject.FindGameObjectsWithTag(preAnomalyTag);
        GameObject[] postObjects = GameObject.FindGameObjectsWithTag(postAnomalyTag);

        foreach (GameObject obj in preObjects)
        {
            if (IsObjectInRoom(obj))
            {
                Debug.Log("Pre-Anomaly in " + gameObject.name + ": " + obj.name);
            }
        }

        foreach (GameObject obj in postObjects)
        {
            if (IsObjectInRoom(obj))
            {
                Debug.Log("Post-Anomaly in " + gameObject.name + ": " + obj.name);
                obj.SetActive(false); // turn it off, we have to leave it on in editor so it can be detected
            }
        }
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
    void Update()
    {

    }
}
