using DFun.UnityDataTypes;
using UnityEditor;
using UnityEngine;

namespace DFun.Invoker
{
    public class MenuCommandDataTypeDrawer : BaseDataTypeDrawer
    {
        public override float ElementHeight => base.ElementHeight * 4f;
        private const string UserDataLabel = nameof(MenuCommand.userData);
        private const string ContextLabel = nameof(MenuCommand.context);

        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(MenuCommandDataType.Instance);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            MenuCommand value = (MenuCommand)Serializer.Deserialize(stringValue);

            EditorGUI.LabelField(GetFieldRect(rect, 0), label);

            return new MenuCommand(
                DrawContextField(rect, value.context),
                // value.context,
                DrawUserDataField(rect, value.userData)
            );
        }

        private static Rect GetFieldRect(Rect parentRect, int fieldIndex)
        {
            float fieldHeight = EditorGUIUtility.singleLineHeight;
            return new Rect(parentRect.x, parentRect.y + fieldIndex * fieldHeight, parentRect.width, fieldHeight);
        }

        private Object DrawContextField(Rect parentRect, Object menuCommandContext)
        {
            Rect fieldRect = GetFieldRect(parentRect, 1);
            return EditorGUI.ObjectField(
                fieldRect,
                UserDataLabel,
                menuCommandContext,
                typeof(Object),
                true
            );
        }

        private int DrawUserDataField(Rect parentRect, int userData)
        {
            Rect fieldRect = GetFieldRect(parentRect, 2);
            return EditorGUI.IntField(
                fieldRect,
                ContextLabel,
                userData
            );
        }
    }
}