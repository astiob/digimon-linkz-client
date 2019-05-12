using System;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Messaging
{
	internal interface ISerializationRootObject
	{
		void RootSetObjectData(SerializationInfo info, StreamingContext context);
	}
}
