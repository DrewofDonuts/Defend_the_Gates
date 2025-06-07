using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class VirtualActor : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: Tooltip("Model received and destroyed at runtime.")]
        [field: SerializeField] public GameObject VirtualActorModel { get; private set; }
        [field: Tooltip("Used to reference the model when testing a timeline.")]
        [field: SerializeField] public GameObject PracticeModel { get; private set; }


        GameObject virtualModelClone;

        IEnumerator Start()
        {
            yield return new WaitUntil(()=> CinematicManager.Instance != null);
            Register();
        }

        void Awake()
        {
            if (PracticeModel != null)
                PracticeModel.SetActive(false);
        }
        
        public void Register()
        {
            CinematicManager.Instance.Register(this);
        }

        public Transform GetVirtualActorModelTransform()
        {
            return transform;
        }

        public void SetVirtualModel(GameObject referenceModel)
        {
            VirtualActorModel = referenceModel;
        }

        public void DestroyVirtualModel()
        {
            if (VirtualActorModel == null)
                throw new Exception("VirtualModel is null.");

            Destroy(virtualModelClone);
        }

        public void InstantiateVirtualModel()
        {
            if (VirtualActorModel == null)
                throw new Exception("VirtualModel is null.");

            var _transform = transform;
            virtualModelClone = Instantiate(VirtualActorModel, _transform.position, _transform.rotation, _transform);
        }

#if UNITY_EDITOR

        [Button("Toggle Practice Model")]
        public void TogglePracticeModel()
        {
            if (PracticeModel == null)
                throw new Exception("PracticeModel is null.");

            if (PracticeModel.activeSelf == false)
                PracticeModel.SetActive(true);
            else if (PracticeModel.activeSelf)
                PracticeModel.SetActive(false);
        }

#endif
    }
}