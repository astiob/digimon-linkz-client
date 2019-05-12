using System;
using System.Text;

namespace System.Xml.Serialization
{
	/// <summary>Controls the XML schema that is generated when the attribute target is serialized by the <see cref="T:System.Xml.Serialization.XmlSerializer" />.</summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
	public class XmlTypeAttribute : Attribute
	{
		private bool includeInSchema = true;

		private string ns;

		private string typeName;

		private bool anonymousType;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlTypeAttribute" /> class.</summary>
		public XmlTypeAttribute()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlTypeAttribute" /> class and specifies the name of the XML type.</summary>
		/// <param name="typeName">The name of the XML type that the <see cref="T:System.Xml.Serialization.XmlSerializer" /> generates when it serializes the class instance (and recognizes when it deserializes the class instance). </param>
		public XmlTypeAttribute(string typeName)
		{
			this.typeName = typeName;
		}

		/// <summary>Gets or sets a value that determines whether the resulting schema type is an XSD anonymous type.</summary>
		/// <returns>true, if the resulting schema type is an XSD anonymous type; otherwise, false.</returns>
		public bool AnonymousType
		{
			get
			{
				return this.anonymousType;
			}
			set
			{
				this.anonymousType = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to include the type in XML schema documents.</summary>
		/// <returns>true to include the type in XML schema documents; otherwise, false.</returns>
		public bool IncludeInSchema
		{
			get
			{
				return this.includeInSchema;
			}
			set
			{
				this.includeInSchema = value;
			}
		}

		/// <summary>Gets or sets the namespace of the XML type.</summary>
		/// <returns>The namespace of the XML type.</returns>
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

		/// <summary>Gets or sets the name of the XML type.</summary>
		/// <returns>The name of the XML type.</returns>
		public string TypeName
		{
			get
			{
				if (this.typeName == null)
				{
					return string.Empty;
				}
				return this.typeName;
			}
			set
			{
				this.typeName = value;
			}
		}

		internal void AddKeyHash(StringBuilder sb)
		{
			sb.Append("XTA ");
			KeyHelper.AddField(sb, 1, this.ns);
			KeyHelper.AddField(sb, 2, this.typeName);
			KeyHelper.AddField(sb, 4, this.includeInSchema);
			sb.Append('|');
		}
	}
}
