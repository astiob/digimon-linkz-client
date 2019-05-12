using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdParticleStateManager
	{
		private Hashtable table;

		private XmlSchemaContentProcessing processContents;

		public XmlSchemaElement CurrentElement;

		public Stack ContextStack = new Stack();

		public XsdValidationContext Context = new XsdValidationContext();

		public XsdParticleStateManager()
		{
			this.table = new Hashtable();
			this.processContents = XmlSchemaContentProcessing.Strict;
		}

		public XmlSchemaContentProcessing ProcessContents
		{
			get
			{
				return this.processContents;
			}
		}

		public void PushContext()
		{
			this.ContextStack.Push(this.Context.Clone());
		}

		public void PopContext()
		{
			this.Context = (XsdValidationContext)this.ContextStack.Pop();
		}

		internal void SetProcessContents(XmlSchemaContentProcessing value)
		{
			this.processContents = value;
		}

		public XsdValidationState Get(XmlSchemaParticle xsobj)
		{
			XsdValidationState xsdValidationState = this.table[xsobj] as XsdValidationState;
			if (xsdValidationState == null)
			{
				xsdValidationState = this.Create(xsobj);
			}
			return xsdValidationState;
		}

		public XsdValidationState Create(XmlSchemaObject xsobj)
		{
			string name = xsobj.GetType().Name;
			string text = name;
			switch (text)
			{
			case "XmlSchemaElement":
				return this.AddElement((XmlSchemaElement)xsobj);
			case "XmlSchemaSequence":
				return this.AddSequence((XmlSchemaSequence)xsobj);
			case "XmlSchemaChoice":
				return this.AddChoice((XmlSchemaChoice)xsobj);
			case "XmlSchemaAll":
				return this.AddAll((XmlSchemaAll)xsobj);
			case "XmlSchemaAny":
				return this.AddAny((XmlSchemaAny)xsobj);
			case "EmptyParticle":
				return this.AddEmpty();
			}
			throw new InvalidOperationException("Should not occur.");
		}

		internal XsdValidationState MakeSequence(XsdValidationState head, XsdValidationState rest)
		{
			if (head is XsdEmptyValidationState)
			{
				return rest;
			}
			return new XsdAppendedValidationState(this, head, rest);
		}

		private XsdElementValidationState AddElement(XmlSchemaElement element)
		{
			return new XsdElementValidationState(element, this);
		}

		private XsdSequenceValidationState AddSequence(XmlSchemaSequence sequence)
		{
			return new XsdSequenceValidationState(sequence, this);
		}

		private XsdChoiceValidationState AddChoice(XmlSchemaChoice choice)
		{
			return new XsdChoiceValidationState(choice, this);
		}

		private XsdAllValidationState AddAll(XmlSchemaAll all)
		{
			return new XsdAllValidationState(all, this);
		}

		private XsdAnyValidationState AddAny(XmlSchemaAny any)
		{
			return new XsdAnyValidationState(any, this);
		}

		private XsdEmptyValidationState AddEmpty()
		{
			return new XsdEmptyValidationState(this);
		}
	}
}
