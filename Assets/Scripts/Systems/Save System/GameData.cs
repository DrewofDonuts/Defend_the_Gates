using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Etheral
{
    [Serializable]
    public class GameData
    {
        public List<PlayerData> playerDataList = new();
        public PlayerData playerData = new();
        public bool isSavedData;
        public string GameName;
        public string CurrentLevelName;
        public List<LevelData> LevelData = new();
        public string dialogueQuestData;
        public List<SlotData> slotData = new();
        public QuestSaveData questSaveData = new();

        public PlayerAbilityAndResourceData playerAbilityAndResourceData = new();
        public List<PlayerAbilityAndResourceData> playerAbilityAndResourceDataList = new();
    }

    [Serializable]
    public class QuestSaveData : INamed
    {
        public Dictionary<string, bool> questDictionary = new();
        public string Name { get; set; }
    }
    
    [Serializable]
    public class LevelData
    {
        public string LevelName;
        public List<EnemyData> EnemyData = new();
        public List<DoorData> DoorData = new();
        public List<PickUpItemData> ItemData = new();
        public List<EntityData> EntityData = new();
        public PlayerPositionData PlayerPositionData = new();

    }
    
    [Serializable]
    public class PlayerPositionData
    {
        public bool isSavedPosition;
        public Vector3 playerPosition;
        public Quaternion playerRotation;
        
        public void SetLastPlayerPositionForLevel(Vector3 position, Quaternion rotation)
        {
            playerPosition = position;
            playerRotation = rotation;
            isSavedPosition = true;
        }
    }

    [Serializable]
    public class EnemyData : CharacterData, INamed
    {
        public bool isDead;
        public Vector3 position;
        public Quaternion rotation;
    }

    [Serializable]
    public class PlayerData : CharacterData, INamed
    {
        public List<string> Items = new();
        public int currentHeals;
        public int currentPotions;
        public bool isSavedData;
        
        public Vector3 position;
        public Quaternion rotation;
        public HealData healData = new();

        
        //TO TEST IF I CAN'T GET POSITION TO WORK FROM LEVEL DATA - REMOVE IF LEVEL DATA WORKS
        public List<PlayerPositionData> playerPositionData = new();

        // public PermanentResources permanentResources;
    }

    public class CharacterData
    {
        [field: SerializeField] public string Name { get; set; }
        public float health = 100f;
        public float defense = 100f;
        public float holyWill = 100f;
    }

    [Serializable]
    public class SlotData
    {
        public string slotName;
        public string itemName;
    }

    [Serializable]
    public class DoorData : INamed
    {
        public bool isOpen;
        [field: SerializeField] public string Name { get; set; }
    }

    [Serializable]
    public class PickUpItemData : INamed
    {
        [field: SerializeField] public string Name { get; set; }
        public bool isPickedUp;
    }

    [Serializable]
    public class EntityData : INamed, IKey
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Key { get; set; }
        public bool isDisabled;
        public bool isDestroyed;
        public Vector3 position;
        public Quaternion rotation;
        public bool isSpawned;
    }

    [Serializable]
    public class PlayerAbilityAndResourceData : INamed
    {
        [field: SerializeField] public string Name { get; set; }
        public bool isSavedData;
        public PlayerStatsData playerStatsData = new();
        [Header("Resources")]
        public PermanentResources GatheredResources = new();
    }

    [Serializable]
    public class HealData : INamed
    {
        public string Name { get; set; }
        public int healsRemaining;
        public int potionsRemaining;
        public bool hasSaved;
    }


    public class UnityJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Vector3 vector)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(vector.x);
                writer.WritePropertyName("y");
                writer.WriteValue(vector.y);
                writer.WritePropertyName("z");
                writer.WriteValue(vector.z);
                writer.WriteEndObject();
            }
            else if (value is Quaternion quaternion)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(quaternion.x);
                writer.WritePropertyName("y");
                writer.WriteValue(quaternion.y);
                writer.WritePropertyName("z");
                writer.WriteValue(quaternion.z);
                writer.WritePropertyName("w");
                writer.WriteValue(quaternion.w);
                writer.WriteEndObject();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            float x = jsonObject["x"].Value<float>();
            float y = jsonObject["y"].Value<float>();
            float z = jsonObject["z"].Value<float>();

            if (objectType == typeof(Vector3))
            {
                return new Vector3(x, y, z);
            }
            else if (objectType == typeof(Quaternion))
            {
                float w = jsonObject["w"].Value<float>();
                return new Quaternion(x, y, z, w);
            }

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector3) || objectType == typeof(Quaternion);
        }
    }
}