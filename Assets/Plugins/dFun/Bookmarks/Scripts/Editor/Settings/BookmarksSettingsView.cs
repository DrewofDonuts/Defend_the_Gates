using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarksSettingsView
    {
        private bool _showAdvanced = false;

        public void Draw()
        {
            EditorGUILayout.Space();
            BookmarksStorageSettingsView.Draw();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            DrawAdvanced();
        }

        private void DrawAdvanced()
        {
            _showAdvanced = EditorGUILayout.Foldout(_showAdvanced, BookmarksSettingsProvider.Content.AdvancedHeader);
            if (_showAdvanced)
            {
                EditorGUI.indentLevel++;
                DrawSceneObjectResolver();
                EditorGUI.indentLevel--;
            }
        }

        private void DrawSceneObjectResolver()
        {
            TypeCache.TypeCollection availableResolvers = DynamicObjectResolverTypeHelper.AvailableResolvers;
            if (availableResolvers.Count == 0)
            {
                EditorGUILayout.LabelField(
                    BookmarksSettingsProvider.Content.DynamicObjectResolverNotAvailableHeader,
                    BookmarksSettingsProvider.Content.DynamicObjectResolverHeaderStyle
                );
                return;
            }

            string currentResolverClass = BookmarksStorage.Get().Settings.DynamicObjectResolverClass;
            bool isCurrentProviderInTheList = DynamicObjectResolverTypeHelper.FindIndexOfResolver(
                currentResolverClass, availableResolvers, out int currentProviderIndex
            );
            if (!isCurrentProviderInTheList)
            {
                // EditorGUILayout.LabelField(
                //     BookmarksSettingsProvider.Content.SceneObjectResolverErrorHeader,
                //     BookmarksSettingsProvider.Content.SceneObjectResolverHeaderStyle
                // );
                // return;
                currentProviderIndex = 0;
            }

            int newDynamicObjectResolverIndex;
            EditorGUILayout.BeginHorizontal();
            {
                {
                    float labelWidthCache = EditorGUIUtility.labelWidth;
                    const float dynamicObjectResolverLabelWidth = 170f;
                    EditorGUIUtility.labelWidth = dynamicObjectResolverLabelWidth;
                    newDynamicObjectResolverIndex = EditorGUILayout.Popup(
                        BookmarksSettingsProvider.Content.DynamicObjectResolverLabel,
                        currentProviderIndex,
                        DynamicObjectResolverTypeHelper.GetNames(availableResolvers)
                    );
                    EditorGUIUtility.labelWidth = labelWidthCache;
                }
                {
                    if (GUILayout.Button(Icons.HelpIcon, Styles.HelpButtonStyle, GUILayout.Width(20)))
                    {
                        DocumentationHelper.OpenInBrowser(DocumentationHelper.DocsSection.DynamicObjectResolver);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            if (newDynamicObjectResolverIndex != currentProviderIndex)
            {
                string newResolverClass = DynamicObjectResolverTypeHelper.GetTypeSerializableName(
                    availableResolvers[newDynamicObjectResolverIndex]
                );
                BookmarksStorage.Get().Settings.DynamicObjectResolverClass = newResolverClass;
                BookmarksStorage.Save();
                SceneBookmarkHelper.ResetDynamicObjectResolver();
            }
        }
    }
}