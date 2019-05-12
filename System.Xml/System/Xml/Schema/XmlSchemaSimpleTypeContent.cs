using System;

namespace System.Xml.Schema
{
	/// <summary>Abstract class for simple type content classes.</summary>
	public abstract class XmlSchemaSimpleTypeContent : XmlSchemaAnnotated
	{
		internal XmlSchemaSimpleType OwnerType;

		internal object ActualBaseSchemaType
		{
			get
			{
				return this.OwnerType.BaseSchemaType;
			}
		}

		internal virtual string Normalize(string s, XmlNameTable nt, XmlNamespaceManager nsmgr)
		{
			return s;
		}
	}
}
