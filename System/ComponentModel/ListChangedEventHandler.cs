using System;

namespace System.ComponentModel
{
	/// <summary>Represents the method that will handle the <see cref="E:System.ComponentModel.IBindingList.ListChanged" /> event of the <see cref="T:System.ComponentModel.IBindingList" /> class.</summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">A <see cref="T:System.ComponentModel.ListChangedEventArgs" /> that contains the event data. </param>
	public delegate void ListChangedEventHandler(object sender, ListChangedEventArgs e);
}
