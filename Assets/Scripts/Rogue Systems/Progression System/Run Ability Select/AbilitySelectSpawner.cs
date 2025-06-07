using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Etheral
{
    public class AbilitySelectSpawner : MessengerClass
    {
        [SerializeField] Transform[] position;
        [SerializeField] RunAbilityItem runAbilityPrefab;

        RunSceneData runSceneData;

        void Start()
        {
            if (EtheralSceneManager.Instance != null)
                runSceneData = EtheralSceneManager.Instance.GetSceneData(SceneManager.GetActiveScene().name);
            
            keyToReceive = runSceneData.KeyToReceive;
        }


        protected override void HandleReceivingKey()
        {
            if (!runSceneData.CanSpawnAbilityItems) return;
            base.HandleReceivingKey();
            SpawnAbilityItems();
        }

        void SpawnAbilityItems()
        {
            var unlockedAbilities = GameManager.Instance.GameData.playerAbilityAndResourceData.playerStatsData.unlockedAbilities;

            // Ensure there are abilities to choose from
            if (unlockedAbilities == null || unlockedAbilities.Count == 0)
            {
                return;
            }

            // Shuffle and take up to two

            if (unlockedAbilities.Count == 1)
            {
                // Only one ability, select it
                var abilityItem = Instantiate(runAbilityPrefab, transform);
                abilityItem.transform.position = position[0].position;
                abilityItem.SetAbilityData(unlockedAbilities[0]);
            }
            else
            {
                var random = new System.Random();
                var selectedAbilities = unlockedAbilities.OrderBy(a => random.Next()).Distinct().Take(2).ToList();


                for (int i = 0; i < selectedAbilities.Count; i++)
                {
                    var abilityItem = Instantiate(runAbilityPrefab, transform);
                    abilityItem.transform.position = position[i].position;
                    abilityItem.SetAbilityData(selectedAbilities[i]);
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            foreach (var pos in position)
            {
                Gizmos.DrawWireSphere(pos.position, 0.5f);
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            
        }


#if UNITY_EDITOR
        [Button("Spawn Ability Items")]
        void SpawnAbilityItemsEditor()
        {
            GameManager.Instance.GameData.playerAbilityAndResourceData.playerStatsData.unlockedAbilities.Add(PlayerAbilityTypes.Leap);
            GameManager.Instance.GameData.playerAbilityAndResourceData.playerStatsData.unlockedAbilities.Add(PlayerAbilityTypes.BlessedGround);
            SpawnAbilityItems();
        }


#endif
    }
}