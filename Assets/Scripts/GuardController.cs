using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Light torchlight;
    [SerializeField] float detectionAngle = 90f;
    [SerializeField] int maxRayDistance = 30;
    [SerializeField] private float raycastHeightPercentage = 0.9f;
    [SerializeField] float idleTime = 10f;
    [SerializeField] float deathProximity = 1.5f;
    [SerializeField] float speed = 4.5f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] private float m_StepInterval;
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] GameFinished gameFinished;

    private AudioSource m_AudioSource;
    private float guardHeight;
    private bool playerCaught;
    private float m_StepCycle;
    private float m_NextStep;

    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        guardHeight = GetComponent<Collider>().bounds.size.y;
        torchlight.spotAngle = detectionAngle;
        agent.speed = speed;

        m_StepCycle = 0f;
        m_NextStep = m_StepCycle/2f;
    }

    private float lastCalledTime;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            moveTo(player.transform.position);
        }

        RotateTowardsMovementDirection();

        if (CanSeePlayer())
        {
            playerDetected();
            Invoke("playerDetected", 1f);
            lastCalledTime = Time.time + 1f;
            agent.speed += acceleration * Time.deltaTime;

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= deathProximity && !playerCaught)
            {
                playerCaught = true;
                gameFinished.LoseGame("Player was caught by the guard!");
            }
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    torchlight.color = Color.green;
                    if (Time.time - lastCalledTime >= idleTime)
                    {
                        lastCalledTime = Time.time;
                        agent.speed = speed;
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

    }

    void FixedUpdate()
    {
        ProgressStepCycle(agent.speed);
    }

    private void ProgressStepCycle(float speed)
    {
        if (agent.velocity.sqrMagnitude > 0)
        {
            m_StepCycle += (agent.velocity.magnitude + speed)*Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio()
    {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }

/*    private bool isChasing()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
            return false;
        }
        return false;
    }*/
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
        torchlight.color = Color.red;
        moveTo(player.transform.position);
    }
}
