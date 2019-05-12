using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	/// <summary>Wraps an XSD base64Binary type. </summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapBase64Binary : ISoapXsd
	{
		private byte[] _value;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapBase64Binary" /> class.</summary>
		public SoapBase64Binary()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapBase64Binary" /> class with the binary representation of a 64-bit number.</summary>
		/// <param name="value">A <see cref="T:System.Byte" /> array that contains a 64-bit number. </param>
		public SoapBase64Binary(byte[] value)
		{
			this._value = value;
		}

		/// <summary>Gets or sets the binary representation of a 64-bit number.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array that contains the binary representation of a 64-bit number.</returns>
		public byte[] Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		/// <summary>Gets the XML Schema definition language (XSD) of the current SOAP type.</summary>
		/// <returns>A <see cref="T:System.String" /> that indicates the XSD of the current SOAP type.</returns>
		public static string XsdType
		{
			get
			{
				return "base64Binary";
			}
		}

		/// <summary>Returns the XML Schema definition language (XSD) of the current SOAP type.</summary>
		/// <returns>A <see cref="T:System.String" /> that indicates the XSD of the current SOAP type.</returns>
		public string GetXsdType()
		{
			return SoapBase64Binary.XsdType;
		}

		/// <summary>Converts the specified <see cref="T:System.String" /> into a <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapBase64Binary" /> object.</summary>
		/// <returns>A <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapBase64Binary" /> object that is obtained from <paramref name="value" />.</returns>
		/// <param name="value">The String to convert. </param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">One of the following:<paramref name="value" /> is null. The length of <paramref name="value" /> is less than 4.The length of <paramref name="value" /> is not a multiple of 4. </exception>
		public static SoapBase64Binary Parse(string value)
		{
			return new SoapBase64Binary(Convert.FromBase64String(value));
		}

		/// <summary>Returns <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapBase64Binary.Value" /> as a <see cref="T:System.String" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that is obtained from <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapBase64Binary.Value" />.</returns>
		public override string ToString()
		{
			return Convert.ToBase64String(this._value);
		}
	}
}
