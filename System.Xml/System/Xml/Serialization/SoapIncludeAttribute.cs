using System;

namespace System.Xml.Serialization
{
	/// <summary>Allows the <see cref="T:System.Xml.Serialization.XmlSerializer" /> to recognize a type when it serializes or deserializes an object as encoded SOAP XML.</summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true)]
	public class SoapIncludeAttribute : Attribute
	{
		private Type type;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.SoapIncludeAttribute" /> class using the specified type.</summary>
		/// <param name="type">The type of the object to include. </param>
		public SoapIncludeAttribute(Type type)
		{
			this.type = type;
		}

		/// <summary>Gets or sets the type of the object to use when serializing or deserializing an object.</summary>
		/// <returns>The type of the object to include.</returns>
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
	}
}
