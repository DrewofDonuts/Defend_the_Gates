using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class AnimationSelector : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] DeathType deathType;
        [SerializeField] PrefabContainer prefabContainer;

        string animationName;


        // Start is called before the first frame update
        void Start()
        {
            
            float randomYRotation = Random.Range(0f, 360f);

            // Apply the random rotation to the GameObject
            transform.rotation = Quaternion.Euler(0, randomYRotation, 0);
            
            if (deathType == DeathType.Kneeling)
            {
                animationName = "kneeling";
            }
            else if (deathType != DeathType.Kneeling)
            {
                int randomDeathType = Random.Range(0, 3);
                switch (randomDeathType)
                {
                    case 0:
                        animationName = "down";
                        break;

                    case 1:
                        animationName = "up";
                        break;

                    case 2:
                        animationName = "side";
                        break;
                }
            }

            animator.Play(animationName);

            if (prefabContainer != null)
                InstantiateRandomPrefab();
        }

        void InstantiateRandomPrefab()
        {
            int randomPrefab = Random.Range(0, prefabContainer.Prefabs.Length);
            int randomScale = Random.Range(1, 3);
            float randomLocalOffset = Random.Range(0, 1.5f);

            var blood = Instantiate(prefabContainer.Prefabs[randomPrefab], transform.position, Quaternion.identity,
                transform);

            blood.transform.rotation = Quaternion.Euler(0, 360, 0);
            blood.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            blood.transform.localPosition = new Vector3(randomLocalOffset, 0, randomLocalOffset);
        }
    }


    enum DeathType
    {
        FaceDown,
        FaceUp,
        Side,
        Kneeling
    }
}