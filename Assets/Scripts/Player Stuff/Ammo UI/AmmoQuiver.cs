using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class AmmoQuiver : MonoBehaviour
    {

        [SerializeField] int maxAmmo = 20;
        
        // [SerializeField] List<GameObject> arrows = new();
        [ReadOnly]
        public int currentAmmo;
        public int MaxAmmo => maxAmmo;
        public int CurrentAmmo => currentAmmo;
        // public List<GameObject> Arrows => arrows;
        
        [Header("New Ammo UI")]
        [SerializeField] Slider currentAmmoSlider;


        public void Init()
        {
            // currentAmmo = arrows.Count;

            currentAmmo = MaxAmmo;
            currentAmmoSlider.maxValue = MaxAmmo;
            currentAmmoSlider.value = currentAmmo;
        }

        public void AddAmmo()
        {

            currentAmmo = MaxAmmo;
            currentAmmoSlider.value = MaxAmmo;

            //Disabled while testing radial fill
            // for (int i = 0; i < arrows.Count; i++)
            // {
            //     if (!arrows[i].activeSelf)
            //     {
            //         arrows[i].SetActive(true);
            //         currentAmmo++;
            //         // break;
            //     }
            // }

            // foreach (var arrow in arrows)
            // {
            //     if (!arrow.activeSelf)
            //     {
            //         arrow.SetActive(true);
            //         currentAmmo++;
            //         break;
            //     }
            // }
        }

        public void UseAmmo()
        {
            if (currentAmmo <= 0) return;
            
            // arrows.FirstOrDefault(x => x.activeSelf)?.SetActive(false);
            currentAmmo--;
            
            currentAmmoSlider.value = currentAmmo;
        }

#if UNITY_EDITOR
        // [Button("Add to list")]
        // void AddToList()
        // {
        //     arrows.Clear();
        //     foreach (Transform child in transform)
        //     {
        //         arrows.Add(child.gameObject);
        //     }
        // }
#endif
    }
}