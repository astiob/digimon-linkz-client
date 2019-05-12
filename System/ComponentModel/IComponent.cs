using System;

namespace System.ComponentModel
{
	/// <summary>Provides functionality required by all components. </summary>
	public interface IComponent : IDisposable
	{
		/// <summary>Represents the method that handles the <see cref="E:System.ComponentModel.IComponent.Disposed" /> event of a component.</summary>
		event EventHandler Disposed;

		/// <summary>Gets or sets the <see cref="T:System.ComponentModel.ISite" /> associated with the <see cref="T:System.ComponentModel.IComponent" />.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISite" /> object associated with the component; or null, if the component does not have a site.</returns>
		ISite Site { get; set; }
	}
}
