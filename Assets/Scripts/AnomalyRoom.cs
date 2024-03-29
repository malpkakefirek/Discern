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

        // Filter objects to those within the bounds of the room
        foreach (GameObject obj in preObjects)
        {
            if (IsObjectInRoom(obj))
            {
                // Print the name of the pre-anomaly object in the room
                Debug.Log("Pre-Anomaly in " + gameObject.name + ": " + obj.name);
            }
        }

        foreach (GameObject obj in postObjects)
        {
            if (IsObjectInRoom(obj))
            {
                // Print the name of the post-anomaly object in the room
                Debug.Log("Post-Anomaly in " + gameObject.name + ": " + obj.name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private bool IsObjectInRoom(GameObject obj)
    {
        // Check if the object is within the bounds of the room
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
}
