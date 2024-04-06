using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Light light;
    [SerializeField] float detectionAngle = 90f;
    [SerializeField] int maxRayDistance = 30;
    [SerializeField] private float raycastHeightPercentage = 0.9f;
    [SerializeField] float idleTime = 10f;
    [SerializeField] float deathProximity = 1.5f;

    private float guardHeight;

    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        guardHeight = GetComponent<Collider>().bounds.size.y;
        light.spotAngle = detectionAngle;
    }

    private float lastCalledTime;
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
            lastCalledTime = Time.time + 1f;
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    light.color = Color.green;
                    if (Time.time - lastCalledTime >= idleTime)
                    {
                        lastCalledTime = Time.time;
                        Vector3 randomPosition = GetRandomWalkablePosition();
                        agent.SetDestination(randomPosition);
                    }
                }
                else
                {
                    lastCalledTime = Time.time;
                }
            }
            else
            {
                lastCalledTime = Time.time;
            }
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= deathProximity)
        {
            Debug.LogError("Player was caught by the guard!");
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
            int layer = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Player");
            RaycastHit hit;
            if (Physics.Raycast(raycastOrigin, directionToPlayer, out hit, maxRayDistance, layer, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject == player)
                {
                    return true;
                }
            }
        }
        return false;
    }
    Vector3 GetRandomWalkablePosition()
    {
        NavMeshHit hit;
        Vector3 randomPosition = Vector3.zero;

        float minX = 8f;
        float maxX = 90f;
        float minZ = 6f;
        float maxZ = 50f;

        Vector3 randomPoint = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));

        if (NavMesh.SamplePosition(randomPoint, out hit, 100f, NavMesh.AllAreas))
        {
            randomPosition = hit.position;
        }

        return randomPosition;
    }

    void playerDetected()
    {
        light.color = Color.red;
        moveTo(player.transform.position);
    }
}
