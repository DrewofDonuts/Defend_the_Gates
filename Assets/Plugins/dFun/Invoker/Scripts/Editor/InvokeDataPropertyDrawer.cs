using System;
using System.Collections.Generic;
using System.Reflection;
using DFun.GameObjectResolver;
using DFun.UnityDataTypes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DFun.Invoker
{
    [CustomPropertyDrawer(typeof(InvokeData))]
    public class InvokeDataPropertyDrawer : PropertyDrawer
    {
        private const string AllowedMethodsFieldName = "allowedMethods";
        private const string CallStateFieldName = "callState";
        private const string ComponentAssemblyQualifiedNameFieldName = "componentAssemblyQualifiedName";
        private const string MethodNameFieldName = "methodName";
        private const string ComponentIndexFieldName = "componentIndex";
        private const string ArgumentsDataFieldName = "argumentsData";

        private const string ArgumentDataTypeFieldName = "dataType";
        private const string ArgumentArgNameFieldName = "argName";
        private const string ArgumentDataSerializedValueFieldName = "serializedValue";

        private const string SelectedOptionDisplayText = "No method";
        private const string NoOptionsDisplayText = "No suitable methods";
        private const string MethodNotFoundOptionDisplayText = "Method may not exist: ";
        private List<MethodDrawData> _methodsDrawData;

        private static float SingleLineHeight => EditorGUIUtility.singleLineHeight * 1.2f;

        public override void OnGUI(Rect position, SerializedProperty mInvokeData, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, mInvokeData);
            {
                Draw(position, mInvokeData);
            }
            EditorGUI.EndProperty();
        }

        private static SerializedProperty GetComponentAssemblyQualifiedName(SerializedProperty sInvokeData)
        {
            return sInvokeData.FindPropertyRelative(ComponentAssemblyQualifiedNameFieldName);
        }

        private static SerializedProperty GetMethodName(SerializedProperty sInvokeData)
        {
            return sInvokeData.FindPropertyRelative(MethodNameFieldName);
        }

        private static SerializedProperty GetComponentIndex(SerializedProperty sInvokeData)
        {
            return sInvokeData.FindPropertyRelative(ComponentIndexFieldName);
        }

        private static SerializedProperty GetArgumentsDataArray(SerializedProperty sInvokeData)
        {
            return sInvokeData.FindPropertyRelative(ArgumentsDataFieldName);
        }

        private static int GetArgsAmount(SerializedProperty sInvokeData)
        {
            return GetArgumentsDataArray(sInvokeData).arraySize;
        }

        private void Draw(Rect position, SerializedProperty sInvokeData)
        {
            float yOffset = 0f;
            Rect methodsTypesRect = new Rect(
                position.x, position.y + yOffset,
                position.width, SingleLineHeight
            );

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(methodsTypesRect, GetAllowedMethodsTypesProperty(sInvokeData));
            if (EditorGUI.EndChangeCheck())
            {
                ResetMethodsDrawData();
            }
            yOffset += methodsTypesRect.height;

            Rect callStateRect = new Rect(
                position.x, position.y + yOffset,
                position.width, SingleLineHeight
            );

            EditorGUI.PropertyField(callStateRect, GetCallStateProperty(sInvokeData));
            yOffset += callStateRect.height;

            Rect dropDownButtonRect = new Rect(
                position.x, position.y + yOffset,
                position.width, SingleLineHeight
            );

            UpdateMethodsDrawData(sInvokeData);
            int selectedMethodIndex = -1;

            string selectedMethodText;
            if (_methodsDrawData.Count == 0)
            {
                selectedMethodText = NoOptionsDisplayText;
            }
            else
            {
                if (FindSelectedMethod(sInvokeData, out selectedMethodIndex))
                {
                    selectedMethodText = _methodsDrawData[selectedMethodIndex].DisplayName;
                }
                else
                {
                    SerializedProperty sMethodName = GetMethodName(sInvokeData);
                    string methodName = sMethodName.stringValue;
                    selectedMethodText = string.IsNullOrEmpty(methodName)
                        ? SelectedOptionDisplayText
                        : $"{MethodNotFoundOptionDisplayText}'{methodName}'";
                }
            }

            if (GUI.Button(dropDownButtonRect, selectedMethodText, EditorStyles.popup))
            {
                ShowSelectMethodPopup(sInvokeData, selectedMethodIndex);
            }
            yOffset += dropDownButtonRect.height;

            int indentCache = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;
            {
                SerializedProperty argumentsDataArray = GetArgumentsDataArray(sInvokeData);
                for (int i = 0, iSize = argumentsDataArray.arraySize; i < iSize; i++)
                {
                    SerializedProperty sArgumentData = argumentsDataArray.GetArrayElementAtIndex(i);
                    yOffset += DrawArgumentData(sArgumentData, position, yOffset);
                }
            }
            EditorGUI.indentLevel = indentCache;
        }

        private float DrawArgumentData(SerializedProperty sArgData, Rect basePosition, float yOffset)
        {
            string argName = GetArgName(sArgData);

            SerializedProperty sArgValue = sArgData.FindPropertyRelative(
                ArgumentDataSerializedValueFieldName
            );
            string argStringValue = sArgValue.stringValue;

            IDataTypeDrawer argDrawer = GetArgDrawer(sArgData);

            float elementHeight = argDrawer.ElementHeight;
            Rect agrFieldRect = new Rect(
                basePosition.x,
                basePosition.y + yOffset,
                basePosition.width,
                elementHeight
            );

            EditorGUI.BeginChangeCheck();
            object newNonSerializedValue = argDrawer.Draw(agrFieldRect, argName, argStringValue);
            if (EditorGUI.EndChangeCheck())
            {
                sArgValue.stringValue = argDrawer.Serializer.Serialize(newNonSerializedValue);
            }

            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.ContextClick && agrFieldRect.Contains(currentEvent.mousePosition))
            {
                ContextMenuData contextMenuData = new ContextMenuData();
                contextMenuData.NonSerializedValue = newNonSerializedValue;
                contextMenuData.OnValueChange = changedNonSerializedValue =>
                {
                    sArgValue.stringValue = argDrawer.Serializer.Serialize(changedNonSerializedValue);
                    SerializedObject serializedObject = sArgValue.serializedObject;
                    if (serializedObject != null)
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                };

                argDrawer.ShowCustomContextMenu(contextMenuData);
                if (contextMenuData.WasShown)
                {
                    currentEvent.Use();
                }
            }

            return elementHeight;
        }

        private bool FindSelectedMethod(SerializedProperty sInvokeData, out int selectedMethodIndex)
        {
            SerializedProperty sComponentAssemblyQualifiedName = GetComponentAssemblyQualifiedName(sInvokeData);
            string targetComponentAssemblyQualifiedName = sComponentAssemblyQualifiedName.stringValue;
            if (string.IsNullOrEmpty(targetComponentAssemblyQualifiedName))
            {
                selectedMethodIndex = -1;
                return false;
            }

            SerializedProperty sMethodName = GetMethodName(sInvokeData);
            string targetMethodName = sMethodName.stringValue;

            if (string.IsNullOrEmpty(targetMethodName))
            {
                selectedMethodIndex = -1;
                return false;
            }

            SerializedProperty sComponentIndex = GetComponentIndex(sInvokeData);
            int targetComponentIndex = sComponentIndex.intValue;

            Type[] argumentsTypes = GetArgumentsTypes(sInvokeData);

            for (int i = 0, iSize = _methodsDrawData.Count; i < iSize; i++)
            {
                MethodDrawData methodDrawData = _methodsDrawData[i];
                if (methodDrawData.InvokeObjectAssemblyQualifiedName != targetComponentAssemblyQualifiedName)
                {
                    continue;
                }

                if (methodDrawData.ComponentIndex != targetComponentIndex)
                {
                    continue;
                }

                if (methodDrawData.MethodName != targetMethodName)
                {
                    continue;
                }

                if (AreTheSameArgs(methodDrawData.MethodInfo, argumentsTypes))
                {
                    selectedMethodIndex = i;
                    return true;
                }
            }

            selectedMethodIndex = -1;
            return false;
        }

        private bool AreTheSameArgs(MethodInfo methodInfo, Type[] targetArgumentsTypes)
        {
            ParameterInfo[] methodArgs = methodInfo.GetParameters();
            if (methodArgs.Length != targetArgumentsTypes.Length) return false;

            for (int i = 0, iSize = methodArgs.Length; i < iSize; i++)
            {
                if (methodArgs[i].ParameterType != targetArgumentsTypes[i]) return false;
            }

            return true;
        }

        private Type[] GetArgumentsTypes(SerializedProperty sInvokeData)
        {
            SerializedProperty sArgumentsArray = GetArgumentsDataArray(sInvokeData);
            Type[] argumentsTypes = new Type[sArgumentsArray.arraySize];
            for (int i = 0, iSize = argumentsTypes.Length; i < iSize; i++)
            {
                SerializedProperty sArgData = sArgumentsArray.GetArrayElementAtIndex(i);
                SerializedProperty sArgType = sArgData.FindPropertyRelative(ArgumentDataTypeFieldName);
                argumentsTypes[i] = SupportedDataTypes.GetDataType(sArgType.stringValue).Type;
            }
            return argumentsTypes;
        }

        private void ShowSelectMethodPopup(SerializedProperty sInvokeData, int selectedIndex)
        {
            List<MethodDrawData> methods = _methodsDrawData;
            GenericMenu menu = new GenericMenu();
            for (int i = 0, iSize = methods.Count; i < iSize; i++)
            {
                MethodDrawData methodDrawData = methods[i];
                menu.AddItem(new GUIContent(methodDrawData.DropdownOption), selectedIndex == i, () =>
                {
                    UpdateSelectedMethod(sInvokeData, methodDrawData);
                });
            }
            menu.ShowAsContext();
        }

        private void UpdateSelectedMethod(SerializedProperty sInvokeData, MethodDrawData selectedMethod)
        {
            MethodInfo selectedMethodInfo = selectedMethod.MethodInfo;
            GetComponentAssemblyQualifiedName(sInvokeData).stringValue =
                selectedMethod.InvokeObjectAssemblyQualifiedName;
            GetMethodName(sInvokeData).stringValue = selectedMethodInfo.Name;
            GetComponentIndex(sInvokeData).intValue = selectedMethod.ComponentIndex;

            UpdateSelectedMethodArgumentsData(sInvokeData, selectedMethodInfo);

            sInvokeData.serializedObject.ApplyModifiedProperties();
        }

        private void UpdateSelectedMethodArgumentsData(
            SerializedProperty sInvokeData, MethodInfo selectedMethodInfo)
        {
            SerializedProperty sArgsDataArray = GetArgumentsDataArray(sInvokeData);
            ParameterInfo[] selectedMethodArguments = selectedMethodInfo.GetParameters();
            sArgsDataArray.arraySize = selectedMethodArguments.Length;
            for (int i = 0, iSize = sArgsDataArray.arraySize; i < iSize; i++)
            {
                Type argumentType = selectedMethodArguments[i].ParameterType;
                SerializedProperty sArgData = sArgsDataArray.GetArrayElementAtIndex(i);

                //arg type
                SerializedProperty serializedArgType = sArgData.FindPropertyRelative(
                    ArgumentDataTypeFieldName
                );
                serializedArgType.stringValue = argumentType.AssemblyQualifiedName;

                //arg name
                SerializedProperty sArgName = sArgData.FindPropertyRelative(
                    ArgumentArgNameFieldName
                );
                sArgName.stringValue = selectedMethodArguments[i].Name;

                //arg value
                SerializedProperty sArgValue = sArgData.FindPropertyRelative(
                    ArgumentDataSerializedValueFieldName
                );

                string defaultSerializedValue;

                if (argumentType.IsEnum)
                {
                    EnumDataTypeSerializer enumDataTypeSerializer = (EnumDataTypeSerializer)
                        DataTypeSerialization.GetDataTypeSerializer(SupportedDataTypes.Enum);
                    defaultSerializedValue = enumDataTypeSerializer.GetDefaultSerializedValue(argumentType);
                }
                else
                {
                    defaultSerializedValue = DataTypeSerialization.GetDefaultSerializedValue(
                        argumentType
                    );
                }

                sArgValue.stringValue = defaultSerializedValue;
            }
        }

        private void ResetMethodsDrawData()
        {
            _methodsDrawData = null;
        }

        private void UpdateMethodsDrawData(SerializedProperty sInvokeData)
        {
            if (_methodsDrawData != null) return;
            _methodsDrawData = new List<MethodDrawData>();
            CustomDataTypeHelper.Initialize();

            Object targetObj = sInvokeData.serializedObject.context != null
                ? sInvokeData.serializedObject.context
                : sInvokeData.serializedObject.targetObject;

            MethodsTypes allowedMethodsTypes = GetAllowedMethodsTypes(sInvokeData);

            if (IsTypeReference(targetObj, out TypeReference typeReference))
            {
                UpdateMethodsDrawDataForScript(typeReference, allowedMethodsTypes);
                return;
            }

            if (IsAssetAScript(targetObj))
            {
                UpdateMethodsDrawDataForScript(targetObj, allowedMethodsTypes);
                return;
            }
            UpdateMethodsDrawDataForComponents(targetObj, allowedMethodsTypes);
        }

        private bool IsTypeReference(Object targetObj, out TypeReference typeReference)
        {
            if (targetObj == null)
            {
                typeReference = default;
                return false;
            }

            if (targetObj is TypeReferenceWrapper typeReferenceWrapper)
            {
                typeReference = typeReferenceWrapper.TypeReference;
                return true;
            }

            typeReference = default;
            return false;
        }

        private void UpdateMethodsDrawDataForScript(Object scriptObj, MethodsTypes allowMethodsTypes)
        {
            if (scriptObj == null) return;

            MonoScript monoScript = (MonoScript)scriptObj;
            if (monoScript == null) return;

            Type targetClassType = monoScript.GetClass();
            if (targetClassType == null) return;

            List<MethodInfo> validMethods = InvokeMethodsProvider.GetValidMethods(
                targetClassType, allowMethodsTypes, true
            );
            List<MethodInfo> orderedMethods = InvokeFormatter.SortByName(validMethods);

            AddMethodDrawData(monoScript, 0, orderedMethods);
        }

        private void UpdateMethodsDrawDataForComponents(Object targetObj, MethodsTypes allowMethodsTypes)
        {
            List<Object> components = UnityObjectHelper.GetObjectComponents(targetObj);
            Dictionary<Type, int> componentTypeToAppearances = new Dictionary<Type, int>(components.Count);

            for (int i = 0, iSize = components.Count; i < iSize; i++)
            {
                Object component = components[i];
                List<MethodInfo> validMethods = InvokeMethodsProvider.GetValidMethods(
                    component, allowMethodsTypes, false
                );
                List<MethodInfo> orderedMethods = InvokeFormatter.SortByName(validMethods);

                Type componentType = component.GetType();
                if (componentTypeToAppearances.ContainsKey(componentType))
                {
                    componentTypeToAppearances[componentType]++;
                }
                else
                {
                    componentTypeToAppearances[componentType] = 0;
                }
                int componentIndex = componentTypeToAppearances[componentType];

                AddMethodDrawData(component, componentIndex, orderedMethods);
            }
        }

        private void UpdateMethodsDrawDataForScript(TypeReference typeReference, MethodsTypes allowMethodsTypes)
        {
            if (!typeReference.ContainsData) return;
            if (!typeReference.TryToResolveType(out Type targetClassType)) return;

            List<MethodInfo> validMethods = InvokeMethodsProvider.GetValidMethods(
                targetClassType, allowMethodsTypes, true
            );
            List<MethodInfo> orderedMethods = InvokeFormatter.SortByName(validMethods);

            AddMethodDrawData(targetClassType, 0, orderedMethods);
        }

        private static bool IsAssetAScript(Object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == typeof(MonoScript);
        }

        private void AddMethodDrawData(
            Object obj, int componentIndex, List<MethodInfo> methods)
        {
            for (int i = 0, iSize = methods.Count; i < iSize; i++)
            {
                _methodsDrawData.Add(
                    new MethodDrawData(obj, componentIndex, methods[i])
                );
            }
        }

        private void AddMethodDrawData(
            Type type, int componentIndex, List<MethodInfo> methods)
        {
            for (int i = 0, iSize = methods.Count; i < iSize; i++)
            {
                _methodsDrawData.Add(
                    new MethodDrawData(type, componentIndex, methods[i])
                );
            }
        }

        private static string GetArgName(SerializedProperty sArgData)
        {
            SerializedProperty sArgName = sArgData.FindPropertyRelative(
                ArgumentArgNameFieldName
            );

            return sArgName.stringValue;
        }

        private static MethodsTypes GetAllowedMethodsTypes(SerializedProperty sArgData)
        {
            SerializedProperty sMethodsTypes = GetAllowedMethodsTypesProperty(sArgData);
            return (MethodsTypes)sMethodsTypes.intValue;
        }

        private static SerializedProperty GetAllowedMethodsTypesProperty(SerializedProperty sArgData)
        {
            return sArgData.FindPropertyRelative(
                AllowedMethodsFieldName
            );
        }

        private static SerializedProperty GetCallStateProperty(SerializedProperty sArgData)
        {
            return sArgData.FindPropertyRelative(
                CallStateFieldName
            );
        }

        private static IDataTypeDrawer GetArgDrawer(SerializedProperty sArgData)
        {
            SerializedProperty sArgType = sArgData.FindPropertyRelative(
                ArgumentDataTypeFieldName
            );

            string argTypeString = sArgType.stringValue;
            return DataTypeDrawers.GetDataTypeDrawer(argTypeString);
        }

        private static float GetArgPropertyHeight(SerializedProperty sArgData)
        {
            return GetArgDrawer(sArgData).ElementHeight;
        }

        public override float GetPropertyHeight(SerializedProperty sInvokeData, GUIContent label)
        {
            float propertyHeight = 3 * SingleLineHeight;

            SerializedProperty sArgsArray = GetArgumentsDataArray(sInvokeData);
            for (int i = 0, iSize = sArgsArray.arraySize; i < iSize; i++)
            {
                propertyHeight += GetArgPropertyHeight(
                    sArgsArray.GetArrayElementAtIndex(i)
                );
            }

            return propertyHeight;
        }

        public static void SetDefaultValues(SerializedProperty sInvokeData)
        {
            InvokeData defaultData = InvokeData.Default;
            GetAllowedMethodsTypesProperty(sInvokeData).intValue = (int)defaultData.AllowedMethods;
            GetCallStateProperty(sInvokeData).intValue = (int)defaultData.CallState;
            GetComponentAssemblyQualifiedName(sInvokeData).stringValue = defaultData.ComponentAssemblyQualifiedName;
            GetComponentIndex(sInvokeData).intValue = defaultData.ComponentIndex;
            GetMethodName(sInvokeData).stringValue = defaultData.MethodName;
            GetArgumentsDataArray(sInvokeData).arraySize = 0;
        }
    }
}