using System;

namespace System.ComponentModel
{
	/// <summary>Specifies how the list changed.</summary>
	public enum ListChangedType
	{
		/// <summary>Much of the list has changed. Any listening controls should refresh all their data from the list.</summary>
		Reset,
		/// <summary>An item added to the list. <see cref="P:System.ComponentModel.ListChangedEventArgs.NewIndex" /> contains the index of the item that was added.</summary>
		ItemAdded,
		/// <summary>An item deleted from the list. <see cref="P:System.ComponentModel.ListChangedEventArgs.NewIndex" /> contains the index of the item that was deleted.</summary>
		ItemDeleted,
		/// <summary>An item moved within the list. <see cref="P:System.ComponentModel.ListChangedEventArgs.OldIndex" /> contains the previous index for the item, whereas <see cref="P:System.ComponentModel.ListChangedEventArgs.NewIndex" /> contains the new index for the item.</summary>
		ItemMoved,
		/// <summary>An item changed in the list. <see cref="P:System.ComponentModel.ListChangedEventArgs.NewIndex" /> contains the index of the item that was changed.</summary>
		ItemChanged,
		/// <summary>A <see cref="T:System.ComponentModel.PropertyDescriptor" /> was added, which changed the schema.</summary>
		PropertyDescriptorAdded,
		/// <summary>A <see cref="T:System.ComponentModel.PropertyDescriptor" /> was deleted, which changed the schema.</summary>
		PropertyDescriptorDeleted,
		/// <summary>A <see cref="T:System.ComponentModel.PropertyDescriptor" /> was changed, which changed the schema.</summary>
		PropertyDescriptorChanged
	}
}
