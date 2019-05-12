using System;
using System.Text;

namespace System.Xml.Serialization
{
	/// <summary>Controls how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes an enumeration member.</summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class XmlEnumAttribute : Attribute
	{
		private string name;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlEnumAttribute" /> class.</summary>
		public XmlEnumAttribute()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlEnumAttribute" /> class, and specifies the XML value that the <see cref="T:System.Xml.Serialization.XmlSerializer" /> generates or recognizes (when it serializes or deserializes the enumeration, respectively).</summary>
		/// <param name="name">The overriding name of the enumeration member. </param>
		public XmlEnumAttribute(string name)
		{
			this.name = name;
		}

		/// <summary>Gets or sets the value generated in an XML-document instance when the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes an enumeration, or the value recognized when it deserializes the enumeration member.</summary>
		/// <returns>The value generated in an XML-document instance when the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes the enumeration, or the value recognized when it is deserializes the enumeration member.</returns>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		internal void AddKeyHash(StringBuilder sb)
		{
			sb.Append("XENA ");
			KeyHelper.AddField(sb, 1, this.name);
			sb.Append('|');
		}
	}
}
