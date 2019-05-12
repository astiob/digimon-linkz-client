using System;
using System.Linq;
using System.Reflection;

namespace Facebook.Unity.Mobile.Android
{
	internal class AndroidWrapper : IAndroidWrapper
	{
		private const string FacebookJavaClassName = "com.facebook.unity.FB";

		private const string UnityEngineAssemblyName = "UnityEngine";

		private const string AndroidJavaClassName = "UnityEngine.AndroidJavaClass";

		private const string CallStaticMethodInfoName = "CallStatic";

		private static Type androidJavaClassType;

		private static object androidJavaClassObject;

		private static MethodInfo callStaticMethodInfo;

		private static MethodInfo callStaticMethodInfoGeneric;

		private static Type AndroidJavaClassType
		{
			get
			{
				if (AndroidWrapper.androidJavaClassType != null)
				{
					return AndroidWrapper.androidJavaClassType;
				}
				Assembly assembly = Assembly.Load("UnityEngine");
				AndroidWrapper.androidJavaClassType = assembly.GetType("UnityEngine.AndroidJavaClass");
				if (AndroidWrapper.androidJavaClassType == null)
				{
					throw new InvalidOperationException("Failed to load type: UnityEngine.AndroidJavaClass");
				}
				return AndroidWrapper.androidJavaClassType;
			}
		}

		private static object AndroidJavaClassObject
		{
			get
			{
				if (AndroidWrapper.androidJavaClassObject != null)
				{
					return AndroidWrapper.androidJavaClassObject;
				}
				AndroidWrapper.androidJavaClassObject = Activator.CreateInstance(AndroidWrapper.AndroidJavaClassType, new object[]
				{
					"com.facebook.unity.FB"
				});
				if (AndroidWrapper.androidJavaClassObject == null)
				{
					throw new InvalidOperationException("Failed to institate object of type: " + AndroidWrapper.AndroidJavaClassType.FullName);
				}
				return AndroidWrapper.androidJavaClassObject;
			}
		}

		private static MethodInfo CallStaticMethodInfo
		{
			get
			{
				if (AndroidWrapper.callStaticMethodInfo != null)
				{
					return AndroidWrapper.callStaticMethodInfo;
				}
				AndroidWrapper.callStaticMethodInfo = AndroidWrapper.AndroidJavaClassType.GetMethod("CallStatic", new Type[]
				{
					typeof(string),
					typeof(object[])
				});
				if (AndroidWrapper.callStaticMethodInfo == null)
				{
					throw new InvalidOperationException("Failed to locate method: CallStatic");
				}
				return AndroidWrapper.callStaticMethodInfo;
			}
		}

		private static MethodInfo CallStaticMethodInfoGeneric
		{
			get
			{
				if (AndroidWrapper.callStaticMethodInfoGeneric != null)
				{
					return AndroidWrapper.callStaticMethodInfoGeneric;
				}
				MethodInfo[] methods = AndroidWrapper.AndroidJavaClassType.GetMethods();
				foreach (MethodInfo methodInfo in methods)
				{
					if (!(methodInfo.Name != "CallStatic"))
					{
						if (methodInfo.GetGenericArguments().Count<Type>() == 1)
						{
							ParameterInfo[] parameters = methodInfo.GetParameters();
							if (parameters.Count<ParameterInfo>() == 2)
							{
								if (parameters[0].ParameterType.IsAssignableFrom(typeof(string)))
								{
									if (parameters[1].ParameterType.IsAssignableFrom(typeof(object[])))
									{
										AndroidWrapper.callStaticMethodInfoGeneric = methodInfo;
										break;
									}
								}
							}
						}
					}
				}
				if (AndroidWrapper.callStaticMethodInfoGeneric == null)
				{
					throw new InvalidOperationException("Failed to locate generic method: CallStatic");
				}
				return AndroidWrapper.callStaticMethodInfoGeneric;
			}
		}

		public void CallStatic(string methodName, params object[] args)
		{
			AndroidWrapper.CallStaticMethodInfo.Invoke(AndroidWrapper.AndroidJavaClassObject, new object[]
			{
				methodName,
				args
			});
		}

		public T CallStatic<T>(string methodName)
		{
			MethodInfo methodInfo = AndroidWrapper.CallStaticMethodInfoGeneric.MakeGenericMethod(new Type[]
			{
				typeof(T)
			});
			if (methodInfo == null)
			{
				throw new InvalidOperationException("Failed to make generic method for calling static");
			}
			return (T)((object)methodInfo.Invoke(AndroidWrapper.AndroidJavaClassObject, new object[]
			{
				methodName,
				new object[0]
			}));
		}
	}
}
