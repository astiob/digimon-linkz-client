using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace System.Xml.Serialization
{
	/// <summary>Represents a collection of attribute objects that control how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes and deserializes an object.</summary>
	public class XmlAttributes
	{
		private XmlAnyAttributeAttribute xmlAnyAttribute;

		private XmlAnyElementAttributes xmlAnyElements = new XmlAnyElementAttributes();

		private XmlArrayAttribute xmlArray;

		private XmlArrayItemAttributes xmlArrayItems = new XmlArrayItemAttributes();

		private XmlAttributeAttribute xmlAttribute;

		private XmlChoiceIdentifierAttribute xmlChoiceIdentifier;

		private object xmlDefaultValue = DBNull.Value;

		private XmlElementAttributes xmlElements = new XmlElementAttributes();

		private XmlEnumAttribute xmlEnum;

		private bool xmlIgnore;

		private bool xmlns;

		private XmlRootAttribute xmlRoot;

		private XmlTextAttribute xmlText;

		private XmlTypeAttribute xmlType;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlAttributes" /> class.</summary>
		public XmlAttributes()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlAttributes" /> class and customizes how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes and deserializes an object. </summary>
		/// <param name="provider">A class that can provide alternative implementations of attributes that control XML serialization.</param>
		public XmlAttributes(ICustomAttributeProvider provider)
		{
			object[] customAttributes = provider.GetCustomAttributes(false);
			foreach (object obj in customAttributes)
			{
				if (obj is XmlAnyAttributeAttribute)
				{
					this.xmlAnyAttribute = (XmlAnyAttributeAttribute)obj;
				}
				else if (obj is XmlAnyElementAttribute)
				{
					this.xmlAnyElements.Add((XmlAnyElementAttribute)obj);
				}
				else if (obj is XmlArrayAttribute)
				{
					this.xmlArray = (XmlArrayAttribute)obj;
				}
				else if (obj is XmlArrayItemAttribute)
				{
					this.xmlArrayItems.Add((XmlArrayItemAttribute)obj);
				}
				else if (obj is XmlAttributeAttribute)
				{
					this.xmlAttribute = (XmlAttributeAttribute)obj;
				}
				else if (obj is XmlChoiceIdentifierAttribute)
				{
					this.xmlChoiceIdentifier = (XmlChoiceIdentifierAttribute)obj;
				}
				else if (obj is DefaultValueAttribute)
				{
					this.xmlDefaultValue = ((DefaultValueAttribute)obj).Value;
				}
				else if (obj is XmlElementAttribute)
				{
					this.xmlElements.Add((XmlElementAttribute)obj);
				}
				else if (obj is XmlEnumAttribute)
				{
					this.xmlEnum = (XmlEnumAttribute)obj;
				}
				else if (obj is XmlIgnoreAttribute)
				{
					this.xmlIgnore = true;
				}
				else if (obj is XmlNamespaceDeclarationsAttribute)
				{
					this.xmlns = true;
				}
				else if (obj is XmlRootAttribute)
				{
					this.xmlRoot = (XmlRootAttribute)obj;
				}
				else if (obj is XmlTextAttribute)
				{
					this.xmlText = (XmlTextAttribute)obj;
				}
				else if (obj is XmlTypeAttribute)
				{
					this.xmlType = (XmlTypeAttribute)obj;
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Serialization.XmlAnyAttributeAttribute" /> to override.</summary>
		/// <returns>The <see cref="T:System.Xml.Serialization.XmlAnyAttributeAttribute" /> to override.</returns>
		public XmlAnyAttributeAttribute XmlAnyAttribute
		{
			get
			{
				return this.xmlAnyAttribute;
			}
			set
			{
				this.xmlAnyAttribute = value;
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> objects to override.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlAnyElementAttributes" /> object that represents the collection of <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> objects.</returns>
		public XmlAnyElementAttributes XmlAnyElements
		{
			get
			{
				return this.xmlAnyElements;
			}
		}

		/// <summary>Gets or sets an object that specifies how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes a public field or read/write property that returns an array.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlArrayAttribute" /> that specifies how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes a public field or read/write property that returns an array.</returns>
		public XmlArrayAttribute XmlArray
		{
			get
			{
				return this.xmlArray;
			}
			set
			{
				this.xmlArray = value;
			}
		}

		/// <summary>Gets or sets a collection of objects that specify how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes items inserted into an array returned by a public field or read/write property.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlArrayItemAttributes" /> object that contains a collection of <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> objects.</returns>
		public XmlArrayItemAttributes XmlArrayItems
		{
			get
			{
				return this.xmlArrayItems;
			}
		}

		/// <summary>Gets or sets an object that specifies how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes a public field or public read/write property as an XML attribute.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlAttributeAttribute" /> that controls the serialization of a public field or read/write property as an XML attribute.</returns>
		public XmlAttributeAttribute XmlAttribute
		{
			get
			{
				return this.xmlAttribute;
			}
			set
			{
				this.xmlAttribute = value;
			}
		}

		/// <summary>Gets or sets an object that allows you to distinguish between a set of choices.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlChoiceIdentifierAttribute" /> that can be applied to a class member that is serialized as an xsi:choice element.</returns>
		public XmlChoiceIdentifierAttribute XmlChoiceIdentifier
		{
			get
			{
				return this.xmlChoiceIdentifier;
			}
		}

		/// <summary>Gets or sets the default value of an XML element or attribute.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the default value of an XML element or attribute.</returns>
		public object XmlDefaultValue
		{
			get
			{
				return this.xmlDefaultValue;
			}
			set
			{
				this.xmlDefaultValue = value;
			}
		}

		/// <summary>Gets a collection of objects that specify how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes a public field or read/write property as an XML element.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlElementAttributes" /> that contains a collection of <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> objects.</returns>
		public XmlElementAttributes XmlElements
		{
			get
			{
				return this.xmlElements;
			}
		}

		/// <summary>Gets or sets an object that specifies how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes an enumeration member.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlEnumAttribute" /> that specifies how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes an enumeration member.</returns>
		public XmlEnumAttribute XmlEnum
		{
			get
			{
				return this.xmlEnum;
			}
			set
			{
				this.xmlEnum = value;
			}
		}

		/// <summary>Gets or sets a value that specifies whether or not the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes a public field or public read/write property.</summary>
		/// <returns>true if the <see cref="T:System.Xml.Serialization.XmlSerializer" /> must not serialize the field or property; otherwise, false.</returns>
		public bool XmlIgnore
		{
			get
			{
				return this.xmlIgnore;
			}
			set
			{
				this.xmlIgnore = value;
			}
		}

		/// <summary>Gets or sets a value that specifies whether to keep all namespace declarations when an object containing a member that returns an <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> object is overridden.</summary>
		/// <returns>true if the namespace declarations should be kept; otherwise, false.</returns>
		public bool Xmlns
		{
			get
			{
				return this.xmlns;
			}
			set
			{
				this.xmlns = value;
			}
		}

		/// <summary>Gets or sets an object that specifies how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes a class as an XML root element.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlRootAttribute" /> that overrides a class attributed as an XML root element.</returns>
		public XmlRootAttribute XmlRoot
		{
			get
			{
				return this.xmlRoot;
			}
			set
			{
				this.xmlRoot = value;
			}
		}

		/// <summary>Gets or sets an object that instructs the <see cref="T:System.Xml.Serialization.XmlSerializer" /> to serialize a public field or public read/write property as XML text.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlTextAttribute" /> that overrides the default serialization of a public property or field.</returns>
		public XmlTextAttribute XmlText
		{
			get
			{
				return this.xmlText;
			}
			set
			{
				this.xmlText = value;
			}
		}

		/// <summary>Gets or sets an object that specifies how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes a class to which the <see cref="T:System.Xml.Serialization.XmlTypeAttribute" /> has been applied.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlTypeAttribute" /> that overrides an <see cref="T:System.Xml.Serialization.XmlTypeAttribute" /> applied to a class declaration.</returns>
		public XmlTypeAttribute XmlType
		{
			get
			{
				return this.xmlType;
			}
			set
			{
				this.xmlType = value;
			}
		}

		internal void AddKeyHash(StringBuilder sb)
		{
			sb.Append("XA ");
			KeyHelper.AddField(sb, 1, this.xmlIgnore);
			KeyHelper.AddField(sb, 2, this.xmlns);
			KeyHelper.AddField(sb, 3, this.xmlAnyAttribute != null);
			this.xmlAnyElements.AddKeyHash(sb);
			this.xmlArrayItems.AddKeyHash(sb);
			this.xmlElements.AddKeyHash(sb);
			if (this.xmlArray != null)
			{
				this.xmlArray.AddKeyHash(sb);
			}
			if (this.xmlAttribute != null)
			{
				this.xmlAttribute.AddKeyHash(sb);
			}
			if (this.xmlDefaultValue == null)
			{
				sb.Append("n");
			}
			else if (!(this.xmlDefaultValue is DBNull))
			{
				string str = XmlCustomFormatter.ToXmlString(TypeTranslator.GetTypeData(this.xmlDefaultValue.GetType()), this.xmlDefaultValue);
				sb.Append("v" + str);
			}
			if (this.xmlEnum != null)
			{
				this.xmlEnum.AddKeyHash(sb);
			}
			if (this.xmlRoot != null)
			{
				this.xmlRoot.AddKeyHash(sb);
			}
			if (this.xmlText != null)
			{
				this.xmlText.AddKeyHash(sb);
			}
			if (this.xmlType != null)
			{
				this.xmlType.AddKeyHash(sb);
			}
			if (this.xmlChoiceIdentifier != null)
			{
				this.xmlChoiceIdentifier.AddKeyHash(sb);
			}
			sb.Append("|");
		}
	}
}
