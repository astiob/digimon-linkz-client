using System;
using System.Collections;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	/// <summary>Populates <see cref="T:System.Xml.Schema.XmlSchema" /> objects with XML schema element declarations that are found in type mapping objects. </summary>
	public class XmlSchemaExporter
	{
		private XmlSchemas schemas;

		private Hashtable exportedMaps = new Hashtable();

		private Hashtable exportedElements = new Hashtable();

		private bool encodedFormat;

		private XmlDocument xmlDoc;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSchemaExporter" /> class. </summary>
		/// <param name="schemas">A collection of <see cref="T:System.Xml.Schema.XmlSchema" /> objects to which element declarations obtained from type mappings are added.</param>
		public XmlSchemaExporter(XmlSchemas schemas)
		{
			this.schemas = schemas;
		}

		internal XmlSchemaExporter(XmlSchemas schemas, bool encodedFormat)
		{
			this.encodedFormat = encodedFormat;
			this.schemas = schemas;
		}

		/// <summary>Exports an &lt;any&gt; element to the <see cref="T:System.Xml.Schema.XmlSchema" /> object that is identified by the specified namespace.</summary>
		/// <returns>An arbitrary name assigned to the &lt;any&gt; element declaration.</returns>
		/// <param name="ns">The namespace of the XML schema document to which to add an &lt;any&gt; element.</param>
		[MonoTODO]
		public string ExportAnyType(string ns)
		{
			throw new NotImplementedException();
		}

		/// <summary>Adds an element declaration for an object or type to a SOAP message or to an <see cref="T:System.Xml.Schema.XmlSchema" /> object.</summary>
		/// <returns>The string "any" with an appended integer. </returns>
		/// <param name="members">An <see cref="T:System.Xml.Serialization.XmlMembersMapping" />  that contains mappings to export.</param>
		[MonoNotSupported("")]
		public string ExportAnyType(XmlMembersMapping members)
		{
			throw new NotImplementedException();
		}

		/// <summary>Adds an element declaration to the applicable <see cref="T:System.Xml.Schema.XmlSchema" /> for each of the element parts of a literal SOAP message definition. </summary>
		/// <param name="xmlMembersMapping">The internal .NET Framework type mappings for the element parts of a Web Services Description Language (WSDL) message definition.</param>
		public void ExportMembersMapping(XmlMembersMapping xmlMembersMapping)
		{
			this.ExportMembersMapping(xmlMembersMapping, true);
		}

		/// <summary>Adds an element declaration to the applicable <see cref="T:System.Xml.Schema.XmlSchema" /> for each of the element parts of a literal SOAP message definition, and specifies whether enclosing elements are included.</summary>
		/// <param name="xmlMembersMapping">The internal mapping between a .NET Framework type and an XML schema element.</param>
		/// <param name="exportEnclosingType">true if the schema elements that enclose the schema are to be included; otherwise, false.</param>
		public void ExportMembersMapping(XmlMembersMapping xmlMembersMapping, bool exportEnclosingType)
		{
			ClassMap classMap = (ClassMap)xmlMembersMapping.ObjectMap;
			if (xmlMembersMapping.HasWrapperElement && exportEnclosingType)
			{
				XmlSchema schema = this.GetSchema(xmlMembersMapping.Namespace);
				XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
				XmlSchemaSequence particle;
				XmlSchemaAnyAttribute anyAttribute;
				this.ExportMembersMapSchema(schema, classMap, null, xmlSchemaComplexType.Attributes, out particle, out anyAttribute);
				xmlSchemaComplexType.Particle = particle;
				xmlSchemaComplexType.AnyAttribute = anyAttribute;
				if (this.encodedFormat)
				{
					xmlSchemaComplexType.Name = xmlMembersMapping.ElementName;
					schema.Items.Add(xmlSchemaComplexType);
				}
				else
				{
					XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
					xmlSchemaElement.Name = xmlMembersMapping.ElementName;
					xmlSchemaElement.SchemaType = xmlSchemaComplexType;
					schema.Items.Add(xmlSchemaElement);
				}
			}
			else
			{
				ICollection elementMembers = classMap.ElementMembers;
				if (elementMembers != null)
				{
					foreach (object obj in elementMembers)
					{
						XmlTypeMapMemberElement xmlTypeMapMemberElement = (XmlTypeMapMemberElement)obj;
						if (xmlTypeMapMemberElement is XmlTypeMapMemberAnyElement && xmlTypeMapMemberElement.TypeData.IsListType)
						{
							XmlSchema schema2 = this.GetSchema(xmlMembersMapping.Namespace);
							XmlSchemaParticle schemaArrayElement = this.GetSchemaArrayElement(schema2, xmlTypeMapMemberElement.ElementInfo);
							if (schemaArrayElement is XmlSchemaAny)
							{
								XmlSchemaComplexType xmlSchemaComplexType2 = this.FindComplexType(schema2.Items, "any");
								if (xmlSchemaComplexType2 != null)
								{
									continue;
								}
								xmlSchemaComplexType2 = new XmlSchemaComplexType();
								xmlSchemaComplexType2.Name = "any";
								xmlSchemaComplexType2.IsMixed = true;
								XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
								xmlSchemaComplexType2.Particle = xmlSchemaSequence;
								xmlSchemaSequence.Items.Add(schemaArrayElement);
								schema2.Items.Add(xmlSchemaComplexType2);
								continue;
							}
						}
						XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)xmlTypeMapMemberElement.ElementInfo[0];
						XmlSchema schema3;
						if (this.encodedFormat)
						{
							schema3 = this.GetSchema(xmlMembersMapping.Namespace);
							this.ImportNamespace(schema3, "http://schemas.xmlsoap.org/soap/encoding/");
						}
						else
						{
							schema3 = this.GetSchema(xmlTypeMapElementInfo.Namespace);
						}
						XmlSchemaElement xmlSchemaElement2 = this.FindElement(schema3.Items, xmlTypeMapElementInfo.ElementName);
						XmlSchemaExporter.XmlSchemaObjectContainer container = null;
						if (!this.encodedFormat)
						{
							container = new XmlSchemaExporter.XmlSchemaObjectContainer(schema3);
						}
						Type type = xmlTypeMapMemberElement.GetType();
						if (xmlTypeMapMemberElement is XmlTypeMapMemberFlatList)
						{
							throw new InvalidOperationException("Unwrapped arrays not supported as parameters");
						}
						XmlSchemaElement xmlSchemaElement3;
						if (type == typeof(XmlTypeMapMemberElement))
						{
							xmlSchemaElement3 = (XmlSchemaElement)this.GetSchemaElement(schema3, xmlTypeMapElementInfo, xmlTypeMapMemberElement.DefaultValue, false, container);
						}
						else
						{
							xmlSchemaElement3 = (XmlSchemaElement)this.GetSchemaElement(schema3, xmlTypeMapElementInfo, false, container);
						}
						if (xmlSchemaElement2 != null)
						{
							if (!xmlSchemaElement2.SchemaTypeName.Equals(xmlSchemaElement3.SchemaTypeName))
							{
								string text = "The XML element named '" + xmlTypeMapElementInfo.ElementName + "' ";
								string text2 = text;
								text = string.Concat(new string[]
								{
									text2,
									"from namespace '",
									schema3.TargetNamespace,
									"' references distinct types ",
									xmlSchemaElement3.SchemaTypeName.Name,
									" and ",
									xmlSchemaElement2.SchemaTypeName.Name,
									". "
								});
								text += "Use XML attributes to specify another XML name or namespace for the element or types.";
								throw new InvalidOperationException(text);
							}
							schema3.Items.Remove(xmlSchemaElement3);
						}
					}
				}
			}
			this.CompileSchemas();
		}

		/// <summary>Adds an element declaration to the applicable <see cref="T:System.Xml.Schema.XmlSchema" /> object for a single element part of a literal SOAP message definition.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlQualifiedName" /> that represents the qualified XML name of the exported element declaration.</returns>
		/// <param name="xmlMembersMapping">Internal .NET Framework type mappings for the element parts of a Web Services Description Language (WSDL) message definition.</param>
		[MonoTODO]
		public XmlQualifiedName ExportTypeMapping(XmlMembersMapping xmlMembersMapping)
		{
			throw new NotImplementedException();
		}

		/// <summary>Adds an element declaration for a .NET Framework type to the applicable <see cref="T:System.Xml.Schema.XmlSchema" /> object. </summary>
		/// <param name="xmlTypeMapping">The internal mapping between a .NET Framework type and an XML schema element.</param>
		public void ExportTypeMapping(XmlTypeMapping xmlTypeMapping)
		{
			if (!xmlTypeMapping.IncludeInSchema)
			{
				return;
			}
			if (this.IsElementExported(xmlTypeMapping))
			{
				return;
			}
			if (this.encodedFormat)
			{
				this.ExportClassSchema(xmlTypeMapping);
				XmlSchema schema = this.GetSchema(xmlTypeMapping.XmlTypeNamespace);
				this.ImportNamespace(schema, "http://schemas.xmlsoap.org/soap/encoding/");
			}
			else
			{
				XmlSchema schema2 = this.GetSchema(xmlTypeMapping.Namespace);
				XmlTypeMapElementInfo xmlTypeMapElementInfo = new XmlTypeMapElementInfo(null, xmlTypeMapping.TypeData);
				xmlTypeMapElementInfo.Namespace = xmlTypeMapping.Namespace;
				xmlTypeMapElementInfo.ElementName = xmlTypeMapping.ElementName;
				if (xmlTypeMapping.TypeData.IsComplexType)
				{
					xmlTypeMapElementInfo.MappedType = xmlTypeMapping;
				}
				xmlTypeMapElementInfo.IsNullable = xmlTypeMapping.IsNullable;
				this.GetSchemaElement(schema2, xmlTypeMapElementInfo, false, new XmlSchemaExporter.XmlSchemaObjectContainer(schema2));
				this.SetElementExported(xmlTypeMapping);
			}
			this.CompileSchemas();
		}

		private void ExportXmlSerializableSchema(XmlSchema currentSchema, XmlSerializableMapping map)
		{
			if (this.IsMapExported(map))
			{
				return;
			}
			this.SetMapExported(map);
			if (map.Schema == null)
			{
				return;
			}
			string targetNamespace = map.Schema.TargetNamespace;
			XmlSchema xmlSchema = this.schemas[targetNamespace];
			if (xmlSchema == null)
			{
				this.schemas.Add(map.Schema);
				this.ImportNamespace(currentSchema, targetNamespace);
			}
			else if (xmlSchema != map.Schema && !XmlSchemaExporter.CanBeDuplicated(xmlSchema, map.Schema))
			{
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"The namespace '",
					targetNamespace,
					"' defined by the class '",
					map.TypeFullName,
					"' is a duplicate."
				}));
			}
		}

		private static bool CanBeDuplicated(XmlSchema existingSchema, XmlSchema schema)
		{
			return XmlSchemas.IsDataSet(existingSchema) && XmlSchemas.IsDataSet(schema) && existingSchema.Id == schema.Id;
		}

		private void ExportClassSchema(XmlTypeMapping map)
		{
			if (this.IsMapExported(map))
			{
				return;
			}
			this.SetMapExported(map);
			if (map.TypeData.Type == typeof(object))
			{
				foreach (object obj in map.DerivedTypes)
				{
					XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)obj;
					if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Class)
					{
						this.ExportClassSchema(xmlTypeMapping);
					}
				}
				return;
			}
			XmlSchema schema = this.GetSchema(map.XmlTypeNamespace);
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			xmlSchemaComplexType.Name = map.XmlType;
			schema.Items.Add(xmlSchemaComplexType);
			ClassMap classMap = (ClassMap)map.ObjectMap;
			if (classMap.HasSimpleContent)
			{
				XmlSchemaSimpleContent xmlSchemaSimpleContent = new XmlSchemaSimpleContent();
				xmlSchemaComplexType.ContentModel = xmlSchemaSimpleContent;
				XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = new XmlSchemaSimpleContentExtension();
				xmlSchemaSimpleContent.Content = xmlSchemaSimpleContentExtension;
				XmlSchemaSequence xmlSchemaSequence;
				XmlSchemaAnyAttribute anyAttribute;
				this.ExportMembersMapSchema(schema, classMap, map.BaseMap, xmlSchemaSimpleContentExtension.Attributes, out xmlSchemaSequence, out anyAttribute);
				xmlSchemaSimpleContentExtension.AnyAttribute = anyAttribute;
				if (map.BaseMap == null)
				{
					xmlSchemaSimpleContentExtension.BaseTypeName = classMap.SimpleContentBaseType;
				}
				else
				{
					xmlSchemaSimpleContentExtension.BaseTypeName = new XmlQualifiedName(map.BaseMap.XmlType, map.BaseMap.XmlTypeNamespace);
					this.ImportNamespace(schema, map.BaseMap.XmlTypeNamespace);
					this.ExportClassSchema(map.BaseMap);
				}
			}
			else if (map.BaseMap != null && map.BaseMap.IncludeInSchema)
			{
				XmlSchemaComplexContent xmlSchemaComplexContent = new XmlSchemaComplexContent();
				XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = new XmlSchemaComplexContentExtension();
				xmlSchemaComplexContentExtension.BaseTypeName = new XmlQualifiedName(map.BaseMap.XmlType, map.BaseMap.XmlTypeNamespace);
				xmlSchemaComplexContent.Content = xmlSchemaComplexContentExtension;
				xmlSchemaComplexType.ContentModel = xmlSchemaComplexContent;
				XmlSchemaSequence particle;
				XmlSchemaAnyAttribute anyAttribute2;
				this.ExportMembersMapSchema(schema, classMap, map.BaseMap, xmlSchemaComplexContentExtension.Attributes, out particle, out anyAttribute2);
				xmlSchemaComplexContentExtension.Particle = particle;
				xmlSchemaComplexContentExtension.AnyAttribute = anyAttribute2;
				xmlSchemaComplexType.IsMixed = this.HasMixedContent(map);
				xmlSchemaComplexContent.IsMixed = this.BaseHasMixedContent(map);
				this.ImportNamespace(schema, map.BaseMap.XmlTypeNamespace);
				this.ExportClassSchema(map.BaseMap);
			}
			else
			{
				XmlSchemaSequence particle2;
				XmlSchemaAnyAttribute anyAttribute3;
				this.ExportMembersMapSchema(schema, classMap, map.BaseMap, xmlSchemaComplexType.Attributes, out particle2, out anyAttribute3);
				xmlSchemaComplexType.Particle = particle2;
				xmlSchemaComplexType.AnyAttribute = anyAttribute3;
				xmlSchemaComplexType.IsMixed = (classMap.XmlTextCollector != null);
			}
			foreach (object obj2 in map.DerivedTypes)
			{
				XmlTypeMapping xmlTypeMapping2 = (XmlTypeMapping)obj2;
				if (xmlTypeMapping2.TypeData.SchemaType == SchemaTypes.Class)
				{
					this.ExportClassSchema(xmlTypeMapping2);
				}
			}
		}

		private bool BaseHasMixedContent(XmlTypeMapping map)
		{
			ClassMap classMap = (ClassMap)map.ObjectMap;
			return classMap.XmlTextCollector != null && map.BaseMap != null && this.DefinedInBaseMap(map.BaseMap, classMap.XmlTextCollector);
		}

		private bool HasMixedContent(XmlTypeMapping map)
		{
			ClassMap classMap = (ClassMap)map.ObjectMap;
			return classMap.XmlTextCollector != null && (map.BaseMap == null || !this.DefinedInBaseMap(map.BaseMap, classMap.XmlTextCollector));
		}

		private void ExportMembersMapSchema(XmlSchema schema, ClassMap map, XmlTypeMapping baseMap, XmlSchemaObjectCollection outAttributes, out XmlSchemaSequence particle, out XmlSchemaAnyAttribute anyAttribute)
		{
			particle = null;
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ICollection elementMembers = map.ElementMembers;
			if (elementMembers != null && !map.HasSimpleContent)
			{
				foreach (object obj in elementMembers)
				{
					XmlTypeMapMemberElement xmlTypeMapMemberElement = (XmlTypeMapMemberElement)obj;
					if (baseMap == null || !this.DefinedInBaseMap(baseMap, xmlTypeMapMemberElement))
					{
						Type type = xmlTypeMapMemberElement.GetType();
						if (type == typeof(XmlTypeMapMemberFlatList))
						{
							XmlSchemaParticle schemaArrayElement = this.GetSchemaArrayElement(schema, xmlTypeMapMemberElement.ElementInfo);
							if (schemaArrayElement != null)
							{
								xmlSchemaSequence.Items.Add(schemaArrayElement);
							}
						}
						else if (type == typeof(XmlTypeMapMemberAnyElement))
						{
							xmlSchemaSequence.Items.Add(this.GetSchemaArrayElement(schema, xmlTypeMapMemberElement.ElementInfo));
						}
						else if (type == typeof(XmlTypeMapMemberElement))
						{
							this.GetSchemaElement(schema, (XmlTypeMapElementInfo)xmlTypeMapMemberElement.ElementInfo[0], xmlTypeMapMemberElement.DefaultValue, true, new XmlSchemaExporter.XmlSchemaObjectContainer(xmlSchemaSequence));
						}
						else
						{
							this.GetSchemaElement(schema, (XmlTypeMapElementInfo)xmlTypeMapMemberElement.ElementInfo[0], true, new XmlSchemaExporter.XmlSchemaObjectContainer(xmlSchemaSequence));
						}
					}
				}
			}
			if (xmlSchemaSequence.Items.Count > 0)
			{
				particle = xmlSchemaSequence;
			}
			ICollection attributeMembers = map.AttributeMembers;
			if (attributeMembers != null)
			{
				foreach (object obj2 in attributeMembers)
				{
					XmlTypeMapMemberAttribute xmlTypeMapMemberAttribute = (XmlTypeMapMemberAttribute)obj2;
					if (baseMap == null || !this.DefinedInBaseMap(baseMap, xmlTypeMapMemberAttribute))
					{
						outAttributes.Add(this.GetSchemaAttribute(schema, xmlTypeMapMemberAttribute, true));
					}
				}
			}
			XmlTypeMapMember defaultAnyAttributeMember = map.DefaultAnyAttributeMember;
			if (defaultAnyAttributeMember != null)
			{
				anyAttribute = new XmlSchemaAnyAttribute();
			}
			else
			{
				anyAttribute = null;
			}
		}

		private XmlSchemaElement FindElement(XmlSchemaObjectCollection col, string name)
		{
			foreach (XmlSchemaObject xmlSchemaObject in col)
			{
				XmlSchemaElement xmlSchemaElement = xmlSchemaObject as XmlSchemaElement;
				if (xmlSchemaElement != null && xmlSchemaElement.Name == name)
				{
					return xmlSchemaElement;
				}
			}
			return null;
		}

		private XmlSchemaComplexType FindComplexType(XmlSchemaObjectCollection col, string name)
		{
			foreach (XmlSchemaObject xmlSchemaObject in col)
			{
				XmlSchemaComplexType xmlSchemaComplexType = xmlSchemaObject as XmlSchemaComplexType;
				if (xmlSchemaComplexType != null && xmlSchemaComplexType.Name == name)
				{
					return xmlSchemaComplexType;
				}
			}
			return null;
		}

		private XmlSchemaAttribute GetSchemaAttribute(XmlSchema currentSchema, XmlTypeMapMemberAttribute attinfo, bool isTypeMember)
		{
			XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
			if (attinfo.DefaultValue != DBNull.Value)
			{
				xmlSchemaAttribute.DefaultValue = this.ExportDefaultValue(attinfo.TypeData, attinfo.MappedType, attinfo.DefaultValue);
			}
			else if (!attinfo.IsOptionalValueType && attinfo.TypeData.IsValueType)
			{
				xmlSchemaAttribute.Use = XmlSchemaUse.Required;
			}
			this.ImportNamespace(currentSchema, attinfo.Namespace);
			XmlSchema xmlSchema;
			if (attinfo.Namespace.Length == 0 && attinfo.Form != XmlSchemaForm.Qualified)
			{
				xmlSchema = currentSchema;
			}
			else
			{
				xmlSchema = this.GetSchema(attinfo.Namespace);
			}
			if (currentSchema != xmlSchema && !this.encodedFormat)
			{
				xmlSchemaAttribute.RefName = new XmlQualifiedName(attinfo.AttributeName, attinfo.Namespace);
				foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Items)
				{
					if (xmlSchemaObject is XmlSchemaAttribute && ((XmlSchemaAttribute)xmlSchemaObject).Name == attinfo.AttributeName)
					{
						return xmlSchemaAttribute;
					}
				}
				xmlSchema.Items.Add(this.GetSchemaAttribute(xmlSchema, attinfo, false));
				return xmlSchemaAttribute;
			}
			xmlSchemaAttribute.Name = attinfo.AttributeName;
			if (isTypeMember)
			{
				xmlSchemaAttribute.Form = attinfo.Form;
			}
			if (attinfo.TypeData.SchemaType == SchemaTypes.Enum)
			{
				this.ImportNamespace(currentSchema, attinfo.DataTypeNamespace);
				this.ExportEnumSchema(attinfo.MappedType);
				xmlSchemaAttribute.SchemaTypeName = new XmlQualifiedName(attinfo.TypeData.XmlType, attinfo.DataTypeNamespace);
			}
			else if (attinfo.TypeData.SchemaType == SchemaTypes.Array && TypeTranslator.IsPrimitive(attinfo.TypeData.ListItemType))
			{
				xmlSchemaAttribute.SchemaType = this.GetSchemaSimpleListType(attinfo.TypeData);
			}
			else
			{
				xmlSchemaAttribute.SchemaTypeName = new XmlQualifiedName(attinfo.TypeData.XmlType, attinfo.DataTypeNamespace);
			}
			return xmlSchemaAttribute;
		}

		private XmlSchemaParticle GetSchemaElement(XmlSchema currentSchema, XmlTypeMapElementInfo einfo, bool isTypeMember)
		{
			return this.GetSchemaElement(currentSchema, einfo, DBNull.Value, isTypeMember, null);
		}

		private XmlSchemaParticle GetSchemaElement(XmlSchema currentSchema, XmlTypeMapElementInfo einfo, bool isTypeMember, XmlSchemaExporter.XmlSchemaObjectContainer container)
		{
			return this.GetSchemaElement(currentSchema, einfo, DBNull.Value, isTypeMember, container);
		}

		private XmlSchemaParticle GetSchemaElement(XmlSchema currentSchema, XmlTypeMapElementInfo einfo, object defaultValue, bool isTypeMember, XmlSchemaExporter.XmlSchemaObjectContainer container)
		{
			if (einfo.IsTextElement)
			{
				return null;
			}
			if (einfo.IsUnnamedAnyElement)
			{
				XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
				xmlSchemaAny.MinOccurs = 0m;
				xmlSchemaAny.MaxOccurs = 1m;
				if (container != null)
				{
					container.Items.Add(xmlSchemaAny);
				}
				return xmlSchemaAny;
			}
			XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
			xmlSchemaElement.IsNillable = einfo.IsNullable;
			if (container != null)
			{
				container.Items.Add(xmlSchemaElement);
			}
			if (isTypeMember)
			{
				xmlSchemaElement.MaxOccurs = 1m;
				xmlSchemaElement.MinOccurs = ((!einfo.IsNullable) ? 0 : 1);
				if ((defaultValue == DBNull.Value && einfo.TypeData.IsValueType && einfo.Member != null && !einfo.Member.IsOptionalValueType) || this.encodedFormat)
				{
					xmlSchemaElement.MinOccurs = 1m;
				}
			}
			XmlSchema xmlSchema = null;
			if (!this.encodedFormat)
			{
				xmlSchema = this.GetSchema(einfo.Namespace);
				this.ImportNamespace(currentSchema, einfo.Namespace);
			}
			if (currentSchema != xmlSchema && !this.encodedFormat && isTypeMember)
			{
				xmlSchemaElement.RefName = new XmlQualifiedName(einfo.ElementName, einfo.Namespace);
				foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Items)
				{
					if (xmlSchemaObject is XmlSchemaElement && ((XmlSchemaElement)xmlSchemaObject).Name == einfo.ElementName)
					{
						return xmlSchemaElement;
					}
				}
				this.GetSchemaElement(xmlSchema, einfo, defaultValue, false, new XmlSchemaExporter.XmlSchemaObjectContainer(xmlSchema));
				return xmlSchemaElement;
			}
			if (isTypeMember)
			{
				xmlSchemaElement.IsNillable = einfo.IsNullable;
			}
			xmlSchemaElement.Name = einfo.ElementName;
			if (defaultValue != DBNull.Value)
			{
				xmlSchemaElement.DefaultValue = this.ExportDefaultValue(einfo.TypeData, einfo.MappedType, defaultValue);
			}
			if (einfo.Form != XmlSchemaForm.Qualified)
			{
				xmlSchemaElement.Form = einfo.Form;
			}
			switch (einfo.TypeData.SchemaType)
			{
			case SchemaTypes.Primitive:
				xmlSchemaElement.SchemaTypeName = new XmlQualifiedName(einfo.TypeData.XmlType, einfo.DataTypeNamespace);
				if (!einfo.TypeData.IsXsdType)
				{
					this.ImportNamespace(currentSchema, einfo.MappedType.XmlTypeNamespace);
					this.ExportDerivedSchema(einfo.MappedType);
				}
				break;
			case SchemaTypes.Enum:
				xmlSchemaElement.SchemaTypeName = new XmlQualifiedName(einfo.MappedType.XmlType, einfo.MappedType.XmlTypeNamespace);
				this.ImportNamespace(currentSchema, einfo.MappedType.XmlTypeNamespace);
				this.ExportEnumSchema(einfo.MappedType);
				break;
			case SchemaTypes.Array:
			{
				XmlQualifiedName xmlQualifiedName = this.ExportArraySchema(einfo.MappedType, currentSchema.TargetNamespace);
				xmlSchemaElement.SchemaTypeName = xmlQualifiedName;
				this.ImportNamespace(currentSchema, xmlQualifiedName.Namespace);
				break;
			}
			case SchemaTypes.Class:
				if (einfo.MappedType.TypeData.Type != typeof(object))
				{
					xmlSchemaElement.SchemaTypeName = new XmlQualifiedName(einfo.MappedType.XmlType, einfo.MappedType.XmlTypeNamespace);
					this.ImportNamespace(currentSchema, einfo.MappedType.XmlTypeNamespace);
				}
				else if (this.encodedFormat)
				{
					xmlSchemaElement.SchemaTypeName = new XmlQualifiedName(einfo.MappedType.XmlType, einfo.MappedType.XmlTypeNamespace);
				}
				this.ExportClassSchema(einfo.MappedType);
				break;
			case SchemaTypes.XmlSerializable:
				this.SetSchemaXmlSerializableType(einfo.MappedType as XmlSerializableMapping, xmlSchemaElement);
				this.ExportXmlSerializableSchema(currentSchema, einfo.MappedType as XmlSerializableMapping);
				break;
			case SchemaTypes.XmlNode:
				xmlSchemaElement.SchemaType = this.GetSchemaXmlNodeType();
				break;
			}
			return xmlSchemaElement;
		}

		private void ImportNamespace(XmlSchema schema, string ns)
		{
			if (ns == null || ns.Length == 0 || ns == schema.TargetNamespace || ns == "http://www.w3.org/2001/XMLSchema")
			{
				return;
			}
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				if (xmlSchemaObject is XmlSchemaImport && ((XmlSchemaImport)xmlSchemaObject).Namespace == ns)
				{
					return;
				}
			}
			XmlSchemaImport xmlSchemaImport = new XmlSchemaImport();
			xmlSchemaImport.Namespace = ns;
			schema.Includes.Add(xmlSchemaImport);
		}

		private bool DefinedInBaseMap(XmlTypeMapping map, XmlTypeMapMember member)
		{
			return ((ClassMap)map.ObjectMap).FindMember(member.Name) != null || (map.BaseMap != null && this.DefinedInBaseMap(map.BaseMap, member));
		}

		private XmlSchemaType GetSchemaXmlNodeType()
		{
			return new XmlSchemaComplexType
			{
				IsMixed = true,
				Particle = new XmlSchemaSequence
				{
					Items = 
					{
						new XmlSchemaAny()
					}
				}
			};
		}

		private void SetSchemaXmlSerializableType(XmlSerializableMapping map, XmlSchemaElement elem)
		{
			if (map.SchemaType != null && map.Schema != null)
			{
				elem.SchemaType = map.SchemaType;
				return;
			}
			if (map.SchemaType == null && map.SchemaTypeName != null)
			{
				elem.SchemaTypeName = map.SchemaTypeName;
				elem.Name = map.SchemaTypeName.Name;
				return;
			}
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			if (map.Schema == null)
			{
				XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
				xmlSchemaElement.RefName = new XmlQualifiedName("schema", "http://www.w3.org/2001/XMLSchema");
				xmlSchemaSequence.Items.Add(xmlSchemaElement);
				xmlSchemaSequence.Items.Add(new XmlSchemaAny());
			}
			else
			{
				XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
				xmlSchemaAny.Namespace = map.Schema.TargetNamespace;
				xmlSchemaSequence.Items.Add(xmlSchemaAny);
			}
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			elem.SchemaType = xmlSchemaComplexType;
		}

		private XmlSchemaSimpleType GetSchemaSimpleListType(TypeData typeData)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
			XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = new XmlSchemaSimpleTypeList();
			TypeData typeData2 = TypeTranslator.GetTypeData(typeData.ListItemType);
			xmlSchemaSimpleTypeList.ItemTypeName = new XmlQualifiedName(typeData2.XmlType, "http://www.w3.org/2001/XMLSchema");
			xmlSchemaSimpleType.Content = xmlSchemaSimpleTypeList;
			return xmlSchemaSimpleType;
		}

		private XmlSchemaParticle GetSchemaArrayElement(XmlSchema currentSchema, XmlTypeMapElementInfoList infos)
		{
			int num = infos.Count;
			if (num > 0 && ((XmlTypeMapElementInfo)infos[0]).IsTextElement)
			{
				num--;
			}
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				XmlSchemaParticle schemaElement = this.GetSchemaElement(currentSchema, (XmlTypeMapElementInfo)infos[infos.Count - 1], true);
				schemaElement.MinOccursString = "0";
				schemaElement.MaxOccursString = "unbounded";
				return schemaElement;
			}
			XmlSchemaChoice xmlSchemaChoice = new XmlSchemaChoice();
			xmlSchemaChoice.MinOccursString = "0";
			xmlSchemaChoice.MaxOccursString = "unbounded";
			foreach (object obj in infos)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
				if (!xmlTypeMapElementInfo.IsTextElement)
				{
					xmlSchemaChoice.Items.Add(this.GetSchemaElement(currentSchema, xmlTypeMapElementInfo, true));
				}
			}
			return xmlSchemaChoice;
		}

		private string ExportDefaultValue(TypeData typeData, XmlTypeMapping map, object defaultValue)
		{
			if (typeData.SchemaType == SchemaTypes.Enum)
			{
				EnumMap enumMap = (EnumMap)map.ObjectMap;
				return enumMap.GetXmlName(map.TypeFullName, defaultValue);
			}
			return XmlCustomFormatter.ToXmlString(typeData, defaultValue);
		}

		private void ExportDerivedSchema(XmlTypeMapping map)
		{
			if (this.IsMapExported(map))
			{
				return;
			}
			this.SetMapExported(map);
			XmlSchema schema = this.GetSchema(map.XmlTypeNamespace);
			for (int i = 0; i < schema.Items.Count; i++)
			{
				XmlSchemaSimpleType xmlSchemaSimpleType = schema.Items[i] as XmlSchemaSimpleType;
				if (xmlSchemaSimpleType != null && xmlSchemaSimpleType.Name == map.ElementName)
				{
					return;
				}
			}
			XmlSchemaSimpleType xmlSchemaSimpleType2 = new XmlSchemaSimpleType();
			xmlSchemaSimpleType2.Name = map.ElementName;
			schema.Items.Add(xmlSchemaSimpleType2);
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
			xmlSchemaSimpleTypeRestriction.BaseTypeName = new XmlQualifiedName(map.TypeData.MappedType.XmlType, "http://www.w3.org/2001/XMLSchema");
			XmlSchemaPatternFacet xmlSchemaPatternFacet = map.TypeData.XmlSchemaPatternFacet;
			if (xmlSchemaPatternFacet != null)
			{
				xmlSchemaSimpleTypeRestriction.Facets.Add(xmlSchemaPatternFacet);
			}
			xmlSchemaSimpleType2.Content = xmlSchemaSimpleTypeRestriction;
		}

		private void ExportEnumSchema(XmlTypeMapping map)
		{
			if (this.IsMapExported(map))
			{
				return;
			}
			this.SetMapExported(map);
			XmlSchema schema = this.GetSchema(map.XmlTypeNamespace);
			XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
			xmlSchemaSimpleType.Name = map.ElementName;
			schema.Items.Add(xmlSchemaSimpleType);
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
			xmlSchemaSimpleTypeRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
			EnumMap enumMap = (EnumMap)map.ObjectMap;
			foreach (EnumMap.EnumMapMember enumMapMember in enumMap.Members)
			{
				XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet = new XmlSchemaEnumerationFacet();
				xmlSchemaEnumerationFacet.Value = enumMapMember.XmlName;
				xmlSchemaSimpleTypeRestriction.Facets.Add(xmlSchemaEnumerationFacet);
			}
			if (enumMap.IsFlags)
			{
				xmlSchemaSimpleType.Content = new XmlSchemaSimpleTypeList
				{
					ItemType = new XmlSchemaSimpleType
					{
						Content = xmlSchemaSimpleTypeRestriction
					}
				};
			}
			else
			{
				xmlSchemaSimpleType.Content = xmlSchemaSimpleTypeRestriction;
			}
		}

		private XmlQualifiedName ExportArraySchema(XmlTypeMapping map, string defaultNamespace)
		{
			ListMap listMap = (ListMap)map.ObjectMap;
			if (this.encodedFormat)
			{
				string str;
				string text;
				listMap.GetArrayType(-1, out str, out text);
				string text2;
				if (text == "http://www.w3.org/2001/XMLSchema")
				{
					text2 = defaultNamespace;
				}
				else
				{
					text2 = text;
				}
				if (this.IsMapExported(map))
				{
					return new XmlQualifiedName(listMap.GetSchemaArrayName(), text2);
				}
				this.SetMapExported(map);
				XmlSchema schema = this.GetSchema(text2);
				XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
				xmlSchemaComplexType.Name = listMap.GetSchemaArrayName();
				schema.Items.Add(xmlSchemaComplexType);
				XmlSchemaComplexContent xmlSchemaComplexContent = new XmlSchemaComplexContent();
				xmlSchemaComplexContent.IsMixed = false;
				xmlSchemaComplexType.ContentModel = xmlSchemaComplexContent;
				XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = new XmlSchemaComplexContentRestriction();
				xmlSchemaComplexContent.Content = xmlSchemaComplexContentRestriction;
				xmlSchemaComplexContentRestriction.BaseTypeName = new XmlQualifiedName("Array", "http://schemas.xmlsoap.org/soap/encoding/");
				XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
				xmlSchemaComplexContentRestriction.Attributes.Add(xmlSchemaAttribute);
				xmlSchemaAttribute.RefName = new XmlQualifiedName("arrayType", "http://schemas.xmlsoap.org/soap/encoding/");
				XmlAttribute xmlAttribute = this.Document.CreateAttribute("arrayType", "http://schemas.xmlsoap.org/wsdl/");
				xmlAttribute.Value = text + ((!(text != string.Empty)) ? string.Empty : ":") + str;
				xmlSchemaAttribute.UnhandledAttributes = new XmlAttribute[]
				{
					xmlAttribute
				};
				this.ImportNamespace(schema, "http://schemas.xmlsoap.org/wsdl/");
				XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)listMap.ItemInfo[0];
				if (xmlTypeMapElementInfo.MappedType != null)
				{
					switch (xmlTypeMapElementInfo.TypeData.SchemaType)
					{
					case SchemaTypes.Enum:
						this.ExportEnumSchema(xmlTypeMapElementInfo.MappedType);
						break;
					case SchemaTypes.Array:
						this.ExportArraySchema(xmlTypeMapElementInfo.MappedType, text2);
						break;
					case SchemaTypes.Class:
						this.ExportClassSchema(xmlTypeMapElementInfo.MappedType);
						break;
					}
				}
				return new XmlQualifiedName(listMap.GetSchemaArrayName(), text2);
			}
			else
			{
				if (this.IsMapExported(map))
				{
					return new XmlQualifiedName(map.XmlType, map.XmlTypeNamespace);
				}
				this.SetMapExported(map);
				XmlSchema schema2 = this.GetSchema(map.XmlTypeNamespace);
				XmlSchemaComplexType xmlSchemaComplexType2 = new XmlSchemaComplexType();
				xmlSchemaComplexType2.Name = map.ElementName;
				schema2.Items.Add(xmlSchemaComplexType2);
				XmlSchemaParticle schemaArrayElement = this.GetSchemaArrayElement(schema2, listMap.ItemInfo);
				if (schemaArrayElement is XmlSchemaChoice)
				{
					xmlSchemaComplexType2.Particle = schemaArrayElement;
				}
				else
				{
					xmlSchemaComplexType2.Particle = new XmlSchemaSequence
					{
						Items = 
						{
							schemaArrayElement
						}
					};
				}
				return new XmlQualifiedName(map.XmlType, map.XmlTypeNamespace);
			}
		}

		private XmlDocument Document
		{
			get
			{
				if (this.xmlDoc == null)
				{
					this.xmlDoc = new XmlDocument();
				}
				return this.xmlDoc;
			}
		}

		private bool IsMapExported(XmlTypeMapping map)
		{
			return this.exportedMaps.ContainsKey(this.GetMapKey(map));
		}

		private void SetMapExported(XmlTypeMapping map)
		{
			this.exportedMaps[this.GetMapKey(map)] = map;
		}

		private bool IsElementExported(XmlTypeMapping map)
		{
			return this.exportedElements.ContainsKey(this.GetMapKey(map)) || map.TypeData.Type == typeof(object);
		}

		private void SetElementExported(XmlTypeMapping map)
		{
			this.exportedElements[this.GetMapKey(map)] = map;
		}

		private string GetMapKey(XmlTypeMapping map)
		{
			if (map.TypeData.IsListType)
			{
				return string.Concat(new string[]
				{
					this.GetArrayKeyName(map.TypeData),
					" ",
					map.XmlType,
					" ",
					map.XmlTypeNamespace
				});
			}
			return string.Concat(new string[]
			{
				map.TypeData.FullTypeName,
				" ",
				map.XmlType,
				" ",
				map.XmlTypeNamespace
			});
		}

		private string GetArrayKeyName(TypeData td)
		{
			TypeData listItemTypeData = td.ListItemTypeData;
			return "*arrayof*" + ((!listItemTypeData.IsListType) ? listItemTypeData.FullTypeName : this.GetArrayKeyName(listItemTypeData));
		}

		private void CompileSchemas()
		{
		}

		private XmlSchema GetSchema(string ns)
		{
			XmlSchema xmlSchema = this.schemas[ns];
			if (xmlSchema == null)
			{
				xmlSchema = new XmlSchema();
				if (ns != null && ns.Length > 0)
				{
					xmlSchema.TargetNamespace = ns;
				}
				if (!this.encodedFormat)
				{
					xmlSchema.ElementFormDefault = XmlSchemaForm.Qualified;
				}
				this.schemas.Add(xmlSchema);
			}
			return xmlSchema;
		}

		private class XmlSchemaObjectContainer
		{
			private readonly XmlSchemaObject _xmlSchemaObject;

			public XmlSchemaObjectContainer(XmlSchema schema)
			{
				this._xmlSchemaObject = schema;
			}

			public XmlSchemaObjectContainer(XmlSchemaGroupBase group)
			{
				this._xmlSchemaObject = group;
			}

			public XmlSchemaObjectCollection Items
			{
				get
				{
					if (this._xmlSchemaObject is XmlSchema)
					{
						return ((XmlSchema)this._xmlSchemaObject).Items;
					}
					return ((XmlSchemaGroupBase)this._xmlSchemaObject).Items;
				}
			}
		}
	}
}
