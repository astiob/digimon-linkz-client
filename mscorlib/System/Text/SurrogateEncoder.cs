using System;
using System.Runtime.Serialization;

namespace System.Text
{
	[Serializable]
	internal sealed class SurrogateEncoder : ISerializable, IObjectReference
	{
		private Encoding encoding;

		private Encoder realObject;

		private SurrogateEncoder(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.encoding = (Encoding)info.GetValue("m_encoding", typeof(Encoding));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new ArgumentException("This class cannot be serialized.");
		}

		public object GetRealObject(StreamingContext context)
		{
			if (this.realObject == null)
			{
				this.realObject = this.encoding.GetEncoder();
			}
			return this.realObject;
		}
	}
}
