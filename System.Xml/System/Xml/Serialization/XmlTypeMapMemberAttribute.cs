using System;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	internal class XmlTypeMapMemberAttribute : XmlTypeMapMember
	{
		private string _attributeName;

		private string _namespace = string.Empty;

		private XmlSchemaForm _form;

		private XmlTypeMapping _mappedType;

		public string AttributeName
		{
			get
			{
				return this._attributeName;
			}
			set
			{
				this._attributeName = value;
			}
		}

		public string Namespace
		{
			get
			{
				return this._namespace;
			}
			set
			{
				this._namespace = value;
			}
		}

		public string DataTypeNamespace
		{
			get
			{
				if (this._mappedType == null)
				{
					return "http://www.w3.org/2001/XMLSchema";
				}
				return this._mappedType.Namespace;
			}
		}

		public XmlSchemaForm Form
		{
			get
			{
				return this._form;
			}
			set
			{
				this._form = value;
			}
		}

		public XmlTypeMapping MappedType
		{
			get
			{
				return this._mappedType;
			}
			set
			{
				this._mappedType = value;
			}
		}
	}
}
