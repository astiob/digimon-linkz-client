using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	internal static class Factories
	{
		private static Dictionary<string, Func<IUxmlAttributes, CreationContext, VisualElement>> s_Factories;

		internal static void RegisterFactory(string fullTypeName, Func<IUxmlAttributes, CreationContext, VisualElement> factory)
		{
			Factories.DiscoverFactories();
			Factories.s_Factories.Add(fullTypeName, factory);
		}

		internal static void RegisterFactory<T>(Func<IUxmlAttributes, CreationContext, VisualElement> factory) where T : VisualElement
		{
			Factories.RegisterFactory(typeof(T).FullName, factory);
		}

		private static void DiscoverFactories()
		{
			if (Factories.s_Factories == null)
			{
				Factories.s_Factories = new Dictionary<string, Func<IUxmlAttributes, CreationContext, VisualElement>>();
				CoreFactories.RegisterAll();
			}
		}

		internal static bool TryGetValue(string fullTypeName, out Func<IUxmlAttributes, CreationContext, VisualElement> factory)
		{
			Factories.DiscoverFactories();
			factory = null;
			return Factories.s_Factories != null && Factories.s_Factories.TryGetValue(fullTypeName, out factory);
		}
	}
}
