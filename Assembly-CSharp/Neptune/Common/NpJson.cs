using JsonFx.Json;
using System;

namespace Neptune.Common
{
	public static class NpJson
	{
		public static T ConvertToObject<T>(string jsonString) where T : class
		{
			return JsonReader.Deserialize(jsonString, typeof(T)) as T;
		}

		public static string ConvertToJson(object obj)
		{
			return JsonWriter.Serialize(obj);
		}
	}
}
