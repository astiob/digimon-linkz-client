using System;

namespace System.ComponentModel
{
	/// <summary>
	///   <see cref="T:System.ComponentModel.DesignTimeVisibleAttribute" /> marks a component's visibility. If <see cref="F:System.ComponentModel.DesignTimeVisibleAttribute.Yes" /> is present, a visual designer can show this component on a designer.</summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class DesignTimeVisibleAttribute : Attribute
	{
		private bool visible;

		/// <summary>The default visibility which is Yes.</summary>
		public static readonly DesignTimeVisibleAttribute Default = new DesignTimeVisibleAttribute(true);

		/// <summary>Marks a component as not visible in a visual designer.</summary>
		public static readonly DesignTimeVisibleAttribute No = new DesignTimeVisibleAttribute(false);

		/// <summary>Marks a component as visible in a visual designer.</summary>
		public static readonly DesignTimeVisibleAttribute Yes = new DesignTimeVisibleAttribute(true);

		/// <summary>Creates a new <see cref="T:System.ComponentModel.DesignTimeVisibleAttribute" /> set to the default value of false.</summary>
		public DesignTimeVisibleAttribute() : this(true)
		{
		}

		/// <summary>Creates a new <see cref="T:System.ComponentModel.DesignTimeVisibleAttribute" /> with the <see cref="P:System.ComponentModel.DesignTimeVisibleAttribute.Visible" /> property set to the given value in <paramref name="visible" />.</summary>
		/// <param name="visible">The value that the <see cref="P:System.ComponentModel.DesignTimeVisibleAttribute.Visible" /> property will be set against. </param>
		public DesignTimeVisibleAttribute(bool visible)
		{
			this.visible = visible;
		}

		/// <summary>Gets or sets whether the component should be shown at design time.</summary>
		/// <returns>true if this component should be shown at design time, or false if it shouldn't.</returns>
		public bool Visible
		{
			get
			{
				return this.visible;
			}
		}

		/// <param name="obj">The object to compare.</param>
		public override bool Equals(object obj)
		{
			return obj is DesignTimeVisibleAttribute && (obj == this || ((DesignTimeVisibleAttribute)obj).Visible == this.visible);
		}

		public override int GetHashCode()
		{
			return this.visible.GetHashCode();
		}

		/// <summary>Gets a value indicating if this instance is equal to the <see cref="F:System.ComponentModel.DesignTimeVisibleAttribute.Default" /> value.</summary>
		/// <returns>true, if this instance is equal to the <see cref="F:System.ComponentModel.DesignTimeVisibleAttribute.Default" /> value; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return this.visible == DesignTimeVisibleAttribute.Default.Visible;
		}
	}
}
