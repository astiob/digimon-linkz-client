using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class UnityExtensionMethod
{
	private const string SEPARATOR = ",";

	private const string FORMAT = "{0}:{1}";

	public static void TryInstantiate(this GameObject original, Action<GameObject> action)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		if (gameObject != null)
		{
			action(gameObject);
		}
	}

	public static void TryFind(this Transform transform, string name, Action<Transform> action)
	{
		Transform transform2 = transform.Find(name);
		if (transform2 != null)
		{
			action(transform2);
		}
	}

	public static void TryGetComponent<TComponentT>(this Component component, Action<TComponentT> action) where TComponentT : Component
	{
		TComponentT component2 = component.GetComponent<TComponentT>();
		if (component2 != null)
		{
			action(component2);
		}
	}

	public static void TryGetComponentInChildren<TComponentT>(this Component component, Action<TComponentT> action) where TComponentT : Component
	{
		TComponentT componentInChildren = component.GetComponentInChildren<TComponentT>();
		if (componentInChildren != null)
		{
			action(componentInChildren);
		}
	}

	public static void TryGetComponentsInChildren<TComponentT>(this Component component, Action<TComponentT[]> action) where TComponentT : Component
	{
		TComponentT[] componentsInChildren = component.GetComponentsInChildren<TComponentT>();
		if (componentsInChildren != null)
		{
			action(componentsInChildren);
		}
	}

	public static void TryGetComponent<TComponentT>(this GameObject go, Action<TComponentT> action) where TComponentT : Component
	{
		TComponentT component = go.GetComponent<TComponentT>();
		if (component != null)
		{
			action(component);
		}
	}

	public static void TryGetComponentInChildren<TComponentT>(this GameObject go, Action<TComponentT> action) where TComponentT : Component
	{
		TComponentT componentInChildren = go.GetComponentInChildren<TComponentT>();
		if (componentInChildren != null)
		{
			action(componentInChildren);
		}
	}

	public static void TryGetComponent<TComponentT>(this Transform t, Action<TComponentT> action) where TComponentT : Component
	{
		TComponentT component = t.GetComponent<TComponentT>();
		if (component != null)
		{
			action(component);
		}
	}

	public static void TryGetComponentInChildren<TComponentT>(this Transform t, Action<TComponentT> action) where TComponentT : Component
	{
		TComponentT componentInChildren = t.GetComponentInChildren<TComponentT>();
		if (componentInChildren != null)
		{
			action(componentInChildren);
		}
	}

	public static void AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, TValue value)
	{
		if (self.ContainsKey(key))
		{
			self[key] = value;
		}
		else
		{
			self.Add(key, value);
		}
	}

	public static bool TryAdd<T>(this IList<T> self, T value)
	{
		if (self.Contains(value))
		{
			return false;
		}
		self.Add(value);
		return true;
	}

	public static void SetLayer(this GameObject gameObject, int layerNo, bool setChildrens = true)
	{
		if (gameObject == null)
		{
			return;
		}
		gameObject.layer = layerNo;
		if (!setChildrens)
		{
			return;
		}
		IEnumerator enumerator = gameObject.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.gameObject.SetLayer(layerNo, true);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
	{
		T t = gameObject.GetComponent<T>();
		if (t == null)
		{
			t = gameObject.AddComponent<T>();
		}
		return t;
	}

	public static string ToStringFields<T>(this T obj)
	{
		return string.Join(",", obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).Select((FieldInfo c) => string.Format("{0}:{1}", c.Name, c.GetValue(obj))).ToArray<string>());
	}

	public static string ToStringProperties<T>(this T obj)
	{
		return string.Join(",", obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where((PropertyInfo c) => c.CanRead).Select((PropertyInfo c) => string.Format("{0}:{1}", c.Name, c.GetValue(obj, null))).ToArray<string>());
	}

	public static string ToStringReflection<T>(this T obj)
	{
		return string.Format("{0},{1}", obj.ToStringFields<T>(), obj.ToStringProperties<T>());
	}
}
