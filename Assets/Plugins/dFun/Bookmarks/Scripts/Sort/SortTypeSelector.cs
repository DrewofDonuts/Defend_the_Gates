using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class SortTypeSelector
    {
        private const string CustomName = "Custom ↕";
        private const string NameAscendingName = "Name ↓";
        private const string NameDescendingName = "Name ↑";
        private const string DateCreatedAscendingName = "Date added ↓";
        private const string DateCreatedDescendingName = "Date added ↑";

        private const string CustomShortName = "C↕ ";
        private const string NameAscendingShortName = "N↓ ";
        private const string NameDescendingShortName = "N↑ ";
        private const string DateCreatedAscendingShortName = "D↓ ";
        private const string DateCreatedDescendingShortName = "D↑ ";

        private static readonly Dictionary<SortType, GUIContent> SortShortNameContent =
            new Dictionary<SortType, GUIContent>();

        public static void Show(SortType activeSortType)
        {
            GenericMenu menu = new GenericMenu();

            AddSelectMenuItem(menu, SortType.Custom, CustomName, activeSortType);
            AddSelectMenuItem(menu, SortType.NameAscending, NameAscendingName, activeSortType);
            AddSelectMenuItem(menu, SortType.NameDescending, NameDescendingName, activeSortType);
            AddSelectMenuItem(menu, SortType.DateCreatedAscending, DateCreatedAscendingName, activeSortType);
            AddSelectMenuItem(menu, SortType.DateCreatedDescending, DateCreatedDescendingName, activeSortType);

            menu.ShowAsContext();
        }

        private static void AddSelectMenuItem(
            GenericMenu menu, SortType sortType, string name, SortType activeSortType)
        {
            menu.AddItem(
                new GUIContent(name),
                sortType == activeSortType,
                SaveNewSortType,
                sortType
            );
        }

        private static void SaveNewSortType(object newSortTypeObj)
        {
            SortType newSortType = (SortType)newSortTypeObj;
            Bookmarks bookmarks = BookmarksStorage.Get();
            BookmarksUndo.BeforeSortBookmarks();
            bookmarks.SortType = newSortType;
        }

        private static string GetShortName(SortType sortType)
        {
            switch (sortType)
            {
                case SortType.None: return string.Empty;
                case SortType.Custom: return CustomShortName;
                case SortType.NameAscending: return NameAscendingShortName;
                case SortType.NameDescending: return NameDescendingShortName;
                case SortType.DateCreatedAscending: return DateCreatedAscendingShortName;
                case SortType.DateCreatedDescending: return DateCreatedDescendingShortName;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortType), sortType, null);
            }
        }

        public static GUIContent GetShortNameContent(SortType sortType)
        {
            if (SortShortNameContent.TryGetValue(sortType, out GUIContent guiContent))
            {
                return guiContent;
            }

            GUIContent newContent = new GUIContent(GetShortName(sortType));
            SortShortNameContent[sortType] = newContent;

            return newContent;
        }
    }
}