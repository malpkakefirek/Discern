using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Light light;
    [SerializeField] float detectionAngle = 90f;
    [SerializeField] private float raycastHeightPercentage = 0.9f;

    private float guardHeight;

    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        guardHeight = GetComponent<Collider>().bounds.size.y;
        light.spotAngle = detectionAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            moveTo(player.transform.position);
        }

        RotateTowardsMovementDirection();

        if (CanSeePlayer())
        {
            playerDetected();
            Invoke("playerDetected", 1f);
        }
        else
        {
            light.color = Color.green;
        }
    }

    public void moveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }
    public void moveTo(float x, float y, float z)
    {
        agent.SetDestination(new Vector3(x,y,z));
    }
    void RotateTowardsMovementDirection()
    {
        if (agent.velocity.magnitude > 0.05f)
        {
            Vector3 direction = agent.velocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);
        if (angleToPlayer < detectionAngle / 2f)
        {
            Vector3 raycastOrigin = transform.position + Vector3.up * guardHeight * raycastHeightPercentage;
            RaycastHit hit;
            if (Physics.Raycast(raycastOrigin, directionToPlayer, out hit))
            {
                if (hit.collider.gameObject == player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void playerDetected()
    {
        light.color = Color.red;
        moveTo(player.transform.position);
    }
}
