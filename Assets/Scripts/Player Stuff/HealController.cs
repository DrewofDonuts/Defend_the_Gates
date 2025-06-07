using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


namespace Etheral
{
    public class HealController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int startingHeals;
        [SerializeField] int startingPotions;
        [SerializeField] int maxHeals = 3;
        [SerializeField] int maxPotions = 3;

        [Header("References")]
        [SerializeField] WeaponInventory weaponInventory;
        [SerializeField] Transform spellParent;
        [SerializeField] Transform potionParent;
        [SerializeField] CanvasGroup canvasGroup;

        [FormerlySerializedAs("spellPrefab")]
        [Header("Prefabs")]
        [SerializeField] GameObject spellUIPrefab;
        [FormerlySerializedAs("potionPrefab")] [SerializeField]
        GameObject potionUIPrefab;
        [SerializeField] GameObject healthDrinkingPrefab;

        [Header("Current Inventory")]
        [ReadOnly] public int potionsRemaining => healData.potionsRemaining;
        [ReadOnly] public int healsRemaining => healData.healsRemaining;

        PlayerData playerData;
        public HealData healData;

        public List<GameObject> spells = new();
        public List<GameObject> potions = new();
        public int MaxHeals => maxHeals;

        public GameObject InstantiatedBottle { get; private set; }

        void Start()
        {
            InstantiatePotionForDrinking(weaponInventory.RightHand);
        }

        public bool HealsRemaining() => healsRemaining > 0;

        public void UseHeal()
        {
            if (healsRemaining <= 0) return;
            spells.FirstOrDefault(x => x.activeSelf)?.SetActive(false);
            healData.healsRemaining--;

            // if (healsRemaining <= 0)
            //     StartCoroutine(FadeOutUIText());
        }

        public bool PotionsRemaining() => potionsRemaining > 0;


        [ContextMenu("Use Potion")]
        public void UsePotion()
        {
            if (potionsRemaining <= 0) return;
            potions.FirstOrDefault(x => x.activeSelf)?.SetActive(false);
            healData.potionsRemaining--;
        }

        [ContextMenu("Add Potion")]
        void AddPotion()
        {
            if (potionsRemaining >= maxPotions) return;

            healData.potionsRemaining++;
            potions.FirstOrDefault(x => x.activeSelf == false)?.SetActive(true);
        }

        public void AddWill(WillItem willItem)
        {
            if (healsRemaining >= maxHeals) return;

            healData.healsRemaining++;
            spells.FirstOrDefault(x => x.activeSelf == false)?.SetActive(true);
        }

        void InstantiatePotionForDrinking(Transform parentTransform)
        {
            InstantiatedBottle = Instantiate(healthDrinkingPrefab, parentTransform);
            InstantiatedBottle.gameObject.SetActive(false);
        }

        public void SetBottleGO(bool isActive) => InstantiatedBottle.gameObject.SetActive(isActive);


        void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out PickUpItem healthPotion))
            {
                if (potionsRemaining < maxPotions && healthPotion.Item.itemType == ItemType.Potion &&
                    Vector3.Distance(transform.position, healthPotion.transform.position) < 2f && !healthPotion.IsUsed)
                {
                    AddPotion();
                    healthPotion.PickUp();
                }
            }
        }

        IEnumerator FadeOutUIText()
        {
            float duration = 2f; // Duration of the fade
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
            spellParent.gameObject.SetActive(false);
        }

        public void Bind(PlayerData _data, bool isSavedGame)
        {
            playerData = _data;
            if (playerData == null) return;

            // Bind the heal data to the player data
            healData = playerData.healData;

            if (!isSavedGame)
            {
                healData = new HealData
                {
                    healsRemaining = startingHeals,
                    potionsRemaining = startingPotions
                };
            }


            // Set the initial values for heals and potions
            IfFreshLevelGiveStarting();

            // Clear existing spells and potions
            ClearSpellsAndPotions();

            for (int i = 0; i < healData.healsRemaining; i++)
            {
                var addedSpell = Instantiate(spellUIPrefab, spellParent);
                spells.Add(addedSpell);
            }

            for (int i = 0; i < healData.potionsRemaining; i++)
            {
                // var addedPotion = Instantiate(potionUIPrefab, potionParent);
                // potions.Add(addedPotion);

                //Enable potions that are added in editor
                potions.FirstOrDefault(x => x.activeSelf == false).SetActive(true);
            }
        }


        void IfFreshLevelGiveStarting()
        {
            if (SceneManager.GetActiveScene().name.Contains("00"))
            {
                healData.healsRemaining = startingHeals;
                healData.potionsRemaining = startingPotions;
            }
        }

        void ClearSpellsAndPotions()
        {
            foreach (var spell in spells)
            {
                Destroy(spell);
            }

            spells.Clear();

            foreach (var potion in potions)
            {
                potion.SetActive(false);

                // Destroy(potion);
            }
        }
    }
}