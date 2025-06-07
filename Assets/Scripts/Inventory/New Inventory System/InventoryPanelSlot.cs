using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral
{
    public class InventoryPanelSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler
    {
        static InventoryPanelSlot Focused;
        public ItemSlot itemSlot;
        [SerializeField] Outline outline;
        [SerializeField] Color draggingColor = Color.gray;
        [SerializeField] Image itemIcon;

        [FormerlySerializedAs("dragIcon")] [SerializeField]
        Image draggedItemIcon;


        public void Bind(ItemSlot inventoryItemSlot)
        {
            itemSlot = inventoryItemSlot;
            UpdateIcon();

            //using the event system to update the icon when the item changes
            itemSlot.Changed += UpdateIcon;
        }

        void UpdateIcon()
        {
            if (itemSlot.Item != null)
            {
                itemIcon.sprite = itemSlot.Item.Icon;
                itemIcon.enabled = true;
            }
            else
            {
                itemIcon.sprite = null;
                itemIcon.enabled = false;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Focused = this;
            outline.enabled = true;
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            if (Focused == this)
                Focused = null;
            outline.enabled = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemSlot.IsEmpty)
                return;

            itemIcon.color = draggingColor;
            
            //setting the dragged item icon
            draggedItemIcon.sprite = itemIcon.sprite;
            draggedItemIcon.enabled = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            draggedItemIcon.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //if the slot is not empty and we have a focused slot
            if(!itemSlot.IsEmpty && Focused != null)
                itemSlot.Swap(Focused.itemSlot);
            
            itemIcon.color = Color.white;
            draggedItemIcon.sprite = null;
            draggedItemIcon.enabled = false;
        }
    }
}