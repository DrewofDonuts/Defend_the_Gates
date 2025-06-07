using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral
{
    public class AmmoController : MonoBehaviour
    {
        [FormerlySerializedAs("ammoSlider")] [SerializeField] Slider reloadSlider;
        [SerializeField] AudioSource reloadSound;
        [SerializeField] AudioClip reloadClip;
        [Tooltip("The lower the number, the slower the reload time")]
        [SerializeField] float rechargeRate = 0.5f;
        public AmmoQuiver ammoQuiver;


        bool shouldReload;

        void Start()
        {
            ammoQuiver.Init();
            reloadSlider.value = 0;
        }

        [ContextMenu("Use Ammo")]
        public void UseAmmo()
        {
            ammoQuiver.UseAmmo();
            if (ammoQuiver.CurrentAmmo <= 0)
                shouldReload = true;
        }

        [ContextMenu("Add Ammo")]
        public void AddAmmo() => ammoQuiver.AddAmmo();

        public bool HasAmmo() => ammoQuiver.CurrentAmmo > 0;
        public int GetCurrentAmmo => ammoQuiver.CurrentAmmo;

        void Update()
        {
            if (shouldReload)
            {
                if (ammoQuiver.CurrentAmmo >= ammoQuiver.MaxAmmo)
                {
                    shouldReload = false;
                    return;
                }

                reloadSlider.value += Time.deltaTime * rechargeRate;
                if (reloadSlider.value >= reloadSlider.maxValue)
                {
                    AddAmmo();
                    reloadSlider.value = 0;
                    reloadSound.PlayOneShot(reloadClip);
                    shouldReload = false;
                }
            }
        }


        void OnTriggerStay(Collider other)
        {
            return;
            if (other.TryGetComponent(out PickUpItem ammo))
            {
                Debug.Log($"Should pick up ammo: {ammo.Item.itemType}");
                if (ammoQuiver.CurrentAmmo >= ammoQuiver.MaxAmmo) return;
                if (ammo.IsUsed) return;

                if (ammo.Item.itemType == ItemType.Ammo &&
                    Vector3.Distance(transform.position, ammo.transform.position) < 2f)
                {
                    AddAmmo();
                    ammo.PickUp();
                }
            }
        }


#if UNITY_EDITOR
        [Button("Get Ammo Quiver")]
        void GetAmmoQuiver()
        {
            ammoQuiver = transform.parent.GetComponentInChildren<AmmoQuiver>();
        }
#endif
    }
}