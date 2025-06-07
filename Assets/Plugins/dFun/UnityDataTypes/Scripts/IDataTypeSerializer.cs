namespace DFun.UnityDataTypes
{
    public interface IDataTypeSerializer
    {
        string DefaultSerializedValue { get; }
        string Serialize(object value);
        object Deserialize(string stringValue);
    }
}