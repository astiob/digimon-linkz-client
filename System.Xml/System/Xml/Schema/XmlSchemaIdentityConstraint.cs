using Mono.Xml.Schema;
using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Class for the identity constraints: key, keyref, and unique elements.</summary>
	public class XmlSchemaIdentityConstraint : XmlSchemaAnnotated
	{
		private XmlSchemaObjectCollection fields;

		private string name;

		private XmlQualifiedName qName;

		private XmlSchemaXPath selector;

		private XsdIdentitySelector compiledSelector;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaIdentityConstraint" /> class.</summary>
		public XmlSchemaIdentityConstraint()
		{
			this.fields = new XmlSchemaObjectCollection();
			this.qName = XmlQualifiedName.Empty;
		}

		/// <summary>Gets or sets the name of the identity constraint.</summary>
		/// <returns>The name of the identity constraint.</returns>
		[XmlAttribute("name")]
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

		/// <summary>Gets or sets the XPath expression selector element.</summary>
		/// <returns>The XPath expression selector element.</returns>
		[XmlElement("selector", typeof(XmlSchemaXPath))]
		public XmlSchemaXPath Selector
		{
			get
			{
				return this.selector;
			}
			set
			{
				this.selector = value;
			}
		}

		/// <summary>Gets the collection of fields that apply as children for the XML Path Language (XPath) expression selector.</summary>
		/// <returns>The collection of fields.</returns>
		[XmlElement("field", typeof(XmlSchemaXPath))]
		public XmlSchemaObjectCollection Fields
		{
			get
			{
				return this.fields;
			}
		}

		/// <summary>Gets the qualified name of the identity constraint, which holds the post-compilation value of the QualifiedName property.</summary>
		/// <returns>The post-compilation value of the QualifiedName property.</returns>
		[XmlIgnore]
		public XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qName;
			}
		}

		internal XsdIdentitySelector CompiledSelector
		{
			get
			{
				return this.compiledSelector;
			}
		}

		internal override void SetParent(XmlSchemaObject parent)
		{
			base.SetParent(parent);
			if (this.Selector != null)
			{
				this.Selector.SetParent(this);
			}
			foreach (XmlSchemaObject xmlSchemaObject in this.Fields)
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
			if (this.Name == null)
			{
				base.error(h, "Required attribute name must be present");
			}
			else if (!XmlSchemaUtil.CheckNCName(this.name))
			{
				base.error(h, "attribute name must be NCName");
			}
			else
			{
				this.qName = new XmlQualifiedName(this.Name, base.AncestorSchema.TargetNamespace);
				if (schema.NamedIdentities.Contains(this.qName))
				{
					XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = schema.NamedIdentities[this.qName] as XmlSchemaIdentityConstraint;
					base.error(h, string.Format("There is already same named identity constraint in this namespace. Existing item is at {0}({1},{2})", xmlSchemaIdentityConstraint.SourceUri, xmlSchemaIdentityConstraint.LineNumber, xmlSchemaIdentityConstraint.LinePosition));
				}
				else
				{
					schema.NamedIdentities.Add(this.qName, this);
				}
			}
			if (this.Selector == null)
			{
				base.error(h, "selector must be present");
			}
			else
			{
				this.Selector.isSelector = true;
				this.errorCount += this.Selector.Compile(h, schema);
				if (this.selector.errorCount == 0)
				{
					this.compiledSelector = new XsdIdentitySelector(this.Selector);
				}
			}
			if (this.errorCount > 0)
			{
				return this.errorCount;
			}
			if (this.Fields.Count == 0)
			{
				base.error(h, "atleast one field value must be present");
			}
			else
			{
				for (int i = 0; i < this.Fields.Count; i++)
				{
					XmlSchemaXPath xmlSchemaXPath = this.Fields[i] as XmlSchemaXPath;
					if (xmlSchemaXPath != null)
					{
						this.errorCount += xmlSchemaXPath.Compile(h, schema);
						if (xmlSchemaXPath.errorCount == 0)
						{
							this.compiledSelector.AddField(new XsdIdentityField(xmlSchemaXPath, i));
						}
					}
					else
					{
						base.error(h, "Object of type " + this.Fields[i].GetType() + " is invalid in the Fields Collection");
					}
				}
			}
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}
	}
}
