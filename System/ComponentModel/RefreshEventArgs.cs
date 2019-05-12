using System;

namespace System.ComponentModel
{
	/// <summary>Provides data for the <see cref="E:System.ComponentModel.TypeDescriptor.Refreshed" /> event.</summary>
	public class RefreshEventArgs : EventArgs
	{
		private object component;

		private Type type;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.RefreshEventArgs" /> class with the component that has changed.</summary>
		/// <param name="componentChanged">The component that changed. </param>
		public RefreshEventArgs(object componentChanged)
		{
			if (componentChanged == null)
			{
				throw new ArgumentNullException("componentChanged");
			}
			this.component = componentChanged;
			this.type = this.component.GetType();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.RefreshEventArgs" /> class with the type of component that has changed.</summary>
		/// <param name="typeChanged">The <see cref="T:System.Type" /> that changed. </param>
		public RefreshEventArgs(Type typeChanged)
		{
			this.type = typeChanged;
		}

		/// <summary>Gets the component that changed its properties, events, or extenders.</summary>
		/// <returns>The component that changed its properties, events, or extenders, or null if all components of the same type have changed.</returns>
		public object ComponentChanged
		{
			get
			{
				return this.component;
			}
		}

		/// <summary>Gets the <see cref="T:System.Type" /> that changed its properties or events.</summary>
		/// <returns>The <see cref="T:System.Type" /> that changed its properties or events.</returns>
		public Type TypeChanged
		{
			get
			{
				return this.type;
			}
		}
	}
}
