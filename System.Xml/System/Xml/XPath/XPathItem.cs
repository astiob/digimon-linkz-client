using System;
using System.Xml.Schema;

namespace System.Xml.XPath
{
	/// <summary>Represents an item in the XQuery 1.0 and XPath 2.0 Data Model.</summary>
	public abstract class XPathItem
	{
		/// <summary>Returns the item's value as the specified type.</summary>
		/// <returns>The value of the item as the type requested.</returns>
		/// <param name="returnType">The type to return the item value as.</param>
		/// <exception cref="T:System.FormatException">The item's value is not in the correct format for the target type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.OverflowException">The attempted cast resulted in an overflow.</exception>
		public virtual object ValueAs(Type type)
		{
			return this.ValueAs(type, null);
		}

		/// <summary>When overridden in a derived class, returns the item's value as the type specified using the <see cref="T:System.Xml.IXmlNamespaceResolver" /> object specified to resolve namespace prefixes.</summary>
		/// <returns>The value of the item as the type requested.</returns>
		/// <param name="returnType">The type to return the item's value as.</param>
		/// <param name="nsResolver">The <see cref="T:System.Xml.IXmlNamespaceResolver" /> object used to resolve namespace prefixes.</param>
		/// <exception cref="T:System.FormatException">The item's value is not in the correct format for the target type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.OverflowException">The attempted cast resulted in an overflow.</exception>
		public abstract object ValueAs(Type type, IXmlNamespaceResolver nsResolver);

		/// <summary>When overridden in a derived class, gets a value indicating whether the item represents an XPath node or an atomic value.</summary>
		/// <returns>true if the item represents an XPath node; false if the item represents an atomic value.</returns>
		public abstract bool IsNode { get; }

		/// <summary>When overridden in a derived class, gets the current item as a boxed object of the most appropriate .NET Framework version 2.0 type according to its schema type.</summary>
		/// <returns>The current item as a boxed object of the most appropriate .NET Framework type.</returns>
		public abstract object TypedValue { get; }

		/// <summary>When overridden in a derived class, gets the string value of the item.</summary>
		/// <returns>The string value of the item.</returns>
		public abstract string Value { get; }

		/// <summary>When overridden in a derived class, gets the item's value as a <see cref="T:System.Boolean" />.</summary>
		/// <returns>The item's value as a <see cref="T:System.Boolean" />.</returns>
		/// <exception cref="T:System.FormatException">The item's value is not in the correct format for the <see cref="T:System.Boolean" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Boolean" /> is not valid.</exception>
		public abstract bool ValueAsBoolean { get; }

		/// <summary>When overridden in a derived class, gets the item's value as a <see cref="T:System.DateTime" />.</summary>
		/// <returns>The item's value as a <see cref="T:System.DateTime" />.</returns>
		/// <exception cref="T:System.FormatException">The item's value is not in the correct format for the <see cref="T:System.DateTime" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.DateTime" /> is not valid.</exception>
		public abstract DateTime ValueAsDateTime { get; }

		/// <summary>When overridden in a derived class, gets the item's value as a <see cref="T:System.Double" />.</summary>
		/// <returns>The item's value as a <see cref="T:System.Double" />.</returns>
		/// <exception cref="T:System.FormatException">The item's value is not in the correct format for the <see cref="T:System.Double" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Double" /> is not valid.</exception>
		/// <exception cref="T:System.OverflowException">The attempted cast resulted in an overflow.</exception>
		public abstract double ValueAsDouble { get; }

		/// <summary>When overridden in a derived class, gets the item's value as an <see cref="T:System.Int32" />.</summary>
		/// <returns>The item's value as an <see cref="T:System.Int32" />.</returns>
		/// <exception cref="T:System.FormatException">The item's value is not in the correct format for the <see cref="T:System.Int32" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Int32" /> is not valid.</exception>
		/// <exception cref="T:System.OverflowException">The attempted cast resulted in an overflow.</exception>
		public abstract int ValueAsInt { get; }

		/// <summary>When overridden in a derived class, gets the item's value as an <see cref="T:System.Int64" />.</summary>
		/// <returns>The item's value as an <see cref="T:System.Int64" />.</returns>
		/// <exception cref="T:System.FormatException">The item's value is not in the correct format for the <see cref="T:System.Int64" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Int64" /> is not valid.</exception>
		/// <exception cref="T:System.OverflowException">The attempted cast resulted in an overflow.</exception>
		public abstract long ValueAsLong { get; }

		/// <summary>When overridden in a derived class, gets the .NET Framework version 2.0 type of the item.</summary>
		/// <returns>The .NET Framework type of the item. The default value is <see cref="T:System.String" />.</returns>
		public abstract Type ValueType { get; }

		/// <summary>When overridden in a derived class, gets the <see cref="T:System.Xml.Schema.XmlSchemaType" /> for the item.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaType" /> for the item.</returns>
		public abstract XmlSchemaType XmlType { get; }
	}
}
