using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdValidationContext
	{
		private object xsi_type;

		internal XsdValidationState State;

		private Stack element_stack = new Stack();

		public object XsiType
		{
			get
			{
				return this.xsi_type;
			}
			set
			{
				this.xsi_type = value;
			}
		}

		public XmlSchemaElement Element
		{
			get
			{
				return (this.element_stack.Count <= 0) ? null : (this.element_stack.Peek() as XmlSchemaElement);
			}
		}

		public void PushCurrentElement(XmlSchemaElement element)
		{
			this.element_stack.Push(element);
		}

		public void PopCurrentElement()
		{
			this.element_stack.Pop();
		}

		public object ActualType
		{
			get
			{
				if (this.element_stack.Count == 0)
				{
					return null;
				}
				if (this.XsiType != null)
				{
					return this.XsiType;
				}
				return (this.Element == null) ? null : this.Element.ElementType;
			}
		}

		public XmlSchemaType ActualSchemaType
		{
			get
			{
				object actualType = this.ActualType;
				if (actualType == null)
				{
					return null;
				}
				XmlSchemaType xmlSchemaType = actualType as XmlSchemaType;
				if (xmlSchemaType == null)
				{
					xmlSchemaType = XmlSchemaType.GetBuiltInSimpleType(((XmlSchemaDatatype)actualType).TypeCode);
				}
				return xmlSchemaType;
			}
		}

		public bool IsInvalid
		{
			get
			{
				return this.State == XsdValidationState.Invalid;
			}
		}

		public object Clone()
		{
			return base.MemberwiseClone();
		}

		public void EvaluateStartElement(string localName, string ns)
		{
			this.State = this.State.EvaluateStartElement(localName, ns);
		}

		public bool EvaluateEndElement()
		{
			return this.State.EvaluateEndElement();
		}
	}
}
