using System;
using System.Collections;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	/// <summary>Generates internal mappings to .NET Framework types for XML schema element declarations, including literal XSD message parts in a WSDL document. </summary>
	public class XmlSchemaImporter
	{
		private const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";

		private XmlSchemas schemas;

		private CodeIdentifiers typeIdentifiers;

		private CodeIdentifiers elemIdentifiers = new CodeIdentifiers();

		private Hashtable mappedTypes = new Hashtable();

		private Hashtable primitiveDerivedMappedTypes = new Hashtable();

		private Hashtable dataMappedTypes = new Hashtable();

		private Queue pendingMaps = new Queue();

		private Hashtable sharedAnonymousTypes = new Hashtable();

		private bool encodedFormat;

		private XmlReflectionImporter auxXmlRefImporter;

		private SoapReflectionImporter auxSoapRefImporter;

		private bool anyTypeImported;

		private CodeGenerationOptions options;

		private static readonly XmlQualifiedName anyType = new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema");

		private static readonly XmlQualifiedName arrayType = new XmlQualifiedName("Array", "http://schemas.xmlsoap.org/soap/encoding/");

		private static readonly XmlQualifiedName arrayTypeRefName = new XmlQualifiedName("arrayType", "http://schemas.xmlsoap.org/soap/encoding/");

		private XmlSchemaElement anyElement;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSchemaImporter" /> class, taking a collection of <see cref="T:System.Xml.Schema.XmlSchema" /> objects representing the XML schemas used by SOAP literal messages defined in a WSDL document. </summary>
		/// <param name="schemas">A collection of <see cref="T:System.Xml.Schema.XmlSchema" /> objects.</param>
		public XmlSchemaImporter(XmlSchemas schemas)
		{
			this.schemas = schemas;
			this.typeIdentifiers = new CodeIdentifiers();
			this.InitializeExtensions();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSchemaImporter" /> class, taking a collection of <see cref="T:System.Xml.Schema.XmlSchema" /> objects that represents the XML schemas used by SOAP literal messages, plus classes being generated for bindings defined in a Web Services Description Language (WSDL) document. </summary>
		/// <param name="schemas">An <see cref="T:System.Xml.Serialization.XmlSchemas" /> object.</param>
		/// <param name="typeIdentifiers">A <see cref="T:System.Xml.Serialization.CodeIdentifiers" /> object that specifies a collection of classes being generated for bindings defined in a WSDL document.</param>
		public XmlSchemaImporter(XmlSchemas schemas, CodeIdentifiers typeIdentifiers) : this(schemas)
		{
			this.typeIdentifiers = typeIdentifiers;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSchemaImporter" /> class for a collection of XML schemas, using the specified code generation options and import context.</summary>
		/// <param name="schemas">A collection of <see cref="T:System.Xml.Schema.XmlSchema" /> objects.</param>
		/// <param name="options">A <see cref="T:System.Xml.Serialization.CodeGenerationOptions" /> enumeration that specifies code generation options.</param>
		/// <param name="context">A <see cref="T:System.Xml.Serialization.ImportContext" /> instance that specifies the import context.</param>
		public XmlSchemaImporter(XmlSchemas schemas, CodeGenerationOptions options, ImportContext context)
		{
			this.schemas = schemas;
			this.options = options;
			if (context != null)
			{
				this.typeIdentifiers = context.TypeIdentifiers;
				this.InitSharedData(context);
			}
			else
			{
				this.typeIdentifiers = new CodeIdentifiers();
			}
			this.InitializeExtensions();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSchemaImporter" /> class, taking a collection of <see cref="T:System.Xml.Schema.XmlSchema" /> objects that represents the XML schemas used by SOAP literal messages, plus classes being generated for bindings defined in a WSDL document, and a <see cref="T:System.Xml.Serialization.CodeGenerationOptions" /> enumeration value.</summary>
		/// <param name="schemas">A collection of <see cref="T:System.Xml.Schema.XmlSchema" /> objects.</param>
		/// <param name="typeIdentifiers">A <see cref="T:System.Xml.Serialization.CodeIdentifiers" /> object that specifies a collection of classes being generated for bindings defined in a WSDL document.</param>
		/// <param name="options">A bitwise combination of the <see cref="T:System.Xml.Serialization.CodeGenerationOptions" /> values that specifies the options to use when generating .NET Framework types for a Web service.</param>
		public XmlSchemaImporter(XmlSchemas schemas, CodeIdentifiers typeIdentifiers, CodeGenerationOptions options)
		{
			this.typeIdentifiers = typeIdentifiers;
			this.schemas = schemas;
			this.options = options;
			this.InitializeExtensions();
		}

		private void InitSharedData(ImportContext context)
		{
			if (context.ShareTypes)
			{
				this.mappedTypes = context.MappedTypes;
				this.dataMappedTypes = context.DataMappedTypes;
				this.sharedAnonymousTypes = context.SharedAnonymousTypes;
			}
		}

		internal bool UseEncodedFormat
		{
			get
			{
				return this.encodedFormat;
			}
			set
			{
				this.encodedFormat = value;
			}
		}

		private void InitializeExtensions()
		{
		}

		/// <summary>Generates internal type mapping information for a single, (SOAP) literal element part defined in a WSDL document.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlMembersMapping" /> representing the .NET Framework type mapping for a single element part of a WSDL message definition.</returns>
		/// <param name="typeName">An <see cref="T:System.Xml.XmlQualifiedName" /> that specifies the name of an element's type for which a .NET Framework type is generated.</param>
		/// <param name="elementName">The name of the part element in the WSDL document.</param>
		public XmlMembersMapping ImportAnyType(XmlQualifiedName typeName, string elementName)
		{
			if (typeName == XmlQualifiedName.Empty)
			{
				XmlTypeMapMemberAnyElement xmlTypeMapMemberAnyElement = new XmlTypeMapMemberAnyElement();
				xmlTypeMapMemberAnyElement.Name = typeName.Name;
				xmlTypeMapMemberAnyElement.TypeData = TypeTranslator.GetTypeData(typeof(XmlNode));
				xmlTypeMapMemberAnyElement.ElementInfo.Add(this.CreateElementInfo(typeName.Namespace, xmlTypeMapMemberAnyElement, typeName.Name, xmlTypeMapMemberAnyElement.TypeData, true, XmlSchemaForm.None));
				return new XmlMembersMapping(new XmlMemberMapping[]
				{
					new XmlMemberMapping(typeName.Name, typeName.Namespace, xmlTypeMapMemberAnyElement, this.encodedFormat)
				});
			}
			XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)this.schemas.Find(typeName, typeof(XmlSchemaComplexType));
			if (xmlSchemaComplexType == null)
			{
				throw new InvalidOperationException("Referenced type '" + typeName + "' not found");
			}
			if (!this.CanBeAnyElement(xmlSchemaComplexType))
			{
				throw new InvalidOperationException("The type '" + typeName + "' is not valid for a collection of any elements");
			}
			ClassMap classMap = new ClassMap();
			CodeIdentifiers classIds = new CodeIdentifiers();
			bool isMixed = xmlSchemaComplexType.IsMixed;
			this.ImportSequenceContent(typeName, classMap, ((XmlSchemaSequence)xmlSchemaComplexType.Particle).Items, classIds, false, ref isMixed);
			XmlTypeMapMemberAnyElement xmlTypeMapMemberAnyElement2 = (XmlTypeMapMemberAnyElement)classMap.AllMembers[0];
			xmlTypeMapMemberAnyElement2.Name = typeName.Name;
			return new XmlMembersMapping(new XmlMemberMapping[]
			{
				new XmlMemberMapping(typeName.Name, typeName.Namespace, xmlTypeMapMemberAnyElement2, this.encodedFormat)
			});
		}

		/// <summary>Generates internal type mapping information for an element defined in an XML schema document. </summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlTypeMapping" /> representing the.NET Framework type mapping information for an XML schema element.</returns>
		/// <param name="name">An <see cref="T:System.Xml.XmlQualifiedName" /> that specifies the name of an element defined in an XML schema document.</param>
		/// <param name="baseType">A base type for the .NET Framework type that is generated to correspond to an XSD element's type.</param>
		public XmlTypeMapping ImportDerivedTypeMapping(XmlQualifiedName name, Type baseType)
		{
			return this.ImportDerivedTypeMapping(name, baseType, true);
		}

		/// <summary>Generates internal type mapping information for an element defined in an XML schema document or as a part in a WSDL document.</summary>
		/// <returns>The .NET Framework type mapping information for an XML schema element.</returns>
		/// <param name="name">An <see cref="T:System.Xml.XmlQualifiedName" /> that specifies the name of an element defined in an XML schema document.</param>
		/// <param name="baseType">A base type for the .NET Framework type that is generated to correspond to an XSD element's type.</param>
		/// <param name="baseTypeCanBeIndirect">true to indicate that the type corresponding to an XSD element can indirectly inherit from the base type; otherwise, false.</param>
		public XmlTypeMapping ImportDerivedTypeMapping(XmlQualifiedName name, Type baseType, bool baseTypeCanBeIndirect)
		{
			XmlQualifiedName typeQName;
			XmlSchemaType xmlSchemaType;
			if (this.encodedFormat)
			{
				typeQName = name;
				xmlSchemaType = (this.schemas.Find(name, typeof(XmlSchemaComplexType)) as XmlSchemaComplexType);
				if (xmlSchemaType == null)
				{
					throw new InvalidOperationException("Schema type '" + name + "' not found or not valid");
				}
			}
			else if (!this.LocateElement(name, out typeQName, out xmlSchemaType))
			{
				return null;
			}
			XmlTypeMapping xmlTypeMapping = this.GetRegisteredTypeMapping(typeQName, baseType);
			if (xmlTypeMapping != null)
			{
				this.SetMapBaseType(xmlTypeMapping, baseType);
				xmlTypeMapping.UpdateRoot(name);
				return xmlTypeMapping;
			}
			xmlTypeMapping = this.CreateTypeMapping(typeQName, SchemaTypes.Class, name);
			if (xmlSchemaType != null)
			{
				xmlTypeMapping.Documentation = this.GetDocumentation(xmlSchemaType);
				this.RegisterMapFixup(xmlTypeMapping, typeQName, (XmlSchemaComplexType)xmlSchemaType);
			}
			else
			{
				ClassMap classMap = new ClassMap();
				CodeIdentifiers classIds = new CodeIdentifiers();
				xmlTypeMapping.ObjectMap = classMap;
				this.AddTextMember(typeQName, classMap, classIds);
			}
			this.BuildPendingMaps();
			this.SetMapBaseType(xmlTypeMapping, baseType);
			return xmlTypeMapping;
		}

		private void SetMapBaseType(XmlTypeMapping map, Type baseType)
		{
			XmlTypeMapping xmlTypeMapping = null;
			while (map != null)
			{
				if (map.TypeData.Type == baseType)
				{
					return;
				}
				xmlTypeMapping = map;
				map = map.BaseMap;
			}
			XmlTypeMapping xmlTypeMapping2 = this.ReflectType(baseType);
			xmlTypeMapping.BaseMap = xmlTypeMapping2;
			xmlTypeMapping2.DerivedTypes.Add(xmlTypeMapping);
			xmlTypeMapping2.DerivedTypes.AddRange(xmlTypeMapping.DerivedTypes);
			ClassMap classMap = (ClassMap)xmlTypeMapping2.ObjectMap;
			ClassMap classMap2 = (ClassMap)xmlTypeMapping.ObjectMap;
			foreach (object obj in classMap.AllMembers)
			{
				XmlTypeMapMember member = (XmlTypeMapMember)obj;
				classMap2.AddMember(member);
			}
			foreach (object obj2 in xmlTypeMapping.DerivedTypes)
			{
				XmlTypeMapping xmlTypeMapping3 = (XmlTypeMapping)obj2;
				classMap2 = (ClassMap)xmlTypeMapping3.ObjectMap;
				foreach (object obj3 in classMap.AllMembers)
				{
					XmlTypeMapMember member2 = (XmlTypeMapMember)obj3;
					classMap2.AddMember(member2);
				}
			}
		}

		/// <summary>Generates internal type mapping information for a single element part of a literal-use SOAP message defined in a WSDL document. </summary>
		/// <returns>The .NET Framework type mapping for a WSDL message definition containing a single element part.</returns>
		/// <param name="name">An <see cref="T:System.Xml.XmlQualifiedName" /> that specifies the name of the message part.</param>
		public XmlMembersMapping ImportMembersMapping(XmlQualifiedName name)
		{
			XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.schemas.Find(name, typeof(XmlSchemaElement));
			if (xmlSchemaElement == null)
			{
				throw new InvalidOperationException("Schema element '" + name + "' not found or not valid");
			}
			XmlSchemaComplexType xmlSchemaComplexType;
			if (xmlSchemaElement.SchemaType != null)
			{
				xmlSchemaComplexType = (xmlSchemaElement.SchemaType as XmlSchemaComplexType);
			}
			else
			{
				if (xmlSchemaElement.SchemaTypeName.IsEmpty)
				{
					return null;
				}
				object obj = this.schemas.Find(xmlSchemaElement.SchemaTypeName, typeof(XmlSchemaComplexType));
				if (obj == null)
				{
					if (this.IsPrimitiveTypeNamespace(xmlSchemaElement.SchemaTypeName.Namespace))
					{
						return null;
					}
					throw new InvalidOperationException("Schema type '" + xmlSchemaElement.SchemaTypeName + "' not found");
				}
				else
				{
					xmlSchemaComplexType = (obj as XmlSchemaComplexType);
				}
			}
			if (xmlSchemaComplexType == null)
			{
				throw new InvalidOperationException("Schema element '" + name + "' not found or not valid");
			}
			XmlMemberMapping[] mapping = this.ImportMembersMappingComposite(xmlSchemaComplexType, name);
			return new XmlMembersMapping(name.Name, name.Namespace, mapping);
		}

		/// <summary>Generates internal type mapping information for the element parts of a literal-use SOAP message defined in a WSDL document. </summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlMembersMapping" /> that represents the .NET Framework type mappings for the element parts of a WSDL message definition.</returns>
		/// <param name="names">An array of type <see cref="T:System.Xml.XmlQualifiedName" /> that specifies the names of the message parts.</param>
		public XmlMembersMapping ImportMembersMapping(XmlQualifiedName[] names)
		{
			XmlMemberMapping[] array = new XmlMemberMapping[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.schemas.Find(names[i], typeof(XmlSchemaElement));
				if (xmlSchemaElement == null)
				{
					throw new InvalidOperationException("Schema element '" + names[i] + "' not found");
				}
				XmlQualifiedName xmlQualifiedName = new XmlQualifiedName("Message", names[i].Namespace);
				XmlTypeMapping emap;
				TypeData elementTypeData = this.GetElementTypeData(xmlQualifiedName, xmlSchemaElement, names[i], out emap);
				array[i] = this.ImportMemberMapping(xmlSchemaElement.Name, xmlQualifiedName.Namespace, xmlSchemaElement.IsNillable, elementTypeData, emap);
			}
			this.BuildPendingMaps();
			return new XmlMembersMapping(array);
		}

		/// <summary>Generates internal type mapping information for the element parts of a literal-use SOAP message defined in a WSDL document.</summary>
		/// <returns>A <see cref="T:System.Xml.Serialization.XmlMembersMapping" /> that contains type mapping information.</returns>
		/// <param name="name">The name of the element for which to generate a mapping.</param>
		/// <param name="ns">The namespace of the element for which to generate a mapping.</param>
		/// <param name="members">An array of <see cref="T:System.Xml.Serialization.SoapSchemaMember" /> instances that specifies the members of the element for which to generate a mapping.</param>
		[MonoTODO]
		public XmlMembersMapping ImportMembersMapping(string name, string ns, SoapSchemaMember[] members)
		{
			throw new NotImplementedException();
		}

		/// <summary>Generates internal type mapping information for an element defined in an XML schema document. </summary>
		/// <returns>A <see cref="T:System.Xml.Serialization.XmlTypeMapping" /> object that describes a type mapping.</returns>
		/// <param name="typeName">A <see cref="T:System.Xml.XmlQualifiedName" /> that specifies an XML element.</param>
		public XmlTypeMapping ImportSchemaType(XmlQualifiedName typeName)
		{
			return this.ImportSchemaType(typeName, typeof(object));
		}

		/// <summary>Generates internal type mapping information for an element defined in an XML schema document. </summary>
		/// <returns>A <see cref="T:System.Xml.Serialization.XmlTypeMapping" /> object that describes a type mapping.</returns>
		/// <param name="typeName">A <see cref="T:System.Xml.XmlQualifiedName" /> that specifies an XML element.</param>
		/// <param name="baseType">A <see cref="T:System.Type" /> object that specifies a base type.</param>
		public XmlTypeMapping ImportSchemaType(XmlQualifiedName typeName, Type baseType)
		{
			return this.ImportSchemaType(typeName, typeof(object), false);
		}

		/// <summary>Generates internal type mapping information for an element defined in an XML schema document. </summary>
		/// <returns>A <see cref="T:System.Xml.Serialization.XmlTypeMapping" /> object that describes a type mapping.</returns>
		/// <param name="typeName">A <see cref="T:System.Xml.XmlQualifiedName" /> that specifies an XML element.</param>
		/// <param name="baseType">A <see cref="T:System.Type" /> object that specifies a base type.</param>
		/// <param name="baseTypeCanBeIndirect">A <see cref="T:System.Boolean" /> value that specifies whether the generated type can indirectly inherit the <paramref name="baseType" />.</param>
		[MonoTODO("baseType and baseTypeCanBeIndirect are ignored")]
		public XmlTypeMapping ImportSchemaType(XmlQualifiedName typeName, Type baseType, bool baseTypeCanBeIndirect)
		{
			XmlSchemaType stype = ((XmlSchemaType)this.schemas.Find(typeName, typeof(XmlSchemaComplexType))) ?? ((XmlSchemaType)this.schemas.Find(typeName, typeof(XmlSchemaSimpleType)));
			return this.ImportTypeCommon(typeName, typeName, stype, true);
		}

		internal XmlMembersMapping ImportEncodedMembersMapping(string name, string ns, SoapSchemaMember[] members, bool hasWrapperElement)
		{
			XmlMemberMapping[] array = new XmlMemberMapping[members.Length];
			for (int i = 0; i < members.Length; i++)
			{
				TypeData typeData = this.GetTypeData(members[i].MemberType, null, false);
				XmlTypeMapping typeMapping = this.GetTypeMapping(typeData);
				array[i] = this.ImportMemberMapping(members[i].MemberName, members[i].MemberType.Namespace, true, typeData, typeMapping);
			}
			this.BuildPendingMaps();
			return new XmlMembersMapping(name, ns, hasWrapperElement, false, array);
		}

		internal XmlMembersMapping ImportEncodedMembersMapping(string name, string ns, SoapSchemaMember member)
		{
			XmlSchemaComplexType xmlSchemaComplexType = this.schemas.Find(member.MemberType, typeof(XmlSchemaComplexType)) as XmlSchemaComplexType;
			if (xmlSchemaComplexType == null)
			{
				throw new InvalidOperationException("Schema type '" + member.MemberType + "' not found or not valid");
			}
			XmlMemberMapping[] mapping = this.ImportMembersMappingComposite(xmlSchemaComplexType, member.MemberType);
			return new XmlMembersMapping(name, ns, mapping);
		}

		private XmlMemberMapping[] ImportMembersMappingComposite(XmlSchemaComplexType stype, XmlQualifiedName refer)
		{
			if (stype.Particle == null)
			{
				return new XmlMemberMapping[0];
			}
			ClassMap classMap = new ClassMap();
			XmlSchemaSequence xmlSchemaSequence = stype.Particle as XmlSchemaSequence;
			if (xmlSchemaSequence == null)
			{
				throw new InvalidOperationException("Schema element '" + refer + "' cannot be imported as XmlMembersMapping");
			}
			CodeIdentifiers classIds = new CodeIdentifiers();
			this.ImportParticleComplexContent(refer, classMap, xmlSchemaSequence, classIds, false);
			this.ImportAttributes(refer, classMap, stype.Attributes, stype.AnyAttribute, classIds);
			this.BuildPendingMaps();
			int num = 0;
			XmlMemberMapping[] array = new XmlMemberMapping[classMap.AllMembers.Count];
			foreach (object obj in classMap.AllMembers)
			{
				XmlTypeMapMember xmlTypeMapMember = (XmlTypeMapMember)obj;
				array[num++] = new XmlMemberMapping(xmlTypeMapMember.Name, refer.Namespace, xmlTypeMapMember, this.encodedFormat);
			}
			return array;
		}

		private XmlMemberMapping ImportMemberMapping(string name, string ns, bool isNullable, TypeData type, XmlTypeMapping emap)
		{
			XmlTypeMapMemberElement xmlTypeMapMemberElement;
			if (type.IsListType)
			{
				xmlTypeMapMemberElement = new XmlTypeMapMemberList();
			}
			else
			{
				xmlTypeMapMemberElement = new XmlTypeMapMemberElement();
			}
			xmlTypeMapMemberElement.Name = name;
			xmlTypeMapMemberElement.TypeData = type;
			xmlTypeMapMemberElement.ElementInfo.Add(this.CreateElementInfo(ns, xmlTypeMapMemberElement, name, type, isNullable, XmlSchemaForm.None, emap));
			return new XmlMemberMapping(name, ns, xmlTypeMapMemberElement, this.encodedFormat);
		}

		/// <summary>Generates internal type mapping information for the element parts of a literal-use SOAP message defined in a WSDL document.</summary>
		/// <returns>The .NET Framework type mappings for the element parts of a WSDL message definition.</returns>
		/// <param name="names">An array of type <see cref="T:System.Xml.XmlQualifiedName" /> that specifies the names of the message parts.</param>
		/// <param name="baseType">A base type for all .NET Framework types that are generated to correspond to message parts.</param>
		/// <param name="baseTypeCanBeIndirect">true to indicate that the types corresponding to message parts can indirectly inherit from the base type; otherwise, false.</param>
		[MonoTODO]
		public XmlMembersMapping ImportMembersMapping(XmlQualifiedName[] names, Type baseType, bool baseTypeCanBeIndirect)
		{
			throw new NotImplementedException();
		}

		/// <summary>Generates internal type mapping information for an element defined in an XML schema document. </summary>
		/// <returns>The .NET Framework type mapping information for an XML schema element.</returns>
		/// <param name="name">An <see cref="T:System.Xml.XmlQualifiedName" /> that specifies the name of an element defined in an XML schema document.</param>
		public XmlTypeMapping ImportTypeMapping(XmlQualifiedName name)
		{
			XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.schemas.Find(name, typeof(XmlSchemaElement));
			XmlQualifiedName qname;
			XmlSchemaType stype;
			if (!this.LocateElement(xmlSchemaElement, out qname, out stype))
			{
				throw new InvalidOperationException(string.Format("'{0}' is missing.", name));
			}
			return this.ImportTypeCommon(name, qname, stype, xmlSchemaElement.IsNillable);
		}

		private XmlTypeMapping ImportTypeCommon(XmlQualifiedName name, XmlQualifiedName qname, XmlSchemaType stype, bool isNullable)
		{
			if (stype == null)
			{
				if (qname == XmlSchemaImporter.anyType)
				{
					XmlTypeMapping typeMapping = this.GetTypeMapping(TypeTranslator.GetTypeData(typeof(object)));
					this.BuildPendingMaps();
					return typeMapping;
				}
				TypeData primitiveTypeData = TypeTranslator.GetPrimitiveTypeData(qname.Name);
				return this.ReflectType(primitiveTypeData, name.Namespace);
			}
			else
			{
				XmlTypeMapping xmlTypeMapping = this.GetRegisteredTypeMapping(qname);
				if (xmlTypeMapping != null)
				{
					return xmlTypeMapping;
				}
				if (stype is XmlSchemaSimpleType)
				{
					return this.ImportClassSimpleType(stype.QualifiedName, (XmlSchemaSimpleType)stype, name);
				}
				xmlTypeMapping = this.CreateTypeMapping(qname, SchemaTypes.Class, name);
				xmlTypeMapping.Documentation = this.GetDocumentation(stype);
				xmlTypeMapping.IsNullable = isNullable;
				this.RegisterMapFixup(xmlTypeMapping, qname, (XmlSchemaComplexType)stype);
				this.BuildPendingMaps();
				return xmlTypeMapping;
			}
		}

		private bool LocateElement(XmlQualifiedName name, out XmlQualifiedName qname, out XmlSchemaType stype)
		{
			XmlSchemaElement elem = (XmlSchemaElement)this.schemas.Find(name, typeof(XmlSchemaElement));
			return this.LocateElement(elem, out qname, out stype);
		}

		private bool LocateElement(XmlSchemaElement elem, out XmlQualifiedName qname, out XmlSchemaType stype)
		{
			qname = null;
			stype = null;
			if (elem == null)
			{
				return false;
			}
			if (elem.SchemaType != null)
			{
				stype = elem.SchemaType;
				qname = elem.QualifiedName;
			}
			else
			{
				if (elem.ElementType == XmlSchemaComplexType.AnyType)
				{
					qname = XmlSchemaImporter.anyType;
					return true;
				}
				if (elem.SchemaTypeName.IsEmpty)
				{
					return false;
				}
				object obj = this.schemas.Find(elem.SchemaTypeName, typeof(XmlSchemaComplexType));
				if (obj == null)
				{
					obj = this.schemas.Find(elem.SchemaTypeName, typeof(XmlSchemaSimpleType));
				}
				if (obj == null)
				{
					if (this.IsPrimitiveTypeNamespace(elem.SchemaTypeName.Namespace))
					{
						qname = elem.SchemaTypeName;
						return true;
					}
					throw new InvalidOperationException("Schema type '" + elem.SchemaTypeName + "' not found");
				}
				else
				{
					stype = (XmlSchemaType)obj;
					qname = stype.QualifiedName;
					XmlSchemaType xmlSchemaType = stype.BaseSchemaType as XmlSchemaType;
					if (xmlSchemaType != null && xmlSchemaType.QualifiedName == elem.SchemaTypeName)
					{
						throw new InvalidOperationException(string.Concat(new string[]
						{
							"Cannot import schema for type '",
							elem.SchemaTypeName.Name,
							"' from namespace '",
							elem.SchemaTypeName.Namespace,
							"'. Redefine not supported"
						}));
					}
				}
			}
			return true;
		}

		private XmlTypeMapping ImportType(XmlQualifiedName name, XmlQualifiedName root, bool throwOnError)
		{
			XmlTypeMapping registeredTypeMapping = this.GetRegisteredTypeMapping(name);
			if (registeredTypeMapping != null)
			{
				registeredTypeMapping.UpdateRoot(root);
				return registeredTypeMapping;
			}
			XmlSchemaType xmlSchemaType = (XmlSchemaType)this.schemas.Find(name, typeof(XmlSchemaComplexType));
			if (xmlSchemaType == null)
			{
				xmlSchemaType = (XmlSchemaType)this.schemas.Find(name, typeof(XmlSchemaSimpleType));
			}
			if (xmlSchemaType != null)
			{
				return this.ImportType(name, xmlSchemaType, root);
			}
			if (!throwOnError)
			{
				return null;
			}
			if (name.Namespace == "http://schemas.xmlsoap.org/soap/encoding/")
			{
				throw new InvalidOperationException("Referenced type '" + name + "' valid only for encoded SOAP.");
			}
			throw new InvalidOperationException("Referenced type '" + name + "' not found.");
		}

		private XmlTypeMapping ImportClass(XmlQualifiedName name)
		{
			XmlTypeMapping xmlTypeMapping = this.ImportType(name, null, true);
			if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Class)
			{
				return xmlTypeMapping;
			}
			XmlSchemaComplexType stype = this.schemas.Find(name, typeof(XmlSchemaComplexType)) as XmlSchemaComplexType;
			return this.CreateClassMap(name, stype, new XmlQualifiedName(xmlTypeMapping.ElementName, xmlTypeMapping.Namespace));
		}

		private XmlTypeMapping ImportType(XmlQualifiedName name, XmlSchemaType stype, XmlQualifiedName root)
		{
			XmlTypeMapping registeredTypeMapping = this.GetRegisteredTypeMapping(name);
			if (registeredTypeMapping != null)
			{
				XmlSchemaComplexType xmlSchemaComplexType = stype as XmlSchemaComplexType;
				if (registeredTypeMapping.TypeData.SchemaType != SchemaTypes.Class || xmlSchemaComplexType == null || !this.CanBeArray(name, xmlSchemaComplexType))
				{
					registeredTypeMapping.UpdateRoot(root);
					return registeredTypeMapping;
				}
			}
			if (stype is XmlSchemaComplexType)
			{
				return this.ImportClassComplexType(name, (XmlSchemaComplexType)stype, root);
			}
			if (stype is XmlSchemaSimpleType)
			{
				return this.ImportClassSimpleType(name, (XmlSchemaSimpleType)stype, root);
			}
			throw new NotSupportedException("Schema type not supported: " + stype.GetType());
		}

		private XmlTypeMapping ImportClassComplexType(XmlQualifiedName typeQName, XmlSchemaComplexType stype, XmlQualifiedName root)
		{
			Type anyElementType = this.GetAnyElementType(stype);
			if (anyElementType != null)
			{
				return this.GetTypeMapping(TypeTranslator.GetTypeData(anyElementType));
			}
			if (this.CanBeArray(typeQName, stype))
			{
				TypeData arrayTypeData;
				ListMap listMap = this.BuildArrayMap(typeQName, stype, out arrayTypeData);
				if (listMap != null)
				{
					XmlTypeMapping xmlTypeMapping = this.CreateArrayTypeMapping(typeQName, arrayTypeData);
					xmlTypeMapping.ObjectMap = listMap;
					return xmlTypeMapping;
				}
			}
			else if (this.CanBeIXmlSerializable(stype))
			{
				return this.ImportXmlSerializableMapping(typeQName.Namespace);
			}
			return this.CreateClassMap(typeQName, stype, root);
		}

		private XmlTypeMapping CreateClassMap(XmlQualifiedName typeQName, XmlSchemaComplexType stype, XmlQualifiedName root)
		{
			XmlTypeMapping xmlTypeMapping = this.CreateTypeMapping(typeQName, SchemaTypes.Class, root);
			xmlTypeMapping.Documentation = this.GetDocumentation(stype);
			this.RegisterMapFixup(xmlTypeMapping, typeQName, stype);
			return xmlTypeMapping;
		}

		private void RegisterMapFixup(XmlTypeMapping map, XmlQualifiedName typeQName, XmlSchemaComplexType stype)
		{
			XmlSchemaImporter.MapFixup mapFixup = new XmlSchemaImporter.MapFixup();
			mapFixup.Map = map;
			mapFixup.SchemaType = stype;
			mapFixup.TypeName = typeQName;
			this.pendingMaps.Enqueue(mapFixup);
		}

		private void BuildPendingMaps()
		{
			while (this.pendingMaps.Count > 0)
			{
				XmlSchemaImporter.MapFixup mapFixup = (XmlSchemaImporter.MapFixup)this.pendingMaps.Dequeue();
				if (mapFixup.Map.ObjectMap == null)
				{
					this.BuildClassMap(mapFixup.Map, mapFixup.TypeName, mapFixup.SchemaType);
					if (mapFixup.Map.ObjectMap == null)
					{
						this.pendingMaps.Enqueue(mapFixup);
					}
				}
			}
		}

		private void BuildPendingMap(XmlTypeMapping map)
		{
			if (map.ObjectMap != null)
			{
				return;
			}
			foreach (object obj in this.pendingMaps)
			{
				XmlSchemaImporter.MapFixup mapFixup = (XmlSchemaImporter.MapFixup)obj;
				if (mapFixup.Map == map)
				{
					this.BuildClassMap(mapFixup.Map, mapFixup.TypeName, mapFixup.SchemaType);
					return;
				}
			}
			throw new InvalidOperationException("Can't complete map of type " + map.XmlType + " : " + map.Namespace);
		}

		private void BuildClassMap(XmlTypeMapping map, XmlQualifiedName typeQName, XmlSchemaComplexType stype)
		{
			CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
			codeIdentifiers.AddReserved(map.TypeData.TypeName);
			ClassMap classMap = new ClassMap();
			map.ObjectMap = classMap;
			bool isMixed = stype.IsMixed;
			if (stype.Particle != null)
			{
				this.ImportParticleComplexContent(typeQName, classMap, stype.Particle, codeIdentifiers, isMixed);
			}
			else if (stype.ContentModel is XmlSchemaSimpleContent)
			{
				this.ImportSimpleContent(typeQName, map, (XmlSchemaSimpleContent)stype.ContentModel, codeIdentifiers, isMixed);
			}
			else if (stype.ContentModel is XmlSchemaComplexContent)
			{
				this.ImportComplexContent(typeQName, map, (XmlSchemaComplexContent)stype.ContentModel, codeIdentifiers, isMixed);
			}
			this.ImportAttributes(typeQName, classMap, stype.Attributes, stype.AnyAttribute, codeIdentifiers);
			this.ImportExtensionTypes(typeQName);
			if (isMixed)
			{
				this.AddTextMember(typeQName, classMap, codeIdentifiers);
			}
			this.AddObjectDerivedMap(map);
		}

		private void ImportAttributes(XmlQualifiedName typeQName, ClassMap cmap, XmlSchemaObjectCollection atts, XmlSchemaAnyAttribute anyat, CodeIdentifiers classIds)
		{
			atts = this.CollectAttributeUsesNonOverlap(atts, cmap);
			if (anyat != null)
			{
				XmlTypeMapMemberAnyAttribute xmlTypeMapMemberAnyAttribute = new XmlTypeMapMemberAnyAttribute();
				xmlTypeMapMemberAnyAttribute.Name = classIds.AddUnique("AnyAttribute", xmlTypeMapMemberAnyAttribute);
				xmlTypeMapMemberAnyAttribute.TypeData = TypeTranslator.GetTypeData(typeof(XmlAttribute[]));
				cmap.AddMember(xmlTypeMapMemberAnyAttribute);
			}
			foreach (XmlSchemaObject xmlSchemaObject in atts)
			{
				if (xmlSchemaObject is XmlSchemaAttribute)
				{
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)xmlSchemaObject;
					string @namespace;
					XmlSchemaAttribute refAttribute = this.GetRefAttribute(typeQName, xmlSchemaAttribute, out @namespace);
					XmlTypeMapMemberAttribute xmlTypeMapMemberAttribute = new XmlTypeMapMemberAttribute();
					xmlTypeMapMemberAttribute.Name = classIds.AddUnique(CodeIdentifier.MakeValid(refAttribute.Name), xmlTypeMapMemberAttribute);
					xmlTypeMapMemberAttribute.Documentation = this.GetDocumentation(xmlSchemaAttribute);
					xmlTypeMapMemberAttribute.AttributeName = refAttribute.Name;
					xmlTypeMapMemberAttribute.Namespace = @namespace;
					xmlTypeMapMemberAttribute.Form = refAttribute.Form;
					xmlTypeMapMemberAttribute.TypeData = this.GetAttributeTypeData(typeQName, xmlSchemaAttribute);
					if (refAttribute.DefaultValue != null)
					{
						xmlTypeMapMemberAttribute.DefaultValue = this.ImportDefaultValue(xmlTypeMapMemberAttribute.TypeData, refAttribute.DefaultValue);
					}
					else if (xmlTypeMapMemberAttribute.TypeData.IsValueType)
					{
						xmlTypeMapMemberAttribute.IsOptionalValueType = (refAttribute.ValidatedUse != XmlSchemaUse.Required);
					}
					if (xmlTypeMapMemberAttribute.TypeData.IsComplexType)
					{
						xmlTypeMapMemberAttribute.MappedType = this.GetTypeMapping(xmlTypeMapMemberAttribute.TypeData);
					}
					cmap.AddMember(xmlTypeMapMemberAttribute);
				}
				else if (xmlSchemaObject is XmlSchemaAttributeGroupRef)
				{
					XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = (XmlSchemaAttributeGroupRef)xmlSchemaObject;
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup = this.FindRefAttributeGroup(xmlSchemaAttributeGroupRef.RefName);
					this.ImportAttributes(typeQName, cmap, xmlSchemaAttributeGroup.Attributes, xmlSchemaAttributeGroup.AnyAttribute, classIds);
				}
			}
		}

		private XmlSchemaObjectCollection CollectAttributeUsesNonOverlap(XmlSchemaObjectCollection src, ClassMap map)
		{
			XmlSchemaObjectCollection xmlSchemaObjectCollection = new XmlSchemaObjectCollection();
			foreach (XmlSchemaObject xmlSchemaObject in src)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)xmlSchemaObject;
				if (map.GetAttribute(xmlSchemaAttribute.QualifiedName.Name, xmlSchemaAttribute.QualifiedName.Namespace) == null)
				{
					xmlSchemaObjectCollection.Add(xmlSchemaAttribute);
				}
			}
			return xmlSchemaObjectCollection;
		}

		private ListMap BuildArrayMap(XmlQualifiedName typeQName, XmlSchemaComplexType stype, out TypeData arrayTypeData)
		{
			if (this.encodedFormat)
			{
				XmlSchemaComplexContent xmlSchemaComplexContent = stype.ContentModel as XmlSchemaComplexContent;
				XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = xmlSchemaComplexContent.Content as XmlSchemaComplexContentRestriction;
				XmlSchemaAttribute xmlSchemaAttribute = this.FindArrayAttribute(xmlSchemaComplexContentRestriction.Attributes);
				if (xmlSchemaAttribute != null)
				{
					XmlAttribute[] unhandledAttributes = xmlSchemaAttribute.UnhandledAttributes;
					if (unhandledAttributes == null || unhandledAttributes.Length == 0)
					{
						throw new InvalidOperationException("arrayType attribute not specified in array declaration: " + typeQName);
					}
					XmlAttribute xmlAttribute = null;
					foreach (XmlAttribute xmlAttribute2 in unhandledAttributes)
					{
						if (xmlAttribute2.LocalName == "arrayType" && xmlAttribute2.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/")
						{
							xmlAttribute = xmlAttribute2;
							break;
						}
					}
					if (xmlAttribute == null)
					{
						throw new InvalidOperationException("arrayType attribute not specified in array declaration: " + typeQName);
					}
					string str;
					string ns;
					string str2;
					TypeTranslator.ParseArrayType(xmlAttribute.Value, out str, out ns, out str2);
					return this.BuildEncodedArrayMap(str + str2, ns, out arrayTypeData);
				}
				else
				{
					XmlSchemaElement xmlSchemaElement = null;
					XmlSchemaSequence xmlSchemaSequence = xmlSchemaComplexContentRestriction.Particle as XmlSchemaSequence;
					if (xmlSchemaSequence != null && xmlSchemaSequence.Items.Count == 1)
					{
						xmlSchemaElement = (xmlSchemaSequence.Items[0] as XmlSchemaElement);
					}
					else
					{
						XmlSchemaAll xmlSchemaAll = xmlSchemaComplexContentRestriction.Particle as XmlSchemaAll;
						if (xmlSchemaAll != null && xmlSchemaAll.Items.Count == 1)
						{
							xmlSchemaElement = (xmlSchemaAll.Items[0] as XmlSchemaElement);
						}
					}
					if (xmlSchemaElement == null)
					{
						throw new InvalidOperationException("Unknown array format");
					}
					return this.BuildEncodedArrayMap(xmlSchemaElement.SchemaTypeName.Name + "[]", xmlSchemaElement.SchemaTypeName.Namespace, out arrayTypeData);
				}
			}
			else
			{
				ClassMap classMap = new ClassMap();
				CodeIdentifiers classIds = new CodeIdentifiers();
				this.ImportParticleComplexContent(typeQName, classMap, stype.Particle, classIds, stype.IsMixed);
				XmlTypeMapMemberFlatList xmlTypeMapMemberFlatList = (classMap.AllMembers.Count != 1) ? null : (classMap.AllMembers[0] as XmlTypeMapMemberFlatList);
				if (xmlTypeMapMemberFlatList != null && xmlTypeMapMemberFlatList.ChoiceMember == null)
				{
					arrayTypeData = xmlTypeMapMemberFlatList.TypeData;
					return xmlTypeMapMemberFlatList.ListMap;
				}
				arrayTypeData = null;
				return null;
			}
		}

		private ListMap BuildEncodedArrayMap(string type, string ns, out TypeData arrayTypeData)
		{
			ListMap listMap = new ListMap();
			int num = type.LastIndexOf("[");
			if (num == -1)
			{
				throw new InvalidOperationException("Invalid arrayType value: " + type);
			}
			if (type.IndexOf(",", num) != -1)
			{
				throw new InvalidOperationException("Multidimensional arrays are not supported");
			}
			string text = type.Substring(0, num);
			TypeData typeData;
			if (text.IndexOf("[") != -1)
			{
				ListMap objectMap = this.BuildEncodedArrayMap(text, ns, out typeData);
				int dimensions = text.Split(new char[]
				{
					'['
				}).Length - 1;
				string arrayName = TypeTranslator.GetArrayName(type, dimensions);
				XmlQualifiedName typeQName = new XmlQualifiedName(arrayName, ns);
				XmlTypeMapping xmlTypeMapping = this.CreateArrayTypeMapping(typeQName, typeData);
				xmlTypeMapping.ObjectMap = objectMap;
			}
			else
			{
				typeData = this.GetTypeData(new XmlQualifiedName(text, ns), null, false);
			}
			arrayTypeData = typeData.ListTypeData;
			listMap.ItemInfo = new XmlTypeMapElementInfoList();
			listMap.ItemInfo.Add(this.CreateElementInfo(string.Empty, null, "Item", typeData, true, XmlSchemaForm.None));
			return listMap;
		}

		private XmlSchemaAttribute FindArrayAttribute(XmlSchemaObjectCollection atts)
		{
			foreach (object obj in atts)
			{
				XmlSchemaAttribute xmlSchemaAttribute = obj as XmlSchemaAttribute;
				if (xmlSchemaAttribute != null && xmlSchemaAttribute.RefName == XmlSchemaImporter.arrayTypeRefName)
				{
					return xmlSchemaAttribute;
				}
				XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = obj as XmlSchemaAttributeGroupRef;
				if (xmlSchemaAttributeGroupRef != null)
				{
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup = this.FindRefAttributeGroup(xmlSchemaAttributeGroupRef.RefName);
					xmlSchemaAttribute = this.FindArrayAttribute(xmlSchemaAttributeGroup.Attributes);
					if (xmlSchemaAttribute != null)
					{
						return xmlSchemaAttribute;
					}
				}
			}
			return null;
		}

		private void ImportParticleComplexContent(XmlQualifiedName typeQName, ClassMap cmap, XmlSchemaParticle particle, CodeIdentifiers classIds, bool isMixed)
		{
			this.ImportParticleContent(typeQName, cmap, particle, classIds, false, ref isMixed);
			if (isMixed)
			{
				this.AddTextMember(typeQName, cmap, classIds);
			}
		}

		private void AddTextMember(XmlQualifiedName typeQName, ClassMap cmap, CodeIdentifiers classIds)
		{
			if (cmap.XmlTextCollector == null)
			{
				XmlTypeMapMemberFlatList xmlTypeMapMemberFlatList = new XmlTypeMapMemberFlatList();
				xmlTypeMapMemberFlatList.Name = classIds.AddUnique("Text", xmlTypeMapMemberFlatList);
				xmlTypeMapMemberFlatList.TypeData = TypeTranslator.GetTypeData(typeof(string[]));
				xmlTypeMapMemberFlatList.ElementInfo.Add(this.CreateTextElementInfo(typeQName.Namespace, xmlTypeMapMemberFlatList, xmlTypeMapMemberFlatList.TypeData.ListItemTypeData));
				xmlTypeMapMemberFlatList.IsXmlTextCollector = true;
				xmlTypeMapMemberFlatList.ListMap = new ListMap();
				xmlTypeMapMemberFlatList.ListMap.ItemInfo = xmlTypeMapMemberFlatList.ElementInfo;
				cmap.AddMember(xmlTypeMapMemberFlatList);
			}
		}

		private void ImportParticleContent(XmlQualifiedName typeQName, ClassMap cmap, XmlSchemaParticle particle, CodeIdentifiers classIds, bool multiValue, ref bool isMixed)
		{
			if (particle == null)
			{
				return;
			}
			if (particle is XmlSchemaGroupRef)
			{
				particle = this.GetRefGroupParticle((XmlSchemaGroupRef)particle);
			}
			if (particle.MaxOccurs > 1m)
			{
				multiValue = true;
			}
			if (particle is XmlSchemaSequence)
			{
				this.ImportSequenceContent(typeQName, cmap, ((XmlSchemaSequence)particle).Items, classIds, multiValue, ref isMixed);
			}
			else if (particle is XmlSchemaChoice)
			{
				if (((XmlSchemaChoice)particle).Items.Count == 1)
				{
					this.ImportSequenceContent(typeQName, cmap, ((XmlSchemaChoice)particle).Items, classIds, multiValue, ref isMixed);
				}
				else
				{
					this.ImportChoiceContent(typeQName, cmap, (XmlSchemaChoice)particle, classIds, multiValue);
				}
			}
			else if (particle is XmlSchemaAll)
			{
				this.ImportSequenceContent(typeQName, cmap, ((XmlSchemaAll)particle).Items, classIds, multiValue, ref isMixed);
			}
		}

		private void ImportSequenceContent(XmlQualifiedName typeQName, ClassMap cmap, XmlSchemaObjectCollection items, CodeIdentifiers classIds, bool multiValue, ref bool isMixed)
		{
			foreach (XmlSchemaObject xmlSchemaObject in items)
			{
				if (xmlSchemaObject is XmlSchemaElement)
				{
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaObject;
					XmlTypeMapping emap;
					TypeData typeData = this.GetElementTypeData(typeQName, xmlSchemaElement, null, out emap);
					string ns;
					XmlSchemaElement refElement = this.GetRefElement(typeQName, xmlSchemaElement, out ns);
					if (xmlSchemaElement.MaxOccurs == 1m && !multiValue)
					{
						XmlTypeMapMemberElement xmlTypeMapMemberElement;
						if (typeData.SchemaType != SchemaTypes.Array)
						{
							xmlTypeMapMemberElement = new XmlTypeMapMemberElement();
							if (refElement.DefaultValue != null)
							{
								xmlTypeMapMemberElement.DefaultValue = this.ImportDefaultValue(typeData, refElement.DefaultValue);
							}
						}
						else if (this.GetTypeMapping(typeData).IsSimpleType)
						{
							xmlTypeMapMemberElement = new XmlTypeMapMemberElement();
							typeData = TypeTranslator.GetTypeData(typeof(string));
						}
						else
						{
							xmlTypeMapMemberElement = new XmlTypeMapMemberList();
						}
						if (xmlSchemaElement.MinOccurs == 0m && typeData.IsValueType)
						{
							xmlTypeMapMemberElement.IsOptionalValueType = true;
						}
						xmlTypeMapMemberElement.Name = classIds.AddUnique(CodeIdentifier.MakeValid(refElement.Name), xmlTypeMapMemberElement);
						xmlTypeMapMemberElement.Documentation = this.GetDocumentation(xmlSchemaElement);
						xmlTypeMapMemberElement.TypeData = typeData;
						xmlTypeMapMemberElement.ElementInfo.Add(this.CreateElementInfo(ns, xmlTypeMapMemberElement, refElement.Name, typeData, refElement.IsNillable, refElement.Form, emap));
						cmap.AddMember(xmlTypeMapMemberElement);
					}
					else
					{
						XmlTypeMapMemberFlatList xmlTypeMapMemberFlatList = new XmlTypeMapMemberFlatList();
						xmlTypeMapMemberFlatList.ListMap = new ListMap();
						xmlTypeMapMemberFlatList.Name = classIds.AddUnique(CodeIdentifier.MakeValid(refElement.Name), xmlTypeMapMemberFlatList);
						xmlTypeMapMemberFlatList.Documentation = this.GetDocumentation(xmlSchemaElement);
						xmlTypeMapMemberFlatList.TypeData = typeData.ListTypeData;
						xmlTypeMapMemberFlatList.ElementInfo.Add(this.CreateElementInfo(ns, xmlTypeMapMemberFlatList, refElement.Name, typeData, refElement.IsNillable, refElement.Form, emap));
						xmlTypeMapMemberFlatList.ListMap.ItemInfo = xmlTypeMapMemberFlatList.ElementInfo;
						cmap.AddMember(xmlTypeMapMemberFlatList);
					}
				}
				else if (xmlSchemaObject is XmlSchemaAny)
				{
					XmlSchemaAny xmlSchemaAny = (XmlSchemaAny)xmlSchemaObject;
					XmlTypeMapMemberAnyElement xmlTypeMapMemberAnyElement = new XmlTypeMapMemberAnyElement();
					xmlTypeMapMemberAnyElement.Name = classIds.AddUnique("Any", xmlTypeMapMemberAnyElement);
					xmlTypeMapMemberAnyElement.Documentation = this.GetDocumentation(xmlSchemaAny);
					Type type;
					if (xmlSchemaAny.MaxOccurs != 1m || multiValue)
					{
						type = ((!isMixed) ? typeof(XmlElement[]) : typeof(XmlNode[]));
					}
					else
					{
						type = ((!isMixed) ? typeof(XmlElement) : typeof(XmlNode));
					}
					xmlTypeMapMemberAnyElement.TypeData = TypeTranslator.GetTypeData(type);
					XmlTypeMapElementInfo xmlTypeMapElementInfo = new XmlTypeMapElementInfo(xmlTypeMapMemberAnyElement, xmlTypeMapMemberAnyElement.TypeData);
					xmlTypeMapElementInfo.IsUnnamedAnyElement = true;
					xmlTypeMapMemberAnyElement.ElementInfo.Add(xmlTypeMapElementInfo);
					if (isMixed)
					{
						xmlTypeMapElementInfo = this.CreateTextElementInfo(typeQName.Namespace, xmlTypeMapMemberAnyElement, xmlTypeMapMemberAnyElement.TypeData);
						xmlTypeMapMemberAnyElement.ElementInfo.Add(xmlTypeMapElementInfo);
						xmlTypeMapMemberAnyElement.IsXmlTextCollector = true;
						isMixed = false;
					}
					cmap.AddMember(xmlTypeMapMemberAnyElement);
				}
				else if (xmlSchemaObject is XmlSchemaParticle)
				{
					this.ImportParticleContent(typeQName, cmap, (XmlSchemaParticle)xmlSchemaObject, classIds, multiValue, ref isMixed);
				}
			}
		}

		private object ImportDefaultValue(TypeData typeData, string value)
		{
			if (typeData.SchemaType != SchemaTypes.Enum)
			{
				return XmlCustomFormatter.FromXmlString(typeData, value);
			}
			XmlTypeMapping typeMapping = this.GetTypeMapping(typeData);
			EnumMap enumMap = (EnumMap)typeMapping.ObjectMap;
			string enumName = enumMap.GetEnumName(typeMapping.TypeFullName, value);
			if (enumName == null)
			{
				throw new InvalidOperationException("'" + value + "' is not a valid enumeration value");
			}
			return enumName;
		}

		private void ImportChoiceContent(XmlQualifiedName typeQName, ClassMap cmap, XmlSchemaChoice choice, CodeIdentifiers classIds, bool multiValue)
		{
			XmlTypeMapElementInfoList xmlTypeMapElementInfoList = new XmlTypeMapElementInfoList();
			multiValue = (this.ImportChoices(typeQName, null, xmlTypeMapElementInfoList, choice.Items) || multiValue);
			if (xmlTypeMapElementInfoList.Count == 0)
			{
				return;
			}
			if (choice.MaxOccurs > 1m)
			{
				multiValue = true;
			}
			XmlTypeMapMemberElement xmlTypeMapMemberElement;
			if (multiValue)
			{
				xmlTypeMapMemberElement = new XmlTypeMapMemberFlatList();
				xmlTypeMapMemberElement.Name = classIds.AddUnique("Items", xmlTypeMapMemberElement);
				ListMap listMap = new ListMap();
				listMap.ItemInfo = xmlTypeMapElementInfoList;
				((XmlTypeMapMemberFlatList)xmlTypeMapMemberElement).ListMap = listMap;
			}
			else
			{
				xmlTypeMapMemberElement = new XmlTypeMapMemberElement();
				xmlTypeMapMemberElement.Name = classIds.AddUnique("Item", xmlTypeMapMemberElement);
			}
			TypeData typeData = null;
			bool flag = false;
			bool flag2 = true;
			Hashtable hashtable = new Hashtable();
			for (int i = xmlTypeMapElementInfoList.Count - 1; i >= 0; i--)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)xmlTypeMapElementInfoList[i];
				if (cmap.GetElement(xmlTypeMapElementInfo.ElementName, xmlTypeMapElementInfo.Namespace) != null || xmlTypeMapElementInfoList.IndexOfElement(xmlTypeMapElementInfo.ElementName, xmlTypeMapElementInfo.Namespace) != i)
				{
					xmlTypeMapElementInfoList.RemoveAt(i);
				}
				else
				{
					if (hashtable.ContainsKey(xmlTypeMapElementInfo.TypeData))
					{
						flag = true;
					}
					else
					{
						hashtable.Add(xmlTypeMapElementInfo.TypeData, xmlTypeMapElementInfo);
					}
					TypeData typeData2 = xmlTypeMapElementInfo.TypeData;
					if (typeData2.SchemaType == SchemaTypes.Class)
					{
						XmlTypeMapping xmlTypeMapping = this.GetTypeMapping(typeData2);
						this.BuildPendingMap(xmlTypeMapping);
						while (xmlTypeMapping.BaseMap != null)
						{
							xmlTypeMapping = xmlTypeMapping.BaseMap;
							this.BuildPendingMap(xmlTypeMapping);
							typeData2 = xmlTypeMapping.TypeData;
						}
					}
					if (typeData == null)
					{
						typeData = typeData2;
					}
					else if (typeData != typeData2)
					{
						flag2 = false;
					}
				}
			}
			if (!flag2)
			{
				typeData = TypeTranslator.GetTypeData(typeof(object));
			}
			if (flag)
			{
				XmlTypeMapMemberElement xmlTypeMapMemberElement2 = new XmlTypeMapMemberElement();
				xmlTypeMapMemberElement2.Ignore = true;
				xmlTypeMapMemberElement2.Name = classIds.AddUnique(xmlTypeMapMemberElement.Name + "ElementName", xmlTypeMapMemberElement2);
				xmlTypeMapMemberElement.ChoiceMember = xmlTypeMapMemberElement2.Name;
				XmlTypeMapping xmlTypeMapping2 = this.CreateTypeMapping(new XmlQualifiedName(xmlTypeMapMemberElement.Name + "ChoiceType", typeQName.Namespace), SchemaTypes.Enum, null);
				xmlTypeMapping2.IncludeInSchema = false;
				CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
				EnumMap.EnumMapMember[] array = new EnumMap.EnumMapMember[xmlTypeMapElementInfoList.Count];
				for (int j = 0; j < xmlTypeMapElementInfoList.Count; j++)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo2 = (XmlTypeMapElementInfo)xmlTypeMapElementInfoList[j];
					bool flag3 = xmlTypeMapElementInfo2.Namespace != null && xmlTypeMapElementInfo2.Namespace != string.Empty && xmlTypeMapElementInfo2.Namespace != typeQName.Namespace;
					string xmlName = (!flag3) ? xmlTypeMapElementInfo2.ElementName : (xmlTypeMapElementInfo2.Namespace + ":" + xmlTypeMapElementInfo2.ElementName);
					string enumName = codeIdentifiers.AddUnique(CodeIdentifier.MakeValid(xmlTypeMapElementInfo2.ElementName), xmlTypeMapElementInfo2);
					array[j] = new EnumMap.EnumMapMember(xmlName, enumName);
				}
				xmlTypeMapping2.ObjectMap = new EnumMap(array, false);
				xmlTypeMapMemberElement2.TypeData = ((!multiValue) ? xmlTypeMapping2.TypeData : xmlTypeMapping2.TypeData.ListTypeData);
				xmlTypeMapMemberElement2.ElementInfo.Add(this.CreateElementInfo(typeQName.Namespace, xmlTypeMapMemberElement2, xmlTypeMapMemberElement2.Name, xmlTypeMapMemberElement2.TypeData, false, XmlSchemaForm.None));
				cmap.AddMember(xmlTypeMapMemberElement2);
			}
			if (typeData == null)
			{
				return;
			}
			if (multiValue)
			{
				typeData = typeData.ListTypeData;
			}
			xmlTypeMapMemberElement.ElementInfo = xmlTypeMapElementInfoList;
			xmlTypeMapMemberElement.Documentation = this.GetDocumentation(choice);
			xmlTypeMapMemberElement.TypeData = typeData;
			cmap.AddMember(xmlTypeMapMemberElement);
		}

		private bool ImportChoices(XmlQualifiedName typeQName, XmlTypeMapMember member, XmlTypeMapElementInfoList choices, XmlSchemaObjectCollection items)
		{
			bool flag = false;
			foreach (XmlSchemaObject xmlSchemaObject in items)
			{
				XmlSchemaObject xmlSchemaObject2 = xmlSchemaObject;
				if (xmlSchemaObject2 is XmlSchemaGroupRef)
				{
					xmlSchemaObject2 = this.GetRefGroupParticle((XmlSchemaGroupRef)xmlSchemaObject2);
				}
				if (xmlSchemaObject2 is XmlSchemaElement)
				{
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaObject2;
					XmlTypeMapping emap;
					TypeData elementTypeData = this.GetElementTypeData(typeQName, xmlSchemaElement, null, out emap);
					string ns;
					XmlSchemaElement refElement = this.GetRefElement(typeQName, xmlSchemaElement, out ns);
					choices.Add(this.CreateElementInfo(ns, member, refElement.Name, elementTypeData, refElement.IsNillable, refElement.Form, emap));
					if (xmlSchemaElement.MaxOccurs > 1m)
					{
						flag = true;
					}
				}
				else if (xmlSchemaObject2 is XmlSchemaAny)
				{
					choices.Add(new XmlTypeMapElementInfo(member, TypeTranslator.GetTypeData(typeof(XmlElement)))
					{
						IsUnnamedAnyElement = true
					});
				}
				else if (xmlSchemaObject2 is XmlSchemaChoice)
				{
					flag = (this.ImportChoices(typeQName, member, choices, ((XmlSchemaChoice)xmlSchemaObject2).Items) || flag);
				}
				else if (xmlSchemaObject2 is XmlSchemaSequence)
				{
					flag = (this.ImportChoices(typeQName, member, choices, ((XmlSchemaSequence)xmlSchemaObject2).Items) || flag);
				}
			}
			return flag;
		}

		private void ImportSimpleContent(XmlQualifiedName typeQName, XmlTypeMapping map, XmlSchemaSimpleContent content, CodeIdentifiers classIds, bool isMixed)
		{
			XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = content.Content as XmlSchemaSimpleContentExtension;
			ClassMap classMap = (ClassMap)map.ObjectMap;
			XmlQualifiedName contentBaseType = this.GetContentBaseType(content.Content);
			TypeData typeData = null;
			if (!this.IsPrimitiveTypeNamespace(contentBaseType.Namespace))
			{
				XmlTypeMapping xmlTypeMapping = this.ImportType(contentBaseType, null, true);
				this.BuildPendingMap(xmlTypeMapping);
				if (xmlTypeMapping.IsSimpleType)
				{
					typeData = xmlTypeMapping.TypeData;
				}
				else
				{
					ClassMap classMap2 = (ClassMap)xmlTypeMapping.ObjectMap;
					foreach (object obj in classMap2.AllMembers)
					{
						XmlTypeMapMember member = (XmlTypeMapMember)obj;
						classMap.AddMember(member);
					}
					map.BaseMap = xmlTypeMapping;
					xmlTypeMapping.DerivedTypes.Add(map);
				}
			}
			else
			{
				typeData = this.FindBuiltInType(contentBaseType);
			}
			if (typeData != null)
			{
				XmlTypeMapMemberElement xmlTypeMapMemberElement = new XmlTypeMapMemberElement();
				xmlTypeMapMemberElement.Name = classIds.AddUnique("Value", xmlTypeMapMemberElement);
				xmlTypeMapMemberElement.TypeData = typeData;
				xmlTypeMapMemberElement.ElementInfo.Add(this.CreateTextElementInfo(typeQName.Namespace, xmlTypeMapMemberElement, xmlTypeMapMemberElement.TypeData));
				xmlTypeMapMemberElement.IsXmlTextCollector = true;
				classMap.AddMember(xmlTypeMapMemberElement);
			}
			if (xmlSchemaSimpleContentExtension != null)
			{
				this.ImportAttributes(typeQName, classMap, xmlSchemaSimpleContentExtension.Attributes, xmlSchemaSimpleContentExtension.AnyAttribute, classIds);
			}
		}

		private TypeData FindBuiltInType(XmlQualifiedName qname)
		{
			XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)this.schemas.Find(qname, typeof(XmlSchemaComplexType));
			if (xmlSchemaComplexType != null)
			{
				XmlSchemaSimpleContent xmlSchemaSimpleContent = xmlSchemaComplexType.ContentModel as XmlSchemaSimpleContent;
				if (xmlSchemaSimpleContent == null)
				{
					throw new InvalidOperationException("Invalid schema");
				}
				return this.FindBuiltInType(this.GetContentBaseType(xmlSchemaSimpleContent.Content));
			}
			else
			{
				XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)this.schemas.Find(qname, typeof(XmlSchemaSimpleType));
				if (xmlSchemaSimpleType != null)
				{
					return this.FindBuiltInType(qname, xmlSchemaSimpleType);
				}
				if (this.IsPrimitiveTypeNamespace(qname.Namespace))
				{
					return TypeTranslator.GetPrimitiveTypeData(qname.Name);
				}
				throw new InvalidOperationException("Definition of type '" + qname + "' not found");
			}
		}

		private TypeData FindBuiltInType(XmlQualifiedName qname, XmlSchemaSimpleType st)
		{
			if (this.CanBeEnum(st) && qname != null)
			{
				return this.ImportType(qname, null, true).TypeData;
			}
			if (st.Content is XmlSchemaSimpleTypeRestriction)
			{
				XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)st.Content;
				XmlQualifiedName contentBaseType = this.GetContentBaseType(xmlSchemaSimpleTypeRestriction);
				if (contentBaseType == XmlQualifiedName.Empty && xmlSchemaSimpleTypeRestriction.BaseType != null)
				{
					return this.FindBuiltInType(qname, xmlSchemaSimpleTypeRestriction.BaseType);
				}
				return this.FindBuiltInType(contentBaseType);
			}
			else
			{
				if (st.Content is XmlSchemaSimpleTypeList)
				{
					return this.FindBuiltInType(this.GetContentBaseType(st.Content)).ListTypeData;
				}
				if (st.Content is XmlSchemaSimpleTypeUnion)
				{
					return this.FindBuiltInType(new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"));
				}
				return null;
			}
		}

		private XmlQualifiedName GetContentBaseType(XmlSchemaObject ob)
		{
			if (ob is XmlSchemaSimpleContentExtension)
			{
				return ((XmlSchemaSimpleContentExtension)ob).BaseTypeName;
			}
			if (ob is XmlSchemaSimpleContentRestriction)
			{
				return ((XmlSchemaSimpleContentRestriction)ob).BaseTypeName;
			}
			if (ob is XmlSchemaSimpleTypeRestriction)
			{
				return ((XmlSchemaSimpleTypeRestriction)ob).BaseTypeName;
			}
			if (ob is XmlSchemaSimpleTypeList)
			{
				return ((XmlSchemaSimpleTypeList)ob).ItemTypeName;
			}
			return null;
		}

		private void ImportComplexContent(XmlQualifiedName typeQName, XmlTypeMapping map, XmlSchemaComplexContent content, CodeIdentifiers classIds, bool isMixed)
		{
			ClassMap classMap = (ClassMap)map.ObjectMap;
			XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = content.Content as XmlSchemaComplexContentExtension;
			XmlQualifiedName baseTypeName;
			if (xmlSchemaComplexContentExtension != null)
			{
				baseTypeName = xmlSchemaComplexContentExtension.BaseTypeName;
			}
			else
			{
				baseTypeName = ((XmlSchemaComplexContentRestriction)content.Content).BaseTypeName;
			}
			if (baseTypeName == typeQName)
			{
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"Cannot import schema for type '",
					typeQName.Name,
					"' from namespace '",
					typeQName.Namespace,
					"'. Redefine not supported"
				}));
			}
			XmlTypeMapping xmlTypeMapping = this.ImportClass(baseTypeName);
			this.BuildPendingMap(xmlTypeMapping);
			ClassMap classMap2 = (ClassMap)xmlTypeMapping.ObjectMap;
			foreach (object obj in classMap2.AllMembers)
			{
				XmlTypeMapMember member = (XmlTypeMapMember)obj;
				classMap.AddMember(member);
			}
			if (classMap2.XmlTextCollector != null)
			{
				isMixed = false;
			}
			else if (content.IsMixed)
			{
				isMixed = true;
			}
			map.BaseMap = xmlTypeMapping;
			xmlTypeMapping.DerivedTypes.Add(map);
			if (xmlSchemaComplexContentExtension != null)
			{
				this.ImportParticleComplexContent(typeQName, classMap, xmlSchemaComplexContentExtension.Particle, classIds, isMixed);
				this.ImportAttributes(typeQName, classMap, xmlSchemaComplexContentExtension.Attributes, xmlSchemaComplexContentExtension.AnyAttribute, classIds);
			}
			else if (isMixed)
			{
				this.ImportParticleComplexContent(typeQName, classMap, null, classIds, true);
			}
		}

		private void ImportExtensionTypes(XmlQualifiedName qname)
		{
			foreach (object obj in this.schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Items)
				{
					XmlSchemaComplexType xmlSchemaComplexType = xmlSchemaObject as XmlSchemaComplexType;
					if (xmlSchemaComplexType != null && xmlSchemaComplexType.ContentModel is XmlSchemaComplexContent)
					{
						XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = xmlSchemaComplexType.ContentModel.Content as XmlSchemaComplexContentExtension;
						XmlQualifiedName baseTypeName;
						if (xmlSchemaComplexContentExtension != null)
						{
							baseTypeName = xmlSchemaComplexContentExtension.BaseTypeName;
						}
						else
						{
							baseTypeName = ((XmlSchemaComplexContentRestriction)xmlSchemaComplexType.ContentModel.Content).BaseTypeName;
						}
						if (baseTypeName == qname)
						{
							this.ImportType(new XmlQualifiedName(xmlSchemaComplexType.Name, xmlSchema.TargetNamespace), xmlSchemaComplexType, null);
						}
					}
				}
			}
		}

		private XmlTypeMapping ImportClassSimpleType(XmlQualifiedName typeQName, XmlSchemaSimpleType stype, XmlQualifiedName root)
		{
			if (this.CanBeEnum(stype))
			{
				CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
				XmlTypeMapping xmlTypeMapping = this.CreateTypeMapping(typeQName, SchemaTypes.Enum, root);
				xmlTypeMapping.Documentation = this.GetDocumentation(stype);
				bool isFlags = false;
				if (stype.Content is XmlSchemaSimpleTypeList)
				{
					stype = ((XmlSchemaSimpleTypeList)stype.Content).ItemType;
					isFlags = true;
				}
				XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)stype.Content;
				codeIdentifiers.AddReserved(xmlTypeMapping.TypeData.TypeName);
				EnumMap.EnumMapMember[] array = new EnumMap.EnumMapMember[xmlSchemaSimpleTypeRestriction.Facets.Count];
				for (int i = 0; i < xmlSchemaSimpleTypeRestriction.Facets.Count; i++)
				{
					XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet = (XmlSchemaEnumerationFacet)xmlSchemaSimpleTypeRestriction.Facets[i];
					string enumName = codeIdentifiers.AddUnique(CodeIdentifier.MakeValid(xmlSchemaEnumerationFacet.Value), xmlSchemaEnumerationFacet);
					array[i] = new EnumMap.EnumMapMember(xmlSchemaEnumerationFacet.Value, enumName);
					array[i].Documentation = this.GetDocumentation(xmlSchemaEnumerationFacet);
				}
				xmlTypeMapping.ObjectMap = new EnumMap(array, isFlags);
				xmlTypeMapping.IsSimpleType = true;
				return xmlTypeMapping;
			}
			if (stype.Content is XmlSchemaSimpleTypeList)
			{
				XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = (XmlSchemaSimpleTypeList)stype.Content;
				TypeData typeData = this.FindBuiltInType(xmlSchemaSimpleTypeList.ItemTypeName, stype);
				ListMap listMap = new ListMap();
				listMap.ItemInfo = new XmlTypeMapElementInfoList();
				listMap.ItemInfo.Add(this.CreateElementInfo(typeQName.Namespace, null, "Item", typeData.ListItemTypeData, false, XmlSchemaForm.None));
				XmlTypeMapping xmlTypeMapping2 = this.CreateArrayTypeMapping(typeQName, typeData);
				xmlTypeMapping2.ObjectMap = listMap;
				xmlTypeMapping2.IsSimpleType = true;
				return xmlTypeMapping2;
			}
			TypeData typeData2 = this.FindBuiltInType(typeQName, stype);
			XmlTypeMapping typeMapping = this.GetTypeMapping(typeData2);
			typeMapping.IsSimpleType = true;
			return typeMapping;
		}

		private bool CanBeEnum(XmlSchemaSimpleType stype)
		{
			if (stype.Content is XmlSchemaSimpleTypeRestriction)
			{
				XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)stype.Content;
				if (xmlSchemaSimpleTypeRestriction.Facets.Count == 0)
				{
					return false;
				}
				foreach (object obj in xmlSchemaSimpleTypeRestriction.Facets)
				{
					if (!(obj is XmlSchemaEnumerationFacet))
					{
						return false;
					}
				}
				return true;
			}
			else
			{
				if (stype.Content is XmlSchemaSimpleTypeList)
				{
					XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = (XmlSchemaSimpleTypeList)stype.Content;
					return xmlSchemaSimpleTypeList.ItemType != null && this.CanBeEnum(xmlSchemaSimpleTypeList.ItemType);
				}
				return false;
			}
		}

		private bool CanBeArray(XmlQualifiedName typeQName, XmlSchemaComplexType stype)
		{
			if (!this.encodedFormat)
			{
				return stype.Attributes.Count <= 0 && stype.AnyAttribute == null && !stype.IsMixed && this.CanBeArray(typeQName, stype.Particle, false);
			}
			XmlSchemaComplexContent xmlSchemaComplexContent = stype.ContentModel as XmlSchemaComplexContent;
			if (xmlSchemaComplexContent == null)
			{
				return false;
			}
			XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = xmlSchemaComplexContent.Content as XmlSchemaComplexContentRestriction;
			return xmlSchemaComplexContentRestriction != null && xmlSchemaComplexContentRestriction.BaseTypeName == XmlSchemaImporter.arrayType;
		}

		private bool CanBeArray(XmlQualifiedName typeQName, XmlSchemaParticle particle, bool multiValue)
		{
			if (particle == null)
			{
				return false;
			}
			multiValue = (multiValue || particle.MaxOccurs > 1m);
			if (particle is XmlSchemaGroupRef)
			{
				return this.CanBeArray(typeQName, this.GetRefGroupParticle((XmlSchemaGroupRef)particle), multiValue);
			}
			if (particle is XmlSchemaElement)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)particle;
				if (!xmlSchemaElement.RefName.IsEmpty)
				{
					return this.CanBeArray(typeQName, this.FindRefElement(xmlSchemaElement), multiValue);
				}
				return multiValue && !typeQName.Equals(((XmlSchemaElement)particle).SchemaTypeName);
			}
			else
			{
				if (particle is XmlSchemaAny)
				{
					return multiValue;
				}
				if (particle is XmlSchemaSequence)
				{
					XmlSchemaSequence xmlSchemaSequence = particle as XmlSchemaSequence;
					return xmlSchemaSequence.Items.Count == 1 && this.CanBeArray(typeQName, (XmlSchemaParticle)xmlSchemaSequence.Items[0], multiValue);
				}
				if (particle is XmlSchemaChoice)
				{
					ArrayList types = new ArrayList();
					return this.CheckChoiceType(typeQName, particle, types, ref multiValue) && multiValue;
				}
				return false;
			}
		}

		private bool CheckChoiceType(XmlQualifiedName typeQName, XmlSchemaParticle particle, ArrayList types, ref bool multiValue)
		{
			XmlQualifiedName xmlQualifiedName = null;
			multiValue = (multiValue || particle.MaxOccurs > 1m);
			if (particle is XmlSchemaGroupRef)
			{
				return this.CheckChoiceType(typeQName, this.GetRefGroupParticle((XmlSchemaGroupRef)particle), types, ref multiValue);
			}
			if (particle is XmlSchemaElement)
			{
				XmlSchemaElement elem = (XmlSchemaElement)particle;
				string text;
				XmlSchemaElement refElement = this.GetRefElement(typeQName, elem, out text);
				if (refElement.SchemaType != null)
				{
					return true;
				}
				xmlQualifiedName = refElement.SchemaTypeName;
			}
			else
			{
				if (!(particle is XmlSchemaAny))
				{
					if (particle is XmlSchemaSequence)
					{
						XmlSchemaSequence xmlSchemaSequence = particle as XmlSchemaSequence;
						foreach (XmlSchemaObject xmlSchemaObject in xmlSchemaSequence.Items)
						{
							XmlSchemaParticle particle2 = (XmlSchemaParticle)xmlSchemaObject;
							if (!this.CheckChoiceType(typeQName, particle2, types, ref multiValue))
							{
								return false;
							}
						}
						return true;
					}
					if (particle is XmlSchemaChoice)
					{
						foreach (XmlSchemaObject xmlSchemaObject2 in ((XmlSchemaChoice)particle).Items)
						{
							XmlSchemaParticle particle3 = (XmlSchemaParticle)xmlSchemaObject2;
							if (!this.CheckChoiceType(typeQName, particle3, types, ref multiValue))
							{
								return false;
							}
						}
						return true;
					}
					goto IL_177;
				}
				xmlQualifiedName = XmlSchemaImporter.anyType;
			}
			IL_177:
			if (typeQName.Equals(xmlQualifiedName))
			{
				return false;
			}
			string text2;
			if (this.IsPrimitiveTypeNamespace(xmlQualifiedName.Namespace))
			{
				text2 = TypeTranslator.GetPrimitiveTypeData(xmlQualifiedName.Name).FullTypeName + ":" + xmlQualifiedName.Namespace;
			}
			else
			{
				text2 = xmlQualifiedName.Name + ":" + xmlQualifiedName.Namespace;
			}
			if (types.Contains(text2))
			{
				return false;
			}
			types.Add(text2);
			return true;
		}

		private bool CanBeAnyElement(XmlSchemaComplexType stype)
		{
			XmlSchemaSequence xmlSchemaSequence = stype.Particle as XmlSchemaSequence;
			return xmlSchemaSequence != null && xmlSchemaSequence.Items.Count == 1 && xmlSchemaSequence.Items[0] is XmlSchemaAny;
		}

		private Type GetAnyElementType(XmlSchemaComplexType stype)
		{
			XmlSchemaSequence xmlSchemaSequence = stype.Particle as XmlSchemaSequence;
			if (xmlSchemaSequence == null || xmlSchemaSequence.Items.Count != 1 || !(xmlSchemaSequence.Items[0] is XmlSchemaAny))
			{
				return null;
			}
			if (this.encodedFormat)
			{
				return typeof(object);
			}
			XmlSchemaAny xmlSchemaAny = xmlSchemaSequence.Items[0] as XmlSchemaAny;
			if (xmlSchemaAny.MaxOccurs == 1m)
			{
				if (stype.IsMixed)
				{
					return typeof(XmlNode);
				}
				return typeof(XmlElement);
			}
			else
			{
				if (stype.IsMixed)
				{
					return typeof(XmlNode[]);
				}
				return typeof(XmlElement[]);
			}
		}

		private bool CanBeIXmlSerializable(XmlSchemaComplexType stype)
		{
			XmlSchemaSequence xmlSchemaSequence = stype.Particle as XmlSchemaSequence;
			if (xmlSchemaSequence == null)
			{
				return false;
			}
			if (xmlSchemaSequence.Items.Count != 2)
			{
				return false;
			}
			XmlSchemaElement xmlSchemaElement = xmlSchemaSequence.Items[0] as XmlSchemaElement;
			return xmlSchemaElement != null && !(xmlSchemaElement.RefName != new XmlQualifiedName("schema", "http://www.w3.org/2001/XMLSchema")) && xmlSchemaSequence.Items[1] is XmlSchemaAny;
		}

		private XmlTypeMapping ImportXmlSerializableMapping(string ns)
		{
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName("System.Data.DataSet", ns);
			XmlTypeMapping xmlTypeMapping = this.GetRegisteredTypeMapping(xmlQualifiedName);
			if (xmlTypeMapping != null)
			{
				return xmlTypeMapping;
			}
			TypeData typeData = new TypeData("System.Data.DataSet", "System.Data.DataSet", "System.Data.DataSet", SchemaTypes.XmlSerializable, null);
			xmlTypeMapping = new XmlTypeMapping("System.Data.DataSet", string.Empty, typeData, "System.Data.DataSet", ns);
			xmlTypeMapping.IncludeInSchema = true;
			this.RegisterTypeMapping(xmlQualifiedName, typeData, xmlTypeMapping);
			return xmlTypeMapping;
		}

		private XmlTypeMapElementInfo CreateElementInfo(string ns, XmlTypeMapMember member, string name, TypeData typeData, bool isNillable, XmlSchemaForm form)
		{
			if (typeData.IsComplexType)
			{
				return this.CreateElementInfo(ns, member, name, typeData, isNillable, form, this.GetTypeMapping(typeData));
			}
			return this.CreateElementInfo(ns, member, name, typeData, isNillable, form, null);
		}

		private XmlTypeMapElementInfo CreateElementInfo(string ns, XmlTypeMapMember member, string name, TypeData typeData, bool isNillable, XmlSchemaForm form, XmlTypeMapping emap)
		{
			XmlTypeMapElementInfo xmlTypeMapElementInfo = new XmlTypeMapElementInfo(member, typeData);
			xmlTypeMapElementInfo.ElementName = name;
			xmlTypeMapElementInfo.Namespace = ns;
			xmlTypeMapElementInfo.IsNullable = isNillable;
			xmlTypeMapElementInfo.Form = form;
			if (typeData.IsComplexType)
			{
				xmlTypeMapElementInfo.MappedType = emap;
			}
			return xmlTypeMapElementInfo;
		}

		private XmlTypeMapElementInfo CreateTextElementInfo(string ns, XmlTypeMapMember member, TypeData typeData)
		{
			XmlTypeMapElementInfo xmlTypeMapElementInfo = new XmlTypeMapElementInfo(member, typeData);
			xmlTypeMapElementInfo.IsTextElement = true;
			xmlTypeMapElementInfo.WrappedElement = false;
			if (typeData.IsComplexType)
			{
				xmlTypeMapElementInfo.MappedType = this.GetTypeMapping(typeData);
			}
			return xmlTypeMapElementInfo;
		}

		private XmlTypeMapping CreateTypeMapping(XmlQualifiedName typeQName, SchemaTypes schemaType, XmlQualifiedName root)
		{
			string text = CodeIdentifier.MakeValid(typeQName.Name);
			text = this.typeIdentifiers.AddUnique(text, null);
			TypeData typeData = new TypeData(text, text, text, schemaType, null);
			string name;
			string ns;
			if (root != null)
			{
				name = root.Name;
				ns = root.Namespace;
			}
			else
			{
				name = typeQName.Name;
				ns = string.Empty;
			}
			XmlTypeMapping xmlTypeMapping = new XmlTypeMapping(name, ns, typeData, typeQName.Name, typeQName.Namespace);
			xmlTypeMapping.IncludeInSchema = true;
			this.RegisterTypeMapping(typeQName, typeData, xmlTypeMapping);
			return xmlTypeMapping;
		}

		private XmlTypeMapping CreateArrayTypeMapping(XmlQualifiedName typeQName, TypeData arrayTypeData)
		{
			XmlTypeMapping xmlTypeMapping;
			if (this.encodedFormat)
			{
				xmlTypeMapping = new XmlTypeMapping("Array", "http://schemas.xmlsoap.org/soap/encoding/", arrayTypeData, "Array", "http://schemas.xmlsoap.org/soap/encoding/");
			}
			else
			{
				xmlTypeMapping = new XmlTypeMapping(arrayTypeData.XmlType, typeQName.Namespace, arrayTypeData, arrayTypeData.XmlType, typeQName.Namespace);
			}
			xmlTypeMapping.IncludeInSchema = true;
			this.RegisterTypeMapping(typeQName, arrayTypeData, xmlTypeMapping);
			return xmlTypeMapping;
		}

		private XmlSchemaElement GetRefElement(XmlQualifiedName typeQName, XmlSchemaElement elem, out string ns)
		{
			if (!elem.RefName.IsEmpty)
			{
				ns = elem.RefName.Namespace;
				return this.FindRefElement(elem);
			}
			ns = typeQName.Namespace;
			return elem;
		}

		private XmlSchemaAttribute GetRefAttribute(XmlQualifiedName typeQName, XmlSchemaAttribute attr, out string ns)
		{
			if (attr.RefName.IsEmpty)
			{
				ns = ((!attr.ParentIsSchema) ? string.Empty : typeQName.Namespace);
				return attr;
			}
			ns = attr.RefName.Namespace;
			XmlSchemaAttribute xmlSchemaAttribute = this.FindRefAttribute(attr.RefName);
			if (xmlSchemaAttribute == null)
			{
				throw new InvalidOperationException("The attribute " + attr.RefName + " is missing");
			}
			return xmlSchemaAttribute;
		}

		private TypeData GetElementTypeData(XmlQualifiedName typeQName, XmlSchemaElement elem, XmlQualifiedName root, out XmlTypeMapping map)
		{
			bool sharedAnnType = false;
			map = null;
			if (!elem.RefName.IsEmpty)
			{
				XmlSchemaElement xmlSchemaElement = this.FindRefElement(elem);
				if (xmlSchemaElement == null)
				{
					throw new InvalidOperationException("Global element not found: " + elem.RefName);
				}
				root = elem.RefName;
				elem = xmlSchemaElement;
				sharedAnnType = true;
			}
			TypeData typeData;
			if (!elem.SchemaTypeName.IsEmpty)
			{
				typeData = this.GetTypeData(elem.SchemaTypeName, root, elem.IsNillable);
				map = this.GetRegisteredTypeMapping(typeData);
			}
			else if (elem.SchemaType == null)
			{
				typeData = TypeTranslator.GetTypeData(typeof(object));
			}
			else
			{
				typeData = this.GetTypeData(elem.SchemaType, typeQName, elem.Name, sharedAnnType, root);
			}
			if (map == null && typeData.IsComplexType)
			{
				map = this.GetTypeMapping(typeData);
			}
			return typeData;
		}

		private TypeData GetAttributeTypeData(XmlQualifiedName typeQName, XmlSchemaAttribute attr)
		{
			bool sharedAnnType = false;
			if (!attr.RefName.IsEmpty)
			{
				XmlSchemaAttribute xmlSchemaAttribute = this.FindRefAttribute(attr.RefName);
				if (xmlSchemaAttribute == null)
				{
					throw new InvalidOperationException("Global attribute not found: " + attr.RefName);
				}
				attr = xmlSchemaAttribute;
				sharedAnnType = true;
			}
			if (!attr.SchemaTypeName.IsEmpty)
			{
				return this.GetTypeData(attr.SchemaTypeName, null, false);
			}
			if (attr.SchemaType == null)
			{
				return TypeTranslator.GetTypeData(typeof(string));
			}
			return this.GetTypeData(attr.SchemaType, typeQName, attr.Name, sharedAnnType, null);
		}

		private TypeData GetTypeData(XmlQualifiedName typeQName, XmlQualifiedName root, bool isNullable)
		{
			if (this.IsPrimitiveTypeNamespace(typeQName.Namespace))
			{
				XmlTypeMapping xmlTypeMapping = this.ImportType(typeQName, root, false);
				if (xmlTypeMapping != null)
				{
					return xmlTypeMapping.TypeData;
				}
				return TypeTranslator.GetPrimitiveTypeData(typeQName.Name, isNullable);
			}
			else
			{
				if (this.encodedFormat && typeQName.Namespace == string.Empty)
				{
					return TypeTranslator.GetPrimitiveTypeData(typeQName.Name);
				}
				return this.ImportType(typeQName, root, true).TypeData;
			}
		}

		private TypeData GetTypeData(XmlSchemaType stype, XmlQualifiedName typeQNname, string propertyName, bool sharedAnnType, XmlQualifiedName root)
		{
			string text;
			if (sharedAnnType)
			{
				TypeData typeData = this.sharedAnonymousTypes[stype] as TypeData;
				if (typeData != null)
				{
					return typeData;
				}
				text = propertyName;
			}
			else
			{
				text = typeQNname.Name + this.typeIdentifiers.MakeRightCase(propertyName);
			}
			text = this.elemIdentifiers.AddUnique(text, stype);
			XmlQualifiedName name = new XmlQualifiedName(text, typeQNname.Namespace);
			XmlTypeMapping xmlTypeMapping = this.ImportType(name, stype, root);
			if (sharedAnnType)
			{
				this.sharedAnonymousTypes[stype] = xmlTypeMapping.TypeData;
			}
			return xmlTypeMapping.TypeData;
		}

		private XmlTypeMapping GetTypeMapping(TypeData typeData)
		{
			if (typeData.Type == typeof(object) && !this.anyTypeImported)
			{
				this.ImportAllObjectTypes();
			}
			XmlTypeMapping xmlTypeMapping = this.GetRegisteredTypeMapping(typeData);
			if (xmlTypeMapping != null)
			{
				return xmlTypeMapping;
			}
			if (typeData.IsListType)
			{
				XmlTypeMapping typeMapping = this.GetTypeMapping(typeData.ListItemTypeData);
				xmlTypeMapping = new XmlTypeMapping(typeData.XmlType, typeMapping.Namespace, typeData, typeData.XmlType, typeMapping.Namespace);
				xmlTypeMapping.IncludeInSchema = true;
				xmlTypeMapping.ObjectMap = new ListMap
				{
					ItemInfo = new XmlTypeMapElementInfoList(),
					ItemInfo = 
					{
						this.CreateElementInfo(typeMapping.Namespace, null, typeData.ListItemTypeData.XmlType, typeData.ListItemTypeData, false, XmlSchemaForm.None)
					}
				};
				this.RegisterTypeMapping(new XmlQualifiedName(xmlTypeMapping.ElementName, xmlTypeMapping.Namespace), typeData, xmlTypeMapping);
				return xmlTypeMapping;
			}
			if (typeData.SchemaType == SchemaTypes.Primitive || typeData.Type == typeof(object) || typeof(XmlNode).IsAssignableFrom(typeData.Type))
			{
				return this.CreateSystemMap(typeData);
			}
			throw new InvalidOperationException("Map for type " + typeData.TypeName + " not found");
		}

		private void AddObjectDerivedMap(XmlTypeMapping map)
		{
			TypeData typeData = TypeTranslator.GetTypeData(typeof(object));
			XmlTypeMapping xmlTypeMapping = this.GetRegisteredTypeMapping(typeData);
			if (xmlTypeMapping == null)
			{
				xmlTypeMapping = this.CreateSystemMap(typeData);
			}
			xmlTypeMapping.DerivedTypes.Add(map);
		}

		private XmlTypeMapping CreateSystemMap(TypeData typeData)
		{
			XmlTypeMapping xmlTypeMapping = new XmlTypeMapping(typeData.XmlType, "http://www.w3.org/2001/XMLSchema", typeData, typeData.XmlType, "http://www.w3.org/2001/XMLSchema");
			xmlTypeMapping.IncludeInSchema = false;
			xmlTypeMapping.ObjectMap = new ClassMap();
			this.dataMappedTypes[typeData] = xmlTypeMapping;
			return xmlTypeMapping;
		}

		private void ImportAllObjectTypes()
		{
			this.anyTypeImported = true;
			foreach (object obj in this.schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Items)
				{
					XmlSchemaComplexType xmlSchemaComplexType = xmlSchemaObject as XmlSchemaComplexType;
					if (xmlSchemaComplexType != null)
					{
						this.ImportType(new XmlQualifiedName(xmlSchemaComplexType.Name, xmlSchema.TargetNamespace), xmlSchemaComplexType, null);
					}
				}
			}
		}

		private XmlTypeMapping GetRegisteredTypeMapping(XmlQualifiedName typeQName, Type baseType)
		{
			if (this.IsPrimitiveTypeNamespace(typeQName.Namespace))
			{
				return (XmlTypeMapping)this.primitiveDerivedMappedTypes[typeQName];
			}
			return (XmlTypeMapping)this.mappedTypes[typeQName];
		}

		private XmlTypeMapping GetRegisteredTypeMapping(XmlQualifiedName typeQName)
		{
			return (XmlTypeMapping)this.mappedTypes[typeQName];
		}

		private XmlTypeMapping GetRegisteredTypeMapping(TypeData typeData)
		{
			return (XmlTypeMapping)this.dataMappedTypes[typeData];
		}

		private void RegisterTypeMapping(XmlQualifiedName qname, TypeData typeData, XmlTypeMapping map)
		{
			this.dataMappedTypes[typeData] = map;
			if (this.IsPrimitiveTypeNamespace(qname.Namespace) && !map.IsSimpleType)
			{
				this.primitiveDerivedMappedTypes[qname] = map;
			}
			else
			{
				this.mappedTypes[qname] = map;
			}
		}

		private XmlSchemaParticle GetRefGroupParticle(XmlSchemaGroupRef refGroup)
		{
			XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)this.schemas.Find(refGroup.RefName, typeof(XmlSchemaGroup));
			return xmlSchemaGroup.Particle;
		}

		private XmlSchemaElement FindRefElement(XmlSchemaElement elem)
		{
			XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.schemas.Find(elem.RefName, typeof(XmlSchemaElement));
			if (xmlSchemaElement != null)
			{
				return xmlSchemaElement;
			}
			if (!this.IsPrimitiveTypeNamespace(elem.RefName.Namespace))
			{
				return null;
			}
			if (this.anyElement != null)
			{
				return this.anyElement;
			}
			this.anyElement = new XmlSchemaElement();
			this.anyElement.Name = "any";
			this.anyElement.SchemaTypeName = XmlSchemaImporter.anyType;
			return this.anyElement;
		}

		private XmlSchemaAttribute FindRefAttribute(XmlQualifiedName refName)
		{
			if (refName.Namespace == "http://www.w3.org/XML/1998/namespace")
			{
				return new XmlSchemaAttribute
				{
					Name = refName.Name,
					SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
				};
			}
			return (XmlSchemaAttribute)this.schemas.Find(refName, typeof(XmlSchemaAttribute));
		}

		private XmlSchemaAttributeGroup FindRefAttributeGroup(XmlQualifiedName refName)
		{
			XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)this.schemas.Find(refName, typeof(XmlSchemaAttributeGroup));
			foreach (XmlSchemaObject xmlSchemaObject in xmlSchemaAttributeGroup.Attributes)
			{
				if (xmlSchemaObject is XmlSchemaAttributeGroupRef && ((XmlSchemaAttributeGroupRef)xmlSchemaObject).RefName == refName)
				{
					throw new InvalidOperationException(string.Concat(new string[]
					{
						"Cannot import attribute group '",
						refName.Name,
						"' from namespace '",
						refName.Namespace,
						"'. Redefine not supported"
					}));
				}
			}
			return xmlSchemaAttributeGroup;
		}

		private XmlTypeMapping ReflectType(Type type)
		{
			TypeData typeData = TypeTranslator.GetTypeData(type);
			return this.ReflectType(typeData, null);
		}

		private XmlTypeMapping ReflectType(TypeData typeData, string ns)
		{
			if (!this.encodedFormat)
			{
				if (this.auxXmlRefImporter == null)
				{
					this.auxXmlRefImporter = new XmlReflectionImporter();
				}
				return this.auxXmlRefImporter.ImportTypeMapping(typeData, ns);
			}
			if (this.auxSoapRefImporter == null)
			{
				this.auxSoapRefImporter = new SoapReflectionImporter();
			}
			return this.auxSoapRefImporter.ImportTypeMapping(typeData, ns);
		}

		private string GetDocumentation(XmlSchemaAnnotated elem)
		{
			string text = string.Empty;
			XmlSchemaAnnotation annotation = elem.Annotation;
			if (annotation == null || annotation.Items == null)
			{
				return null;
			}
			foreach (object obj in annotation.Items)
			{
				XmlSchemaDocumentation xmlSchemaDocumentation = obj as XmlSchemaDocumentation;
				if (xmlSchemaDocumentation != null && xmlSchemaDocumentation.Markup != null && xmlSchemaDocumentation.Markup.Length > 0)
				{
					if (text != string.Empty)
					{
						text += "\n";
					}
					foreach (XmlNode xmlNode in xmlSchemaDocumentation.Markup)
					{
						text += xmlNode.Value;
					}
				}
			}
			return text;
		}

		private bool IsPrimitiveTypeNamespace(string ns)
		{
			return ns == "http://www.w3.org/2001/XMLSchema" || (this.encodedFormat && ns == "http://schemas.xmlsoap.org/soap/encoding/");
		}

		private class MapFixup
		{
			public XmlTypeMapping Map;

			public XmlSchemaComplexType SchemaType;

			public XmlQualifiedName TypeName;
		}
	}
}
