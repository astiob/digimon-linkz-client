using System;

namespace System.ComponentModel
{
	/// <summary>Specifies whether a property or event should be displayed in a Properties window.</summary>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class BrowsableAttribute : Attribute
	{
		private bool browsable;

		/// <summary>Specifies the default value for the <see cref="T:System.ComponentModel.BrowsableAttribute" />, which is <see cref="F:System.ComponentModel.BrowsableAttribute.Yes" />. This static field is read-only.</summary>
		public static readonly BrowsableAttribute Default = new BrowsableAttribute(true);

		/// <summary>Specifies that a property or event cannot be modified at design time. This static field is read-only.</summary>
		public static readonly BrowsableAttribute No = new BrowsableAttribute(false);

		/// <summary>Specifies that a property or event can be modified at design time. This static field is read-only.</summary>
		public static readonly BrowsableAttribute Yes = new BrowsableAttribute(true);

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.BrowsableAttribute" /> class.</summary>
		/// <param name="browsable">true if a property or event can be modified at design time; otherwise, false. The default is true. </param>
		public BrowsableAttribute(bool browsable)
		{
			this.browsable = browsable;
		}

		/// <summary>Gets a value indicating whether an object is browsable.</summary>
		/// <returns>true if the object is browsable; otherwise, false.</returns>
		public bool Browsable
		{
			get
			{
				return this.browsable;
			}
		}

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <returns>true if <paramref name="obj" /> is equal to this instance; otherwise, false.</returns>
		/// <param name="obj">Another object to compare to. </param>
		public override bool Equals(object obj)
		{
			return obj is BrowsableAttribute && (obj == this || ((BrowsableAttribute)obj).Browsable == this.browsable);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return this.browsable.GetHashCode();
		}

		/// <summary>Determines if this attribute is the default.</summary>
		/// <returns>true if the attribute is the default value for this attribute class; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return this.browsable == BrowsableAttribute.Default.Browsable;
		}
	}
}
