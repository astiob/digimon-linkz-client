using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace System.Xml.Serialization
{
	internal class XmlSerializationWriterInterpreter : XmlSerializationWriter
	{
		private const string xmlNamespace = "http://www.w3.org/2000/xmlns/";

		private XmlMapping _typeMap;

		private SerializationFormat _format;

		public XmlSerializationWriterInterpreter(XmlMapping typeMap)
		{
			this._typeMap = typeMap;
			this._format = typeMap.Format;
		}

		protected override void InitCallbacks()
		{
			ArrayList relatedMaps = this._typeMap.RelatedMaps;
			if (relatedMaps != null)
			{
				foreach (object obj in relatedMaps)
				{
					XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)obj;
					XmlSerializationWriterInterpreter.CallbackInfo @object = new XmlSerializationWriterInterpreter.CallbackInfo(this, xmlTypeMapping);
					if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Enum)
					{
						base.AddWriteCallback(xmlTypeMapping.TypeData.Type, xmlTypeMapping.XmlType, xmlTypeMapping.Namespace, new XmlSerializationWriteCallback(@object.WriteEnum));
					}
					else
					{
						base.AddWriteCallback(xmlTypeMapping.TypeData.Type, xmlTypeMapping.XmlType, xmlTypeMapping.Namespace, new XmlSerializationWriteCallback(@object.WriteObject));
					}
				}
			}
		}

		public void WriteRoot(object ob)
		{
			base.WriteStartDocument();
			if (this._typeMap is XmlTypeMapping)
			{
				XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)this._typeMap;
				if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Class || xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Array)
				{
					base.TopLevelElement();
				}
				if (this._format == SerializationFormat.Literal)
				{
					this.WriteObject(xmlTypeMapping, ob, xmlTypeMapping.ElementName, xmlTypeMapping.Namespace, true, false, true);
				}
				else
				{
					base.WritePotentiallyReferencingElement(xmlTypeMapping.ElementName, xmlTypeMapping.Namespace, ob, xmlTypeMapping.TypeData.Type, true, false);
				}
			}
			else
			{
				if (!(ob is object[]))
				{
					throw base.CreateUnknownTypeException(ob);
				}
				this.WriteMessage((XmlMembersMapping)this._typeMap, (object[])ob);
			}
			base.WriteReferencedElements();
		}

		protected XmlTypeMapping GetTypeMap(Type type)
		{
			ArrayList relatedMaps = this._typeMap.RelatedMaps;
			if (relatedMaps != null)
			{
				foreach (object obj in relatedMaps)
				{
					XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)obj;
					if (xmlTypeMapping.TypeData.Type == type)
					{
						return xmlTypeMapping;
					}
				}
			}
			throw new InvalidOperationException("Type " + type + " not mapped");
		}

		protected virtual void WriteObject(XmlTypeMapping typeMap, object ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					if (this._format == SerializationFormat.Literal)
					{
						base.WriteNullTagLiteral(element, namesp);
					}
					else
					{
						base.WriteNullTagEncoded(element, namesp);
					}
				}
				return;
			}
			if (ob is XmlNode)
			{
				if (this._format == SerializationFormat.Literal)
				{
					base.WriteElementLiteral((XmlNode)ob, string.Empty, string.Empty, true, false);
				}
				else
				{
					base.WriteElementEncoded((XmlNode)ob, string.Empty, string.Empty, true, false);
				}
				return;
			}
			if (typeMap.TypeData.SchemaType == SchemaTypes.XmlSerializable)
			{
				base.WriteSerializable((IXmlSerializable)ob, element, namesp, isNullable);
				return;
			}
			XmlTypeMapping realTypeMap = typeMap.GetRealTypeMap(ob.GetType());
			if (realTypeMap == null)
			{
				if (ob.GetType().IsArray && typeof(XmlNode).IsAssignableFrom(ob.GetType().GetElementType()))
				{
					base.Writer.WriteStartElement(element, namesp);
					foreach (object obj in ((IEnumerable)ob))
					{
						XmlNode xmlNode = (XmlNode)obj;
						xmlNode.WriteTo(base.Writer);
					}
					base.Writer.WriteEndElement();
				}
				else
				{
					base.WriteTypedPrimitive(element, namesp, ob, true);
				}
				return;
			}
			if (writeWrappingElem)
			{
				if (realTypeMap != typeMap || this._format == SerializationFormat.Encoded)
				{
					needType = true;
				}
				base.WriteStartElement(element, namesp, ob);
			}
			if (needType)
			{
				base.WriteXsiType(realTypeMap.XmlType, realTypeMap.XmlTypeNamespace);
			}
			switch (realTypeMap.TypeData.SchemaType)
			{
			case SchemaTypes.Primitive:
				this.WritePrimitiveElement(realTypeMap, ob, element, namesp);
				break;
			case SchemaTypes.Enum:
				this.WriteEnumElement(realTypeMap, ob, element, namesp);
				break;
			case SchemaTypes.Array:
				this.WriteListElement(realTypeMap, ob, element, namesp);
				break;
			case SchemaTypes.Class:
				this.WriteObjectElement(realTypeMap, ob, element, namesp);
				break;
			}
			if (writeWrappingElem)
			{
				base.WriteEndElement(ob);
			}
		}

		protected virtual void WriteMessage(XmlMembersMapping membersMap, object[] parameters)
		{
			if (membersMap.HasWrapperElement)
			{
				base.TopLevelElement();
				base.WriteStartElement(membersMap.ElementName, membersMap.Namespace, this._format == SerializationFormat.Encoded);
				if (base.Writer.LookupPrefix("http://www.w3.org/2001/XMLSchema") == null)
				{
					base.WriteAttribute("xmlns", "xsd", "http://www.w3.org/2001/XMLSchema", "http://www.w3.org/2001/XMLSchema");
				}
				if (base.Writer.LookupPrefix("http://www.w3.org/2001/XMLSchema-instance") == null)
				{
					base.WriteAttribute("xmlns", "xsi", "http://www.w3.org/2001/XMLSchema-instance", "http://www.w3.org/2001/XMLSchema-instance");
				}
			}
			this.WriteMembers((ClassMap)membersMap.ObjectMap, parameters, true);
			if (membersMap.HasWrapperElement)
			{
				base.WriteEndElement();
			}
		}

		protected virtual void WriteObjectElement(XmlTypeMapping typeMap, object ob, string element, string namesp)
		{
			ClassMap classMap = (ClassMap)typeMap.ObjectMap;
			if (classMap.NamespaceDeclarations != null)
			{
				base.WriteNamespaceDeclarations((XmlSerializerNamespaces)classMap.NamespaceDeclarations.GetValue(ob));
			}
			this.WriteObjectElementAttributes(typeMap, ob);
			this.WriteObjectElementElements(typeMap, ob);
		}

		protected virtual void WriteObjectElementAttributes(XmlTypeMapping typeMap, object ob)
		{
			ClassMap map = (ClassMap)typeMap.ObjectMap;
			this.WriteAttributeMembers(map, ob, false);
		}

		protected virtual void WriteObjectElementElements(XmlTypeMapping typeMap, object ob)
		{
			ClassMap map = (ClassMap)typeMap.ObjectMap;
			this.WriteElementMembers(map, ob, false);
		}

		private void WriteMembers(ClassMap map, object ob, bool isValueList)
		{
			this.WriteAttributeMembers(map, ob, isValueList);
			this.WriteElementMembers(map, ob, isValueList);
		}

		private void WriteAttributeMembers(ClassMap map, object ob, bool isValueList)
		{
			XmlTypeMapMember defaultAnyAttributeMember = map.DefaultAnyAttributeMember;
			if (defaultAnyAttributeMember != null && this.MemberHasValue(defaultAnyAttributeMember, ob, isValueList))
			{
				ICollection collection = (ICollection)this.GetMemberValue(defaultAnyAttributeMember, ob, isValueList);
				if (collection != null)
				{
					foreach (object obj in collection)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
			}
			ICollection attributeMembers = map.AttributeMembers;
			if (attributeMembers != null)
			{
				foreach (object obj2 in attributeMembers)
				{
					XmlTypeMapMemberAttribute xmlTypeMapMemberAttribute = (XmlTypeMapMemberAttribute)obj2;
					if (this.MemberHasValue(xmlTypeMapMemberAttribute, ob, isValueList))
					{
						base.WriteAttribute(xmlTypeMapMemberAttribute.AttributeName, xmlTypeMapMemberAttribute.Namespace, this.GetStringValue(xmlTypeMapMemberAttribute.MappedType, xmlTypeMapMemberAttribute.TypeData, this.GetMemberValue(xmlTypeMapMemberAttribute, ob, isValueList)));
					}
				}
			}
		}

		private void WriteElementMembers(ClassMap map, object ob, bool isValueList)
		{
			ICollection elementMembers = map.ElementMembers;
			if (elementMembers != null)
			{
				foreach (object obj in elementMembers)
				{
					XmlTypeMapMemberElement xmlTypeMapMemberElement = (XmlTypeMapMemberElement)obj;
					if (this.MemberHasValue(xmlTypeMapMemberElement, ob, isValueList))
					{
						object memberValue = this.GetMemberValue(xmlTypeMapMemberElement, ob, isValueList);
						Type type = xmlTypeMapMemberElement.GetType();
						if (type == typeof(XmlTypeMapMemberList))
						{
							this.WriteMemberElement((XmlTypeMapElementInfo)xmlTypeMapMemberElement.ElementInfo[0], memberValue);
						}
						else if (type == typeof(XmlTypeMapMemberFlatList))
						{
							if (memberValue != null)
							{
								this.WriteListContent(ob, xmlTypeMapMemberElement.TypeData, ((XmlTypeMapMemberFlatList)xmlTypeMapMemberElement).ListMap, memberValue, null);
							}
						}
						else if (type == typeof(XmlTypeMapMemberAnyElement))
						{
							if (memberValue != null)
							{
								this.WriteAnyElementContent((XmlTypeMapMemberAnyElement)xmlTypeMapMemberElement, memberValue);
							}
						}
						else if (type != typeof(XmlTypeMapMemberAnyAttribute))
						{
							if (type != typeof(XmlTypeMapMemberElement))
							{
								throw new InvalidOperationException("Unknown member type");
							}
							XmlTypeMapElementInfo elem = xmlTypeMapMemberElement.FindElement(ob, memberValue);
							this.WriteMemberElement(elem, memberValue);
						}
					}
				}
			}
		}

		private object GetMemberValue(XmlTypeMapMember member, object ob, bool isValueList)
		{
			if (isValueList)
			{
				return ((object[])ob)[member.GlobalIndex];
			}
			return member.GetValue(ob);
		}

		private bool MemberHasValue(XmlTypeMapMember member, object ob, bool isValueList)
		{
			if (isValueList)
			{
				return member.GlobalIndex < ((object[])ob).Length;
			}
			if (member.DefaultValue != DBNull.Value)
			{
				object obj = this.GetMemberValue(member, ob, isValueList);
				if (obj == null && member.DefaultValue == null)
				{
					return false;
				}
				if (obj != null && obj.GetType().IsEnum)
				{
					if (obj.Equals(member.DefaultValue))
					{
						return false;
					}
					Type underlyingType = Enum.GetUnderlyingType(obj.GetType());
					obj = Convert.ChangeType(obj, underlyingType);
				}
				if (obj != null && obj.Equals(member.DefaultValue))
				{
					return false;
				}
			}
			else if (member.IsOptionalValueType)
			{
				return member.GetValueSpecified(ob);
			}
			return true;
		}

		private void WriteMemberElement(XmlTypeMapElementInfo elem, object memberValue)
		{
			switch (elem.TypeData.SchemaType)
			{
			case SchemaTypes.Primitive:
			case SchemaTypes.Enum:
				if (this._format == SerializationFormat.Literal)
				{
					this.WritePrimitiveValueLiteral(memberValue, elem.ElementName, elem.Namespace, elem.MappedType, elem.TypeData, elem.WrappedElement, elem.IsNullable);
				}
				else
				{
					this.WritePrimitiveValueEncoded(memberValue, elem.ElementName, elem.Namespace, new XmlQualifiedName(elem.DataTypeName, elem.DataTypeNamespace), elem.MappedType, elem.TypeData, elem.WrappedElement, elem.IsNullable);
				}
				break;
			case SchemaTypes.Array:
				if (memberValue == null)
				{
					if (!elem.IsNullable)
					{
						return;
					}
					if (this._format == SerializationFormat.Literal)
					{
						base.WriteNullTagLiteral(elem.ElementName, elem.Namespace);
					}
					else
					{
						base.WriteNullTagEncoded(elem.ElementName, elem.Namespace);
					}
				}
				else if (elem.MappedType.MultiReferenceType)
				{
					base.WriteReferencingElement(elem.ElementName, elem.Namespace, memberValue, elem.IsNullable);
				}
				else
				{
					base.WriteStartElement(elem.ElementName, elem.Namespace, memberValue);
					this.WriteListContent(null, elem.TypeData, (ListMap)elem.MappedType.ObjectMap, memberValue, null);
					base.WriteEndElement(memberValue);
				}
				break;
			case SchemaTypes.Class:
				if (elem.MappedType.MultiReferenceType)
				{
					if (elem.MappedType.TypeData.Type == typeof(object))
					{
						base.WritePotentiallyReferencingElement(elem.ElementName, elem.Namespace, memberValue, null, false, elem.IsNullable);
					}
					else
					{
						base.WriteReferencingElement(elem.ElementName, elem.Namespace, memberValue, elem.IsNullable);
					}
				}
				else
				{
					this.WriteObject(elem.MappedType, memberValue, elem.ElementName, elem.Namespace, elem.IsNullable, false, true);
				}
				break;
			case SchemaTypes.XmlSerializable:
				if (!elem.MappedType.TypeData.Type.IsInstanceOfType(memberValue))
				{
					memberValue = this.ImplicitConvert(memberValue, elem.MappedType.TypeData.Type);
				}
				base.WriteSerializable((IXmlSerializable)memberValue, elem.ElementName, elem.Namespace, elem.IsNullable);
				break;
			case SchemaTypes.XmlNode:
			{
				string name = (!elem.WrappedElement) ? string.Empty : elem.ElementName;
				if (this._format == SerializationFormat.Literal)
				{
					base.WriteElementLiteral((XmlNode)memberValue, name, elem.Namespace, elem.IsNullable, false);
				}
				else
				{
					base.WriteElementEncoded((XmlNode)memberValue, name, elem.Namespace, elem.IsNullable, false);
				}
				break;
			}
			default:
				throw new NotSupportedException("Invalid value type");
			}
		}

		private object ImplicitConvert(object obj, Type type)
		{
			if (obj == null)
			{
				return null;
			}
			for (Type type2 = type; type2 != typeof(object); type2 = type2.BaseType)
			{
				MethodInfo method = type2.GetMethod("op_Implicit", new Type[]
				{
					type2
				});
				if (method != null && method.ReturnType.IsAssignableFrom(obj.GetType()))
				{
					return method.Invoke(null, new object[]
					{
						obj
					});
				}
			}
			for (Type type3 = obj.GetType(); type3 != typeof(object); type3 = type3.BaseType)
			{
				MethodInfo method2 = type3.GetMethod("op_Implicit", new Type[]
				{
					type3
				});
				if (method2 != null && method2.ReturnType == type)
				{
					return method2.Invoke(null, new object[]
					{
						obj
					});
				}
			}
			return obj;
		}

		private void WritePrimitiveValueLiteral(object memberValue, string name, string ns, XmlTypeMapping mappedType, TypeData typeData, bool wrapped, bool isNullable)
		{
			if (!wrapped)
			{
				base.WriteValue(this.GetStringValue(mappedType, typeData, memberValue));
			}
			else if (isNullable)
			{
				if (typeData.Type == typeof(XmlQualifiedName))
				{
					base.WriteNullableQualifiedNameLiteral(name, ns, (XmlQualifiedName)memberValue);
				}
				else
				{
					base.WriteNullableStringLiteral(name, ns, this.GetStringValue(mappedType, typeData, memberValue));
				}
			}
			else if (typeData.Type == typeof(XmlQualifiedName))
			{
				base.WriteElementQualifiedName(name, ns, (XmlQualifiedName)memberValue);
			}
			else
			{
				base.WriteElementString(name, ns, this.GetStringValue(mappedType, typeData, memberValue));
			}
		}

		private void WritePrimitiveValueEncoded(object memberValue, string name, string ns, XmlQualifiedName xsiType, XmlTypeMapping mappedType, TypeData typeData, bool wrapped, bool isNullable)
		{
			if (!wrapped)
			{
				base.WriteValue(this.GetStringValue(mappedType, typeData, memberValue));
			}
			else if (isNullable)
			{
				if (typeData.Type == typeof(XmlQualifiedName))
				{
					base.WriteNullableQualifiedNameEncoded(name, ns, (XmlQualifiedName)memberValue, xsiType);
				}
				else
				{
					base.WriteNullableStringEncoded(name, ns, this.GetStringValue(mappedType, typeData, memberValue), xsiType);
				}
			}
			else if (typeData.Type == typeof(XmlQualifiedName))
			{
				base.WriteElementQualifiedName(name, ns, (XmlQualifiedName)memberValue, xsiType);
			}
			else
			{
				base.WriteElementString(name, ns, this.GetStringValue(mappedType, typeData, memberValue), xsiType);
			}
		}

		protected virtual void WriteListElement(XmlTypeMapping typeMap, object ob, string element, string namesp)
		{
			if (this._format == SerializationFormat.Encoded)
			{
				int listCount = this.GetListCount(typeMap.TypeData, ob);
				string text;
				string text2;
				((ListMap)typeMap.ObjectMap).GetArrayType(listCount, out text, out text2);
				string value = (!(text2 != string.Empty)) ? text : base.FromXmlQualifiedName(new XmlQualifiedName(text, text2));
				base.WriteAttribute("arrayType", "http://schemas.xmlsoap.org/soap/encoding/", value);
			}
			this.WriteListContent(null, typeMap.TypeData, (ListMap)typeMap.ObjectMap, ob, null);
		}

		private void WriteListContent(object container, TypeData listType, ListMap map, object ob, StringBuilder targetString)
		{
			if (listType.Type.IsArray)
			{
				Array array = (Array)ob;
				for (int i = 0; i < array.Length; i++)
				{
					object value = array.GetValue(i);
					XmlTypeMapElementInfo xmlTypeMapElementInfo = map.FindElement(container, i, value);
					if (xmlTypeMapElementInfo != null && targetString == null)
					{
						this.WriteMemberElement(xmlTypeMapElementInfo, value);
					}
					else if (xmlTypeMapElementInfo != null && targetString != null)
					{
						targetString.Append(this.GetStringValue(xmlTypeMapElementInfo.MappedType, xmlTypeMapElementInfo.TypeData, value)).Append(" ");
					}
					else if (value != null)
					{
						throw base.CreateUnknownTypeException(value);
					}
				}
			}
			else if (ob is ICollection)
			{
				int num = (int)ob.GetType().GetProperty("Count").GetValue(ob, null);
				PropertyInfo indexerProperty = TypeData.GetIndexerProperty(listType.Type);
				object[] array2 = new object[1];
				for (int j = 0; j < num; j++)
				{
					array2[0] = j;
					object value2 = indexerProperty.GetValue(ob, array2);
					XmlTypeMapElementInfo xmlTypeMapElementInfo2 = map.FindElement(container, j, value2);
					if (xmlTypeMapElementInfo2 != null && targetString == null)
					{
						this.WriteMemberElement(xmlTypeMapElementInfo2, value2);
					}
					else if (xmlTypeMapElementInfo2 != null && targetString != null)
					{
						targetString.Append(this.GetStringValue(xmlTypeMapElementInfo2.MappedType, xmlTypeMapElementInfo2.TypeData, value2)).Append(" ");
					}
					else if (value2 != null)
					{
						throw base.CreateUnknownTypeException(value2);
					}
				}
			}
			else
			{
				if (!(ob is IEnumerable))
				{
					throw new Exception("Unsupported collection type");
				}
				IEnumerable enumerable = (IEnumerable)ob;
				foreach (object obj in enumerable)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo3 = map.FindElement(container, -1, obj);
					if (xmlTypeMapElementInfo3 != null && targetString == null)
					{
						this.WriteMemberElement(xmlTypeMapElementInfo3, obj);
					}
					else if (xmlTypeMapElementInfo3 != null && targetString != null)
					{
						targetString.Append(this.GetStringValue(xmlTypeMapElementInfo3.MappedType, xmlTypeMapElementInfo3.TypeData, obj)).Append(" ");
					}
					else if (obj != null)
					{
						throw base.CreateUnknownTypeException(obj);
					}
				}
			}
		}

		private int GetListCount(TypeData listType, object ob)
		{
			if (listType.Type.IsArray)
			{
				return ((Array)ob).Length;
			}
			return (int)listType.Type.GetProperty("Count").GetValue(ob, null);
		}

		private void WriteAnyElementContent(XmlTypeMapMemberAnyElement member, object memberValue)
		{
			if (member.TypeData.Type == typeof(XmlElement))
			{
				memberValue = new object[]
				{
					memberValue
				};
			}
			Array array = (Array)memberValue;
			foreach (object obj in array)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode is XmlElement)
				{
					if (!member.IsElementDefined(xmlNode.Name, xmlNode.NamespaceURI))
					{
						throw base.CreateUnknownAnyElementException(xmlNode.Name, xmlNode.NamespaceURI);
					}
					if (this._format == SerializationFormat.Literal)
					{
						base.WriteElementLiteral(xmlNode, string.Empty, string.Empty, false, true);
					}
					else
					{
						base.WriteElementEncoded(xmlNode, string.Empty, string.Empty, false, true);
					}
				}
				else
				{
					xmlNode.WriteTo(base.Writer);
				}
			}
		}

		protected virtual void WritePrimitiveElement(XmlTypeMapping typeMap, object ob, string element, string namesp)
		{
			base.Writer.WriteString(this.GetStringValue(typeMap, typeMap.TypeData, ob));
		}

		protected virtual void WriteEnumElement(XmlTypeMapping typeMap, object ob, string element, string namesp)
		{
			base.Writer.WriteString(this.GetEnumXmlValue(typeMap, ob));
		}

		private string GetStringValue(XmlTypeMapping typeMap, TypeData type, object value)
		{
			if (type.SchemaType == SchemaTypes.Array)
			{
				if (value == null)
				{
					return null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				this.WriteListContent(null, typeMap.TypeData, (ListMap)typeMap.ObjectMap, value, stringBuilder);
				return stringBuilder.ToString().Trim();
			}
			else
			{
				if (type.SchemaType == SchemaTypes.Enum)
				{
					return this.GetEnumXmlValue(typeMap, value);
				}
				if (type.Type == typeof(XmlQualifiedName))
				{
					return base.FromXmlQualifiedName((XmlQualifiedName)value);
				}
				if (value == null)
				{
					return null;
				}
				return XmlCustomFormatter.ToXmlString(type, value);
			}
		}

		private string GetEnumXmlValue(XmlTypeMapping typeMap, object ob)
		{
			if (ob == null)
			{
				return null;
			}
			EnumMap enumMap = (EnumMap)typeMap.ObjectMap;
			return enumMap.GetXmlName(typeMap.TypeFullName, ob);
		}

		private class CallbackInfo
		{
			private XmlSerializationWriterInterpreter _swi;

			private XmlTypeMapping _typeMap;

			public CallbackInfo(XmlSerializationWriterInterpreter swi, XmlTypeMapping typeMap)
			{
				this._swi = swi;
				this._typeMap = typeMap;
			}

			internal void WriteObject(object ob)
			{
				this._swi.WriteObject(this._typeMap, ob, this._typeMap.ElementName, this._typeMap.Namespace, false, false, false);
			}

			internal void WriteEnum(object ob)
			{
				this._swi.WriteObject(this._typeMap, ob, this._typeMap.ElementName, this._typeMap.Namespace, false, true, false);
			}
		}
	}
}
