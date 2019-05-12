using System;
using System.Collections;

namespace System.Xml.Serialization
{
	internal class ClassMap : ObjectMap
	{
		private Hashtable _elements = new Hashtable();

		private ArrayList _elementMembers;

		private Hashtable _attributeMembers;

		private XmlTypeMapMemberAttribute[] _attributeMembersArray;

		private XmlTypeMapElementInfo[] _elementsByIndex;

		private ArrayList _flatLists;

		private ArrayList _allMembers = new ArrayList();

		private ArrayList _membersWithDefault;

		private ArrayList _listMembers;

		private XmlTypeMapMemberAnyElement _defaultAnyElement;

		private XmlTypeMapMemberAnyAttribute _defaultAnyAttribute;

		private XmlTypeMapMemberNamespaces _namespaceDeclarations;

		private XmlTypeMapMember _xmlTextCollector;

		private XmlTypeMapMember _returnMember;

		private bool _ignoreMemberNamespace;

		private bool _canBeSimpleType = true;

		public void AddMember(XmlTypeMapMember member)
		{
			member.GlobalIndex = this._allMembers.Count;
			this._allMembers.Add(member);
			if (!(member.DefaultValue is DBNull) && member.DefaultValue != null)
			{
				if (this._membersWithDefault == null)
				{
					this._membersWithDefault = new ArrayList();
				}
				this._membersWithDefault.Add(member);
			}
			if (member.IsReturnValue)
			{
				this._returnMember = member;
			}
			if (!(member is XmlTypeMapMemberAttribute))
			{
				if (member is XmlTypeMapMemberFlatList)
				{
					this.RegisterFlatList((XmlTypeMapMemberFlatList)member);
				}
				else if (member is XmlTypeMapMemberAnyElement)
				{
					XmlTypeMapMemberAnyElement xmlTypeMapMemberAnyElement = (XmlTypeMapMemberAnyElement)member;
					if (xmlTypeMapMemberAnyElement.IsDefaultAny)
					{
						this._defaultAnyElement = xmlTypeMapMemberAnyElement;
					}
					if (xmlTypeMapMemberAnyElement.TypeData.IsListType)
					{
						this.RegisterFlatList(xmlTypeMapMemberAnyElement);
					}
				}
				else
				{
					if (member is XmlTypeMapMemberAnyAttribute)
					{
						this._defaultAnyAttribute = (XmlTypeMapMemberAnyAttribute)member;
						return;
					}
					if (member is XmlTypeMapMemberNamespaces)
					{
						this._namespaceDeclarations = (XmlTypeMapMemberNamespaces)member;
						return;
					}
				}
				if (member is XmlTypeMapMemberElement && ((XmlTypeMapMemberElement)member).IsXmlTextCollector)
				{
					if (this._xmlTextCollector != null)
					{
						throw new InvalidOperationException("XmlTextAttribute can only be applied once in a class");
					}
					this._xmlTextCollector = member;
				}
				if (this._elementMembers == null)
				{
					this._elementMembers = new ArrayList();
					this._elements = new Hashtable();
				}
				member.Index = this._elementMembers.Count;
				this._elementMembers.Add(member);
				ICollection elementInfo = ((XmlTypeMapMemberElement)member).ElementInfo;
				foreach (object obj in elementInfo)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
					string key = this.BuildKey(xmlTypeMapElementInfo.ElementName, xmlTypeMapElementInfo.Namespace);
					if (this._elements.ContainsKey(key))
					{
						throw new InvalidOperationException(string.Concat(new string[]
						{
							"The XML element named '",
							xmlTypeMapElementInfo.ElementName,
							"' from namespace '",
							xmlTypeMapElementInfo.Namespace,
							"' is already present in the current scope. Use XML attributes to specify another XML name or namespace for the element."
						}));
					}
					this._elements.Add(key, xmlTypeMapElementInfo);
				}
				if (member.TypeData.IsListType && member.TypeData.Type != null && !member.TypeData.Type.IsArray)
				{
					if (this._listMembers == null)
					{
						this._listMembers = new ArrayList();
					}
					this._listMembers.Add(member);
				}
				return;
			}
			XmlTypeMapMemberAttribute xmlTypeMapMemberAttribute = (XmlTypeMapMemberAttribute)member;
			if (this._attributeMembers == null)
			{
				this._attributeMembers = new Hashtable();
			}
			string key2 = this.BuildKey(xmlTypeMapMemberAttribute.AttributeName, xmlTypeMapMemberAttribute.Namespace);
			if (this._attributeMembers.ContainsKey(key2))
			{
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"The XML attribute named '",
					xmlTypeMapMemberAttribute.AttributeName,
					"' from namespace '",
					xmlTypeMapMemberAttribute.Namespace,
					"' is already present in the current scope. Use XML attributes to specify another XML name or namespace for the attribute."
				}));
			}
			member.Index = this._attributeMembers.Count;
			this._attributeMembers.Add(key2, member);
		}

		private void RegisterFlatList(XmlTypeMapMemberExpandable member)
		{
			if (this._flatLists == null)
			{
				this._flatLists = new ArrayList();
			}
			member.FlatArrayIndex = this._flatLists.Count;
			this._flatLists.Add(member);
		}

		public XmlTypeMapMemberAttribute GetAttribute(string name, string ns)
		{
			if (this._attributeMembers == null)
			{
				return null;
			}
			return (XmlTypeMapMemberAttribute)this._attributeMembers[this.BuildKey(name, ns)];
		}

		public XmlTypeMapElementInfo GetElement(string name, string ns)
		{
			if (this._elements == null)
			{
				return null;
			}
			return (XmlTypeMapElementInfo)this._elements[this.BuildKey(name, ns)];
		}

		public XmlTypeMapElementInfo GetElement(int index)
		{
			if (this._elements == null)
			{
				return null;
			}
			if (this._elementsByIndex == null)
			{
				this._elementsByIndex = new XmlTypeMapElementInfo[this._elementMembers.Count];
				foreach (object obj in this._elementMembers)
				{
					XmlTypeMapMemberElement xmlTypeMapMemberElement = (XmlTypeMapMemberElement)obj;
					if (xmlTypeMapMemberElement.ElementInfo.Count != 1)
					{
						throw new InvalidOperationException("Read by order only possible for encoded/bare format");
					}
					this._elementsByIndex[xmlTypeMapMemberElement.Index] = (XmlTypeMapElementInfo)xmlTypeMapMemberElement.ElementInfo[0];
				}
			}
			return this._elementsByIndex[index];
		}

		private string BuildKey(string name, string ns)
		{
			if (this._ignoreMemberNamespace)
			{
				return name;
			}
			return name + " / " + ns;
		}

		public ICollection AllElementInfos
		{
			get
			{
				return this._elements.Values;
			}
		}

		public bool IgnoreMemberNamespace
		{
			get
			{
				return this._ignoreMemberNamespace;
			}
			set
			{
				this._ignoreMemberNamespace = value;
			}
		}

		public XmlTypeMapMember FindMember(string name)
		{
			for (int i = 0; i < this._allMembers.Count; i++)
			{
				if (((XmlTypeMapMember)this._allMembers[i]).Name == name)
				{
					return (XmlTypeMapMember)this._allMembers[i];
				}
			}
			return null;
		}

		public XmlTypeMapMemberAnyElement DefaultAnyElementMember
		{
			get
			{
				return this._defaultAnyElement;
			}
		}

		public XmlTypeMapMemberAnyAttribute DefaultAnyAttributeMember
		{
			get
			{
				return this._defaultAnyAttribute;
			}
		}

		public XmlTypeMapMemberNamespaces NamespaceDeclarations
		{
			get
			{
				return this._namespaceDeclarations;
			}
		}

		public ICollection AttributeMembers
		{
			get
			{
				if (this._attributeMembers == null)
				{
					return null;
				}
				if (this._attributeMembersArray != null)
				{
					return this._attributeMembersArray;
				}
				this._attributeMembersArray = new XmlTypeMapMemberAttribute[this._attributeMembers.Count];
				foreach (object obj in this._attributeMembers.Values)
				{
					XmlTypeMapMemberAttribute xmlTypeMapMemberAttribute = (XmlTypeMapMemberAttribute)obj;
					this._attributeMembersArray[xmlTypeMapMemberAttribute.Index] = xmlTypeMapMemberAttribute;
				}
				return this._attributeMembersArray;
			}
		}

		public ICollection ElementMembers
		{
			get
			{
				return this._elementMembers;
			}
		}

		public ArrayList AllMembers
		{
			get
			{
				return this._allMembers;
			}
		}

		public ArrayList FlatLists
		{
			get
			{
				return this._flatLists;
			}
		}

		public ArrayList MembersWithDefault
		{
			get
			{
				return this._membersWithDefault;
			}
		}

		public ArrayList ListMembers
		{
			get
			{
				return this._listMembers;
			}
		}

		public XmlTypeMapMember XmlTextCollector
		{
			get
			{
				return this._xmlTextCollector;
			}
		}

		public XmlTypeMapMember ReturnMember
		{
			get
			{
				return this._returnMember;
			}
		}

		public XmlQualifiedName SimpleContentBaseType
		{
			get
			{
				if (!this._canBeSimpleType || this._elementMembers == null || this._elementMembers.Count != 1)
				{
					return null;
				}
				XmlTypeMapMemberElement xmlTypeMapMemberElement = (XmlTypeMapMemberElement)this._elementMembers[0];
				if (xmlTypeMapMemberElement.ElementInfo.Count != 1)
				{
					return null;
				}
				XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)xmlTypeMapMemberElement.ElementInfo[0];
				if (!xmlTypeMapElementInfo.IsTextElement)
				{
					return null;
				}
				if (xmlTypeMapMemberElement.TypeData.SchemaType == SchemaTypes.Primitive || xmlTypeMapMemberElement.TypeData.SchemaType == SchemaTypes.Enum)
				{
					return new XmlQualifiedName(xmlTypeMapElementInfo.TypeData.XmlType, xmlTypeMapElementInfo.DataTypeNamespace);
				}
				return null;
			}
		}

		public void SetCanBeSimpleType(bool can)
		{
			this._canBeSimpleType = can;
		}

		public bool HasSimpleContent
		{
			get
			{
				return this.SimpleContentBaseType != null;
			}
		}
	}
}
