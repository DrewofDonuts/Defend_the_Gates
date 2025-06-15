using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Etheral
{
    public class PlayerHealth : Health
    {
        [SerializeField] PlayerStatsController playerStatsController;
        public PlayerData playerData = new();
        public PlayerPositionData playerPositionData = new();
        public string Name => CharacterAttributes.Name;

        [Header("Dev Mode")]
        public bool godHealth;


        public override float CurrentHolyCharge { get; set; }

        public override float CurrentHealth
        {
            get => playerData.health;
            set => playerData.health = value;
        }
        public override float CurrentDefense
        {
            get => playerData.defense;
            set => playerData.defense = value;
        }

        public override float MaxHealth => playerStatsController.GetMaxHealth();
        public override float MaxDefense => playerStatsController.GetMaxArmor();
        public override float MaxHolyCharge => 100f;

        protected override void Start()
        {
            base.Start();


            StartCoroutine(JoinPlayer());
            EventBusPlayerController.OnInjurePlayerEvent += InjurePlayer;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventBusPlayerController.OnInjurePlayerEvent -= InjurePlayer;
        }


        void InjurePlayer(IDamage damage)
        {
            HandleGroundDamage(damage);
        }


        IEnumerator JoinPlayer()
        {
            // yield return new WaitUntil(() => GameManager.Instance != null);

            yield return new WaitForSeconds(.15f);

            //if GameManager is not available, Current stats are set from CharacterAttributes
            if (GameManager.Instance == null)
            {
                healthProcessor = new HealthProcessor(this, playerData);
                healthProcessor.SetInitialCurrentHealth(CharacterAttributes.MaxHealth);
            }

            CurrentDefense = 0;
            CurrentHolyCharge = 0;
            SetStatBars();
            ValueChanged();
        }

        public void BindPositionData(PlayerPositionData levelDataPlayerPositionData)
        {
            playerPositionData = levelDataPlayerPositionData;

            if (playerPositionData.isSavedPosition)
                RestorePosition(playerPositionData);

            playerPositionData.isSavedPosition = true;


            Debug.Log($"playerPositionData: {playerPositionData.playerPosition}" +
                      $" playerRotation: {playerPositionData.playerRotation}");
        }

        public void Bind(GameData _gameData)
        {
            //Bind player data if it exists
            if (_gameData.playerDataList != null)
            {
                playerData = _gameData.playerDataList.FirstOrDefault(t => t.Name == CharacterAttributes.Name);
            }

            // Initialize health processor and set initial values if it's a new game
            InitializeHealthProcessor(playerData);


            //If fresh game, set initial values  
            if (!_gameData.isSavedData && healthProcessor != null)
            {
                healthProcessor.SetInitialCurrentHealth(CharacterAttributes.MaxHealth);
            }
        }

        protected override void Update()
        {
            base.Update();

            //Skip saving player data if game is loading

            if (healthProcessor != null)
            {
                healthProcessor.LoseDefenseAfterTime(Time.deltaTime);
            }
            else if (healthProcessor == null)
            {
                Debug.Log("Health Processor is null");
            }

            if (GameManager.Instance == null) return;
            if (GameManager.Instance.IsLoading) return;

            playerPositionData.playerPosition = transform.position;
            playerPositionData.playerRotation = transform.rotation;
        }

        public void RestorePosition(PlayerPositionData _playerPositionData)
        {
            // if (GameManager.Instance.IsLoading)

            transform.position = _playerPositionData.playerPosition;
            transform.rotation = _playerPositionData.playerRotation;
        }

        public void AddWill(WillItem willItem)
        {
            if (willItem.WillType == WillType.Defense)
                healthProcessor.HealDefense(willItem.WillAmount);
            if (willItem.WillType == WillType.HolyCharge)
                healthProcessor.HealHoly(willItem.WillAmount);
        }


        public override void TakeHit(IDamage damage, float angle = default)
        {
            if (godHealth) return;

            if (CurrentHealth <= 0 || IsVulnerable) return;


            if (IsBlocking && IsWithinBlockAngle(angle))
            {
                HandleBlock(damage);
            }
            else
            {
                HandleHitDamage(damage);
            }
        }
    }
}