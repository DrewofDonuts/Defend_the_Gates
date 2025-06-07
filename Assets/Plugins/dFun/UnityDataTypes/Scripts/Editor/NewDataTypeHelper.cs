namespace DFun.UnityDataTypes
{
    public static class NewDataTypeHelper
    {
        public static bool IsDataTypeFullySupported(DataType dataType)
        {
            if (SupportedDataTypes.GetDataType(dataType.Type) == SupportedDataTypes.Unknown)
            {
                return false;
            }

            if (DataTypeSerialization.GetDataTypeSerializer(dataType) == default)
            {
                return false;
            }

            if (DataTypeDrawers.GetDataTypeDrawer(dataType) == default)
            {
                return false;
            }

            return true;
        }

        /// <returns>true if the newDataType was added successfully; otherwise, false.</returns>
        public static bool AddNewDataType(
            DataType newDataType, IDataTypeSerializer serializer, IDataTypeDrawer drawer)
        {
            bool dataTypeAdded = SupportedDataTypes.AddDataType(newDataType);
            bool serializerAdded = DataTypeSerialization.AddSerializer(newDataType, serializer);
            bool drawerAdded = DataTypeDrawers.AddDataTypeDrawer(newDataType, drawer);
            return dataTypeAdded
                   && serializerAdded
                   && drawerAdded;
        }
    }
}