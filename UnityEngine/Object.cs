using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Internal;
using UnityEngineInternal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Base class for all objects Unity can reference.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class Object
	{
		private int m_InstanceID;

		private IntPtr m_CachedPtr;

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object Internal_CloneSingle(Object data);

		private static Object Internal_InstantiateSingle(Object data, Vector3 pos, Quaternion rot)
		{
			return Object.INTERNAL_CALL_Internal_InstantiateSingle(data, ref pos, ref rot);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object INTERNAL_CALL_Internal_InstantiateSingle(Object data, ref Vector3 pos, ref Quaternion rot);

		/// <summary>
		///   <para>Removes a gameobject, component or asset.</para>
		/// </summary>
		/// <param name="obj">The object to destroy.</param>
		/// <param name="t">The optional amount of time to delay before destroying the object.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Destroy(Object obj, [DefaultValue("0.0F")] float t);

		/// <summary>
		///   <para>Removes a gameobject, component or asset.</para>
		/// </summary>
		/// <param name="obj">The object to destroy.</param>
		/// <param name="t">The optional amount of time to delay before destroying the object.</param>
		[ExcludeFromDocs]
		public static void Destroy(Object obj)
		{
			float t = 0f;
			Object.Destroy(obj, t);
		}

		/// <summary>
		///   <para>Destroys the object obj immediately. You are strongly recommended to use Destroy instead.</para>
		/// </summary>
		/// <param name="obj">Object to be destroyed.</param>
		/// <param name="allowDestroyingAssets">Set to true to allow assets to be destoyed.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DestroyImmediate(Object obj, [DefaultValue("false")] bool allowDestroyingAssets);

		/// <summary>
		///   <para>Destroys the object obj immediately. You are strongly recommended to use Destroy instead.</para>
		/// </summary>
		/// <param name="obj">Object to be destroyed.</param>
		/// <param name="allowDestroyingAssets">Set to true to allow assets to be destoyed.</param>
		[ExcludeFromDocs]
		public static void DestroyImmediate(Object obj)
		{
			bool allowDestroyingAssets = false;
			Object.DestroyImmediate(obj, allowDestroyingAssets);
		}

		/// <summary>
		///   <para>Returns a list of all active loaded objects of Type type.</para>
		/// </summary>
		/// <param name="type">The type of object to find.</param>
		/// <returns>
		///   <para>The array of objects found matching the type specified.</para>
		/// </returns>
		[TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object[] FindObjectsOfType(Type type);

		/// <summary>
		///   <para>The name of the object.</para>
		/// </summary>
		public extern string name { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Makes the object target not be destroyed automatically when loading a new scene.</para>
		/// </summary>
		/// <param name="target"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DontDestroyOnLoad(Object target);

		/// <summary>
		///   <para>Should the object be hidden, saved with the scene or modifiable by the user?</para>
		/// </summary>
		public extern HideFlags hideFlags { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DestroyObject(Object obj, [DefaultValue("0.0F")] float t);

		[ExcludeFromDocs]
		public static void DestroyObject(Object obj)
		{
			float t = 0f;
			Object.DestroyObject(obj, t);
		}

		[Obsolete("use Object.FindObjectsOfType instead.")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object[] FindSceneObjectsOfType(Type type);

		/// <summary>
		///   <para>Returns a list of all active and inactive loaded objects of Type type, including assets.</para>
		/// </summary>
		/// <param name="type">The type of object or asset to find.</param>
		/// <returns>
		///   <para>The array of objects and assets found matching the type specified.</para>
		/// </returns>
		[WrapperlessIcall]
		[Obsolete("use Resources.FindObjectsOfTypeAll instead.")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object[] FindObjectsOfTypeIncludingAssets(Type type);

		/// <summary>
		///   <para>Returns a list of all active and inactive loaded objects of Type type.</para>
		/// </summary>
		/// <param name="type">The type of object to find.</param>
		/// <returns>
		///   <para>The array of objects found matching the type specified.</para>
		/// </returns>
		[Obsolete("Please use Resources.FindObjectsOfTypeAll instead")]
		public static Object[] FindObjectsOfTypeAll(Type type)
		{
			return Resources.FindObjectsOfTypeAll(type);
		}

		/// <summary>
		///   <para>Returns the name of the game object.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern string ToString();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool DoesObjectWithInstanceIDExist(int instanceID);

		public override bool Equals(object o)
		{
			return Object.CompareBaseObjects(this, o as Object);
		}

		public override int GetHashCode()
		{
			return this.GetInstanceID();
		}

		private static bool CompareBaseObjects(Object lhs, Object rhs)
		{
			bool flag = lhs == null;
			bool flag2 = rhs == null;
			if (flag2 && flag)
			{
				return true;
			}
			if (flag2)
			{
				return !Object.IsNativeObjectAlive(lhs);
			}
			if (flag)
			{
				return !Object.IsNativeObjectAlive(rhs);
			}
			return lhs.m_InstanceID == rhs.m_InstanceID;
		}

		private static bool IsNativeObjectAlive(Object o)
		{
			return o.GetCachedPtr() != IntPtr.Zero;
		}

		/// <summary>
		///   <para>Returns the instance id of the object.</para>
		/// </summary>
		public int GetInstanceID()
		{
			return this.m_InstanceID;
		}

		private IntPtr GetCachedPtr()
		{
			return this.m_CachedPtr;
		}

		/// <summary>
		///   <para>Clones the object original and returns the clone.</para>
		/// </summary>
		/// <param name="original">An existing object that you want to make a copy of.</param>
		/// <param name="position">Position for the new object.</param>
		/// <param name="rotation">Orientation of the new object.</param>
		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation)
		{
			Object.CheckNullArgument(original, "The thing you want to instantiate is null.");
			return Object.Internal_InstantiateSingle(original, position, rotation);
		}

		/// <summary>
		///   <para>Clones the object original and returns the clone.</para>
		/// </summary>
		/// <param name="original">An existing object that you want to make a copy of.</param>
		/// <param name="position">Position for the new object.</param>
		/// <param name="rotation">Orientation of the new object.</param>
		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original)
		{
			Object.CheckNullArgument(original, "The thing you want to instantiate is null.");
			return Object.Internal_CloneSingle(original);
		}

		public static T Instantiate<T>(T original) where T : Object
		{
			Object.CheckNullArgument(original, "The thing you want to instantiate is null.");
			return (T)((object)Object.Internal_CloneSingle(original));
		}

		private static void CheckNullArgument(object arg, string message)
		{
			if (arg == null)
			{
				throw new ArgumentException(message);
			}
		}

		public static T[] FindObjectsOfType<T>() where T : Object
		{
			return Resources.ConvertObjects<T>(Object.FindObjectsOfType(typeof(T)));
		}

		/// <summary>
		///   <para>Returns the first active loaded object of Type type.</para>
		/// </summary>
		/// <param name="type"></param>
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public static Object FindObjectOfType(Type type)
		{
			Object[] array = Object.FindObjectsOfType(type);
			if (array.Length > 0)
			{
				return array[0];
			}
			return null;
		}

		public static T FindObjectOfType<T>() where T : Object
		{
			return (T)((object)Object.FindObjectOfType(typeof(T)));
		}

		public static implicit operator bool(Object exists)
		{
			return !Object.CompareBaseObjects(exists, null);
		}

		public static bool operator ==(Object x, Object y)
		{
			return Object.CompareBaseObjects(x, y);
		}

		public static bool operator !=(Object x, Object y)
		{
			return !Object.CompareBaseObjects(x, y);
		}
	}
}
