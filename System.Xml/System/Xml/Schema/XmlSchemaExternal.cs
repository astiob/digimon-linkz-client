using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>An abstract class. Provides information about the included schema.</summary>
	public abstract class XmlSchemaExternal : XmlSchemaObject
	{
		private string id;

		private XmlSchema schema;

		private string location;

		private XmlAttribute[] unhandledAttributes;

		/// <summary>Gets or sets the Uniform Resource Identifier (URI) location for the schema, which tells the schema processor where the schema physically resides.</summary>
		/// <returns>The URI location for the schema.Optional for imported schemas.</returns>
		[XmlAttribute("schemaLocation", DataType = "anyURI")]
		public string SchemaLocation
		{
			get
			{
				return this.location;
			}
			set
			{
				this.location = value;
			}
		}

		/// <summary>Gets or sets the XmlSchema for the referenced schema.</summary>
		/// <returns>The XmlSchema for the referenced schema.</returns>
		[XmlIgnore]
		public XmlSchema Schema
		{
			get
			{
				return this.schema;
			}
			set
			{
				this.schema = value;
			}
		}

		/// <summary>Gets or sets the string id.</summary>
		/// <returns>The string id. The default is String.Empty.Optional.</returns>
		[XmlAttribute("id", DataType = "ID")]
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		/// <summary>Gets and sets the qualified attributes, which do not belong to the schema target namespace.</summary>
		/// <returns>Qualified attributes that belong to another target namespace.</returns>
		[XmlAnyAttribute]
		public XmlAttribute[] UnhandledAttributes
		{
			get
			{
				if (this.unhandledAttributeList != null)
				{
					this.unhandledAttributes = (XmlAttribute[])this.unhandledAttributeList.ToArray(typeof(XmlAttribute));
					this.unhandledAttributeList = null;
				}
				return this.unhandledAttributes;
			}
			set
			{
				this.unhandledAttributes = value;
				this.unhandledAttributeList = null;
			}
		}
	}
}
