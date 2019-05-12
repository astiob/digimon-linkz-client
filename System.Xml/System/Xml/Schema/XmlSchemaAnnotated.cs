using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>The base class for any element that can contain annotation elements.</summary>
	public class XmlSchemaAnnotated : XmlSchemaObject
	{
		private XmlSchemaAnnotation annotation;

		private string id;

		private XmlAttribute[] unhandledAttributes;

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

		/// <summary>Gets or sets the annotation property.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaAnnotation" /> representing the annotation property.</returns>
		[XmlElement("annotation", Type = typeof(XmlSchemaAnnotation))]
		public XmlSchemaAnnotation Annotation
		{
			get
			{
				return this.annotation;
			}
			set
			{
				this.annotation = value;
			}
		}

		/// <summary>Gets or sets the qualified attributes that do not belong to the current schema's target namespace.</summary>
		/// <returns>An array of qualified <see cref="T:System.Xml.XmlAttribute" /> objects that do not belong to the schema's target namespace.</returns>
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
