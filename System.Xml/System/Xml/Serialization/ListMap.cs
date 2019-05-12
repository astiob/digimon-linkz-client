using System;

namespace System.Xml.Serialization
{
	internal class ListMap : ObjectMap
	{
		private XmlTypeMapElementInfoList _itemInfo;

		private bool _gotNestedMapping;

		private XmlTypeMapping _nestedArrayMapping;

		private string _choiceMember;

		public bool IsMultiArray
		{
			get
			{
				return this.NestedArrayMapping != null;
			}
		}

		public string ChoiceMember
		{
			get
			{
				return this._choiceMember;
			}
			set
			{
				this._choiceMember = value;
			}
		}

		public XmlTypeMapping NestedArrayMapping
		{
			get
			{
				if (this._gotNestedMapping)
				{
					return this._nestedArrayMapping;
				}
				this._gotNestedMapping = true;
				this._nestedArrayMapping = ((XmlTypeMapElementInfo)this._itemInfo[0]).MappedType;
				if (this._nestedArrayMapping == null)
				{
					return null;
				}
				if (this._nestedArrayMapping.TypeData.SchemaType != SchemaTypes.Array)
				{
					this._nestedArrayMapping = null;
					return null;
				}
				foreach (object obj in this._itemInfo)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
					if (xmlTypeMapElementInfo.MappedType != this._nestedArrayMapping)
					{
						this._nestedArrayMapping = null;
						return null;
					}
				}
				return this._nestedArrayMapping;
			}
		}

		public XmlTypeMapElementInfoList ItemInfo
		{
			get
			{
				return this._itemInfo;
			}
			set
			{
				this._itemInfo = value;
			}
		}

		public XmlTypeMapElementInfo FindElement(object ob, int index, object memberValue)
		{
			if (this._itemInfo.Count == 1)
			{
				return (XmlTypeMapElementInfo)this._itemInfo[0];
			}
			if (this._choiceMember != null && index != -1)
			{
				Array array = (Array)XmlTypeMapMember.GetValue(ob, this._choiceMember);
				if (array == null || index >= array.Length)
				{
					throw new InvalidOperationException("Invalid or missing choice enum value in member '" + this._choiceMember + "'.");
				}
				object value = array.GetValue(index);
				foreach (object obj in this._itemInfo)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
					if (xmlTypeMapElementInfo.ChoiceValue != null && xmlTypeMapElementInfo.ChoiceValue.Equals(value))
					{
						return xmlTypeMapElementInfo;
					}
				}
			}
			else
			{
				if (memberValue == null)
				{
					return null;
				}
				Type type = memberValue.GetType();
				foreach (object obj2 in this._itemInfo)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo2 = (XmlTypeMapElementInfo)obj2;
					if (xmlTypeMapElementInfo2.TypeData.Type == type)
					{
						return xmlTypeMapElementInfo2;
					}
				}
			}
			return null;
		}

		public XmlTypeMapElementInfo FindElement(string elementName, string ns)
		{
			foreach (object obj in this._itemInfo)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
				if (xmlTypeMapElementInfo.ElementName == elementName && xmlTypeMapElementInfo.Namespace == ns)
				{
					return xmlTypeMapElementInfo;
				}
			}
			return null;
		}

		public XmlTypeMapElementInfo FindTextElement()
		{
			foreach (object obj in this._itemInfo)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
				if (xmlTypeMapElementInfo.IsTextElement)
				{
					return xmlTypeMapElementInfo;
				}
			}
			return null;
		}

		public string GetSchemaArrayName()
		{
			XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)this._itemInfo[0];
			if (xmlTypeMapElementInfo.MappedType != null)
			{
				return TypeTranslator.GetArrayName(xmlTypeMapElementInfo.MappedType.XmlType);
			}
			return TypeTranslator.GetArrayName(xmlTypeMapElementInfo.TypeData.XmlType);
		}

		public void GetArrayType(int itemCount, out string localName, out string ns)
		{
			string str;
			if (itemCount != -1)
			{
				str = "[" + itemCount + "]";
			}
			else
			{
				str = "[]";
			}
			XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)this._itemInfo[0];
			if (xmlTypeMapElementInfo.TypeData.SchemaType == SchemaTypes.Array)
			{
				string str2;
				((ListMap)xmlTypeMapElementInfo.MappedType.ObjectMap).GetArrayType(-1, out str2, out ns);
				localName = str2 + str;
			}
			else if (xmlTypeMapElementInfo.MappedType != null)
			{
				localName = xmlTypeMapElementInfo.MappedType.XmlType + str;
				ns = xmlTypeMapElementInfo.MappedType.Namespace;
			}
			else
			{
				localName = xmlTypeMapElementInfo.TypeData.XmlType + str;
				ns = xmlTypeMapElementInfo.DataTypeNamespace;
			}
		}

		public override bool Equals(object other)
		{
			ListMap listMap = other as ListMap;
			if (listMap == null)
			{
				return false;
			}
			if (this._itemInfo.Count != listMap._itemInfo.Count)
			{
				return false;
			}
			for (int i = 0; i < this._itemInfo.Count; i++)
			{
				if (!this._itemInfo[i].Equals(listMap._itemInfo[i]))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
