using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class SelectScriptPopup : PopupWindowContent
    {
        private bool _initialized;
        private IReadOnlyList<Type> _allScriptsSorted;
        private List<Type> _filteredScripts;

        private const string CaptionLabel = "Select script";
        private const string SelectButtonLabel = "Select";

        private Vector2 _scrollPos;
        private string _searchFilter = string.Empty;

        private const string SearchFieldControlId = "BookmarkNameTextField";
        private bool _focusOnSearchField = true;

        private int _selectedScriptIndex = -1;
        private double _lastClickTime;

        private bool _isFiltering;
        private double Now => EditorApplication.timeSinceStartup;
        private const double FilterDelay = 0.1f;
        private double _nextFilterTime;

        public event Action<Type> onScriptSelected;

        public override void OnOpen()
        {
            _focusOnSearchField = true;
            _initialized = false;
            ResetSelectedScript();
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        public override void OnClose()
        {
            base.OnClose();
            EditorApplication.update -= OnUpdate;
        }

        private void TryToInitialize()
        {
            if (_initialized) return;
            _initialized = true;

            TypeCache.TypeCollection allTypesCollection = TypeCache.GetTypesDerivedFrom<object>();

            //optimization: filter and sort scripts and then add them to read only sorted list
            _filteredScripts = new List<Type>(allTypesCollection.Count / 2);
            for (int i = 0, iSize = allTypesCollection.Count; i < iSize; i++)
            {
                Type type = allTypesCollection[i];
                if (!type.IsClass) continue;
                if (!StaticClassChecker.IsStaticOrContainsStaticMember(type)) continue;

                _filteredScripts.Add(type);
            }
            _filteredScripts.Sort((t1, t2) => string.Compare(t1.Name, t2.Name, StringComparison.Ordinal));

            _allScriptsSorted = new List<Type>(_filteredScripts);

            FilterScripts();
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(
                SelectScriptPopupStyles.WindowWidth,
                SelectScriptPopupStyles.WindowHeight
            );
        }

        public override void OnGUI(Rect rect)
        {
            TryToInitialize();
            DrawCaption();
            DrawSearchField();
            if (_isFiltering)
            {
                GUILayout.Label(
                    Icons.DelayIcon, SelectScriptPopupStyles.WaitIconStyle, SelectScriptPopupStyles.WaitIconOpt
                );
            }
            else
            {
                EditorGUILayout.Space(SelectScriptPopupStyles.WaitStubLineHeight);
            }

            _scrollPos = EditorGUILayout.BeginScrollView(
                _scrollPos,
                GUILayout.Height(SelectScriptPopupStyles.ScrollViewHeight)
            );
            {
                DrawScriptsList();
            }
            EditorGUILayout.EndScrollView();

            HandleFocus();
            PopupHelper.HandlePopupKeysEvents(Close);
        }

        private void OnUpdate()
        {
            UpdateFilterTimer();
        }

        private void UpdateFilterTimer()
        {
            if (!_isFiltering) return;

            if (Now >= _nextFilterTime)
            {
                _isFiltering = false;
                FilterScripts();

                if (editorWindow != null)
                {
                    editorWindow.Repaint();
                }
            }
        }

        private void DrawCaption()
        {
            EditorGUILayout.LabelField(
                $"{CaptionLabel} ({_filteredScripts.Count} found)",
                SelectScriptPopupStyles.Caption
            );
        }

        private void DrawScriptsList()
        {
            for (int i = 0, iSize = _filteredScripts.Count; i < iSize; i++)
            {
                DrawScript(_filteredScripts[i], i);
            }
        }

        private void DrawScript(Type scriptType, int index)
        {
            EditorGUILayout.BeginHorizontal();
            {
                bool isSelected = index == _selectedScriptIndex;
                GUIStyle buttonStyle = isSelected
                    ? SelectScriptPopupStyles.ScriptButtonStyleSelected
                    : SelectScriptPopupStyles.ScriptButtonStyle;

                string tooltip = CreateTooltip(scriptType);
                GUIContent label = new GUIContent(
                    CreateLabel(scriptType),
                    tooltip
                );
                if (GUILayout.Button(label, buttonStyle, SelectScriptPopupStyles.LabelLayoutOpt))
                {
                    _selectedScriptIndex = index;
                    bool isDoubleClick = MouseHelper.WasDoubleClicked(ref _lastClickTime);
                    if (isDoubleClick)
                    {
                        DoSelect(scriptType);
                    }
                }

                GUILayout.Label(
                    Icons.HelpIconWithTooltip(tooltip), Styles.HelpButtonStyle,
                    SelectScriptPopupStyles.TooltipButtonOpt
                );

                if (GUILayout.Button(SelectButtonLabel, SelectScriptPopupStyles.SelectButtonLayoutOpt))
                {
                    _selectedScriptIndex = index;
                    DoSelect(scriptType);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DoSelect(Type selectedScript)
        {
            onScriptSelected?.Invoke(selectedScript);
            Close();
        }

        private static string CreateTooltip(Type scriptType)
        {
            return scriptType.AssemblyQualifiedName;
        }

        private string CreateLabel(Type scriptType)
        {
            if (scriptType.IsNested && scriptType.DeclaringType != null)
            {
                return $"{scriptType.Name} ({scriptType.DeclaringType.Name})";
            }
            return scriptType.Name;
        }

        private void DrawSearchField()
        {
            bool doSearch;

            GUILayout.BeginHorizontal();
            {
                GUI.SetNextControlName(SearchFieldControlId);
                string newSearchFilter;
                EditorGUI.BeginChangeCheck();
                {
                    newSearchFilter = GUILayout.TextField(
                        _searchFilter,
                        SelectScriptPopupStyles.SearchField
                    );
                }
                doSearch = EditorGUI.EndChangeCheck();

                if (doSearch)
                {
                    if (newSearchFilter != _searchFilter)
                    {
                        _searchFilter = newSearchFilter;
                    }
                    else
                    {
                        doSearch = false;
                    }
                }

                if (GUILayout.Button(string.Empty, SelectScriptPopupStyles.CleanSearchButton))
                {
                    if (!string.IsNullOrEmpty(_searchFilter))
                    {
                        _searchFilter = string.Empty;
                        doSearch = true;
                        GUI.FocusControl(null);
                    }
                }
            }
            GUILayout.EndHorizontal();

            if (doSearch && !_isFiltering)
            {
                _isFiltering = true;
                _nextFilterTime = Now + FilterDelay;
            }
        }

        private void FilterScripts()
        {
            string searchFilter = _searchFilter.ToLower();
            int countBefore = _filteredScripts.Count;
            _filteredScripts.Clear();
            if (string.IsNullOrEmpty(searchFilter))
            {
                _filteredScripts.AddRange(_allScriptsSorted);
                return;
            }

            for (int i = 0, iSize = _allScriptsSorted.Count; i < iSize; i++)
            {
                Type scriptType = _allScriptsSorted[i];
                if (scriptType.Name.ToLower().Contains(searchFilter))
                {
                    _filteredScripts.Add(scriptType);
                }
            }

            if (_filteredScripts.Count != countBefore)
            {
                ResetSelectedScript();
            }
        }

        private void ResetSelectedScript()
        {
            _selectedScriptIndex = -1;
        }

        private void HandleFocus()
        {
            if (_focusOnSearchField)
            {
                _focusOnSearchField = false;
                EditorGUI.FocusTextInControl(SearchFieldControlId);
            }
        }

        private void Close()
        {
            editorWindow.Close();
        }
    }
}