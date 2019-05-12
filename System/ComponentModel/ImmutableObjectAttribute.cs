using System;

namespace System.ComponentModel
{
	/// <summary>Specifies that an object has no subproperties capable of being edited. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ImmutableObjectAttribute : Attribute
	{
		private bool immutable;

		/// <summary>Represents the default value for <see cref="T:System.ComponentModel.ImmutableObjectAttribute" />.</summary>
		public static readonly ImmutableObjectAttribute Default = new ImmutableObjectAttribute(false);

		/// <summary>Specifies that an object has at least one editable subproperty. This static field is read-only.</summary>
		public static readonly ImmutableObjectAttribute No = new ImmutableObjectAttribute(false);

		/// <summary>Specifies that an object has no subproperties that can be edited. This static field is read-only.</summary>
		public static readonly ImmutableObjectAttribute Yes = new ImmutableObjectAttribute(true);

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ImmutableObjectAttribute" /> class.</summary>
		/// <param name="immutable">true if the object is immutable; otherwise, false. </param>
		public ImmutableObjectAttribute(bool immutable)
		{
			this.immutable = immutable;
		}

		/// <summary>Gets whether the object is immutable.</summary>
		/// <returns>true if the object is immutable; otherwise, false.</returns>
		public bool Immutable
		{
			get
			{
				return this.immutable;
			}
		}

		/// <returns>true if <paramref name="obj" /> equals the type and value of this instance; otherwise, false.</returns>
		/// <param name="obj">An <see cref="T:System.Object" /> to compare with this instance or null. </param>
		public override bool Equals(object obj)
		{
			return obj is ImmutableObjectAttribute && (obj == this || ((ImmutableObjectAttribute)obj).Immutable == this.immutable);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A hash code for the current <see cref="T:System.ComponentModel.ImmutableObjectAttribute" />.</returns>
		public override int GetHashCode()
		{
			return this.immutable.GetHashCode();
		}

		/// <summary>Indicates whether the value of this instance is the default value.</summary>
		/// <returns>true if this instance is the default attribute for the class; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return this.immutable == ImmutableObjectAttribute.Default.Immutable;
		}
	}
}
