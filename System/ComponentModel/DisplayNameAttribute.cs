using System;

namespace System.ComponentModel
{
	/// <summary>Specifies the display name for a property, event, or public void method which takes no arguments. </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
	public class DisplayNameAttribute : Attribute
	{
		/// <summary>Specifies the default value for the <see cref="T:System.ComponentModel.DisplayNameAttribute" />. This field is read-only.</summary>
		public static readonly DisplayNameAttribute Default = new DisplayNameAttribute();

		private string attributeDisplayName;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DisplayNameAttribute" /> class.</summary>
		public DisplayNameAttribute()
		{
			this.attributeDisplayName = string.Empty;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DisplayNameAttribute" /> class using the display name.</summary>
		/// <param name="displayName">The display name.</param>
		public DisplayNameAttribute(string displayName)
		{
			this.attributeDisplayName = displayName;
		}

		/// <summary>Determines if this attribute is the default.</summary>
		/// <returns>true if the attribute is the default value for this attribute class; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return this.attributeDisplayName != null && this.attributeDisplayName.Length == 0;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A hash code for the current <see cref="T:System.ComponentModel.DisplayNameAttribute" />.</returns>
		public override int GetHashCode()
		{
			return this.attributeDisplayName.GetHashCode();
		}

		/// <summary>Determines whether two <see cref="T:System.ComponentModel.DisplayNameAttribute" /> instances are equal.</summary>
		/// <returns>true if the value of the given object is equal to that of the current object; otherwise, false.</returns>
		/// <param name="obj">The <see cref="T:System.ComponentModel.DisplayNameAttribute" /> to test the value equality of.</param>
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DisplayNameAttribute displayNameAttribute = obj as DisplayNameAttribute;
			return displayNameAttribute != null && displayNameAttribute.DisplayName == this.attributeDisplayName;
		}

		/// <summary>Gets the display name for a property, event, or public void method that takes no arguments stored in this attribute.</summary>
		/// <returns>The display name.</returns>
		public virtual string DisplayName
		{
			get
			{
				return this.attributeDisplayName;
			}
		}

		/// <summary>Gets or sets the display name.</summary>
		/// <returns>The display name.</returns>
		protected string DisplayNameValue
		{
			get
			{
				return this.attributeDisplayName;
			}
			set
			{
				this.attributeDisplayName = value;
			}
		}
	}
}
