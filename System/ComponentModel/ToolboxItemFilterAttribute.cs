using System;

namespace System.ComponentModel
{
	/// <summary>Specifies the filter string and filter type to use for a toolbox item.</summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	[Serializable]
	public sealed class ToolboxItemFilterAttribute : Attribute
	{
		private string Filter;

		private ToolboxItemFilterType ItemFilterType;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemFilterAttribute" /> class using the specified filter string.</summary>
		/// <param name="filterString">The filter string for the toolbox item. </param>
		public ToolboxItemFilterAttribute(string filterString)
		{
			this.Filter = filterString;
			this.ItemFilterType = ToolboxItemFilterType.Allow;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemFilterAttribute" /> class using the specified filter string and type.</summary>
		/// <param name="filterString">The filter string for the toolbox item. </param>
		/// <param name="filterType">A <see cref="T:System.ComponentModel.ToolboxItemFilterType" /> indicating the type of the filter. </param>
		public ToolboxItemFilterAttribute(string filterString, ToolboxItemFilterType filterType)
		{
			this.Filter = filterString;
			this.ItemFilterType = filterType;
		}

		/// <summary>Gets the filter string for the toolbox item.</summary>
		/// <returns>The filter string for the toolbox item.</returns>
		public string FilterString
		{
			get
			{
				return this.Filter;
			}
		}

		/// <summary>Gets the type of the filter.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.ToolboxItemFilterType" /> that indicates the type of the filter.</returns>
		public ToolboxItemFilterType FilterType
		{
			get
			{
				return this.ItemFilterType;
			}
		}

		/// <summary>Gets the type ID for the attribute.</summary>
		/// <returns>The type ID for this attribute. All <see cref="T:System.ComponentModel.ToolboxItemFilterAttribute" /> objects with the same filter string return the same type ID.</returns>
		public override object TypeId
		{
			get
			{
				return base.TypeId + this.Filter;
			}
		}

		/// <param name="obj">The object to compare.</param>
		public override bool Equals(object obj)
		{
			return obj is ToolboxItemFilterAttribute && (obj == this || (((ToolboxItemFilterAttribute)obj).FilterString == this.Filter && ((ToolboxItemFilterAttribute)obj).FilterType == this.ItemFilterType));
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		/// <summary>Indicates whether the specified object has a matching filter string.</summary>
		/// <returns>true if the specified object has a matching filter string; otherwise, false.</returns>
		/// <param name="obj">The object to test for a matching filter string. </param>
		public override bool Match(object obj)
		{
			return obj is ToolboxItemFilterAttribute && ((ToolboxItemFilterAttribute)obj).FilterString == this.Filter;
		}

		public override string ToString()
		{
			return string.Format("{0},{1}", this.Filter, this.ItemFilterType);
		}
	}
}
