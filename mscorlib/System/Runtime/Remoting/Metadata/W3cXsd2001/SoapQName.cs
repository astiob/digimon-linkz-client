using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	/// <summary>Wraps an XSD QName type.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapQName : ISoapXsd
	{
		private string _name;

		private string _key;

		private string _namespace;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName" /> class.</summary>
		public SoapQName()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName" /> class with the local part of a qualified name.</summary>
		/// <param name="value">A <see cref="T:System.String" /> that contains the local part of a qualified name. </param>
		public SoapQName(string value)
		{
			this._name = value;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName" /> class with the namespace alias and the local part of a qualified name.</summary>
		/// <param name="key">A <see cref="T:System.String" /> that contains the namespace alias of a qualified name. </param>
		/// <param name="name">A <see cref="T:System.String" /> that contains the local part of a qualified name. </param>
		public SoapQName(string key, string name)
		{
			this._key = key;
			this._name = name;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName" /> class with the namespace alias, the local part of a qualified name, and the namespace that is referenced by the alias.</summary>
		/// <param name="key">A <see cref="T:System.String" /> that contains the namespace alias of a qualified name. </param>
		/// <param name="name">A <see cref="T:System.String" /> that contains the local part of a qualified name. </param>
		/// <param name="namespaceValue">A <see cref="T:System.String" /> that contains the namespace that is referenced by <paramref name="key" />. </param>
		public SoapQName(string key, string name, string namespaceValue)
		{
			this._key = key;
			this._name = name;
			this._namespace = namespaceValue;
		}

		/// <summary>Gets or sets the namespace alias of a qualified name.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the namespace alias of a qualified name.</returns>
		public string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}

		/// <summary>Gets or sets the name portion of a qualified name.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the name portion of a qualified name.</returns>
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		/// <summary>Gets or sets the namespace that is referenced by <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName.Key" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the namespace that is referenced by <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName.Key" />.</returns>
		public string Namespace
		{
			get
			{
				return this._namespace;
			}
			set
			{
				this._namespace = value;
			}
		}

		/// <summary>Gets the XML Schema definition language (XSD) of the current SOAP type.</summary>
		/// <returns>A <see cref="T:System.String" /> that indicates the XSD of the current SOAP type.</returns>
		public static string XsdType
		{
			get
			{
				return "QName";
			}
		}

		/// <summary>Returns the XML Schema definition language (XSD) of the current SOAP type.</summary>
		/// <returns>A <see cref="T:System.String" /> indicating the XSD of the current SOAP type.</returns>
		public string GetXsdType()
		{
			return SoapQName.XsdType;
		}

		/// <summary>Converts the specified <see cref="T:System.String" /> into a <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName" /> object.</summary>
		/// <returns>A <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName" /> object that is obtained from <paramref name="value" />.</returns>
		/// <param name="value">The <see cref="T:System.String" /> to convert. </param>
		public static SoapQName Parse(string value)
		{
			SoapQName soapQName = new SoapQName();
			int num = value.IndexOf(':');
			if (num != -1)
			{
				soapQName.Key = value.Substring(0, num);
				soapQName.Name = value.Substring(num + 1);
			}
			else
			{
				soapQName.Name = value;
			}
			return soapQName;
		}

		/// <summary>Returns the qualified name as a <see cref="T:System.String" />.</summary>
		/// <returns>A <see cref="T:System.String" /> in the format " <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName.Key" /> : <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName.Name" /> ". If <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName.Key" /> is not specified, this method returns <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapQName.Name" />.</returns>
		public override string ToString()
		{
			if (this._key == null || this._key == string.Empty)
			{
				return this._name;
			}
			return this._key + ":" + this._name;
		}
	}
}
