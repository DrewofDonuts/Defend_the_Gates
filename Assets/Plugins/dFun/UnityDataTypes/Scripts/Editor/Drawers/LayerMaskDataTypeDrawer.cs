using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class LayerMaskDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.LayerMask);

        private string[] _layersNames;
        private string[] LayersNames
        {
            get
            {
                if (_layersNames == null) InitLayers();
                return _layersNames;
            }
        }

        private void InitLayers()
        {
            List<string> layers = new List<string>();

            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName))
                {
                    layers.Add(layerName);
                }
            }

            _layersNames = layers.ToArray();
        }

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return (LayerMask)EditorGUI.MaskField(
                rect,
                label,
                (LayerMask)Serializer.Deserialize(stringValue),
                LayersNames
            );
        }
    }
}