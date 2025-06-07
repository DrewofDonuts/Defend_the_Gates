using UnityEngine;
using UnityEngine.AI;

public class AITestStrafe : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    Vector3 lookPos;
    Quaternion rotation;
    public float rotationSpeed = 10f;

    Vector3 destination;
    public float strafeDistance = 10f;
    public float strafeSpeed = 5f;
    bool timePicked;
    float leftOrRight = 1;

    float randomWaitStrafeTime;

    int randomDirection;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ChatGPTStrafeRight();
    }

    public void Randomizer()
    {
        if (!timePicked)
        {
            randomWaitStrafeTime = Random.Range(1f, 4f);
            randomDirection = Random.Range(0, 2);
        }

        // if(timePicked)
    }

    public void ChatGPTStrafeLeft()
    {
        Vector3 strafeDirection = Quaternion.Euler(0, 90, 0) * (player.position - transform.position);
        agent.destination = player.position + strafeDirection.normalized * strafeDistance;
    }


    public void ChatGPTStrafeRight()
    {
        Vector3 strafeDirection = Quaternion.Euler(0, 90, 0) * (player.position - transform.position);
        agent.destination = player.position + strafeDirection.normalized * strafeDistance;
    }
    
    public void StrafeLeft()
    {
        var offset = transform.position - player.transform.position;
        var dir = Vector3.Cross(offset, Vector3.up);
        agent.SetDestination(transform.position + dir);
        lookPos = player.position - transform.position;
        lookPos.y = 0;
        rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 60f);
    }

    public void StrafeRight()
    {
        var offset = player.transform.position - transform.position;
        var dir = Vector3.Cross(offset, Vector3.up);
        agent.SetDestination(transform.position + dir);
        lookPos = player.position - transform.position;
        lookPos.y = 0;
        rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 60f);
    }
}