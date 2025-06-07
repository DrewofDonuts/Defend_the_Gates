using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Matrix4X4DataTypeDrawer : BaseDataTypeDrawer
    {
        public override float ElementHeight => EditorGUIUtility.singleLineHeight * 6f;

        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Matrix4X4);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            Matrix4x4 value = (Matrix4x4)Serializer.Deserialize(stringValue);

            Matrix4x4 newValue = new Matrix4x4();

            const int matrixSize = 4;
            float fieldWidth = rect.width / matrixSize;
            float fieldHeight = EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, fieldHeight), label);

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    Rect fieldRect = new Rect(
                        rect.x + j * fieldWidth, rect.y + i * fieldHeight + fieldHeight, fieldWidth, fieldHeight
                    );
                    newValue[i, j] = EditorGUI.FloatField(fieldRect, value[i, j]);
                }
            }

            return newValue;
        }
    }
}