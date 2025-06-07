using DFun.UnityDataTypes;
using DFun.UnityDataTypes.Tests;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace DFun.Invoker.Tests
{
    public class CustomDataTypesSerializationTest
    {
        [Test]
        public void TestDefaultCustomDataTypesSerialization()
        {
            CustomDataTypeHelper.Initialize();

            DataType[] customDataTypes = CustomEditorDataTypes.TypesList;
            DataTypesSerializationTest.TestDefaultDataTypesSerialization(customDataTypes);
        }

        [Test]
        public void TestDataType_MenuCommand()
        {
            CustomDataTypeHelper.Initialize();

            MenuCommand menuCommand = new MenuCommand(new Object(), 42);
            DataTypesSerializationTest.TestDataTypeBase(menuCommand, MenuCommandComparator);
        }

        [Test]
        public void TestDataType_MenuCommandNull()
        {
            CustomDataTypeHelper.Initialize();

            MenuCommand nullMenuCommand = new MenuCommand(null);
            DataTypesSerializationTest.TestDataTypeBase(nullMenuCommand, MenuCommandComparator);
        }

        private bool MenuCommandComparator(MenuCommand mc1, MenuCommand mc2)
        {
            return mc1.context == mc2.context
                   && mc1.userData == mc2.userData;
        }
    }
}