using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdKeyTable
	{
		public readonly bool alwaysTrue = true;

		private XsdIdentitySelector selector;

		private XmlSchemaIdentityConstraint source;

		private XmlQualifiedName qname;

		private XmlQualifiedName refKeyName;

		public XsdKeyEntryCollection Entries = new XsdKeyEntryCollection();

		public XsdKeyEntryCollection FinishedEntries = new XsdKeyEntryCollection();

		public int StartDepth;

		public XsdKeyTable ReferencedKey;

		public XsdKeyTable(XmlSchemaIdentityConstraint source)
		{
			this.Reset(source);
		}

		public XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qname;
			}
		}

		public XmlQualifiedName RefKeyName
		{
			get
			{
				return this.refKeyName;
			}
		}

		public XmlSchemaIdentityConstraint SourceSchemaIdentity
		{
			get
			{
				return this.source;
			}
		}

		public XsdIdentitySelector Selector
		{
			get
			{
				return this.selector;
			}
		}

		public void Reset(XmlSchemaIdentityConstraint source)
		{
			this.source = source;
			this.selector = source.CompiledSelector;
			this.qname = source.QualifiedName;
			XmlSchemaKeyref xmlSchemaKeyref = source as XmlSchemaKeyref;
			if (xmlSchemaKeyref != null)
			{
				this.refKeyName = xmlSchemaKeyref.Refer;
			}
			this.StartDepth = 0;
		}

		public XsdIdentityPath SelectorMatches(ArrayList qnameStack, int depth)
		{
			for (int i = 0; i < this.Selector.Paths.Length; i++)
			{
				XsdIdentityPath xsdIdentityPath = this.Selector.Paths[i];
				if (depth == this.StartDepth)
				{
					if (xsdIdentityPath.OrderedSteps.Length == 0)
					{
						return xsdIdentityPath;
					}
				}
				else if (depth - this.StartDepth >= xsdIdentityPath.OrderedSteps.Length - 1)
				{
					int num = xsdIdentityPath.OrderedSteps.Length;
					if (xsdIdentityPath.OrderedSteps[num - 1].IsAttribute)
					{
						num--;
					}
					if (!xsdIdentityPath.Descendants || depth >= this.StartDepth + num)
					{
						if (xsdIdentityPath.Descendants || depth == this.StartDepth + num)
						{
							num--;
							int num2 = 0;
							while (0 <= num)
							{
								XsdIdentityStep xsdIdentityStep = xsdIdentityPath.OrderedSteps[num];
								if (!xsdIdentityStep.IsAnyName)
								{
									XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)qnameStack[qnameStack.Count - num2 - 1];
									if (xsdIdentityStep.NsName == null || !(xmlQualifiedName.Namespace == xsdIdentityStep.NsName))
									{
										if (!(xsdIdentityStep.Name == xmlQualifiedName.Name) || !(xsdIdentityStep.Namespace == xmlQualifiedName.Namespace))
										{
											if (this.alwaysTrue)
											{
												break;
											}
										}
									}
								}
								num2++;
								num--;
							}
							if (num < 0)
							{
								return xsdIdentityPath;
							}
						}
					}
				}
			}
			return null;
		}
	}
}
