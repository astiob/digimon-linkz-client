using System;

namespace System.ComponentModel
{
	/// <summary>Represents the method that handles the <see cref="E:System.Data.DataColumnCollection.CollectionChanged" /> event raised when adding elements to or removing elements from a collection.</summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">A <see cref="T:System.ComponentModel.CollectionChangeEventArgs" /> that contains the event data. </param>
	public delegate void CollectionChangeEventHandler(object sender, CollectionChangeEventArgs e);
}
