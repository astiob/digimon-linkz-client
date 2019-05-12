using Mono.Xml.Schema;
using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the restriction element for simple types from XML Schema as specified by the World Wide Web Consortium (W3C). This class can be used restricting simpleType element.</summary>
	public class XmlSchemaSimpleTypeRestriction : XmlSchemaSimpleTypeContent
	{
		private const string xmlname = "restriction";

		private XmlSchemaSimpleType baseType;

		private XmlQualifiedName baseTypeName;

		private XmlSchemaObjectCollection facets;

		private string[] enumarationFacetValues;

		private string[] patternFacetValues;

		private Regex[] rexPatterns;

		private decimal lengthFacet;

		private decimal maxLengthFacet;

		private decimal minLengthFacet;

		private decimal fractionDigitsFacet;

		private decimal totalDigitsFacet;

		private object maxInclusiveFacet;

		private object maxExclusiveFacet;

		private object minInclusiveFacet;

		private object minExclusiveFacet;

		private XmlSchemaFacet.Facet fixedFacets;

		private static NumberStyles lengthStyle = NumberStyles.Integer;

		private static readonly XmlSchemaFacet.Facet listFacets = XmlSchemaFacet.Facet.length | XmlSchemaFacet.Facet.minLength | XmlSchemaFacet.Facet.maxLength | XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.enumeration | XmlSchemaFacet.Facet.whiteSpace;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaSimpleTypeRestriction" /> class.</summary>
		public XmlSchemaSimpleTypeRestriction()
		{
			this.baseTypeName = XmlQualifiedName.Empty;
			this.facets = new XmlSchemaObjectCollection();
		}

		/// <summary>Gets or sets the name of the qualified base type.</summary>
		/// <returns>The qualified name of the simple type restriction base type.</returns>
		[XmlAttribute("base")]
		public XmlQualifiedName BaseTypeName
		{
			get
			{
				return this.baseTypeName;
			}
			set
			{
				this.baseTypeName = value;
			}
		}

		/// <summary>Gets or sets information on the base type.</summary>
		/// <returns>The base type for the simpleType element.</returns>
		[XmlElement("simpleType", Type = typeof(XmlSchemaSimpleType))]
		public XmlSchemaSimpleType BaseType
		{
			get
			{
				return this.baseType;
			}
			set
			{
				this.baseType = value;
			}
		}

		/// <summary>Gets or sets an Xml Schema facet. </summary>
		/// <returns>One of the following facet classes:<see cref="T:System.Xml.Schema.XmlSchemaLengthFacet" />, <see cref="T:System.Xml.Schema.XmlSchemaMinLengthFacet" />, <see cref="T:System.Xml.Schema.XmlSchemaMaxLengthFacet" />, <see cref="T:System.Xml.Schema.XmlSchemaPatternFacet" />, <see cref="T:System.Xml.Schema.XmlSchemaEnumerationFacet" />, <see cref="T:System.Xml.Schema.XmlSchemaMaxInclusiveFacet" />, <see cref="T:System.Xml.Schema.XmlSchemaMaxExclusiveFacet" />, <see cref="T:System.Xml.Schema.XmlSchemaMinInclusiveFacet" />, <see cref="T:System.Xml.Schema.XmlSchemaMinExclusiveFacet" />, <see cref="T:System.Xml.Schema.XmlSchemaFractionDigitsFacet" />, <see cref="T:System.Xml.Schema.XmlSchemaTotalDigitsFacet" />, <see cref="T:System.Xml.Schema.XmlSchemaWhiteSpaceFacet" />.</returns>
		[XmlElement("length", typeof(XmlSchemaLengthFacet))]
		[XmlElement("maxLength", typeof(XmlSchemaMaxLengthFacet))]
		[XmlElement("pattern", typeof(XmlSchemaPatternFacet))]
		[XmlElement("whiteSpace", typeof(XmlSchemaWhiteSpaceFacet))]
		[XmlElement("minExclusive", typeof(XmlSchemaMinExclusiveFacet))]
		[XmlElement("minInclusive", typeof(XmlSchemaMinInclusiveFacet))]
		[XmlElement("maxExclusive", typeof(XmlSchemaMaxExclusiveFacet))]
		[XmlElement("enumeration", typeof(XmlSchemaEnumerationFacet))]
		[XmlElement("maxInclusive", typeof(XmlSchemaMaxInclusiveFacet))]
		[XmlElement("totalDigits", typeof(XmlSchemaTotalDigitsFacet))]
		[XmlElement("fractionDigits", typeof(XmlSchemaFractionDigitsFacet))]
		[XmlElement("minLength", typeof(XmlSchemaMinLengthFacet))]
		public XmlSchemaObjectCollection Facets
		{
			get
			{
				return this.facets;
			}
		}

		internal override void SetParent(XmlSchemaObject parent)
		{
			base.SetParent(parent);
			if (this.BaseType != null)
			{
				this.BaseType.SetParent(this);
			}
			foreach (XmlSchemaObject xmlSchemaObject in this.Facets)
			{
				xmlSchemaObject.SetParent(this);
			}
		}

		internal override int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.CompilationId == schema.CompilationId)
			{
				return 0;
			}
			this.errorCount = 0;
			if (this.baseType != null && !this.BaseTypeName.IsEmpty)
			{
				base.error(h, "both base and simpletype can't be set");
			}
			if (this.baseType == null && this.BaseTypeName.IsEmpty)
			{
				base.error(h, "one of basetype or simpletype must be present");
			}
			if (this.baseType != null)
			{
				this.errorCount += this.baseType.Compile(h, schema);
			}
			if (!XmlSchemaUtil.CheckQName(this.BaseTypeName))
			{
				base.error(h, "BaseTypeName must be a XmlQualifiedName");
			}
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			for (int i = 0; i < this.Facets.Count; i++)
			{
				if (!(this.Facets[i] is XmlSchemaFacet))
				{
					base.error(h, "Only XmlSchemaFacet objects are allowed for Facets property");
				}
			}
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		private bool IsAllowedFacet(XmlSchemaFacet xsf)
		{
			XsdAnySimpleType xsdAnySimpleType = base.ActualBaseSchemaType as XsdAnySimpleType;
			if (xsdAnySimpleType != null)
			{
				return xsdAnySimpleType.AllowsFacet(xsf);
			}
			XmlSchemaSimpleTypeContent content = ((XmlSchemaSimpleType)base.ActualBaseSchemaType).Content;
			if (content != null)
			{
				XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = content as XmlSchemaSimpleTypeRestriction;
				if (xmlSchemaSimpleTypeRestriction != null && xmlSchemaSimpleTypeRestriction != this)
				{
					return xmlSchemaSimpleTypeRestriction.IsAllowedFacet(xsf);
				}
				XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = content as XmlSchemaSimpleTypeList;
				if (xmlSchemaSimpleTypeList != null)
				{
					return (xsf.ThisFacet & XmlSchemaSimpleTypeRestriction.listFacets) != XmlSchemaFacet.Facet.None;
				}
				XmlSchemaSimpleTypeUnion xmlSchemaSimpleTypeUnion = content as XmlSchemaSimpleTypeUnion;
				if (xmlSchemaSimpleTypeUnion != null)
				{
					return xsf is XmlSchemaPatternFacet || xsf is XmlSchemaEnumerationFacet;
				}
			}
			return false;
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			if (base.IsValidated(schema.ValidationId))
			{
				return this.errorCount;
			}
			this.ValidateActualType(h, schema);
			this.lengthFacet = (this.maxLengthFacet = (this.minLengthFacet = (this.fractionDigitsFacet = (this.totalDigitsFacet = -1m))));
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = null;
			if (base.ActualBaseSchemaType is XmlSchemaSimpleType)
			{
				XmlSchemaSimpleTypeContent content = ((XmlSchemaSimpleType)base.ActualBaseSchemaType).Content;
				xmlSchemaSimpleTypeRestriction = (content as XmlSchemaSimpleTypeRestriction);
			}
			if (xmlSchemaSimpleTypeRestriction != null)
			{
				this.fixedFacets = xmlSchemaSimpleTypeRestriction.fixedFacets;
				this.lengthFacet = xmlSchemaSimpleTypeRestriction.lengthFacet;
				this.maxLengthFacet = xmlSchemaSimpleTypeRestriction.maxLengthFacet;
				this.minLengthFacet = xmlSchemaSimpleTypeRestriction.minLengthFacet;
				this.fractionDigitsFacet = xmlSchemaSimpleTypeRestriction.fractionDigitsFacet;
				this.totalDigitsFacet = xmlSchemaSimpleTypeRestriction.totalDigitsFacet;
				this.maxInclusiveFacet = xmlSchemaSimpleTypeRestriction.maxInclusiveFacet;
				this.maxExclusiveFacet = xmlSchemaSimpleTypeRestriction.maxExclusiveFacet;
				this.minInclusiveFacet = xmlSchemaSimpleTypeRestriction.minInclusiveFacet;
				this.minExclusiveFacet = xmlSchemaSimpleTypeRestriction.minExclusiveFacet;
			}
			this.enumarationFacetValues = (this.patternFacetValues = null);
			this.rexPatterns = null;
			XmlSchemaFacet.Facet facet = XmlSchemaFacet.Facet.None;
			ArrayList arrayList = null;
			ArrayList arrayList2 = null;
			for (int i = 0; i < this.facets.Count; i++)
			{
				XmlSchemaFacet xmlSchemaFacet = this.facets[i] as XmlSchemaFacet;
				if (xmlSchemaFacet != null)
				{
					if (!this.IsAllowedFacet(xmlSchemaFacet))
					{
						xmlSchemaFacet.error(h, xmlSchemaFacet.ThisFacet + " is not a valid facet for this type");
					}
					else
					{
						XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet = this.facets[i] as XmlSchemaEnumerationFacet;
						if (xmlSchemaEnumerationFacet != null)
						{
							if (arrayList == null)
							{
								arrayList = new ArrayList();
							}
							arrayList.Add(xmlSchemaEnumerationFacet.Value);
						}
						else
						{
							XmlSchemaPatternFacet xmlSchemaPatternFacet = this.facets[i] as XmlSchemaPatternFacet;
							if (xmlSchemaPatternFacet != null)
							{
								if (arrayList2 == null)
								{
									arrayList2 = new ArrayList();
								}
								arrayList2.Add(xmlSchemaPatternFacet.Value);
							}
							else if ((facet & xmlSchemaFacet.ThisFacet) != XmlSchemaFacet.Facet.None)
							{
								xmlSchemaFacet.error(h, "This is a duplicate '" + xmlSchemaFacet.ThisFacet + "' facet.");
							}
							else
							{
								facet |= xmlSchemaFacet.ThisFacet;
								if (xmlSchemaFacet is XmlSchemaLengthFacet)
								{
									this.checkLengthFacet((XmlSchemaLengthFacet)xmlSchemaFacet, facet, h);
								}
								else if (xmlSchemaFacet is XmlSchemaMaxLengthFacet)
								{
									this.checkMaxLengthFacet((XmlSchemaMaxLengthFacet)xmlSchemaFacet, facet, h);
								}
								else if (xmlSchemaFacet is XmlSchemaMinLengthFacet)
								{
									this.checkMinLengthFacet((XmlSchemaMinLengthFacet)xmlSchemaFacet, facet, h);
								}
								else if (xmlSchemaFacet is XmlSchemaMinInclusiveFacet)
								{
									this.checkMinMaxFacet((XmlSchemaMinInclusiveFacet)xmlSchemaFacet, ref this.minInclusiveFacet, h);
								}
								else if (xmlSchemaFacet is XmlSchemaMaxInclusiveFacet)
								{
									this.checkMinMaxFacet((XmlSchemaMaxInclusiveFacet)xmlSchemaFacet, ref this.maxInclusiveFacet, h);
								}
								else if (xmlSchemaFacet is XmlSchemaMinExclusiveFacet)
								{
									this.checkMinMaxFacet((XmlSchemaMinExclusiveFacet)xmlSchemaFacet, ref this.minExclusiveFacet, h);
								}
								else if (xmlSchemaFacet is XmlSchemaMaxExclusiveFacet)
								{
									this.checkMinMaxFacet((XmlSchemaMaxExclusiveFacet)xmlSchemaFacet, ref this.maxExclusiveFacet, h);
								}
								else if (xmlSchemaFacet is XmlSchemaFractionDigitsFacet)
								{
									this.checkFractionDigitsFacet((XmlSchemaFractionDigitsFacet)xmlSchemaFacet, h);
								}
								else if (xmlSchemaFacet is XmlSchemaTotalDigitsFacet)
								{
									this.checkTotalDigitsFacet((XmlSchemaTotalDigitsFacet)xmlSchemaFacet, h);
								}
								if (xmlSchemaFacet.IsFixed)
								{
									this.fixedFacets |= xmlSchemaFacet.ThisFacet;
								}
							}
						}
					}
				}
			}
			if (arrayList != null)
			{
				this.enumarationFacetValues = (arrayList.ToArray(typeof(string)) as string[]);
			}
			if (arrayList2 != null)
			{
				this.patternFacetValues = (arrayList2.ToArray(typeof(string)) as string[]);
				this.rexPatterns = new Regex[arrayList2.Count];
				for (int j = 0; j < this.patternFacetValues.Length; j++)
				{
					try
					{
						string text = this.patternFacetValues[j];
						StringBuilder stringBuilder = null;
						int num = 0;
						for (int k = 0; k < text.Length; k++)
						{
							if (text[k] == '\\' && text.Length > j + 1)
							{
								string text2 = null;
								char c = text[k + 1];
								if (c != 'C')
								{
									if (c != 'I')
									{
										if (c != 'c')
										{
											if (c == 'i')
											{
												text2 = "[\\p{L}_]";
											}
										}
										else
										{
											text2 = "[\\p{L}\\p{N}_\\.\\-:]";
										}
									}
									else
									{
										text2 = "[^\\p{L}_]";
									}
								}
								else
								{
									text2 = "[^\\p{L}\\p{N}_\\.\\-:]";
								}
								if (text2 != null)
								{
									if (stringBuilder == null)
									{
										stringBuilder = new StringBuilder();
									}
									stringBuilder.Append(text, num, k - num);
									stringBuilder.Append(text2);
									num = k + 2;
								}
							}
						}
						if (stringBuilder != null)
						{
							stringBuilder.Append(text, num, text.Length - num);
							text = stringBuilder.ToString();
						}
						Regex regex = new Regex("^" + text + "$");
						this.rexPatterns[j] = regex;
					}
					catch (Exception innerException)
					{
						XmlSchemaObject.error(h, "Invalid regular expression pattern was specified.", innerException);
					}
				}
			}
			this.ValidationId = schema.ValidationId;
			return this.errorCount;
		}

		internal void ValidateActualType(ValidationEventHandler h, XmlSchema schema)
		{
			this.GetActualType(h, schema, true);
		}

		internal object GetActualType(ValidationEventHandler h, XmlSchema schema, bool validate)
		{
			object obj = null;
			XmlSchemaSimpleType xmlSchemaSimpleType = this.baseType;
			if (xmlSchemaSimpleType == null)
			{
				xmlSchemaSimpleType = (schema.FindSchemaType(this.baseTypeName) as XmlSchemaSimpleType);
			}
			if (xmlSchemaSimpleType != null)
			{
				if (validate)
				{
					this.errorCount += xmlSchemaSimpleType.Validate(h, schema);
				}
				obj = xmlSchemaSimpleType;
			}
			else if (this.baseTypeName == XmlSchemaComplexType.AnyTypeName)
			{
				obj = XmlSchemaSimpleType.AnySimpleType;
			}
			else if (this.baseTypeName.Namespace == "http://www.w3.org/2001/XMLSchema" || this.baseTypeName.Namespace == "http://www.w3.org/2003/11/xpath-datatypes")
			{
				obj = XmlSchemaDatatype.FromName(this.baseTypeName);
				if (obj == null && validate)
				{
					base.error(h, "Invalid schema type name was specified: " + this.baseTypeName);
				}
			}
			else if (!schema.IsNamespaceAbsent(this.baseTypeName.Namespace) && validate)
			{
				base.error(h, "Referenced base schema type " + this.baseTypeName + " was not found in the corresponding schema.");
			}
			return obj;
		}

		private void checkTotalDigitsFacet(XmlSchemaTotalDigitsFacet totf, ValidationEventHandler h)
		{
			if (totf != null)
			{
				try
				{
					decimal num = decimal.Parse(totf.Value.Trim(), XmlSchemaSimpleTypeRestriction.lengthStyle, CultureInfo.InvariantCulture);
					if (num <= 0m)
					{
						totf.error(h, string.Format(CultureInfo.InvariantCulture, "The value '{0}' is an invalid totalDigits value", new object[]
						{
							num
						}));
					}
					if (this.totalDigitsFacet > 0m && num > this.totalDigitsFacet)
					{
						totf.error(h, string.Format(CultureInfo.InvariantCulture, "The value '{0}' is not a valid restriction of the base totalDigits facet '{1}'", new object[]
						{
							num,
							this.totalDigitsFacet
						}));
					}
					this.totalDigitsFacet = num;
				}
				catch (FormatException)
				{
					totf.error(h, string.Format("The value '{0}' is an invalid totalDigits facet specification", totf.Value.Trim()));
				}
			}
		}

		private void checkFractionDigitsFacet(XmlSchemaFractionDigitsFacet fracf, ValidationEventHandler h)
		{
			if (fracf != null)
			{
				try
				{
					decimal num = decimal.Parse(fracf.Value.Trim(), XmlSchemaSimpleTypeRestriction.lengthStyle, CultureInfo.InvariantCulture);
					if (num < 0m)
					{
						fracf.error(h, string.Format(CultureInfo.InvariantCulture, "The value '{0}' is an invalid fractionDigits value", new object[]
						{
							num
						}));
					}
					if (this.fractionDigitsFacet >= 0m && num > this.fractionDigitsFacet)
					{
						fracf.error(h, string.Format(CultureInfo.InvariantCulture, "The value '{0}' is not a valid restriction of the base fractionDigits facet '{1}'", new object[]
						{
							num,
							this.fractionDigitsFacet
						}));
					}
					this.fractionDigitsFacet = num;
				}
				catch (FormatException)
				{
					fracf.error(h, string.Format("The value '{0}' is an invalid fractionDigits facet specification", fracf.Value.Trim()));
				}
			}
		}

		private void checkMinMaxFacet(XmlSchemaFacet facet, ref object baseFacet, ValidationEventHandler h)
		{
			object obj = this.ValidateValueWithDatatype(facet.Value);
			if (obj != null)
			{
				if ((this.fixedFacets & facet.ThisFacet) != XmlSchemaFacet.Facet.None && baseFacet != null)
				{
					XsdAnySimpleType datatype = this.getDatatype();
					if (datatype.Compare(obj, baseFacet) != XsdOrdering.Equal)
					{
						facet.error(h, string.Format(CultureInfo.InvariantCulture, "{0} is not the same as fixed parent {1} facet.", new object[]
						{
							facet.Value,
							facet.ThisFacet
						}));
					}
				}
				baseFacet = obj;
			}
			else
			{
				facet.error(h, string.Format("The value '{0}' is not valid against the base type.", facet.Value));
			}
		}

		private void checkLengthFacet(XmlSchemaLengthFacet lf, XmlSchemaFacet.Facet facetsDefined, ValidationEventHandler h)
		{
			if (lf != null)
			{
				try
				{
					if ((facetsDefined & (XmlSchemaFacet.Facet.minLength | XmlSchemaFacet.Facet.maxLength)) != XmlSchemaFacet.Facet.None)
					{
						lf.error(h, "It is an error for both length and minLength or maxLength to be present.");
					}
					else
					{
						this.lengthFacet = decimal.Parse(lf.Value.Trim(), XmlSchemaSimpleTypeRestriction.lengthStyle, CultureInfo.InvariantCulture);
						if (this.lengthFacet < 0m)
						{
							lf.error(h, "The value '" + this.lengthFacet + "' is an invalid length");
						}
					}
				}
				catch (FormatException)
				{
					lf.error(h, "The value '" + lf.Value + "' is an invalid length facet specification");
				}
			}
		}

		private void checkMaxLengthFacet(XmlSchemaMaxLengthFacet maxlf, XmlSchemaFacet.Facet facetsDefined, ValidationEventHandler h)
		{
			if (maxlf != null)
			{
				try
				{
					if ((facetsDefined & XmlSchemaFacet.Facet.length) != XmlSchemaFacet.Facet.None)
					{
						maxlf.error(h, "It is an error for both length and minLength or maxLength to be present.");
					}
					else
					{
						decimal d = decimal.Parse(maxlf.Value.Trim(), XmlSchemaSimpleTypeRestriction.lengthStyle, CultureInfo.InvariantCulture);
						if ((this.fixedFacets & XmlSchemaFacet.Facet.maxLength) != XmlSchemaFacet.Facet.None && d != this.maxLengthFacet)
						{
							maxlf.error(h, string.Format(CultureInfo.InvariantCulture, "The value '{0}' is not the same as the fixed value '{1}' on the base type", new object[]
							{
								maxlf.Value.Trim(),
								this.maxLengthFacet
							}));
						}
						if (this.maxLengthFacet > 0m && d > this.maxLengthFacet)
						{
							maxlf.error(h, string.Format(CultureInfo.InvariantCulture, "The value '{0}' is not a valid restriction of the value '{1}' on the base maxLength facet", new object[]
							{
								maxlf.Value.Trim(),
								this.maxLengthFacet
							}));
						}
						else
						{
							this.maxLengthFacet = d;
						}
						if (this.maxLengthFacet < 0m)
						{
							maxlf.error(h, "The value '" + this.maxLengthFacet + "' is an invalid maxLength");
						}
						if (this.minLengthFacet >= 0m && this.minLengthFacet > this.maxLengthFacet)
						{
							maxlf.error(h, "minLength is greater than maxLength.");
						}
					}
				}
				catch (FormatException)
				{
					maxlf.error(h, "The value '" + maxlf.Value + "' is an invalid maxLength facet specification");
				}
			}
		}

		private void checkMinLengthFacet(XmlSchemaMinLengthFacet minlf, XmlSchemaFacet.Facet facetsDefined, ValidationEventHandler h)
		{
			if (minlf != null)
			{
				try
				{
					if (this.lengthFacet >= 0m)
					{
						minlf.error(h, "It is an error for both length and minLength or maxLength to be present.");
					}
					else
					{
						decimal d = decimal.Parse(minlf.Value.Trim(), XmlSchemaSimpleTypeRestriction.lengthStyle, CultureInfo.InvariantCulture);
						if ((this.fixedFacets & XmlSchemaFacet.Facet.minLength) != XmlSchemaFacet.Facet.None && d != this.minLengthFacet)
						{
							minlf.error(h, string.Format(CultureInfo.InvariantCulture, "The value '{0}' is not the same as the fixed value '{1}' on the base type", new object[]
							{
								minlf.Value.Trim(),
								this.minLengthFacet
							}));
						}
						if (d < this.minLengthFacet)
						{
							minlf.error(h, string.Format(CultureInfo.InvariantCulture, "The value '{0}' is not a valid restriction of the value '{1}' on the base minLength facet", new object[]
							{
								minlf.Value.Trim(),
								this.minLengthFacet
							}));
						}
						else
						{
							this.minLengthFacet = d;
						}
						if (this.minLengthFacet < 0m)
						{
							minlf.error(h, "The value '" + this.minLengthFacet + "' is an invalid minLength");
						}
						if (this.maxLengthFacet >= 0m && this.minLengthFacet > this.maxLengthFacet)
						{
							minlf.error(h, "minLength is greater than maxLength.");
						}
					}
				}
				catch (FormatException)
				{
					minlf.error(h, "The value '" + minlf.Value + "' is an invalid minLength facet specification");
				}
			}
		}

		private XsdAnySimpleType getDatatype()
		{
			XsdAnySimpleType xsdAnySimpleType = base.ActualBaseSchemaType as XsdAnySimpleType;
			if (xsdAnySimpleType != null)
			{
				return xsdAnySimpleType;
			}
			XmlSchemaSimpleTypeContent content = ((XmlSchemaSimpleType)base.ActualBaseSchemaType).Content;
			if (content is XmlSchemaSimpleTypeRestriction)
			{
				return ((XmlSchemaSimpleTypeRestriction)content).getDatatype();
			}
			if (content is XmlSchemaSimpleTypeList || content is XmlSchemaSimpleTypeUnion)
			{
				return null;
			}
			return null;
		}

		private object ValidateValueWithDatatype(string value)
		{
			XsdAnySimpleType datatype = this.getDatatype();
			object result = null;
			if (datatype != null)
			{
				try
				{
					result = datatype.ParseValue(value, null, null);
					if (base.ActualBaseSchemaType is XmlSchemaSimpleType)
					{
						XmlSchemaSimpleTypeContent content = ((XmlSchemaSimpleType)base.ActualBaseSchemaType).Content;
						if (content is XmlSchemaSimpleTypeRestriction)
						{
							if (((XmlSchemaSimpleTypeRestriction)content).ValidateValueWithFacets(value, null, null))
							{
								return result;
							}
							return null;
						}
					}
				}
				catch (Exception)
				{
					return null;
				}
				return result;
			}
			return result;
		}

		internal bool ValidateValueWithFacets(string value, XmlNameTable nt, IXmlNamespaceResolver nsmgr)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = base.ActualBaseSchemaType as XmlSchemaSimpleType;
			XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = (xmlSchemaSimpleType == null) ? null : (xmlSchemaSimpleType.Content as XmlSchemaSimpleTypeList);
			if (xmlSchemaSimpleTypeList != null)
			{
				return this.ValidateListValueWithFacets(value, nt, nsmgr);
			}
			return this.ValidateNonListValueWithFacets(value, nt, nsmgr);
		}

		private bool ValidateListValueWithFacets(string value, XmlNameTable nt, IXmlNamespaceResolver nsmgr)
		{
			bool result;
			try
			{
				result = this.ValidateListValueWithFacetsCore(value, nt, nsmgr);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		private bool ValidateListValueWithFacetsCore(string value, XmlNameTable nt, IXmlNamespaceResolver nsmgr)
		{
			string[] array = ((XsdAnySimpleType)XmlSchemaDatatype.FromName("anySimpleType", "http://www.w3.org/2001/XMLSchema")).ParseListValue(value, nt);
			if (this.patternFacetValues != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					for (int j = 0; j < this.patternFacetValues.Length; j++)
					{
						if (this.rexPatterns[j] != null && !this.rexPatterns[j].IsMatch(array[i]))
						{
							return false;
						}
					}
				}
			}
			bool flag = false;
			if (this.enumarationFacetValues != null)
			{
				for (int k = 0; k < array.Length; k++)
				{
					for (int l = 0; l < this.enumarationFacetValues.Length; l++)
					{
						if (array[k] == this.enumarationFacetValues[l])
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag && this.enumarationFacetValues != null)
			{
				for (int m = 0; m < array.Length; m++)
				{
					XsdAnySimpleType xsdAnySimpleType = this.getDatatype();
					if (xsdAnySimpleType == null)
					{
						xsdAnySimpleType = (XsdAnySimpleType)XmlSchemaDatatype.FromName("anySimpleType", "http://www.w3.org/2001/XMLSchema");
					}
					object v = xsdAnySimpleType.ParseValue(array[m], nt, nsmgr);
					for (int n = 0; n < this.enumarationFacetValues.Length; n++)
					{
						if (XmlSchemaUtil.AreSchemaDatatypeEqual(xsdAnySimpleType, v, xsdAnySimpleType, xsdAnySimpleType.ParseValue(this.enumarationFacetValues[n], nt, nsmgr)))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			return (!(this.lengthFacet >= 0m) || !(array.Length != this.lengthFacet)) && (!(this.maxLengthFacet >= 0m) || !(array.Length > this.maxLengthFacet)) && (!(this.minLengthFacet >= 0m) || !(array.Length < this.minLengthFacet));
		}

		private bool ValidateNonListValueWithFacets(string value, XmlNameTable nt, IXmlNamespaceResolver nsmgr)
		{
			bool result;
			try
			{
				result = this.ValidateNonListValueWithFacetsCore(value, nt, nsmgr);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		private bool ValidateNonListValueWithFacetsCore(string value, XmlNameTable nt, IXmlNamespaceResolver nsmgr)
		{
			if (this.patternFacetValues != null)
			{
				bool flag = false;
				for (int i = 0; i < this.patternFacetValues.Length; i++)
				{
					if (this.rexPatterns[i] != null && this.rexPatterns[i].IsMatch(value))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			XsdAnySimpleType datatype = this.getDatatype();
			bool flag2 = false;
			if (this.enumarationFacetValues != null)
			{
				for (int j = 0; j < this.enumarationFacetValues.Length; j++)
				{
					if (value == this.enumarationFacetValues[j])
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2 && this.enumarationFacetValues != null)
			{
				XsdAnySimpleType xsdAnySimpleType = datatype;
				if (xsdAnySimpleType == null)
				{
					xsdAnySimpleType = (XsdAnySimpleType)XmlSchemaDatatype.FromName("anySimpleType", "http://www.w3.org/2001/XMLSchema");
				}
				object v = xsdAnySimpleType.ParseValue(value, nt, nsmgr);
				for (int k = 0; k < this.enumarationFacetValues.Length; k++)
				{
					if (XmlSchemaUtil.AreSchemaDatatypeEqual(xsdAnySimpleType, v, xsdAnySimpleType, xsdAnySimpleType.ParseValue(this.enumarationFacetValues[k], nt, nsmgr)))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					return false;
				}
			}
			if (!(datatype is XsdQName) && !(datatype is XsdNotation) && (!(this.lengthFacet == -1m) || !(this.maxLengthFacet == -1m) || !(this.minLengthFacet == -1m)))
			{
				int value2 = datatype.Length(value);
				if (this.lengthFacet >= 0m && value2 != this.lengthFacet)
				{
					return false;
				}
				if (this.maxLengthFacet >= 0m && value2 > this.maxLengthFacet)
				{
					return false;
				}
				if (this.minLengthFacet >= 0m && value2 < this.minLengthFacet)
				{
					return false;
				}
			}
			if (this.totalDigitsFacet >= 0m || this.fractionDigitsFacet >= 0m)
			{
				string text = value.Trim(new char[]
				{
					'+',
					'-',
					'0',
					'.'
				});
				int value3 = 0;
				int num = text.Length;
				int num2 = text.IndexOf(".");
				if (num2 != -1)
				{
					num--;
					value3 = text.Length - num2 - 1;
				}
				if (this.totalDigitsFacet >= 0m && num > this.totalDigitsFacet)
				{
					return false;
				}
				if (this.fractionDigitsFacet >= 0m && value3 > this.fractionDigitsFacet)
				{
					return false;
				}
			}
			if ((this.maxInclusiveFacet != null || this.maxExclusiveFacet != null || this.minInclusiveFacet != null || this.minExclusiveFacet != null) && datatype != null)
			{
				object x;
				try
				{
					x = datatype.ParseValue(value, nt, null);
				}
				catch (OverflowException)
				{
					return false;
				}
				catch (FormatException)
				{
					return false;
				}
				if (this.maxInclusiveFacet != null)
				{
					XsdOrdering xsdOrdering = datatype.Compare(x, this.maxInclusiveFacet);
					if (xsdOrdering != XsdOrdering.LessThan && xsdOrdering != XsdOrdering.Equal)
					{
						return false;
					}
				}
				if (this.maxExclusiveFacet != null)
				{
					XsdOrdering xsdOrdering2 = datatype.Compare(x, this.maxExclusiveFacet);
					if (xsdOrdering2 != XsdOrdering.LessThan)
					{
						return false;
					}
				}
				if (this.minInclusiveFacet != null)
				{
					XsdOrdering xsdOrdering3 = datatype.Compare(x, this.minInclusiveFacet);
					if (xsdOrdering3 != XsdOrdering.GreaterThan && xsdOrdering3 != XsdOrdering.Equal)
					{
						return false;
					}
				}
				if (this.minExclusiveFacet == null)
				{
					return true;
				}
				XsdOrdering xsdOrdering4 = datatype.Compare(x, this.minExclusiveFacet);
				if (xsdOrdering4 != XsdOrdering.GreaterThan)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		internal static XmlSchemaSimpleTypeRestriction Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "restriction")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaSimpleTypeRestriction.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaSimpleTypeRestriction.LineNumber = reader.LineNumber;
			xmlSchemaSimpleTypeRestriction.LinePosition = reader.LinePosition;
			xmlSchemaSimpleTypeRestriction.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaSimpleTypeRestriction.Id = reader.Value;
				}
				else if (reader.Name == "base")
				{
					Exception ex;
					xmlSchemaSimpleTypeRestriction.baseTypeName = XmlSchemaUtil.ReadQNameAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is not a valid value for base attribute", ex);
					}
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for restriction", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaSimpleTypeRestriction);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaSimpleTypeRestriction;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "restriction")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaSimpleTypeRestriction.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaSimpleTypeRestriction.Annotation = xmlSchemaAnnotation;
					}
				}
				else if (num <= 2 && reader.LocalName == "simpleType")
				{
					num = 3;
					XmlSchemaSimpleType xmlSchemaSimpleType = XmlSchemaSimpleType.Read(reader, h);
					if (xmlSchemaSimpleType != null)
					{
						xmlSchemaSimpleTypeRestriction.baseType = xmlSchemaSimpleType;
					}
				}
				else
				{
					if (num <= 3)
					{
						if (reader.LocalName == "minExclusive")
						{
							num = 3;
							XmlSchemaMinExclusiveFacet xmlSchemaMinExclusiveFacet = XmlSchemaMinExclusiveFacet.Read(reader, h);
							if (xmlSchemaMinExclusiveFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaMinExclusiveFacet);
							}
							continue;
						}
						if (reader.LocalName == "minInclusive")
						{
							num = 3;
							XmlSchemaMinInclusiveFacet xmlSchemaMinInclusiveFacet = XmlSchemaMinInclusiveFacet.Read(reader, h);
							if (xmlSchemaMinInclusiveFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaMinInclusiveFacet);
							}
							continue;
						}
						if (reader.LocalName == "maxExclusive")
						{
							num = 3;
							XmlSchemaMaxExclusiveFacet xmlSchemaMaxExclusiveFacet = XmlSchemaMaxExclusiveFacet.Read(reader, h);
							if (xmlSchemaMaxExclusiveFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaMaxExclusiveFacet);
							}
							continue;
						}
						if (reader.LocalName == "maxInclusive")
						{
							num = 3;
							XmlSchemaMaxInclusiveFacet xmlSchemaMaxInclusiveFacet = XmlSchemaMaxInclusiveFacet.Read(reader, h);
							if (xmlSchemaMaxInclusiveFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaMaxInclusiveFacet);
							}
							continue;
						}
						if (reader.LocalName == "totalDigits")
						{
							num = 3;
							XmlSchemaTotalDigitsFacet xmlSchemaTotalDigitsFacet = XmlSchemaTotalDigitsFacet.Read(reader, h);
							if (xmlSchemaTotalDigitsFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaTotalDigitsFacet);
							}
							continue;
						}
						if (reader.LocalName == "fractionDigits")
						{
							num = 3;
							XmlSchemaFractionDigitsFacet xmlSchemaFractionDigitsFacet = XmlSchemaFractionDigitsFacet.Read(reader, h);
							if (xmlSchemaFractionDigitsFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaFractionDigitsFacet);
							}
							continue;
						}
						if (reader.LocalName == "length")
						{
							num = 3;
							XmlSchemaLengthFacet xmlSchemaLengthFacet = XmlSchemaLengthFacet.Read(reader, h);
							if (xmlSchemaLengthFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaLengthFacet);
							}
							continue;
						}
						if (reader.LocalName == "minLength")
						{
							num = 3;
							XmlSchemaMinLengthFacet xmlSchemaMinLengthFacet = XmlSchemaMinLengthFacet.Read(reader, h);
							if (xmlSchemaMinLengthFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaMinLengthFacet);
							}
							continue;
						}
						if (reader.LocalName == "maxLength")
						{
							num = 3;
							XmlSchemaMaxLengthFacet xmlSchemaMaxLengthFacet = XmlSchemaMaxLengthFacet.Read(reader, h);
							if (xmlSchemaMaxLengthFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaMaxLengthFacet);
							}
							continue;
						}
						if (reader.LocalName == "enumeration")
						{
							num = 3;
							XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet = XmlSchemaEnumerationFacet.Read(reader, h);
							if (xmlSchemaEnumerationFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaEnumerationFacet);
							}
							continue;
						}
						if (reader.LocalName == "whiteSpace")
						{
							num = 3;
							XmlSchemaWhiteSpaceFacet xmlSchemaWhiteSpaceFacet = XmlSchemaWhiteSpaceFacet.Read(reader, h);
							if (xmlSchemaWhiteSpaceFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaWhiteSpaceFacet);
							}
							continue;
						}
						if (reader.LocalName == "pattern")
						{
							num = 3;
							XmlSchemaPatternFacet xmlSchemaPatternFacet = XmlSchemaPatternFacet.Read(reader, h);
							if (xmlSchemaPatternFacet != null)
							{
								xmlSchemaSimpleTypeRestriction.facets.Add(xmlSchemaPatternFacet);
							}
							continue;
						}
					}
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaSimpleTypeRestriction;
		}
	}
}
