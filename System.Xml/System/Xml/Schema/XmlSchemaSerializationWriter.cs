using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	internal class XmlSchemaSerializationWriter : XmlSerializationWriter
	{
		private const string xmlNamespace = "http://www.w3.org/2000/xmlns/";

		public void WriteRoot_XmlSchema(object o)
		{
			base.WriteStartDocument();
			XmlSchema ob = (XmlSchema)o;
			base.TopLevelElement();
			this.WriteObject_XmlSchema(ob, "schema", "http://www.w3.org/2001/XMLSchema", true, false, true);
		}

		private void WriteObject_XmlSchema(XmlSchema ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchema))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchema", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				if (ob.AttributeFormDefault != XmlSchemaForm.None)
				{
					base.WriteAttribute("attributeFormDefault", string.Empty, this.GetEnumValue_XmlSchemaForm(ob.AttributeFormDefault));
				}
				if (ob.BlockDefault != XmlSchemaDerivationMethod.None)
				{
					base.WriteAttribute("blockDefault", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.BlockDefault));
				}
				if (ob.FinalDefault != XmlSchemaDerivationMethod.None)
				{
					base.WriteAttribute("finalDefault", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.FinalDefault));
				}
				if (ob.ElementFormDefault != XmlSchemaForm.None)
				{
					base.WriteAttribute("elementFormDefault", string.Empty, this.GetEnumValue_XmlSchemaForm(ob.ElementFormDefault));
				}
				base.WriteAttribute("targetNamespace", string.Empty, ob.TargetNamespace);
				base.WriteAttribute("version", string.Empty, ob.Version);
				base.WriteAttribute("id", string.Empty, ob.Id);
				if (ob.Includes != null)
				{
					for (int i = 0; i < ob.Includes.Count; i++)
					{
						if (ob.Includes[i] != null)
						{
							if (ob.Includes[i].GetType() == typeof(XmlSchemaInclude))
							{
								this.WriteObject_XmlSchemaInclude((XmlSchemaInclude)ob.Includes[i], "include", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Includes[i].GetType() == typeof(XmlSchemaImport))
							{
								this.WriteObject_XmlSchemaImport((XmlSchemaImport)ob.Includes[i], "import", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Includes[i].GetType() != typeof(XmlSchemaRedefine))
								{
									throw base.CreateUnknownTypeException(ob.Includes[i]);
								}
								this.WriteObject_XmlSchemaRedefine((XmlSchemaRedefine)ob.Includes[i], "redefine", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				if (ob.Items != null)
				{
					for (int j = 0; j < ob.Items.Count; j++)
					{
						if (ob.Items[j] != null)
						{
							if (ob.Items[j].GetType() == typeof(XmlSchemaElement))
							{
								this.WriteObject_XmlSchemaElement((XmlSchemaElement)ob.Items[j], "element", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[j].GetType() == typeof(XmlSchemaSimpleType))
							{
								this.WriteObject_XmlSchemaSimpleType((XmlSchemaSimpleType)ob.Items[j], "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[j].GetType() == typeof(XmlSchemaAttribute))
							{
								this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute)ob.Items[j], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[j].GetType() == typeof(XmlSchemaAnnotation))
							{
								this.WriteObject_XmlSchemaAnnotation((XmlSchemaAnnotation)ob.Items[j], "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[j].GetType() == typeof(XmlSchemaAttributeGroup))
							{
								this.WriteObject_XmlSchemaAttributeGroup((XmlSchemaAttributeGroup)ob.Items[j], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[j].GetType() == typeof(XmlSchemaGroup))
							{
								this.WriteObject_XmlSchemaGroup((XmlSchemaGroup)ob.Items[j], "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[j].GetType() == typeof(XmlSchemaComplexType))
							{
								this.WriteObject_XmlSchemaComplexType((XmlSchemaComplexType)ob.Items[j], "complexType", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Items[j].GetType() != typeof(XmlSchemaNotation))
								{
									throw base.CreateUnknownTypeException(ob.Items[j]);
								}
								this.WriteObject_XmlSchemaNotation((XmlSchemaNotation)ob.Items[j], "notation", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private string GetEnumValue_XmlSchemaForm(XmlSchemaForm val)
		{
			if (val == XmlSchemaForm.Qualified)
			{
				return "qualified";
			}
			if (val != XmlSchemaForm.Unqualified)
			{
				return ((long)val).ToString(CultureInfo.InvariantCulture);
			}
			return "unqualified";
		}

		private string GetEnumValue_XmlSchemaDerivationMethod(XmlSchemaDerivationMethod val)
		{
			switch (val)
			{
			case XmlSchemaDerivationMethod.Empty:
				return string.Empty;
			case XmlSchemaDerivationMethod.Substitution:
				return "substitution";
			case XmlSchemaDerivationMethod.Extension:
				return "extension";
			default:
				if (val == XmlSchemaDerivationMethod.Union)
				{
					return "union";
				}
				if (val != XmlSchemaDerivationMethod.All)
				{
					StringBuilder stringBuilder = new StringBuilder();
					string[] array = val.ToString().Split(new char[]
					{
						','
					});
					string[] array2 = array;
					int i = 0;
					while (i < array2.Length)
					{
						string text = array2[i];
						string text2 = text.Trim();
						if (text2 == null)
						{
							goto IL_209;
						}
						if (XmlSchemaSerializationWriter.<>f__switch$map35 == null)
						{
							XmlSchemaSerializationWriter.<>f__switch$map35 = new Dictionary<string, int>(7)
							{
								{
									"Empty",
									0
								},
								{
									"Substitution",
									1
								},
								{
									"Extension",
									2
								},
								{
									"Restriction",
									3
								},
								{
									"List",
									4
								},
								{
									"Union",
									5
								},
								{
									"All",
									6
								}
							};
						}
						int num;
						if (!XmlSchemaSerializationWriter.<>f__switch$map35.TryGetValue(text2, out num))
						{
							goto IL_209;
						}
						switch (num)
						{
						case 0:
							stringBuilder.Append(string.Empty).Append(' ');
							break;
						case 1:
							stringBuilder.Append("substitution").Append(' ');
							break;
						case 2:
							stringBuilder.Append("extension").Append(' ');
							break;
						case 3:
							stringBuilder.Append("restriction").Append(' ');
							break;
						case 4:
							stringBuilder.Append("list").Append(' ');
							break;
						case 5:
							stringBuilder.Append("union").Append(' ');
							break;
						case 6:
							stringBuilder.Append("#all").Append(' ');
							break;
						default:
							goto IL_209;
						}
						IL_222:
						i++;
						continue;
						IL_209:
						stringBuilder.Append(text.Trim()).Append(' ');
						goto IL_222;
					}
					return stringBuilder.ToString().Trim();
				}
				return "#all";
			case XmlSchemaDerivationMethod.Restriction:
				return "restriction";
			case XmlSchemaDerivationMethod.List:
				return "list";
			}
		}

		private void WriteObject_XmlSchemaInclude(XmlSchemaInclude ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaInclude))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaInclude", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("schemaLocation", string.Empty, ob.SchemaLocation);
				base.WriteAttribute("id", string.Empty, ob.Id);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaImport(XmlSchemaImport ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaImport))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaImport", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("schemaLocation", string.Empty, ob.SchemaLocation);
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("namespace", string.Empty, ob.Namespace);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaRedefine(XmlSchemaRedefine ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaRedefine))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaRedefine", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("schemaLocation", string.Empty, ob.SchemaLocation);
				base.WriteAttribute("id", string.Empty, ob.Id);
				if (ob.Items != null)
				{
					for (int i = 0; i < ob.Items.Count; i++)
					{
						if (ob.Items[i] != null)
						{
							if (ob.Items[i].GetType() == typeof(XmlSchemaGroup))
							{
								this.WriteObject_XmlSchemaGroup((XmlSchemaGroup)ob.Items[i], "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[i].GetType() == typeof(XmlSchemaComplexType))
							{
								this.WriteObject_XmlSchemaComplexType((XmlSchemaComplexType)ob.Items[i], "complexType", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[i].GetType() == typeof(XmlSchemaSimpleType))
							{
								this.WriteObject_XmlSchemaSimpleType((XmlSchemaSimpleType)ob.Items[i], "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[i].GetType() == typeof(XmlSchemaAnnotation))
							{
								this.WriteObject_XmlSchemaAnnotation((XmlSchemaAnnotation)ob.Items[i], "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Items[i].GetType() != typeof(XmlSchemaAttributeGroup))
								{
									throw base.CreateUnknownTypeException(ob.Items[i]);
								}
								this.WriteObject_XmlSchemaAttributeGroup((XmlSchemaAttributeGroup)ob.Items[i], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaElement(XmlSchemaElement ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaElement))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaElement", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
				base.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
				if (ob.IsAbstract)
				{
					base.WriteAttribute("abstract", string.Empty, (!ob.IsAbstract) ? "false" : "true");
				}
				if (ob.Block != XmlSchemaDerivationMethod.None)
				{
					base.WriteAttribute("block", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.Block));
				}
				if (ob.DefaultValue != null)
				{
					base.WriteAttribute("default", string.Empty, ob.DefaultValue);
				}
				if (ob.Final != XmlSchemaDerivationMethod.None)
				{
					base.WriteAttribute("final", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.Final));
				}
				if (ob.FixedValue != null)
				{
					base.WriteAttribute("fixed", string.Empty, ob.FixedValue);
				}
				if (ob.Form != XmlSchemaForm.None)
				{
					base.WriteAttribute("form", string.Empty, this.GetEnumValue_XmlSchemaForm(ob.Form));
				}
				if (ob.Name != null)
				{
					base.WriteAttribute("name", string.Empty, ob.Name);
				}
				if (ob.IsNillable)
				{
					base.WriteAttribute("nillable", string.Empty, (!ob.IsNillable) ? "false" : "true");
				}
				base.WriteAttribute("ref", string.Empty, base.FromXmlQualifiedName(ob.RefName));
				base.WriteAttribute("substitutionGroup", string.Empty, base.FromXmlQualifiedName(ob.SubstitutionGroup));
				base.WriteAttribute("type", string.Empty, base.FromXmlQualifiedName(ob.SchemaTypeName));
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.SchemaType is XmlSchemaSimpleType)
				{
					this.WriteObject_XmlSchemaSimpleType((XmlSchemaSimpleType)ob.SchemaType, "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.SchemaType is XmlSchemaComplexType)
				{
					this.WriteObject_XmlSchemaComplexType((XmlSchemaComplexType)ob.SchemaType, "complexType", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				if (ob.Constraints != null)
				{
					for (int i = 0; i < ob.Constraints.Count; i++)
					{
						if (ob.Constraints[i] != null)
						{
							if (ob.Constraints[i].GetType() == typeof(XmlSchemaKeyref))
							{
								this.WriteObject_XmlSchemaKeyref((XmlSchemaKeyref)ob.Constraints[i], "keyref", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Constraints[i].GetType() == typeof(XmlSchemaKey))
							{
								this.WriteObject_XmlSchemaKey((XmlSchemaKey)ob.Constraints[i], "key", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Constraints[i].GetType() != typeof(XmlSchemaUnique))
								{
									throw base.CreateUnknownTypeException(ob.Constraints[i]);
								}
								this.WriteObject_XmlSchemaUnique((XmlSchemaUnique)ob.Constraints[i], "unique", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaSimpleType(XmlSchemaSimpleType ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaSimpleType))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaSimpleType", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("name", string.Empty, ob.Name);
				if (ob.Final != XmlSchemaDerivationMethod.None)
				{
					base.WriteAttribute("final", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.Final));
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Content is XmlSchemaSimpleTypeUnion)
				{
					this.WriteObject_XmlSchemaSimpleTypeUnion((XmlSchemaSimpleTypeUnion)ob.Content, "union", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Content is XmlSchemaSimpleTypeList)
				{
					this.WriteObject_XmlSchemaSimpleTypeList((XmlSchemaSimpleTypeList)ob.Content, "list", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Content is XmlSchemaSimpleTypeRestriction)
				{
					this.WriteObject_XmlSchemaSimpleTypeRestriction((XmlSchemaSimpleTypeRestriction)ob.Content, "restriction", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaAttribute(XmlSchemaAttribute ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaAttribute))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaAttribute", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				if (ob.DefaultValue != null)
				{
					base.WriteAttribute("default", string.Empty, ob.DefaultValue);
				}
				if (ob.FixedValue != null)
				{
					base.WriteAttribute("fixed", string.Empty, ob.FixedValue);
				}
				if (ob.Form != XmlSchemaForm.None)
				{
					base.WriteAttribute("form", string.Empty, this.GetEnumValue_XmlSchemaForm(ob.Form));
				}
				base.WriteAttribute("name", string.Empty, ob.Name);
				base.WriteAttribute("ref", string.Empty, base.FromXmlQualifiedName(ob.RefName));
				base.WriteAttribute("type", string.Empty, base.FromXmlQualifiedName(ob.SchemaTypeName));
				if (ob.Use != XmlSchemaUse.None)
				{
					base.WriteAttribute("use", string.Empty, this.GetEnumValue_XmlSchemaUse(ob.Use));
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				this.WriteObject_XmlSchemaSimpleType(ob.SchemaType, "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaAnnotation(XmlSchemaAnnotation ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaAnnotation))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaAnnotation", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				if (ob.Items != null)
				{
					for (int i = 0; i < ob.Items.Count; i++)
					{
						if (ob.Items[i] != null)
						{
							if (ob.Items[i].GetType() == typeof(XmlSchemaAppInfo))
							{
								this.WriteObject_XmlSchemaAppInfo((XmlSchemaAppInfo)ob.Items[i], "appinfo", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Items[i].GetType() != typeof(XmlSchemaDocumentation))
								{
									throw base.CreateUnknownTypeException(ob.Items[i]);
								}
								this.WriteObject_XmlSchemaDocumentation((XmlSchemaDocumentation)ob.Items[i], "documentation", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaAttributeGroup(XmlSchemaAttributeGroup ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaAttributeGroup))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaAttributeGroup", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("name", string.Empty, ob.Name);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Attributes != null)
				{
					for (int i = 0; i < ob.Attributes.Count; i++)
					{
						if (ob.Attributes[i] != null)
						{
							if (ob.Attributes[i].GetType() == typeof(XmlSchemaAttribute))
							{
								this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute)ob.Attributes[i], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Attributes[i].GetType() != typeof(XmlSchemaAttributeGroupRef))
								{
									throw base.CreateUnknownTypeException(ob.Attributes[i]);
								}
								this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef)ob.Attributes[i], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaGroup(XmlSchemaGroup ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaGroup))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaGroup", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("name", string.Empty, ob.Name);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Particle is XmlSchemaSequence)
				{
					this.WriteObject_XmlSchemaSequence((XmlSchemaSequence)ob.Particle, "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Particle is XmlSchemaChoice)
				{
					this.WriteObject_XmlSchemaChoice((XmlSchemaChoice)ob.Particle, "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Particle is XmlSchemaAll)
				{
					this.WriteObject_XmlSchemaAll((XmlSchemaAll)ob.Particle, "all", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaComplexType(XmlSchemaComplexType ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaComplexType))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaComplexType", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("name", string.Empty, ob.Name);
				if (ob.Final != XmlSchemaDerivationMethod.None)
				{
					base.WriteAttribute("final", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.Final));
				}
				if (ob.IsAbstract)
				{
					base.WriteAttribute("abstract", string.Empty, (!ob.IsAbstract) ? "false" : "true");
				}
				if (ob.Block != XmlSchemaDerivationMethod.None)
				{
					base.WriteAttribute("block", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.Block));
				}
				if (ob.IsMixed)
				{
					base.WriteAttribute("mixed", string.Empty, (!ob.IsMixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.ContentModel is XmlSchemaComplexContent)
				{
					this.WriteObject_XmlSchemaComplexContent((XmlSchemaComplexContent)ob.ContentModel, "complexContent", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.ContentModel is XmlSchemaSimpleContent)
				{
					this.WriteObject_XmlSchemaSimpleContent((XmlSchemaSimpleContent)ob.ContentModel, "simpleContent", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				if (ob.Particle is XmlSchemaAll)
				{
					this.WriteObject_XmlSchemaAll((XmlSchemaAll)ob.Particle, "all", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Particle is XmlSchemaGroupRef)
				{
					this.WriteObject_XmlSchemaGroupRef((XmlSchemaGroupRef)ob.Particle, "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Particle is XmlSchemaSequence)
				{
					this.WriteObject_XmlSchemaSequence((XmlSchemaSequence)ob.Particle, "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Particle is XmlSchemaChoice)
				{
					this.WriteObject_XmlSchemaChoice((XmlSchemaChoice)ob.Particle, "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				if (ob.Attributes != null)
				{
					for (int i = 0; i < ob.Attributes.Count; i++)
					{
						if (ob.Attributes[i] != null)
						{
							if (ob.Attributes[i].GetType() == typeof(XmlSchemaAttributeGroupRef))
							{
								this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef)ob.Attributes[i], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Attributes[i].GetType() != typeof(XmlSchemaAttribute))
								{
									throw base.CreateUnknownTypeException(ob.Attributes[i]);
								}
								this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute)ob.Attributes[i], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaNotation(XmlSchemaNotation ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaNotation))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaNotation", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("name", string.Empty, ob.Name);
				base.WriteAttribute("public", string.Empty, ob.Public);
				base.WriteAttribute("system", string.Empty, ob.System);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaKeyref(XmlSchemaKeyref ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaKeyref))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaKeyref", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("name", string.Empty, ob.Name);
				base.WriteAttribute("refer", string.Empty, base.FromXmlQualifiedName(ob.Refer));
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				this.WriteObject_XmlSchemaXPath(ob.Selector, "selector", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Fields != null)
				{
					for (int i = 0; i < ob.Fields.Count; i++)
					{
						this.WriteObject_XmlSchemaXPath((XmlSchemaXPath)ob.Fields[i], "field", "http://www.w3.org/2001/XMLSchema", false, false, true);
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaKey(XmlSchemaKey ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaKey))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaKey", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("name", string.Empty, ob.Name);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				this.WriteObject_XmlSchemaXPath(ob.Selector, "selector", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Fields != null)
				{
					for (int i = 0; i < ob.Fields.Count; i++)
					{
						this.WriteObject_XmlSchemaXPath((XmlSchemaXPath)ob.Fields[i], "field", "http://www.w3.org/2001/XMLSchema", false, false, true);
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaUnique(XmlSchemaUnique ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaUnique))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaUnique", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("name", string.Empty, ob.Name);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				this.WriteObject_XmlSchemaXPath(ob.Selector, "selector", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Fields != null)
				{
					for (int i = 0; i < ob.Fields.Count; i++)
					{
						this.WriteObject_XmlSchemaXPath((XmlSchemaXPath)ob.Fields[i], "field", "http://www.w3.org/2001/XMLSchema", false, false, true);
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaSimpleTypeUnion(XmlSchemaSimpleTypeUnion ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaSimpleTypeUnion))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaSimpleTypeUnion", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				string value = null;
				if (ob.MemberTypes != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < ob.MemberTypes.Length; i++)
					{
						stringBuilder.Append(base.FromXmlQualifiedName(ob.MemberTypes[i])).Append(" ");
					}
					value = stringBuilder.ToString().Trim();
				}
				base.WriteAttribute("memberTypes", string.Empty, value);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.BaseTypes != null)
				{
					for (int j = 0; j < ob.BaseTypes.Count; j++)
					{
						this.WriteObject_XmlSchemaSimpleType((XmlSchemaSimpleType)ob.BaseTypes[j], "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaSimpleTypeList(XmlSchemaSimpleTypeList ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaSimpleTypeList))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaSimpleTypeList", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("itemType", string.Empty, base.FromXmlQualifiedName(ob.ItemTypeName));
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				this.WriteObject_XmlSchemaSimpleType(ob.ItemType, "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaSimpleTypeRestriction(XmlSchemaSimpleTypeRestriction ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaSimpleTypeRestriction))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaSimpleTypeRestriction", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("base", string.Empty, base.FromXmlQualifiedName(ob.BaseTypeName));
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				this.WriteObject_XmlSchemaSimpleType(ob.BaseType, "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Facets != null)
				{
					for (int i = 0; i < ob.Facets.Count; i++)
					{
						if (ob.Facets[i] != null)
						{
							if (ob.Facets[i].GetType() == typeof(XmlSchemaMaxLengthFacet))
							{
								this.WriteObject_XmlSchemaMaxLengthFacet((XmlSchemaMaxLengthFacet)ob.Facets[i], "maxLength", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaMinLengthFacet))
							{
								this.WriteObject_XmlSchemaMinLengthFacet((XmlSchemaMinLengthFacet)ob.Facets[i], "minLength", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaLengthFacet))
							{
								this.WriteObject_XmlSchemaLengthFacet((XmlSchemaLengthFacet)ob.Facets[i], "length", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaFractionDigitsFacet))
							{
								this.WriteObject_XmlSchemaFractionDigitsFacet((XmlSchemaFractionDigitsFacet)ob.Facets[i], "fractionDigits", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaMaxInclusiveFacet))
							{
								this.WriteObject_XmlSchemaMaxInclusiveFacet((XmlSchemaMaxInclusiveFacet)ob.Facets[i], "maxInclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaMaxExclusiveFacet))
							{
								this.WriteObject_XmlSchemaMaxExclusiveFacet((XmlSchemaMaxExclusiveFacet)ob.Facets[i], "maxExclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaMinExclusiveFacet))
							{
								this.WriteObject_XmlSchemaMinExclusiveFacet((XmlSchemaMinExclusiveFacet)ob.Facets[i], "minExclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaEnumerationFacet))
							{
								this.WriteObject_XmlSchemaEnumerationFacet((XmlSchemaEnumerationFacet)ob.Facets[i], "enumeration", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaTotalDigitsFacet))
							{
								this.WriteObject_XmlSchemaTotalDigitsFacet((XmlSchemaTotalDigitsFacet)ob.Facets[i], "totalDigits", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaMinInclusiveFacet))
							{
								this.WriteObject_XmlSchemaMinInclusiveFacet((XmlSchemaMinInclusiveFacet)ob.Facets[i], "minInclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaWhiteSpaceFacet))
							{
								this.WriteObject_XmlSchemaWhiteSpaceFacet((XmlSchemaWhiteSpaceFacet)ob.Facets[i], "whiteSpace", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Facets[i].GetType() != typeof(XmlSchemaPatternFacet))
								{
									throw base.CreateUnknownTypeException(ob.Facets[i]);
								}
								this.WriteObject_XmlSchemaPatternFacet((XmlSchemaPatternFacet)ob.Facets[i], "pattern", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private string GetEnumValue_XmlSchemaUse(XmlSchemaUse val)
		{
			switch (val)
			{
			case XmlSchemaUse.Optional:
				return "optional";
			case XmlSchemaUse.Prohibited:
				return "prohibited";
			case XmlSchemaUse.Required:
				return "required";
			default:
				return ((long)val).ToString(CultureInfo.InvariantCulture);
			}
		}

		private void WriteObject_XmlSchemaAppInfo(XmlSchemaAppInfo ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaAppInfo))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaAppInfo", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				base.WriteAttribute("source", string.Empty, ob.Source);
				if (ob.Markup != null)
				{
					foreach (XmlNode xmlNode in ob.Markup)
					{
						XmlNode xmlNode2 = xmlNode;
						if (xmlNode2 is XmlElement)
						{
							base.WriteElementLiteral(xmlNode2, string.Empty, string.Empty, false, true);
						}
						else
						{
							xmlNode2.WriteTo(base.Writer);
						}
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaDocumentation(XmlSchemaDocumentation ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaDocumentation))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaDocumentation", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				base.WriteAttribute("source", string.Empty, ob.Source);
				base.WriteAttribute("xml:lang", string.Empty, ob.Language);
				if (ob.Markup != null)
				{
					foreach (XmlNode xmlNode in ob.Markup)
					{
						XmlNode xmlNode2 = xmlNode;
						if (xmlNode2 is XmlElement)
						{
							base.WriteElementLiteral(xmlNode2, string.Empty, string.Empty, false, true);
						}
						else
						{
							xmlNode2.WriteTo(base.Writer);
						}
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaAttributeGroupRef(XmlSchemaAttributeGroupRef ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaAttributeGroupRef))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaAttributeGroupRef", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("ref", string.Empty, base.FromXmlQualifiedName(ob.RefName));
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaAnyAttribute(XmlSchemaAnyAttribute ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaAnyAttribute))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaAnyAttribute", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("namespace", string.Empty, ob.Namespace);
				if (ob.ProcessContents != XmlSchemaContentProcessing.None)
				{
					base.WriteAttribute("processContents", string.Empty, this.GetEnumValue_XmlSchemaContentProcessing(ob.ProcessContents));
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaSequence(XmlSchemaSequence ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaSequence))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaSequence", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
				base.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Items != null)
				{
					for (int i = 0; i < ob.Items.Count; i++)
					{
						if (ob.Items[i] != null)
						{
							if (ob.Items[i].GetType() == typeof(XmlSchemaSequence))
							{
								this.WriteObject_XmlSchemaSequence((XmlSchemaSequence)ob.Items[i], "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[i].GetType() == typeof(XmlSchemaChoice))
							{
								this.WriteObject_XmlSchemaChoice((XmlSchemaChoice)ob.Items[i], "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[i].GetType() == typeof(XmlSchemaGroupRef))
							{
								this.WriteObject_XmlSchemaGroupRef((XmlSchemaGroupRef)ob.Items[i], "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[i].GetType() == typeof(XmlSchemaElement))
							{
								this.WriteObject_XmlSchemaElement((XmlSchemaElement)ob.Items[i], "element", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Items[i].GetType() != typeof(XmlSchemaAny))
								{
									throw base.CreateUnknownTypeException(ob.Items[i]);
								}
								this.WriteObject_XmlSchemaAny((XmlSchemaAny)ob.Items[i], "any", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaChoice(XmlSchemaChoice ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaChoice))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaChoice", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
				base.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Items != null)
				{
					for (int i = 0; i < ob.Items.Count; i++)
					{
						if (ob.Items[i] != null)
						{
							if (ob.Items[i].GetType() == typeof(XmlSchemaGroupRef))
							{
								this.WriteObject_XmlSchemaGroupRef((XmlSchemaGroupRef)ob.Items[i], "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[i].GetType() == typeof(XmlSchemaElement))
							{
								this.WriteObject_XmlSchemaElement((XmlSchemaElement)ob.Items[i], "element", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[i].GetType() == typeof(XmlSchemaAny))
							{
								this.WriteObject_XmlSchemaAny((XmlSchemaAny)ob.Items[i], "any", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Items[i].GetType() == typeof(XmlSchemaSequence))
							{
								this.WriteObject_XmlSchemaSequence((XmlSchemaSequence)ob.Items[i], "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Items[i].GetType() != typeof(XmlSchemaChoice))
								{
									throw base.CreateUnknownTypeException(ob.Items[i]);
								}
								this.WriteObject_XmlSchemaChoice((XmlSchemaChoice)ob.Items[i], "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaAll(XmlSchemaAll ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaAll))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaAll", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
				base.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Items != null)
				{
					for (int i = 0; i < ob.Items.Count; i++)
					{
						this.WriteObject_XmlSchemaElement((XmlSchemaElement)ob.Items[i], "element", "http://www.w3.org/2001/XMLSchema", false, false, true);
					}
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaComplexContent(XmlSchemaComplexContent ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaComplexContent))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaComplexContent", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("mixed", string.Empty, (!ob.IsMixed) ? "false" : "true");
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Content is XmlSchemaComplexContentExtension)
				{
					this.WriteObject_XmlSchemaComplexContentExtension((XmlSchemaComplexContentExtension)ob.Content, "extension", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Content is XmlSchemaComplexContentRestriction)
				{
					this.WriteObject_XmlSchemaComplexContentRestriction((XmlSchemaComplexContentRestriction)ob.Content, "restriction", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaSimpleContent(XmlSchemaSimpleContent ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaSimpleContent))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaSimpleContent", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Content is XmlSchemaSimpleContentExtension)
				{
					this.WriteObject_XmlSchemaSimpleContentExtension((XmlSchemaSimpleContentExtension)ob.Content, "extension", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Content is XmlSchemaSimpleContentRestriction)
				{
					this.WriteObject_XmlSchemaSimpleContentRestriction((XmlSchemaSimpleContentRestriction)ob.Content, "restriction", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaGroupRef(XmlSchemaGroupRef ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaGroupRef))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaGroupRef", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
				base.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
				base.WriteAttribute("ref", string.Empty, base.FromXmlQualifiedName(ob.RefName));
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaXPath(XmlSchemaXPath ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaXPath))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaXPath", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				if (ob.XPath != null)
				{
					base.WriteAttribute("xpath", string.Empty, ob.XPath);
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaMaxLengthFacet(XmlSchemaMaxLengthFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaMaxLengthFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaMaxLengthFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaMinLengthFacet(XmlSchemaMinLengthFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaMinLengthFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaMinLengthFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaLengthFacet(XmlSchemaLengthFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaLengthFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaLengthFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaFractionDigitsFacet(XmlSchemaFractionDigitsFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaFractionDigitsFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaFractionDigitsFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaMaxInclusiveFacet(XmlSchemaMaxInclusiveFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaMaxInclusiveFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaMaxInclusiveFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaMaxExclusiveFacet(XmlSchemaMaxExclusiveFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaMaxExclusiveFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaMaxExclusiveFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaMinExclusiveFacet(XmlSchemaMinExclusiveFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaMinExclusiveFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaMinExclusiveFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaEnumerationFacet(XmlSchemaEnumerationFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaEnumerationFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaEnumerationFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaTotalDigitsFacet(XmlSchemaTotalDigitsFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaTotalDigitsFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaTotalDigitsFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaMinInclusiveFacet(XmlSchemaMinInclusiveFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaMinInclusiveFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaMinInclusiveFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaWhiteSpaceFacet(XmlSchemaWhiteSpaceFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaWhiteSpaceFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaWhiteSpaceFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaPatternFacet(XmlSchemaPatternFacet ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaPatternFacet))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaPatternFacet", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("value", string.Empty, ob.Value);
				if (ob.IsFixed)
				{
					base.WriteAttribute("fixed", string.Empty, (!ob.IsFixed) ? "false" : "true");
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private string GetEnumValue_XmlSchemaContentProcessing(XmlSchemaContentProcessing val)
		{
			switch (val)
			{
			case XmlSchemaContentProcessing.Skip:
				return "skip";
			case XmlSchemaContentProcessing.Lax:
				return "lax";
			case XmlSchemaContentProcessing.Strict:
				return "strict";
			default:
				return ((long)val).ToString(CultureInfo.InvariantCulture);
			}
		}

		private void WriteObject_XmlSchemaAny(XmlSchemaAny ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaAny))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaAny", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
				base.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
				base.WriteAttribute("namespace", string.Empty, ob.Namespace);
				if (ob.ProcessContents != XmlSchemaContentProcessing.None)
				{
					base.WriteAttribute("processContents", string.Empty, this.GetEnumValue_XmlSchemaContentProcessing(ob.ProcessContents));
				}
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaComplexContentExtension(XmlSchemaComplexContentExtension ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaComplexContentExtension))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaComplexContentExtension", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("base", string.Empty, base.FromXmlQualifiedName(ob.BaseTypeName));
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Particle is XmlSchemaGroupRef)
				{
					this.WriteObject_XmlSchemaGroupRef((XmlSchemaGroupRef)ob.Particle, "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Particle is XmlSchemaSequence)
				{
					this.WriteObject_XmlSchemaSequence((XmlSchemaSequence)ob.Particle, "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Particle is XmlSchemaChoice)
				{
					this.WriteObject_XmlSchemaChoice((XmlSchemaChoice)ob.Particle, "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Particle is XmlSchemaAll)
				{
					this.WriteObject_XmlSchemaAll((XmlSchemaAll)ob.Particle, "all", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				if (ob.Attributes != null)
				{
					for (int i = 0; i < ob.Attributes.Count; i++)
					{
						if (ob.Attributes[i] != null)
						{
							if (ob.Attributes[i].GetType() == typeof(XmlSchemaAttribute))
							{
								this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute)ob.Attributes[i], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Attributes[i].GetType() != typeof(XmlSchemaAttributeGroupRef))
								{
									throw base.CreateUnknownTypeException(ob.Attributes[i]);
								}
								this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef)ob.Attributes[i], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaComplexContentRestriction(XmlSchemaComplexContentRestriction ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaComplexContentRestriction))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaComplexContentRestriction", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("base", string.Empty, base.FromXmlQualifiedName(ob.BaseTypeName));
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Particle is XmlSchemaSequence)
				{
					this.WriteObject_XmlSchemaSequence((XmlSchemaSequence)ob.Particle, "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Particle is XmlSchemaChoice)
				{
					this.WriteObject_XmlSchemaChoice((XmlSchemaChoice)ob.Particle, "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Particle is XmlSchemaGroupRef)
				{
					this.WriteObject_XmlSchemaGroupRef((XmlSchemaGroupRef)ob.Particle, "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				else if (ob.Particle is XmlSchemaAll)
				{
					this.WriteObject_XmlSchemaAll((XmlSchemaAll)ob.Particle, "all", "http://www.w3.org/2001/XMLSchema", false, false, true);
				}
				if (ob.Attributes != null)
				{
					for (int i = 0; i < ob.Attributes.Count; i++)
					{
						if (ob.Attributes[i] != null)
						{
							if (ob.Attributes[i].GetType() == typeof(XmlSchemaAttributeGroupRef))
							{
								this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef)ob.Attributes[i], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Attributes[i].GetType() != typeof(XmlSchemaAttribute))
								{
									throw base.CreateUnknownTypeException(ob.Attributes[i]);
								}
								this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute)ob.Attributes[i], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaSimpleContentExtension(XmlSchemaSimpleContentExtension ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaSimpleContentExtension))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaSimpleContentExtension", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("base", string.Empty, base.FromXmlQualifiedName(ob.BaseTypeName));
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Attributes != null)
				{
					for (int i = 0; i < ob.Attributes.Count; i++)
					{
						if (ob.Attributes[i] != null)
						{
							if (ob.Attributes[i].GetType() == typeof(XmlSchemaAttributeGroupRef))
							{
								this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef)ob.Attributes[i], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Attributes[i].GetType() != typeof(XmlSchemaAttribute))
								{
									throw base.CreateUnknownTypeException(ob.Attributes[i]);
								}
								this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute)ob.Attributes[i], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		private void WriteObject_XmlSchemaSimpleContentRestriction(XmlSchemaSimpleContentRestriction ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
		{
			if (ob == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(element, namesp);
				}
				return;
			}
			Type type = ob.GetType();
			if (type == typeof(XmlSchemaSimpleContentRestriction))
			{
				if (writeWrappingElem)
				{
					base.WriteStartElement(element, namesp, ob);
				}
				if (needType)
				{
					base.WriteXsiType("XmlSchemaSimpleContentRestriction", "http://www.w3.org/2001/XMLSchema");
				}
				base.WriteNamespaceDeclarations(ob.Namespaces);
				ICollection unhandledAttributes = ob.UnhandledAttributes;
				if (unhandledAttributes != null)
				{
					foreach (object obj in unhandledAttributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (xmlAttribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
						{
							base.WriteXmlAttribute(xmlAttribute, ob);
						}
					}
				}
				base.WriteAttribute("id", string.Empty, ob.Id);
				base.WriteAttribute("base", string.Empty, base.FromXmlQualifiedName(ob.BaseTypeName));
				this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
				this.WriteObject_XmlSchemaSimpleType(ob.BaseType, "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (ob.Facets != null)
				{
					for (int i = 0; i < ob.Facets.Count; i++)
					{
						if (ob.Facets[i] != null)
						{
							if (ob.Facets[i].GetType() == typeof(XmlSchemaEnumerationFacet))
							{
								this.WriteObject_XmlSchemaEnumerationFacet((XmlSchemaEnumerationFacet)ob.Facets[i], "enumeration", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaMaxLengthFacet))
							{
								this.WriteObject_XmlSchemaMaxLengthFacet((XmlSchemaMaxLengthFacet)ob.Facets[i], "maxLength", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaMinLengthFacet))
							{
								this.WriteObject_XmlSchemaMinLengthFacet((XmlSchemaMinLengthFacet)ob.Facets[i], "minLength", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaLengthFacet))
							{
								this.WriteObject_XmlSchemaLengthFacet((XmlSchemaLengthFacet)ob.Facets[i], "length", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaFractionDigitsFacet))
							{
								this.WriteObject_XmlSchemaFractionDigitsFacet((XmlSchemaFractionDigitsFacet)ob.Facets[i], "fractionDigits", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaTotalDigitsFacet))
							{
								this.WriteObject_XmlSchemaTotalDigitsFacet((XmlSchemaTotalDigitsFacet)ob.Facets[i], "totalDigits", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaMaxInclusiveFacet))
							{
								this.WriteObject_XmlSchemaMaxInclusiveFacet((XmlSchemaMaxInclusiveFacet)ob.Facets[i], "maxInclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaMaxExclusiveFacet))
							{
								this.WriteObject_XmlSchemaMaxExclusiveFacet((XmlSchemaMaxExclusiveFacet)ob.Facets[i], "maxExclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaMinInclusiveFacet))
							{
								this.WriteObject_XmlSchemaMinInclusiveFacet((XmlSchemaMinInclusiveFacet)ob.Facets[i], "minInclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaMinExclusiveFacet))
							{
								this.WriteObject_XmlSchemaMinExclusiveFacet((XmlSchemaMinExclusiveFacet)ob.Facets[i], "minExclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else if (ob.Facets[i].GetType() == typeof(XmlSchemaWhiteSpaceFacet))
							{
								this.WriteObject_XmlSchemaWhiteSpaceFacet((XmlSchemaWhiteSpaceFacet)ob.Facets[i], "whiteSpace", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Facets[i].GetType() != typeof(XmlSchemaPatternFacet))
								{
									throw base.CreateUnknownTypeException(ob.Facets[i]);
								}
								this.WriteObject_XmlSchemaPatternFacet((XmlSchemaPatternFacet)ob.Facets[i], "pattern", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				if (ob.Attributes != null)
				{
					for (int j = 0; j < ob.Attributes.Count; j++)
					{
						if (ob.Attributes[j] != null)
						{
							if (ob.Attributes[j].GetType() == typeof(XmlSchemaAttributeGroupRef))
							{
								this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef)ob.Attributes[j], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
							else
							{
								if (ob.Attributes[j].GetType() != typeof(XmlSchemaAttribute))
								{
									throw base.CreateUnknownTypeException(ob.Attributes[j]);
								}
								this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute)ob.Attributes[j], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
							}
						}
					}
				}
				this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
				if (writeWrappingElem)
				{
					base.WriteEndElement(ob);
				}
				return;
			}
			throw base.CreateUnknownTypeException(ob);
		}

		protected override void InitCallbacks()
		{
		}
	}
}
