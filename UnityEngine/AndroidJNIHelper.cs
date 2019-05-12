using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Helper interface for JNI interaction; signature creation and method lookups.</para>
	/// </summary>
	public sealed class AndroidJNIHelper
	{
		private AndroidJNIHelper()
		{
		}

		/// <summary>
		///   <para>Set debug to true to log calls through the AndroidJNIHelper.</para>
		/// </summary>
		public static extern bool debug { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Scans a particular Java class for a constructor method matching a signature.</para>
		/// </summary>
		/// <param name="javaClass">Raw JNI Java class object (obtained by calling AndroidJNI.FindClass).</param>
		/// <param name="signature">Constructor method signature (e.g. obtained by calling AndroidJNIHelper.GetSignature).</param>
		[ExcludeFromDocs]
		public static IntPtr GetConstructorID(IntPtr javaClass)
		{
			string empty = string.Empty;
			return AndroidJNIHelper.GetConstructorID(javaClass, empty);
		}

		/// <summary>
		///   <para>Scans a particular Java class for a constructor method matching a signature.</para>
		/// </summary>
		/// <param name="javaClass">Raw JNI Java class object (obtained by calling AndroidJNI.FindClass).</param>
		/// <param name="signature">Constructor method signature (e.g. obtained by calling AndroidJNIHelper.GetSignature).</param>
		public static IntPtr GetConstructorID(IntPtr javaClass, [DefaultValue("\"\"")] string signature)
		{
			return _AndroidJNIHelper.GetConstructorID(javaClass, signature);
		}

		/// <summary>
		///   <para>Scans a particular Java class for a method matching a name and a signature.</para>
		/// </summary>
		/// <param name="javaClass">Raw JNI Java class object (obtained by calling AndroidJNI.FindClass).</param>
		/// <param name="methodName">Name of the method as declared in Java.</param>
		/// <param name="signature">Method signature (e.g. obtained by calling AndroidJNIHelper.GetSignature).</param>
		/// <param name="isStatic">Set to &lt;tt&gt;true&lt;tt&gt; for static methods; &lt;tt&gt;false&lt;tt&gt; for instance (nonstatic) methods.</param>
		[ExcludeFromDocs]
		public static IntPtr GetMethodID(IntPtr javaClass, string methodName, string signature)
		{
			bool isStatic = false;
			return AndroidJNIHelper.GetMethodID(javaClass, methodName, signature, isStatic);
		}

		/// <summary>
		///   <para>Scans a particular Java class for a method matching a name and a signature.</para>
		/// </summary>
		/// <param name="javaClass">Raw JNI Java class object (obtained by calling AndroidJNI.FindClass).</param>
		/// <param name="methodName">Name of the method as declared in Java.</param>
		/// <param name="signature">Method signature (e.g. obtained by calling AndroidJNIHelper.GetSignature).</param>
		/// <param name="isStatic">Set to &lt;tt&gt;true&lt;tt&gt; for static methods; &lt;tt&gt;false&lt;tt&gt; for instance (nonstatic) methods.</param>
		[ExcludeFromDocs]
		public static IntPtr GetMethodID(IntPtr javaClass, string methodName)
		{
			bool isStatic = false;
			string empty = string.Empty;
			return AndroidJNIHelper.GetMethodID(javaClass, methodName, empty, isStatic);
		}

		/// <summary>
		///   <para>Scans a particular Java class for a method matching a name and a signature.</para>
		/// </summary>
		/// <param name="javaClass">Raw JNI Java class object (obtained by calling AndroidJNI.FindClass).</param>
		/// <param name="methodName">Name of the method as declared in Java.</param>
		/// <param name="signature">Method signature (e.g. obtained by calling AndroidJNIHelper.GetSignature).</param>
		/// <param name="isStatic">Set to &lt;tt&gt;true&lt;tt&gt; for static methods; &lt;tt&gt;false&lt;tt&gt; for instance (nonstatic) methods.</param>
		public static IntPtr GetMethodID(IntPtr javaClass, string methodName, [DefaultValue("\"\"")] string signature, [DefaultValue("false")] bool isStatic)
		{
			return _AndroidJNIHelper.GetMethodID(javaClass, methodName, signature, isStatic);
		}

		/// <summary>
		///   <para>Scans a particular Java class for a field matching a name and a signature.</para>
		/// </summary>
		/// <param name="javaClass">Raw JNI Java class object (obtained by calling AndroidJNI.FindClass).</param>
		/// <param name="fieldName">Name of the field as declared in Java.</param>
		/// <param name="signature">Field signature (e.g. obtained by calling AndroidJNIHelper.GetSignature).</param>
		/// <param name="isStatic">Set to &lt;tt&gt;true&lt;tt&gt; for static fields; &lt;tt&gt;false&lt;tt&gt; for instance (nonstatic) fields.</param>
		[ExcludeFromDocs]
		public static IntPtr GetFieldID(IntPtr javaClass, string fieldName, string signature)
		{
			bool isStatic = false;
			return AndroidJNIHelper.GetFieldID(javaClass, fieldName, signature, isStatic);
		}

		/// <summary>
		///   <para>Scans a particular Java class for a field matching a name and a signature.</para>
		/// </summary>
		/// <param name="javaClass">Raw JNI Java class object (obtained by calling AndroidJNI.FindClass).</param>
		/// <param name="fieldName">Name of the field as declared in Java.</param>
		/// <param name="signature">Field signature (e.g. obtained by calling AndroidJNIHelper.GetSignature).</param>
		/// <param name="isStatic">Set to &lt;tt&gt;true&lt;tt&gt; for static fields; &lt;tt&gt;false&lt;tt&gt; for instance (nonstatic) fields.</param>
		[ExcludeFromDocs]
		public static IntPtr GetFieldID(IntPtr javaClass, string fieldName)
		{
			bool isStatic = false;
			string empty = string.Empty;
			return AndroidJNIHelper.GetFieldID(javaClass, fieldName, empty, isStatic);
		}

		/// <summary>
		///   <para>Scans a particular Java class for a field matching a name and a signature.</para>
		/// </summary>
		/// <param name="javaClass">Raw JNI Java class object (obtained by calling AndroidJNI.FindClass).</param>
		/// <param name="fieldName">Name of the field as declared in Java.</param>
		/// <param name="signature">Field signature (e.g. obtained by calling AndroidJNIHelper.GetSignature).</param>
		/// <param name="isStatic">Set to &lt;tt&gt;true&lt;tt&gt; for static fields; &lt;tt&gt;false&lt;tt&gt; for instance (nonstatic) fields.</param>
		public static IntPtr GetFieldID(IntPtr javaClass, string fieldName, [DefaultValue("\"\"")] string signature, [DefaultValue("false")] bool isStatic)
		{
			return _AndroidJNIHelper.GetFieldID(javaClass, fieldName, signature, isStatic);
		}

		/// <summary>
		///   <para>Creates a UnityJavaRunnable object (implements java.lang.Runnable).</para>
		/// </summary>
		/// <param name="runnable">A delegate representing the java.lang.Runnable.</param>
		/// <param name="jrunnable"></param>
		public static IntPtr CreateJavaRunnable(AndroidJavaRunnable jrunnable)
		{
			return _AndroidJNIHelper.CreateJavaRunnable(jrunnable);
		}

		/// <summary>
		///   <para>Creates a java proxy object which connects to the supplied proxy implementation.</para>
		/// </summary>
		/// <param name="proxy">An implementatinon of a java interface in c#.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr CreateJavaProxy(AndroidJavaProxy proxy);

		/// <summary>
		///   <para>Creates a Java array from a managed array.</para>
		/// </summary>
		/// <param name="array">Managed array to be converted into a Java array object.</param>
		public static IntPtr ConvertToJNIArray(Array array)
		{
			return _AndroidJNIHelper.ConvertToJNIArray(array);
		}

		/// <summary>
		///   <para>Creates the parameter array to be used as argument list when invoking Java code through CallMethod() in AndroidJNI.</para>
		/// </summary>
		/// <param name="args">An array of objects that should be converted to Call parameters.</param>
		public static jvalue[] CreateJNIArgArray(object[] args)
		{
			return _AndroidJNIHelper.CreateJNIArgArray(args);
		}

		/// <summary>
		///   <para>Deletes any local jni references previously allocated by CreateJNIArgArray().</para>
		/// </summary>
		/// <param name="args">The array of arguments used as a parameter to CreateJNIArgArray().</param>
		/// <param name="jniArgs">The array returned by CreateJNIArgArray().</param>
		public static void DeleteJNIArgArray(object[] args, jvalue[] jniArgs)
		{
			_AndroidJNIHelper.DeleteJNIArgArray(args, jniArgs);
		}

		/// <summary>
		///   <para>Get a JNI method ID for a constructor based on calling arguments.</para>
		/// </summary>
		/// <param name="javaClass">Raw JNI Java class object (obtained by calling AndroidJNI.FindClass).</param>
		/// <param name="args">Array with parameters to be passed to the constructor when invoked.</param>
		/// <param name="jclass"></param>
		public static IntPtr GetConstructorID(IntPtr jclass, object[] args)
		{
			return _AndroidJNIHelper.GetConstructorID(jclass, args);
		}

		/// <summary>
		///   <para>Get a JNI method ID based on calling arguments.</para>
		/// </summary>
		/// <param name="javaClass">Raw JNI Java class object (obtained by calling AndroidJNI.FindClass).</param>
		/// <param name="methodName">Name of the method as declared in Java.</param>
		/// <param name="args">Array with parameters to be passed to the method when invoked.</param>
		/// <param name="isStatic">Set to &lt;tt&gt;true&lt;tt&gt; for static methods; &lt;tt&gt;false&lt;tt&gt; for instance (nonstatic) methods.</param>
		/// <param name="jclass"></param>
		public static IntPtr GetMethodID(IntPtr jclass, string methodName, object[] args, bool isStatic)
		{
			return _AndroidJNIHelper.GetMethodID(jclass, methodName, args, isStatic);
		}

		/// <summary>
		///   <para>Creates the JNI signature string for particular object type.</para>
		/// </summary>
		/// <param name="obj">Object for which a signature is to be produced.</param>
		public static string GetSignature(object obj)
		{
			return _AndroidJNIHelper.GetSignature(obj);
		}

		/// <summary>
		///   <para>Creates the JNI signature string for an object parameter list.</para>
		/// </summary>
		/// <param name="args">Array of object for which a signature is to be produced.</param>
		public static string GetSignature(object[] args)
		{
			return _AndroidJNIHelper.GetSignature(args);
		}

		public static ArrayType ConvertFromJNIArray<ArrayType>(IntPtr array)
		{
			return _AndroidJNIHelper.ConvertFromJNIArray<ArrayType>(array);
		}

		public static IntPtr GetMethodID<ReturnType>(IntPtr jclass, string methodName, object[] args, bool isStatic)
		{
			return _AndroidJNIHelper.GetMethodID<ReturnType>(jclass, methodName, args, isStatic);
		}

		public static IntPtr GetFieldID<FieldType>(IntPtr jclass, string fieldName, bool isStatic)
		{
			return _AndroidJNIHelper.GetFieldID<FieldType>(jclass, fieldName, isStatic);
		}

		public static string GetSignature<ReturnType>(object[] args)
		{
			return _AndroidJNIHelper.GetSignature<ReturnType>(args);
		}
	}
}
