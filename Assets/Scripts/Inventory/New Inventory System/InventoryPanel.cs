using System;
using UnityEngine;

namespace Etheral
{
    public class InventoryPanel : MonoBehaviour
    {
        void Start()
        {
            Bind(Inventory.Instance);
        }

        //Binds the Inventory ItemSlots to the InventoryPanelSlots in the UI
        public void Bind(Inventory inventory)
        {
            var panelSlots = GetComponentsInChildren<InventoryPanelSlot>();
            for (int i = 0; i < panelSlots.Length; i++)
            {
                panelSlots[i].Bind(inventory.ItemSlots[i]);
            }
        }
    }
}