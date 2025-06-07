using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    //This will be used to hold all the components that are needed for the player 
    //Will remove components from StateMachine
    public class PlayerComponents : ComponentHandler
    {
        [field: SerializeReference] public HolyShieldController HolyShieldController { get; private set; }

        [field: SerializeField]
        public PlayerDialogueSystemController PlayerDialogueSystemController { get; private set; }

        [SerializeField] FeedbackHandler feedbackHandler;


        [field: Header("Player  Components")]
        [field: SerializeField] public TargetController TargetController { get; private set; }
        [field: SerializeField] public LockOnController LockOnController { get; private set; }
        [field: SerializeField] public Transform ExecutionPoint { get; private set; }
        [field: SerializeField] public GroundExecutionPointDetector GroundExecutionPointDetector { get; private set; }
        [SerializeField] HealController healController;
        [SerializeField] FocusHandler focusHandler;
        [SerializeField] MouseAimController mouseAimController;
        [SerializeField] PlayerInputController playerInputController;
        [SerializeField] AmmoController ammoController;
        [SerializeField] HighlightEffectControllerBase highlightEffectController;
        [FormerlySerializedAs("runProgressController")] [SerializeField]
        PlayerStatsController playerStatsController;
        [SerializeField] HolyChargeController holyChargeController;
        [SerializeField] CrusadeController crusadeController;
        [SerializeField] ExecutionController executionController;
        [SerializeField] Interactor interactor;
        [SerializeField] CameraHandler cameraHandler;


        public Transform MainCameraTransform { get; private set; }
        public FeedbackHandler GetFeedbackHandler() => feedbackHandler;
        public FocusHandler GetFocusHandler() => focusHandler;
        public HealController GetHealController() => healController;
        public MouseAimController GetMouseAimController() => mouseAimController;
        public PlayerInputController GetPlayerInputController() => playerInputController;
        public AmmoController GetAmmoController() => ammoController;
        public HighlightEffectControllerBase GetHighlightEffectController() => highlightEffectController;
        public PlayerStatsController GetStatsController() => playerStatsController;
        public HolyChargeController GetHolyChargeController() => holyChargeController;
        public CrusadeController GetCrusadeController() => crusadeController;
        public ExecutionController GetExecutionController() => executionController;
        public Interactor GetInteractor() => interactor;
        public CameraHandler GetCameraHandler() => cameraHandler;

        void Start()
        {
            if (Camera.main != null)
                MainCameraTransform = Camera.main.transform;

            // HighlightEffect.highlighted = false;
        }


#if UNITY_EDITOR


        [Button(ButtonSizes.Medium), GUIColor(.25f, .50f, 0f)]
        public override void LoadComponents()
        {
            base.LoadComponents();

            feedbackHandler = GetComponentInChildren<FeedbackHandler>();
            LockOnController = GetComponentInChildren<LockOnController>();
            GroundExecutionPointDetector = GetComponentInChildren<GroundExecutionPointDetector>();
            TargetController = GetComponentInChildren<TargetController>();
            TargetController.LoadComponents(transform);
            PlayerDialogueSystemController = GetComponentInChildren<PlayerDialogueSystemController>();
            HolyShieldController = GetComponentInChildren<HolyShieldController>();
            focusHandler = GetComponentInChildren<FocusHandler>();
            healController = GetComponentInChildren<HealController>();
            mouseAimController = GetComponent<MouseAimController>();
            playerInputController = GetComponentInChildren<PlayerInputController>();
            ammoController = GetComponentInChildren<AmmoController>();
            highlightEffectController = GetComponentInChildren<HighlightEffectControllerBase>();
            playerStatsController = GetComponentInChildren<PlayerStatsController>();
            holyChargeController = GetComponentInChildren<HolyChargeController>();
            crusadeController = GetComponentInChildren<CrusadeController>();
            executionController = GetComponentInChildren<ExecutionController>();
            interactor = GetComponentInChildren<Interactor>();
            cameraHandler = GetComponentInChildren<CameraHandler>();
        }
#endif
    }
}