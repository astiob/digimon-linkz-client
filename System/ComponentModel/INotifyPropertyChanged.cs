using System;

namespace System.ComponentModel
{
	/// <summary>Notifies clients that a property value has changed.</summary>
	public interface INotifyPropertyChanged
	{
		/// <summary>Occurs when a property value changes.</summary>
		event PropertyChangedEventHandler PropertyChanged;
	}
}
