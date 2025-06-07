using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance { get; private set; }


        [FormerlySerializedAs("GeneralInventorySlots")]
        public ItemSlot[] ItemSlots = new ItemSlot[INVENTORY_SIZE];

        [SerializeField] Item debugItem;

        const int INVENTORY_SIZE = 9;

        void Awake()
        {
            Instance = this;


            for (int i = 0; i < INVENTORY_SIZE; i++)
                ItemSlots[i] = new ItemSlot();
        }

        public void AddItem(Item item)
        {
            var firstAvailableSlot = ItemSlots.FirstOrDefault(t => t.IsEmpty);
            if (firstAvailableSlot != null) firstAvailableSlot.SetItem(item);
        }


        public void Bind(List<SlotData> slotDatas)
        {
            for (var i = 0; i < ItemSlots.Length; i++)
            {
                var slot = ItemSlots[i];

                //Checks if the slot data exists, if not, creates a new one
                var slotData = slotDatas.FirstOrDefault(t => t.slotName == "General" + i);

                if (slotData == null)
                {
                    //this actually creates the slot data in GameData.SlotDatas
                    slotData = new SlotData { slotName = "General" + i };
                    slotDatas.Add(slotData);
                }

                //Bind the slot data to inventory slot
                slot.Bind(slotData);
            }
        }

        [Button("Add Debug Item")]
        void AddDebugItem() => AddItem(debugItem);

        //Use reverse loop to move items right
        [Button("Move items Right")]
        void MoveItemsRight()
        {
            var lastItem = ItemSlots.Last().Item;
            for (var i = INVENTORY_SIZE - 1; i > 0; i--)
            {
                ItemSlots[i].SetItem(ItemSlots[i - 1].Item);
            }

            ItemSlots.First().SetItem(lastItem);
        }

        [Button("Instantiate First Item and Remove it")]
        void InstantiateFirstItemAndRemoveIt()
        {
            if (ItemSlots.First().Item != null)
            {
                var item = ItemSlots.First().Item;
                var thing = Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
                thing.transform.localPosition = new Vector3(0, 0, 2) + transform.position;
                ItemSlots.First().Item = null;
            }
        }
    }


    [Serializable]
    public class ItemSlot
    {
        public bool IsEmpty => Item == null;
        public event Action Changed;
        public Item Item;
        SlotData _slotData;

        public void SetItem(Item item)
        {
            var previousItem = Item;
            Item = item;

            //slot Item Name is the name of the item
            _slotData.itemName = item?.name ?? string.Empty;

            //If the item has changed, invoke the changed event
            if (previousItem != Item)
                Changed?.Invoke();
        }

        public void Bind(SlotData slotData)
        {
            _slotData = slotData;

            //If the slot data has an item name, load the item
            var item = Resources.Load<Item>("Items/" + _slotData.itemName);

            // Debug.LogError($"Attempted to load item {_slotData.itemName}");

            SetItem(item);
        }

        public void Swap(ItemSlot draggedSlot)
        {
            // Store the item in the other slot
            var itemInOtherSlot = draggedSlot.Item;

            // Set the item of the other slot to the current item
            draggedSlot.SetItem(Item);

            // Set the current item to the item from the other slot
            SetItem(itemInOtherSlot);
        }
    }
}