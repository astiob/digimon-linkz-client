using System;

namespace System.ComponentModel
{
	/// <summary>Specifies that the property can be used as an application setting.</summary>
	[Obsolete("Use SettingsBindableAttribute instead of RecommendedAsConfigurableAttribute")]
	[AttributeUsage(AttributeTargets.Property)]
	public class RecommendedAsConfigurableAttribute : Attribute
	{
		private bool recommendedAsConfigurable;

		/// <summary>Specifies the default value for the <see cref="T:System.ComponentModel.RecommendedAsConfigurableAttribute" />, which is <see cref="F:System.ComponentModel.RecommendedAsConfigurableAttribute.No" />. This static field is read-only.</summary>
		public static readonly RecommendedAsConfigurableAttribute Default = new RecommendedAsConfigurableAttribute(false);

		/// <summary>Specifies that a property cannot be used as an application setting. This static field is read-only.</summary>
		public static readonly RecommendedAsConfigurableAttribute No = new RecommendedAsConfigurableAttribute(false);

		/// <summary>Specifies that a property can be used as an application setting. This static field is read-only.</summary>
		public static readonly RecommendedAsConfigurableAttribute Yes = new RecommendedAsConfigurableAttribute(true);

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.RecommendedAsConfigurableAttribute" /> class.</summary>
		/// <param name="recommendedAsConfigurable">true if the property this attribute is bound to can be used as an application setting; otherwise, false. </param>
		public RecommendedAsConfigurableAttribute(bool recommendedAsConfigurable)
		{
			this.recommendedAsConfigurable = recommendedAsConfigurable;
		}

		/// <summary>Gets a value indicating whether the property this attribute is bound to can be used as an application setting.</summary>
		/// <returns>true if the property this attribute is bound to can be used as an application setting; otherwise, false.</returns>
		public bool RecommendedAsConfigurable
		{
			get
			{
				return this.recommendedAsConfigurable;
			}
		}

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <returns>true if <paramref name="obj" /> is equal to this instance; otherwise, false.</returns>
		/// <param name="obj">Another object to compare to. </param>
		public override bool Equals(object obj)
		{
			return obj is RecommendedAsConfigurableAttribute && ((RecommendedAsConfigurableAttribute)obj).RecommendedAsConfigurable == this.recommendedAsConfigurable;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A hash code for the current <see cref="T:System.ComponentModel.RecommendedAsConfigurableAttribute" />.</returns>
		public override int GetHashCode()
		{
			return this.recommendedAsConfigurable.GetHashCode();
		}

		/// <summary>Indicates whether the value of this instance is the default value for the class.</summary>
		/// <returns>true if this instance is the default attribute for the class; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return this.recommendedAsConfigurable == RecommendedAsConfigurableAttribute.Default.RecommendedAsConfigurable;
		}
	}
}
