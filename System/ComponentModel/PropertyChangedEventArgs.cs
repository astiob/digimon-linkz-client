using System;

namespace System.ComponentModel
{
	/// <summary>Provides data for the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" /> event.</summary>
	public class PropertyChangedEventArgs : EventArgs
	{
		private string propertyName;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.PropertyChangedEventArgs" /> class.</summary>
		/// <param name="propertyName">The name of the property that changed. </param>
		public PropertyChangedEventArgs(string name)
		{
			this.propertyName = name;
		}

		/// <summary>Gets the name of the property that changed.</summary>
		/// <returns>The name of the property that changed.</returns>
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}
	}
}
