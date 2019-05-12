using System;

namespace System.Xml.Serialization
{
	/// <summary>Allows the <see cref="T:System.Xml.Serialization.XmlSerializer" /> to recognize a type when it serializes or deserializes an object.</summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true)]
	public class XmlIncludeAttribute : Attribute
	{
		private Type type;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlIncludeAttribute" /> class.</summary>
		/// <param name="type">The <see cref="T:System.Type" /> of the object to include. </param>
		public XmlIncludeAttribute(Type type)
		{
			this.type = type;
		}

		/// <summary>Gets or sets the type of the object to include.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the object to include.</returns>
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
