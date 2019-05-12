using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Internal;
using UnityEngineInternal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Base class for all entities in Unity scenes.</para>
	/// </summary>
	public sealed class GameObject : Object
	{
		/// <summary>
		///   <para>Creates a new game object, named name.</para>
		/// </summary>
		/// <param name="name"></param>
		public GameObject(string name)
		{
			GameObject.Internal_CreateGameObject(this, name);
		}

		/// <summary>
		///   <para>Creates a new game object.</para>
		/// </summary>
		public GameObject()
		{
			GameObject.Internal_CreateGameObject(this, null);
		}

		/// <summary>
		///   <para>Creates a game object and attaches the specified components.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="components"></param>
		public GameObject(string name, params Type[] components)
		{
			GameObject.Internal_CreateGameObject(this, name);
			foreach (Type componentType in components)
			{
				this.AddComponent(componentType);
			}
		}

		/// <summary>
		///   <para>Creates a game object with a primitive mesh renderer and appropriate collider.</para>
		/// </summary>
		/// <param name="type">The type of primitive object to create.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern GameObject CreatePrimitive(PrimitiveType type);

		/// <summary>
		///   <para>Returns the component of Type type if the game object has one attached, null if it doesn't.</para>
		/// </summary>
		/// <param name="type">The type of Component to retrieve.</param>
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Component GetComponent(Type type);

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

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Component GetComponentByName(string type);

		/// <summary>
		///   <para>Returns the component with name type if the game object has one attached, null if it doesn't.</para>
		/// </summary>
		/// <param name="type">The type of Component to retrieve.</param>
		public Component GetComponent(string type)
		{
			return this.GetComponentByName(type);
		}

		/// <summary>
		///   <para>Returns the component of Type type in the GameObject or any of its children using depth first search.</para>
		/// </summary>
		/// <param name="type">The type of Component to retrieve.</param>
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInChildren(Type type)
		{
			if (this.activeInHierarchy)
			{
				Component component = this.GetComponent(type);
				if (component != null)
				{
					return component;
				}
			}
			Transform transform = this.transform;
			if (transform != null)
			{
				foreach (object obj in transform)
				{
					Transform transform2 = (Transform)obj;
					Component componentInChildren = transform2.gameObject.GetComponentInChildren(type);
					if (componentInChildren != null)
					{
						return componentInChildren;
					}
				}
			}
			return null;
		}

		public T GetComponentInChildren<T>()
		{
			return (T)((object)this.GetComponentInChildren(typeof(T)));
		}

		/// <summary>
		///   <para>Finds component in the parent.</para>
		/// </summary>
		/// <param name="type">Type of component to find.</param>
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInParent(Type type)
		{
			if (this.activeInHierarchy)
			{
				Component component = this.GetComponent(type);
				if (component != null)
				{
					return component;
				}
			}
			Transform parent = this.transform.parent;
			if (parent != null)
			{
				while (parent != null)
				{
					if (parent.gameObject.activeInHierarchy)
					{
						Component component2 = parent.gameObject.GetComponent(type);
						if (component2 != null)
						{
							return component2;
						}
					}
					parent = parent.parent;
				}
			}
			return null;
		}

		public T GetComponentInParent<T>()
		{
			return (T)((object)this.GetComponentInParent(typeof(T)));
		}

		/// <summary>
		///   <para>Returns all components of Type type in the GameObject.</para>
		/// </summary>
		/// <param name="type">The type of Component to retrieve.</param>
		public Component[] GetComponents(Type type)
		{
			return (Component[])this.GetComponentsInternal(type, false, false, true, false, null);
		}

		public T[] GetComponents<T>()
		{
			return (T[])this.GetComponentsInternal(typeof(T), true, false, true, false, null);
		}

		public void GetComponents(Type type, List<Component> results)
		{
			this.GetComponentsInternal(type, false, false, true, false, results);
		}

		public void GetComponents<T>(List<T> results)
		{
			this.GetComponentsInternal(typeof(T), false, false, true, false, results);
		}

		/// <summary>
		///   <para>Returns all components of Type type in the GameObject or any of its children.</para>
		/// </summary>
		/// <param name="type">The type of Component to retrieve.</param>
		/// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
		[ExcludeFromDocs]
		public Component[] GetComponentsInChildren(Type type)
		{
			bool includeInactive = false;
			return this.GetComponentsInChildren(type, includeInactive);
		}

		/// <summary>
		///   <para>Returns all components of Type type in the GameObject or any of its children.</para>
		/// </summary>
		/// <param name="type">The type of Component to retrieve.</param>
		/// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
		public Component[] GetComponentsInChildren(Type type, [DefaultValue("false")] bool includeInactive)
		{
			return (Component[])this.GetComponentsInternal(type, false, true, includeInactive, false, null);
		}

		public T[] GetComponentsInChildren<T>(bool includeInactive)
		{
			return (T[])this.GetComponentsInternal(typeof(T), true, true, includeInactive, false, null);
		}

		public void GetComponentsInChildren<T>(bool includeInactive, List<T> results)
		{
			this.GetComponentsInternal(typeof(T), true, true, includeInactive, false, results);
		}

		public T[] GetComponentsInChildren<T>()
		{
			return this.GetComponentsInChildren<T>(false);
		}

		public void GetComponentsInChildren<T>(List<T> results)
		{
			this.GetComponentsInChildren<T>(false, results);
		}

		[ExcludeFromDocs]
		public Component[] GetComponentsInParent(Type type)
		{
			bool includeInactive = false;
			return this.GetComponentsInParent(type, includeInactive);
		}

		/// <summary>
		///   <para>Returns all components of Type type in the GameObject or any of its parents.</para>
		/// </summary>
		/// <param name="type">The type of Component to retrieve.</param>
		/// <param name="includeInactive">Should inactive Components be included in the found set?</param>
		public Component[] GetComponentsInParent(Type type, [DefaultValue("false")] bool includeInactive)
		{
			return (Component[])this.GetComponentsInternal(type, false, true, includeInactive, true, null);
		}

		public void GetComponentsInParent<T>(bool includeInactive, List<T> results)
		{
			this.GetComponentsInternal(typeof(T), true, true, includeInactive, true, results);
		}

		public T[] GetComponentsInParent<T>(bool includeInactive)
		{
			return (T[])this.GetComponentsInternal(typeof(T), true, true, includeInactive, true, null);
		}

		public T[] GetComponentsInParent<T>()
		{
			return this.GetComponentsInParent<T>(false);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Array GetComponentsInternal(Type type, bool useSearchTypeAsArrayReturnType, bool recursive, bool includeInactive, bool reverse, object resultList);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Component AddComponentInternal(string className);

		/// <summary>
		///   <para>The Transform attached to this GameObject. (null if there is none attached).</para>
		/// </summary>
		public extern Transform transform { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The layer the game object is in. A layer is in the range [0...31].</para>
		/// </summary>
		public extern int layer { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("GameObject.active is obsolete. Use GameObject.SetActive(), GameObject.activeSelf or GameObject.activeInHierarchy.")]
		public extern bool active { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Activates/Deactivates the GameObject.</para>
		/// </summary>
		/// <param name="value">Activate or deactivation the  object.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetActive(bool value);

		/// <summary>
		///   <para>The local active state of this GameObject. (Read Only)</para>
		/// </summary>
		public extern bool activeSelf { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is the GameObject active in the scene?</para>
		/// </summary>
		public extern bool activeInHierarchy { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[Obsolete("gameObject.SetActiveRecursively() is obsolete. Use GameObject.SetActive(), which is now inherited by children.")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetActiveRecursively(bool state);

		/// <summary>
		///   <para>Editor only API that specifies if a game object is static.</para>
		/// </summary>
		public extern bool isStatic { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		internal extern bool isStaticBatchable { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The tag of this game object.</para>
		/// </summary>
		public extern string tag { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Is this game object tagged with tag ?</para>
		/// </summary>
		/// <param name="tag">The tag to compare.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool CompareTag(string tag);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern GameObject FindGameObjectWithTag(string tag);

		/// <summary>
		///   <para>Returns one active GameObject tagged tag. Returns null if no GameObject was found.</para>
		/// </summary>
		/// <param name="tag">The tag to search for.</param>
		public static GameObject FindWithTag(string tag)
		{
			return GameObject.FindGameObjectWithTag(tag);
		}

		/// <summary>
		///   <para>Returns a list of active GameObjects tagged tag. Returns empty array if no GameObject was found.</para>
		/// </summary>
		/// <param name="tag">The name of the tag to search GameObjects for.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern GameObject[] FindGameObjectsWithTag(string tag);

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
		/// </summary>
		/// <param name="methodName">The name of the method to call.</param>
		/// <param name="value">An optional parameter value to pass to the called method.</param>
		/// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SendMessageUpwards(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
		/// </summary>
		/// <param name="methodName">The name of the method to call.</param>
		/// <param name="value">An optional parameter value to pass to the called method.</param>
		/// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName, object value)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			this.SendMessageUpwards(methodName, value, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
		/// </summary>
		/// <param name="methodName">The name of the method to call.</param>
		/// <param name="value">An optional parameter value to pass to the called method.</param>
		/// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object value = null;
			this.SendMessageUpwards(methodName, value, options);
		}

		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="options"></param>
		public void SendMessageUpwards(string methodName, SendMessageOptions options)
		{
			this.SendMessageUpwards(methodName, null, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
		/// </summary>
		/// <param name="methodName">The name of the method to call.</param>
		/// <param name="value">An optional parameter value to pass to the called method.</param>
		/// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SendMessage(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
		/// </summary>
		/// <param name="methodName">The name of the method to call.</param>
		/// <param name="value">An optional parameter value to pass to the called method.</param>
		/// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
		[ExcludeFromDocs]
		public void SendMessage(string methodName, object value)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			this.SendMessage(methodName, value, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
		/// </summary>
		/// <param name="methodName">The name of the method to call.</param>
		/// <param name="value">An optional parameter value to pass to the called method.</param>
		/// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
		[ExcludeFromDocs]
		public void SendMessage(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object value = null;
			this.SendMessage(methodName, value, options);
		}

		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="options"></param>
		public void SendMessage(string methodName, SendMessageOptions options)
		{
			this.SendMessage(methodName, null, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="parameter"></param>
		/// <param name="options"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BroadcastMessage(string methodName, [DefaultValue("null")] object parameter, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="parameter"></param>
		/// <param name="options"></param>
		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName, object parameter)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			this.BroadcastMessage(methodName, parameter, options);
		}

		/// <summary>
		///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="parameter"></param>
		/// <param name="options"></param>
		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object parameter = null;
			this.BroadcastMessage(methodName, parameter, options);
		}

		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="options"></param>
		public void BroadcastMessage(string methodName, SendMessageOptions options)
		{
			this.BroadcastMessage(methodName, null, options);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Component Internal_AddComponentWithType(Type componentType);

		/// <summary>
		///   <para>Adds a component class of type componentType to the game object. C# Users can use a generic version.</para>
		/// </summary>
		/// <param name="componentType"></param>
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component AddComponent(Type componentType)
		{
			return this.Internal_AddComponentWithType(componentType);
		}

		public T AddComponent<T>() where T : Component
		{
			return this.AddComponent(typeof(T)) as T;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateGameObject([Writable] GameObject mono, string name);

		/// <summary>
		///   <para>Finds a game object by name and returns it.</para>
		/// </summary>
		/// <param name="name"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern GameObject Find(string name);

		public GameObject gameObject
		{
			get
			{
				return this;
			}
		}
	}
}
