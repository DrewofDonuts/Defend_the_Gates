#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SceneTemplate;
using UnityEditor.VersionControl;
using UnityEngine.SceneManagement;
using EditorUtility = UnityEditor.EditorUtility;

public class RPGTemplatePipeline : ISceneTemplatePipeline
{
    public virtual bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset)
    {
        return true;
    }

    public virtual void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive,
        string sceneName) { }

    public virtual void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, Scene scene, bool isAdditive,
        string sceneName)
    {
        //Check if user wishes to save scene
        if (!EditorUtility.DisplayDialog("Save", "Save the scene", "yes", "No"))
            return;

        //Show the Save File panel
        string path = EditorUtility.SaveFilePanel("Save Scene", "Assets/Scenes", sceneName, "unity");

        //Check if a path was specified
        if (string.IsNullOrEmpty(path))
            return;

        //Make the path relative to the project folder to save it
        path = FileUtil.GetProjectRelativePath(path);
        EditorSceneManager.SaveScene(scene, path);

        AssetDatabase.Refresh();

        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        scenes.Add(new EditorBuildSettingsScene(path, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }
}
#endif