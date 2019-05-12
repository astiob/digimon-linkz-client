using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Facebook.Unity
{
	internal class CodelessIAPAutoLog
	{
		internal static void handlePurchaseCompleted(object data)
		{
			try
			{
				if (FB.Mobile.IsImplicitPurchaseLoggingEnabled())
				{
					object property = CodelessIAPAutoLog.GetProperty(data, "metadata");
					object property2 = CodelessIAPAutoLog.GetProperty(data, "definition");
					if (property != null && property2 != null)
					{
						decimal value = (decimal)CodelessIAPAutoLog.GetProperty(property, "localizedPrice");
						string value2 = (string)CodelessIAPAutoLog.GetProperty(property, "isoCurrencyCode");
						string value3 = (string)CodelessIAPAutoLog.GetProperty(property2, "id");
						FB.LogAppEvent("fb_mobile_purchase", new float?((float)value), new Dictionary<string, object>
						{
							{
								"_implicitlyLogged",
								"1"
							},
							{
								"fb_currency",
								value2
							},
							{
								"fb_content_id",
								value3
							}
						});
					}
				}
			}
			catch (Exception ex)
			{
				FacebookLogger.Log("Failed to automatically handle Purchase Completed: " + ex.Message);
			}
		}

		internal static void addListenerToIAPButtons(object listenerObject)
		{
			Object[] array = CodelessIAPAutoLog.FindObjectsOfTypeByName("IAPButton", "UnityEngine.Purchasing");
			if (array == null)
			{
				return;
			}
			Object[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				CodelessIAPAutoLog.addListenerToGameObject(array2[i], listenerObject);
			}
		}

		internal static void addListenerToGameObject(Object gameObject, object listenerObject)
		{
			Type type = CodelessIAPAutoLog.FindTypeInAssemblies("Product", "UnityEngine.Purchasing");
			if (type == null)
			{
				return;
			}
			Type typeFromHandle = typeof(UnityEvent<>);
			Type typeFromHandle2 = typeof(UnityAction<>);
			Type[] typeArguments = new Type[]
			{
				type
			};
			Type type2 = typeFromHandle.MakeGenericType(typeArguments);
			Type type3 = typeFromHandle2.MakeGenericType(typeArguments);
			UnityEventBase obj = (UnityEventBase)CodelessIAPAutoLog.GetField(gameObject, "onPurchaseComplete");
			MethodInfo method = type2.GetMethod("AddListener");
			MethodBase method2 = type2.GetMethod("RemoveListener");
			MethodInfo method3 = listenerObject.GetType().GetMethod("onPurchaseCompleteHandler", BindingFlags.Instance | BindingFlags.Public);
			method2.Invoke(obj, new object[]
			{
				Delegate.CreateDelegate(type3, listenerObject, method3)
			});
			method.Invoke(obj, new object[]
			{
				Delegate.CreateDelegate(type3, listenerObject, method3)
			});
		}

		private static Type FindTypeInAssemblies(string typeName, string nameSpace)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Type[] types = assemblies[i].GetTypes();
				for (int j = 0; j < types.Length; j++)
				{
					if (typeName == types[j].Name && nameSpace == types[j].Namespace)
					{
						return types[j];
					}
				}
			}
			return null;
		}

		private static Object[] FindObjectsOfTypeByName(string typeName, string nameSpace)
		{
			Type type = CodelessIAPAutoLog.FindTypeInAssemblies(typeName, nameSpace);
			if (type == null)
			{
				return null;
			}
			return Object.FindObjectsOfType(type);
		}

		private static object GetField(object inObj, string fieldName)
		{
			object result = null;
			FieldInfo field = inObj.GetType().GetField(fieldName);
			if (field != null)
			{
				result = field.GetValue(inObj);
			}
			return result;
		}

		private static object GetProperty(object inObj, string propertyName)
		{
			object result = null;
			PropertyInfo property = inObj.GetType().GetProperty(propertyName);
			if (property != null)
			{
				result = property.GetValue(inObj, null);
			}
			return result;
		}
	}
}
