using System;

namespace System.ComponentModel
{
	/// <summary>Specifies whether a property can only be set at design time.</summary>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class DesignOnlyAttribute : Attribute
	{
		private bool design_only;

		/// <summary>Specifies the default value for the <see cref="T:System.ComponentModel.DesignOnlyAttribute" />, which is <see cref="F:System.ComponentModel.DesignOnlyAttribute.No" />. This static field is read-only.</summary>
		public static readonly DesignOnlyAttribute Default = new DesignOnlyAttribute(false);

		/// <summary>Specifies that a property can be set at design time or at run time. This static field is read-only.</summary>
		public static readonly DesignOnlyAttribute No = new DesignOnlyAttribute(false);

		/// <summary>Specifies that a property can be set only at design time. This static field is read-only.</summary>
		public static readonly DesignOnlyAttribute Yes = new DesignOnlyAttribute(true);

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DesignOnlyAttribute" /> class.</summary>
		/// <param name="isDesignOnly">true if a property can be set only at design time; false if the property can be set at design time and at run time. </param>
		public DesignOnlyAttribute(bool design_only)
		{
			this.design_only = design_only;
		}

		/// <summary>Gets a value indicating whether a property can be set only at design time.</summary>
		/// <returns>true if a property can be set only at design time; otherwise, false.</returns>
		public bool IsDesignOnly
		{
			get
			{
				return this.design_only;
			}
		}

		/// <summary>Returns whether the value of the given object is equal to the current <see cref="T:System.ComponentModel.DesignOnlyAttribute" />.</summary>
		/// <returns>true if the value of the given object is equal to that of the current; otherwise, false.</returns>
		/// <param name="obj">The object to test the value equality of. </param>
		public override bool Equals(object obj)
		{
			return obj is DesignOnlyAttribute && (obj == this || ((DesignOnlyAttribute)obj).IsDesignOnly == this.design_only);
		}

		public override int GetHashCode()
		{
			return this.design_only.GetHashCode();
		}

		/// <summary>Determines if this attribute is the default.</summary>
		/// <returns>true if the attribute is the default value for this attribute class; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return this.design_only == DesignOnlyAttribute.Default.IsDesignOnly;
		}
	}
}
