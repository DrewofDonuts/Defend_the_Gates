using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;

namespace Etheral
{
#if UNITY_EDITOR

    public class AnimatorControlTool : EditorWindow
    {
        public AnimatorToolFunction animatorToolFunction;
        public AnimatorController animatorController;
        public List<StateAnimationData> animationDataList;
        public StateAnimationData newStateData;

        public string newStateName;
        public AnimationClip newStateMotion;

        public int layerIndex;

        bool isReadyForSet;

        [MenuItem("Tools/Etheral/Animator Control Tool")]
        public static void ShowWindow()
        {
            GetWindow<AnimatorControlTool>("Animator Control Tool");
        }

        void OnGUI()
        {
            animatorController = (AnimatorController)EditorGUILayout.ObjectField("Animator Controller",
                animatorController, typeof(AnimatorController), false);

            animatorToolFunction = (AnimatorToolFunction)EditorGUILayout.EnumPopup("Function", animatorToolFunction);

            SerializedObject serializedObject = new SerializedObject(this);


            if (animatorToolFunction == AnimatorToolFunction.CreateNewState)
            {
                SerializedProperty newStateDataProperty = serializedObject.FindProperty("newStateData");
                EditorGUILayout.PropertyField(newStateDataProperty, true);

                newStateName = EditorGUILayout.TextField("New State Name", newStateName);
                newStateMotion = (AnimationClip)EditorGUILayout.ObjectField("New State Motion", newStateMotion,
                    typeof(AnimationClip),
                    false);

                if (GUILayout.Button("Create new state"))
                {
                    CreateNewState();
                }
            }


            if (animatorToolFunction == AnimatorToolFunction.EditStates)
            {
                SerializedProperty animationDataListProperty = serializedObject.FindProperty("animationDataList");
                EditorGUILayout.PropertyField(animationDataListProperty, true);

                layerIndex = EditorGUILayout.IntField("Layer Index", layerIndex);


                if (GUILayout.Button("Get Animation Data For Every State"))
                {
                    GetAnimatorStatesOnlayerZero();
                }

                // if (GUILayout.Button("Set Animation Data For Every State On All Layers"))
                // {
                //     SetControllerDataFromAnimationDataList();
                // }

                GUI.enabled = animationDataList.Any();
                if (GUILayout.Button("Set Animation Data For Every State On Layer"))
                {
                    SetControllerDataForLayer(layerIndex);
                }

                GUI.enabled = true;
            }
        }


        void GetAnimatorStatesOnlayerZero(int layer = 0)
        {
            foreach (var animationState in animatorController.layers[0].stateMachine.states)
            {
                animationDataList.Add(new StateAnimationData(animationState.state.name,
                    animationState.state.motion as AnimationClip, animationState.state.iKOnFeet, animatorController));
            }
        }

        void SetControllerDataForLayer(int layer = 0)
        {
            foreach (var animationData in animationDataList)
            {
                foreach (var state in animatorController.layers[layer].stateMachine.states)
                {
                    if (state.state.name == animationData.animationName)
                    {
                        state.state.motion = animationData.animationClip;
                    }
                }
            }
        }


        void SetControllerDataFromAnimationDataList()
        {
            foreach (var animationData in animationDataList)
            {
                foreach (var layer in animatorController.layers)
                {
                    foreach (var state in layer.stateMachine.states)
                    {
                        if (state.state.name == animationData.animationName)
                        {
                            state.state.motion = animationData.animationClip;
                        }
                    }
                }
            }
        }

        void CreateNewState()
        {
            var state = new AnimatorState();
            state.name = newStateData.animationName;
            state.motion = newStateData.animationClip;
            state.iKOnFeet = newStateData.isFootIK;
            animatorController.layers[0].stateMachine.AddState(state, Vector3.zero);
        }
    }


    public enum AnimatorToolFunction
    {
        EditStates,
        CreateNewState,
        RemoveState,
    }

    [Serializable]
    public class StateAnimationData
    {
        [ReadOnly]
        public string animationName;

        public AnimationClip animationClip;
        public bool isFootIK;
        AnimatorController animatorController;

        public StateAnimationData()
        {
        }

        public StateAnimationData(string animationName, AnimationClip animationClip, bool isFootIK,
            AnimatorController animatorController)
        {
            this.animationName = animationName;
            this.animationClip = animationClip;
            this.isFootIK = isFootIK;
            this.animatorController = animatorController;
        }
    }

    [Serializable]
    public class BlendTreeAnimationData
    {
        public string blendTreeName;
        public BlendTree blendTree;
        public bool isFootIK;


        public BlendTreeAnimationData()
        {
        }

        public BlendTreeAnimationData(string blendTreeName, BlendTree blendTree, bool isFootIK = true)
        {
            this.blendTreeName = blendTreeName;
            this.blendTree = blendTree;
            this.isFootIK = isFootIK;
        }
    }
#endif
}
