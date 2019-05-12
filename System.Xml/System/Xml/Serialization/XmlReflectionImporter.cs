using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	/// <summary>Generates mappings to XML schema element declarations, including literal XML Schema Definition (XSD) message parts in a Web Services Description Language (WSDL) document, for .NET Framework types or Web service method information. </summary>
	public class XmlReflectionImporter
	{
		private string initialDefaultNamespace;

		private XmlAttributeOverrides attributeOverrides;

		private ArrayList includedTypes;

		private ReflectionHelper helper = new ReflectionHelper();

		private int arrayChoiceCount = 1;

		private ArrayList relatedMaps = new ArrayList();

		private bool allowPrivateTypes;

		private static readonly string errSimple = "Cannot serialize object of type '{0}'. Base type '{1}' has simpleContent and can be only extended by adding XmlAttribute elements. Please consider changing XmlText member of the base class to string array";

		private static readonly string errSimple2 = "Cannot serialize object of type '{0}'. Consider changing type of XmlText member '{1}' from '{2}' to string or string array";

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlReflectionImporter" /> class. </summary>
		public XmlReflectionImporter() : this(null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlReflectionImporter" /> class using the specified default XML namespace. </summary>
		/// <param name="defaultNamespace">The default XML namespace to use for imported type mappings.</param>
		public XmlReflectionImporter(string defaultNamespace) : this(null, defaultNamespace)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlReflectionImporter" /> class using the specified XML serialization overrides. </summary>
		/// <param name="attributeOverrides">An object that overrides how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class serializes mapped types.</param>
		public XmlReflectionImporter(XmlAttributeOverrides attributeOverrides) : this(attributeOverrides, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlReflectionImporter" /> class using the specified XML serialization overrides and default XML namespace. </summary>
		/// <param name="attributeOverrides">An object that overrides how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class serializes mapped types.</param>
		/// <param name="defaultNamespace">The default XML namespace to use for imported type mappings.</param>
		public XmlReflectionImporter(XmlAttributeOverrides attributeOverrides, string defaultNamespace)
		{
			if (defaultNamespace == null)
			{
				this.initialDefaultNamespace = string.Empty;
			}
			else
			{
				this.initialDefaultNamespace = defaultNamespace;
			}
			if (attributeOverrides == null)
			{
				this.attributeOverrides = new XmlAttributeOverrides();
			}
			else
			{
				this.attributeOverrides = attributeOverrides;
			}
		}

		internal bool AllowPrivateTypes
		{
			get
			{
				return this.allowPrivateTypes;
			}
			set
			{
				this.allowPrivateTypes = value;
			}
		}

		/// <summary>Generates internal type mappings for information from a Web service method. </summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlMembersMapping" /> with mappings to the element parts of a WSDL message definition.</returns>
		/// <param name="elementName">An XML element name produced from the Web service method.</param>
		/// <param name="ns">An XML element namespace produced from the Web service method.</param>
		/// <param name="members">An array of <see cref="T:System.Xml.Serialization.XmlReflectionMember" />  objects that contain .NET Framework code entities that belong to a Web service method.</param>
		/// <param name="hasWrapperElement">true if elements that correspond to Web Services Description Language (WSDL) message parts should be enclosed in an extra wrapper element in a SOAP message; otherwise, false.</param>
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members, bool hasWrapperElement)
		{
			return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, true);
		}

		/// <summary>Returns internal type mappings using information from a Web service method, and allows you to specify an XML element name, XML namespace, and other options.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlMembersMapping" /> that contains the mappings.</returns>
		/// <param name="elementName">An XML element name produced from the Web service method.</param>
		/// <param name="ns">An XML element namespace produced from the Web service method.</param>
		/// <param name="members">An array of <see cref="T:System.Xml.Serialization.XmlReflectionMember" />  objects that contain .NET Framework code entities that belong to a Web service method.</param>
		/// <param name="hasWrapperElement">true if elements that correspond to Web Services Description Language (WSDL) message parts should be enclosed in an extra wrapper element in a SOAP message; otherwise, false.</param>
		/// <param name="rpc">true if the method is a remote procedure call; otherwise, false.</param>
		[MonoTODO]
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members, bool hasWrapperElement, bool writeAccessors)
		{
			return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, writeAccessors, true);
		}

		/// <summary>Returns internal type mappings using information from a Web service method, and allows you to specify an XML element name, XML namespace, and other options.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlMembersMapping" /> that contains the mappings.</returns>
		/// <param name="elementName">An XML element name produced from the Web service method.</param>
		/// <param name="ns">An XML element namespace produced from the Web service method.</param>
		/// <param name="members">An array of <see cref="T:System.Xml.Serialization.XmlReflectionMember" />  objects that contain .NET Framework code entities that belong to a Web service method.</param>
		/// <param name="hasWrapperElement">true if elements that correspond to Web Services Description Language (WSDL) message parts should be enclosed in an extra wrapper element in a SOAP message; otherwise, false.</param>
		/// <param name="rpc">true if the method is a remote procedure call; otherwise, false.</param>
		/// <param name="openModel">true to specify that the generated schema type will be marked with the &lt;xs:anyAttribute&gt; element; otherwise, false.</param>
		[MonoTODO]
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members, bool hasWrapperElement, bool writeAccessors, bool validate)
		{
			return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, writeAccessors, validate, XmlMappingAccess.Read | XmlMappingAccess.Write);
		}

		/// <summary>Generates internal type mappings for information from a Web service method.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlMembersMapping" /> that contains the mappings.</returns>
		/// <param name="elementName">An XML element name produced from the Web service method.</param>
		/// <param name="ns">An XML element namespace produced from the Web service method.</param>
		/// <param name="members">An array of <see cref="T:System.Xml.Serialization.XmlReflectionMember" />  objects that contain .NET Framework code entities that belong to a Web service method.</param>
		/// <param name="hasWrapperElement">true if elements that correspond to Web Services Description Language (WSDL) message parts should be enclosed in an extra wrapper element in a SOAP message; otherwise, false.</param>
		/// <param name="rpc">true if the method is a remote procedure call; otherwise, false.</param>
		/// <param name="openModel">true to specify that the generated schema type will be marked with the &lt;xs:anyAttribute&gt; element; otherwise, false.</param>
		/// <param name="access">One of the <see cref="T:System.Xml.Serialization.XmlMappingAccess" /> values. The default is None.</param>
		[MonoTODO]
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members, bool hasWrapperElement, bool writeAccessors, bool validate, XmlMappingAccess access)
		{
			XmlMemberMapping[] array = new XmlMemberMapping[members.Length];
			for (int i = 0; i < members.Length; i++)
			{
				XmlTypeMapMember mapMem = this.CreateMapMember(null, members[i], ns);
				array[i] = new XmlMemberMapping(members[i].MemberName, ns, mapMem, false);
			}
			elementName = XmlConvert.EncodeLocalName(elementName);
			XmlMembersMapping xmlMembersMapping = new XmlMembersMapping(elementName, ns, hasWrapperElement, false, array);
			xmlMembersMapping.RelatedMaps = this.relatedMaps;
			xmlMembersMapping.Format = SerializationFormat.Literal;
			Type[] array2 = (this.includedTypes == null) ? null : ((Type[])this.includedTypes.ToArray(typeof(Type)));
			xmlMembersMapping.Source = new MembersSerializationSource(elementName, hasWrapperElement, members, false, true, ns, array2);
			if (this.allowPrivateTypes)
			{
				xmlMembersMapping.Source.CanBeGenerated = false;
			}
			return xmlMembersMapping;
		}

		/// <summary>Generates a mapping to an XML Schema element for a specified .NET Framework type. </summary>
		/// <returns>Internal .NET Framework mapping of a type to an XML Schema element.</returns>
		/// <param name="type">The .NET Framework type for which to generate a type mapping.</param>
		public XmlTypeMapping ImportTypeMapping(Type type)
		{
			return this.ImportTypeMapping(type, null, null);
		}

		/// <summary>Generates a mapping to an XML Schema element for a .NET Framework type, using the specified type and namespace. </summary>
		/// <returns>Internal .NET Framework mapping of a type to an XML Schema element.</returns>
		/// <param name="type">The .NET Framework type for which to generate a type mapping.</param>
		/// <param name="defaultNamespace">The default XML namespace to use.</param>
		public XmlTypeMapping ImportTypeMapping(Type type, string defaultNamespace)
		{
			return this.ImportTypeMapping(type, null, defaultNamespace);
		}

		/// <summary>Generates a mapping to an XML Schema element for a .NET Framework type, using the specified type and attribute. </summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlTypeMapping" /> that represents a mapping of a .NET Framework type to an XML Schema element.</returns>
		/// <param name="type">The .NET Framework type for which to generate a type mapping.</param>
		/// <param name="root">An <see cref="T:System.Xml.Serialization.XmlRootAttribute" /> attribute that is applied to the type.</param>
		public XmlTypeMapping ImportTypeMapping(Type type, XmlRootAttribute group)
		{
			return this.ImportTypeMapping(type, group, null);
		}

		/// <summary>Generates a mapping to an XML Schema element for a .NET Framework type, using the specified type, attribute, and namespace. </summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlTypeMapping" /> that contains the internal .NET Framework mapping of a type to an XML Schema element.</returns>
		/// <param name="type">The .NET Framework type for which to generate a type mapping.</param>
		/// <param name="root">An <see cref="T:System.Xml.Serialization.XmlRootAttribute" /> attribute that is applied to the type.</param>
		/// <param name="defaultNamespace">The default XML namespace to use.</param>
		public XmlTypeMapping ImportTypeMapping(Type type, XmlRootAttribute root, string defaultNamespace)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type == typeof(void))
			{
				throw new NotSupportedException("The type " + type.FullName + " may not be serialized.");
			}
			return this.ImportTypeMapping(TypeTranslator.GetTypeData(type), root, defaultNamespace);
		}

		internal XmlTypeMapping ImportTypeMapping(TypeData typeData, string defaultNamespace)
		{
			return this.ImportTypeMapping(typeData, null, defaultNamespace);
		}

		private XmlTypeMapping ImportTypeMapping(TypeData typeData, XmlRootAttribute root, string defaultNamespace)
		{
			if (typeData == null)
			{
				throw new ArgumentNullException("typeData");
			}
			if (typeData.Type == null)
			{
				throw new ArgumentException("Specified TypeData instance does not have Type set.");
			}
			if (defaultNamespace == null)
			{
				defaultNamespace = this.initialDefaultNamespace;
			}
			if (defaultNamespace == null)
			{
				defaultNamespace = string.Empty;
			}
			XmlTypeMapping result;
			try
			{
				XmlTypeMapping xmlTypeMapping;
				switch (typeData.SchemaType)
				{
				case SchemaTypes.Primitive:
					xmlTypeMapping = this.ImportPrimitiveMapping(typeData, root, defaultNamespace);
					break;
				case SchemaTypes.Enum:
					xmlTypeMapping = this.ImportEnumMapping(typeData, root, defaultNamespace);
					break;
				case SchemaTypes.Array:
					xmlTypeMapping = this.ImportListMapping(typeData, root, defaultNamespace, null, 0);
					break;
				case SchemaTypes.Class:
					xmlTypeMapping = this.ImportClassMapping(typeData, root, defaultNamespace);
					break;
				case SchemaTypes.XmlSerializable:
					xmlTypeMapping = this.ImportXmlSerializableMapping(typeData, root, defaultNamespace);
					break;
				case SchemaTypes.XmlNode:
					xmlTypeMapping = this.ImportXmlNodeMapping(typeData, root, defaultNamespace);
					break;
				default:
					throw new NotSupportedException("Type " + typeData.Type.FullName + " not supported for XML stialization");
				}
				xmlTypeMapping.SetKey(typeData.Type.ToString());
				xmlTypeMapping.RelatedMaps = this.relatedMaps;
				xmlTypeMapping.Format = SerializationFormat.Literal;
				Type[] array = (this.includedTypes == null) ? null : ((Type[])this.includedTypes.ToArray(typeof(Type)));
				xmlTypeMapping.Source = new XmlTypeSerializationSource(typeData.Type, root, this.attributeOverrides, defaultNamespace, array);
				if (this.allowPrivateTypes)
				{
					xmlTypeMapping.Source.CanBeGenerated = false;
				}
				result = xmlTypeMapping;
			}
			catch (InvalidOperationException innerException)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "There was an error reflecting type '{0}'.", new object[]
				{
					typeData.Type.FullName
				}), innerException);
			}
			return result;
		}

		private XmlTypeMapping CreateTypeMapping(TypeData typeData, XmlRootAttribute root, string defaultXmlType, string defaultNamespace)
		{
			string text = defaultNamespace;
			string text2 = null;
			bool includeInSchema = true;
			XmlAttributes xmlAttributes = null;
			bool isNullable = this.CanBeNull(typeData);
			if (defaultXmlType == null)
			{
				defaultXmlType = typeData.XmlType;
			}
			if (!typeData.IsListType)
			{
				if (this.attributeOverrides != null)
				{
					xmlAttributes = this.attributeOverrides[typeData.Type];
				}
				if (xmlAttributes != null && typeData.SchemaType == SchemaTypes.Primitive)
				{
					throw new InvalidOperationException("XmlRoot and XmlType attributes may not be specified for the type " + typeData.FullTypeName);
				}
			}
			if (xmlAttributes == null)
			{
				xmlAttributes = new XmlAttributes(typeData.Type);
			}
			if (xmlAttributes.XmlRoot != null && root == null)
			{
				root = xmlAttributes.XmlRoot;
			}
			if (xmlAttributes.XmlType != null)
			{
				if (xmlAttributes.XmlType.Namespace != null)
				{
					text2 = xmlAttributes.XmlType.Namespace;
				}
				if (xmlAttributes.XmlType.TypeName != null && xmlAttributes.XmlType.TypeName != string.Empty)
				{
					defaultXmlType = XmlConvert.EncodeLocalName(xmlAttributes.XmlType.TypeName);
				}
				includeInSchema = xmlAttributes.XmlType.IncludeInSchema;
			}
			string elementName = defaultXmlType;
			if (root != null)
			{
				if (root.ElementName.Length != 0)
				{
					elementName = XmlConvert.EncodeLocalName(root.ElementName);
				}
				if (root.Namespace != null)
				{
					text = root.Namespace;
				}
				isNullable = root.IsNullable;
			}
			if (text == null)
			{
				text = string.Empty;
			}
			if (text2 == null)
			{
				text2 = text;
			}
			SchemaTypes schemaType = typeData.SchemaType;
			XmlTypeMapping xmlTypeMapping;
			if (schemaType != SchemaTypes.Primitive)
			{
				if (schemaType != SchemaTypes.XmlSerializable)
				{
					xmlTypeMapping = new XmlTypeMapping(elementName, text, typeData, defaultXmlType, text2);
				}
				else
				{
					xmlTypeMapping = new XmlSerializableMapping(elementName, text, typeData, defaultXmlType, text2);
				}
			}
			else if (!typeData.IsXsdType)
			{
				xmlTypeMapping = new XmlTypeMapping(elementName, text, typeData, defaultXmlType, "http://microsoft.com/wsdl/types/");
			}
			else
			{
				xmlTypeMapping = new XmlTypeMapping(elementName, text, typeData, defaultXmlType, text2);
			}
			xmlTypeMapping.IncludeInSchema = includeInSchema;
			xmlTypeMapping.IsNullable = isNullable;
			this.relatedMaps.Add(xmlTypeMapping);
			return xmlTypeMapping;
		}

		private XmlTypeMapping ImportClassMapping(Type type, XmlRootAttribute root, string defaultNamespace)
		{
			TypeData typeData = TypeTranslator.GetTypeData(type);
			return this.ImportClassMapping(typeData, root, defaultNamespace);
		}

		private XmlTypeMapping ImportClassMapping(TypeData typeData, XmlRootAttribute root, string defaultNamespace)
		{
			Type type = typeData.Type;
			XmlTypeMapping xmlTypeMapping = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, root, defaultNamespace));
			if (xmlTypeMapping != null)
			{
				return xmlTypeMapping;
			}
			if (!this.allowPrivateTypes)
			{
				ReflectionHelper.CheckSerializableType(type, false);
			}
			xmlTypeMapping = this.CreateTypeMapping(typeData, root, null, defaultNamespace);
			this.helper.RegisterClrType(xmlTypeMapping, type, xmlTypeMapping.XmlTypeNamespace);
			this.helper.RegisterSchemaType(xmlTypeMapping, xmlTypeMapping.XmlType, xmlTypeMapping.XmlTypeNamespace);
			ClassMap classMap = new ClassMap();
			xmlTypeMapping.ObjectMap = classMap;
			ICollection reflectionMembers = this.GetReflectionMembers(type);
			foreach (object obj in reflectionMembers)
			{
				XmlReflectionMember xmlReflectionMember = (XmlReflectionMember)obj;
				string xmlTypeNamespace = xmlTypeMapping.XmlTypeNamespace;
				if (!xmlReflectionMember.XmlAttributes.XmlIgnore)
				{
					if (xmlReflectionMember.DeclaringType != null && xmlReflectionMember.DeclaringType != type)
					{
						XmlTypeMapping xmlTypeMapping2 = this.ImportClassMapping(xmlReflectionMember.DeclaringType, root, defaultNamespace);
						xmlTypeNamespace = xmlTypeMapping2.XmlTypeNamespace;
					}
					try
					{
						XmlTypeMapMember xmlTypeMapMember = this.CreateMapMember(type, xmlReflectionMember, xmlTypeNamespace);
						xmlTypeMapMember.CheckOptionalValueType(type);
						classMap.AddMember(xmlTypeMapMember);
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "There was an error reflecting field '{0}'.", new object[]
						{
							xmlReflectionMember.MemberName
						}), innerException);
					}
				}
			}
			if (type == typeof(object) && this.includedTypes != null)
			{
				foreach (object obj2 in this.includedTypes)
				{
					Type type2 = (Type)obj2;
					xmlTypeMapping.DerivedTypes.Add(this.ImportTypeMapping(type2, defaultNamespace));
				}
			}
			if (type.BaseType != null)
			{
				XmlTypeMapping xmlTypeMapping3 = this.ImportClassMapping(type.BaseType, root, defaultNamespace);
				ClassMap classMap2 = xmlTypeMapping3.ObjectMap as ClassMap;
				if (type.BaseType != typeof(object))
				{
					xmlTypeMapping.BaseMap = xmlTypeMapping3;
					if (!classMap2.HasSimpleContent)
					{
						classMap.SetCanBeSimpleType(false);
					}
				}
				this.RegisterDerivedMap(xmlTypeMapping3, xmlTypeMapping);
				if (classMap2.HasSimpleContent && classMap.ElementMembers != null && classMap.ElementMembers.Count != 1)
				{
					throw new InvalidOperationException(string.Format(XmlReflectionImporter.errSimple, xmlTypeMapping.TypeData.TypeName, xmlTypeMapping.BaseMap.TypeData.TypeName));
				}
			}
			this.ImportIncludedTypes(type, defaultNamespace);
			if (classMap.XmlTextCollector != null && !classMap.HasSimpleContent)
			{
				XmlTypeMapMember xmlTextCollector = classMap.XmlTextCollector;
				if (xmlTextCollector.TypeData.Type != typeof(string) && xmlTextCollector.TypeData.Type != typeof(string[]) && xmlTextCollector.TypeData.Type != typeof(object[]) && xmlTextCollector.TypeData.Type != typeof(XmlNode[]))
				{
					throw new InvalidOperationException(string.Format(XmlReflectionImporter.errSimple2, xmlTypeMapping.TypeData.TypeName, xmlTextCollector.Name, xmlTextCollector.TypeData.TypeName));
				}
			}
			return xmlTypeMapping;
		}

		private void RegisterDerivedMap(XmlTypeMapping map, XmlTypeMapping derivedMap)
		{
			map.DerivedTypes.Add(derivedMap);
			map.DerivedTypes.AddRange(derivedMap.DerivedTypes);
			if (map.BaseMap != null)
			{
				this.RegisterDerivedMap(map.BaseMap, derivedMap);
			}
			else
			{
				XmlTypeMapping xmlTypeMapping = this.ImportTypeMapping(typeof(object));
				if (xmlTypeMapping != map)
				{
					xmlTypeMapping.DerivedTypes.Add(derivedMap);
				}
			}
		}

		private string GetTypeNamespace(TypeData typeData, XmlRootAttribute root, string defaultNamespace)
		{
			string text = null;
			XmlAttributes xmlAttributes = null;
			if (!typeData.IsListType && this.attributeOverrides != null)
			{
				xmlAttributes = this.attributeOverrides[typeData.Type];
			}
			if (xmlAttributes == null)
			{
				xmlAttributes = new XmlAttributes(typeData.Type);
			}
			if (xmlAttributes.XmlType != null && xmlAttributes.XmlType.Namespace != null && xmlAttributes.XmlType.Namespace.Length != 0 && typeData.SchemaType != SchemaTypes.Enum)
			{
				text = xmlAttributes.XmlType.Namespace;
			}
			if (text != null && text.Length != 0)
			{
				return text;
			}
			if (xmlAttributes.XmlRoot != null && root == null)
			{
				root = xmlAttributes.XmlRoot;
			}
			if (root != null && root.Namespace != null && root.Namespace.Length != 0)
			{
				return root.Namespace;
			}
			if (defaultNamespace == null)
			{
				return string.Empty;
			}
			return defaultNamespace;
		}

		private XmlTypeMapping ImportListMapping(Type type, XmlRootAttribute root, string defaultNamespace, XmlAttributes atts, int nestingLevel)
		{
			TypeData typeData = TypeTranslator.GetTypeData(type);
			return this.ImportListMapping(typeData, root, defaultNamespace, atts, nestingLevel);
		}

		private XmlTypeMapping ImportListMapping(TypeData typeData, XmlRootAttribute root, string defaultNamespace, XmlAttributes atts, int nestingLevel)
		{
			Type type = typeData.Type;
			ListMap listMap = new ListMap();
			if (!this.allowPrivateTypes)
			{
				ReflectionHelper.CheckSerializableType(type, true);
			}
			if (atts == null)
			{
				atts = new XmlAttributes();
			}
			Type listItemType = typeData.ListItemType;
			bool flag = type.IsArray && TypeTranslator.GetTypeData(listItemType).SchemaType == SchemaTypes.Array && listItemType.IsArray;
			XmlTypeMapElementInfoList xmlTypeMapElementInfoList = new XmlTypeMapElementInfoList();
			foreach (object obj in atts.XmlArrayItems)
			{
				XmlArrayItemAttribute xmlArrayItemAttribute = (XmlArrayItemAttribute)obj;
				if (xmlArrayItemAttribute.Namespace != null && xmlArrayItemAttribute.Form == XmlSchemaForm.Unqualified)
				{
					throw new InvalidOperationException("XmlArrayItemAttribute.Form must not be Unqualified when it has an explicit Namespace value.");
				}
				if (xmlArrayItemAttribute.NestingLevel == nestingLevel)
				{
					Type type2 = (xmlArrayItemAttribute.Type == null) ? listItemType : xmlArrayItemAttribute.Type;
					XmlTypeMapElementInfo xmlTypeMapElementInfo = new XmlTypeMapElementInfo(null, TypeTranslator.GetTypeData(type2, xmlArrayItemAttribute.DataType));
					xmlTypeMapElementInfo.Namespace = ((xmlArrayItemAttribute.Namespace == null) ? defaultNamespace : xmlArrayItemAttribute.Namespace);
					if (xmlTypeMapElementInfo.Namespace == null)
					{
						xmlTypeMapElementInfo.Namespace = string.Empty;
					}
					xmlTypeMapElementInfo.Form = xmlArrayItemAttribute.Form;
					if (xmlArrayItemAttribute.Form == XmlSchemaForm.Unqualified)
					{
						xmlTypeMapElementInfo.Namespace = string.Empty;
					}
					xmlTypeMapElementInfo.IsNullable = (xmlArrayItemAttribute.IsNullable && this.CanBeNull(xmlTypeMapElementInfo.TypeData));
					xmlTypeMapElementInfo.NestingLevel = xmlArrayItemAttribute.NestingLevel;
					if (flag)
					{
						xmlTypeMapElementInfo.MappedType = this.ImportListMapping(type2, null, xmlTypeMapElementInfo.Namespace, atts, nestingLevel + 1);
					}
					else if (xmlTypeMapElementInfo.TypeData.IsComplexType)
					{
						xmlTypeMapElementInfo.MappedType = this.ImportTypeMapping(type2, null, xmlTypeMapElementInfo.Namespace);
					}
					if (xmlArrayItemAttribute.ElementName.Length != 0)
					{
						xmlTypeMapElementInfo.ElementName = XmlConvert.EncodeLocalName(xmlArrayItemAttribute.ElementName);
					}
					else if (xmlTypeMapElementInfo.MappedType != null)
					{
						xmlTypeMapElementInfo.ElementName = xmlTypeMapElementInfo.MappedType.ElementName;
					}
					else
					{
						xmlTypeMapElementInfo.ElementName = TypeTranslator.GetTypeData(type2).XmlType;
					}
					xmlTypeMapElementInfoList.Add(xmlTypeMapElementInfo);
				}
			}
			if (xmlTypeMapElementInfoList.Count == 0)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo2 = new XmlTypeMapElementInfo(null, TypeTranslator.GetTypeData(listItemType));
				if (flag)
				{
					xmlTypeMapElementInfo2.MappedType = this.ImportListMapping(listItemType, null, defaultNamespace, atts, nestingLevel + 1);
				}
				else if (xmlTypeMapElementInfo2.TypeData.IsComplexType)
				{
					xmlTypeMapElementInfo2.MappedType = this.ImportTypeMapping(listItemType, null, defaultNamespace);
				}
				if (xmlTypeMapElementInfo2.MappedType != null)
				{
					xmlTypeMapElementInfo2.ElementName = xmlTypeMapElementInfo2.MappedType.XmlType;
				}
				else
				{
					xmlTypeMapElementInfo2.ElementName = TypeTranslator.GetTypeData(listItemType).XmlType;
				}
				xmlTypeMapElementInfo2.Namespace = ((defaultNamespace == null) ? string.Empty : defaultNamespace);
				xmlTypeMapElementInfo2.IsNullable = this.CanBeNull(xmlTypeMapElementInfo2.TypeData);
				xmlTypeMapElementInfoList.Add(xmlTypeMapElementInfo2);
			}
			listMap.ItemInfo = xmlTypeMapElementInfoList;
			string text;
			if (xmlTypeMapElementInfoList.Count > 1)
			{
				text = "ArrayOfChoice" + this.arrayChoiceCount++;
			}
			else
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo3 = (XmlTypeMapElementInfo)xmlTypeMapElementInfoList[0];
				if (xmlTypeMapElementInfo3.MappedType != null)
				{
					text = TypeTranslator.GetArrayName(xmlTypeMapElementInfo3.MappedType.XmlType);
				}
				else
				{
					text = TypeTranslator.GetArrayName(xmlTypeMapElementInfo3.ElementName);
				}
			}
			int num = 1;
			string text2 = text;
			XmlTypeMapping registeredSchemaType;
			for (;;)
			{
				registeredSchemaType = this.helper.GetRegisteredSchemaType(text2, defaultNamespace);
				if (registeredSchemaType == null)
				{
					num = -1;
				}
				else
				{
					if (listMap.Equals(registeredSchemaType.ObjectMap) && typeData.Type == registeredSchemaType.TypeData.Type)
					{
						break;
					}
					text2 = text + num++;
				}
				if (num == -1)
				{
					goto Block_16;
				}
			}
			return registeredSchemaType;
			Block_16:
			XmlTypeMapping xmlTypeMapping = this.CreateTypeMapping(typeData, root, text2, defaultNamespace);
			xmlTypeMapping.ObjectMap = listMap;
			XmlIncludeAttribute[] array = (XmlIncludeAttribute[])type.GetCustomAttributes(typeof(XmlIncludeAttribute), false);
			XmlTypeMapping xmlTypeMapping2 = this.ImportTypeMapping(typeof(object));
			for (int i = 0; i < array.Length; i++)
			{
				Type type3 = array[i].Type;
				xmlTypeMapping2.DerivedTypes.Add(this.ImportTypeMapping(type3, null, defaultNamespace));
			}
			this.helper.RegisterSchemaType(xmlTypeMapping, text2, defaultNamespace);
			this.ImportTypeMapping(typeof(object)).DerivedTypes.Add(xmlTypeMapping);
			return xmlTypeMapping;
		}

		private XmlTypeMapping ImportXmlNodeMapping(TypeData typeData, XmlRootAttribute root, string defaultNamespace)
		{
			Type type = typeData.Type;
			XmlTypeMapping xmlTypeMapping = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, root, defaultNamespace));
			if (xmlTypeMapping != null)
			{
				return xmlTypeMapping;
			}
			xmlTypeMapping = this.CreateTypeMapping(typeData, root, null, defaultNamespace);
			this.helper.RegisterClrType(xmlTypeMapping, type, xmlTypeMapping.XmlTypeNamespace);
			if (type.BaseType != null)
			{
				XmlTypeMapping xmlTypeMapping2 = this.ImportTypeMapping(type.BaseType, root, defaultNamespace);
				if (type.BaseType != typeof(object))
				{
					xmlTypeMapping.BaseMap = xmlTypeMapping2;
				}
				this.RegisterDerivedMap(xmlTypeMapping2, xmlTypeMapping);
			}
			return xmlTypeMapping;
		}

		private XmlTypeMapping ImportPrimitiveMapping(TypeData typeData, XmlRootAttribute root, string defaultNamespace)
		{
			Type type = typeData.Type;
			XmlTypeMapping xmlTypeMapping = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, root, defaultNamespace));
			if (xmlTypeMapping != null)
			{
				return xmlTypeMapping;
			}
			xmlTypeMapping = this.CreateTypeMapping(typeData, root, null, defaultNamespace);
			this.helper.RegisterClrType(xmlTypeMapping, type, xmlTypeMapping.XmlTypeNamespace);
			return xmlTypeMapping;
		}

		private XmlTypeMapping ImportEnumMapping(TypeData typeData, XmlRootAttribute root, string defaultNamespace)
		{
			Type type = typeData.Type;
			XmlTypeMapping xmlTypeMapping = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, root, defaultNamespace));
			if (xmlTypeMapping != null)
			{
				return xmlTypeMapping;
			}
			if (!this.allowPrivateTypes)
			{
				ReflectionHelper.CheckSerializableType(type, false);
			}
			xmlTypeMapping = this.CreateTypeMapping(typeData, root, null, defaultNamespace);
			xmlTypeMapping.IsNullable = false;
			this.helper.RegisterClrType(xmlTypeMapping, type, xmlTypeMapping.XmlTypeNamespace);
			string[] names = Enum.GetNames(type);
			ArrayList arrayList = new ArrayList();
			foreach (string text in names)
			{
				FieldInfo field = type.GetField(text);
				string text2 = null;
				if (!field.IsDefined(typeof(XmlIgnoreAttribute), false))
				{
					object[] customAttributes = field.GetCustomAttributes(typeof(XmlEnumAttribute), false);
					if (customAttributes.Length > 0)
					{
						text2 = ((XmlEnumAttribute)customAttributes[0]).Name;
					}
					if (text2 == null)
					{
						text2 = text;
					}
					long value = ((IConvertible)field.GetValue(null)).ToInt64(CultureInfo.InvariantCulture);
					arrayList.Add(new EnumMap.EnumMapMember(text2, text, value));
				}
			}
			bool isFlags = type.IsDefined(typeof(FlagsAttribute), false);
			xmlTypeMapping.ObjectMap = new EnumMap((EnumMap.EnumMapMember[])arrayList.ToArray(typeof(EnumMap.EnumMapMember)), isFlags);
			this.ImportTypeMapping(typeof(object)).DerivedTypes.Add(xmlTypeMapping);
			return xmlTypeMapping;
		}

		private XmlTypeMapping ImportXmlSerializableMapping(TypeData typeData, XmlRootAttribute root, string defaultNamespace)
		{
			Type type = typeData.Type;
			XmlTypeMapping xmlTypeMapping = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, root, defaultNamespace));
			if (xmlTypeMapping != null)
			{
				return xmlTypeMapping;
			}
			if (!this.allowPrivateTypes)
			{
				ReflectionHelper.CheckSerializableType(type, false);
			}
			xmlTypeMapping = this.CreateTypeMapping(typeData, root, null, defaultNamespace);
			this.helper.RegisterClrType(xmlTypeMapping, type, xmlTypeMapping.XmlTypeNamespace);
			return xmlTypeMapping;
		}

		private void ImportIncludedTypes(Type type, string defaultNamespace)
		{
			XmlIncludeAttribute[] array = (XmlIncludeAttribute[])type.GetCustomAttributes(typeof(XmlIncludeAttribute), false);
			for (int i = 0; i < array.Length; i++)
			{
				Type type2 = array[i].Type;
				this.ImportTypeMapping(type2, null, defaultNamespace);
			}
		}

		private ICollection GetReflectionMembers(Type type)
		{
			Type type2 = type;
			ArrayList arrayList = new ArrayList();
			arrayList.Add(type2);
			while (type2 != typeof(object))
			{
				type2 = type2.BaseType;
				arrayList.Insert(0, type2);
			}
			ArrayList arrayList2 = new ArrayList();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			type2 = null;
			int num = 0;
			foreach (FieldInfo fieldInfo in fields)
			{
				if (type2 != fieldInfo.DeclaringType)
				{
					type2 = fieldInfo.DeclaringType;
					num = 0;
				}
				arrayList2.Insert(num++, fieldInfo);
			}
			ArrayList arrayList3 = new ArrayList();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			type2 = null;
			num = 0;
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (type2 != propertyInfo.DeclaringType)
				{
					type2 = propertyInfo.DeclaringType;
					num = 0;
				}
				if (propertyInfo.CanRead)
				{
					if (propertyInfo.GetIndexParameters().Length <= 0)
					{
						arrayList3.Insert(num++, propertyInfo);
					}
				}
			}
			ArrayList arrayList4 = new ArrayList();
			int k = 0;
			int l = 0;
			foreach (object obj in arrayList)
			{
				Type type3 = (Type)obj;
				while (k < arrayList2.Count)
				{
					FieldInfo fieldInfo2 = (FieldInfo)arrayList2[k];
					if (fieldInfo2.DeclaringType != type3)
					{
						break;
					}
					k++;
					XmlAttributes xmlAttributes = this.attributeOverrides[type, fieldInfo2.Name];
					if (xmlAttributes == null)
					{
						xmlAttributes = new XmlAttributes(fieldInfo2);
					}
					if (!xmlAttributes.XmlIgnore)
					{
						arrayList4.Add(new XmlReflectionMember(fieldInfo2.Name, fieldInfo2.FieldType, xmlAttributes)
						{
							DeclaringType = fieldInfo2.DeclaringType
						});
					}
				}
				while (l < arrayList3.Count)
				{
					PropertyInfo propertyInfo2 = (PropertyInfo)arrayList3[l];
					if (propertyInfo2.DeclaringType != type3)
					{
						break;
					}
					l++;
					XmlAttributes xmlAttributes2 = this.attributeOverrides[type, propertyInfo2.Name];
					if (xmlAttributes2 == null)
					{
						xmlAttributes2 = new XmlAttributes(propertyInfo2);
					}
					if (!xmlAttributes2.XmlIgnore)
					{
						if (propertyInfo2.CanWrite || (TypeTranslator.GetTypeData(propertyInfo2.PropertyType).SchemaType == SchemaTypes.Array && !propertyInfo2.PropertyType.IsArray))
						{
							arrayList4.Add(new XmlReflectionMember(propertyInfo2.Name, propertyInfo2.PropertyType, xmlAttributes2)
							{
								DeclaringType = propertyInfo2.DeclaringType
							});
						}
					}
				}
			}
			return arrayList4;
		}

		private XmlTypeMapMember CreateMapMember(Type declaringType, XmlReflectionMember rmember, string defaultNamespace)
		{
			XmlAttributes xmlAttributes = rmember.XmlAttributes;
			TypeData typeData = TypeTranslator.GetTypeData(rmember.MemberType);
			if (xmlAttributes.XmlArray != null)
			{
				if (xmlAttributes.XmlArray.Namespace != null && xmlAttributes.XmlArray.Form == XmlSchemaForm.Unqualified)
				{
					throw new InvalidOperationException("XmlArrayAttribute.Form must not be Unqualified when it has an explicit Namespace value.");
				}
				if (typeData.SchemaType != SchemaTypes.Array && (typeData.SchemaType != SchemaTypes.Primitive || typeData.Type != typeof(byte[])))
				{
					throw new InvalidOperationException("XmlArrayAttribute can be applied to members of array or collection type.");
				}
			}
			XmlTypeMapMember xmlTypeMapMember;
			if (xmlAttributes.XmlAnyAttribute != null)
			{
				if (!(rmember.MemberType.FullName == "System.Xml.XmlAttribute[]") && !(rmember.MemberType.FullName == "System.Xml.XmlNode[]"))
				{
					throw new InvalidOperationException("XmlAnyAttributeAttribute can only be applied to members of type XmlAttribute[] or XmlNode[]");
				}
				xmlTypeMapMember = new XmlTypeMapMemberAnyAttribute();
			}
			else if (xmlAttributes.XmlAnyElements != null && xmlAttributes.XmlAnyElements.Count > 0)
			{
				if (!(rmember.MemberType.FullName == "System.Xml.XmlElement[]") && !(rmember.MemberType.FullName == "System.Xml.XmlNode[]") && !(rmember.MemberType.FullName == "System.Xml.XmlElement"))
				{
					throw new InvalidOperationException("XmlAnyElementAttribute can only be applied to members of type XmlElement, XmlElement[] or XmlNode[]");
				}
				XmlTypeMapMemberAnyElement xmlTypeMapMemberAnyElement = new XmlTypeMapMemberAnyElement();
				xmlTypeMapMemberAnyElement.ElementInfo = this.ImportAnyElementInfo(defaultNamespace, rmember, xmlTypeMapMemberAnyElement, xmlAttributes);
				xmlTypeMapMember = xmlTypeMapMemberAnyElement;
			}
			else if (xmlAttributes.Xmlns)
			{
				XmlTypeMapMemberNamespaces xmlTypeMapMemberNamespaces = new XmlTypeMapMemberNamespaces();
				xmlTypeMapMember = xmlTypeMapMemberNamespaces;
			}
			else if (xmlAttributes.XmlAttribute != null)
			{
				if (xmlAttributes.XmlElements != null && xmlAttributes.XmlElements.Count > 0)
				{
					throw new Exception("XmlAttributeAttribute and XmlElementAttribute cannot be applied to the same member");
				}
				XmlTypeMapMemberAttribute xmlTypeMapMemberAttribute = new XmlTypeMapMemberAttribute();
				if (xmlAttributes.XmlAttribute.AttributeName.Length == 0)
				{
					xmlTypeMapMemberAttribute.AttributeName = rmember.MemberName;
				}
				else
				{
					xmlTypeMapMemberAttribute.AttributeName = xmlAttributes.XmlAttribute.AttributeName;
				}
				xmlTypeMapMemberAttribute.AttributeName = XmlConvert.EncodeLocalName(xmlTypeMapMemberAttribute.AttributeName);
				if (typeData.IsComplexType)
				{
					xmlTypeMapMemberAttribute.MappedType = this.ImportTypeMapping(typeData.Type, null, defaultNamespace);
				}
				if (xmlAttributes.XmlAttribute.Namespace != null && xmlAttributes.XmlAttribute.Namespace != defaultNamespace)
				{
					if (xmlAttributes.XmlAttribute.Form == XmlSchemaForm.Unqualified)
					{
						throw new InvalidOperationException("The Form property may not be 'Unqualified' when an explicit Namespace property is present");
					}
					xmlTypeMapMemberAttribute.Form = XmlSchemaForm.Qualified;
					xmlTypeMapMemberAttribute.Namespace = xmlAttributes.XmlAttribute.Namespace;
				}
				else
				{
					xmlTypeMapMemberAttribute.Form = xmlAttributes.XmlAttribute.Form;
					if (xmlAttributes.XmlAttribute.Form == XmlSchemaForm.Qualified)
					{
						xmlTypeMapMemberAttribute.Namespace = defaultNamespace;
					}
					else
					{
						xmlTypeMapMemberAttribute.Namespace = string.Empty;
					}
				}
				typeData = TypeTranslator.GetTypeData(rmember.MemberType, xmlAttributes.XmlAttribute.DataType);
				xmlTypeMapMember = xmlTypeMapMemberAttribute;
			}
			else if (typeData.SchemaType == SchemaTypes.Array)
			{
				if (xmlAttributes.XmlElements.Count > 1 || (xmlAttributes.XmlElements.Count == 1 && xmlAttributes.XmlElements[0].Type != typeData.Type) || xmlAttributes.XmlText != null)
				{
					if (xmlAttributes.XmlArray != null)
					{
						throw new InvalidOperationException("XmlArrayAttribute cannot be used with members which also attributed with XmlElementAttribute or XmlTextAttribute.");
					}
					XmlTypeMapMemberFlatList xmlTypeMapMemberFlatList = new XmlTypeMapMemberFlatList();
					xmlTypeMapMemberFlatList.ListMap = new ListMap();
					xmlTypeMapMemberFlatList.ListMap.ItemInfo = this.ImportElementInfo(declaringType, XmlConvert.EncodeLocalName(rmember.MemberName), defaultNamespace, typeData.ListItemType, xmlTypeMapMemberFlatList, xmlAttributes);
					xmlTypeMapMemberFlatList.ElementInfo = xmlTypeMapMemberFlatList.ListMap.ItemInfo;
					xmlTypeMapMemberFlatList.ListMap.ChoiceMember = xmlTypeMapMemberFlatList.ChoiceMember;
					xmlTypeMapMember = xmlTypeMapMemberFlatList;
				}
				else
				{
					XmlTypeMapMemberList xmlTypeMapMemberList = new XmlTypeMapMemberList();
					xmlTypeMapMemberList.ElementInfo = new XmlTypeMapElementInfoList();
					XmlTypeMapElementInfo xmlTypeMapElementInfo = new XmlTypeMapElementInfo(xmlTypeMapMemberList, typeData);
					xmlTypeMapElementInfo.ElementName = XmlConvert.EncodeLocalName((xmlAttributes.XmlArray == null || xmlAttributes.XmlArray.ElementName.Length == 0) ? rmember.MemberName : xmlAttributes.XmlArray.ElementName);
					xmlTypeMapElementInfo.Namespace = ((xmlAttributes.XmlArray == null || xmlAttributes.XmlArray.Namespace == null) ? defaultNamespace : xmlAttributes.XmlArray.Namespace);
					xmlTypeMapElementInfo.MappedType = this.ImportListMapping(rmember.MemberType, null, xmlTypeMapElementInfo.Namespace, xmlAttributes, 0);
					xmlTypeMapElementInfo.IsNullable = (xmlAttributes.XmlArray != null && xmlAttributes.XmlArray.IsNullable);
					xmlTypeMapElementInfo.Form = ((xmlAttributes.XmlArray == null) ? XmlSchemaForm.Qualified : xmlAttributes.XmlArray.Form);
					if (xmlAttributes.XmlArray != null && xmlAttributes.XmlArray.Form == XmlSchemaForm.Unqualified)
					{
						xmlTypeMapElementInfo.Namespace = string.Empty;
					}
					xmlTypeMapMemberList.ElementInfo.Add(xmlTypeMapElementInfo);
					xmlTypeMapMember = xmlTypeMapMemberList;
				}
			}
			else
			{
				XmlTypeMapMemberElement xmlTypeMapMemberElement = new XmlTypeMapMemberElement();
				xmlTypeMapMemberElement.ElementInfo = this.ImportElementInfo(declaringType, XmlConvert.EncodeLocalName(rmember.MemberName), defaultNamespace, rmember.MemberType, xmlTypeMapMemberElement, xmlAttributes);
				xmlTypeMapMember = xmlTypeMapMemberElement;
			}
			xmlTypeMapMember.DefaultValue = this.GetDefaultValue(typeData, xmlAttributes.XmlDefaultValue);
			xmlTypeMapMember.TypeData = typeData;
			xmlTypeMapMember.Name = rmember.MemberName;
			xmlTypeMapMember.IsReturnValue = rmember.IsReturnValue;
			return xmlTypeMapMember;
		}

		private XmlTypeMapElementInfoList ImportElementInfo(Type cls, string defaultName, string defaultNamespace, Type defaultType, XmlTypeMapMemberElement member, XmlAttributes atts)
		{
			EnumMap enumMap = null;
			Type type = null;
			XmlTypeMapElementInfoList xmlTypeMapElementInfoList = new XmlTypeMapElementInfoList();
			this.ImportTextElementInfo(xmlTypeMapElementInfoList, defaultType, member, atts, defaultNamespace);
			if (atts.XmlChoiceIdentifier != null)
			{
				if (cls == null)
				{
					throw new InvalidOperationException("XmlChoiceIdentifierAttribute not supported in this context.");
				}
				member.ChoiceMember = atts.XmlChoiceIdentifier.MemberName;
				MemberInfo[] member2 = cls.GetMember(member.ChoiceMember, BindingFlags.Instance | BindingFlags.Public);
				if (member2.Length == 0)
				{
					throw new InvalidOperationException(string.Concat(new object[]
					{
						"Choice member '",
						member.ChoiceMember,
						"' not found in class '",
						cls
					}));
				}
				if (member2[0] is PropertyInfo)
				{
					PropertyInfo propertyInfo = (PropertyInfo)member2[0];
					if (!propertyInfo.CanWrite || !propertyInfo.CanRead)
					{
						throw new InvalidOperationException("Choice property '" + member.ChoiceMember + "' must be read/write.");
					}
					type = propertyInfo.PropertyType;
				}
				else
				{
					type = ((FieldInfo)member2[0]).FieldType;
				}
				member.ChoiceTypeData = TypeTranslator.GetTypeData(type);
				if (type.IsArray)
				{
					type = type.GetElementType();
				}
				enumMap = (this.ImportTypeMapping(type).ObjectMap as EnumMap);
				if (enumMap == null)
				{
					throw new InvalidOperationException("The member '" + member2[0].Name + "' is not a valid target for XmlChoiceIdentifierAttribute.");
				}
			}
			if (atts.XmlElements.Count == 0 && xmlTypeMapElementInfoList.Count == 0)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo = new XmlTypeMapElementInfo(member, TypeTranslator.GetTypeData(defaultType));
				xmlTypeMapElementInfo.ElementName = defaultName;
				xmlTypeMapElementInfo.Namespace = defaultNamespace;
				if (xmlTypeMapElementInfo.TypeData.IsComplexType)
				{
					xmlTypeMapElementInfo.MappedType = this.ImportTypeMapping(defaultType, null, defaultNamespace);
				}
				xmlTypeMapElementInfoList.Add(xmlTypeMapElementInfo);
			}
			bool flag = atts.XmlElements.Count > 1;
			foreach (object obj in atts.XmlElements)
			{
				XmlElementAttribute xmlElementAttribute = (XmlElementAttribute)obj;
				Type type2 = (xmlElementAttribute.Type == null) ? defaultType : xmlElementAttribute.Type;
				XmlTypeMapElementInfo xmlTypeMapElementInfo2 = new XmlTypeMapElementInfo(member, TypeTranslator.GetTypeData(type2, xmlElementAttribute.DataType));
				xmlTypeMapElementInfo2.Form = xmlElementAttribute.Form;
				if (xmlTypeMapElementInfo2.Form != XmlSchemaForm.Unqualified)
				{
					xmlTypeMapElementInfo2.Namespace = ((xmlElementAttribute.Namespace == null) ? defaultNamespace : xmlElementAttribute.Namespace);
				}
				xmlTypeMapElementInfo2.IsNullable = xmlElementAttribute.IsNullable;
				if (xmlTypeMapElementInfo2.IsNullable && !xmlTypeMapElementInfo2.TypeData.IsNullable)
				{
					throw new InvalidOperationException(string.Concat(new string[]
					{
						"IsNullable may not be 'true' for value type ",
						xmlTypeMapElementInfo2.TypeData.FullTypeName,
						" in member '",
						defaultName,
						"'"
					}));
				}
				if (xmlTypeMapElementInfo2.TypeData.IsComplexType)
				{
					if (xmlElementAttribute.DataType.Length != 0)
					{
						throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "'{0}' is an invalid value for '{1}.{2}' of type '{3}'. The property may only be specified for primitive types.", new object[]
						{
							xmlElementAttribute.DataType,
							cls.FullName,
							defaultName,
							xmlTypeMapElementInfo2.TypeData.FullTypeName
						}));
					}
					xmlTypeMapElementInfo2.MappedType = this.ImportTypeMapping(type2, null, xmlTypeMapElementInfo2.Namespace);
				}
				if (xmlElementAttribute.ElementName.Length != 0)
				{
					xmlTypeMapElementInfo2.ElementName = XmlConvert.EncodeLocalName(xmlElementAttribute.ElementName);
				}
				else if (flag)
				{
					if (xmlTypeMapElementInfo2.MappedType != null)
					{
						xmlTypeMapElementInfo2.ElementName = xmlTypeMapElementInfo2.MappedType.ElementName;
					}
					else
					{
						xmlTypeMapElementInfo2.ElementName = TypeTranslator.GetTypeData(type2).XmlType;
					}
				}
				else
				{
					xmlTypeMapElementInfo2.ElementName = defaultName;
				}
				if (enumMap != null)
				{
					string enumName = enumMap.GetEnumName(type.FullName, xmlTypeMapElementInfo2.ElementName);
					if (enumName == null)
					{
						throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Type {0} is missing enumeration value '{1}' for element '{1} from namespace '{2}'.", new object[]
						{
							type,
							xmlTypeMapElementInfo2.ElementName,
							xmlTypeMapElementInfo2.Namespace
						}));
					}
					xmlTypeMapElementInfo2.ChoiceValue = Enum.Parse(type, enumName);
				}
				xmlTypeMapElementInfoList.Add(xmlTypeMapElementInfo2);
			}
			return xmlTypeMapElementInfoList;
		}

		private XmlTypeMapElementInfoList ImportAnyElementInfo(string defaultNamespace, XmlReflectionMember rmember, XmlTypeMapMemberElement member, XmlAttributes atts)
		{
			XmlTypeMapElementInfoList xmlTypeMapElementInfoList = new XmlTypeMapElementInfoList();
			this.ImportTextElementInfo(xmlTypeMapElementInfoList, rmember.MemberType, member, atts, defaultNamespace);
			foreach (object obj in atts.XmlAnyElements)
			{
				XmlAnyElementAttribute xmlAnyElementAttribute = (XmlAnyElementAttribute)obj;
				XmlTypeMapElementInfo xmlTypeMapElementInfo = new XmlTypeMapElementInfo(member, TypeTranslator.GetTypeData(typeof(XmlElement)));
				if (xmlAnyElementAttribute.Name.Length != 0)
				{
					xmlTypeMapElementInfo.ElementName = XmlConvert.EncodeLocalName(xmlAnyElementAttribute.Name);
					xmlTypeMapElementInfo.Namespace = ((xmlAnyElementAttribute.Namespace == null) ? string.Empty : xmlAnyElementAttribute.Namespace);
				}
				else
				{
					xmlTypeMapElementInfo.IsUnnamedAnyElement = true;
					xmlTypeMapElementInfo.Namespace = defaultNamespace;
					if (xmlAnyElementAttribute.Namespace != null)
					{
						throw new InvalidOperationException(string.Concat(new string[]
						{
							"The element ",
							rmember.MemberName,
							" has been attributed with an XmlAnyElementAttribute and a namespace '",
							xmlAnyElementAttribute.Namespace,
							"', but no name. When a namespace is supplied, a name is also required. Supply a name or remove the namespace."
						}));
					}
				}
				xmlTypeMapElementInfoList.Add(xmlTypeMapElementInfo);
			}
			return xmlTypeMapElementInfoList;
		}

		private void ImportTextElementInfo(XmlTypeMapElementInfoList list, Type defaultType, XmlTypeMapMemberElement member, XmlAttributes atts, string defaultNamespace)
		{
			if (atts.XmlText != null)
			{
				member.IsXmlTextCollector = true;
				if (atts.XmlText.Type != null)
				{
					TypeData typeData = TypeTranslator.GetTypeData(defaultType);
					if ((typeData.SchemaType == SchemaTypes.Primitive || typeData.SchemaType == SchemaTypes.Enum) && atts.XmlText.Type != defaultType)
					{
						throw new InvalidOperationException("The type for XmlText may not be specified for primitive types.");
					}
					defaultType = atts.XmlText.Type;
				}
				if (defaultType == typeof(XmlNode))
				{
					defaultType = typeof(XmlText);
				}
				XmlTypeMapElementInfo xmlTypeMapElementInfo = new XmlTypeMapElementInfo(member, TypeTranslator.GetTypeData(defaultType, atts.XmlText.DataType));
				if (xmlTypeMapElementInfo.TypeData.SchemaType != SchemaTypes.Primitive && xmlTypeMapElementInfo.TypeData.SchemaType != SchemaTypes.Enum && xmlTypeMapElementInfo.TypeData.SchemaType != SchemaTypes.XmlNode && (xmlTypeMapElementInfo.TypeData.SchemaType != SchemaTypes.Array || xmlTypeMapElementInfo.TypeData.ListItemTypeData.SchemaType != SchemaTypes.XmlNode))
				{
					throw new InvalidOperationException("XmlText cannot be used to encode complex types");
				}
				if (xmlTypeMapElementInfo.TypeData.IsComplexType)
				{
					xmlTypeMapElementInfo.MappedType = this.ImportTypeMapping(defaultType, null, defaultNamespace);
				}
				xmlTypeMapElementInfo.IsTextElement = true;
				xmlTypeMapElementInfo.WrappedElement = false;
				list.Add(xmlTypeMapElementInfo);
			}
		}

		private bool CanBeNull(TypeData type)
		{
			return !type.Type.IsValueType || type.IsNullable;
		}

		/// <summary>Includes mappings for a type for later use when import methods are invoked. </summary>
		/// <param name="type">The .NET Framework type for which to save type mapping information.</param>
		public void IncludeType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this.includedTypes == null)
			{
				this.includedTypes = new ArrayList();
			}
			if (!this.includedTypes.Contains(type))
			{
				this.includedTypes.Add(type);
			}
			if (this.relatedMaps.Count > 0)
			{
				foreach (object obj in ((ArrayList)this.relatedMaps.Clone()))
				{
					XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)obj;
					if (xmlTypeMapping.TypeData.Type == typeof(object))
					{
						xmlTypeMapping.DerivedTypes.Add(this.ImportTypeMapping(type));
					}
				}
			}
		}

		/// <summary>Includes mappings for derived types for later use when import methods are invoked. </summary>
		/// <param name="provider">An instance of the <see cref="T:System.Reflection.ICustomAttributeProvider" />  class that contains custom attributes derived from the <see cref="T:System.Xml.Serialization.XmlIncludeAttribute" /> attribute.</param>
		public void IncludeTypes(ICustomAttributeProvider provider)
		{
			object[] customAttributes = provider.GetCustomAttributes(typeof(XmlIncludeAttribute), true);
			foreach (XmlIncludeAttribute xmlIncludeAttribute in customAttributes)
			{
				this.IncludeType(xmlIncludeAttribute.Type);
			}
		}

		private object GetDefaultValue(TypeData typeData, object defaultValue)
		{
			if (defaultValue == DBNull.Value || typeData.SchemaType != SchemaTypes.Enum)
			{
				return defaultValue;
			}
			string a = Enum.Format(typeData.Type, defaultValue, "g");
			string b = Enum.Format(typeData.Type, defaultValue, "d");
			if (a == b)
			{
				string message = string.Format(CultureInfo.InvariantCulture, "Value '{0}' cannot be converted to {1}.", new object[]
				{
					defaultValue,
					defaultValue.GetType().FullName
				});
				throw new InvalidOperationException(message);
			}
			return defaultValue;
		}
	}
}
