using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Provides data for the <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentRename" /> event.</summary>
	[ComVisible(true)]
	public class ComponentRenameEventArgs : EventArgs
	{
		private object component;

		private string oldName;

		private string newName;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.ComponentRenameEventArgs" /> class.</summary>
		/// <param name="component">The component to be renamed. </param>
		/// <param name="oldName">The old name of the component. </param>
		/// <param name="newName">The new name of the component. </param>
		public ComponentRenameEventArgs(object component, string oldName, string newName)
		{
			this.component = component;
			this.oldName = oldName;
			this.newName = newName;
		}

		/// <summary>Gets the component that is being renamed.</summary>
		/// <returns>The component that is being renamed.</returns>
		public object Component
		{
			get
			{
				return this.component;
			}
		}

		/// <summary>Gets the name of the component after the rename event.</summary>
		/// <returns>The name of the component after the rename event.</returns>
		public virtual string NewName
		{
			get
			{
				return this.newName;
			}
		}

		/// <summary>Gets the name of the component before the rename event.</summary>
		/// <returns>The previous name of the component.</returns>
		public virtual string OldName
		{
			get
			{
				return this.oldName;
			}
		}
	}
}
