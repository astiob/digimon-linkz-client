using System;
using System.Runtime.Serialization;

namespace System.Text
{
	/// <summary>Represents a code page encoding.</summary>
	[Serializable]
	internal sealed class MLangCodePageEncoding : ISerializable, IObjectReference
	{
		private int codePage;

		private bool isReadOnly;

		private EncoderFallback encoderFallback;

		private DecoderFallback decoderFallback;

		private Encoding realObject;

		private MLangCodePageEncoding(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.codePage = (int)info.GetValue("m_codePage", typeof(int));
			try
			{
				this.isReadOnly = (bool)info.GetValue("m_isReadOnly", typeof(bool));
				this.encoderFallback = (EncoderFallback)info.GetValue("encoderFallback", typeof(EncoderFallback));
				this.decoderFallback = (DecoderFallback)info.GetValue("decoderFallback", typeof(DecoderFallback));
			}
			catch (SerializationException)
			{
				this.isReadOnly = true;
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new ArgumentException("This class cannot be serialized.");
		}

		public object GetRealObject(StreamingContext context)
		{
			if (this.realObject == null)
			{
				Encoding encoding = Encoding.GetEncoding(this.codePage);
				if (!this.isReadOnly)
				{
					encoding = (Encoding)encoding.Clone();
					encoding.EncoderFallback = this.encoderFallback;
					encoding.DecoderFallback = this.decoderFallback;
				}
				this.realObject = encoding;
			}
			return this.realObject;
		}

		[Serializable]
		private sealed class MLangEncoder : ISerializable, IObjectReference
		{
			private Encoding encoding;

			private Encoder realObject;

			private MLangEncoder(SerializationInfo info, StreamingContext context)
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

		[Serializable]
		private sealed class MLangDecoder : ISerializable, IObjectReference
		{
			private Encoding encoding;

			private Decoder realObject;

			private MLangDecoder(SerializationInfo info, StreamingContext context)
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
					this.realObject = this.encoding.GetDecoder();
				}
				return this.realObject;
			}
		}
	}
}
