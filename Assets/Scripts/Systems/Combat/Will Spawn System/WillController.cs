using System;
using Etheral;
using UnityEngine;


namespace Etheral
{
    //We want this to always be at 1 on the y-axis and 0 on the x and z axis
    public class WillController : MonoBehaviour
    {
        [SerializeField] PlayerHealth playerHealth;
        [SerializeField] HealController healController;
        
        public PlayerHealth PlayerHealth => playerHealth;

        void Start()
        {
            transform.localPosition = new Vector3(0, 1, 0);
        }

        public void ReceiveWill(WillItem willItem)
        {
            if (willItem.WillType == WillType.Defense || willItem.WillType == WillType.Heals)
                playerHealth.AddWill(willItem);

            if (willItem.WillType == WillType.Health)
                healController.AddWill(willItem);

            Destroy(willItem.gameObject);
        }
    }
}