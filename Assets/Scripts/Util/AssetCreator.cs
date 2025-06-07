using Etheral.CharacterActions;
using UnityEditor;
using UnityEngine;

namespace Etheral
{
#if UNITY_EDITOR
    public class AssetCreator
    {
        public static string EventKeyDirectory = "Assets/Etheral/Scriptable Object Assets/Event Objects/";
        public static string CharacterKeyDirectory =
            "Assets/Etheral/Scriptable Object Assets/Event Objects/Character Keys/";


        public static void CreateNewCharacterActionObject(CharacterAction characterAction)
        {
            var asset = ScriptableObject.CreateInstance<CharacterActionObject>();
            string baseAssetPath = "Assets/Etheral/Scriptable Object Assets/CharacterActions/";

            // Validate and sanitize the name
            string sanitizedName = string.IsNullOrWhiteSpace(characterAction.Name) 
                ? "NewCharacterAction" 
                : string.Concat(characterAction.Name.Split(System.IO.Path.GetInvalidFileNameChars()));

            string assetPath = baseAssetPath + sanitizedName + ".asset";

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
        }

        public static NarratorDialogue NewNarratorDialogue()
        {
            var asset = ScriptableObject.CreateInstance<NarratorDialogue>();
            string baseAssetPath = "Assets/Etheral/Scriptable Object Assets/Quest and Dialogue SOs/Narrator Dialogue/New Narrator Dialogue";
            string assetPath = baseAssetPath + ".asset";
            
            AssetDatabase.CreateAsset(asset, assetPath);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($": {asset.name}");
            return asset;
        }
        
        public static SceneData NewSceneData()
        {
            
            var asset = ScriptableObject.CreateInstance<SceneData>();
            string baseAssetPath = "Assets/Etheral/Scriptable Object Assets/Scene Data/New Scene Data";
            string assetPath = baseAssetPath + ".asset";
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return asset;
        }

        public static EventKey NewEventKey()
        {
            var asset = ScriptableObject.CreateInstance<EventKey>();
            AssetDatabase.CreateAsset(asset,
                EventKeyDirectory + "New Event Key.asset");
            AssetDatabase.SaveAssets();

            return asset;
        }

        public static QuestObject CreateNewQuestObject()
        {
            var asset = ScriptableObject.CreateInstance<QuestObject>();
            AssetDatabase.CreateAsset(asset,
                "Assets/Etheral/Scriptable Object Assets/Quest SOs/New Quest Object.asset");
            AssetDatabase.SaveAssets();
            return asset;
        }

        public static CharacterKey NewCharacterKey()
        {
            var asset = ScriptableObject.CreateInstance<CharacterKey>();
            string baseAssetPath = CharacterKeyDirectory + "New Character Key";
            string assetPath = baseAssetPath + ".asset";
            int counter = 1;

            // Check if the asset already exists and find a unique name
            while (UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterKey>(assetPath) != null)
            {
                assetPath = baseAssetPath + " " + counter + ".asset";
                counter++;
            }

            UnityEditor.AssetDatabase.CreateAsset(asset, assetPath);
            UnityEditor.AssetDatabase.SaveAssets();

            return asset;
        }

        public static Spell NewSpell()
        {
            var asset = ScriptableObject.CreateInstance<Spell>();
            UnityEditor.AssetDatabase.CreateAsset(asset,
                "Assets/Etheral/Scriptable Object Assets/Spell SOs/New Spell.asset");
            UnityEditor.AssetDatabase.SaveAssets();

            return asset;
        }

        public static CollisionConfig NewCollisionConfig()
        {
            var asset = ScriptableObject.CreateInstance<CollisionConfig>();
            UnityEditor.AssetDatabase.CreateAsset(asset,
                "Assets/Etheral/Scriptable Object Assets/Collision Config SOs/New Collision Config.asset");
            UnityEditor.AssetDatabase.SaveAssets();

            return asset;
        }
    }
#endif
}