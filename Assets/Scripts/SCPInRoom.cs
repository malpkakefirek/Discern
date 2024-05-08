using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyInRoom : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject SCP;
    [SerializeField] float timeToLive = 10f;
    private float currentTimer;
    private bool player_inside;
    [SerializeField] GameFinished gameFinished;

    // Start is called before the first frame update
    void Start()
    {
        currentTimer = timeToLive;
    }

    // Update is called once per frame
    void Update()
    {

        if (player_inside)
        {
            //Debug.Log("Player in range of SCP! time remaining: "+currentTimer+"s");
            if (currentTimer <= 0f)
            {
                gameFinished.LoseGame("Player was killed by SCP!");
            }
            else
            {
                currentTimer -= Time.deltaTime;
            }
        }
        else
        {
            if (currentTimer < timeToLive)
            {
                currentTimer += Time.deltaTime * 0.5f;
            }
            else if (currentTimer > timeToLive)
            {
                currentTimer = timeToLive;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            player_inside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            player_inside = false;
        }
    }
}
