using System;
using System.Runtime.CompilerServices;
using UnityEngineInternal;

namespace UnityEngine
{
	/// <summary>
	///   <para>The Resources class allows you to find and access Objects including assets.</para>
	/// </summary>
	public sealed class Resources
	{
		internal static T[] ConvertObjects<T>(Object[] rawObjects) where T : Object
		{
			if (rawObjects == null)
			{
				return null;
			}
			T[] array = new T[rawObjects.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (T)((object)rawObjects[i]);
			}
			return array;
		}

		/// <summary>
		///   <para>Returns a list of all objects of Type type.</para>
		/// </summary>
		/// <param name="type">Type of the class to match while searching.</param>
		/// <returns>
		///   <para>An array of objects whose class is type or is derived from type.</para>
		/// </returns>
		[TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object[] FindObjectsOfTypeAll(Type type);

		public static T[] FindObjectsOfTypeAll<T>() where T : Object
		{
			return Resources.ConvertObjects<T>(Resources.FindObjectsOfTypeAll(typeof(T)));
		}

		/// <summary>
		///   <para>Loads an asset stored at path in a Resources folder.</para>
		/// </summary>
		/// <param name="path">Pathname of the target folder.</param>
		public static Object Load(string path)
		{
			return Resources.Load(path, typeof(Object));
		}

		public static T Load<T>(string path) where T : Object
		{
			return (T)((object)Resources.Load(path, typeof(T)));
		}

		/// <summary>
		///   <para>Loads an asset stored at path in a Resources folder.</para>
		/// </summary>
		/// <param name="path">Pathname of the target folder.</param>
		/// <param name="systemTypeInstance">Type filter for objects returned.</param>
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object Load(string path, Type systemTypeInstance);

		/// <summary>
		///   <para>Asynchronously loads an asset stored at path in a Resources folder.</para>
		/// </summary>
		/// <param name="path">Pathname of the target folder.</param>
		/// <param name="systemTypeInstance">Type filter for objects returned.</param>
		/// <param name="type"></param>
		public static ResourceRequest LoadAsync(string path)
		{
			return Resources.LoadAsync(path, typeof(Object));
		}

		public static ResourceRequest LoadAsync<T>(string path) where T : Object
		{
			return Resources.LoadAsync(path, typeof(T));
		}

		/// <summary>
		///   <para>Asynchronously loads an asset stored at path in a Resources folder.</para>
		/// </summary>
		/// <param name="path">Pathname of the target folder.</param>
		/// <param name="systemTypeInstance">Type filter for objects returned.</param>
		/// <param name="type"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ResourceRequest LoadAsync(string path, Type type);

		/// <summary>
		///   <para>Loads all assets in a folder or file at path in a Resources folder.</para>
		/// </summary>
		/// <param name="path">Pathname of the target folder.</param>
		/// <param name="systemTypeInstance">Type filter for objects returned.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object[] LoadAll(string path, Type systemTypeInstance);

		/// <summary>
		///   <para>Loads all assets in a folder or file at path in a Resources folder.</para>
		/// </summary>
		/// <param name="path">Pathname of the target folder.</param>
		public static Object[] LoadAll(string path)
		{
			return Resources.LoadAll(path, typeof(Object));
		}

		public static T[] LoadAll<T>(string path) where T : Object
		{
			return Resources.ConvertObjects<T>(Resources.LoadAll(path, typeof(T)));
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object GetBuiltinResource(Type type, string path);

		public static T GetBuiltinResource<T>(string path) where T : Object
		{
			return (T)((object)Resources.GetBuiltinResource(typeof(T), path));
		}

		/// <summary>
		///   <para>Unloads assetToUnload from memory.</para>
		/// </summary>
		/// <param name="assetToUnload"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UnloadAsset(Object assetToUnload);

		/// <summary>
		///   <para>Unloads assets that are not used.</para>
		/// </summary>
		/// <returns>
		///   <para>Object on which you can yield to wait until the operation completes.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AsyncOperation UnloadUnusedAssets();
	}
}
