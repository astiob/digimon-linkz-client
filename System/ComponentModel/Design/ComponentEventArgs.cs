using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Provides data for the <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentAdded" />, <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentAdding" />, <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentRemoved" />, and <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentRemoving" /> events.</summary>
	[ComVisible(true)]
	public class ComponentEventArgs : EventArgs
	{
		private IComponent icomp;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.ComponentEventArgs" /> class.</summary>
		/// <param name="component">The component that is the source of the event. </param>
		public ComponentEventArgs(IComponent component)
		{
			this.icomp = component;
		}

		/// <summary>Gets the component associated with the event.</summary>
		/// <returns>The component associated with the event.</returns>
		public virtual IComponent Component
		{
			get
			{
				return this.icomp;
			}
		}
	}
}
