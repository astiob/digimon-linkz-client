using System;
using System.Reflection;

namespace UnityEngine
{
	/// <summary>
	///   <para>This class can be used to implement any java interface. Any java vm method invocation matching the interface on the proxy object will automatically be passed to the c# implementation.</para>
	/// </summary>
	public class AndroidJavaProxy
	{
		/// <summary>
		///   <para>Java interface implemented by the proxy.</para>
		/// </summary>
		public readonly AndroidJavaClass javaInterface;

		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="javaInterface">Java interface to be implemented by the proxy.</param>
		public AndroidJavaProxy(string javaInterface) : this(new AndroidJavaClass(javaInterface))
		{
		}

		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="javaInterface">Java interface to be implemented by the proxy.</param>
		public AndroidJavaProxy(AndroidJavaClass javaInterface)
		{
			this.javaInterface = javaInterface;
		}

		/// <summary>
		///   <para>Called by the java vm whenever a method is invoked on the java proxy interface. You can override this to run special code on method invokation, or you can leave the implementation as is, and leave the default behavior which is to look for c# methods matching the signature of the java method.</para>
		/// </summary>
		/// <param name="methodName">Name of the invoked java method.</param>
		/// <param name="args">Arguments passed from the java vm - converted into AndroidJavaObject, AndroidJavaClass or a primitive.</param>
		/// <param name="javaArgs">Arguments passed from the java vm - all objects are represented by AndroidJavaObject, int for instance is represented by a java.lang.Integer object.</param>
		public virtual AndroidJavaObject Invoke(string methodName, object[] args)
		{
			Exception ex = null;
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			Type[] array = new Type[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				array[i] = ((args[i] != null) ? args[i].GetType() : typeof(AndroidJavaObject));
			}
			try
			{
				MethodInfo method = base.GetType().GetMethod(methodName, bindingAttr, null, array, null);
				if (method != null)
				{
					return _AndroidJNIHelper.Box(method.Invoke(this, args));
				}
			}
			catch (TargetInvocationException ex2)
			{
				ex = ex2.InnerException;
			}
			catch (Exception ex3)
			{
				ex = ex3;
			}
			string[] array2 = new string[args.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array2[j] = array[j].ToString();
			}
			if (ex != null)
			{
				throw new TargetInvocationException(string.Concat(new object[]
				{
					base.GetType(),
					".",
					methodName,
					"(",
					string.Join(",", array2),
					")"
				}), ex);
			}
			throw new Exception(string.Concat(new object[]
			{
				"No such proxy method: ",
				base.GetType(),
				".",
				methodName,
				"(",
				string.Join(",", array2),
				")"
			}));
		}

		/// <summary>
		///   <para>Called by the java vm whenever a method is invoked on the java proxy interface. You can override this to run special code on method invokation, or you can leave the implementation as is, and leave the default behavior which is to look for c# methods matching the signature of the java method.</para>
		/// </summary>
		/// <param name="methodName">Name of the invoked java method.</param>
		/// <param name="args">Arguments passed from the java vm - converted into AndroidJavaObject, AndroidJavaClass or a primitive.</param>
		/// <param name="javaArgs">Arguments passed from the java vm - all objects are represented by AndroidJavaObject, int for instance is represented by a java.lang.Integer object.</param>
		public virtual AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
		{
			object[] array = new object[javaArgs.Length];
			for (int i = 0; i < javaArgs.Length; i++)
			{
				array[i] = _AndroidJNIHelper.Unbox(javaArgs[i]);
			}
			return this.Invoke(methodName, array);
		}
	}
}
