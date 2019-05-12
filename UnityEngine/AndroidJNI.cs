using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>'Raw' JNI interface to Android Dalvik (Java) VM from Mono (CS/JS).</para>
	/// </summary>
	public sealed class AndroidJNI
	{
		private AndroidJNI()
		{
		}

		/// <summary>
		///   <para>Attaches the current thread to a Java (Dalvik) VM.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int AttachCurrentThread();

		/// <summary>
		///   <para>Detaches the current thread from a Java (Dalvik) VM.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int DetachCurrentThread();

		/// <summary>
		///   <para>Returns the version of the native method interface.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetVersion();

		/// <summary>
		///   <para>This function loads a locally-defined class.</para>
		/// </summary>
		/// <param name="name"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr FindClass(string name);

		/// <summary>
		///   <para>Converts a &lt;tt&gt;java.lang.reflect.Method&lt;tt&gt; or &lt;tt&gt;java.lang.reflect.Constructor&lt;tt&gt; object to a method ID.</para>
		/// </summary>
		/// <param name="refMethod"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr FromReflectedMethod(IntPtr refMethod);

		/// <summary>
		///   <para>Converts a &lt;tt&gt;java.lang.reflect.Field&lt;/tt&gt; to a field ID.</para>
		/// </summary>
		/// <param name="refField"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr FromReflectedField(IntPtr refField);

		/// <summary>
		///   <para>Converts a method ID derived from clazz to a &lt;tt&gt;java.lang.reflect.Method&lt;tt&gt; or &lt;tt&gt;java.lang.reflect.Constructor&lt;tt&gt; object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="isStatic"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ToReflectedMethod(IntPtr clazz, IntPtr methodID, bool isStatic);

		/// <summary>
		///   <para>Converts a field ID derived from cls to a &lt;tt&gt;java.lang.reflect.Field&lt;/tt&gt; object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		/// <param name="isStatic"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ToReflectedField(IntPtr clazz, IntPtr fieldID, bool isStatic);

		/// <summary>
		///   <para>If &lt;tt&gt;clazz&lt;tt&gt; represents any class other than the class &lt;tt&gt;Object&lt;tt&gt;, then this function returns the object that represents the superclass of the class specified by &lt;tt&gt;clazz&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="clazz"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetSuperclass(IntPtr clazz);

		/// <summary>
		///   <para>Determines whether an object of &lt;tt&gt;clazz1&lt;tt&gt; can be safely cast to &lt;tt&gt;clazz2&lt;tt&gt;.</para>
		/// </summary>
		/// <param name="clazz1"></param>
		/// <param name="clazz2"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsAssignableFrom(IntPtr clazz1, IntPtr clazz2);

		/// <summary>
		///   <para>Causes a &lt;tt&gt;java.lang.Throwable&lt;/tt&gt; object to be thrown.</para>
		/// </summary>
		/// <param name="obj"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Throw(IntPtr obj);

		/// <summary>
		///   <para>Constructs an exception object from the specified class with the &lt;tt&gt;message&lt;/tt&gt; specified by message and causes that exception to be thrown.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="message"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ThrowNew(IntPtr clazz, string message);

		/// <summary>
		///   <para>Determines if an exception is being thrown.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ExceptionOccurred();

		/// <summary>
		///   <para>Prints an exception and a backtrace of the stack to the &lt;tt&gt;logcat&lt;/tt&gt;</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ExceptionDescribe();

		/// <summary>
		///   <para>Clears any exception that is currently being thrown.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ExceptionClear();

		/// <summary>
		///   <para>Raises a fatal error and does not expect the VM to recover. This function does not return.</para>
		/// </summary>
		/// <param name="message"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void FatalError(string message);

		/// <summary>
		///   <para>Creates a new local reference frame, in which at least a given number of local references can be created.</para>
		/// </summary>
		/// <param name="capacity"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int PushLocalFrame(int capacity);

		/// <summary>
		///   <para>Pops off the current local reference frame, frees all the local references, and returns a local reference in the previous local reference frame for the given &lt;tt&gt;result&lt;/tt&gt; object.</para>
		/// </summary>
		/// <param name="result"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr PopLocalFrame(IntPtr result);

		/// <summary>
		///   <para>Creates a new global reference to the object referred to by the &lt;tt&gt;obj&lt;/tt&gt; argument.</para>
		/// </summary>
		/// <param name="obj"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewGlobalRef(IntPtr obj);

		/// <summary>
		///   <para>Deletes the global reference pointed to by &lt;tt&gt;obj&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="obj"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeleteGlobalRef(IntPtr obj);

		/// <summary>
		///   <para>Creates a new local reference that refers to the same object as &lt;tt&gt;obj&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="obj"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewLocalRef(IntPtr obj);

		/// <summary>
		///   <para>Deletes the local reference pointed to by &lt;tt&gt;obj&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="obj"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeleteLocalRef(IntPtr obj);

		/// <summary>
		///   <para>Tests whether two references refer to the same Java object.</para>
		/// </summary>
		/// <param name="obj1"></param>
		/// <param name="obj2"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsSameObject(IntPtr obj1, IntPtr obj2);

		/// <summary>
		///   <para>Ensures that at least a given number of local references can be created in the current thread.</para>
		/// </summary>
		/// <param name="capacity"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int EnsureLocalCapacity(int capacity);

		/// <summary>
		///   <para>Allocates a new Java object without invoking any of the constructors for the object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr AllocObject(IntPtr clazz);

		/// <summary>
		///   <para>Constructs a new Java object. The method ID indicates which constructor method to invoke. This ID must be obtained by calling GetMethodID() with &lt;init&gt; as the method name and void (V) as the return type.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewObject(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Returns the class of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetObjectClass(IntPtr obj);

		/// <summary>
		///   <para>Tests whether an object is an instance of a class.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="clazz"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsInstanceOf(IntPtr obj, IntPtr clazz);

		/// <summary>
		///   <para>Returns the method ID for an instance (nonstatic) method of a class or interface.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="name"></param>
		/// <param name="sig"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetMethodID(IntPtr clazz, string name, string sig);

		/// <summary>
		///   <para>Returns the field ID for an instance (nonstatic) field of a class.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="name"></param>
		/// <param name="sig"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetFieldID(IntPtr clazz, string name, string sig);

		/// <summary>
		///   <para>Returns the method ID for a static method of a class.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="name"></param>
		/// <param name="sig"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetStaticMethodID(IntPtr clazz, string name, string sig);

		/// <summary>
		///   <para>Returns the field ID for a static field of a class.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="name"></param>
		/// <param name="sig"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetStaticFieldID(IntPtr clazz, string name, string sig);

		/// <summary>
		///   <para>Constructs a new &lt;tt&gt;java.lang.String&lt;/tt&gt; object from an array of characters in modified UTF-8 encoding.</para>
		/// </summary>
		/// <param name="bytes"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewStringUTF(string bytes);

		/// <summary>
		///   <para>Returns the length in bytes of the modified UTF-8 representation of a string.</para>
		/// </summary>
		/// <param name="str"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetStringUTFLength(IntPtr str);

		/// <summary>
		///   <para>Returns a managed string object representing the string in modified UTF-8 encoding.</para>
		/// </summary>
		/// <param name="str"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetStringUTFChars(IntPtr str);

		/// <summary>
		///   <para>Calls an instance (nonstatic) Java method defined by &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string CallStringMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Calls an instance (nonstatic) Java method defined by &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr CallObjectMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Calls an instance (nonstatic) Java method defined by &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int CallIntMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Calls an instance (nonstatic) Java method defined by &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CallBooleanMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Calls an instance (nonstatic) Java method defined by &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short CallShortMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Calls an instance (nonstatic) Java method defined by &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte CallByteMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Calls an instance (nonstatic) Java method defined by &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char CallCharMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Calls an instance (nonstatic) Java method defined by &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CallFloatMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Calls an instance (nonstatic) Java method defined by &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double CallDoubleMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Calls an instance (nonstatic) Java method defined by &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long CallLongMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Calls an instance (nonstatic) Java method defined by &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CallVoidMethod(IntPtr obj, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>This function returns the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetStringField(IntPtr obj, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetObjectField(IntPtr obj, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetBooleanField(IntPtr obj, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte GetByteField(IntPtr obj, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char GetCharField(IntPtr obj, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short GetShortField(IntPtr obj, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetIntField(IntPtr obj, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long GetLongField(IntPtr obj, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetFloatField(IntPtr obj, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double GetDoubleField(IntPtr obj, IntPtr fieldID);

		/// <summary>
		///   <para>This function sets the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStringField(IntPtr obj, IntPtr fieldID, string val);

		/// <summary>
		///   <para>This function sets the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetObjectField(IntPtr obj, IntPtr fieldID, IntPtr val);

		/// <summary>
		///   <para>This function sets the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetBooleanField(IntPtr obj, IntPtr fieldID, bool val);

		/// <summary>
		///   <para>This function sets the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetByteField(IntPtr obj, IntPtr fieldID, byte val);

		/// <summary>
		///   <para>This function sets the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetCharField(IntPtr obj, IntPtr fieldID, char val);

		/// <summary>
		///   <para>This function sets the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetShortField(IntPtr obj, IntPtr fieldID, short val);

		/// <summary>
		///   <para>This function sets the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetIntField(IntPtr obj, IntPtr fieldID, int val);

		/// <summary>
		///   <para>This function sets the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLongField(IntPtr obj, IntPtr fieldID, long val);

		/// <summary>
		///   <para>This function sets the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetFloatField(IntPtr obj, IntPtr fieldID, float val);

		/// <summary>
		///   <para>This function sets the value of an instance (nonstatic) field of an object.</para>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetDoubleField(IntPtr obj, IntPtr fieldID, double val);

		/// <summary>
		///   <para>Invokes a static method on a Java object, according to the specified &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string CallStaticStringMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Invokes a static method on a Java object, according to the specified &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr CallStaticObjectMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Invokes a static method on a Java object, according to the specified &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int CallStaticIntMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Invokes a static method on a Java object, according to the specified &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CallStaticBooleanMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Invokes a static method on a Java object, according to the specified &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short CallStaticShortMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Invokes a static method on a Java object, according to the specified &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte CallStaticByteMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Invokes a static method on a Java object, according to the specified &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char CallStaticCharMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Invokes a static method on a Java object, according to the specified &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CallStaticFloatMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Invokes a static method on a Java object, according to the specified &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double CallStaticDoubleMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Invokes a static method on a Java object, according to the specified &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long CallStaticLongMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>Invokes a static method on a Java object, according to the specified &lt;tt&gt;methodID&lt;tt&gt;, optionally passing an array of arguments (&lt;tt&gt;args&lt;tt&gt;) to the method.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="methodID"></param>
		/// <param name="args"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CallStaticVoidMethod(IntPtr clazz, IntPtr methodID, jvalue[] args);

		/// <summary>
		///   <para>This function returns the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetStaticStringField(IntPtr clazz, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetStaticObjectField(IntPtr clazz, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetStaticBooleanField(IntPtr clazz, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte GetStaticByteField(IntPtr clazz, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char GetStaticCharField(IntPtr clazz, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short GetStaticShortField(IntPtr clazz, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetStaticIntField(IntPtr clazz, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long GetStaticLongField(IntPtr clazz, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetStaticFloatField(IntPtr clazz, IntPtr fieldID);

		/// <summary>
		///   <para>This function returns the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double GetStaticDoubleField(IntPtr clazz, IntPtr fieldID);

		/// <summary>
		///   <para>This function ets the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticStringField(IntPtr clazz, IntPtr fieldID, string val);

		/// <summary>
		///   <para>This function ets the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticObjectField(IntPtr clazz, IntPtr fieldID, IntPtr val);

		/// <summary>
		///   <para>This function ets the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticBooleanField(IntPtr clazz, IntPtr fieldID, bool val);

		/// <summary>
		///   <para>This function ets the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticByteField(IntPtr clazz, IntPtr fieldID, byte val);

		/// <summary>
		///   <para>This function ets the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticCharField(IntPtr clazz, IntPtr fieldID, char val);

		/// <summary>
		///   <para>This function ets the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticShortField(IntPtr clazz, IntPtr fieldID, short val);

		/// <summary>
		///   <para>This function ets the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticIntField(IntPtr clazz, IntPtr fieldID, int val);

		/// <summary>
		///   <para>This function ets the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticLongField(IntPtr clazz, IntPtr fieldID, long val);

		/// <summary>
		///   <para>This function ets the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticFloatField(IntPtr clazz, IntPtr fieldID, float val);

		/// <summary>
		///   <para>This function ets the value of a static field of an object.</para>
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="fieldID"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStaticDoubleField(IntPtr clazz, IntPtr fieldID, double val);

		/// <summary>
		///   <para>Convert a managed array of System.Boolean to a Java array of &lt;tt&gt;boolean&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ToBooleanArray(bool[] array);

		/// <summary>
		///   <para>Convert a managed array of System.Byte to a Java array of &lt;tt&gt;byte&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ToByteArray(byte[] array);

		/// <summary>
		///   <para>Convert a managed array of System.Char to a Java array of &lt;tt&gt;char&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ToCharArray(char[] array);

		/// <summary>
		///   <para>Convert a managed array of System.Int16 to a Java array of &lt;tt&gt;short&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ToShortArray(short[] array);

		/// <summary>
		///   <para>Convert a managed array of System.Int32 to a Java array of &lt;tt&gt;int&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ToIntArray(int[] array);

		/// <summary>
		///   <para>Convert a managed array of System.Int64 to a Java array of &lt;tt&gt;long&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ToLongArray(long[] array);

		/// <summary>
		///   <para>Convert a managed array of System.Single to a Java array of &lt;tt&gt;float&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ToFloatArray(float[] array);

		/// <summary>
		///   <para>Convert a managed array of System.Double to a Java array of &lt;tt&gt;double&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ToDoubleArray(double[] array);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ToObjectArray(IntPtr[] array, IntPtr arrayClass);

		/// <summary>
		///   <para>Convert a managed array of System.IntPtr, representing Java objects, to a Java array of &lt;tt&gt;java.lang.Object&lt;/tt&gt;.</para>
		/// </summary>
		/// <param name="array"></param>
		public static IntPtr ToObjectArray(IntPtr[] array)
		{
			return AndroidJNI.ToObjectArray(array, IntPtr.Zero);
		}

		/// <summary>
		///   <para>Convert a Java array of &lt;tt&gt;boolean&lt;/tt&gt; to a managed array of System.Boolean.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool[] FromBooleanArray(IntPtr array);

		/// <summary>
		///   <para>Convert a Java array of &lt;tt&gt;byte&lt;/tt&gt; to a managed array of System.Byte.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte[] FromByteArray(IntPtr array);

		/// <summary>
		///   <para>Convert a Java array of &lt;tt&gt;char&lt;/tt&gt; to a managed array of System.Char.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char[] FromCharArray(IntPtr array);

		/// <summary>
		///   <para>Convert a Java array of &lt;tt&gt;short&lt;/tt&gt; to a managed array of System.Int16.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short[] FromShortArray(IntPtr array);

		/// <summary>
		///   <para>Convert a Java array of &lt;tt&gt;int&lt;/tt&gt; to a managed array of System.Int32.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int[] FromIntArray(IntPtr array);

		/// <summary>
		///   <para>Convert a Java array of &lt;tt&gt;long&lt;/tt&gt; to a managed array of System.Int64.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long[] FromLongArray(IntPtr array);

		/// <summary>
		///   <para>Convert a Java array of &lt;tt&gt;float&lt;/tt&gt; to a managed array of System.Single.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float[] FromFloatArray(IntPtr array);

		/// <summary>
		///   <para>Convert a Java array of &lt;tt&gt;double&lt;/tt&gt; to a managed array of System.Double.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double[] FromDoubleArray(IntPtr array);

		/// <summary>
		///   <para>Convert a Java array of &lt;tt&gt;java.lang.Object&lt;/tt&gt; to a managed array of System.IntPtr, representing Java objects.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr[] FromObjectArray(IntPtr array);

		/// <summary>
		///   <para>Returns the number of elements in the array.</para>
		/// </summary>
		/// <param name="array"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetArrayLength(IntPtr array);

		/// <summary>
		///   <para>Construct a new primitive array object.</para>
		/// </summary>
		/// <param name="size"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewBooleanArray(int size);

		/// <summary>
		///   <para>Construct a new primitive array object.</para>
		/// </summary>
		/// <param name="size"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewByteArray(int size);

		/// <summary>
		///   <para>Construct a new primitive array object.</para>
		/// </summary>
		/// <param name="size"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewCharArray(int size);

		/// <summary>
		///   <para>Construct a new primitive array object.</para>
		/// </summary>
		/// <param name="size"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewShortArray(int size);

		/// <summary>
		///   <para>Construct a new primitive array object.</para>
		/// </summary>
		/// <param name="size"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewIntArray(int size);

		/// <summary>
		///   <para>Construct a new primitive array object.</para>
		/// </summary>
		/// <param name="size"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewLongArray(int size);

		/// <summary>
		///   <para>Construct a new primitive array object.</para>
		/// </summary>
		/// <param name="size"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewFloatArray(int size);

		/// <summary>
		///   <para>Construct a new primitive array object.</para>
		/// </summary>
		/// <param name="size"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewDoubleArray(int size);

		/// <summary>
		///   <para>Constructs a new array holding objects in class &lt;tt&gt;clazz&lt;tt&gt;. All elements are initially set to &lt;tt&gt;obj&lt;tt&gt;.</para>
		/// </summary>
		/// <param name="size"></param>
		/// <param name="clazz"></param>
		/// <param name="obj"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr NewObjectArray(int size, IntPtr clazz, IntPtr obj);

		/// <summary>
		///   <para>Returns the value of one element of a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetBooleanArrayElement(IntPtr array, int index);

		/// <summary>
		///   <para>Returns the value of one element of a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte GetByteArrayElement(IntPtr array, int index);

		/// <summary>
		///   <para>Returns the value of one element of a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern char GetCharArrayElement(IntPtr array, int index);

		/// <summary>
		///   <para>Returns the value of one element of a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short GetShortArrayElement(IntPtr array, int index);

		/// <summary>
		///   <para>Returns the value of one element of a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetIntArrayElement(IntPtr array, int index);

		/// <summary>
		///   <para>Returns the value of one element of a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long GetLongArrayElement(IntPtr array, int index);

		/// <summary>
		///   <para>Returns the value of one element of a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetFloatArrayElement(IntPtr array, int index);

		/// <summary>
		///   <para>Returns the value of one element of a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double GetDoubleArrayElement(IntPtr array, int index);

		/// <summary>
		///   <para>Returns an element of an &lt;tt&gt;Object&lt;/tt&gt; array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetObjectArrayElement(IntPtr array, int index);

		/// <summary>
		///   <para>Sets the value of one element in a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetBooleanArrayElement(IntPtr array, int index, byte val);

		/// <summary>
		///   <para>Sets the value of one element in a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetByteArrayElement(IntPtr array, int index, sbyte val);

		/// <summary>
		///   <para>Sets the value of one element in a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetCharArrayElement(IntPtr array, int index, char val);

		/// <summary>
		///   <para>Sets the value of one element in a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetShortArrayElement(IntPtr array, int index, short val);

		/// <summary>
		///   <para>Sets the value of one element in a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetIntArrayElement(IntPtr array, int index, int val);

		/// <summary>
		///   <para>Sets the value of one element in a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLongArrayElement(IntPtr array, int index, long val);

		/// <summary>
		///   <para>Sets the value of one element in a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetFloatArrayElement(IntPtr array, int index, float val);

		/// <summary>
		///   <para>Sets the value of one element in a primitive array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <param name="val"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetDoubleArrayElement(IntPtr array, int index, double val);

		/// <summary>
		///   <para>Sets an element of an &lt;tt&gt;Object&lt;/tt&gt; array.</para>
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		/// <param name="obj"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetObjectArrayElement(IntPtr array, int index, IntPtr obj);
	}
}
