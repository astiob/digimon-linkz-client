using System;
using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the choice element (compositor) from the XML Schema as specified by the World Wide Web Consortium (W3C). The choice allows only one of its children to appear in an instance. </summary>
	public class XmlSchemaChoice : XmlSchemaGroupBase
	{
		private const string xmlname = "choice";

		private XmlSchemaObjectCollection items;

		private decimal minEffectiveTotalRange = -1m;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaChoice" /> class.</summary>
		public XmlSchemaChoice()
		{
			this.items = new XmlSchemaObjectCollection();
		}

		/// <summary>Gets the collection of the elements contained with the compositor (choice): XmlSchemaElement, XmlSchemaGroupRef, XmlSchemaChoice, XmlSchemaSequence, or XmlSchemaAny.</summary>
		/// <returns>The collection of elements contained within XmlSchemaChoice.</returns>
		[XmlElement("sequence", typeof(XmlSchemaSequence))]
		[XmlElement("any", typeof(XmlSchemaAny))]
		[XmlElement("choice", typeof(XmlSchemaChoice))]
		[XmlElement("group", typeof(XmlSchemaGroupRef))]
		[XmlElement("element", typeof(XmlSchemaElement))]
		public override XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		internal override void SetParent(XmlSchemaObject parent)
		{
			base.SetParent(parent);
			foreach (XmlSchemaObject xmlSchemaObject in this.Items)
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
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			base.CompileOccurence(h, schema);
			if (this.Items.Count == 0)
			{
				base.warn(h, "Empty choice is unsatisfiable if minOccurs not equals to 0");
			}
			foreach (XmlSchemaObject xmlSchemaObject in this.Items)
			{
				if (xmlSchemaObject is XmlSchemaElement || xmlSchemaObject is XmlSchemaGroupRef || xmlSchemaObject is XmlSchemaChoice || xmlSchemaObject is XmlSchemaSequence || xmlSchemaObject is XmlSchemaAny)
				{
					this.errorCount += xmlSchemaObject.Compile(h, schema);
				}
				else
				{
					base.error(h, "Invalid schema object was specified in the particles of the choice model group.");
				}
			}
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		internal override XmlSchemaParticle GetOptimizedParticle(bool isTop)
		{
			if (this.OptimizedParticle != null)
			{
				return this.OptimizedParticle;
			}
			if (this.Items.Count == 0 || base.ValidatedMaxOccurs == 0m)
			{
				this.OptimizedParticle = XmlSchemaParticle.Empty;
			}
			else if (!isTop && this.Items.Count == 1 && base.ValidatedMinOccurs == 1m && base.ValidatedMaxOccurs == 1m)
			{
				this.OptimizedParticle = ((XmlSchemaParticle)this.Items[0]).GetOptimizedParticle(false);
			}
			else
			{
				XmlSchemaChoice xmlSchemaChoice = new XmlSchemaChoice();
				this.CopyInfo(xmlSchemaChoice);
				for (int i = 0; i < this.Items.Count; i++)
				{
					XmlSchemaParticle xmlSchemaParticle = this.Items[i] as XmlSchemaParticle;
					xmlSchemaParticle = xmlSchemaParticle.GetOptimizedParticle(false);
					if (xmlSchemaParticle != XmlSchemaParticle.Empty)
					{
						if (xmlSchemaParticle is XmlSchemaChoice && xmlSchemaParticle.ValidatedMinOccurs == 1m && xmlSchemaParticle.ValidatedMaxOccurs == 1m)
						{
							XmlSchemaChoice xmlSchemaChoice2 = xmlSchemaParticle as XmlSchemaChoice;
							for (int j = 0; j < xmlSchemaChoice2.Items.Count; j++)
							{
								xmlSchemaChoice.Items.Add(xmlSchemaChoice2.Items[j]);
								xmlSchemaChoice.CompiledItems.Add(xmlSchemaChoice2.Items[j]);
							}
						}
						else
						{
							xmlSchemaChoice.Items.Add(xmlSchemaParticle);
							xmlSchemaChoice.CompiledItems.Add(xmlSchemaParticle);
						}
					}
				}
				if (xmlSchemaChoice.Items.Count == 0)
				{
					this.OptimizedParticle = XmlSchemaParticle.Empty;
				}
				else
				{
					this.OptimizedParticle = xmlSchemaChoice;
				}
			}
			return this.OptimizedParticle;
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			if (base.IsValidated(schema.CompilationId))
			{
				return this.errorCount;
			}
			base.CompiledItems.Clear();
			foreach (XmlSchemaObject xmlSchemaObject in this.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				this.errorCount += xmlSchemaParticle.Validate(h, schema);
				base.CompiledItems.Add(xmlSchemaParticle);
			}
			this.ValidationId = schema.ValidationId;
			return this.errorCount;
		}

		internal override bool ValidateDerivationByRestriction(XmlSchemaParticle baseParticle, ValidationEventHandler h, XmlSchema schema, bool raiseError)
		{
			XmlSchemaAny xmlSchemaAny = baseParticle as XmlSchemaAny;
			if (xmlSchemaAny != null)
			{
				return base.ValidateNSRecurseCheckCardinality(xmlSchemaAny, h, schema, raiseError);
			}
			XmlSchemaChoice xmlSchemaChoice = baseParticle as XmlSchemaChoice;
			if (xmlSchemaChoice != null)
			{
				return this.ValidateOccurenceRangeOK(xmlSchemaChoice, h, schema, raiseError) && ((xmlSchemaChoice.ValidatedMinOccurs == 0m && xmlSchemaChoice.ValidatedMaxOccurs == 0m && base.ValidatedMinOccurs == 0m && base.ValidatedMaxOccurs == 0m) || base.ValidateSeqRecurseMapSumCommon(xmlSchemaChoice, h, schema, true, false, raiseError));
			}
			if (raiseError)
			{
				base.error(h, "Invalid choice derivation by restriction was found.");
			}
			return false;
		}

		internal override decimal GetMinEffectiveTotalRange()
		{
			if (this.minEffectiveTotalRange >= 0m)
			{
				return this.minEffectiveTotalRange;
			}
			decimal num = 0m;
			if (this.Items.Count == 0)
			{
				num = 0m;
			}
			else
			{
				foreach (XmlSchemaObject xmlSchemaObject in this.Items)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					decimal num2 = xmlSchemaParticle.GetMinEffectiveTotalRange();
					if (num > num2)
					{
						num = num2;
					}
				}
			}
			this.minEffectiveTotalRange = num;
			return num;
		}

		internal override void ValidateUniqueParticleAttribution(XmlSchemaObjectTable qnames, ArrayList nsNames, ValidationEventHandler h, XmlSchema schema)
		{
			foreach (XmlSchemaObject xmlSchemaObject in this.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				xmlSchemaParticle.ValidateUniqueParticleAttribution(qnames, nsNames, h, schema);
			}
		}

		internal override void ValidateUniqueTypeAttribution(XmlSchemaObjectTable labels, ValidationEventHandler h, XmlSchema schema)
		{
			foreach (XmlSchemaObject xmlSchemaObject in this.Items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				xmlSchemaParticle.ValidateUniqueTypeAttribution(labels, h, schema);
			}
		}

		internal static XmlSchemaChoice Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaChoice xmlSchemaChoice = new XmlSchemaChoice();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "choice")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaChoice.Read, name=" + reader.Name, null);
				reader.SkipToEnd();
				return null;
			}
			xmlSchemaChoice.LineNumber = reader.LineNumber;
			xmlSchemaChoice.LinePosition = reader.LinePosition;
			xmlSchemaChoice.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaChoice.Id = reader.Value;
				}
				else if (reader.Name == "maxOccurs")
				{
					try
					{
						xmlSchemaChoice.MaxOccursString = reader.Value;
					}
					catch (Exception innerException)
					{
						XmlSchemaObject.error(h, reader.Value + " is an invalid value for maxOccurs", innerException);
					}
				}
				else if (reader.Name == "minOccurs")
				{
					try
					{
						xmlSchemaChoice.MinOccursString = reader.Value;
					}
					catch (Exception innerException2)
					{
						XmlSchemaObject.error(h, reader.Value + " is an invalid value for minOccurs", innerException2);
					}
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for choice", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaChoice);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaChoice;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "choice")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaChoice.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaChoice.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					if (num <= 2)
					{
						if (reader.LocalName == "element")
						{
							num = 2;
							XmlSchemaElement xmlSchemaElement = XmlSchemaElement.Read(reader, h);
							if (xmlSchemaElement != null)
							{
								xmlSchemaChoice.items.Add(xmlSchemaElement);
							}
							continue;
						}
						if (reader.LocalName == "group")
						{
							num = 2;
							XmlSchemaGroupRef xmlSchemaGroupRef = XmlSchemaGroupRef.Read(reader, h);
							if (xmlSchemaGroupRef != null)
							{
								xmlSchemaChoice.items.Add(xmlSchemaGroupRef);
							}
							continue;
						}
						if (reader.LocalName == "choice")
						{
							num = 2;
							XmlSchemaChoice xmlSchemaChoice2 = XmlSchemaChoice.Read(reader, h);
							if (xmlSchemaChoice2 != null)
							{
								xmlSchemaChoice.items.Add(xmlSchemaChoice2);
							}
							continue;
						}
						if (reader.LocalName == "sequence")
						{
							num = 2;
							XmlSchemaSequence xmlSchemaSequence = XmlSchemaSequence.Read(reader, h);
							if (xmlSchemaSequence != null)
							{
								xmlSchemaChoice.items.Add(xmlSchemaSequence);
							}
							continue;
						}
						if (reader.LocalName == "any")
						{
							num = 2;
							XmlSchemaAny xmlSchemaAny = XmlSchemaAny.Read(reader, h);
							if (xmlSchemaAny != null)
							{
								xmlSchemaChoice.items.Add(xmlSchemaAny);
							}
							continue;
						}
					}
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaChoice;
		}
	}
}
