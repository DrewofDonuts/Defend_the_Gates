using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [Obsolete("PickUpItem replaces this class")]

    public abstract class ItemAbstract : MonoBehaviour, IBind<PickUpItemData>
    {
        public string Name;

        // Transform transform { get; set; }
        public Sprite icon { get; set; }
        public bool isUnique;
        public PickUpItemData data;
        public GameObject itemGameObject;

        [FormerlySerializedAs("destroyOnPickup")]
        public bool destroyOnPickUp;

        public void Use()
        {
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerInventory>(out var playerInventory))
            {
                playerInventory.PickUp(this, true);
                HandlePickUp();
            }
        }

        void HandlePickUp()
        {
            data.isPickedUp = true;

            if (destroyOnPickUp)
                Destroy(gameObject);
        }
        
        //this is to control the item in the scene, not the item in the inventory
        public void Bind(PickUpItemData _data)
        {
            data = _data;
            name = data.Name;

            if (data.isPickedUp)
                Destroy(gameObject);
        }
    }
}