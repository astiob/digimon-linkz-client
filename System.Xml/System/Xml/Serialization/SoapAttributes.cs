using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace System.Xml.Serialization
{
	/// <summary>Represents a collection of attribute objects that control how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes and deserializes SOAP methods.</summary>
	public class SoapAttributes
	{
		private SoapAttributeAttribute soapAttribute;

		private object soapDefaultValue = DBNull.Value;

		private SoapElementAttribute soapElement;

		private SoapEnumAttribute soapEnum;

		private bool soapIgnore;

		private SoapTypeAttribute soapType;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.SoapAttributes" /> class.</summary>
		public SoapAttributes()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.SoapAttributes" /> class using the specified custom type.</summary>
		/// <param name="provider">Any object that implements the <see cref="T:System.Reflection.ICustomAttributeProvider" /> interface, such as the <see cref="T:System.Type" /> class. </param>
		public SoapAttributes(ICustomAttributeProvider provider)
		{
			object[] customAttributes = provider.GetCustomAttributes(false);
			foreach (object obj in customAttributes)
			{
				if (obj is SoapAttributeAttribute)
				{
					this.soapAttribute = (SoapAttributeAttribute)obj;
				}
				else if (obj is DefaultValueAttribute)
				{
					this.soapDefaultValue = ((DefaultValueAttribute)obj).Value;
				}
				else if (obj is SoapElementAttribute)
				{
					this.soapElement = (SoapElementAttribute)obj;
				}
				else if (obj is SoapEnumAttribute)
				{
					this.soapEnum = (SoapEnumAttribute)obj;
				}
				else if (obj is SoapIgnoreAttribute)
				{
					this.soapIgnore = true;
				}
				else if (obj is SoapTypeAttribute)
				{
					this.soapType = (SoapTypeAttribute)obj;
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Serialization.SoapAttributeAttribute" /> to override.</summary>
		/// <returns>A <see cref="T:System.Xml.Serialization.SoapAttributeAttribute" /> that overrides the behavior of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> when the member is serialized.</returns>
		public SoapAttributeAttribute SoapAttribute
		{
			get
			{
				return this.soapAttribute;
			}
			set
			{
				this.soapAttribute = value;
			}
		}

		/// <summary>Gets or sets the default value of an XML element or attribute.</summary>
		/// <returns>An object that represents the default value of an XML element or attribute.</returns>
		public object SoapDefaultValue
		{
			get
			{
				return this.soapDefaultValue;
			}
			set
			{
				this.soapDefaultValue = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Xml.Serialization.SoapElementAttribute" /> to override.</summary>
		/// <returns>The <see cref="T:System.Xml.Serialization.SoapElementAttribute" /> to override.</returns>
		public SoapElementAttribute SoapElement
		{
			get
			{
				return this.soapElement;
			}
			set
			{
				this.soapElement = value;
			}
		}

		/// <summary>Gets or sets an object that specifies how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes a SOAP enumeration.</summary>
		/// <returns>A <see cref="T:System.Xml.Serialization.SoapEnumAttribute" />.</returns>
		public SoapEnumAttribute SoapEnum
		{
			get
			{
				return this.soapEnum;
			}
			set
			{
				this.soapEnum = value;
			}
		}

		/// <summary>Gets or sets a value that specifies whether the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes a public field or property as encoded SOAP XML.</summary>
		/// <returns>true if the <see cref="T:System.Xml.Serialization.XmlSerializer" /> must not serialize the field or property; otherwise, false.</returns>
		public bool SoapIgnore
		{
			get
			{
				return this.soapIgnore;
			}
			set
			{
				this.soapIgnore = value;
			}
		}

		/// <summary>Gets or sets an object that instructs the <see cref="T:System.Xml.Serialization.XmlSerializer" /> how to serialize an object type into encoded SOAP XML.</summary>
		/// <returns>A <see cref="T:System.Xml.Serialization.SoapTypeAttribute" /> that either overrides a <see cref="T:System.Xml.Serialization.SoapTypeAttribute" /> applied to a class declaration, or is applied to a class declaration.</returns>
		public SoapTypeAttribute SoapType
		{
			get
			{
				return this.soapType;
			}
			set
			{
				this.soapType = value;
			}
		}

		internal void AddKeyHash(StringBuilder sb)
		{
			sb.Append("SA ");
			if (this.soapIgnore)
			{
				sb.Append('i');
			}
			if (this.soapAttribute != null)
			{
				this.soapAttribute.AddKeyHash(sb);
			}
			if (this.soapElement != null)
			{
				this.soapElement.AddKeyHash(sb);
			}
			if (this.soapEnum != null)
			{
				this.soapEnum.AddKeyHash(sb);
			}
			if (this.soapType != null)
			{
				this.soapType.AddKeyHash(sb);
			}
			if (this.soapDefaultValue == null)
			{
				sb.Append("n");
			}
			else if (!(this.soapDefaultValue is DBNull))
			{
				string str = XmlCustomFormatter.ToXmlString(TypeTranslator.GetTypeData(this.soapDefaultValue.GetType()), this.soapDefaultValue);
				sb.Append("v" + str);
			}
			sb.Append("|");
		}
	}
}
