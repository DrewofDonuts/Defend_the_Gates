using System;
using UnityEngine;

namespace Etheral
{
    [RequireComponent(typeof(SphereCollider), typeof(AudioSource))]
    public class PickUpItem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Rigidbody rb;
        [SerializeField] Item item;
        [SerializeField] AudioSource audioSource;
        [SerializeField] GameObject itemModel;


        [Header("Spawn Physics Settings")]
        [SerializeField] float forwardForce = 2f;
        [SerializeField] float upwardForce = 2f;

        public Item Item => item;
        public bool IsUsed { get; private set; }

        void Start() { }

        // void OnTriggerEnter(Collider other)
        // {
        //     if (other.TryGetComponent<Inventory>(out var inventory))
        //     {
        //         inventory.AddItem(item);
        //         Destroy(gameObject);
        //     }
        // }

        public void PickUp()
        {
            itemModel.SetActive(false);
            AudioProcessor.PlaySingleOneShot(audioSource, item.itemPickupSound, AudioType.pickup, .70f, 1.20f);
            IsUsed = true;
            Destroy(gameObject, 3f);
        }
        
        public void SpawnAfterMoving(Vector3 position)
        {
            transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            transform.position = position;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.up * upwardForce + transform.forward * forwardForce, ForceMode.Impulse);
        }
    }
}