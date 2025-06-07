using System;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class ConfirmEditDataView
    {
        public bool IsVisible { get; set; }

        private readonly Func<string> _descriptionProvider;
        private readonly Action _onConfirm;
        private readonly Action _onCancel;

        public ConfirmEditDataView(Func<string> descriptionProvider, Action onConfirm, Action onCancel)
        {
            _descriptionProvider = descriptionProvider;
            _onConfirm = onConfirm;
            _onCancel = onCancel;
        }

        public bool DrawIfVisible()
        {
            if (!IsVisible)
            {
                return false;
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(
                _descriptionProvider.Invoke(), Styles.ConfirmationPopupLabel, GUILayout.Height(100)
            );
            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("CANCEL"))
                {
                    _onCancel?.Invoke();
                }

                if (GUILayout.Button("CONFIRM"))
                {
                    _onConfirm?.Invoke();
                }
            }
            EditorGUILayout.EndHorizontal();

            return true;
        }
    }
}