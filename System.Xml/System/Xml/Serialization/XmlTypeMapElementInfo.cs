using System;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	internal class XmlTypeMapElementInfo
	{
		private string _elementName;

		private string _namespace = string.Empty;

		private XmlSchemaForm _form;

		private XmlTypeMapMember _member;

		private object _choiceValue;

		private bool _isNullable;

		private int _nestingLevel;

		private XmlTypeMapping _mappedType;

		private TypeData _type;

		private bool _wrappedElement = true;

		public XmlTypeMapElementInfo(XmlTypeMapMember member, TypeData type)
		{
			this._member = member;
			this._type = type;
			if (type.IsValueType && type.IsNullable)
			{
				this._isNullable = true;
			}
		}

		public TypeData TypeData
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		public object ChoiceValue
		{
			get
			{
				return this._choiceValue;
			}
			set
			{
				this._choiceValue = value;
			}
		}

		public string ElementName
		{
			get
			{
				return this._elementName;
			}
			set
			{
				this._elementName = value;
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
				return this._mappedType.XmlTypeNamespace;
			}
		}

		public string DataTypeName
		{
			get
			{
				if (this._mappedType == null)
				{
					return this.TypeData.XmlType;
				}
				return this._mappedType.XmlType;
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

		public bool IsNullable
		{
			get
			{
				return this._isNullable;
			}
			set
			{
				this._isNullable = value;
			}
		}

		internal bool IsPrimitive
		{
			get
			{
				return this._mappedType == null;
			}
		}

		public XmlTypeMapMember Member
		{
			get
			{
				return this._member;
			}
			set
			{
				this._member = value;
			}
		}

		public int NestingLevel
		{
			get
			{
				return this._nestingLevel;
			}
			set
			{
				this._nestingLevel = value;
			}
		}

		public bool MultiReferenceType
		{
			get
			{
				return this._mappedType != null && this._mappedType.MultiReferenceType;
			}
		}

		public bool WrappedElement
		{
			get
			{
				return this._wrappedElement;
			}
			set
			{
				this._wrappedElement = value;
			}
		}

		public bool IsTextElement
		{
			get
			{
				return this.ElementName == "<text>";
			}
			set
			{
				if (!value)
				{
					throw new Exception("INTERNAL ERROR; someone wrote unexpected code in sys.xml");
				}
				this.ElementName = "<text>";
				this.Namespace = string.Empty;
			}
		}

		public bool IsUnnamedAnyElement
		{
			get
			{
				return this.ElementName == string.Empty;
			}
			set
			{
				if (!value)
				{
					throw new Exception("INTERNAL ERROR; someone wrote unexpected code in sys.xml");
				}
				this.ElementName = string.Empty;
				this.Namespace = string.Empty;
			}
		}

		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)other;
			return !(this._elementName != xmlTypeMapElementInfo._elementName) && !(this._type.XmlType != xmlTypeMapElementInfo._type.XmlType) && !(this._namespace != xmlTypeMapElementInfo._namespace) && this._form == xmlTypeMapElementInfo._form && this._type == xmlTypeMapElementInfo._type && this._isNullable == xmlTypeMapElementInfo._isNullable && (this._choiceValue == null || this._choiceValue.Equals(xmlTypeMapElementInfo._choiceValue)) && this._choiceValue == xmlTypeMapElementInfo._choiceValue;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
