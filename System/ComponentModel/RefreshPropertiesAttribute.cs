using System;

namespace System.ComponentModel
{
	/// <summary>Indicates that the property grid should refresh when the associated property value changes. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class RefreshPropertiesAttribute : Attribute
	{
		private RefreshProperties refresh;

		/// <summary>Indicates that all properties are queried again and refreshed if the property value is changed. This field is read-only.</summary>
		public static readonly RefreshPropertiesAttribute All = new RefreshPropertiesAttribute(RefreshProperties.All);

		/// <summary>Indicates that no other properties are refreshed if the property value is changed. This field is read-only.</summary>
		public static readonly RefreshPropertiesAttribute Default = new RefreshPropertiesAttribute(RefreshProperties.None);

		/// <summary>Indicates that all properties are repainted if the property value is changed. This field is read-only.</summary>
		public static readonly RefreshPropertiesAttribute Repaint = new RefreshPropertiesAttribute(RefreshProperties.Repaint);

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.RefreshPropertiesAttribute" /> class.</summary>
		/// <param name="refresh">A <see cref="T:System.ComponentModel.RefreshProperties" /> value indicating the nature of the refresh.</param>
		public RefreshPropertiesAttribute(RefreshProperties refresh)
		{
			this.refresh = refresh;
		}

		/// <summary>Gets the refresh properties for the member.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.RefreshProperties" /> that indicates the current refresh properties for the member.</returns>
		public RefreshProperties RefreshProperties
		{
			get
			{
				return this.refresh;
			}
		}

		/// <summary>Overrides the object's <see cref="Overload:System.Object.Equals" /> method.</summary>
		/// <returns>true if the specified object is the same; otherwise, false.</returns>
		/// <param name="value">The object to test for equality. </param>
		public override bool Equals(object obj)
		{
			return obj is RefreshPropertiesAttribute && (obj == this || ((RefreshPropertiesAttribute)obj).RefreshProperties == this.refresh);
		}

		/// <summary>Returns the hash code for this object.</summary>
		/// <returns>The hash code for the object that the attribute belongs to.</returns>
		public override int GetHashCode()
		{
			return this.refresh.GetHashCode();
		}

		/// <summary>Gets a value indicating whether the current value of the attribute is the default value for the attribute.</summary>
		/// <returns>true if the current value of the attribute is the default; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return this == RefreshPropertiesAttribute.Default;
		}
	}
}
