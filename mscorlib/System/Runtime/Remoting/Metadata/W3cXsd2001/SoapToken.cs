using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	/// <summary>Wraps an XML token type.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapToken : ISoapXsd
	{
		private string _value;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapToken" /> class.</summary>
		public SoapToken()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapToken" /> class with an XML token.</summary>
		/// <param name="value">A <see cref="T:System.String" /> that contains an XML token. </param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">One of the following: <paramref name="value" /> contains invalid characters (0xD or 0x9).<paramref name="value" /> [0] or <paramref name="value" /> [ <paramref name="value" />.Length - 1] contains white space.<paramref name="value" /> contains any spaces. </exception>
		public SoapToken(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		/// <summary>Gets or sets an XML token.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains an XML token.</returns>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">One of the following: <paramref name="value" /> contains invalid characters (0xD or 0x9).<paramref name="value" /> [0] or <paramref name="value" /> [ <paramref name="value" />.Length - 1] contains white space.<paramref name="value" /> contains any spaces. </exception>
		public string Value
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
				return "token";
			}
		}

		/// <summary>Returns the XML Schema definition language (XSD) of the current SOAP type.</summary>
		/// <returns>A <see cref="T:System.String" /> that indicates the XSD of the current SOAP type.</returns>
		public string GetXsdType()
		{
			return SoapToken.XsdType;
		}

		/// <summary>Converts the specified <see cref="T:System.String" /> into a <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapToken" /> object.</summary>
		/// <returns>A <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapToken" /> object that is obtained from <paramref name="value" />.</returns>
		/// <param name="value">The String to convert. </param>
		public static SoapToken Parse(string value)
		{
			return new SoapToken(value);
		}

		/// <summary>Returns <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapToken.Value" /> as a <see cref="T:System.String" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that is obtained from <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapToken.Value" />.</returns>
		public override string ToString()
		{
			return this._value;
		}
	}
}
