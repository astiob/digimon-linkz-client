using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A class you can derive from if you want to create objects that don't need to be attached to game objects.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class ScriptableObject : Object
	{
		public ScriptableObject()
		{
			ScriptableObject.Internal_CreateScriptableObject(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateScriptableObject([Writable] ScriptableObject self);

		[Obsolete("Use EditorUtility.SetDirty instead")]
		public void SetDirty()
		{
			ScriptableObject.INTERNAL_CALL_SetDirty(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetDirty(ScriptableObject self);

		/// <summary>
		///   <para>Creates an instance of a scriptable object with className.</para>
		/// </summary>
		/// <param name="className"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ScriptableObject CreateInstance(string className);

		/// <summary>
		///   <para>Creates an instance of a scriptable object with type.</para>
		/// </summary>
		/// <param name="type"></param>
		public static ScriptableObject CreateInstance(Type type)
		{
			return ScriptableObject.CreateInstanceFromType(type);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ScriptableObject CreateInstanceFromType(Type type);

		public static T CreateInstance<T>() where T : ScriptableObject
		{
			return (T)((object)ScriptableObject.CreateInstance(typeof(T)));
		}
	}
}
