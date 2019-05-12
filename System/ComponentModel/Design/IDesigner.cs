using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Provides the basic framework for building a custom designer.</summary>
	[ComVisible(true)]
	public interface IDesigner : IDisposable
	{
		/// <summary>Gets the base component that this designer is designing.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.IComponent" /> indicating the base component that this designer is designing.</returns>
		IComponent Component { get; }

		/// <summary>Gets a collection of the design-time verbs supported by the designer.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.Design.DesignerVerbCollection" /> that contains the verbs supported by the designer, or null if the component has no verbs.</returns>
		DesignerVerbCollection Verbs { get; }

		/// <summary>Performs the default action for this designer.</summary>
		void DoDefaultAction();

		/// <summary>Initializes the designer with the specified component.</summary>
		/// <param name="component">The component to associate with this designer. </param>
		void Initialize(IComponent component);
	}
}
