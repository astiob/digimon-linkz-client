using System;
using System.Collections;

namespace System.ComponentModel
{
	/// <summary>Extends the <see cref="T:System.ComponentModel.IBindingList" /> interface by providing advanced sorting and filtering capabilities.</summary>
	public interface IBindingListView : ICollection, IEnumerable, IList, IBindingList
	{
		/// <summary>Gets or sets the filter to be used to exclude items from the collection of items returned by the data source</summary>
		/// <returns>The string used to filter items out in the item collection returned by the data source. </returns>
		string Filter { get; set; }

		/// <summary>Gets the collection of sort descriptions currently applied to the data source.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ListSortDescriptionCollection" /> currently applied to the data source.</returns>
		ListSortDescriptionCollection SortDescriptions { get; }

		/// <summary>Gets a value indicating whether the data source supports advanced sorting. </summary>
		/// <returns>true if the data source supports advanced sorting; otherwise, false. </returns>
		bool SupportsAdvancedSorting { get; }

		/// <summary>Gets a value indicating whether the data source supports filtering. </summary>
		/// <returns>true if the data source supports filtering; otherwise, false. </returns>
		bool SupportsFiltering { get; }

		/// <summary>Sorts the data source based on the given <see cref="T:System.ComponentModel.ListSortDescriptionCollection" />.</summary>
		/// <param name="sorts">The <see cref="T:System.ComponentModel.ListSortDescriptionCollection" /> containing the sorts to apply to the data source.</param>
		void ApplySort(ListSortDescriptionCollection sorts);

		/// <summary>Removes the current filter applied to the data source.</summary>
		void RemoveFilter();
	}
}
