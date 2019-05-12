using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace System.Xml.Serialization
{
	/// <summary>Generates mappings to SOAP-encoded messages from .NET Framework types or Web service method information. </summary>
	public class SoapReflectionImporter
	{
		private SoapAttributeOverrides attributeOverrides;

		private string initialDefaultNamespace;

		private ArrayList includedTypes;

		private ArrayList relatedMaps = new ArrayList();

		private ReflectionHelper helper = new ReflectionHelper();

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.SoapReflectionImporter" /> class. </summary>
		public SoapReflectionImporter() : this(null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.SoapReflectionImporter" /> class, specifying overrides for XML serialization. </summary>
		/// <param name="attributeOverrides">A <see cref="T:System.Xml.Serialization.SoapAttributeOverrides" /> object that overrides how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class serializes mapped types using SOAP encoding.</param>
		public SoapReflectionImporter(SoapAttributeOverrides attributeOverrides) : this(attributeOverrides, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.SoapReflectionImporter" /> class, specifying a default XML namespace for imported type mappings. </summary>
		/// <param name="defaultNamespace">The default XML namespace to use for imported type mappings.</param>
		public SoapReflectionImporter(string defaultNamespace) : this(null, defaultNamespace)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.SoapReflectionImporter" /> class, specifying XML serialization overrides and a default XML namespace. </summary>
		/// <param name="attributeOverrides">A <see cref="T:System.Xml.Serialization.SoapAttributeOverrides" /> object that overrides how the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class serializes mapped types using SOAP encoding.</param>
		/// <param name="defaultNamespace">The default XML namespace to use for imported type mappings.</param>
		public SoapReflectionImporter(SoapAttributeOverrides attributeOverrides, string defaultNamespace)
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
				this.attributeOverrides = new SoapAttributeOverrides();
			}
			else
			{
				this.attributeOverrides = attributeOverrides;
			}
		}

		/// <summary>Generates internal type mappings for information that is gathered from a Web service method. </summary>
		/// <returns>Internal .NET Framework type mappings to the element parts of a WSDL message definition.</returns>
		/// <param name="elementName">An XML element name produced from the Web service method.</param>
		/// <param name="ns">An XML element namespace produced from the Web service method.</param>
		/// <param name="members">An array of .NET Framework code entities that belong to a Web service method.</param>
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members)
		{
			return this.ImportMembersMapping(elementName, ns, members, true, true, false);
		}

		/// <summary>Generates internal type mappings for information that is gathered from a Web service method. </summary>
		/// <returns>Internal .NET Framework type mappings to the element parts of a WSDL message definition.</returns>
		/// <param name="elementName">An XML element name produced from the Web service method.</param>
		/// <param name="ns">An XML element namespace produced from the Web service method.</param>
		/// <param name="members">An array of .NET Framework code entities that belong to a Web service method.</param>
		/// <param name="hasWrapperElement">true to indicate that elements that correspond to WSDL message parts should be enclosed in an extra wrapper element in a SOAP message; otherwise, false.</param>
		/// <param name="writeAccessors">true to indicate an RPC-style Web service binding; false to indicate a document-style Web service binding or a SOAP header.</param>
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members, bool hasWrapperElement, bool writeAccessors)
		{
			return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, writeAccessors, false);
		}

		/// <summary>Generates internal type mappings for information that is gathered from a Web service method. </summary>
		/// <returns>Internal .NET Framework type mappings to the element parts of a WSDL message definition.</returns>
		/// <param name="elementName">An XML element name produced from the Web service method.</param>
		/// <param name="ns">An XML element namespace produced from the Web service method.</param>
		/// <param name="members">An array of .NET Framework code entities that belong to a Web service method.</param>
		/// <param name="hasWrapperElement">true to indicate that elements that correspond to WSDL message parts should be enclosed in an extra wrapper element in a SOAP message; otherwise, false.</param>
		/// <param name="writeAccessors">true to indicate an RPC-style Web service binding; false to indicate a document-style Web service binding or a SOAP header.</param>
		/// <param name="validate">true to indicate that a generated deserializer should check for the expected qualified name of the wrapper element; otherwise, false. This parameter's value is relevant only if the <paramref name="hasWrapperElement" /> parameter's value is true.</param>
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members, bool hasWrapperElement, bool writeAccessors, bool validate)
		{
			return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, writeAccessors, validate, XmlMappingAccess.Read | XmlMappingAccess.Write);
		}

		/// <summary>Generates internal type mappings for information that is gathered from a Web service method.</summary>
		/// <returns>Internal .NET Framework type mappings to the element parts of a WSDL message definition.</returns>
		/// <param name="elementName">An XML element name produced from the Web service method.</param>
		/// <param name="ns">An XML element namespace produced from the Web service method.</param>
		/// <param name="members">An array of .NET Framework code entities that belong to a Web service method.</param>
		/// <param name="hasWrapperElement">true to indicate that elements that correspond to WSDL message parts should be enclosed in an extra wrapper element in a SOAP message; otherwise, false.</param>
		/// <param name="writeAccessors">true to indicate an RPC-style Web service binding; false to indicate a document-style Web service binding or a SOAP header.</param>
		/// <param name="validate">true to indicate that a generated deserializer should check for the expected qualified name of the wrapper element; otherwise, false. This parameter's value is relevant only if the <paramref name="hasWrapperElement" /> parameter's value is true.</param>
		/// <param name="access">One of the <see cref="T:System.Xml.Serialization.XmlMappingAccess" /> values.</param>
		[MonoTODO]
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members, bool hasWrapperElement, bool writeAccessors, bool validate, XmlMappingAccess access)
		{
			elementName = XmlConvert.EncodeLocalName(elementName);
			XmlMemberMapping[] array = new XmlMemberMapping[members.Length];
			for (int i = 0; i < members.Length; i++)
			{
				XmlTypeMapMember mapMem = this.CreateMapMember(members[i], ns);
				array[i] = new XmlMemberMapping(XmlConvert.EncodeLocalName(members[i].MemberName), ns, mapMem, true);
			}
			XmlMembersMapping xmlMembersMapping = new XmlMembersMapping(elementName, ns, hasWrapperElement, writeAccessors, array);
			xmlMembersMapping.RelatedMaps = this.relatedMaps;
			xmlMembersMapping.Format = SerializationFormat.Encoded;
			Type[] array2 = (this.includedTypes == null) ? null : ((Type[])this.includedTypes.ToArray(typeof(Type)));
			xmlMembersMapping.Source = new MembersSerializationSource(elementName, hasWrapperElement, members, writeAccessors, false, null, array2);
			return xmlMembersMapping;
		}

		/// <summary>Generates a mapping to an XML Schema element for a .NET Framework type.</summary>
		/// <returns>Internal .NET Framework mapping of a type to an XML Schema element. </returns>
		/// <param name="type">The .NET Framework type for which to generate a type mapping. </param>
		public XmlTypeMapping ImportTypeMapping(Type type)
		{
			return this.ImportTypeMapping(type, null);
		}

		/// <summary>Generates a mapping to an XML Schema element for a .NET Framework type.</summary>
		/// <returns>Internal .NET Framework mapping of a type to an XML Schema element.</returns>
		/// <param name="type">The .NET Framework type for which to generate a type mapping. </param>
		/// <param name="defaultNamespace">The default XML namespace to use.</param>
		public XmlTypeMapping ImportTypeMapping(Type type, string defaultNamespace)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type == typeof(void))
			{
				throw new InvalidOperationException("Type " + type.Name + " may not be serialized.");
			}
			return this.ImportTypeMapping(TypeTranslator.GetTypeData(type), defaultNamespace);
		}

		internal XmlTypeMapping ImportTypeMapping(TypeData typeData, string defaultNamespace)
		{
			if (typeData == null)
			{
				throw new ArgumentNullException("typeData");
			}
			if (typeData.Type == null)
			{
				throw new ArgumentException("Specified TypeData instance does not have Type set.");
			}
			string text = this.initialDefaultNamespace;
			if (defaultNamespace == null)
			{
				defaultNamespace = this.initialDefaultNamespace;
			}
			if (defaultNamespace == null)
			{
				defaultNamespace = string.Empty;
			}
			this.initialDefaultNamespace = defaultNamespace;
			XmlTypeMapping xmlTypeMapping;
			switch (typeData.SchemaType)
			{
			case SchemaTypes.Primitive:
				xmlTypeMapping = this.ImportPrimitiveMapping(typeData, defaultNamespace);
				goto IL_E1;
			case SchemaTypes.Enum:
				xmlTypeMapping = this.ImportEnumMapping(typeData, defaultNamespace);
				goto IL_E1;
			case SchemaTypes.Array:
				xmlTypeMapping = this.ImportListMapping(typeData, defaultNamespace);
				goto IL_E1;
			case SchemaTypes.Class:
				xmlTypeMapping = this.ImportClassMapping(typeData, defaultNamespace);
				goto IL_E1;
			case SchemaTypes.XmlNode:
				throw this.CreateTypeException(typeData.Type);
			}
			throw new NotSupportedException("Type " + typeData.Type.FullName + " not supported for XML serialization");
			IL_E1:
			xmlTypeMapping.RelatedMaps = this.relatedMaps;
			xmlTypeMapping.Format = SerializationFormat.Encoded;
			Type[] array = (this.includedTypes == null) ? null : ((Type[])this.includedTypes.ToArray(typeof(Type)));
			xmlTypeMapping.Source = new SoapTypeSerializationSource(typeData.Type, this.attributeOverrides, defaultNamespace, array);
			this.initialDefaultNamespace = text;
			return xmlTypeMapping;
		}

		private XmlTypeMapping CreateTypeMapping(TypeData typeData, string defaultXmlType, string defaultNamespace)
		{
			string text = defaultNamespace;
			bool includeInSchema = true;
			SoapAttributes soapAttributes = null;
			if (defaultXmlType == null)
			{
				defaultXmlType = typeData.XmlType;
			}
			if (!typeData.IsListType)
			{
				if (this.attributeOverrides != null)
				{
					soapAttributes = this.attributeOverrides[typeData.Type];
				}
				if (soapAttributes != null && typeData.SchemaType == SchemaTypes.Primitive)
				{
					throw new InvalidOperationException("SoapType attribute may not be specified for the type " + typeData.FullTypeName);
				}
			}
			if (soapAttributes == null)
			{
				soapAttributes = new SoapAttributes(typeData.Type);
			}
			if (soapAttributes.SoapType != null)
			{
				if (soapAttributes.SoapType.Namespace != null && soapAttributes.SoapType.Namespace != string.Empty)
				{
					text = soapAttributes.SoapType.Namespace;
				}
				if (soapAttributes.SoapType.TypeName != null && soapAttributes.SoapType.TypeName != string.Empty)
				{
					defaultXmlType = XmlConvert.EncodeLocalName(soapAttributes.SoapType.TypeName);
				}
				includeInSchema = soapAttributes.SoapType.IncludeInSchema;
			}
			if (text == null)
			{
				text = string.Empty;
			}
			XmlTypeMapping xmlTypeMapping = new XmlTypeMapping(defaultXmlType, text, typeData, defaultXmlType, text);
			xmlTypeMapping.IncludeInSchema = includeInSchema;
			this.relatedMaps.Add(xmlTypeMapping);
			return xmlTypeMapping;
		}

		private XmlTypeMapping ImportClassMapping(Type type, string defaultNamespace)
		{
			TypeData typeData = TypeTranslator.GetTypeData(type);
			return this.ImportClassMapping(typeData, defaultNamespace);
		}

		private XmlTypeMapping ImportClassMapping(TypeData typeData, string defaultNamespace)
		{
			Type type = typeData.Type;
			if (type.IsValueType)
			{
				throw this.CreateStructException(type);
			}
			if (type == typeof(object))
			{
				defaultNamespace = "http://www.w3.org/2001/XMLSchema";
			}
			ReflectionHelper.CheckSerializableType(type, false);
			XmlTypeMapping xmlTypeMapping = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, defaultNamespace));
			if (xmlTypeMapping != null)
			{
				return xmlTypeMapping;
			}
			xmlTypeMapping = this.CreateTypeMapping(typeData, null, defaultNamespace);
			this.helper.RegisterClrType(xmlTypeMapping, type, xmlTypeMapping.Namespace);
			xmlTypeMapping.MultiReferenceType = true;
			ClassMap classMap = new ClassMap();
			xmlTypeMapping.ObjectMap = classMap;
			ICollection reflectionMembers = this.GetReflectionMembers(type);
			foreach (object obj in reflectionMembers)
			{
				XmlReflectionMember xmlReflectionMember = (XmlReflectionMember)obj;
				if (!xmlReflectionMember.SoapAttributes.SoapIgnore)
				{
					classMap.AddMember(this.CreateMapMember(xmlReflectionMember, defaultNamespace));
				}
			}
			SoapIncludeAttribute[] array = (SoapIncludeAttribute[])type.GetCustomAttributes(typeof(SoapIncludeAttribute), false);
			for (int i = 0; i < array.Length; i++)
			{
				Type type2 = array[i].Type;
				this.ImportTypeMapping(type2);
			}
			if (type == typeof(object) && this.includedTypes != null)
			{
				foreach (object obj2 in this.includedTypes)
				{
					Type type3 = (Type)obj2;
					xmlTypeMapping.DerivedTypes.Add(this.ImportTypeMapping(type3));
				}
			}
			if (type.BaseType != null)
			{
				XmlTypeMapping xmlTypeMapping2 = this.ImportClassMapping(type.BaseType, defaultNamespace);
				if (type.BaseType != typeof(object))
				{
					xmlTypeMapping.BaseMap = xmlTypeMapping2;
				}
				this.RegisterDerivedMap(xmlTypeMapping2, xmlTypeMapping);
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

		private string GetTypeNamespace(TypeData typeData, string defaultNamespace)
		{
			string text = defaultNamespace;
			SoapAttributes soapAttributes = null;
			if (!typeData.IsListType && this.attributeOverrides != null)
			{
				soapAttributes = this.attributeOverrides[typeData.Type];
			}
			if (soapAttributes == null)
			{
				soapAttributes = new SoapAttributes(typeData.Type);
			}
			if (soapAttributes.SoapType != null && soapAttributes.SoapType.Namespace != null && soapAttributes.SoapType.Namespace != string.Empty)
			{
				text = soapAttributes.SoapType.Namespace;
			}
			if (text == null)
			{
				return string.Empty;
			}
			return text;
		}

		private XmlTypeMapping ImportListMapping(TypeData typeData, string defaultNamespace)
		{
			Type type = typeData.Type;
			XmlTypeMapping xmlTypeMapping = this.helper.GetRegisteredClrType(type, "http://schemas.xmlsoap.org/soap/encoding/");
			if (xmlTypeMapping != null)
			{
				return xmlTypeMapping;
			}
			ListMap listMap = new ListMap();
			TypeData listItemTypeData = typeData.ListItemTypeData;
			xmlTypeMapping = this.CreateTypeMapping(typeData, "Array", "http://schemas.xmlsoap.org/soap/encoding/");
			this.helper.RegisterClrType(xmlTypeMapping, type, "http://schemas.xmlsoap.org/soap/encoding/");
			xmlTypeMapping.MultiReferenceType = true;
			xmlTypeMapping.ObjectMap = listMap;
			XmlTypeMapElementInfo xmlTypeMapElementInfo = new XmlTypeMapElementInfo(null, listItemTypeData);
			if (xmlTypeMapElementInfo.TypeData.IsComplexType)
			{
				xmlTypeMapElementInfo.MappedType = this.ImportTypeMapping(typeData.ListItemType, defaultNamespace);
				xmlTypeMapElementInfo.TypeData = xmlTypeMapElementInfo.MappedType.TypeData;
			}
			xmlTypeMapElementInfo.ElementName = "Item";
			xmlTypeMapElementInfo.Namespace = string.Empty;
			xmlTypeMapElementInfo.IsNullable = true;
			listMap.ItemInfo = new XmlTypeMapElementInfoList
			{
				xmlTypeMapElementInfo
			};
			XmlTypeMapping xmlTypeMapping2 = this.ImportTypeMapping(typeof(object), defaultNamespace);
			xmlTypeMapping2.DerivedTypes.Add(xmlTypeMapping);
			SoapIncludeAttribute[] array = (SoapIncludeAttribute[])type.GetCustomAttributes(typeof(SoapIncludeAttribute), false);
			for (int i = 0; i < array.Length; i++)
			{
				Type type2 = array[i].Type;
				xmlTypeMapping2.DerivedTypes.Add(this.ImportTypeMapping(type2, defaultNamespace));
			}
			return xmlTypeMapping;
		}

		private XmlTypeMapping ImportPrimitiveMapping(TypeData typeData, string defaultNamespace)
		{
			if (typeData.SchemaType == SchemaTypes.Primitive)
			{
				defaultNamespace = ((!typeData.IsXsdType) ? "http://microsoft.com/wsdl/types/" : "http://www.w3.org/2001/XMLSchema");
			}
			Type type = typeData.Type;
			XmlTypeMapping xmlTypeMapping = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, defaultNamespace));
			if (xmlTypeMapping != null)
			{
				return xmlTypeMapping;
			}
			xmlTypeMapping = this.CreateTypeMapping(typeData, null, defaultNamespace);
			this.helper.RegisterClrType(xmlTypeMapping, type, xmlTypeMapping.Namespace);
			return xmlTypeMapping;
		}

		private XmlTypeMapping ImportEnumMapping(TypeData typeData, string defaultNamespace)
		{
			Type type = typeData.Type;
			XmlTypeMapping xmlTypeMapping = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, defaultNamespace));
			if (xmlTypeMapping != null)
			{
				return xmlTypeMapping;
			}
			ReflectionHelper.CheckSerializableType(type, false);
			xmlTypeMapping = this.CreateTypeMapping(typeData, null, defaultNamespace);
			this.helper.RegisterClrType(xmlTypeMapping, type, xmlTypeMapping.Namespace);
			xmlTypeMapping.MultiReferenceType = true;
			string[] names = Enum.GetNames(type);
			EnumMap.EnumMapMember[] array = new EnumMap.EnumMapMember[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				FieldInfo field = type.GetField(names[i]);
				string name = names[i];
				object[] customAttributes = field.GetCustomAttributes(typeof(SoapEnumAttribute), false);
				if (customAttributes.Length > 0)
				{
					name = ((SoapEnumAttribute)customAttributes[0]).Name;
				}
				long value = ((IConvertible)field.GetValue(null)).ToInt64(CultureInfo.InvariantCulture);
				array[i] = new EnumMap.EnumMapMember(XmlConvert.EncodeLocalName(name), names[i], value);
			}
			bool isFlags = type.IsDefined(typeof(FlagsAttribute), false);
			xmlTypeMapping.ObjectMap = new EnumMap(array, isFlags);
			this.ImportTypeMapping(typeof(object), defaultNamespace).DerivedTypes.Add(xmlTypeMapping);
			return xmlTypeMapping;
		}

		private ICollection GetReflectionMembers(Type type)
		{
			ArrayList arrayList = new ArrayList();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.CanRead)
				{
					if (propertyInfo.CanWrite || (TypeTranslator.GetTypeData(propertyInfo.PropertyType).SchemaType == SchemaTypes.Array && !propertyInfo.PropertyType.IsArray))
					{
						SoapAttributes soapAttributes = this.attributeOverrides[type, propertyInfo.Name];
						if (soapAttributes == null)
						{
							soapAttributes = new SoapAttributes(propertyInfo);
						}
						if (!soapAttributes.SoapIgnore)
						{
							XmlReflectionMember value = new XmlReflectionMember(propertyInfo.Name, propertyInfo.PropertyType, soapAttributes);
							arrayList.Add(value);
						}
					}
				}
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				SoapAttributes soapAttributes2 = this.attributeOverrides[type, fieldInfo.Name];
				if (soapAttributes2 == null)
				{
					soapAttributes2 = new SoapAttributes(fieldInfo);
				}
				if (!soapAttributes2.SoapIgnore)
				{
					XmlReflectionMember value2 = new XmlReflectionMember(fieldInfo.Name, fieldInfo.FieldType, soapAttributes2);
					arrayList.Add(value2);
				}
			}
			return arrayList;
		}

		private XmlTypeMapMember CreateMapMember(XmlReflectionMember rmember, string defaultNamespace)
		{
			SoapAttributes soapAttributes = rmember.SoapAttributes;
			TypeData typeData = TypeTranslator.GetTypeData(rmember.MemberType);
			XmlTypeMapMember xmlTypeMapMember;
			if (soapAttributes.SoapAttribute != null)
			{
				if (typeData.SchemaType != SchemaTypes.Enum && typeData.SchemaType != SchemaTypes.Primitive)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot serialize member '{0}' of type {1}. SoapAttribute cannot be used to encode complex types.", new object[]
					{
						rmember.MemberName,
						typeData.FullTypeName
					}));
				}
				if (soapAttributes.SoapElement != null)
				{
					throw new Exception("SoapAttributeAttribute and SoapElementAttribute cannot be applied to the same member");
				}
				XmlTypeMapMemberAttribute xmlTypeMapMemberAttribute = new XmlTypeMapMemberAttribute();
				if (soapAttributes.SoapAttribute.AttributeName.Length == 0)
				{
					xmlTypeMapMemberAttribute.AttributeName = XmlConvert.EncodeLocalName(rmember.MemberName);
				}
				else
				{
					xmlTypeMapMemberAttribute.AttributeName = XmlConvert.EncodeLocalName(soapAttributes.SoapAttribute.AttributeName);
				}
				xmlTypeMapMemberAttribute.Namespace = ((soapAttributes.SoapAttribute.Namespace == null) ? string.Empty : soapAttributes.SoapAttribute.Namespace);
				if (typeData.IsComplexType)
				{
					xmlTypeMapMemberAttribute.MappedType = this.ImportTypeMapping(typeData.Type, defaultNamespace);
				}
				typeData = TypeTranslator.GetTypeData(rmember.MemberType, soapAttributes.SoapAttribute.DataType);
				xmlTypeMapMember = xmlTypeMapMemberAttribute;
				xmlTypeMapMember.DefaultValue = this.GetDefaultValue(typeData, soapAttributes.SoapDefaultValue);
			}
			else
			{
				if (typeData.SchemaType == SchemaTypes.Array)
				{
					xmlTypeMapMember = new XmlTypeMapMemberList();
				}
				else
				{
					xmlTypeMapMember = new XmlTypeMapMemberElement();
				}
				if (soapAttributes.SoapElement != null && soapAttributes.SoapElement.DataType.Length != 0)
				{
					typeData = TypeTranslator.GetTypeData(rmember.MemberType, soapAttributes.SoapElement.DataType);
				}
				XmlTypeMapElementInfoList xmlTypeMapElementInfoList = new XmlTypeMapElementInfoList();
				XmlTypeMapElementInfo xmlTypeMapElementInfo = new XmlTypeMapElementInfo(xmlTypeMapMember, typeData);
				xmlTypeMapElementInfo.ElementName = XmlConvert.EncodeLocalName((soapAttributes.SoapElement == null || soapAttributes.SoapElement.ElementName.Length == 0) ? rmember.MemberName : soapAttributes.SoapElement.ElementName);
				xmlTypeMapElementInfo.Namespace = string.Empty;
				xmlTypeMapElementInfo.IsNullable = (soapAttributes.SoapElement != null && soapAttributes.SoapElement.IsNullable);
				if (typeData.IsComplexType)
				{
					xmlTypeMapElementInfo.MappedType = this.ImportTypeMapping(typeData.Type, defaultNamespace);
				}
				xmlTypeMapElementInfoList.Add(xmlTypeMapElementInfo);
				((XmlTypeMapMemberElement)xmlTypeMapMember).ElementInfo = xmlTypeMapElementInfoList;
			}
			xmlTypeMapMember.TypeData = typeData;
			xmlTypeMapMember.Name = rmember.MemberName;
			xmlTypeMapMember.IsReturnValue = rmember.IsReturnValue;
			return xmlTypeMapMember;
		}

		/// <summary>Places mappings for a type in the <see cref="T:System.Xml.Serialization.SoapReflectionImporter" /> instance's context for later use when import methods are invoked. </summary>
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
		}

		/// <summary>Places mappings for derived types in the <see cref="T:System.Xml.Serialization.SoapReflectionImporter" /> instance's context for later use when import methods are invoked. </summary>
		/// <param name="provider">An <see cref="T:System.Reflection.ICustomAttributeProvider" /> reflection object that contains custom attributes that are derived from the <see cref="T:System.Xml.Serialization.SoapIncludeAttribute" /> attribute.</param>
		public void IncludeTypes(ICustomAttributeProvider provider)
		{
			object[] customAttributes = provider.GetCustomAttributes(typeof(SoapIncludeAttribute), true);
			foreach (SoapIncludeAttribute soapIncludeAttribute in customAttributes)
			{
				this.IncludeType(soapIncludeAttribute.Type);
			}
		}

		private Exception CreateTypeException(Type type)
		{
			return new NotSupportedException("The type " + type.FullName + " may not be serialized with SOAP-encoded messages. Set the Use for your message to Literal");
		}

		private Exception CreateStructException(Type type)
		{
			return new NotSupportedException("Cannot serialize " + type.FullName + ". Nested structs are not supported with encoded SOAP");
		}

		private object GetDefaultValue(TypeData typeData, object defaultValue)
		{
			if (defaultValue == DBNull.Value || typeData.SchemaType != SchemaTypes.Enum)
			{
				return defaultValue;
			}
			if (typeData.Type != defaultValue.GetType())
			{
				string message = string.Format(CultureInfo.InvariantCulture, "Enum {0} cannot be converted to {1}.", new object[]
				{
					defaultValue.GetType().FullName,
					typeData.FullTypeName
				});
				throw new InvalidOperationException(message);
			}
			string a = Enum.Format(typeData.Type, defaultValue, "g");
			string b = Enum.Format(typeData.Type, defaultValue, "d");
			if (a == b)
			{
				string message2 = string.Format(CultureInfo.InvariantCulture, "Value '{0}' cannot be converted to {1}.", new object[]
				{
					defaultValue,
					defaultValue.GetType().FullName
				});
				throw new InvalidOperationException(message2);
			}
			return defaultValue;
		}
	}
}
