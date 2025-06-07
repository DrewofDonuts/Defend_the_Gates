using System;
using System.Collections;
using Etheral;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class FallHazard : MonoBehaviour
{
    [SerializeField] GameObject fallingStuffPrefab;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] Transform targetOverride;
    [SerializeField] bool repeatDropping;
    [FormerlySerializedAs("spawnRange")] [SerializeField]
    float minSpawnTime = 2f;
    [SerializeField] float maxSpawnTime = 4f;

    Vector3 spawnPoint;
    PlayerStateMachine playerStateMachine;
    bool isPlayerInSpace;
    bool shouldDropHazards;

    void Start()
    {
        playerStateMachine = EventBusPlayerController.PlayerStateMachine;
    }

    public void SetDropping(bool isDrop)
    {
        shouldDropHazards = isDrop;
    }


    [Button("Drop Hazard")]
    public void TargetThenDropOnPlayer()
    {
        if (!shouldDropHazards) return;
        StartCoroutine(WaitBeforeDropping());
    }

    IEnumerator WaitBeforeDropping()
    {
        spawnPoint = !targetOverride ? playerStateMachine.transform.position : targetOverride.position;
        spawnPoint += Vector3.up * .2f;
        var target = Instantiate(targetPrefab, spawnPoint, Quaternion.identity);

        yield return new WaitForSeconds(2f);
        var dropPoint = spawnPoint + Vector3.up * 3f;
        var fallingStuff = Instantiate(fallingStuffPrefab, dropPoint, Quaternion.identity);
        Destroy(target);

        var randomTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);

        yield return new WaitForSeconds(randomTime);

        if (!repeatDropping)
            shouldDropHazards = false;
        TargetThenDropOnPlayer();
    }
}