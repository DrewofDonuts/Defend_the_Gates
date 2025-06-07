using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DFun.Bookmarks
{
    public static class ProjectBrowserHelper
    {
        public static bool ShowFolderContents(Object folder)
        {
            try
            {
                return ShowFolderContents(folder.GetInstanceID());
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// Based on https://forum.unity.com/threads/tutorial-how-to-to-show-specific-folder-content-in-the-project-window-via-editor-scripting.508247/
        private static bool ShowFolderContents(int folderInstanceID)
        {
            // Find the internal ProjectBrowser class in the editor assembly.
            Assembly editorAssembly = typeof(Editor).Assembly;
            Type projectBrowserType = editorAssembly.GetType("UnityEditor.ProjectBrowser");

            // This is the internal method, which performs the desired action.
            // Should only be called if the project window is in two column mode.
            MethodInfo showFolderContents = projectBrowserType.GetMethod(
                "ShowFolderContents", BindingFlags.Instance | BindingFlags.NonPublic);

            // Find any open project browser windows.
            Object[] projectBrowserInstances = Resources.FindObjectsOfTypeAll(projectBrowserType);

            if (projectBrowserInstances.Length > 0)
            {
                bool showedInAnyWindow = false;
                for (int i = 0, iSize = projectBrowserInstances.Length; i < iSize; i++)
                {
                    showedInAnyWindow |= ShowFolderContentsInternal(
                        projectBrowserInstances[i], showFolderContents, folderInstanceID
                    );
                }

                return showedInAnyWindow;
            }
            else
            {
                EditorWindow projectBrowser = OpenNewProjectBrowser(projectBrowserType);
                return ShowFolderContentsInternal(projectBrowser, showFolderContents, folderInstanceID);
            }
        }

        /// Based on https://forum.unity.com/threads/tutorial-how-to-to-show-specific-folder-content-in-the-project-window-via-editor-scripting.508247/
        private static bool ShowFolderContentsInternal(
            Object projectBrowser, MethodInfo showFolderContents, int folderInstanceID)
        {
            // Sadly, there is no method to check for the view mode.
            // We can use the serialized object to find the private property.
            SerializedObject serializedObject = new SerializedObject(projectBrowser);
            bool inTwoColumnMode = serializedObject.FindProperty("m_ViewMode").enumValueIndex == 1;

            if (!inTwoColumnMode)
            {
                return false;
            }

            bool revealAndFrameInFolderTree = true;
            showFolderContents.Invoke(projectBrowser, new object[] {folderInstanceID, revealAndFrameInFolderTree});
            return true;
        }

        /// https://forum.unity.com/threads/tutorial-how-to-to-show-specific-folder-content-in-the-project-window-via-editor-scripting.508247/
        private static EditorWindow OpenNewProjectBrowser(Type projectBrowserType)
        {
            EditorWindow projectBrowser = EditorWindow.GetWindow(projectBrowserType);
            projectBrowser.Show();

            // Unity does some special initialization logic, which we must call,
            // before we can use the ShowFolderContents method (else we get a NullReferenceException).
            MethodInfo init = projectBrowserType.GetMethod("Init", BindingFlags.Instance | BindingFlags.Public);
            init.Invoke(projectBrowser, null);

            return projectBrowser;
        }
    }
}