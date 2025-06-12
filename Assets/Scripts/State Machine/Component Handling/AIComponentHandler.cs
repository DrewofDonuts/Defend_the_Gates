using System;
using Etheral.Combat;
using PixelCrushers.QuestMachine;
using Sirenix.OdinInspector;
using TeleportFX;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Etheral
{
    public class AIComponentHandler : ComponentHandler
    {
        [FoldoutGroup("AI Components")]
        [SerializeField] protected AIStateSelector stateSelector;

        [FoldoutGroup("AI Components")]
        [SerializeField] AIDialogueSystemController dialogueSystemController;

        [field: FoldoutGroup("AI Components")]
        [field: SerializeField] public NavMeshAgentController navMeshAgentController { get; private set; }


        [FoldoutGroup("AI Components")]
        [SerializeField] EnemyControlledSpawner enemySpawner;
        [FoldoutGroup("AI Components")]
        [SerializeField] LineController lineController;
        [FoldoutGroup("AI Components")]
        [SerializeField] PatrolController patrolController;
        [FoldoutGroup("AI Components")]
        [SerializeField] StatHandler statHandler;
        [FormerlySerializedAs("enemyGateHandler")]
        [FoldoutGroup("AI Components")]
        [SerializeField] AIGateHandler aiGateHandler;


        public AIStateSelector GetStateSelector() => stateSelector;
        public AIDialogueSystemController GetDialogueSystemController() => dialogueSystemController;

        [FormerlySerializedAs("highlightEffectController")]
        [FoldoutGroup("Enemy Components")]
        [SerializeField] HighlightEffectControllerAI _highlightEffectControllerAI;
        [FoldoutGroup("Enemy Components")]
        [SerializeField] HeadExecutionPoint headExecutionPoint;
        [FormerlySerializedAs("enemyLockOnController")]
        [FoldoutGroup("Enemy Components")]
        [SerializeField] AILockOnController aiLockOnController;
        [FormerlySerializedAs("overrideStateController")]
        [FoldoutGroup("Enemy Components")]
        [SerializeField] StateOverrideController _stateOverrideController;


        [FoldoutGroup("Boss Components")]
        [SerializeField] Component customController;

        public HeadExecutionPoint GetHeadExecutionPoint() => headExecutionPoint;
        public AILockOnController GetAILockOnController() => aiLockOnController;
        public StateOverrideController GetOverrideStateController() => _stateOverrideController;
        public NavMeshAgentController GetNavMeshAgentController() => navMeshAgentController;
        public EnemyControlledSpawner GetEnemySpawner() => enemySpawner;
        public LineController GetLineController() => lineController;
        public PatrolController GetPatrolController() => patrolController;
        public HighlightEffectControllerAI GetHighlightEffectController() => _highlightEffectControllerAI;
        public StatHandler GetStatHandler() => statHandler;
        public AIGateHandler GetAIGateHandler() => aiGateHandler;

        public T GetCustomController<T>() where T : Component
        {
            return customController as T;
        }


#if UNITY_EDITOR
        [Button("Load AI Components")]
        public virtual void LoadAIComponents()
        {
            base.LoadComponents();

            var dialogueSys = GetComponentInChildren<AIDialogueSystemController>();
            navMeshAgentController = GetComponent<NavMeshAgentController>();
            dialogueSystemController = GetComponentInChildren<AIDialogueSystemController>();

            if (dialogueSys != null)
                dialogueSystemController = dialogueSys;
            else
                Debug.LogError("Dialogue System Controller is null");

            enemySpawner = GetComponentInChildren<EnemyControlledSpawner>();
            lineController = GetComponentInChildren<LineController>();
            patrolController = GetComponentInChildren<PatrolController>();
        }

        [Button("Load Enemy  Components")]
        public void LoadEnemyComponents()
        {
            aiLockOnController = GetComponentInChildren<AILockOnController>();
            _stateOverrideController = GetComponent<StateOverrideController>();
            _highlightEffectControllerAI = GetComponentInChildren<HighlightEffectControllerAI>();
            headExecutionPoint = GetComponentInChildren<HeadExecutionPoint>();
        }

        [Button("Load Prefab Variant Components")]
        public void LoadEnemyVariantComponents()
        {
            if (headExecutionPoint != null) headExecutionPoint.SetHeadTransform(GetHead().transform);
        }
#endif
    }
}