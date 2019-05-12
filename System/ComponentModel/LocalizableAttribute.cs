using System;

namespace System.ComponentModel
{
	/// <summary>Specifies whether a property should be localized. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class LocalizableAttribute : Attribute
	{
		private bool localizable;

		/// <summary>Specifies the default value, which is <see cref="F:System.ComponentModel.LocalizableAttribute.No" />. This static field is read-only.</summary>
		public static readonly LocalizableAttribute Default = new LocalizableAttribute(false);

		/// <summary>Specifies that a property should not be localized. This static field is read-only.</summary>
		public static readonly LocalizableAttribute No = new LocalizableAttribute(false);

		/// <summary>Specifies that a property should be localized. This static field is read-only.</summary>
		public static readonly LocalizableAttribute Yes = new LocalizableAttribute(true);

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.LocalizableAttribute" /> class.</summary>
		/// <param name="isLocalizable">true if a property should be localized; otherwise, false. </param>
		public LocalizableAttribute(bool localizable)
		{
			this.localizable = localizable;
		}

		/// <summary>Gets a value indicating whether a property should be localized.</summary>
		/// <returns>true if a property should be localized; otherwise, false.</returns>
		public bool IsLocalizable
		{
			get
			{
				return this.localizable;
			}
		}

		/// <summary>Returns whether the value of the given object is equal to the current <see cref="T:System.ComponentModel.LocalizableAttribute" />.</summary>
		/// <returns>true if the value of the given object is equal to that of the current; otherwise, false.</returns>
		/// <param name="obj">The object to test the value equality of. </param>
		public override bool Equals(object obj)
		{
			return obj is LocalizableAttribute && (obj == this || ((LocalizableAttribute)obj).IsLocalizable == this.localizable);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A hash code for the current <see cref="T:System.ComponentModel.LocalizableAttribute" />.</returns>
		public override int GetHashCode()
		{
			return this.localizable.GetHashCode();
		}

		/// <summary>Determines if this attribute is the default.</summary>
		/// <returns>true if the attribute is the default value for this attribute class; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return this.localizable == LocalizableAttribute.Default.IsLocalizable;
		}
	}
}
