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

    [Serializable]
    public class AnimatorEditor
    {
        public AnimatorController animatorController;
        public AnimatorToolFunction animatorToolFunction;

        [ShowIf("animatorToolFunction", AnimatorToolFunction.CreateNewState)]
        public bool isChildState;

        [ShowIf("isChildState")]
        public bool createNewSubStateMachine;

        [ShowIf("createNewSubStateMachine")]
        public string newSubStateMachineName;

        [ValueDropdown("subStateMachines")]
        [ShowIf("isChildState")]
        public AnimatorStateMachine subStateMachine;

        [ShowIf("animatorToolFunction", AnimatorToolFunction.CreateNewState)]
        public string newStateName;

        [ShowIf("animatorToolFunction", AnimatorToolFunction.CreateNewState)]
        public AnimationClip newAnimationClip;

        [ShowIf("animatorToolFunction", AnimatorToolFunction.CreateNewState)]
        public bool isFootIK = true;


        [ShowIf("falseAlways")]
        public List<AnimatorStateMachine> subStateMachines;


        [ShowIf("animatorToolFunction", AnimatorToolFunction.RemoveState)]
        [ValueDropdown("animationNames")]
        public string stateToRemove;


        [ShowIf("animatorToolFunction", AnimatorToolFunction.EditStates)]
        [Searchable] [ListDrawerSettings(NumberOfItemsPerPage = 5)]
        public List<BlendTreeAnimationData> blendTreeAnimationData;

        [ShowIf("animatorToolFunction", AnimatorToolFunction.EditStates)]
        [Searchable] [ListDrawerSettings(NumberOfItemsPerPage = 5)]
        public List<StateAnimationData> animationDataList;

        List<String> animationNames => animationDataList.Select(x => x.animationName).ToList();

        public int layer;

        bool isDirty => animationDataList.Count > 0;
        bool falseAlways;


        [ShowIf("animatorToolFunction", AnimatorToolFunction.EditStates)]
        [Button("Get Animation Data For Every State")]
        public void GetAnimatorStatesOnSelectedLayer()
        {
            ClearLists();
            GetTopLayerStates();
            GetStatesFromSubState();
            GetBlendTrees();
            SortAnimationDataListAlpha();
        }

        void SortAnimationDataListAlpha() => animationDataList.Sort((x, y) =>
            string.Compare(x.animationName, y.animationName, StringComparison.Ordinal));


        void GetTopLayerStates()
        {
            foreach (var animatorState in animatorController.layers[layer].stateMachine.states)
            {
                if (animatorState.state.motion is BlendTree) continue;

                if (animationDataList.Exists(x => x.animationName == animatorState.state.name)) continue;

                animationDataList.Add(new StateAnimationData(animatorState.state.name,
                    animatorState.state.motion as AnimationClip, animatorState.state.iKOnFeet, animatorController));
            }
        }

        void GetStatesFromSubState()
        {
            foreach (var substate in animatorController.layers[layer].stateMachine.stateMachines)
            {
                if (!subStateMachines.Contains(substate.stateMachine))
                    subStateMachines.Add(substate.stateMachine);

                foreach (var animationState in substate.stateMachine.states)
                {
                    if (animationDataList.Exists(x => x.animationName == animationState.state.name)) continue;

                    animationDataList.Add(new StateAnimationData(animationState.state.name,
                        animationState.state.motion as AnimationClip, animationState.state.iKOnFeet,
                        animatorController));
                }
            }
        }

        void GetBlendTrees()
        {
            foreach (var animationState in animatorController.layers[layer].stateMachine.states)
            {
                if (animationState.state.motion is BlendTree blendTree)
                {
                    if (blendTreeAnimationData.Exists(x => x.blendTreeName == animationState.state.name)) continue;

                    // blendTrees.Add(blendTree);
                    BlendTreeAnimationData newTreeData = new BlendTreeAnimationData(animationState.state.name,
                        blendTree, animationState.state.iKOnFeet);

                    blendTreeAnimationData.Add(newTreeData);
                }
            }
        }


        [ShowIf("animatorToolFunction", AnimatorToolFunction.EditStates)]
        [EnableIf("isDirty")]
        [Button("Set Animation Data For Every State On All Layers")]
        void SetControllerDataFromAnimationDataList()
        {
            if (animationDataList.Count == 0) return;

            foreach (var animationData in animationDataList)
            {
                foreach (var layer in animatorController.layers)
                {
                    UpdateTopLevelStates(layer, animationData);

                    UpdateStateInSubStates(layer, animationData);
                }
            }
        }

        static void UpdateTopLevelStates(AnimatorControllerLayer layer, StateAnimationData animationData)
        {
            foreach (var state in layer.stateMachine.states)
            {
                if (state.state.name == animationData.animationName)
                {
                    if (state.state.motion == animationData.animationClip) return;
                    Debug.Log("Setting state: " + state.state.name);
                    state.state.motion = animationData.animationClip;
                }
            }
        }

        static void UpdateStateInSubStates(AnimatorControllerLayer layer, StateAnimationData animationData)
        {
            foreach (var subStateMachine in layer.stateMachine.stateMachines)
            {
                foreach (var state in subStateMachine.stateMachine.states)
                {
                    if (state.state.name == animationData.animationName)
                    {
                        if (state.state.motion == animationData.animationClip) return;

                        Debug.Log("Setting state in sub-state machine: " + state.state.name);
                        state.state.motion = animationData.animationClip;
                    }
                }
            }
        }

        [ShowIf("animatorToolFunction", AnimatorToolFunction.EditStates)]
        [EnableIf("isDirty")]
        [Button("Clear Animation Data List")]
        void ClearLists()
        {
            animationDataList.Clear();
            blendTreeAnimationData.Clear();
            subStateMachines.Clear();
        }

        [ShowIf("animatorToolFunction", AnimatorToolFunction.CreateNewState)]
        [Button("Create new state")]
        void CreateNewState()
        {
            if (newStateName == "")
            {
                EditorUtility.DisplayDialog("Warning", "You must enter a name for the new state.", "OK");
                return;
            }

            GetAnimatorStatesOnSelectedLayer();
            AnimatorState state = new AnimatorState();

            state.name = newStateName;
            state.motion = newAnimationClip;
            state.iKOnFeet = isFootIK;

            if (animatorController.layers[layer].stateMachine.states.Any(x => x.state.name == state.name))
            {
                UnityEditor.EditorUtility.DisplayDialog("Warning",
                    "A state with the same name already exists in the AnimatorController.", "OK");
                newStateName = "";
                return;
            }


            if (isChildState && createNewSubStateMachine)
            {
                if (newSubStateMachineName == "")
                    EditorUtility.DisplayDialog("Warning",
                        "You must enter a name for the new sub-state machine.", "OK");

                if (subStateMachines.Any(x => x.name == newSubStateMachineName))
                    subStateMachine = subStateMachines.Find(x => x.name == newSubStateMachineName);
                else
                    subStateMachine = animatorController.layers[layer].stateMachine
                        .AddStateMachine(newSubStateMachineName, Vector3.zero);
            }

            if (isChildState && subStateMachine != null)
                subStateMachine.AddState(state, Vector3.zero);
            else if (isChildState && subStateMachine == null)
                EditorUtility.DisplayDialog("Warning",
                    "You must create a new sub-state machine to add a child state.", "OK");
            else
                animatorController.layers[layer].stateMachine.AddState(state, Vector3.zero);

            isChildState = false;
            createNewSubStateMachine = false;

            newStateName = "";
            newAnimationClip = null;
            GetAnimatorStatesOnSelectedLayer();

            EditorUtility.DisplayDialog("Success", "State created successfully.", "OK");
        }

        [ShowIf("animatorToolFunction", AnimatorToolFunction.RemoveState)]
        [Button("Remove State")]
        void RemoveState()
        {
            if (stateToRemove == null) return;

            Debug.Log("Removing state: " + stateToRemove);


            foreach (var layer in animatorController.layers)
            {
                // Remove state from top-level states
                foreach (var state in layer.stateMachine.states)
                {
                    if (state.state.name == stateToRemove)
                    {
                        if (EditorUtility.DisplayDialog("DELETE STATE",
                                "Are you sure you want to delete the state?", "Yes",
                                "No"))
                        {
                            layer.stateMachine.RemoveState(state.state);
                            ClearLists();
                            GetAnimatorStatesOnSelectedLayer();
                        }
                        else
                            return;
                    }
                }

                // Remove state from substate machines
                foreach (var subStateMachine in layer.stateMachine.stateMachines)
                {
                    foreach (var state in subStateMachine.stateMachine.states)
                    {
                        if (state.state.name == stateToRemove)
                        {
                            if (EditorUtility.DisplayDialog("DELETE STATE",
                                    "Are you sure you want to delete the state?", "Yes",
                                    "No"))
                            {
                                subStateMachine.stateMachine.RemoveState(state.state);
                                ClearLists();
                                GetAnimatorStatesOnSelectedLayer();
                            }
                            else
                                return;
                        }
                    }
                }
            }
        }
    }
#endif
}