using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	/// <summary>Provides functionality for containers. Containers are objects that logically contain zero or more components.</summary>
	[ComVisible(true)]
	public interface IContainer : IDisposable
	{
		/// <summary>Gets all the components in the <see cref="T:System.ComponentModel.IContainer" />.</summary>
		/// <returns>A collection of <see cref="T:System.ComponentModel.IComponent" /> objects that represents all the components in the <see cref="T:System.ComponentModel.IContainer" />.</returns>
		ComponentCollection Components { get; }

		/// <summary>Adds the specified <see cref="T:System.ComponentModel.IComponent" /> to the <see cref="T:System.ComponentModel.IContainer" /> at the end of the list.</summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent" /> to add. </param>
		void Add(IComponent component);

		/// <summary>Adds the specified <see cref="T:System.ComponentModel.IComponent" /> to the <see cref="T:System.ComponentModel.IContainer" /> at the end of the list, and assigns a name to the component.</summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent" /> to add. </param>
		/// <param name="name">The unique, case-insensitive name to assign to the component.-or- null that leaves the component unnamed. </param>
		void Add(IComponent component, string name);

		/// <summary>Removes a component from the <see cref="T:System.ComponentModel.IContainer" />.</summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent" /> to remove. </param>
		void Remove(IComponent component);
	}
}
