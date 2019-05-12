using System;
using System.Runtime.Serialization;

namespace System.Text
{
	[Serializable]
	internal sealed class CodePageEncoding : ISerializable, IObjectReference
	{
		private int codePage;

		private bool isReadOnly;

		private EncoderFallback encoderFallback;

		private DecoderFallback decoderFallback;

		private Encoding realObject;

		private CodePageEncoding(SerializationInfo info, StreamingContext context)
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
		private sealed class Decoder : ISerializable, IObjectReference
		{
			private Encoding encoding;

			private System.Text.Decoder realObject;

			private Decoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
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
