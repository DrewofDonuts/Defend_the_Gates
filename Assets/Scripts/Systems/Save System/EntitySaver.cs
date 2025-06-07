using System;
using System.Collections;
using Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class EntitySaver : MonoBehaviour, IBind<EntityData>, IKey, ISetKey
    {
        [field: SerializeField] public string Key { get; set; }
        public bool isDisabledSave = true;
        public bool isDestroyedSave = true;
        public bool isPositionSave;
        public bool isRotationSave;
        public bool isSpawned;
        public EntityData data;


        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RegisterEntity(this);
                data.isSpawned = isSpawned;

                GameManager.Instance.OnSave += HandleDataOnSave;
                HandleGameObjectStateOnLoad();
            }
        }

        void HandleDataOnSave()
        {
            if (this == null)
            {
                Debug.LogWarning("EntitySaver object has been destroyed.");
                return;
            }

            data.position = transform.position;
            data.rotation = transform.rotation;
        }

        public void Bind(EntityData _data)
        {
            if (_data != null)
                data = _data;

            if (data.isDestroyed)
                Destroy(gameObject);
        }

        void OnEnable()
        {
            Debug.Log("OnEnable");

            if (data != null)
            {
                //used if instantiated during runtime
                SetKeyIfSpawned();
            }
        }

        void SetKeyIfSpawned()
        {
            if (isSpawned)
                SetKey();
        }

        void HandleGameObjectStateOnLoad()
        {
            if (data.isDestroyed)
            {
                Destroy(gameObject);
                return;
            }

            gameObject.SetActive(!data.isDisabled);


            if (isPositionSave && data.position != Vector3.zero)
                transform.position = data.position;

            if (isRotationSave && data.rotation != Quaternion.identity)
                transform.rotation = data.rotation;
        }


        void OnDisable()
        {
            if (isDisabledSave)
                data.isDisabled = gameObject.activeSelf;

            if (isPositionSave)
                data.position = transform.position;

            if (isRotationSave)
                data.rotation = transform.rotation;
        }

        void OnDestroy()
        {
            if (isDisabledSave)
                data.isDisabled = gameObject.activeSelf;

            if (isDestroyedSave)
                data.isDestroyed = true;

            if (isPositionSave)
                data.position = transform.position;

            if (isRotationSave)
                data.rotation = transform.rotation;
        }

        [Button("Set Key")]
        public void SetKey()
        {
            if (!Key.IsNullOrWhitespace()) return;

            Key = Guid.NewGuid().ToString();
        }
    }
}