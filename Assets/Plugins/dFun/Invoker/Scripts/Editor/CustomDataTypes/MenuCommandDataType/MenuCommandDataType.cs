using DFun.UnityDataTypes;
using UnityEditor;

namespace DFun.Invoker
{
    public class MenuCommandDataType : ICustomDataType
    {
        private static DataType _instance;
        public static DataType Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataType(typeof(MenuCommand), "Menu Command");
                }
                return _instance;
            }
        }

        private static bool _initialized;

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            NewDataTypeHelper.AddNewDataType(
                Instance,
                new MenuCommandDataTypeSerializer(),
                new MenuCommandDataTypeDrawer()
            );
        }
    }
}