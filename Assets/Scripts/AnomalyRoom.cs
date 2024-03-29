using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyRoom : MonoBehaviour
{
    [System.Serializable]
    public class AnomalyPair
    {
        public GameObject preObject;
        public GameObject postObject;
    }
    public List<AnomalyPair> anomalyPairs = new List<AnomalyPair>();

    private string preAnomalyTag = "AnomalyObjectPre";
    private string postAnomalyTag = "AnomalyObjectPost";
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Objects in " + gameObject.name + ":");

        GameObject[] preObjects = GameObject.FindGameObjectsWithTag(preAnomalyTag);
        GameObject[] postObjects = GameObject.FindGameObjectsWithTag(postAnomalyTag);

        foreach (GameObject preObj in preObjects)
        {
            if (IsObjectInRoom(preObj))
            {
                foreach (GameObject postObj in postObjects)
                {
                    if (IsObjectInRoom(postObj) && postObj.name == preObj.name)
                    {
                        anomalyPairs.Add(new AnomalyPair { preObject = preObj, postObject = postObj });

                        //Debug.Log("Matched Pair in " + gameObject.name + ": " + preObj.name + " (Pre) - " + postObj.name + " (Post)");

                        // Set the post object inactive
                        postObj.SetActive(false);
                        break;
                    }
                }
            }
        }
        Debug.Log("Objects in " + gameObject.name + " : "+ anomalyPairs.Count);
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
            FlipObjectStates();
            timeToUpdate += updateTimeIncrement;
        }
    }
    private void FlipObjectStates()
    {
        if (anomalyPairs.Count > 0)
        {
            int randomIndex = Random.Range(0, anomalyPairs.Count);
            AnomalyPair pair = anomalyPairs[randomIndex];

            if (pair.preObject.activeSelf)
            {
                pair.preObject.SetActive(false);
                pair.postObject.SetActive(true);
                //Debug.Log("Flipped states in " + gameObject.name + " : " + pair.preObject.name + " (Pre) - " + pair.postObject.name + " (Post)");
            }
            else
            {
                pair.preObject.SetActive(true);
                pair.postObject.SetActive(false);
                //Debug.Log("Flipped states in "+gameObject.name+" : " + pair.preObject.name + " (Post) - " + pair.postObject.name + " (Pre)");
            }
        }
    }
}
