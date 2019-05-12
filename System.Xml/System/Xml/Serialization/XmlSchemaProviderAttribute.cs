using System;

namespace System.Xml.Serialization
{
	/// <summary>When applied to a type, stores the name of a static method of the type that returns an XML schema and a <see cref="T:System.Xml.XmlQualifiedName" /> (or <see cref="T:System.Xml.Schema.XmlSchemaType" /> for anonymous types) that controls the serialization of the type.</summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	public sealed class XmlSchemaProviderAttribute : Attribute
	{
		private string _methodName;

		private bool _isAny;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> class, taking the name of the static method that supplies the type's XML schema.</summary>
		/// <param name="methodName">The name of the static method that must be implemented.</param>
		public XmlSchemaProviderAttribute(string methodName)
		{
			this._methodName = methodName;
		}

		/// <summary>Gets the name of the static method that supplies the type's XML schema and the name of its XML Schema data type.</summary>
		/// <returns>The name of the method that is invoked by the XML infrastructure to return an XML schema.</returns>
		public string MethodName
		{
			get
			{
				return this._methodName;
			}
		}

		/// <summary>Gets or sets a value that determines whether the target class is a wildcard, or that the schema for the class has contains only an xs:any element.</summary>
		/// <returns>true, if the class is a wildcard, or if the schema contains only the xs:any element; otherwise, false.</returns>
		public bool IsAny
		{
			get
			{
				return this._isAny;
			}
			set
			{
				this._isAny = value;
			}
		}
	}
}
