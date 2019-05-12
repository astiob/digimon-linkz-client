using Mono.Xml.Schema;
using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the simpleType element for simple content from XML Schema as specified by the World Wide Web Consortium (W3C). This class defines a simple type. Simple types can specify information and constraints for the value of attributes or elements with text-only content.</summary>
	public class XmlSchemaSimpleType : XmlSchemaType
	{
		private const string xmlname = "simpleType";

		private static XmlSchemaSimpleType schemaLocationType = new XmlSchemaSimpleType
		{
			Content = new XmlSchemaSimpleTypeList
			{
				ItemTypeName = new XmlQualifiedName("anyURI", "http://www.w3.org/2001/XMLSchema")
			},
			BaseXmlSchemaTypeInternal = null,
			variety = XmlSchemaDerivationMethod.List
		};

		private XmlSchemaSimpleTypeContent content;

		internal bool islocal = true;

		private bool recursed;

		private XmlSchemaDerivationMethod variety;

		internal static readonly XmlSchemaSimpleType XsAnySimpleType = XmlSchemaSimpleType.BuildSchemaType("anySimpleType", null);

		internal static readonly XmlSchemaSimpleType XsString = XmlSchemaSimpleType.BuildSchemaType("string", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsBoolean = XmlSchemaSimpleType.BuildSchemaType("boolean", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsDecimal = XmlSchemaSimpleType.BuildSchemaType("decimal", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsFloat = XmlSchemaSimpleType.BuildSchemaType("float", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsDouble = XmlSchemaSimpleType.BuildSchemaType("double", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsDuration = XmlSchemaSimpleType.BuildSchemaType("duration", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsDateTime = XmlSchemaSimpleType.BuildSchemaType("dateTime", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsTime = XmlSchemaSimpleType.BuildSchemaType("time", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsDate = XmlSchemaSimpleType.BuildSchemaType("date", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsGYearMonth = XmlSchemaSimpleType.BuildSchemaType("gYearMonth", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsGYear = XmlSchemaSimpleType.BuildSchemaType("gYear", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsGMonthDay = XmlSchemaSimpleType.BuildSchemaType("gMonthDay", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsGDay = XmlSchemaSimpleType.BuildSchemaType("gDay", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsGMonth = XmlSchemaSimpleType.BuildSchemaType("gMonth", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsHexBinary = XmlSchemaSimpleType.BuildSchemaType("hexBinary", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsBase64Binary = XmlSchemaSimpleType.BuildSchemaType("base64Binary", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsAnyUri = XmlSchemaSimpleType.BuildSchemaType("anyURI", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsQName = XmlSchemaSimpleType.BuildSchemaType("QName", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsNotation = XmlSchemaSimpleType.BuildSchemaType("NOTATION", "anySimpleType");

		internal static readonly XmlSchemaSimpleType XsNormalizedString = XmlSchemaSimpleType.BuildSchemaType("normalizedString", "string");

		internal static readonly XmlSchemaSimpleType XsToken = XmlSchemaSimpleType.BuildSchemaType("token", "normalizedString");

		internal static readonly XmlSchemaSimpleType XsLanguage = XmlSchemaSimpleType.BuildSchemaType("language", "token");

		internal static readonly XmlSchemaSimpleType XsNMToken = XmlSchemaSimpleType.BuildSchemaType("NMTOKEN", "token");

		internal static readonly XmlSchemaSimpleType XsNMTokens;

		internal static readonly XmlSchemaSimpleType XsName = XmlSchemaSimpleType.BuildSchemaType("Name", "token");

		internal static readonly XmlSchemaSimpleType XsNCName = XmlSchemaSimpleType.BuildSchemaType("NCName", "Name");

		internal static readonly XmlSchemaSimpleType XsID = XmlSchemaSimpleType.BuildSchemaType("ID", "NCName");

		internal static readonly XmlSchemaSimpleType XsIDRef = XmlSchemaSimpleType.BuildSchemaType("IDREF", "NCName");

		internal static readonly XmlSchemaSimpleType XsIDRefs;

		internal static readonly XmlSchemaSimpleType XsEntity = XmlSchemaSimpleType.BuildSchemaType("ENTITY", "NCName");

		internal static readonly XmlSchemaSimpleType XsEntities;

		internal static readonly XmlSchemaSimpleType XsInteger = XmlSchemaSimpleType.BuildSchemaType("integer", "decimal");

		internal static readonly XmlSchemaSimpleType XsNonPositiveInteger = XmlSchemaSimpleType.BuildSchemaType("nonPositiveInteger", "integer");

		internal static readonly XmlSchemaSimpleType XsNegativeInteger = XmlSchemaSimpleType.BuildSchemaType("negativeInteger", "nonPositiveInteger");

		internal static readonly XmlSchemaSimpleType XsLong = XmlSchemaSimpleType.BuildSchemaType("long", "integer");

		internal static readonly XmlSchemaSimpleType XsInt = XmlSchemaSimpleType.BuildSchemaType("int", "long");

		internal static readonly XmlSchemaSimpleType XsShort = XmlSchemaSimpleType.BuildSchemaType("short", "int");

		internal static readonly XmlSchemaSimpleType XsByte = XmlSchemaSimpleType.BuildSchemaType("byte", "short");

		internal static readonly XmlSchemaSimpleType XsNonNegativeInteger = XmlSchemaSimpleType.BuildSchemaType("nonNegativeInteger", "integer");

		internal static readonly XmlSchemaSimpleType XsUnsignedLong = XmlSchemaSimpleType.BuildSchemaType("unsignedLong", "nonNegativeInteger");

		internal static readonly XmlSchemaSimpleType XsUnsignedInt = XmlSchemaSimpleType.BuildSchemaType("unsignedInt", "unsignedLong");

		internal static readonly XmlSchemaSimpleType XsUnsignedShort = XmlSchemaSimpleType.BuildSchemaType("unsignedShort", "unsignedInt");

		internal static readonly XmlSchemaSimpleType XsUnsignedByte = XmlSchemaSimpleType.BuildSchemaType("unsignedByte", "unsignedShort");

		internal static readonly XmlSchemaSimpleType XsPositiveInteger = XmlSchemaSimpleType.BuildSchemaType("positiveInteger", "nonNegativeInteger");

		internal static readonly XmlSchemaSimpleType XdtUntypedAtomic;

		internal static readonly XmlSchemaSimpleType XdtAnyAtomicType = XmlSchemaSimpleType.BuildSchemaType("anyAtomicType", "anySimpleType", true, false);

		internal static readonly XmlSchemaSimpleType XdtYearMonthDuration;

		internal static readonly XmlSchemaSimpleType XdtDayTimeDuration;

		static XmlSchemaSimpleType()
		{
			XmlSchemaSimpleType.XdtUntypedAtomic = XmlSchemaSimpleType.BuildSchemaType("untypedAtomic", "anyAtomicType", true, true);
			XmlSchemaSimpleType.XdtDayTimeDuration = XmlSchemaSimpleType.BuildSchemaType("dayTimeDuration", "duration", true, false);
			XmlSchemaSimpleType.XdtYearMonthDuration = XmlSchemaSimpleType.BuildSchemaType("yearMonthDuration", "duration", true, false);
			XmlSchemaSimpleType.XsIDRefs = new XmlSchemaSimpleType();
			XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = new XmlSchemaSimpleTypeList();
			xmlSchemaSimpleTypeList.ItemType = XmlSchemaSimpleType.XsIDRef;
			XmlSchemaSimpleType.XsIDRefs.Content = xmlSchemaSimpleTypeList;
			XmlSchemaSimpleType.XsEntities = new XmlSchemaSimpleType();
			xmlSchemaSimpleTypeList = new XmlSchemaSimpleTypeList();
			xmlSchemaSimpleTypeList.ItemType = XmlSchemaSimpleType.XsEntity;
			XmlSchemaSimpleType.XsEntities.Content = xmlSchemaSimpleTypeList;
			XmlSchemaSimpleType.XsNMTokens = new XmlSchemaSimpleType();
			xmlSchemaSimpleTypeList = new XmlSchemaSimpleTypeList();
			xmlSchemaSimpleTypeList.ItemType = XmlSchemaSimpleType.XsNMToken;
			XmlSchemaSimpleType.XsNMTokens.Content = xmlSchemaSimpleTypeList;
		}

		private static XmlSchemaSimpleType BuildSchemaType(string name, string baseName)
		{
			return XmlSchemaSimpleType.BuildSchemaType(name, baseName, false, false);
		}

		private static XmlSchemaSimpleType BuildSchemaType(string name, string baseName, bool xdt, bool baseXdt)
		{
			string ns = (!xdt) ? "http://www.w3.org/2001/XMLSchema" : "http://www.w3.org/2003/11/xpath-datatypes";
			string ns2 = (!baseXdt) ? "http://www.w3.org/2001/XMLSchema" : "http://www.w3.org/2003/11/xpath-datatypes";
			XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
			xmlSchemaSimpleType.QNameInternal = new XmlQualifiedName(name, ns);
			if (baseName != null)
			{
				xmlSchemaSimpleType.BaseXmlSchemaTypeInternal = XmlSchemaType.GetBuiltInSimpleType(new XmlQualifiedName(baseName, ns2));
			}
			xmlSchemaSimpleType.DatatypeInternal = XmlSchemaDatatype.FromName(xmlSchemaSimpleType.QualifiedName);
			return xmlSchemaSimpleType;
		}

		internal static XsdAnySimpleType AnySimpleType
		{
			get
			{
				return XsdAnySimpleType.Instance;
			}
		}

		internal static XmlSchemaSimpleType SchemaLocationType
		{
			get
			{
				return XmlSchemaSimpleType.schemaLocationType;
			}
		}

		/// <summary>Gets or sets one of <see cref="T:System.Xml.Schema.XmlSchemaSimpleTypeUnion" />, <see cref="T:System.Xml.Schema.XmlSchemaSimpleTypeList" />, or <see cref="T:System.Xml.Schema.XmlSchemaSimpleTypeRestriction" />.</summary>
		/// <returns>One of XmlSchemaSimpleTypeUnion, XmlSchemaSimpleTypeList, or XmlSchemaSimpleTypeRestriction.</returns>
		[XmlElement("list", typeof(XmlSchemaSimpleTypeList))]
		[XmlElement("restriction", typeof(XmlSchemaSimpleTypeRestriction))]
		[XmlElement("union", typeof(XmlSchemaSimpleTypeUnion))]
		public XmlSchemaSimpleTypeContent Content
		{
			get
			{
				return this.content;
			}
			set
			{
				this.content = value;
			}
		}

		internal XmlSchemaDerivationMethod Variety
		{
			get
			{
				return this.variety;
			}
		}

		internal override void SetParent(XmlSchemaObject parent)
		{
			base.SetParent(parent);
			if (this.Content != null)
			{
				this.Content.SetParent(this);
			}
		}

		internal override int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.CompilationId == schema.CompilationId)
			{
				return 0;
			}
			this.errorCount = 0;
			if (this.islocal)
			{
				if (base.Name != null)
				{
					base.error(h, "Name is prohibited in a local simpletype");
				}
				else
				{
					this.QNameInternal = new XmlQualifiedName(base.Name, base.AncestorSchema.TargetNamespace);
				}
				if (base.Final != XmlSchemaDerivationMethod.None)
				{
					base.error(h, "Final is prohibited in a local simpletype");
				}
			}
			else
			{
				if (base.Name == null)
				{
					base.error(h, "Name is required in top level simpletype");
				}
				else if (!XmlSchemaUtil.CheckNCName(base.Name))
				{
					base.error(h, "name attribute of a simpleType must be NCName");
				}
				else
				{
					this.QNameInternal = new XmlQualifiedName(base.Name, base.AncestorSchema.TargetNamespace);
				}
				XmlSchemaDerivationMethod final = base.Final;
				if (final != XmlSchemaDerivationMethod.All)
				{
					if (final != XmlSchemaDerivationMethod.None)
					{
						if (final == XmlSchemaDerivationMethod.Restriction || final == XmlSchemaDerivationMethod.List || final == XmlSchemaDerivationMethod.Union)
						{
							this.finalResolved = base.Final;
							goto IL_19F;
						}
						base.error(h, "The value of final attribute is not valid for simpleType");
					}
					XmlSchemaDerivationMethod xmlSchemaDerivationMethod = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union;
					XmlSchemaDerivationMethod finalDefault = schema.FinalDefault;
					if (finalDefault != XmlSchemaDerivationMethod.All)
					{
						if (finalDefault != XmlSchemaDerivationMethod.None)
						{
							this.finalResolved = (schema.FinalDefault & xmlSchemaDerivationMethod);
						}
						else
						{
							this.finalResolved = XmlSchemaDerivationMethod.Empty;
						}
					}
					else
					{
						this.finalResolved = XmlSchemaDerivationMethod.All;
					}
				}
				else
				{
					this.finalResolved = XmlSchemaDerivationMethod.All;
				}
			}
			IL_19F:
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			if (this.Content != null)
			{
				this.Content.OwnerType = this;
			}
			if (this.Content == null)
			{
				base.error(h, "Content is required in a simpletype");
			}
			else if (this.Content is XmlSchemaSimpleTypeRestriction)
			{
				this.resolvedDerivedBy = XmlSchemaDerivationMethod.Restriction;
				this.errorCount += ((XmlSchemaSimpleTypeRestriction)this.Content).Compile(h, schema);
			}
			else if (this.Content is XmlSchemaSimpleTypeList)
			{
				this.resolvedDerivedBy = XmlSchemaDerivationMethod.List;
				this.errorCount += ((XmlSchemaSimpleTypeList)this.Content).Compile(h, schema);
			}
			else if (this.Content is XmlSchemaSimpleTypeUnion)
			{
				this.resolvedDerivedBy = XmlSchemaDerivationMethod.Union;
				this.errorCount += ((XmlSchemaSimpleTypeUnion)this.Content).Compile(h, schema);
			}
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		internal void CollectBaseType(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.Content is XmlSchemaSimpleTypeRestriction)
			{
				object actualType = ((XmlSchemaSimpleTypeRestriction)this.Content).GetActualType(h, schema, false);
				this.BaseXmlSchemaTypeInternal = (actualType as XmlSchemaSimpleType);
				if (this.BaseXmlSchemaTypeInternal != null)
				{
					this.DatatypeInternal = this.BaseXmlSchemaTypeInternal.Datatype;
				}
				else
				{
					this.DatatypeInternal = (actualType as XmlSchemaDatatype);
				}
			}
			else
			{
				this.DatatypeInternal = XmlSchemaSimpleType.AnySimpleType;
			}
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			if (base.IsValidated(schema.ValidationId))
			{
				return this.errorCount;
			}
			if (this.recursed)
			{
				base.error(h, "Circular type reference was found.");
				return this.errorCount;
			}
			this.recursed = true;
			this.CollectBaseType(h, schema);
			if (this.content != null)
			{
				this.errorCount += this.content.Validate(h, schema);
			}
			XmlSchemaSimpleType xmlSchemaSimpleType = base.BaseXmlSchemaType as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType != null)
			{
				this.DatatypeInternal = xmlSchemaSimpleType.Datatype;
			}
			XmlSchemaSimpleType xmlSchemaSimpleType2 = base.BaseXmlSchemaType as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType2 != null && (xmlSchemaSimpleType2.FinalResolved & this.resolvedDerivedBy) != XmlSchemaDerivationMethod.Empty)
			{
				base.error(h, "Specified derivation is prohibited by the base simple type.");
			}
			if (this.resolvedDerivedBy == XmlSchemaDerivationMethod.Restriction && xmlSchemaSimpleType2 != null)
			{
				this.variety = xmlSchemaSimpleType2.Variety;
			}
			else
			{
				this.variety = this.resolvedDerivedBy;
			}
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = this.Content as XmlSchemaSimpleTypeRestriction;
			object baseType = (base.BaseXmlSchemaType == null) ? base.Datatype : base.BaseXmlSchemaType;
			if (xmlSchemaSimpleTypeRestriction != null)
			{
				this.ValidateDerivationValid(baseType, xmlSchemaSimpleTypeRestriction.Facets, h, schema);
			}
			XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = this.Content as XmlSchemaSimpleTypeList;
			if (xmlSchemaSimpleTypeList != null)
			{
				XmlSchemaSimpleType xmlSchemaSimpleType3 = xmlSchemaSimpleTypeList.ValidatedListItemType as XmlSchemaSimpleType;
				if (xmlSchemaSimpleType3 != null && xmlSchemaSimpleType3.Content is XmlSchemaSimpleTypeList)
				{
					base.error(h, "List type must not be derived from another list type.");
				}
			}
			this.recursed = false;
			this.ValidationId = schema.ValidationId;
			return this.errorCount;
		}

		internal void ValidateDerivationValid(object baseType, XmlSchemaObjectCollection facets, ValidationEventHandler h, XmlSchema schema)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = baseType as XmlSchemaSimpleType;
			XmlSchemaDerivationMethod xmlSchemaDerivationMethod = this.Variety;
			if (xmlSchemaDerivationMethod != XmlSchemaDerivationMethod.Restriction)
			{
				if (xmlSchemaDerivationMethod != XmlSchemaDerivationMethod.List)
				{
					if (xmlSchemaDerivationMethod == XmlSchemaDerivationMethod.Union)
					{
						if (facets != null)
						{
							foreach (XmlSchemaObject xmlSchemaObject in facets)
							{
								XmlSchemaFacet xmlSchemaFacet = (XmlSchemaFacet)xmlSchemaObject;
								if (!(xmlSchemaFacet is XmlSchemaEnumerationFacet) && !(xmlSchemaFacet is XmlSchemaPatternFacet))
								{
									base.error(h, "Not allowed facet was found on this simple type which derives list type.");
								}
							}
						}
					}
				}
				else if (facets != null)
				{
					foreach (XmlSchemaObject xmlSchemaObject2 in facets)
					{
						XmlSchemaFacet xmlSchemaFacet2 = (XmlSchemaFacet)xmlSchemaObject2;
						if (!(xmlSchemaFacet2 is XmlSchemaLengthFacet) && !(xmlSchemaFacet2 is XmlSchemaMaxLengthFacet) && !(xmlSchemaFacet2 is XmlSchemaMinLengthFacet) && !(xmlSchemaFacet2 is XmlSchemaEnumerationFacet) && !(xmlSchemaFacet2 is XmlSchemaPatternFacet))
						{
							base.error(h, "Not allowed facet was found on this simple type which derives list type.");
						}
					}
				}
			}
			else
			{
				if (xmlSchemaSimpleType != null && xmlSchemaSimpleType.resolvedDerivedBy != XmlSchemaDerivationMethod.Restriction)
				{
					base.error(h, "Base schema type is not either atomic type or primitive type.");
				}
				if (xmlSchemaSimpleType != null && (xmlSchemaSimpleType.FinalResolved & XmlSchemaDerivationMethod.Restriction) != XmlSchemaDerivationMethod.Empty)
				{
					base.error(h, "Derivation by restriction is prohibited by the base simple type.");
				}
			}
		}

		internal bool ValidateTypeDerivationOK(object baseType, ValidationEventHandler h, XmlSchema schema, bool raiseError)
		{
			if (this == baseType || baseType == XmlSchemaSimpleType.AnySimpleType || baseType == XmlSchemaComplexType.AnyType)
			{
				return true;
			}
			XmlSchemaSimpleType xmlSchemaSimpleType = baseType as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType != null && (xmlSchemaSimpleType.FinalResolved & this.resolvedDerivedBy) != XmlSchemaDerivationMethod.Empty)
			{
				if (raiseError)
				{
					base.error(h, "Specified derivation is prohibited by the base type.");
				}
				return false;
			}
			if (base.BaseXmlSchemaType == baseType || base.Datatype == baseType)
			{
				return true;
			}
			XmlSchemaSimpleType xmlSchemaSimpleType2 = base.BaseXmlSchemaType as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType2 != null && xmlSchemaSimpleType2.ValidateTypeDerivationOK(baseType, h, schema, false))
			{
				return true;
			}
			XmlSchemaDerivationMethod xmlSchemaDerivationMethod = this.Variety;
			if (xmlSchemaDerivationMethod == XmlSchemaDerivationMethod.List || xmlSchemaDerivationMethod == XmlSchemaDerivationMethod.Union)
			{
				if (baseType == XmlSchemaSimpleType.AnySimpleType)
				{
					return true;
				}
			}
			if (xmlSchemaSimpleType != null && xmlSchemaSimpleType.Variety == XmlSchemaDerivationMethod.Union)
			{
				foreach (object baseType2 in ((XmlSchemaSimpleTypeUnion)xmlSchemaSimpleType.Content).ValidatedTypes)
				{
					if (this.ValidateTypeDerivationOK(baseType2, h, schema, false))
					{
						return true;
					}
				}
			}
			if (raiseError)
			{
				base.error(h, "Invalid simple type derivation was found.");
			}
			return false;
		}

		internal string Normalize(string s, XmlNameTable nt, XmlNamespaceManager nsmgr)
		{
			return this.Content.Normalize(s, nt, nsmgr);
		}

		internal static XmlSchemaSimpleType Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "simpleType")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaGroup.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaSimpleType.LineNumber = reader.LineNumber;
			xmlSchemaSimpleType.LinePosition = reader.LinePosition;
			xmlSchemaSimpleType.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "final")
				{
					Exception ex;
					xmlSchemaSimpleType.Final = XmlSchemaUtil.ReadDerivationAttribute(reader, out ex, "final", XmlSchemaUtil.FinalAllowed);
					if (ex != null)
					{
						XmlSchemaObject.error(h, "some invalid values not a valid value for final", ex);
					}
				}
				else if (reader.Name == "id")
				{
					xmlSchemaSimpleType.Id = reader.Value;
				}
				else if (reader.Name == "name")
				{
					xmlSchemaSimpleType.Name = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for simpleType", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaSimpleType);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaSimpleType;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "simpleType")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaSimpleType.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaSimpleType.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					if (num <= 2)
					{
						if (reader.LocalName == "restriction")
						{
							num = 3;
							XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = XmlSchemaSimpleTypeRestriction.Read(reader, h);
							if (xmlSchemaSimpleTypeRestriction != null)
							{
								xmlSchemaSimpleType.content = xmlSchemaSimpleTypeRestriction;
							}
							continue;
						}
						if (reader.LocalName == "list")
						{
							num = 3;
							XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = XmlSchemaSimpleTypeList.Read(reader, h);
							if (xmlSchemaSimpleTypeList != null)
							{
								xmlSchemaSimpleType.content = xmlSchemaSimpleTypeList;
							}
							continue;
						}
						if (reader.LocalName == "union")
						{
							num = 3;
							XmlSchemaSimpleTypeUnion xmlSchemaSimpleTypeUnion = XmlSchemaSimpleTypeUnion.Read(reader, h);
							if (xmlSchemaSimpleTypeUnion != null)
							{
								xmlSchemaSimpleType.content = xmlSchemaSimpleTypeUnion;
							}
							continue;
						}
					}
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaSimpleType;
		}
	}
}
