using System;

namespace UnityEngine.Experimental.UIElements
{
	internal interface ISerializableJsonDictionary
	{
		void Set<T>(string key, T value) where T : class;

		T Get<T>(string key) where T : class;

		T GetScriptable<T>(string key) where T : ScriptableObject;

		void Overwrite(object obj, string key);

		bool ContainsKey(string key);

		void OnBeforeSerialize();

		void OnAfterDeserialize();
	}
}
