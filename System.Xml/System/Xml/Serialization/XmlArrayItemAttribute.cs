using System;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	/// <summary>Specifies the derived types that the <see cref="T:System.Xml.Serialization.XmlSerializer" /> can place in a serialized array.</summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
	public class XmlArrayItemAttribute : Attribute
	{
		private string dataType;

		private string elementName;

		private XmlSchemaForm form;

		private string ns;

		private bool isNullable;

		private bool isNullableSpecified;

		private int nestingLevel;

		private Type type;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> class.</summary>
		public XmlArrayItemAttribute()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> class and specifies the name of the XML element generated in the XML document.</summary>
		/// <param name="elementName">The name of the XML element. </param>
		public XmlArrayItemAttribute(string elementName)
		{
			this.elementName = elementName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> class and specifies the <see cref="T:System.Type" /> that can be inserted into the serialized array.</summary>
		/// <param name="type">The <see cref="T:System.Type" /> of the object to serialize. </param>
		public XmlArrayItemAttribute(Type type)
		{
			this.type = type;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> class and specifies the name of the XML element generated in the XML document and the <see cref="T:System.Type" /> that can be inserted into the generated XML document.</summary>
		/// <param name="elementName">The name of the XML element. </param>
		/// <param name="type">The <see cref="T:System.Type" /> of the object to serialize. </param>
		public XmlArrayItemAttribute(string elementName, Type type)
		{
			this.elementName = elementName;
			this.type = type;
		}

		/// <summary>Gets or sets the XML data type of the generated XML element.</summary>
		/// <returns>An XML Schema definition (XSD) data type, as defined by the World Wide Web Consortium (www.w3.org) document "XML Schema Part 2: DataTypes".</returns>
		public string DataType
		{
			get
			{
				if (this.dataType == null)
				{
					return string.Empty;
				}
				return this.dataType;
			}
			set
			{
				this.dataType = value;
			}
		}

		/// <summary>Gets or sets the name of the generated XML element.</summary>
		/// <returns>The name of the generated XML element. The default is the member identifier.</returns>
		public string ElementName
		{
			get
			{
				if (this.elementName == null)
				{
					return string.Empty;
				}
				return this.elementName;
			}
			set
			{
				this.elementName = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the name of the generated XML element is qualified.</summary>
		/// <returns>One of the <see cref="T:System.Xml.Schema.XmlSchemaForm" /> values. The default is XmlSchemaForm.None.</returns>
		/// <exception cref="T:System.Exception">The <see cref="P:System.Xml.Serialization.XmlArrayItemAttribute.Form" /> property is set to XmlSchemaForm.Unqualified and a <see cref="P:System.Xml.Serialization.XmlArrayItemAttribute.Namespace" /> value is specified. </exception>
		public XmlSchemaForm Form
		{
			get
			{
				return this.form;
			}
			set
			{
				this.form = value;
			}
		}

		/// <summary>Gets or sets the namespace of the generated XML element.</summary>
		/// <returns>The namespace of the generated XML element.</returns>
		public string Namespace
		{
			get
			{
				return this.ns;
			}
			set
			{
				this.ns = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Xml.Serialization.XmlSerializer" /> must serialize a member as an empty XML tag with the xsi:nil attribute set to true.</summary>
		/// <returns>true if the <see cref="T:System.Xml.Serialization.XmlSerializer" /> generates the xsi:nil attribute; otherwise, false, and no instance is generated. The default is true.</returns>
		public bool IsNullable
		{
			get
			{
				return this.isNullable;
			}
			set
			{
				this.isNullableSpecified = true;
				this.isNullable = value;
			}
		}

		internal bool IsNullableSpecified
		{
			get
			{
				return this.isNullableSpecified;
			}
		}

		/// <summary>Gets or sets the type allowed in an array.</summary>
		/// <returns>A <see cref="T:System.Type" /> that is allowed in the array.</returns>
		public Type Type
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

		/// <summary>Gets or sets the level in a hierarchy of XML elements that the <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> affects.</summary>
		/// <returns>The zero-based index of a set of indexes in an array of arrays.</returns>
		public int NestingLevel
		{
			get
			{
				return this.nestingLevel;
			}
			set
			{
				this.nestingLevel = value;
			}
		}

		internal void AddKeyHash(StringBuilder sb)
		{
			sb.Append("XAIA ");
			KeyHelper.AddField(sb, 1, this.ns);
			KeyHelper.AddField(sb, 2, this.elementName);
			KeyHelper.AddField(sb, 3, this.form.ToString(), XmlSchemaForm.None.ToString());
			KeyHelper.AddField(sb, 4, this.isNullable, true);
			KeyHelper.AddField(sb, 5, this.dataType);
			KeyHelper.AddField(sb, 6, this.nestingLevel, 0);
			KeyHelper.AddField(sb, 7, this.type);
			sb.Append('|');
		}
	}
}
