using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Internal;
using UnityEngineInternal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Base class for everything attached to GameObjects.</para>
	/// </summary>
	public class Component : Object
	{
		/// <summary>
		///   <para>The Transform attached to this GameObject (null if there is none attached).</para>
		/// </summary>
		public extern Transform transform { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The game object this component is attached to. A component is always attached to a game object.</para>
		/// </summary>
		public extern GameObject gameObject { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns the component of Type type if the game object has one attached, null if it doesn't.</para>
		/// </summary>
		/// <param name="type">The type of Component to retrieve.</param>
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponent(Type type)
		{
			return this.gameObject.GetComponent(type);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void GetComponentFastPath(Type type, IntPtr oneFurtherThanResultValue);

		[SecuritySafeCritical]
		public unsafe T GetComponent<T>()
		{
			CastHelper<T> castHelper = default(CastHelper<T>);
			this.GetComponentFastPath(typeof(T), new IntPtr((void*)(&castHelper.onePointerFurtherThanT)));
			return castHelper.t;
		}

		/// <summary>
		///   <para>Returns the component with name type if the game object has one attached, null if it doesn't.</para>
		/// </summary>
		/// <param name="type"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Component GetComponent(string type);

		/// <summary>
		///   <para>Returns the component of Type type in the GameObject or any of its children using depth first search.</para>
		/// </summary>
		/// <param name="t">The type of Component to retrieve.</param>
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInChildren(Type t)
		{
			return this.gameObject.GetComponentInChildren(t);
		}

		public T GetComponentInChildren<T>()
		{
			return (T)((object)this.GetComponentInChildren(typeof(T)));
		}

		/// <summary>
		///   <para>Returns all components of Type type in the GameObject or any of its children.</para>
		/// </summary>
		/// <param name="t">The type of Component to retrieve.</param>
		/// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
		[ExcludeFromDocs]
		public Component[] GetComponentsInChildren(Type t)
		{
			bool includeInactive = false;
			return this.GetComponentsInChildren(t, includeInactive);
		}

		/// <summary>
		///   <para>Returns all components of Type type in the GameObject or any of its children.</para>
		/// </summary>
		/// <param name="t">The type of Component to retrieve.</param>
		/// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
		public Component[] GetComponentsInChildren(Type t, [DefaultValue("false")] bool includeInactive)
		{
			return this.gameObject.GetComponentsInChildren(t, includeInactive);
		}

		public T[] GetComponentsInChildren<T>(bool includeInactive)
		{
			return this.gameObject.GetComponentsInChildren<T>(includeInactive);
		}

		public void GetComponentsInChildren<T>(bool includeInactive, List<T> result)
		{
			this.gameObject.GetComponentsInChildren<T>(includeInactive, result);
		}

		public T[] GetComponentsInChildren<T>()
		{
			return this.GetComponentsInChildren<T>(false);
		}

		public void GetComponentsInChildren<T>(List<T> results)
		{
			this.GetComponentsInChildren<T>(false, results);
		}

		/// <summary>
		///   <para>Returns the component of Type type in the GameObject or any of its parents.</para>
		/// </summary>
		/// <param name="t">The type of Component to retrieve.</param>
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInParent(Type t)
		{
			return this.gameObject.GetComponentInParent(t);
		}

		public T GetComponentInParent<T>()
		{
			return (T)((object)this.GetComponentInParent(typeof(T)));
		}

		[ExcludeFromDocs]
		public Component[] GetComponentsInParent(Type t)
		{
			bool includeInactive = false;
			return this.GetComponentsInParent(t, includeInactive);
		}

		/// <summary>
		///   <para>Returns all components of Type type in the GameObject or any of its parents.</para>
		/// </summary>
		/// <param name="t">The type of Component to retrieve.</param>
		/// <param name="includeInactive">Should inactive Components be included in the found set?</param>
		public Component[] GetComponentsInParent(Type t, [DefaultValue("false")] bool includeInactive)
		{
			return this.gameObject.GetComponentsInParent(t, includeInactive);
		}

		public T[] GetComponentsInParent<T>(bool includeInactive)
		{
			return this.gameObject.GetComponentsInParent<T>(includeInactive);
		}

		public void GetComponentsInParent<T>(bool includeInactive, List<T> results)
		{
			this.gameObject.GetComponentsInParent<T>(includeInactive, results);
		}

		public T[] GetComponentsInParent<T>()
		{
			return this.GetComponentsInParent<T>(false);
		}

		/// <summary>
		///   <para>Returns all components of Type type in the GameObject.</para>
		/// </summary>
		/// <param name="type">The type of Component to retrieve.</param>
		public Component[] GetComponents(Type type)
		{
			return this.gameObject.GetComponents(type);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetComponentsForListInternal(Type searchType, object resultList);

		public void GetComponents(Type type, List<Component> results)
		{
			this.GetComponentsForListInternal(type, results);
		}

		public void GetComponents<T>(List<T> results)
		{
			this.GetComponentsForListInternal(typeof(T), results);
		}

		/// <summary>
		///   <para>The tag of this game object.</para>
		/// </summary>
		public string tag
		{
			get
			{
				return this.gameObject.tag;
			}
			set
			{
				this.gameObject.tag = value;
			}
		}

		public T[] GetComponents<T>()
		{
			return this.gameObject.GetComponents<T>();
		}

		/// <summary>
		///   <para>Is this game object tagged with tag ?</para>
		/// </summary>
		/// <param name="tag">The tag to compare.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool CompareTag(string tag);

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
		/// </summary>
		/// <param name="methodName">Name of method to call.</param>
		/// <param name="value">Optional parameter value for the method.</param>
		/// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SendMessageUpwards(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
		/// </summary>
		/// <param name="methodName">Name of method to call.</param>
		/// <param name="value">Optional parameter value for the method.</param>
		/// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName, object value)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			this.SendMessageUpwards(methodName, value, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
		/// </summary>
		/// <param name="methodName">Name of method to call.</param>
		/// <param name="value">Optional parameter value for the method.</param>
		/// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object value = null;
			this.SendMessageUpwards(methodName, value, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
		/// </summary>
		/// <param name="methodName">Name of method to call.</param>
		/// <param name="value">Optional parameter value for the method.</param>
		/// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
		public void SendMessageUpwards(string methodName, SendMessageOptions options)
		{
			this.SendMessageUpwards(methodName, null, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
		/// </summary>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="value">Optional parameter for the method.</param>
		/// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SendMessage(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
		/// </summary>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="value">Optional parameter for the method.</param>
		/// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
		[ExcludeFromDocs]
		public void SendMessage(string methodName, object value)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			this.SendMessage(methodName, value, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
		/// </summary>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="value">Optional parameter for the method.</param>
		/// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
		[ExcludeFromDocs]
		public void SendMessage(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object value = null;
			this.SendMessage(methodName, value, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
		/// </summary>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="value">Optional parameter for the method.</param>
		/// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
		public void SendMessage(string methodName, SendMessageOptions options)
		{
			this.SendMessage(methodName, null, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
		/// </summary>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
		/// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BroadcastMessage(string methodName, [DefaultValue("null")] object parameter, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
		/// </summary>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
		/// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName, object parameter)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			this.BroadcastMessage(methodName, parameter, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
		/// </summary>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
		/// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object parameter = null;
			this.BroadcastMessage(methodName, parameter, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
		/// </summary>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
		/// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
		public void BroadcastMessage(string methodName, SendMessageOptions options)
		{
			this.BroadcastMessage(methodName, null, options);
		}
	}
}
