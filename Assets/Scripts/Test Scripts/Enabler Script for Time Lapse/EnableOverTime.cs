using Sirenix.OdinInspector;
using UnityEngine;

public class EnableOverTime : MonoBehaviour
{
    public GameObject[] objectsToEnable;
    public float timeToEnable = .5f;
    public bool enableOnStart = false;


    [Button("Enable Objects")]
    public void EnableObjects()
    {
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(false);
        }

        StartCoroutine(EnableObjectsOverTime());
    }

    System.Collections.IEnumerator EnableObjectsOverTime()
    {
        yield return new WaitForSeconds(timeToEnable);

        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }


    [Button("Disable Objects")]
    public void DisableObjects()
    {
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(false);
        }
    }
}