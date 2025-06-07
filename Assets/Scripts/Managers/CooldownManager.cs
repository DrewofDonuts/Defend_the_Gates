using System;
using System.Collections.Generic;
using Etheral;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral
{
    public class CooldownManager : MonoBehaviour
    {
        public List<CharacterAction> actionsOnCooldownWithImage = new();
        public List<CharacterAction> actionsOncooldownNoImage = new();
        public List<Image> cooldownImages = new();

        public GameObject cooldownHolder;


        static CooldownManager _instance;

        public static CooldownManager Instance => _instance;

        void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = this;


            //Only works on root Game Objects
            // DontDestroyOnLoad(this);
        }


        // Start the cooldown for a specific ability
        public void StartCooldown(CharacterAction characterAction)
        {
            if (characterAction == null) return;


            if (characterAction.CooldownPrefab != null)
            {
                if (!actionsOnCooldownWithImage.Contains(characterAction))
                {
                    characterAction.SetCurrentCooldown(characterAction.MaxCooldown);
                    actionsOnCooldownWithImage.Add(characterAction);

                    // cooldownImages.Add(characterAction.CooldownPrefab.GetComponent<Image>());

                    var instantiatedCooldown = Instantiate(characterAction.CooldownPrefab, cooldownHolder.transform);
                    cooldownImages.Add(instantiatedCooldown.IconImage);
                    instantiatedCooldown.SetFillAmount(characterAction.MaxCooldown);
                }
            }
            else
            {
                if (!actionsOncooldownNoImage.Contains(characterAction))
                {
                    characterAction.SetCurrentCooldown(characterAction.MaxCooldown);
                    actionsOncooldownNoImage.Add(characterAction);
                }
            }
        }

        void Update()
        {
            IterateCooldownsWithImage();
            IterateCooldownsNoImage();
        }

        void IterateCooldownsNoImage()
        {
            if (actionsOncooldownNoImage.Count == 0) return;

            for (int i = 0; i < actionsOncooldownNoImage.Count; i++)
            {
                actionsOncooldownNoImage[i].DecrementCurrentCooldown(Time.deltaTime);

                if (actionsOncooldownNoImage[i].GetCurrentCooldown() <= 0)
                {
                    actionsOncooldownNoImage[i].SetCurrentCooldown(0);
                    actionsOncooldownNoImage.RemoveAt(i);
                }
            }
        }

        void IterateCooldownsWithImage()
        {
            if (actionsOnCooldownWithImage.Count == 0)
                return;


            for (int i = 0; i < actionsOnCooldownWithImage.Count; i++)
            {
                actionsOnCooldownWithImage[i].DecrementCurrentCooldown(Time.deltaTime);

                if (actionsOnCooldownWithImage[i].CooldownPrefab != null)
                    cooldownImages[i].fillAmount = actionsOnCooldownWithImage[i].GetCurrentCooldown() /
                                                   actionsOnCooldownWithImage[i].MaxCooldown;


                if (actionsOnCooldownWithImage[i].GetCurrentCooldown() <= 0)
                {
                    actionsOnCooldownWithImage[i].SetCurrentCooldown(0);
                    actionsOnCooldownWithImage.RemoveAt(i);
                    cooldownImages.RemoveAt(i);
                }
            }
        }
    }
}