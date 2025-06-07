using System;
using System.Collections.Generic;
using System.Reflection;
using DFun.UnityDataTypes;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DFun.Invoker
{
    public static class InvokeDataHelper
    {
        public static void Invoke(
            InvokeData invokeData, Object objectInstance, bool catchExceptions = true, bool logExceptions = true)
        {
            if (!CanInvokeNow(invokeData))
            {
                return;
            }

            if (!invokeData.HasAssignedMethod)
            {
                //ignore empty invokes
                return;
            }

            CustomDataTypeHelper.Initialize();

            try
            {
                InvokeUnsafe(invokeData, objectInstance);
            }
            catch (Exception e)
            {
                if (!catchExceptions)
                {
                    throw;
                }

                if (logExceptions)
                {
                    Debug.LogException(e);
                }
            }
        }

        public static void Invoke(
            InvokeData invokeData, Type type, bool catchExceptions = true, bool logExceptions = true)
        {
            if (!CanInvokeNow(invokeData))
            {
                return;
            }

            if (!invokeData.HasAssignedMethod)
            {
                //ignore empty invokes
                return;
            }

            CustomDataTypeHelper.Initialize();

            try
            {
                InvokeUnsafe(invokeData, type);
            }
            catch (Exception e)
            {
                if (!catchExceptions)
                {
                    throw;
                }

                if (logExceptions)
                {
                    Debug.LogException(e);
                }
            }
        }

        private static bool CanInvokeNow(InvokeData invokeData)
        {
            CallStates callState = invokeData.CallState;
            bool isPlaying = Application.isPlaying;

            if (!isPlaying && callState.IsCallStateSelected(CallStates.EditorMode))
            {
                return true;
            }

            if (isPlaying && callState.IsCallStateSelected(CallStates.PlayMode))
            {
                return true;
            }

            return false;
        }

        private static void InvokeUnsafe(InvokeData invokeData, Object objectInstance)
        {
            if (!FindTargetMethod(invokeData, objectInstance, out Object targetComponent, out MethodInfo targetMethod))
            {
                FireMethodNotFoundException(invokeData);
            }

            object invokeResult = targetMethod.Invoke(targetComponent, invokeData.GetInvokeParameters());
            HandleInvokeResult(invokeResult, targetMethod);
        }

        private static void InvokeUnsafe(InvokeData invokeData, Type type)
        {
            if (!FindTargetMethod(invokeData, type, out MethodInfo targetMethod))
            {
                FireMethodNotFoundException(invokeData);
            }

            object invokeResult = targetMethod.Invoke(null, invokeData.GetInvokeParameters());
            HandleInvokeResult(invokeResult, targetMethod);
        }

        private static void HandleInvokeResult(object invokeResult, MethodInfo invokeMethod)
        {
            if (invokeMethod == null) return;

            if (invokeMethod.ReturnType != typeof(void))
            {
                Debug.Log(invokeResult);
            }
        }

        private static void FireMethodNotFoundException(InvokeData invokeData)
        {
            throw new Exception(
                $"Method '{invokeData.MethodName}' not found for type '{invokeData.ComponentAssemblyQualifiedName}'"
            );
        }

        private static bool FindTargetMethod(InvokeData invokeData, Object invokeObject,
            out Object targetComponent, out MethodInfo targetMethod)
        {
            if (TryToGetMonoScriptClass(invokeObject, out Type targetType))
            {
                // static method, so no need object for reflection invokes
                targetComponent = default;
            }
            else
            {
                if (!FindTargetComponent(invokeData, invokeObject, out targetComponent))
                {
                    targetMethod = default;
                    return false;
                }

                targetType = targetComponent.GetType();
            }

            return FindTargetMethod(invokeData, targetType, out targetMethod);
        }

        private static bool FindTargetMethod(
            InvokeData invokeData, Type type, out MethodInfo targetMethod)
        {
            targetMethod = UnityEventBase.GetValidMethodInfo(
                type, invokeData.MethodName, invokeData.GetArgumentsTypes()
            );

            return targetMethod != null;
        }

        private static bool TryToGetMonoScriptClass(Object obj, out Type monoScriptClass)
        {
#if UNITY_EDITOR
            if (obj is MonoScript monoScript)
            {
                monoScriptClass = monoScript.GetClass();
                return true;
            }
#endif
            monoScriptClass = default;
            return false;
        }

        private static bool FindTargetComponent(
            InvokeData invokeData, Object objectInstance, out Object targetComponent)
        {
            List<Object> componentsCandidates = UnityObjectHelper.GetObjectComponents(objectInstance);
            int targetTypeComponentCounter = 0;

            for (int i = 0, iSize = componentsCandidates.Count; i < iSize; i++)
            {
                Object candidate = componentsCandidates[i];
                if (candidate.GetType().AssemblyQualifiedName != invokeData.ComponentAssemblyQualifiedName)
                {
                    continue;
                }

                if (targetTypeComponentCounter == invokeData.ComponentIndex)
                {
                    targetComponent = candidate;
                    return true;
                }
                else
                {
                    targetTypeComponentCounter++;
                }
            }

            targetComponent = default;
            return false;
        }
    }
}