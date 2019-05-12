using System;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the element element from XML Schema as specified by the World Wide Web Consortium (W3C). This class is the base class for all particle types and is used to describe an element in an XML document.</summary>
	public class XmlSchemaElement : XmlSchemaParticle
	{
		private const string xmlname = "element";

		private XmlSchemaDerivationMethod block;

		private XmlSchemaObjectCollection constraints;

		private string defaultValue;

		private object elementType;

		private XmlSchemaType elementSchemaType;

		private XmlSchemaDerivationMethod final;

		private string fixedValue;

		private XmlSchemaForm form;

		private bool isAbstract;

		private bool isNillable;

		private string name;

		private XmlQualifiedName refName;

		private XmlSchemaType schemaType;

		private XmlQualifiedName schemaTypeName;

		private XmlQualifiedName substitutionGroup;

		private XmlSchema schema;

		internal bool parentIsSchema;

		private XmlQualifiedName qName;

		private XmlSchemaDerivationMethod blockResolved;

		private XmlSchemaDerivationMethod finalResolved;

		private XmlSchemaElement referencedElement;

		private ArrayList substitutingElements = new ArrayList();

		private XmlSchemaElement substitutionGroupElement;

		private bool actualIsAbstract;

		private bool actualIsNillable;

		private string validatedDefaultValue;

		private string validatedFixedValue;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaElement" /> class.</summary>
		public XmlSchemaElement()
		{
			this.block = XmlSchemaDerivationMethod.None;
			this.final = XmlSchemaDerivationMethod.None;
			this.constraints = new XmlSchemaObjectCollection();
			this.refName = XmlQualifiedName.Empty;
			this.schemaTypeName = XmlQualifiedName.Empty;
			this.substitutionGroup = XmlQualifiedName.Empty;
			this.InitPostCompileInformations();
		}

		private void InitPostCompileInformations()
		{
			this.qName = XmlQualifiedName.Empty;
			this.schema = null;
			this.blockResolved = XmlSchemaDerivationMethod.None;
			this.finalResolved = XmlSchemaDerivationMethod.None;
			this.referencedElement = null;
			this.substitutingElements.Clear();
			this.substitutionGroupElement = null;
			this.actualIsAbstract = false;
			this.actualIsNillable = false;
			this.validatedDefaultValue = null;
			this.validatedFixedValue = null;
		}

		/// <summary>Gets or sets information to indicate if the element can be used in an instance document.</summary>
		/// <returns>If true, the element cannot appear in the instance document. The default is false.Optional.</returns>
		[DefaultValue(false)]
		[XmlAttribute("abstract")]
		public bool IsAbstract
		{
			get
			{
				return this.isAbstract;
			}
			set
			{
				this.isAbstract = value;
			}
		}

		/// <summary>Gets or sets a Block derivation.</summary>
		/// <returns>The attribute used to block a type derivation. Default value is XmlSchemaDerivationMethod.None.Optional.</returns>
		[XmlAttribute("block")]
		[DefaultValue(XmlSchemaDerivationMethod.None)]
		public XmlSchemaDerivationMethod Block
		{
			get
			{
				return this.block;
			}
			set
			{
				this.block = value;
			}
		}

		/// <summary>Gets or sets the default value of the element if its content is a simple type or content of the element is textOnly.</summary>
		/// <returns>The default value for the element. The default is a null reference.Optional.</returns>
		[XmlAttribute("default")]
		[DefaultValue(null)]
		public string DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
			set
			{
				this.defaultValue = value;
			}
		}

		/// <summary>Gets or sets the Final property to indicate that no further derivations are allowed.</summary>
		/// <returns>The Final property. The default is XmlSchemaDerivationMethod.None.Optional.</returns>
		[XmlAttribute("final")]
		[DefaultValue(XmlSchemaDerivationMethod.None)]
		public XmlSchemaDerivationMethod Final
		{
			get
			{
				return this.final;
			}
			set
			{
				this.final = value;
			}
		}

		/// <summary>Gets or sets the fixed value.</summary>
		/// <returns>The fixed value that is predetermined and unchangeable. The default is a null reference.Optional.</returns>
		[XmlAttribute("fixed")]
		[DefaultValue(null)]
		public string FixedValue
		{
			get
			{
				return this.fixedValue;
			}
			set
			{
				this.fixedValue = value;
			}
		}

		/// <summary>Gets or sets the form for the element.</summary>
		/// <returns>The form for the element. The default is the <see cref="P:System.Xml.Schema.XmlSchema.ElementFormDefault" /> value.Optional.</returns>
		[XmlAttribute("form")]
		[DefaultValue(XmlSchemaForm.None)]
		public XmlSchemaForm Form
		{
			get
			{
				return this.form;
			}
			set
			{
				this.form = value;
			}
		}

		/// <summary>Gets or sets the name of the element.</summary>
		/// <returns>The name of the element. The default is String.Empty.</returns>
		[XmlAttribute("name")]
		[DefaultValue("")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>Gets or sets information that indicates if xsi:nil can occur in the instance data. Indicates if an explicit nil value can be assigned to the element.</summary>
		/// <returns>If nillable is true, this enables an instance of the element to have the nil attribute set to true. The nil attribute is defined as part of the XML Schema namespace for instances. The default is false.Optional.</returns>
		[XmlAttribute("nillable")]
		[DefaultValue(false)]
		public bool IsNillable
		{
			get
			{
				return this.isNillable;
			}
			set
			{
				this.isNillable = value;
			}
		}

		/// <summary>Gets or sets the reference name of an element declared in this schema (or another schema indicated by the specified namespace).</summary>
		/// <returns>The reference name of the element.</returns>
		[XmlAttribute("ref")]
		public XmlQualifiedName RefName
		{
			get
			{
				return this.refName;
			}
			set
			{
				this.refName = value;
			}
		}

		/// <summary>Gets or sets the name of an element that is being substituted by this element.</summary>
		/// <returns>The qualified name of an element that is being substituted by this element.Optional.</returns>
		[XmlAttribute("substitutionGroup")]
		public XmlQualifiedName SubstitutionGroup
		{
			get
			{
				return this.substitutionGroup;
			}
			set
			{
				this.substitutionGroup = value;
			}
		}

		/// <summary>Gets or sets the name of a built-in data type defined in this schema or another schema indicated by the specified namespace.</summary>
		/// <returns>The name of the built-in data type.</returns>
		[XmlAttribute("type")]
		public XmlQualifiedName SchemaTypeName
		{
			get
			{
				return this.schemaTypeName;
			}
			set
			{
				this.schemaTypeName = value;
			}
		}

		/// <summary>Gets or sets the type of the element. This can either be a complex type or a simple type.</summary>
		/// <returns>The type of the element.</returns>
		[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
		[XmlElement("complexType", typeof(XmlSchemaComplexType))]
		public XmlSchemaType SchemaType
		{
			get
			{
				return this.schemaType;
			}
			set
			{
				this.schemaType = value;
			}
		}

		/// <summary>Gets the collection of constraints on the element.</summary>
		/// <returns>The collection of constraints.</returns>
		[XmlElement("keyref", typeof(XmlSchemaKeyref))]
		[XmlElement("unique", typeof(XmlSchemaUnique))]
		[XmlElement("key", typeof(XmlSchemaKey))]
		public XmlSchemaObjectCollection Constraints
		{
			get
			{
				return this.constraints;
			}
		}

		/// <summary>Gets the actual qualified name for the given element. </summary>
		/// <returns>The qualified name of the element. The post-compilation value of the QualifiedName property.</returns>
		[XmlIgnore]
		public XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qName;
			}
		}

		/// <summary>Gets a common language runtime (CLR) object based on the <see cref="T:System.Xml.Schema.XmlSchemaElement" /> or <see cref="T:System.Xml.Schema.XmlSchemaElement" /> of the element, which holds the post-compilation value of the ElementType property.</summary>
		/// <returns>The common language runtime object. The post-compilation value of the ElementType property.</returns>
		[XmlIgnore]
		[Obsolete]
		public object ElementType
		{
			get
			{
				if (this.referencedElement != null)
				{
					return this.referencedElement.ElementType;
				}
				return this.elementType;
			}
		}

		/// <summary>Gets an <see cref="T:System.Xml.Schema.XmlSchemaType" /> object representing the type of the element based on the <see cref="P:System.Xml.Schema.XmlSchemaElement.SchemaType" /> or <see cref="P:System.Xml.Schema.XmlSchemaElement.SchemaTypeName" /> values of the element.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaType" /> object.</returns>
		[XmlIgnore]
		public XmlSchemaType ElementSchemaType
		{
			get
			{
				if (this.referencedElement != null)
				{
					return this.referencedElement.ElementSchemaType;
				}
				return this.elementSchemaType;
			}
		}

		/// <summary>Gets the post-compilation value of the Block property.</summary>
		/// <returns>The post-compilation value of the Block property. The default is the BlockDefault value on the schema element.</returns>
		[XmlIgnore]
		public XmlSchemaDerivationMethod BlockResolved
		{
			get
			{
				if (this.referencedElement != null)
				{
					return this.referencedElement.BlockResolved;
				}
				return this.blockResolved;
			}
		}

		/// <summary>Gets the post-compilation value of the Final property.</summary>
		/// <returns>The post-compilation value of the Final property. Default value is the FinalDefault value on the schema element.</returns>
		[XmlIgnore]
		public XmlSchemaDerivationMethod FinalResolved
		{
			get
			{
				if (this.referencedElement != null)
				{
					return this.referencedElement.FinalResolved;
				}
				return this.finalResolved;
			}
		}

		internal bool ActualIsNillable
		{
			get
			{
				if (this.referencedElement != null)
				{
					return this.referencedElement.ActualIsNillable;
				}
				return this.actualIsNillable;
			}
		}

		internal bool ActualIsAbstract
		{
			get
			{
				if (this.referencedElement != null)
				{
					return this.referencedElement.ActualIsAbstract;
				}
				return this.actualIsAbstract;
			}
		}

		internal string ValidatedDefaultValue
		{
			get
			{
				if (this.referencedElement != null)
				{
					return this.referencedElement.ValidatedDefaultValue;
				}
				return this.validatedDefaultValue;
			}
		}

		internal string ValidatedFixedValue
		{
			get
			{
				if (this.referencedElement != null)
				{
					return this.referencedElement.ValidatedFixedValue;
				}
				return this.validatedFixedValue;
			}
		}

		internal ArrayList SubstitutingElements
		{
			get
			{
				if (this.referencedElement != null)
				{
					return this.referencedElement.SubstitutingElements;
				}
				return this.substitutingElements;
			}
		}

		internal XmlSchemaElement SubstitutionGroupElement
		{
			get
			{
				if (this.referencedElement != null)
				{
					return this.referencedElement.SubstitutionGroupElement;
				}
				return this.substitutionGroupElement;
			}
		}

		internal override void SetParent(XmlSchemaObject parent)
		{
			base.SetParent(parent);
			if (this.SchemaType != null)
			{
				this.SchemaType.SetParent(this);
			}
			foreach (XmlSchemaObject xmlSchemaObject in this.Constraints)
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
			this.InitPostCompileInformations();
			this.schema = schema;
			if (this.defaultValue != null && this.fixedValue != null)
			{
				base.error(h, "both default and fixed can't be present");
			}
			if (this.parentIsSchema || this.isRedefineChild)
			{
				if (this.refName != null && !this.RefName.IsEmpty)
				{
					base.error(h, "ref must be absent");
				}
				if (this.name == null)
				{
					base.error(h, "Required attribute name must be present");
				}
				else if (!XmlSchemaUtil.CheckNCName(this.name))
				{
					base.error(h, "attribute name must be NCName");
				}
				else
				{
					this.qName = new XmlQualifiedName(this.name, base.AncestorSchema.TargetNamespace);
				}
				if (this.form != XmlSchemaForm.None)
				{
					base.error(h, "form must be absent");
				}
				if (base.MinOccursString != null)
				{
					base.error(h, "minOccurs must be absent");
				}
				if (base.MaxOccursString != null)
				{
					base.error(h, "maxOccurs must be absent");
				}
				XmlSchemaDerivationMethod xmlSchemaDerivationMethod = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;
				if (this.final == XmlSchemaDerivationMethod.All)
				{
					this.finalResolved = xmlSchemaDerivationMethod;
				}
				else if (this.final == XmlSchemaDerivationMethod.None)
				{
					this.finalResolved = XmlSchemaDerivationMethod.Empty;
				}
				else
				{
					if ((this.final | XmlSchemaUtil.FinalAllowed) != XmlSchemaUtil.FinalAllowed)
					{
						base.error(h, "some values for final are invalid in this context");
					}
					this.finalResolved = (this.final & xmlSchemaDerivationMethod);
				}
				if (this.schemaType != null && this.schemaTypeName != null && !this.schemaTypeName.IsEmpty)
				{
					base.error(h, "both schemaType and content can't be present");
				}
				if (this.schemaType != null)
				{
					if (this.schemaType is XmlSchemaSimpleType)
					{
						this.errorCount += ((XmlSchemaSimpleType)this.schemaType).Compile(h, schema);
					}
					else if (this.schemaType is XmlSchemaComplexType)
					{
						this.errorCount += ((XmlSchemaComplexType)this.schemaType).Compile(h, schema);
					}
					else
					{
						base.error(h, "only simpletype or complextype is allowed");
					}
				}
				if (this.schemaTypeName != null && !this.schemaTypeName.IsEmpty && !XmlSchemaUtil.CheckQName(this.SchemaTypeName))
				{
					base.error(h, "SchemaTypeName must be an XmlQualifiedName");
				}
				if (this.SubstitutionGroup != null && !this.SubstitutionGroup.IsEmpty && !XmlSchemaUtil.CheckQName(this.SubstitutionGroup))
				{
					base.error(h, "SubstitutionGroup must be a valid XmlQualifiedName");
				}
				foreach (XmlSchemaObject xmlSchemaObject in this.constraints)
				{
					if (xmlSchemaObject is XmlSchemaUnique)
					{
						this.errorCount += ((XmlSchemaUnique)xmlSchemaObject).Compile(h, schema);
					}
					else if (xmlSchemaObject is XmlSchemaKey)
					{
						this.errorCount += ((XmlSchemaKey)xmlSchemaObject).Compile(h, schema);
					}
					else if (xmlSchemaObject is XmlSchemaKeyref)
					{
						this.errorCount += ((XmlSchemaKeyref)xmlSchemaObject).Compile(h, schema);
					}
				}
			}
			else
			{
				if (this.substitutionGroup != null && !this.substitutionGroup.IsEmpty)
				{
					base.error(h, "substitutionGroup must be absent");
				}
				if (this.final != XmlSchemaDerivationMethod.None)
				{
					base.error(h, "final must be absent");
				}
				base.CompileOccurence(h, schema);
				if (this.refName == null || this.RefName.IsEmpty)
				{
					string ns = string.Empty;
					if (this.form == XmlSchemaForm.Qualified || (this.form == XmlSchemaForm.None && base.AncestorSchema.ElementFormDefault == XmlSchemaForm.Qualified))
					{
						ns = base.AncestorSchema.TargetNamespace;
					}
					if (this.name == null)
					{
						base.error(h, "Required attribute name must be present");
					}
					else if (!XmlSchemaUtil.CheckNCName(this.name))
					{
						base.error(h, "attribute name must be NCName");
					}
					else
					{
						this.qName = new XmlQualifiedName(this.name, ns);
					}
					if (this.schemaType != null && this.schemaTypeName != null && !this.schemaTypeName.IsEmpty)
					{
						base.error(h, "both schemaType and content can't be present");
					}
					if (this.schemaType != null)
					{
						if (this.schemaType is XmlSchemaSimpleType)
						{
							this.errorCount += ((XmlSchemaSimpleType)this.schemaType).Compile(h, schema);
						}
						else if (this.schemaType is XmlSchemaComplexType)
						{
							this.errorCount += ((XmlSchemaComplexType)this.schemaType).Compile(h, schema);
						}
						else
						{
							base.error(h, "only simpletype or complextype is allowed");
						}
					}
					if (this.schemaTypeName != null && !this.schemaTypeName.IsEmpty && !XmlSchemaUtil.CheckQName(this.SchemaTypeName))
					{
						base.error(h, "SchemaTypeName must be an XmlQualifiedName");
					}
					if (this.SubstitutionGroup != null && !this.SubstitutionGroup.IsEmpty && !XmlSchemaUtil.CheckQName(this.SubstitutionGroup))
					{
						base.error(h, "SubstitutionGroup must be a valid XmlQualifiedName");
					}
					foreach (XmlSchemaObject xmlSchemaObject2 in this.constraints)
					{
						if (xmlSchemaObject2 is XmlSchemaUnique)
						{
							this.errorCount += ((XmlSchemaUnique)xmlSchemaObject2).Compile(h, schema);
						}
						else if (xmlSchemaObject2 is XmlSchemaKey)
						{
							this.errorCount += ((XmlSchemaKey)xmlSchemaObject2).Compile(h, schema);
						}
						else if (xmlSchemaObject2 is XmlSchemaKeyref)
						{
							this.errorCount += ((XmlSchemaKeyref)xmlSchemaObject2).Compile(h, schema);
						}
					}
				}
				else
				{
					if (!XmlSchemaUtil.CheckQName(this.RefName))
					{
						base.error(h, "RefName must be a XmlQualifiedName");
					}
					if (this.name != null)
					{
						base.error(h, "name must not be present when ref is present");
					}
					if (this.Constraints.Count != 0)
					{
						base.error(h, "key, keyref and unique must be absent");
					}
					if (this.isNillable)
					{
						base.error(h, "nillable must be absent");
					}
					if (this.defaultValue != null)
					{
						base.error(h, "default must be absent");
					}
					if (this.fixedValue != null)
					{
						base.error(h, "fixed must be null");
					}
					if (this.form != XmlSchemaForm.None)
					{
						base.error(h, "form must be absent");
					}
					if (this.block != XmlSchemaDerivationMethod.None)
					{
						base.error(h, "block must be absent");
					}
					if (this.schemaTypeName != null && !this.schemaTypeName.IsEmpty)
					{
						base.error(h, "type must be absent");
					}
					if (this.SchemaType != null)
					{
						base.error(h, "simpleType or complexType must be absent");
					}
					this.qName = this.RefName;
				}
			}
			XmlSchemaDerivationMethod xmlSchemaDerivationMethod2 = this.block;
			if (xmlSchemaDerivationMethod2 != XmlSchemaDerivationMethod.All)
			{
				if (xmlSchemaDerivationMethod2 != XmlSchemaDerivationMethod.None)
				{
					if ((this.block | XmlSchemaUtil.ElementBlockAllowed) != XmlSchemaUtil.ElementBlockAllowed)
					{
						base.error(h, "Some of the values for block are invalid in this context");
					}
					this.blockResolved = this.block;
				}
				else
				{
					this.blockResolved = XmlSchemaDerivationMethod.Empty;
				}
			}
			else
			{
				this.blockResolved = XmlSchemaDerivationMethod.All;
			}
			if (this.Constraints != null)
			{
				XmlSchemaObjectTable table = new XmlSchemaObjectTable();
				foreach (XmlSchemaObject xmlSchemaObject3 in this.Constraints)
				{
					XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)xmlSchemaObject3;
					XmlSchemaUtil.AddToTable(table, xmlSchemaIdentityConstraint, xmlSchemaIdentityConstraint.QualifiedName, h);
				}
			}
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		internal override XmlSchemaParticle GetOptimizedParticle(bool isTop)
		{
			if (this.OptimizedParticle != null)
			{
				return this.OptimizedParticle;
			}
			if (this.RefName != null && this.RefName != XmlQualifiedName.Empty)
			{
				this.referencedElement = this.schema.FindElement(this.RefName);
			}
			if (base.ValidatedMaxOccurs == 0m)
			{
				this.OptimizedParticle = XmlSchemaParticle.Empty;
			}
			else if (this.SubstitutingElements != null && this.SubstitutingElements.Count > 0)
			{
				XmlSchemaChoice xmlSchemaChoice = new XmlSchemaChoice();
				xmlSchemaChoice.MinOccurs = base.MinOccurs;
				xmlSchemaChoice.MaxOccurs = base.MaxOccurs;
				xmlSchemaChoice.Compile(null, this.schema);
				XmlSchemaElement xmlSchemaElement = base.MemberwiseClone() as XmlSchemaElement;
				xmlSchemaElement.MinOccurs = 1m;
				xmlSchemaElement.MaxOccurs = 1m;
				xmlSchemaElement.substitutionGroupElement = null;
				xmlSchemaElement.substitutingElements = null;
				for (int i = 0; i < this.SubstitutingElements.Count; i++)
				{
					XmlSchemaElement el = this.SubstitutingElements[i] as XmlSchemaElement;
					this.AddSubstElementRecursively(xmlSchemaChoice.Items, el);
					this.AddSubstElementRecursively(xmlSchemaChoice.CompiledItems, el);
				}
				if (!xmlSchemaChoice.Items.Contains(xmlSchemaElement))
				{
					xmlSchemaChoice.Items.Add(xmlSchemaElement);
					xmlSchemaChoice.CompiledItems.Add(xmlSchemaElement);
				}
				this.OptimizedParticle = xmlSchemaChoice;
			}
			else
			{
				this.OptimizedParticle = this;
			}
			return this.OptimizedParticle;
		}

		private void AddSubstElementRecursively(XmlSchemaObjectCollection col, XmlSchemaElement el)
		{
			if (el.SubstitutingElements != null)
			{
				for (int i = 0; i < el.SubstitutingElements.Count; i++)
				{
					this.AddSubstElementRecursively(col, el.SubstitutingElements[i] as XmlSchemaElement);
				}
			}
			if (!col.Contains(el))
			{
				col.Add(el);
			}
		}

		internal void FillSubstitutionElementInfo()
		{
			if (this.substitutionGroupElement != null)
			{
				return;
			}
			if (this.SubstitutionGroup != XmlQualifiedName.Empty)
			{
				XmlSchemaElement xmlSchemaElement = this.schema.FindElement(this.SubstitutionGroup);
				this.substitutionGroupElement = xmlSchemaElement;
				if (xmlSchemaElement != null)
				{
					xmlSchemaElement.substitutingElements.Add(this);
				}
			}
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			if (base.IsValidated(schema.CompilationId))
			{
				return this.errorCount;
			}
			this.actualIsNillable = this.IsNillable;
			this.actualIsAbstract = this.IsAbstract;
			if (this.SubstitutionGroup != XmlQualifiedName.Empty)
			{
				XmlSchemaElement xmlSchemaElement = this.substitutionGroupElement;
				if (xmlSchemaElement != null)
				{
					xmlSchemaElement.Validate(h, schema);
				}
			}
			XmlSchemaDatatype xmlSchemaDatatype = null;
			if (this.schemaType != null)
			{
				this.elementType = this.schemaType;
			}
			else if (this.SchemaTypeName != XmlQualifiedName.Empty)
			{
				XmlSchemaType xmlSchemaType = schema.FindSchemaType(this.SchemaTypeName);
				if (xmlSchemaType != null)
				{
					xmlSchemaType.Validate(h, schema);
					this.elementType = xmlSchemaType;
				}
				else if (this.SchemaTypeName == XmlSchemaComplexType.AnyTypeName)
				{
					this.elementType = XmlSchemaComplexType.AnyType;
				}
				else if (XmlSchemaUtil.IsBuiltInDatatypeName(this.SchemaTypeName))
				{
					xmlSchemaDatatype = XmlSchemaDatatype.FromName(this.SchemaTypeName);
					if (xmlSchemaDatatype == null)
					{
						base.error(h, "Invalid schema datatype was specified.");
					}
					else
					{
						this.elementType = xmlSchemaDatatype;
					}
				}
				else if (!schema.IsNamespaceAbsent(this.SchemaTypeName.Namespace))
				{
					base.error(h, "Referenced element schema type " + this.SchemaTypeName + " was not found in the corresponding schema.");
				}
			}
			else if (this.RefName != XmlQualifiedName.Empty)
			{
				XmlSchemaElement xmlSchemaElement2 = schema.FindElement(this.RefName);
				if (xmlSchemaElement2 != null)
				{
					this.referencedElement = xmlSchemaElement2;
					this.errorCount += xmlSchemaElement2.Validate(h, schema);
				}
				else if (!schema.IsNamespaceAbsent(this.RefName.Namespace))
				{
					base.error(h, "Referenced element " + this.RefName + " was not found in the corresponding schema.");
				}
			}
			if (this.referencedElement == null)
			{
				if (this.elementType == null && this.substitutionGroupElement != null)
				{
					this.elementType = this.substitutionGroupElement.ElementType;
				}
				if (this.elementType == null)
				{
					this.elementType = XmlSchemaComplexType.AnyType;
				}
			}
			XmlSchemaType xmlSchemaType2 = this.elementType as XmlSchemaType;
			if (xmlSchemaType2 != null)
			{
				this.errorCount += xmlSchemaType2.Validate(h, schema);
				xmlSchemaDatatype = xmlSchemaType2.Datatype;
			}
			if (this.SubstitutionGroup != XmlQualifiedName.Empty)
			{
				XmlSchemaElement xmlSchemaElement3 = schema.FindElement(this.SubstitutionGroup);
				if (xmlSchemaElement3 != null)
				{
					XmlSchemaType xmlSchemaType3 = xmlSchemaElement3.ElementType as XmlSchemaType;
					if (xmlSchemaType3 != null)
					{
						if ((xmlSchemaElement3.FinalResolved & XmlSchemaDerivationMethod.Substitution) != XmlSchemaDerivationMethod.Empty)
						{
							base.error(h, "Substituted element blocks substitution.");
						}
						if (xmlSchemaType2 != null && (xmlSchemaElement3.FinalResolved & xmlSchemaType2.DerivedBy) != XmlSchemaDerivationMethod.Empty)
						{
							base.error(h, "Invalid derivation was found. Substituted element prohibits this derivation method: " + xmlSchemaType2.DerivedBy + ".");
						}
					}
					XmlSchemaComplexType xmlSchemaComplexType = xmlSchemaType2 as XmlSchemaComplexType;
					if (xmlSchemaComplexType != null)
					{
						xmlSchemaComplexType.ValidateTypeDerivationOK(xmlSchemaElement3.ElementType, h, schema);
					}
					else
					{
						XmlSchemaSimpleType xmlSchemaSimpleType = xmlSchemaType2 as XmlSchemaSimpleType;
						if (xmlSchemaSimpleType != null)
						{
							xmlSchemaSimpleType.ValidateTypeDerivationOK(xmlSchemaElement3.ElementType, h, schema, true);
						}
					}
				}
				else if (!schema.IsNamespaceAbsent(this.SubstitutionGroup.Namespace))
				{
					base.error(h, "Referenced element type " + this.SubstitutionGroup + " was not found in the corresponding schema.");
				}
			}
			if (this.defaultValue != null || this.fixedValue != null)
			{
				this.ValidateElementDefaultValidImmediate(h, schema);
				if (xmlSchemaDatatype != null && xmlSchemaDatatype.TokenizedType == XmlTokenizedType.ID)
				{
					base.error(h, "Element type is ID, which does not allows default or fixed values.");
				}
			}
			foreach (XmlSchemaObject xmlSchemaObject in this.Constraints)
			{
				XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)xmlSchemaObject;
				xmlSchemaIdentityConstraint.Validate(h, schema);
			}
			if (this.elementType != null)
			{
				this.elementSchemaType = (this.elementType as XmlSchemaType);
				if (this.elementType == XmlSchemaSimpleType.AnySimpleType)
				{
					this.elementSchemaType = XmlSchemaSimpleType.XsAnySimpleType;
				}
				if (this.elementSchemaType == null)
				{
					this.elementSchemaType = XmlSchemaType.GetBuiltInSimpleType(this.SchemaTypeName);
				}
			}
			this.ValidationId = schema.ValidationId;
			return this.errorCount;
		}

		internal override bool ParticleEquals(XmlSchemaParticle other)
		{
			XmlSchemaElement xmlSchemaElement = other as XmlSchemaElement;
			if (xmlSchemaElement == null)
			{
				return false;
			}
			if (base.ValidatedMaxOccurs != xmlSchemaElement.ValidatedMaxOccurs || base.ValidatedMinOccurs != xmlSchemaElement.ValidatedMinOccurs)
			{
				return false;
			}
			if (this.QualifiedName != xmlSchemaElement.QualifiedName || this.ElementType != xmlSchemaElement.ElementType || this.Constraints.Count != xmlSchemaElement.Constraints.Count)
			{
				return false;
			}
			for (int i = 0; i < this.Constraints.Count; i++)
			{
				XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = this.Constraints[i] as XmlSchemaIdentityConstraint;
				XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint2 = xmlSchemaElement.Constraints[i] as XmlSchemaIdentityConstraint;
				if (xmlSchemaIdentityConstraint.QualifiedName != xmlSchemaIdentityConstraint2.QualifiedName || xmlSchemaIdentityConstraint.Selector.XPath != xmlSchemaIdentityConstraint2.Selector.XPath || xmlSchemaIdentityConstraint.Fields.Count != xmlSchemaIdentityConstraint2.Fields.Count)
				{
					return false;
				}
				for (int j = 0; j < xmlSchemaIdentityConstraint.Fields.Count; j++)
				{
					XmlSchemaXPath xmlSchemaXPath = xmlSchemaIdentityConstraint.Fields[j] as XmlSchemaXPath;
					XmlSchemaXPath xmlSchemaXPath2 = xmlSchemaIdentityConstraint2.Fields[j] as XmlSchemaXPath;
					if (xmlSchemaXPath.XPath != xmlSchemaXPath2.XPath)
					{
						return false;
					}
				}
			}
			return this.BlockResolved == xmlSchemaElement.BlockResolved && this.FinalResolved == xmlSchemaElement.FinalResolved && !(this.ValidatedDefaultValue != xmlSchemaElement.ValidatedDefaultValue) && !(this.ValidatedFixedValue != xmlSchemaElement.ValidatedFixedValue);
		}

		internal override bool ValidateDerivationByRestriction(XmlSchemaParticle baseParticle, ValidationEventHandler h, XmlSchema schema, bool raiseError)
		{
			XmlSchemaElement xmlSchemaElement = baseParticle as XmlSchemaElement;
			if (xmlSchemaElement != null)
			{
				return this.ValidateDerivationByRestrictionNameAndTypeOK(xmlSchemaElement, h, schema, raiseError);
			}
			XmlSchemaAny xmlSchemaAny = baseParticle as XmlSchemaAny;
			if (xmlSchemaAny != null)
			{
				return xmlSchemaAny.ValidateWildcardAllowsNamespaceName(this.QualifiedName.Namespace, h, schema, raiseError) && this.ValidateOccurenceRangeOK(xmlSchemaAny, h, schema, raiseError);
			}
			XmlSchemaGroupBase xmlSchemaGroupBase = null;
			if (baseParticle is XmlSchemaSequence)
			{
				xmlSchemaGroupBase = new XmlSchemaSequence();
			}
			else if (baseParticle is XmlSchemaChoice)
			{
				xmlSchemaGroupBase = new XmlSchemaChoice();
			}
			else if (baseParticle is XmlSchemaAll)
			{
				xmlSchemaGroupBase = new XmlSchemaAll();
			}
			if (xmlSchemaGroupBase != null)
			{
				xmlSchemaGroupBase.Items.Add(this);
				xmlSchemaGroupBase.Compile(h, schema);
				xmlSchemaGroupBase.Validate(h, schema);
				return xmlSchemaGroupBase.ValidateDerivationByRestriction(baseParticle, h, schema, raiseError);
			}
			return true;
		}

		private bool ValidateDerivationByRestrictionNameAndTypeOK(XmlSchemaElement baseElement, ValidationEventHandler h, XmlSchema schema, bool raiseError)
		{
			if (this.QualifiedName != baseElement.QualifiedName)
			{
				if (raiseError)
				{
					base.error(h, "Invalid derivation by restriction of particle was found. Both elements must have the same name.");
				}
				return false;
			}
			if (this.isNillable && !baseElement.isNillable)
			{
				if (raiseError)
				{
					base.error(h, "Invalid element derivation by restriction of particle was found. Base element is not nillable and derived type is nillable.");
				}
				return false;
			}
			if (!this.ValidateOccurenceRangeOK(baseElement, h, schema, raiseError))
			{
				return false;
			}
			if (baseElement.ValidatedFixedValue != null && baseElement.ValidatedFixedValue != this.ValidatedFixedValue)
			{
				if (raiseError)
				{
					base.error(h, "Invalid element derivation by restriction of particle was found. Both fixed value must be the same.");
				}
				return false;
			}
			if ((baseElement.BlockResolved | this.BlockResolved) != this.BlockResolved)
			{
				if (raiseError)
				{
					base.error(h, "Invalid derivation by restriction of particle was found. Derived element must contain all of the base element's block value.");
				}
				return false;
			}
			if (baseElement.ElementType != null)
			{
				XmlSchemaComplexType xmlSchemaComplexType = this.ElementType as XmlSchemaComplexType;
				if (xmlSchemaComplexType != null)
				{
					xmlSchemaComplexType.ValidateDerivationValidRestriction(baseElement.ElementType as XmlSchemaComplexType, h, schema);
					xmlSchemaComplexType.ValidateTypeDerivationOK(baseElement.ElementType, h, schema);
				}
				else
				{
					XmlSchemaSimpleType xmlSchemaSimpleType = this.ElementType as XmlSchemaSimpleType;
					if (xmlSchemaSimpleType != null)
					{
						xmlSchemaSimpleType.ValidateTypeDerivationOK(baseElement.ElementType, h, schema, true);
					}
					else if (baseElement.ElementType != XmlSchemaComplexType.AnyType && baseElement.ElementType != this.ElementType)
					{
						if (raiseError)
						{
							base.error(h, "Invalid element derivation by restriction of particle was found. Both primitive types differ.");
						}
						return false;
					}
				}
			}
			return true;
		}

		internal override void CheckRecursion(int depth, ValidationEventHandler h, XmlSchema schema)
		{
			XmlSchemaComplexType xmlSchemaComplexType = this.ElementType as XmlSchemaComplexType;
			if (xmlSchemaComplexType == null || xmlSchemaComplexType.Particle == null)
			{
				return;
			}
			xmlSchemaComplexType.Particle.CheckRecursion(depth + 1, h, schema);
		}

		internal override void ValidateUniqueParticleAttribution(XmlSchemaObjectTable qnames, ArrayList nsNames, ValidationEventHandler h, XmlSchema schema)
		{
			if (qnames.Contains(this.QualifiedName))
			{
				base.error(h, "Ambiguous element label was detected: " + this.QualifiedName);
			}
			else
			{
				foreach (object obj in nsNames)
				{
					XmlSchemaAny xmlSchemaAny = (XmlSchemaAny)obj;
					if (!(xmlSchemaAny.ValidatedMaxOccurs == 0m))
					{
						if (xmlSchemaAny.HasValueAny || (xmlSchemaAny.HasValueLocal && this.QualifiedName.Namespace == string.Empty) || (xmlSchemaAny.HasValueOther && this.QualifiedName.Namespace != this.QualifiedName.Namespace) || (xmlSchemaAny.HasValueTargetNamespace && this.QualifiedName.Namespace == this.QualifiedName.Namespace))
						{
							base.error(h, "Ambiguous element label which is contained by -any- particle was detected: " + this.QualifiedName);
							break;
						}
						if (!xmlSchemaAny.HasValueOther)
						{
							bool flag = false;
							foreach (string a in xmlSchemaAny.ResolvedNamespaces)
							{
								if (a == this.QualifiedName.Namespace)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								base.error(h, "Ambiguous element label which is contained by -any- particle was detected: " + this.QualifiedName);
								break;
							}
						}
						else if (xmlSchemaAny.TargetNamespace != this.QualifiedName.Namespace)
						{
							base.error(h, string.Format("Ambiguous element label '{0}' which is contained by -any- particle with ##other value than '{1}' was detected: ", this.QualifiedName.Namespace, xmlSchemaAny.TargetNamespace));
						}
					}
				}
				qnames.Add(this.QualifiedName, this);
			}
		}

		internal override void ValidateUniqueTypeAttribution(XmlSchemaObjectTable labels, ValidationEventHandler h, XmlSchema schema)
		{
			XmlSchemaElement xmlSchemaElement = labels[this.QualifiedName] as XmlSchemaElement;
			if (xmlSchemaElement == null)
			{
				labels.Add(this.QualifiedName, this);
			}
			else if (xmlSchemaElement.ElementType != this.ElementType)
			{
				base.error(h, "Different types are specified on the same named elements in the same sequence. Element name is " + this.QualifiedName);
			}
		}

		private void ValidateElementDefaultValidImmediate(ValidationEventHandler h, XmlSchema schema)
		{
			XmlSchemaDatatype xmlSchemaDatatype = this.elementType as XmlSchemaDatatype;
			XmlSchemaSimpleType xmlSchemaSimpleType = this.elementType as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType != null)
			{
				xmlSchemaDatatype = xmlSchemaSimpleType.Datatype;
			}
			if (xmlSchemaDatatype == null)
			{
				XmlSchemaComplexType xmlSchemaComplexType = this.elementType as XmlSchemaComplexType;
				XmlSchemaContentType contentType = xmlSchemaComplexType.ContentType;
				if (contentType == XmlSchemaContentType.Empty || contentType == XmlSchemaContentType.ElementOnly)
				{
					base.error(h, "Element content type must be simple type or mixed.");
				}
				xmlSchemaDatatype = XmlSchemaSimpleType.AnySimpleType;
			}
			XmlNamespaceManager xmlNamespaceManager = null;
			if (xmlSchemaDatatype.TokenizedType == XmlTokenizedType.QName && base.Namespaces != null)
			{
				foreach (XmlQualifiedName xmlQualifiedName in base.Namespaces.ToArray())
				{
					xmlNamespaceManager.AddNamespace(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
				}
			}
			try
			{
				if (this.defaultValue != null)
				{
					this.validatedDefaultValue = xmlSchemaDatatype.Normalize(this.defaultValue);
					xmlSchemaDatatype.ParseValue(this.validatedDefaultValue, null, xmlNamespaceManager);
				}
			}
			catch (Exception innerException)
			{
				XmlSchemaObject.error(h, "The Element's default value is invalid with respect to its type definition.", innerException);
			}
			try
			{
				if (this.fixedValue != null)
				{
					this.validatedFixedValue = xmlSchemaDatatype.Normalize(this.fixedValue);
					xmlSchemaDatatype.ParseValue(this.validatedFixedValue, null, xmlNamespaceManager);
				}
			}
			catch (Exception innerException2)
			{
				XmlSchemaObject.error(h, "The Element's fixed value is invalid with its type definition.", innerException2);
			}
		}

		internal static XmlSchemaElement Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "element")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaElement.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaElement.LineNumber = reader.LineNumber;
			xmlSchemaElement.LinePosition = reader.LinePosition;
			xmlSchemaElement.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "abstract")
				{
					Exception ex;
					xmlSchemaElement.IsAbstract = XmlSchemaUtil.ReadBoolAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is invalid value for abstract", ex);
					}
				}
				else if (reader.Name == "block")
				{
					Exception ex;
					xmlSchemaElement.block = XmlSchemaUtil.ReadDerivationAttribute(reader, out ex, "block", XmlSchemaUtil.ElementBlockAllowed);
					if (ex != null)
					{
						XmlSchemaObject.error(h, "some invalid values for block attribute were found", ex);
					}
				}
				else if (reader.Name == "default")
				{
					xmlSchemaElement.defaultValue = reader.Value;
				}
				else if (reader.Name == "final")
				{
					Exception ex;
					xmlSchemaElement.Final = XmlSchemaUtil.ReadDerivationAttribute(reader, out ex, "final", XmlSchemaUtil.FinalAllowed);
					if (ex != null)
					{
						XmlSchemaObject.error(h, "some invalid values for final attribute were found", ex);
					}
				}
				else if (reader.Name == "fixed")
				{
					xmlSchemaElement.fixedValue = reader.Value;
				}
				else if (reader.Name == "form")
				{
					Exception ex;
					xmlSchemaElement.form = XmlSchemaUtil.ReadFormAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is an invalid value for form attribute", ex);
					}
				}
				else if (reader.Name == "id")
				{
					xmlSchemaElement.Id = reader.Value;
				}
				else if (reader.Name == "maxOccurs")
				{
					try
					{
						xmlSchemaElement.MaxOccursString = reader.Value;
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
						xmlSchemaElement.MinOccursString = reader.Value;
					}
					catch (Exception innerException2)
					{
						XmlSchemaObject.error(h, reader.Value + " is an invalid value for minOccurs", innerException2);
					}
				}
				else if (reader.Name == "name")
				{
					xmlSchemaElement.Name = reader.Value;
				}
				else if (reader.Name == "nillable")
				{
					Exception ex;
					xmlSchemaElement.IsNillable = XmlSchemaUtil.ReadBoolAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + "is not a valid value for nillable", ex);
					}
				}
				else if (reader.Name == "ref")
				{
					Exception ex;
					xmlSchemaElement.refName = XmlSchemaUtil.ReadQNameAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is not a valid value for ref attribute", ex);
					}
				}
				else if (reader.Name == "substitutionGroup")
				{
					Exception ex;
					xmlSchemaElement.substitutionGroup = XmlSchemaUtil.ReadQNameAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is not a valid value for substitutionGroup attribute", ex);
					}
				}
				else if (reader.Name == "type")
				{
					Exception ex;
					xmlSchemaElement.SchemaTypeName = XmlSchemaUtil.ReadQNameAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is not a valid value for type attribute", ex);
					}
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for element", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaElement);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaElement;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "element")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaElement.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaElement.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					if (num <= 2)
					{
						if (reader.LocalName == "simpleType")
						{
							num = 3;
							XmlSchemaSimpleType xmlSchemaSimpleType = XmlSchemaSimpleType.Read(reader, h);
							if (xmlSchemaSimpleType != null)
							{
								xmlSchemaElement.SchemaType = xmlSchemaSimpleType;
							}
							continue;
						}
						if (reader.LocalName == "complexType")
						{
							num = 3;
							XmlSchemaComplexType xmlSchemaComplexType = XmlSchemaComplexType.Read(reader, h);
							if (xmlSchemaComplexType != null)
							{
								xmlSchemaElement.SchemaType = xmlSchemaComplexType;
							}
							continue;
						}
					}
					if (num <= 3)
					{
						if (reader.LocalName == "unique")
						{
							num = 3;
							XmlSchemaUnique xmlSchemaUnique = XmlSchemaUnique.Read(reader, h);
							if (xmlSchemaUnique != null)
							{
								xmlSchemaElement.constraints.Add(xmlSchemaUnique);
							}
							continue;
						}
						if (reader.LocalName == "key")
						{
							num = 3;
							XmlSchemaKey xmlSchemaKey = XmlSchemaKey.Read(reader, h);
							if (xmlSchemaKey != null)
							{
								xmlSchemaElement.constraints.Add(xmlSchemaKey);
							}
							continue;
						}
						if (reader.LocalName == "keyref")
						{
							num = 3;
							XmlSchemaKeyref xmlSchemaKeyref = XmlSchemaKeyref.Read(reader, h);
							if (xmlSchemaKeyref != null)
							{
								xmlSchemaElement.constraints.Add(xmlSchemaKeyref);
							}
							continue;
						}
					}
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaElement;
		}
	}
}
