using System;

namespace System.Xml.Schema
{
	/// <summary>Represents the post-schema-validation infoset of a validated XML node.</summary>
	[MonoTODO]
	public class XmlSchemaInfo : IXmlSchemaInfo
	{
		private bool isDefault;

		private bool isNil;

		private XmlSchemaSimpleType memberType;

		private XmlSchemaAttribute attr;

		private XmlSchemaElement elem;

		private XmlSchemaType type;

		private XmlSchemaValidity validity;

		private XmlSchemaContentType contentType;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaInfo" /> class.</summary>
		public XmlSchemaInfo()
		{
		}

		internal XmlSchemaInfo(IXmlSchemaInfo info)
		{
			this.isDefault = info.IsDefault;
			this.isNil = info.IsNil;
			this.memberType = info.MemberType;
			this.attr = info.SchemaAttribute;
			this.elem = info.SchemaElement;
			this.type = info.SchemaType;
			this.validity = info.Validity;
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Schema.XmlSchemaContentType" /> object that corresponds to the content type of this validated XML node.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaContentType" /> object.</returns>
		[MonoTODO]
		public XmlSchemaContentType ContentType
		{
			get
			{
				return this.contentType;
			}
			set
			{
				this.contentType = value;
			}
		}

		/// <summary>Gets or sets a value indicating if this validated XML node was set as the result of a default being applied during XML Schema Definition Language (XSD) schema validation.</summary>
		/// <returns>A bool value.</returns>
		[MonoTODO]
		public bool IsDefault
		{
			get
			{
				return this.isDefault;
			}
			set
			{
				this.isDefault = value;
			}
		}

		/// <summary>Gets or sets a value indicating if the value for this validated XML node is nil.</summary>
		/// <returns>A bool value.</returns>
		[MonoTODO]
		public bool IsNil
		{
			get
			{
				return this.isNil;
			}
			set
			{
				this.isNil = value;
			}
		}

		/// <summary>Gets or sets the dynamic schema type for this validated XML node.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaSimpleType" /> object.</returns>
		[MonoTODO]
		public XmlSchemaSimpleType MemberType
		{
			get
			{
				return this.memberType;
			}
			set
			{
				this.memberType = value;
			}
		}

		/// <summary>Gets or sets the compiled <see cref="T:System.Xml.Schema.XmlSchemaAttribute" /> object that corresponds to this validated XML node.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaAttribute" /> object.</returns>
		[MonoTODO]
		public XmlSchemaAttribute SchemaAttribute
		{
			get
			{
				return this.attr;
			}
			set
			{
				this.attr = value;
			}
		}

		/// <summary>Gets or sets the compiled <see cref="T:System.Xml.Schema.XmlSchemaElement" /> object that corresponds to this validated XML node.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaElement" /> object.</returns>
		[MonoTODO]
		public XmlSchemaElement SchemaElement
		{
			get
			{
				return this.elem;
			}
			set
			{
				this.elem = value;
			}
		}

		/// <summary>Gets or sets the static XML Schema Definition Language (XSD) schema type of this validated XML node.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaType" /> object.</returns>
		[MonoTODO]
		public XmlSchemaType SchemaType
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Schema.XmlSchemaValidity" /> value of this validated XML node.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaValidity" /> value.</returns>
		[MonoTODO]
		public XmlSchemaValidity Validity
		{
			get
			{
				return this.validity;
			}
			set
			{
				this.validity = value;
			}
		}
	}
}
