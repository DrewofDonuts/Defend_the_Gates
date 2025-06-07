using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    /// For in-asset bookmark view only. 
    [CustomPropertyDrawer(typeof(Bookmark))]
    public class BookmarkPropertyDrawer : PropertyDrawer
    {
        private const string DefaultButtonName = "open bookmark";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                SerializedProperty nameProperty = property.FindPropertyRelative("bookmarkName");
                string buttonName = nameProperty != null ? nameProperty.stringValue : DefaultButtonName;
                if (string.IsNullOrEmpty(buttonName))
                {
                    buttonName = DefaultButtonName;
                }

                if (GUI.Button(position, buttonName, Styles.BookmarkButtonStyleListView))
                {
                    if (FindBookmark(property, out Bookmark targetBookmark))
                    {
                        BookmarkSelectionHelper.OpenSingleBookmark(targetBookmark);
                        GUIUtility.ExitGUI();
                    }
                }
            }
            EditorGUI.EndProperty();
        }

        private bool FindBookmark(SerializedProperty bookmarkProperty, out Bookmark targetBookmark)
        {
            targetBookmark = default;
            if (bookmarkProperty == null)
            {
                return false;
            }

            SerializedObject serializedBookmarksWrapper = bookmarkProperty.serializedObject;
            if (serializedBookmarksWrapper == null)
            {
                return false;
            }

            BookmarksWrapper bookmarksWrapper = serializedBookmarksWrapper.targetObject as BookmarksWrapper;
            if (bookmarksWrapper == null)
            {
                return false;
            }

            Bookmarks bookmarks = bookmarksWrapper.Bookmarks;
            if (bookmarks == null)
            {
                return false;
            }

            SerializedProperty serializedBookmarks = serializedBookmarksWrapper.FindProperty(
                nameof(BookmarksWrapper.Bookmarks).ToLower()
            );
            if (serializedBookmarks == null)
            {
                return false;
            }

            SerializedProperty serializedGroups = serializedBookmarks.FindPropertyRelative(
                nameof(Bookmarks.Groups).ToLower()
            );
            if (serializedGroups == null || !serializedGroups.isArray)
            {
                return false;
            }

            for (int groupIndex = 0, iSize = serializedGroups.arraySize; groupIndex < iSize; groupIndex++)
            {
                SerializedProperty serializedGroup = serializedGroups.GetArrayElementAtIndex(groupIndex);
                if (!FindBookmarkInGroup(serializedGroup, bookmarkProperty, out int targetBookmarkIndex))
                {
                    continue;
                }

                BookmarksGroup[] groups = bookmarks.Groups;
                if (groupIndex > groups.Length - 1)
                {
                    return false;
                }

                BookmarksGroup targetGroup = groups[groupIndex];
                List<Bookmark> targetGroupBookmarks = targetGroup.Bookmarks;

                if (targetBookmarkIndex > targetGroupBookmarks.Count - 1)
                {
                    return false;
                }

                targetBookmark = targetGroupBookmarks[targetBookmarkIndex];
                return true;
            }

            return false;
        }

        private bool FindBookmarkInGroup(
            SerializedProperty groupProperty, SerializedProperty targetSerializedBookmark, out int targetBookmarkIndex)
        {
            targetBookmarkIndex = -1;
            SerializedProperty bookmarksProperty = groupProperty.FindPropertyRelative(
                nameof(BookmarksGroup.Bookmarks).ToLower()
            );
            if (bookmarksProperty == null || !bookmarksProperty.isArray)
            {
                return false;
            }

            for (int i = 0, iSize = bookmarksProperty.arraySize; i < iSize; i++)
            {
                SerializedProperty bookmarkProperty = bookmarksProperty.GetArrayElementAtIndex(i);
                if (SerializedProperty.EqualContents(targetSerializedBookmark, bookmarkProperty))
                {
                    targetBookmarkIndex = i;
                    return true;
                }
            }

            return false;
        }
    }
}