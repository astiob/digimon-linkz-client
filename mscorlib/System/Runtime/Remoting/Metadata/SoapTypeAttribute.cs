using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	/// <summary>Customizes SOAP generation and processing for target types. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
	[ComVisible(true)]
	public sealed class SoapTypeAttribute : SoapAttribute
	{
		private SoapOption _soapOption;

		private bool _useAttribute;

		private string _xmlElementName;

		private XmlFieldOrderOption _xmlFieldOrder;

		private string _xmlNamespace;

		private string _xmlTypeName;

		private string _xmlTypeNamespace;

		private bool _isType;

		private bool _isElement;

		/// <summary>Gets or sets a <see cref="T:System.Runtime.Remoting.Metadata.SoapOption" /> configuration value.</summary>
		/// <returns>A <see cref="T:System.Runtime.Remoting.Metadata.SoapOption" /> value.</returns>
		public SoapOption SoapOptions
		{
			get
			{
				return this._soapOption;
			}
			set
			{
				this._soapOption = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the target of the current attribute will be serialized as an XML attribute instead of an XML field.</summary>
		/// <returns>The current implementation always returns false.</returns>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">An attempt was made to set the current property. </exception>
		public override bool UseAttribute
		{
			get
			{
				return this._useAttribute;
			}
			set
			{
				this._useAttribute = value;
			}
		}

		/// <summary>Gets or sets the XML element name.</summary>
		/// <returns>The XML element name.</returns>
		public string XmlElementName
		{
			get
			{
				return this._xmlElementName;
			}
			set
			{
				this._isElement = (value != null);
				this._xmlElementName = value;
			}
		}

		/// <summary>You should not use this property; it is not used by the .NET Framework remoting infrastructure.</summary>
		/// <returns>A <see cref="T:System.Runtime.Remoting.Metadata.XmlFieldOrderOption" />.</returns>
		public XmlFieldOrderOption XmlFieldOrder
		{
			get
			{
				return this._xmlFieldOrder;
			}
			set
			{
				this._xmlFieldOrder = value;
			}
		}

		/// <summary>Gets or sets the XML namespace that is used during serialization of the target object type.</summary>
		/// <returns>The XML namespace that is used during serialization of the target object type.</returns>
		public override string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
			set
			{
				this._isElement = (value != null);
				this._xmlNamespace = value;
			}
		}

		/// <summary>Gets or sets the XML type name for the target object type.</summary>
		/// <returns>The XML type name for the target object type.</returns>
		public string XmlTypeName
		{
			get
			{
				return this._xmlTypeName;
			}
			set
			{
				this._isType = (value != null);
				this._xmlTypeName = value;
			}
		}

		/// <summary>Gets or sets the XML type namespace for the current object type.</summary>
		/// <returns>The XML type namespace for the current object type.</returns>
		public string XmlTypeNamespace
		{
			get
			{
				return this._xmlTypeNamespace;
			}
			set
			{
				this._isType = (value != null);
				this._xmlTypeNamespace = value;
			}
		}

		internal bool IsInteropXmlElement
		{
			get
			{
				return this._isElement;
			}
		}

		internal bool IsInteropXmlType
		{
			get
			{
				return this._isType;
			}
		}

		internal override void SetReflectionObject(object reflectionObject)
		{
			Type type = (Type)reflectionObject;
			if (this._xmlElementName == null)
			{
				this._xmlElementName = type.Name;
			}
			if (this._xmlTypeName == null)
			{
				this._xmlTypeName = type.Name;
			}
			if (this._xmlTypeNamespace == null)
			{
				string assemblyName;
				if (type.Assembly == typeof(object).Assembly)
				{
					assemblyName = string.Empty;
				}
				else
				{
					assemblyName = type.Assembly.GetName().Name;
				}
				this._xmlTypeNamespace = SoapServices.CodeXmlNamespaceForClrTypeNamespace(type.Namespace, assemblyName);
			}
			if (this._xmlNamespace == null)
			{
				this._xmlNamespace = this._xmlTypeNamespace;
			}
		}
	}
}
