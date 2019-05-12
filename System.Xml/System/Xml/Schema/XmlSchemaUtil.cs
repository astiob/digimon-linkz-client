using Mono.Xml;
using Mono.Xml.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	internal class XmlSchemaUtil
	{
		internal static XmlSchemaDerivationMethod FinalAllowed;

		internal static XmlSchemaDerivationMethod ElementBlockAllowed;

		internal static XmlSchemaDerivationMethod ComplexTypeBlockAllowed;

		internal static readonly bool StrictMsCompliant = Environment.GetEnvironmentVariable("MONO_STRICT_MS_COMPLIANT") == "yes";

		static XmlSchemaUtil()
		{
			XmlSchemaUtil.FinalAllowed = (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction);
			XmlSchemaUtil.ComplexTypeBlockAllowed = XmlSchemaUtil.FinalAllowed;
			XmlSchemaUtil.ElementBlockAllowed = (XmlSchemaDerivationMethod.Substitution | XmlSchemaUtil.FinalAllowed);
		}

		public static void AddToTable(XmlSchemaObjectTable table, XmlSchemaObject obj, XmlQualifiedName qname, ValidationEventHandler h)
		{
			if (table.Contains(qname))
			{
				if (obj.isRedefineChild)
				{
					if (obj.redefinedObject != null)
					{
						obj.error(h, string.Format("Named item {0} was already contained in the schema object table.", qname));
					}
					else
					{
						obj.redefinedObject = table[qname];
					}
					table.Set(qname, obj);
				}
				else
				{
					if (table[qname].isRedefineChild)
					{
						if (table[qname].redefinedObject != null)
						{
							obj.error(h, string.Format("Named item {0} was already contained in the schema object table.", qname));
						}
						else
						{
							table[qname].redefinedObject = obj;
						}
						return;
					}
					if (XmlSchemaUtil.StrictMsCompliant)
					{
						table.Set(qname, obj);
					}
					else
					{
						obj.error(h, string.Format("Named item {0} was already contained in the schema object table. {1}", qname, "Consider setting MONO_STRICT_MS_COMPLIANT to 'yes' to mimic MS implementation."));
					}
				}
			}
			else
			{
				table.Set(qname, obj);
			}
		}

		public static void CompileID(string id, XmlSchemaObject xso, Hashtable idCollection, ValidationEventHandler h)
		{
			if (id == null)
			{
				return;
			}
			if (!XmlSchemaUtil.CheckNCName(id))
			{
				xso.error(h, id + " is not a valid id attribute");
			}
			else if (idCollection.ContainsKey(id))
			{
				xso.error(h, "Duplicate id attribute " + id);
			}
			else
			{
				idCollection.Add(id, xso);
			}
		}

		public static bool CheckAnyUri(string uri)
		{
			return !uri.StartsWith("##");
		}

		public static bool CheckNormalizedString(string token)
		{
			return true;
		}

		public static bool CheckNCName(string name)
		{
			return XmlChar.IsNCName(name);
		}

		public static bool CheckQName(XmlQualifiedName qname)
		{
			return true;
		}

		public static XmlParserContext GetParserContext(XmlReader reader)
		{
			IHasXmlParserContext hasXmlParserContext = reader as IHasXmlParserContext;
			if (hasXmlParserContext != null)
			{
				return hasXmlParserContext.ParserContext;
			}
			return null;
		}

		public static bool IsBuiltInDatatypeName(XmlQualifiedName qname)
		{
			string name;
			if (qname.Namespace == "http://www.w3.org/2003/11/xpath-datatypes")
			{
				name = qname.Name;
				if (name != null)
				{
					if (XmlSchemaUtil.<>f__switch$map36 == null)
					{
						XmlSchemaUtil.<>f__switch$map36 = new Dictionary<string, int>(4)
						{
							{
								"anyAtomicType",
								0
							},
							{
								"untypedAtomic",
								0
							},
							{
								"dayTimeDuration",
								0
							},
							{
								"yearMonthDuration",
								0
							}
						};
					}
					int num;
					if (XmlSchemaUtil.<>f__switch$map36.TryGetValue(name, out num))
					{
						if (num == 0)
						{
							return true;
						}
					}
				}
				return false;
			}
			if (qname.Namespace != "http://www.w3.org/2001/XMLSchema")
			{
				return false;
			}
			name = qname.Name;
			if (name != null)
			{
				if (XmlSchemaUtil.<>f__switch$map37 == null)
				{
					XmlSchemaUtil.<>f__switch$map37 = new Dictionary<string, int>(45)
					{
						{
							"anySimpleType",
							0
						},
						{
							"duration",
							0
						},
						{
							"dateTime",
							0
						},
						{
							"time",
							0
						},
						{
							"date",
							0
						},
						{
							"gYearMonth",
							0
						},
						{
							"gYear",
							0
						},
						{
							"gMonthDay",
							0
						},
						{
							"gDay",
							0
						},
						{
							"gMonth",
							0
						},
						{
							"boolean",
							0
						},
						{
							"base64Binary",
							0
						},
						{
							"hexBinary",
							0
						},
						{
							"float",
							0
						},
						{
							"double",
							0
						},
						{
							"anyURI",
							0
						},
						{
							"QName",
							0
						},
						{
							"NOTATION",
							0
						},
						{
							"string",
							0
						},
						{
							"normalizedString",
							0
						},
						{
							"token",
							0
						},
						{
							"language",
							0
						},
						{
							"Name",
							0
						},
						{
							"NCName",
							0
						},
						{
							"ID",
							0
						},
						{
							"IDREF",
							0
						},
						{
							"IDREFS",
							0
						},
						{
							"ENTITY",
							0
						},
						{
							"ENTITIES",
							0
						},
						{
							"NMTOKEN",
							0
						},
						{
							"NMTOKENS",
							0
						},
						{
							"decimal",
							0
						},
						{
							"integer",
							0
						},
						{
							"nonPositiveInteger",
							0
						},
						{
							"negativeInteger",
							0
						},
						{
							"nonNegativeInteger",
							0
						},
						{
							"unsignedLong",
							0
						},
						{
							"unsignedInt",
							0
						},
						{
							"unsignedShort",
							0
						},
						{
							"unsignedByte",
							0
						},
						{
							"positiveInteger",
							0
						},
						{
							"long",
							0
						},
						{
							"int",
							0
						},
						{
							"short",
							0
						},
						{
							"byte",
							0
						}
					};
				}
				int num;
				if (XmlSchemaUtil.<>f__switch$map37.TryGetValue(name, out num))
				{
					if (num == 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool AreSchemaDatatypeEqual(XmlSchemaSimpleType st1, object v1, XmlSchemaSimpleType st2, object v2)
		{
			if (st1.Datatype is XsdAnySimpleType)
			{
				return XmlSchemaUtil.AreSchemaDatatypeEqual(st1.Datatype as XsdAnySimpleType, v1, st2.Datatype as XsdAnySimpleType, v2);
			}
			string[] array = v1 as string[];
			string[] array2 = v2 as string[];
			if (st1 != st2 || array == null || array2 == null || array.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		public static bool AreSchemaDatatypeEqual(XsdAnySimpleType st1, object v1, XsdAnySimpleType st2, object v2)
		{
			if (v1 == null || v2 == null)
			{
				return false;
			}
			if (st1 == null)
			{
				st1 = XmlSchemaSimpleType.AnySimpleType;
			}
			if (st2 == null)
			{
				st2 = XmlSchemaSimpleType.AnySimpleType;
			}
			Type type = st2.GetType();
			if (st1 is XsdFloat)
			{
				return st2 is XsdFloat && Convert.ToSingle(v1) == Convert.ToSingle(v2);
			}
			if (st1 is XsdDouble)
			{
				return st2 is XsdDouble && Convert.ToDouble(v1) == Convert.ToDouble(v2);
			}
			if (st1 is XsdDecimal)
			{
				if (!(st2 is XsdDecimal) || Convert.ToDecimal(v1) != Convert.ToDecimal(v2))
				{
					return false;
				}
				if (st1 is XsdNonPositiveInteger)
				{
					return st2 is XsdNonPositiveInteger || type == typeof(XsdDecimal) || type == typeof(XsdInteger);
				}
				if (st1 is XsdPositiveInteger)
				{
					return st2 is XsdPositiveInteger || type == typeof(XsdDecimal) || type == typeof(XsdInteger) || type == typeof(XsdNonNegativeInteger);
				}
				if (st1 is XsdUnsignedLong)
				{
					return st2 is XsdUnsignedLong || type == typeof(XsdDecimal) || type == typeof(XsdInteger) || type == typeof(XsdNonNegativeInteger);
				}
				if (st1 is XsdNonNegativeInteger)
				{
					return st2 is XsdNonNegativeInteger || type == typeof(XsdDecimal) || type == typeof(XsdInteger);
				}
				return !(st1 is XsdLong) || st2 is XsdLong || type == typeof(XsdDecimal) || type == typeof(XsdInteger);
			}
			else
			{
				if (!v1.Equals(v2))
				{
					return false;
				}
				if (st1 is XsdString)
				{
					if (!(st2 is XsdString))
					{
						return false;
					}
					if (st1 is XsdNMToken && (st2 is XsdLanguage || st2 is XsdName))
					{
						return false;
					}
					if (st2 is XsdNMToken && (st1 is XsdLanguage || st1 is XsdName))
					{
						return false;
					}
					if (st1 is XsdName && (st2 is XsdLanguage || st2 is XsdNMToken))
					{
						return false;
					}
					if (st2 is XsdName && (st1 is XsdLanguage || st1 is XsdNMToken))
					{
						return false;
					}
					if (st1 is XsdID && st2 is XsdIDRef)
					{
						return false;
					}
					if (st1 is XsdIDRef && st2 is XsdID)
					{
						return false;
					}
				}
				else if (st1 != st2)
				{
					return false;
				}
				return true;
			}
		}

		public static bool IsValidQName(string qname)
		{
			foreach (string name in qname.Split(new char[]
			{
				':'
			}, 2))
			{
				if (!XmlSchemaUtil.CheckNCName(name))
				{
					return false;
				}
			}
			return true;
		}

		public static string[] SplitList(string list)
		{
			if (list == null || list == string.Empty)
			{
				return new string[0];
			}
			ArrayList arrayList = null;
			int num = 0;
			bool flag = true;
			int i = 0;
			while (i < list.Length)
			{
				char c = list[i];
				switch (c)
				{
				case '\t':
				case '\n':
				case '\r':
					goto IL_5F;
				default:
					if (c == ' ')
					{
						goto IL_5F;
					}
					if (flag)
					{
						flag = false;
						num = i;
					}
					break;
				}
				IL_98:
				i++;
				continue;
				IL_5F:
				if (!flag)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(list.Substring(num, i - num));
				}
				flag = true;
				goto IL_98;
			}
			if (!flag && num == 0)
			{
				return new string[]
				{
					list
				};
			}
			if (!flag && num < list.Length)
			{
				arrayList.Add((num != 0) ? list.Substring(num) : list);
			}
			return arrayList.ToArray(typeof(string)) as string[];
		}

		public static void ReadUnhandledAttribute(XmlReader reader, XmlSchemaObject xso)
		{
			if (reader.Prefix == "xmlns")
			{
				xso.Namespaces.Add(reader.LocalName, reader.Value);
			}
			else if (reader.Name == "xmlns")
			{
				xso.Namespaces.Add(string.Empty, reader.Value);
			}
			else
			{
				if (xso.unhandledAttributeList == null)
				{
					xso.unhandledAttributeList = new ArrayList();
				}
				XmlAttribute xmlAttribute = new XmlDocument().CreateAttribute(reader.LocalName, reader.NamespaceURI);
				xmlAttribute.Value = reader.Value;
				XmlSchemaUtil.ParseWsdlArrayType(reader, xmlAttribute);
				xso.unhandledAttributeList.Add(xmlAttribute);
			}
		}

		private static void ParseWsdlArrayType(XmlReader reader, XmlAttribute attr)
		{
			if (attr.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && attr.LocalName == "arrayType")
			{
				string text = string.Empty;
				string str;
				string str2;
				TypeTranslator.ParseArrayType(attr.Value, out str, out text, out str2);
				if (text != string.Empty)
				{
					text = reader.LookupNamespace(text) + ":";
				}
				attr.Value = text + str + str2;
			}
		}

		public static bool ReadBoolAttribute(XmlReader reader, out Exception innerExcpetion)
		{
			innerExcpetion = null;
			bool result;
			try
			{
				bool flag = XmlConvert.ToBoolean(reader.Value);
				result = flag;
			}
			catch (Exception ex)
			{
				innerExcpetion = ex;
				result = false;
			}
			return result;
		}

		public static decimal ReadDecimalAttribute(XmlReader reader, out Exception innerExcpetion)
		{
			innerExcpetion = null;
			decimal result;
			try
			{
				decimal num = XmlConvert.ToDecimal(reader.Value);
				result = num;
			}
			catch (Exception ex)
			{
				innerExcpetion = ex;
				result = 0m;
			}
			return result;
		}

		public static XmlSchemaDerivationMethod ReadDerivationAttribute(XmlReader reader, out Exception innerExcpetion, string name, XmlSchemaDerivationMethod allowed)
		{
			innerExcpetion = null;
			XmlSchemaDerivationMethod result;
			try
			{
				string value = reader.Value;
				string text = string.Empty;
				XmlSchemaDerivationMethod xmlSchemaDerivationMethod = XmlSchemaDerivationMethod.Empty;
				if (value.IndexOf("#all") != -1 && value.Trim() != "#all")
				{
					innerExcpetion = new Exception(value + " is not a valid value for " + name + ". #all if present must be the only value");
					result = XmlSchemaDerivationMethod.All;
				}
				else
				{
					string[] array = XmlSchemaUtil.SplitList(value);
					int i = 0;
					while (i < array.Length)
					{
						string text2 = array[i];
						string text3 = text2;
						if (text3 == null)
						{
							goto IL_192;
						}
						if (XmlSchemaUtil.<>f__switch$map38 == null)
						{
							XmlSchemaUtil.<>f__switch$map38 = new Dictionary<string, int>(7)
							{
								{
									string.Empty,
									0
								},
								{
									"#all",
									1
								},
								{
									"substitution",
									2
								},
								{
									"extension",
									3
								},
								{
									"restriction",
									4
								},
								{
									"list",
									5
								},
								{
									"union",
									6
								}
							};
						}
						int num;
						if (!XmlSchemaUtil.<>f__switch$map38.TryGetValue(text3, out num))
						{
							goto IL_192;
						}
						switch (num)
						{
						case 0:
							xmlSchemaDerivationMethod = XmlSchemaUtil.AddFlag(xmlSchemaDerivationMethod, XmlSchemaDerivationMethod.Empty, allowed);
							break;
						case 1:
							xmlSchemaDerivationMethod = XmlSchemaUtil.AddFlag(xmlSchemaDerivationMethod, XmlSchemaDerivationMethod.All, allowed);
							break;
						case 2:
							xmlSchemaDerivationMethod = XmlSchemaUtil.AddFlag(xmlSchemaDerivationMethod, XmlSchemaDerivationMethod.Substitution, allowed);
							break;
						case 3:
							xmlSchemaDerivationMethod = XmlSchemaUtil.AddFlag(xmlSchemaDerivationMethod, XmlSchemaDerivationMethod.Extension, allowed);
							break;
						case 4:
							xmlSchemaDerivationMethod = XmlSchemaUtil.AddFlag(xmlSchemaDerivationMethod, XmlSchemaDerivationMethod.Restriction, allowed);
							break;
						case 5:
							xmlSchemaDerivationMethod = XmlSchemaUtil.AddFlag(xmlSchemaDerivationMethod, XmlSchemaDerivationMethod.List, allowed);
							break;
						case 6:
							xmlSchemaDerivationMethod = XmlSchemaUtil.AddFlag(xmlSchemaDerivationMethod, XmlSchemaDerivationMethod.Union, allowed);
							break;
						default:
							goto IL_192;
						}
						IL_1A4:
						i++;
						continue;
						IL_192:
						text = text + text2 + " ";
						goto IL_1A4;
					}
					if (text != string.Empty)
					{
						innerExcpetion = new Exception(text + "is/are not valid values for " + name);
					}
					result = xmlSchemaDerivationMethod;
				}
			}
			catch (Exception ex)
			{
				innerExcpetion = ex;
				result = XmlSchemaDerivationMethod.None;
			}
			return result;
		}

		private static XmlSchemaDerivationMethod AddFlag(XmlSchemaDerivationMethod dst, XmlSchemaDerivationMethod add, XmlSchemaDerivationMethod allowed)
		{
			if ((add & allowed) == XmlSchemaDerivationMethod.Empty && allowed != XmlSchemaDerivationMethod.All)
			{
				throw new ArgumentException(add + " is not allowed in this attribute.");
			}
			if ((dst & add) != XmlSchemaDerivationMethod.Empty)
			{
				throw new ArgumentException(add + " is already specified in this attribute.");
			}
			return dst | add;
		}

		public static XmlSchemaForm ReadFormAttribute(XmlReader reader, out Exception innerExcpetion)
		{
			innerExcpetion = null;
			XmlSchemaForm result = XmlSchemaForm.None;
			string value = reader.Value;
			if (value != null)
			{
				if (XmlSchemaUtil.<>f__switch$map39 == null)
				{
					XmlSchemaUtil.<>f__switch$map39 = new Dictionary<string, int>(2)
					{
						{
							"qualified",
							0
						},
						{
							"unqualified",
							1
						}
					};
				}
				int num;
				if (XmlSchemaUtil.<>f__switch$map39.TryGetValue(value, out num))
				{
					if (num == 0)
					{
						return XmlSchemaForm.Qualified;
					}
					if (num == 1)
					{
						return XmlSchemaForm.Unqualified;
					}
				}
			}
			innerExcpetion = new Exception("only qualified or unqulified is a valid value");
			return result;
		}

		public static XmlSchemaContentProcessing ReadProcessingAttribute(XmlReader reader, out Exception innerExcpetion)
		{
			innerExcpetion = null;
			XmlSchemaContentProcessing result = XmlSchemaContentProcessing.None;
			string value = reader.Value;
			switch (value)
			{
			case "lax":
				return XmlSchemaContentProcessing.Lax;
			case "strict":
				return XmlSchemaContentProcessing.Strict;
			case "skip":
				return XmlSchemaContentProcessing.Skip;
			}
			innerExcpetion = new Exception("only lax , strict or skip are valid values for processContents");
			return result;
		}

		public static XmlSchemaUse ReadUseAttribute(XmlReader reader, out Exception innerExcpetion)
		{
			innerExcpetion = null;
			XmlSchemaUse result = XmlSchemaUse.None;
			string value = reader.Value;
			switch (value)
			{
			case "optional":
				return XmlSchemaUse.Optional;
			case "prohibited":
				return XmlSchemaUse.Prohibited;
			case "required":
				return XmlSchemaUse.Required;
			}
			innerExcpetion = new Exception("only optional , prohibited or required are valid values for use");
			return result;
		}

		public static XmlQualifiedName ReadQNameAttribute(XmlReader reader, out Exception innerEx)
		{
			return XmlSchemaUtil.ToQName(reader, reader.Value, out innerEx);
		}

		public static XmlQualifiedName ToQName(XmlReader reader, string qnamestr, out Exception innerEx)
		{
			innerEx = null;
			if (!XmlSchemaUtil.IsValidQName(qnamestr))
			{
				innerEx = new Exception(qnamestr + " is an invalid QName. Either name or namespace is not a NCName");
				return XmlQualifiedName.Empty;
			}
			string[] array = qnamestr.Split(new char[]
			{
				':'
			}, 2);
			string text;
			string name;
			if (array.Length == 2)
			{
				text = reader.LookupNamespace(array[0]);
				if (text == null)
				{
					innerEx = new Exception("Namespace Prefix '" + array[0] + "could not be resolved");
					return XmlQualifiedName.Empty;
				}
				name = array[1];
			}
			else
			{
				text = reader.LookupNamespace(string.Empty);
				name = array[0];
			}
			return new XmlQualifiedName(name, text);
		}

		public static int ValidateAttributesResolved(XmlSchemaObjectTable attributesResolved, ValidationEventHandler h, XmlSchema schema, XmlSchemaObjectCollection attributes, XmlSchemaAnyAttribute anyAttribute, ref XmlSchemaAnyAttribute anyAttributeUse, XmlSchemaAttributeGroup redefined, bool skipEquivalent)
		{
			int num = 0;
			if (anyAttribute != null && anyAttributeUse == null)
			{
				anyAttributeUse = anyAttribute;
			}
			ArrayList arrayList = new ArrayList();
			foreach (XmlSchemaObject xmlSchemaObject in attributes)
			{
				XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = xmlSchemaObject as XmlSchemaAttributeGroupRef;
				if (xmlSchemaAttributeGroupRef != null)
				{
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup = null;
					if (redefined != null && xmlSchemaAttributeGroupRef.RefName == redefined.QualifiedName)
					{
						xmlSchemaAttributeGroup = redefined;
					}
					else
					{
						xmlSchemaAttributeGroup = schema.FindAttributeGroup(xmlSchemaAttributeGroupRef.RefName);
					}
					if (xmlSchemaAttributeGroup == null)
					{
						if (!schema.missedSubComponents)
						{
							xmlSchemaAttributeGroupRef.error(h, "Referenced attribute group " + xmlSchemaAttributeGroupRef.RefName + " was not found in the corresponding schema.");
						}
					}
					else if (xmlSchemaAttributeGroup.AttributeGroupRecursionCheck)
					{
						xmlSchemaAttributeGroup.error(h, "Attribute group recursion was found: " + xmlSchemaAttributeGroupRef.RefName);
					}
					else
					{
						try
						{
							xmlSchemaAttributeGroup.AttributeGroupRecursionCheck = true;
							num += xmlSchemaAttributeGroup.Validate(h, schema);
						}
						finally
						{
							xmlSchemaAttributeGroup.AttributeGroupRecursionCheck = false;
						}
						if (xmlSchemaAttributeGroup.AnyAttributeUse != null && anyAttribute == null)
						{
							anyAttributeUse = xmlSchemaAttributeGroup.AnyAttributeUse;
						}
						foreach (object obj in xmlSchemaAttributeGroup.AttributeUses)
						{
							XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)((DictionaryEntry)obj).Value;
							if (!XmlSchemaUtil.StrictMsCompliant || xmlSchemaAttribute.Use != XmlSchemaUse.Prohibited)
							{
								if (xmlSchemaAttribute.RefName != null && xmlSchemaAttribute.RefName != XmlQualifiedName.Empty && (!skipEquivalent || !XmlSchemaUtil.AreAttributesEqual(xmlSchemaAttribute, attributesResolved[xmlSchemaAttribute.RefName] as XmlSchemaAttribute)))
								{
									XmlSchemaUtil.AddToTable(attributesResolved, xmlSchemaAttribute, xmlSchemaAttribute.RefName, h);
								}
								else if (!skipEquivalent || !XmlSchemaUtil.AreAttributesEqual(xmlSchemaAttribute, attributesResolved[xmlSchemaAttribute.QualifiedName] as XmlSchemaAttribute))
								{
									XmlSchemaUtil.AddToTable(attributesResolved, xmlSchemaAttribute, xmlSchemaAttribute.QualifiedName, h);
								}
							}
						}
					}
				}
				else
				{
					XmlSchemaAttribute xmlSchemaAttribute2 = xmlSchemaObject as XmlSchemaAttribute;
					if (xmlSchemaAttribute2 != null)
					{
						num += xmlSchemaAttribute2.Validate(h, schema);
						if (arrayList.Contains(xmlSchemaAttribute2.QualifiedName))
						{
							xmlSchemaAttribute2.error(h, string.Format("Duplicate attributes was found for '{0}'", xmlSchemaAttribute2.QualifiedName));
						}
						arrayList.Add(xmlSchemaAttribute2.QualifiedName);
						if (!XmlSchemaUtil.StrictMsCompliant || xmlSchemaAttribute2.Use != XmlSchemaUse.Prohibited)
						{
							if (xmlSchemaAttribute2.RefName != null && xmlSchemaAttribute2.RefName != XmlQualifiedName.Empty && (!skipEquivalent || !XmlSchemaUtil.AreAttributesEqual(xmlSchemaAttribute2, attributesResolved[xmlSchemaAttribute2.RefName] as XmlSchemaAttribute)))
							{
								XmlSchemaUtil.AddToTable(attributesResolved, xmlSchemaAttribute2, xmlSchemaAttribute2.RefName, h);
							}
							else if (!skipEquivalent || !XmlSchemaUtil.AreAttributesEqual(xmlSchemaAttribute2, attributesResolved[xmlSchemaAttribute2.QualifiedName] as XmlSchemaAttribute))
							{
								XmlSchemaUtil.AddToTable(attributesResolved, xmlSchemaAttribute2, xmlSchemaAttribute2.QualifiedName, h);
							}
						}
					}
					else if (anyAttribute == null)
					{
						anyAttributeUse = (XmlSchemaAnyAttribute)xmlSchemaObject;
						anyAttribute.Validate(h, schema);
					}
				}
			}
			return num;
		}

		internal static bool AreAttributesEqual(XmlSchemaAttribute one, XmlSchemaAttribute another)
		{
			return one != null && another != null && (one.AttributeType == another.AttributeType && one.Form == another.Form && one.ValidatedUse == another.ValidatedUse && one.ValidatedDefaultValue == another.ValidatedDefaultValue) && one.ValidatedFixedValue == another.ValidatedFixedValue;
		}

		public static object ReadTypedValue(XmlReader reader, object type, IXmlNamespaceResolver nsResolver, StringBuilder tmpBuilder)
		{
			if (tmpBuilder == null)
			{
				tmpBuilder = new StringBuilder();
			}
			XmlSchemaDatatype xmlSchemaDatatype = type as XmlSchemaDatatype;
			XmlSchemaSimpleType xmlSchemaSimpleType = type as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType != null)
			{
				xmlSchemaDatatype = xmlSchemaSimpleType.Datatype;
			}
			if (xmlSchemaDatatype == null)
			{
				return null;
			}
			XmlNodeType nodeType = reader.NodeType;
			if (nodeType != XmlNodeType.Element)
			{
				if (nodeType != XmlNodeType.Attribute)
				{
					return null;
				}
				return xmlSchemaDatatype.ParseValue(reader.Value, reader.NameTable, nsResolver);
			}
			else
			{
				if (reader.IsEmptyElement)
				{
					return null;
				}
				tmpBuilder.Length = 0;
				bool flag = true;
				for (;;)
				{
					reader.Read();
					XmlNodeType nodeType2 = reader.NodeType;
					switch (nodeType2)
					{
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
						goto IL_9E;
					default:
						if (nodeType2 == XmlNodeType.SignificantWhitespace)
						{
							goto IL_9E;
						}
						flag = false;
						break;
					case XmlNodeType.Comment:
						break;
					}
					IL_BC:
					if (!flag || reader.EOF || reader.ReadState != ReadState.Interactive)
					{
						break;
					}
					continue;
					IL_9E:
					tmpBuilder.Append(reader.Value);
					goto IL_BC;
				}
				return xmlSchemaDatatype.ParseValue(tmpBuilder.ToString(), reader.NameTable, nsResolver);
			}
		}

		public static XmlSchemaObject FindAttributeDeclaration(string ns, XmlSchemaSet schemas, XmlSchemaComplexType cType, XmlQualifiedName qname)
		{
			XmlSchemaObject xmlSchemaObject = cType.AttributeUses[qname];
			if (xmlSchemaObject != null)
			{
				return xmlSchemaObject;
			}
			if (cType.AttributeWildcard == null)
			{
				return null;
			}
			if (!XmlSchemaUtil.AttributeWildcardItemValid(cType.AttributeWildcard, qname, ns))
			{
				return null;
			}
			if (cType.AttributeWildcard.ResolvedProcessContents == XmlSchemaContentProcessing.Skip)
			{
				return cType.AttributeWildcard;
			}
			XmlSchemaAttribute xmlSchemaAttribute = schemas.GlobalAttributes[qname] as XmlSchemaAttribute;
			if (xmlSchemaAttribute != null)
			{
				return xmlSchemaAttribute;
			}
			if (cType.AttributeWildcard.ResolvedProcessContents == XmlSchemaContentProcessing.Lax)
			{
				return cType.AttributeWildcard;
			}
			return null;
		}

		private static bool AttributeWildcardItemValid(XmlSchemaAnyAttribute anyAttr, XmlQualifiedName qname, string ns)
		{
			if (anyAttr.HasValueAny)
			{
				return true;
			}
			if (anyAttr.HasValueOther && (anyAttr.TargetNamespace == string.Empty || ns != anyAttr.TargetNamespace))
			{
				return true;
			}
			if (anyAttr.HasValueTargetNamespace && ns == anyAttr.TargetNamespace)
			{
				return true;
			}
			if (anyAttr.HasValueLocal && ns == string.Empty)
			{
				return true;
			}
			for (int i = 0; i < anyAttr.ResolvedNamespaces.Count; i++)
			{
				if (anyAttr.ResolvedNamespaces[i] == ns)
				{
					return true;
				}
			}
			return false;
		}
	}
}
