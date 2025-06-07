using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DFun.Bookmarks
{
    [Serializable]
    public class Bookmarks
    {
        [SerializeField] private BookmarksGroup[] groups = new BookmarksGroup[1];
        [SerializeField] private SortType sortType = SortType.Custom;
        [SerializeField] private bool autoSave = true;
        [SerializeField] private BookmarksSettings settings;

        private const int DefaultGroupsAmount = 1;
        public BookmarksGroup[] Groups => groups;

        public BookmarksSettings Settings
        {
            get => settings ?? new BookmarksSettings();
            private set => settings = value;
        }

        public int ActiveGroupIndex
        {
            get => ActiveBookmarksGroup.GetActiveGroupIndex(Groups);
            set => ActiveBookmarksGroup.SetActiveGroupIndex(value);
        }

        public bool AutoSave
        {
            get => autoSave;
            set => autoSave = value;
        }

        public SortType SortType
        {
            get => sortType;
            set
            {
                sortType = value;
                this.SortByType(sortType);
                Dirty = true;
            }
        }

        public float GridSizeNormalized
        {
            get
            {
                if (TryToGetActiveGroup(out BookmarksGroup activeGroup) && activeGroup.GroupGridSize.UseGroupGridSize)
                {
                    return activeGroup.GroupGridSize.GroupGridSizeNormalized;
                }

                return BookmarksGridSizeData.GetGridSizeNormalized();
            }
            set
            {
                if (TryToGetActiveGroup(out BookmarksGroup activeGroup) && activeGroup.GroupGridSize.UseGroupGridSize)
                {
                    activeGroup.GroupGridSize.GroupGridSizeNormalized = value;
                    Dirty = true;
                }
                else
                {
                    BookmarksGridSizeData.SetGridSizeNormalized(value);
                }
            }
        }

        public bool IsGridSizeLockedToActiveGroup
        {
            get
            {
                if (TryToGetActiveGroup(out BookmarksGroup activeGroup))
                {
                    return activeGroup.GroupGridSize.UseGroupGridSize;
                }

                return false;
            }
            set
            {
                if (TryToGetActiveGroup(out BookmarksGroup activeGroup))
                {
                    activeGroup.GroupGridSize.GroupGridSizeNormalized = GridSizeNormalized;
                    activeGroup.GroupGridSize.UseGroupGridSize = value;
                    Dirty = true;
                }
            }
        }

        public int TotalGroupsCount
        {
            get
            {
                if (Groups == null) return 0;
                return Groups.Length;
            }
        }

        public int TotalBookmarksCount
        {
            get
            {
                if (Groups == null) return 0;
                int bookmarksCounter = 0;
                BookmarksGroup[] bookmarksGroups = Groups;
                for (int i = 0, iSize = bookmarksGroups.Length; i < iSize; i++)
                {
                    bookmarksCounter += bookmarksGroups[i].Bookmarks.Count;
                }
                return bookmarksCounter;
            }
        }

        public bool Dirty { get; set; }

        private Bookmarks()
        {
        }

        public static Bookmarks CreateInstance()
        {
            Bookmarks instance = new Bookmarks();
            instance.Init();
            return instance;
        }

        public void Init()
        {
            if (groups == null)
            {
                groups = new BookmarksGroup[DefaultGroupsAmount];
            }

            for (int i = 0, iSize = groups.Length; i < iSize; i++)
            {
                BookmarksGroup group = groups[i];
                string defaultGroupName = GenerateDefaultGroupName(i);

                if (group == null)
                {
                    group = new BookmarksGroup(defaultGroupName);
                    groups[i] = group;
                }

                group.NormalizeName(defaultGroupName);
            }
        }

        public void AddItems(Object[] objectReferences)
        {
            if (groups.Length == 0)
            {
                ActiveGroupIndex = 0;
                groups = new[]
                {
                    new BookmarksGroup(
                        GenerateDefaultGroupName(ActiveGroupIndex)
                    )
                };
            }

            if (ActiveGroupIndex > groups.Length - 1)
            {
                Debug.LogWarning($"Group with index {ActiveGroupIndex} does not exits");
                ActiveGroupIndex = 0;
                return;
            }

            groups[ActiveGroupIndex].AddItems(objectReferences);
            groups[ActiveGroupIndex].SortGroup(SortType);

            Dirty = true;
        }

        public void AddBookmarkToGroup(Bookmark bookmark, BookmarksGroup group)
        {
            group.AddBookmark(bookmark);
            group.SortGroup(SortType);

            Dirty = true;
        }

        public void Remove(Bookmark bookmark)
        {
            if (Groups == null || Groups.Length == 0)
            {
                return;
            }

            if (ActiveGroupIndex > Groups.Length - 1)
            {
                return;
            }

            Groups[ActiveGroupIndex].Bookmarks.Remove(bookmark);
            Dirty = true;
        }

        public void RemoveBookmarksFromGroup(BookmarksGroup group, List<int> bookmarksIndicesToRemove)
        {
            if (group == null || bookmarksIndicesToRemove == null || bookmarksIndicesToRemove.Count == 0)
            {
                return;
            }

            group.RemoveByIndices(bookmarksIndicesToRemove);

            Dirty = true;
        }

        public Bookmarks DeepCopy(Bookmarks copyFrom, bool changeActiveGroupIndex = true)
        {
            groups = new BookmarksGroup[copyFrom.Groups.Length];
            for (int i = 0, iSize = groups.Length; i < iSize; i++)
            {
                groups[i] = new BookmarksGroup(copyFrom.Groups[i]);
            }

            if (changeActiveGroupIndex)
            {
                ActiveGroupIndex = copyFrom.ActiveGroupIndex;
            }
            Settings = new BookmarksSettings(copyFrom.Settings);

            return this;
        }

        public void AddNewGroup(bool changeActiveGroupIndex = true)
        {
            int newGroupIndex = groups.Length;
            AddGroup(
                new BookmarksGroup(GenerateDefaultGroupName(newGroupIndex)),
                changeActiveGroupIndex
            );
        }

        public void AddGroup(BookmarksGroup group, bool changeActiveGroupIndex = true)
        {
            Array.Resize(ref groups, groups.Length + 1);

            int newGroupIndex = groups.Length - 1;
            groups[newGroupIndex] = group;

            if (changeActiveGroupIndex)
            {
                ActiveGroupIndex = newGroupIndex;
            }

            Dirty = true;
        }

        private static string GenerateDefaultGroupName(int groupIndex)
        {
            if (groupIndex == 0) return "General";
            return (groupIndex + 1).ToString();
        }

        public bool TryToGetActiveGroup(out BookmarksGroup activeGroup)
        {
            if (Groups == null || Groups.Length == 0)
            {
                activeGroup = default;
                return false;
            }

            int activeGroupIndex = ActiveGroupIndex;
            if (activeGroupIndex < 0 | activeGroupIndex > Groups.Length - 1)
            {
                activeGroup = default;
                return false;
            }

            activeGroup = Groups[activeGroupIndex];
            return true;
        }

        public void RemoveGroup(BookmarksGroup groupToRemove)
        {
            if (groups == null)
            {
                return;
            }

            int groupToRemoveIndex = Array.IndexOf(groups, groupToRemove);
            RemoveGroupAt(groupToRemoveIndex);
        }

        public void RemoveGroupAt(int groupToRemoveIndex)
        {
            if (groups == null)
            {
                return;
            }

            if (groupToRemoveIndex < 0 || groupToRemoveIndex > groups.Length - 1)
            {
                return;
            }

            groups = groups.RemoveAt(groupToRemoveIndex);

            Dirty = true;
        }

        public void RemoveAll()
        {
            groups = null;
            Init();

            Dirty = true;
        }

        public void UpdateCustomSortIndices()
        {
            for (int i = 0, iSize = groups.Length; i < iSize; i++)
            {
                groups[i].UpdateCustomSortIndices();
            }
            Dirty = true;
        }
    }
}