using System;
using System.Collections;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdKeyEntryField
	{
		private XsdKeyEntry entry;

		private XsdIdentityField field;

		public bool FieldFound;

		public int FieldLineNumber;

		public int FieldLinePosition;

		public bool FieldHasLineInfo;

		public XsdAnySimpleType FieldType;

		public object Identity;

		public bool IsXsiNil;

		public int FieldFoundDepth;

		public XsdIdentityPath FieldFoundPath;

		public bool Consuming;

		public bool Consumed;

		public XsdKeyEntryField(XsdKeyEntry entry, XsdIdentityField field)
		{
			this.entry = entry;
			this.field = field;
		}

		public XsdIdentityField Field
		{
			get
			{
				return this.field;
			}
		}

		public bool SetIdentityField(object identity, bool isXsiNil, XsdAnySimpleType type, int depth, IXmlLineInfo li)
		{
			this.FieldFoundDepth = depth;
			this.Identity = identity;
			this.IsXsiNil = isXsiNil;
			this.FieldFound = (this.FieldFound || isXsiNil);
			this.FieldType = type;
			this.Consuming = false;
			this.Consumed = true;
			if (li != null && li.HasLineInfo())
			{
				this.FieldHasLineInfo = true;
				this.FieldLineNumber = li.LineNumber;
				this.FieldLinePosition = li.LinePosition;
			}
			if (!(this.entry.OwnerSequence.SourceSchemaIdentity is XmlSchemaKeyref))
			{
				for (int i = 0; i < this.entry.OwnerSequence.FinishedEntries.Count; i++)
				{
					XsdKeyEntry other = this.entry.OwnerSequence.FinishedEntries[i];
					if (this.entry.CompareIdentity(other))
					{
						return false;
					}
				}
			}
			return true;
		}

		internal XsdIdentityPath Matches(bool matchesAttr, object sender, XmlNameTable nameTable, ArrayList qnameStack, string sourceUri, object schemaType, IXmlNamespaceResolver nsResolver, IXmlLineInfo lineInfo, int depth, string attrName, string attrNS, object attrValue)
		{
			XsdIdentityPath xsdIdentityPath = null;
			for (int i = 0; i < this.field.Paths.Length; i++)
			{
				XsdIdentityPath xsdIdentityPath2 = this.field.Paths[i];
				bool isAttribute = xsdIdentityPath2.IsAttribute;
				if (matchesAttr == isAttribute)
				{
					if (xsdIdentityPath2.IsAttribute)
					{
						XsdIdentityStep xsdIdentityStep = xsdIdentityPath2.OrderedSteps[xsdIdentityPath2.OrderedSteps.Length - 1];
						bool flag = false;
						if (xsdIdentityStep.IsAnyName || xsdIdentityStep.NsName != null)
						{
							if (xsdIdentityStep.IsAnyName || attrNS == xsdIdentityStep.NsName)
							{
								flag = true;
							}
						}
						else if (xsdIdentityStep.Name == attrName && xsdIdentityStep.Namespace == attrNS)
						{
							flag = true;
						}
						if (!flag)
						{
							goto IL_2AF;
						}
						if (this.entry.StartDepth + (xsdIdentityPath2.OrderedSteps.Length - 1) != depth - 1)
						{
							goto IL_2AF;
						}
						xsdIdentityPath = xsdIdentityPath2;
					}
					if (!this.FieldFound || depth <= this.FieldFoundDepth || this.FieldFoundPath != xsdIdentityPath2)
					{
						if (xsdIdentityPath2.OrderedSteps.Length == 0)
						{
							if (depth == this.entry.StartDepth)
							{
								return xsdIdentityPath2;
							}
						}
						else if (depth - this.entry.StartDepth >= xsdIdentityPath2.OrderedSteps.Length - 1)
						{
							int j = xsdIdentityPath2.OrderedSteps.Length;
							if (isAttribute)
							{
								j--;
							}
							if (!xsdIdentityPath2.Descendants || depth >= this.entry.StartDepth + j)
							{
								if (xsdIdentityPath2.Descendants || depth == this.entry.StartDepth + j)
								{
									for (j--; j >= 0; j--)
									{
										XsdIdentityStep xsdIdentityStep = xsdIdentityPath2.OrderedSteps[j];
										if (!xsdIdentityStep.IsCurrent && !xsdIdentityStep.IsAnyName)
										{
											XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)qnameStack[this.entry.StartDepth + j + ((!isAttribute) ? 1 : 0)];
											if (xsdIdentityStep.NsName == null || !(xmlQualifiedName.Namespace == xsdIdentityStep.NsName))
											{
												if ((!(xsdIdentityStep.Name == "*") && !(xsdIdentityStep.Name == xmlQualifiedName.Name)) || !(xsdIdentityStep.Namespace == xmlQualifiedName.Namespace))
												{
													break;
												}
											}
										}
									}
									if (j < 0)
									{
										if (!matchesAttr)
										{
											return xsdIdentityPath2;
										}
									}
								}
							}
						}
					}
				}
				IL_2AF:;
			}
			if (xsdIdentityPath != null)
			{
				this.FillAttributeFieldValue(sender, nameTable, sourceUri, schemaType, nsResolver, attrValue, lineInfo, depth);
				if (this.Identity != null)
				{
					return xsdIdentityPath;
				}
			}
			return null;
		}

		private void FillAttributeFieldValue(object sender, XmlNameTable nameTable, string sourceUri, object schemaType, IXmlNamespaceResolver nsResolver, object identity, IXmlLineInfo lineInfo, int depth)
		{
			if (this.FieldFound)
			{
				throw new XmlSchemaValidationException(string.Format("The key value was already found as '{0}'{1}.", this.Identity, (!this.FieldHasLineInfo) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " at line {0}, position {1}", new object[]
				{
					this.FieldLineNumber,
					this.FieldLinePosition
				})), sender, sourceUri, this.entry.OwnerSequence.SourceSchemaIdentity, null);
			}
			XmlSchemaDatatype xmlSchemaDatatype = schemaType as XmlSchemaDatatype;
			XmlSchemaSimpleType xmlSchemaSimpleType = schemaType as XmlSchemaSimpleType;
			if (xmlSchemaDatatype == null && xmlSchemaSimpleType != null)
			{
				xmlSchemaDatatype = xmlSchemaSimpleType.Datatype;
			}
			try
			{
				if (!this.SetIdentityField(identity, false, xmlSchemaDatatype as XsdAnySimpleType, depth, lineInfo))
				{
					throw new XmlSchemaValidationException("Two or more identical field was found.", sender, sourceUri, this.entry.OwnerSequence.SourceSchemaIdentity, null);
				}
				this.Consuming = true;
				this.FieldFound = true;
			}
			catch (Exception innerException)
			{
				throw new XmlSchemaValidationException("Failed to read typed value.", sender, sourceUri, this.entry.OwnerSequence.SourceSchemaIdentity, innerException);
			}
		}
	}
}
