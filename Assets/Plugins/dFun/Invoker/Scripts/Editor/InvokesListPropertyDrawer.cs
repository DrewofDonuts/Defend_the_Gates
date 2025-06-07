using DFun.UnityDataTypes;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DFun.Invoker
{
    [CustomPropertyDrawer(typeof(InvokesList))]
    public class InvokesListPropertyDrawer : PropertyDrawer
    {
        private const bool Draggable = true;
        private const bool DisplayAddButton = true;
        private const bool DisplayRemoveButton = true;

        private const string InvokesFieldName = "invokes";

        private ReorderableList _reorderableList;
        private SerializedProperty _serializableInvokesDataArray;

        private readonly bool _displayHeader;

        private string _customHeader;
        public string CustomHeader
        {
            get => _customHeader;
            set => _customHeader = value;
        }

        public InvokesListPropertyDrawer() : this(true)
        {
        }

        public InvokesListPropertyDrawer(bool displayHeader)
        {
            _displayHeader = displayHeader;
        }

        private void Init(SerializedProperty property)
        {
            CustomDataTypeHelper.Initialize();

            if (_serializableInvokesDataArray == null)
            {
                _serializableInvokesDataArray = property.FindPropertyRelative(InvokesFieldName);
            }

            if (_reorderableList == null)
            {
                _reorderableList = new ReorderableList(
                    property.serializedObject, _serializableInvokesDataArray,
                    Draggable, _displayHeader, DisplayAddButton, DisplayRemoveButton
                );

                _reorderableList.drawElementCallback = DrawInvokeData;
                _reorderableList.drawHeaderCallback += rect =>
                {
                    DrawHeader(property, rect);
                };
                _reorderableList.elementHeightCallback += index =>
                {
                    if (_serializableInvokesDataArray == null || index > _serializableInvokesDataArray.arraySize - 1)
                    {
                        return 0f;
                    }
                    return EditorGUI.GetPropertyHeight(
                        _serializableInvokesDataArray.GetArrayElementAtIndex(index)
                    );
                };
                _reorderableList.onAddCallback += list =>
                {
                    int index = list.serializedProperty.arraySize;
                    list.serializedProperty.arraySize++;
                    list.index = index;

                    SerializedProperty sInvokeData = list.serializedProperty.GetArrayElementAtIndex(index);
                    InvokeDataPropertyDrawer.SetDefaultValues(sInvokeData);
                };
            }
        }

        private void DrawHeader(SerializedProperty property, Rect rect)
        {
            EditorGUI.LabelField(rect, string.IsNullOrEmpty(CustomHeader) ? property.displayName : CustomHeader);
        }

        private void DrawInvokeData(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty serializableInvokeData = _serializableInvokesDataArray.GetArrayElementAtIndex(index);
            rect.height -= 4;
            rect.y += 2;
            EditorGUI.PropertyField(rect, serializableInvokeData, GUIContent.none);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init(property);
            _reorderableList.DoList(position);
        }

        public void DrawInLayout(SerializedProperty property)
        {
            Init(property);
            _reorderableList.DoLayoutList();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);
            return _reorderableList.GetHeight();
        }
    }
}