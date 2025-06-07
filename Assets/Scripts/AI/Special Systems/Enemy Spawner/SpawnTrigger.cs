using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] GameObject activeEffect;
    [SerializeField] GameObject inactiveEffect;

    bool isTriggered;

    public bool GetIsTriggered() => isTriggered;

    public void SetTriggered(bool value)
    {
        isTriggered = value;
        activeEffect.SetActive(false);

        var effect = Instantiate(inactiveEffect, transform.position, Quaternion.identity);
    }
}