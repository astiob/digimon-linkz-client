using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IUxmlAttributes
	{
		string GetPropertyString(string propertyName);

		long GetPropertyLong(string propertyName, long defaultValue);

		float GetPropertyFloat(string propertyName, float def);

		int GetPropertyInt(string propertyName, int def);

		bool GetPropertyBool(string propertyName, bool def);

		Color GetPropertyColor(string propertyName, Color def);

		T GetPropertyEnum<T>(string propertyName, T def);
	}
}
