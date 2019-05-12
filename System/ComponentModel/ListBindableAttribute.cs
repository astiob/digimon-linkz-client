using System;

namespace System.ComponentModel
{
	/// <summary>Specifies that a list can be used as a data source. A visual designer should use this attribute to determine whether to display a particular list in a data-binding picker. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class ListBindableAttribute : Attribute
	{
		/// <summary>Represents the default value for <see cref="T:System.ComponentModel.ListBindableAttribute" />.</summary>
		public static readonly ListBindableAttribute Default = new ListBindableAttribute(true);

		/// <summary>Specifies that the list is not bindable. This static field is read-only.</summary>
		public static readonly ListBindableAttribute No = new ListBindableAttribute(false);

		/// <summary>Specifies that the list is bindable. This static field is read-only.</summary>
		public static readonly ListBindableAttribute Yes = new ListBindableAttribute(true);

		private bool bindable;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ListBindableAttribute" /> class using a value to indicate whether the list is bindable.</summary>
		/// <param name="listBindable">true if the list is bindable; otherwise, false. </param>
		public ListBindableAttribute(bool listBindable)
		{
			this.bindable = listBindable;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ListBindableAttribute" /> class using <see cref="T:System.ComponentModel.BindableSupport" /> to indicate whether the list is bindable.</summary>
		/// <param name="flags">A <see cref="T:System.ComponentModel.BindableSupport" /> that indicates whether the list is bindable. </param>
		public ListBindableAttribute(BindableSupport flags)
		{
			if (flags == BindableSupport.No)
			{
				this.bindable = false;
			}
			else
			{
				this.bindable = true;
			}
		}

		/// <summary>Returns whether the object passed is equal to this <see cref="T:System.ComponentModel.ListBindableAttribute" />.</summary>
		/// <returns>true if the object passed is equal to this <see cref="T:System.ComponentModel.ListBindableAttribute" />; otherwise, false.</returns>
		/// <param name="obj">The object to test equality with. </param>
		public override bool Equals(object obj)
		{
			return obj is ListBindableAttribute && ((ListBindableAttribute)obj).ListBindable.Equals(this.bindable);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A hash code for the current <see cref="T:System.ComponentModel.ListBindableAttribute" />.</returns>
		public override int GetHashCode()
		{
			return this.bindable.GetHashCode();
		}

		/// <summary>Returns whether <see cref="P:System.ComponentModel.ListBindableAttribute.ListBindable" /> is set to the default value.</summary>
		/// <returns>true if <see cref="P:System.ComponentModel.ListBindableAttribute.ListBindable" /> is set to the default value; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ListBindableAttribute.Default);
		}

		/// <summary>Gets whether the list is bindable.</summary>
		/// <returns>true if the list is bindable; otherwise, false.</returns>
		public bool ListBindable
		{
			get
			{
				return this.bindable;
			}
		}
	}
}
