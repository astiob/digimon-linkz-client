using JsonFx.Json;
using System;

public class TCPData<T> where T : class
{
	public static T Convert(object obj)
	{
		string value = JsonWriter.Serialize(obj);
		return (T)((object)JsonReader.Deserialize(value, typeof(T)));
	}
}
