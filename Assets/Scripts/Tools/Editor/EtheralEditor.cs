#if UNITY_EDITOR
using System.Linq;
using Sirenix.OdinInspector.Demos.RPGEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Etheral
{
    public class EtheralEditor : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Etheral/Etheral Editor")]
        static void Open()
        {
            var window = GetWindow<EtheralEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);

            GetWindow<EtheralEditor>().Show();
        }

        protected override OdinMenuTree BuildMenuTree() //builds the menu tree
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 30.00f;
            tree.Config.DrawSearchToolbar = true;

            PopulatePlayer(tree);
            PopulateBosses(tree);
            PopulateEnemies(tree);
            PopulateNPCs(tree);
            PopulateWeaponItems(tree);
            PopulatePlayerSpells(tree);
            PopulateEnemySpells(tree);
            PopulateCharacterBuilderSOs(tree);
            PopulateDialoagueAudioObjects(tree);


            return tree;
        }

        void PopulateBosses(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath("Bosses", "Assets/Etheral/Prefabs - Characters/Bosses",
                typeof(AIAttributes), true, true);
            tree.EnumerateTree().AddIcons<AIAttributes>(x => x.Icon);
        }

        void PopulateDialoagueAudioObjects(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath("Dialogue Audio", "Assets/Etheral/Scriptable Object Assets/Audio SOs",
                typeof(DialogueAudioObject), true, true);
            tree.EnumerateTree().AddIcons<DialogueAudioObject>(x => x.icon);
        }

        void PopulateNPCs(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath("NPCs", "Assets/Etheral/Prefabs - Characters/NPCs",
                typeof(AIAttributes), true, true);
            tree.EnumerateTree().AddIcons<AIAttributes>(x => x.Icon);
        }

        void PopulateWeaponItems(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath("Weapons", "Assets/Etheral/Weapons",
                typeof(WeaponItem), true);
            tree.EnumerateTree().AddIcons<WeaponItem>(x => x.Icon);
        }

        void PopulateEnemies(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath("Enemies", "Assets/Etheral/Prefabs - Characters/Enemies",
                typeof(AIAttributes), true, true);
            tree.EnumerateTree().AddIcons<AIAttributes>(x => x.Icon);
        }

        void PopulatePlayer(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath("Player", "Assets/Etheral/Prefabs - Characters/Player",
                typeof(PlayerAttributes), true, true);
            tree.EnumerateTree().AddIcons<PlayerAttributes>(x => x.Icon);
        }

        void PopulatePlayerSpells(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath("Player Spells", "Assets/Etheral/Spells and Collision Objects/Spells/Player Spells",
                typeof(Spell), true, true);
            tree.EnumerateTree().AddIcons<Spell>(x => x.icon);
        }

        void PopulateEnemySpells(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath("Enemy Spells", "Assets/Etheral/Spells and Collision Objects/Spells/Enemy Spells",
                typeof(Spell), true, true);
            tree.EnumerateTree().AddIcons<Spell>(x => x.icon);
        }
        
        void PopulateCharacterBuilderSOs(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath("Character Builders", "Assets/Etheral/Asset Holders",
                typeof(CharacterBuilderSO), true, true);
            tree.EnumerateTree().AddIcons<CharacterBuilderSO>(x => x.icon);
        }


        protected override void OnBeginDrawEditors()
        {
            if (MenuTree.Selection.FirstOrDefault() == null) return;

            var selected = MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Item")))
                {
                    ScriptableObjectCreator.ShowDialog<WeaponItem>("Assets/Etheral/Weapons",
                        obj =>
                        {
                            obj.itemName = obj.name;
                            base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                        });

                    GUIUtility.ExitGUI();
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Spell")))
                {
                    ScriptableObjectCreator.ShowDialog<Spell>("Assets/Etheral/Spells and Collision Objects/Spells",
                        obj =>
                        {
                            // obj.SpellName = obj.name;
                            base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                        });
                    GUIUtility.ExitGUI();
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Character")))
                {
                    ScriptableObjectCreator.ShowDialog<Character>("Assets/Etheral/Enemies", obj =>
                    {
                        obj.Name = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });

                    GUIUtility.ExitGUI();
                }

                SirenixEditorGUI.EndHorizontalToolbar();
            }
        }
    }
}
#endif


// CreateNewEnemyData createNewEnemyData;
//
//
// protected override void OnDestroy() //ensures we destroy the instance from memory
// {
//     base.OnDestroy();
//     if (createNewEnemyData != null)
//         DestroyImmediate(createNewEnemyData.enemyAttributes);
// }


// public class CreateWeaponData
// {
//     public CreateWeaponData() //creates new data but in memory
//     {
//         enemyAttributes = CreateInstance<MeleeWeaponItem>();
//         enemyAttributes.Name = "New Enemy Data";
//     }
//
//     [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
//     public EnemyAttributes enemyAttributes;
//
//     [Button("Add New Enemy SO")]
//     void SavesNewData() //saves the asset to media
//     {
//         AssetDatabase.CreateAsset(enemyAttributes, "Assets/Etheral/Enemies/" + enemyAttributes.Name + ".asset");
//         AssetDatabase.SaveAssets();
//
//         //Create new instance of the SO
//         enemyAttributes = CreateInstance<EnemyAttributes>();
//         enemyAttributes.Name = "New Enemy Data";
//     }
// }


// if (SirenixEditorGUI.ToolbarButton(new GUIContent("Scenes")))
// {
//     GenericMenu menu = new GenericMenu();
//     Scene currentScene = EditorSceneManager.GetActiveScene();
//     string[] sceneGuids = AssetDatabase.FindAssets("t:scene", new[] { "Assets/Etheral/Scenes" });
//     for (int i = 0; i < sceneGuids.Length; i++)
//     {
//         string path = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);
//         string name = Path.GetFileNameWithoutExtension(path);
//
//         menu.AddItem(new GUIContent(name), string.Compare(currentScene.name, name) == 0,
//             () => OpenScene(currentScene, path));
//     }
//
//     menu.ShowAsContext();
// }
//
// void OpenScene(Scene currentScene, string path)
// {
//     if (currentScene.isDirty)
//     {
//         if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
//             EditorSceneManager.OpenScene(path);
//     }
//     else
//     {
//         EditorSceneManager.OpenScene(path);
//     }
// }