using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [Obsolete("Inventory replaces this class", true)]

    public class PlayerInventory : MonoBehaviour, IBind<PlayerData>
    {
        public List<ItemAbstract> items = new();
        PlayerData data;
        

        internal void PickUp(ItemAbstract item, bool isNew = false)
        {
            // items.Add(item);

            // //Check if the item is new and if it's not already in the list
            // if (isNew && !data.items.Contains(item))
            //     data.items.Add(item);
        }

        public void Bind(PlayerData _data)
        {
            // Debug.Log("Binding player data");
            // data = _data;
            //
            //
            // var itemsCopy = new List<ItemAbstract>(data.items);
            //
            // foreach (var item in itemsCopy)
            // {
            //     if (item != null && !items.Contains(item))
            //         PickUp(item);
            // }
            
        }
    }
}