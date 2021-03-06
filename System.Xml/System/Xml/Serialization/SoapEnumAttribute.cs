﻿using System;
using System.Text;

namespace System.Xml.Serialization
{
	/// <summary>Controls how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes an enumeration member.</summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class SoapEnumAttribute : Attribute
	{
		private string name;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.SoapEnumAttribute" /> class.</summary>
		public SoapEnumAttribute()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.SoapEnumAttribute" /> class using the specified element name.</summary>
		/// <param name="name">The XML element name generated by the <see cref="T:System.Xml.Serialization.XmlSerializer" />. </param>
		public SoapEnumAttribute(string name)
		{
			this.name = name;
		}

		/// <summary>Gets or sets the value generated in an XML document when the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes an enumeration, or the value recognized when it deserializes the enumeration member.</summary>
		/// <returns>The value generated in an XML document when the <see cref="T:System.Xml.Serialization.XmlSerializer" /> serializes the enumeration, or the value recognized when it deserializes the enumeration member.</returns>
		public string Name
		{
			get
			{
				if (this.name == null)
				{
					return string.Empty;
				}
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		internal void AddKeyHash(StringBuilder sb)
		{
			sb.Append("SENA ");
			KeyHelper.AddField(sb, 1, this.name);
			sb.Append('|');
		}
	}
}
