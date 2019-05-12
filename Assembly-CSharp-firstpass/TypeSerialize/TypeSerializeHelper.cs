using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TypeSerialize
{
	public static class TypeSerializeHelper
	{
		public static void BytesToData<T>(byte[] bytes, out T data)
		{
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(bytes))
				{
					try
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						data = (T)((object)binaryFormatter.Deserialize(memoryStream));
					}
					catch (Exception ex)
					{
						Debug.Log("Exception : " + ex.Message);
						throw ex;
					}
				}
			}
			catch (Exception ex2)
			{
				Debug.Log("Exception : " + ex2.Message);
				throw ex2;
			}
		}

		public static void DataToBytes<T>(T data, out byte[] bytes)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					IFormatter formatter = new BinaryFormatter();
					formatter.Serialize(memoryStream, data);
					bytes = memoryStream.ToArray();
				}
				catch (Exception ex)
				{
					Debug.Log("Exception : " + ex.Message);
					throw ex;
				}
			}
		}
	}
}
