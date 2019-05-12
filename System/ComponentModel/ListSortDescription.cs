using System;

namespace System.ComponentModel
{
	/// <summary>Provides a description of the sort operation applied to a data source.</summary>
	public class ListSortDescription
	{
		private PropertyDescriptor propertyDescriptor;

		private ListSortDirection sortDirection;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ListSortDescription" /> class with the specified property description and direction.</summary>
		/// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> that describes the property by which the data source is sorted.</param>
		/// <param name="direction">One of the <see cref="T:System.ComponentModel.ListSortDescription" />  values.</param>
		public ListSortDescription(PropertyDescriptor propertyDescriptor, ListSortDirection sortDirection)
		{
			this.propertyDescriptor = propertyDescriptor;
			this.sortDirection = sortDirection;
		}

		/// <summary>Gets or sets the abstract description of a class property associated with this <see cref="T:System.ComponentModel.ListSortDescription" /></summary>
		/// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor" /> associated with this <see cref="T:System.ComponentModel.ListSortDescription" />. </returns>
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.propertyDescriptor;
			}
			set
			{
				this.propertyDescriptor = value;
			}
		}

		/// <summary>Gets or sets the direction of the sort operation associated with this <see cref="T:System.ComponentModel.ListSortDescription" />.</summary>
		/// <returns>One of the <see cref="T:System.ComponentModel.ListSortDirection" /> values. </returns>
		public ListSortDirection SortDirection
		{
			get
			{
				return this.sortDirection;
			}
			set
			{
				this.sortDirection = value;
			}
		}
	}
}
