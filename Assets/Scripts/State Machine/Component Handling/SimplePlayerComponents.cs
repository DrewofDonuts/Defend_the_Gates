using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class SimplePlayerComponents : MonoBehaviour
    {
        [SerializeField] InputObject inputObject;
        [SerializeField] MouseAimController mouseAimController;
        [SerializeField] PlayerInputController playerInputController;
        [SerializeField] CharacterController characterController;
        [SerializeField] ForceReceiver forceReceiver;
        [SerializeField] AnimationHandler animationHandler;
        [SerializeField] CharacterAudio characterAudio;
        [SerializeField] GameObject potionPrefab;


        public InputObject GetInput() => inputObject;
        public MouseAimController GetMouseAimController() => mouseAimController;
        public PlayerInputController GetPlayerInputController() => playerInputController;
        public CharacterController GetCC() => characterController;
        public ForceReceiver GetForceReceiver() => forceReceiver;
        public Transform MainCameraTransform => Camera.main?.transform;
        public AnimationHandler GetAnimationHandler() => animationHandler;
        public CharacterAudio GetCharacterAudio() => characterAudio;
        public GameObject GetPotionPrefab() => potionPrefab;


        void Start()
        {
            potionPrefab.SetActive(false);
        }


#if UNITY_EDITOR


        [Button("Load Components")]
        public void LoadComponents()
        {
            mouseAimController = GetComponentInChildren<MouseAimController>();
            playerInputController = GetComponentInChildren<PlayerInputController>();
            characterController = GetComponent<CharacterController>();
            forceReceiver = GetComponentInChildren<ForceReceiver>();
            animationHandler = GetComponentInChildren<AnimationHandler>();
            characterAudio = GetComponentInChildren<CharacterAudio>();
            
        }



#endif
    }
}