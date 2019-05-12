using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Provides an interface to add and remove the event handlers for events that add, change, remove or rename components, and provides methods to raise a <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentChanged" /> or <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentChanging" /> event.</summary>
	[ComVisible(true)]
	public interface IComponentChangeService
	{
		/// <summary>Occurs when a component has been added.</summary>
		event ComponentEventHandler ComponentAdded;

		/// <summary>Occurs when a component is in the process of being added.</summary>
		event ComponentEventHandler ComponentAdding;

		/// <summary>Occurs when a component has been changed.</summary>
		event ComponentChangedEventHandler ComponentChanged;

		/// <summary>Occurs when a component is in the process of being changed.</summary>
		event ComponentChangingEventHandler ComponentChanging;

		/// <summary>Occurs when a component has been removed.</summary>
		event ComponentEventHandler ComponentRemoved;

		/// <summary>Occurs when a component is in the process of being removed.</summary>
		event ComponentEventHandler ComponentRemoving;

		/// <summary>Occurs when a component is renamed.</summary>
		event ComponentRenameEventHandler ComponentRename;

		/// <summary>Announces to the component change service that a particular component has changed.</summary>
		/// <param name="component">The component that has changed. </param>
		/// <param name="member">The member that has changed. This is null if this change is not related to a single member. </param>
		/// <param name="oldValue">The old value of the member. This is valid only if the member is not null. </param>
		/// <param name="newValue">The new value of the member. This is valid only if the member is not null. </param>
		void OnComponentChanged(object component, MemberDescriptor member, object oldValue, object newValue);

		/// <summary>Announces to the component change service that a particular component is changing.</summary>
		/// <param name="component">The component that is about to change. </param>
		/// <param name="member">The member that is changing. This is null if this change is not related to a single member. </param>
		void OnComponentChanging(object component, MemberDescriptor member);
	}
}
