using System;

namespace UnityEngine
{
	public interface ISerializationCallbackReceiver
	{
		/// <summary>
		///   <para>Implement this method to receive a callback after unity serialized your object.</para>
		/// </summary>
		void OnBeforeSerialize();

		/// <summary>
		///   <para>See ISerializationCallbackReceiver.OnBeforeSerialize for documentation on how to use this method.</para>
		/// </summary>
		void OnAfterDeserialize();
	}
}
