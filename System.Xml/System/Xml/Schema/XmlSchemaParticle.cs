using System;
using System.Collections;
using System.Globalization;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Abstract class for that is the base class for all particle types (e.g. <see cref="T:System.Xml.Schema.XmlSchemaAny" />).</summary>
	public abstract class XmlSchemaParticle : XmlSchemaAnnotated
	{
		private decimal minOccurs;

		private decimal maxOccurs;

		private string minstr;

		private string maxstr;

		private static XmlSchemaParticle empty;

		private decimal validatedMinOccurs = 1m;

		private decimal validatedMaxOccurs = 1m;

		internal int recursionDepth = -1;

		private decimal minEffectiveTotalRange = -1m;

		internal bool parentIsGroupDefinition;

		internal XmlSchemaParticle OptimizedParticle;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaParticle" /> class.</summary>
		protected XmlSchemaParticle()
		{
			this.minOccurs = 1m;
			this.maxOccurs = 1m;
		}

		internal static XmlSchemaParticle Empty
		{
			get
			{
				if (XmlSchemaParticle.empty == null)
				{
					XmlSchemaParticle.empty = new XmlSchemaParticle.EmptyParticle();
				}
				return XmlSchemaParticle.empty;
			}
		}

		/// <summary>Gets or sets the number as a string value. The minimum number of times the particle can occur.</summary>
		/// <returns>The number as a string value. String.Empty indicates that MinOccurs is equal to the default value. The default is a null reference.</returns>
		[XmlAttribute("minOccurs")]
		public string MinOccursString
		{
			get
			{
				return this.minstr;
			}
			set
			{
				if (value == null)
				{
					this.minOccurs = 1m;
					this.minstr = value;
					return;
				}
				decimal num = decimal.Parse(value, CultureInfo.InvariantCulture);
				if (num >= 0m && num == decimal.Truncate(num))
				{
					this.minOccurs = num;
					this.minstr = num.ToString(CultureInfo.InvariantCulture);
					return;
				}
				throw new XmlSchemaException("MinOccursString must be a non-negative number", null);
			}
		}

		/// <summary>Gets or sets the number as a string value. Maximum number of times the particle can occur.</summary>
		/// <returns>The number as a string value. String.Empty indicates that MaxOccurs is equal to the default value. The default is a null reference.</returns>
		[XmlAttribute("maxOccurs")]
		public string MaxOccursString
		{
			get
			{
				return this.maxstr;
			}
			set
			{
				if (value == "unbounded")
				{
					this.maxstr = value;
					this.maxOccurs = decimal.MaxValue;
				}
				else
				{
					decimal num = decimal.Parse(value, CultureInfo.InvariantCulture);
					if (!(num >= 0m) || !(num == decimal.Truncate(num)))
					{
						throw new XmlSchemaException("MaxOccurs must be a non-negative integer", null);
					}
					this.maxOccurs = num;
					this.maxstr = num.ToString(CultureInfo.InvariantCulture);
					if (num == 0m && this.minstr == null)
					{
						this.minOccurs = 0m;
					}
				}
			}
		}

		/// <summary>Gets or sets the minimum number of times the particle can occur.</summary>
		/// <returns>The minimum number of times the particle can occur. The default is 1.</returns>
		[XmlIgnore]
		public decimal MinOccurs
		{
			get
			{
				return this.minOccurs;
			}
			set
			{
				this.MinOccursString = value.ToString(CultureInfo.InvariantCulture);
			}
		}

		/// <summary>Gets or sets the maximum number of times the particle can occur.</summary>
		/// <returns>The maximum number of times the particle can occur. The default is 1.</returns>
		[XmlIgnore]
		public decimal MaxOccurs
		{
			get
			{
				return this.maxOccurs;
			}
			set
			{
				if (value == 79228162514264337593543950335m)
				{
					this.MaxOccursString = "unbounded";
				}
				else
				{
					this.MaxOccursString = value.ToString(CultureInfo.InvariantCulture);
				}
			}
		}

		internal decimal ValidatedMinOccurs
		{
			get
			{
				return this.validatedMinOccurs;
			}
		}

		internal decimal ValidatedMaxOccurs
		{
			get
			{
				return this.validatedMaxOccurs;
			}
		}

		internal virtual XmlSchemaParticle GetOptimizedParticle(bool isTop)
		{
			return null;
		}

		internal XmlSchemaParticle GetShallowClone()
		{
			return (XmlSchemaParticle)base.MemberwiseClone();
		}

		internal void CompileOccurence(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.MinOccurs > this.MaxOccurs && (!(this.MaxOccurs == 0m) || this.MinOccursString != null))
			{
				base.error(h, "minOccurs must be less than or equal to maxOccurs");
			}
			else
			{
				if (this.MaxOccursString == "unbounded")
				{
					this.validatedMaxOccurs = decimal.MaxValue;
				}
				else
				{
					this.validatedMaxOccurs = this.maxOccurs;
				}
				if (this.validatedMaxOccurs == 0m)
				{
					this.validatedMinOccurs = 0m;
				}
				else
				{
					this.validatedMinOccurs = this.minOccurs;
				}
			}
		}

		internal override void CopyInfo(XmlSchemaParticle obj)
		{
			base.CopyInfo(obj);
			if (this.MaxOccursString == "unbounded")
			{
				obj.maxOccurs = (obj.validatedMaxOccurs = decimal.MaxValue);
			}
			else
			{
				obj.maxOccurs = (obj.validatedMaxOccurs = this.ValidatedMaxOccurs);
			}
			if (this.MaxOccurs == 0m)
			{
				obj.minOccurs = (obj.validatedMinOccurs = 0m);
			}
			else
			{
				obj.minOccurs = (obj.validatedMinOccurs = this.ValidatedMinOccurs);
			}
			if (this.MinOccursString != null)
			{
				obj.MinOccursString = this.MinOccursString;
			}
			if (this.MaxOccursString != null)
			{
				obj.MaxOccursString = this.MaxOccursString;
			}
		}

		internal virtual bool ValidateOccurenceRangeOK(XmlSchemaParticle other, ValidationEventHandler h, XmlSchema schema, bool raiseError)
		{
			if (this.ValidatedMinOccurs < other.ValidatedMinOccurs || (other.ValidatedMaxOccurs != 79228162514264337593543950335m && this.ValidatedMaxOccurs > other.ValidatedMaxOccurs))
			{
				if (raiseError)
				{
					base.error(h, "Invalid derivation occurence range was found.");
				}
				return false;
			}
			return true;
		}

		internal virtual decimal GetMinEffectiveTotalRange()
		{
			return this.ValidatedMinOccurs;
		}

		internal decimal GetMinEffectiveTotalRangeAllAndSequence()
		{
			if (this.minEffectiveTotalRange >= 0m)
			{
				return this.minEffectiveTotalRange;
			}
			decimal num = 0m;
			XmlSchemaObjectCollection items;
			if (this is XmlSchemaAll)
			{
				items = ((XmlSchemaAll)this).Items;
			}
			else
			{
				items = ((XmlSchemaSequence)this).Items;
			}
			foreach (XmlSchemaObject xmlSchemaObject in items)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				num += xmlSchemaParticle.GetMinEffectiveTotalRange();
			}
			this.minEffectiveTotalRange = num;
			return num;
		}

		internal virtual bool ValidateIsEmptiable()
		{
			return this.validatedMinOccurs == 0m || this.GetMinEffectiveTotalRange() == 0m;
		}

		internal virtual bool ValidateDerivationByRestriction(XmlSchemaParticle baseParticle, ValidationEventHandler h, XmlSchema schema, bool raiseError)
		{
			return false;
		}

		internal virtual void ValidateUniqueParticleAttribution(XmlSchemaObjectTable qnames, ArrayList nsNames, ValidationEventHandler h, XmlSchema schema)
		{
		}

		internal virtual void ValidateUniqueTypeAttribution(XmlSchemaObjectTable labels, ValidationEventHandler h, XmlSchema schema)
		{
		}

		internal virtual void CheckRecursion(int depth, ValidationEventHandler h, XmlSchema schema)
		{
		}

		internal virtual bool ParticleEquals(XmlSchemaParticle other)
		{
			return false;
		}

		internal class EmptyParticle : XmlSchemaParticle
		{
			internal EmptyParticle()
			{
			}

			internal override XmlSchemaParticle GetOptimizedParticle(bool isTop)
			{
				return this;
			}

			internal override bool ParticleEquals(XmlSchemaParticle other)
			{
				return other == this || other == XmlSchemaParticle.Empty;
			}

			internal override bool ValidateDerivationByRestriction(XmlSchemaParticle baseParticle, ValidationEventHandler h, XmlSchema schema, bool raiseError)
			{
				return true;
			}

			internal override void CheckRecursion(int depth, ValidationEventHandler h, XmlSchema schema)
			{
			}

			internal override void ValidateUniqueParticleAttribution(XmlSchemaObjectTable qnames, ArrayList nsNames, ValidationEventHandler h, XmlSchema schema)
			{
			}

			internal override void ValidateUniqueTypeAttribution(XmlSchemaObjectTable labels, ValidationEventHandler h, XmlSchema schema)
			{
			}
		}
	}
}
