using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Provides an interface for managing designer transactions and components.</summary>
	[ComVisible(true)]
	public interface IDesignerHost : IServiceContainer, IServiceProvider
	{
		/// <summary>Occurs when this designer is activated.</summary>
		event EventHandler Activated;

		/// <summary>Occurs when this designer is deactivated.</summary>
		event EventHandler Deactivated;

		/// <summary>Occurs when this designer completes loading its document.</summary>
		event EventHandler LoadComplete;

		/// <summary>Adds an event handler for the <see cref="E:System.ComponentModel.Design.IDesignerHost.TransactionClosed" /> event.</summary>
		event DesignerTransactionCloseEventHandler TransactionClosed;

		/// <summary>Adds an event handler for the <see cref="E:System.ComponentModel.Design.IDesignerHost.TransactionClosing" /> event.</summary>
		event DesignerTransactionCloseEventHandler TransactionClosing;

		/// <summary>Adds an event handler for the <see cref="E:System.ComponentModel.Design.IDesignerHost.TransactionOpened" /> event.</summary>
		event EventHandler TransactionOpened;

		/// <summary>Adds an event handler for the <see cref="E:System.ComponentModel.Design.IDesignerHost.TransactionOpening" /> event.</summary>
		event EventHandler TransactionOpening;

		/// <summary>Gets the container for this designer host.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.IContainer" /> for this host.</returns>
		IContainer Container { get; }

		/// <summary>Gets a value indicating whether the designer host is currently in a transaction.</summary>
		/// <returns>true if a transaction is in progress; otherwise, false.</returns>
		bool InTransaction { get; }

		/// <summary>Gets a value indicating whether the designer host is currently loading the document.</summary>
		/// <returns>true if the designer host is currently loading the document; otherwise, false.</returns>
		bool Loading { get; }

		/// <summary>Gets the instance of the base class used as the root component for the current design.</summary>
		/// <returns>The instance of the root component class.</returns>
		IComponent RootComponent { get; }

		/// <summary>Gets the fully qualified name of the class being designed.</summary>
		/// <returns>The fully qualified name of the base component class.</returns>
		string RootComponentClassName { get; }

		/// <summary>Gets the description of the current transaction.</summary>
		/// <returns>A description of the current transaction.</returns>
		string TransactionDescription { get; }

		/// <summary>Activates the designer that this host is hosting.</summary>
		void Activate();

		/// <summary>Creates a component of the specified type and adds it to the design document.</summary>
		/// <returns>The newly created component.</returns>
		/// <param name="componentClass">The type of the component to create. </param>
		IComponent CreateComponent(Type componentClass);

		/// <summary>Creates a component of the specified type and name, and adds it to the design document.</summary>
		/// <returns>The newly created component.</returns>
		/// <param name="componentClass">The type of the component to create. </param>
		/// <param name="name">The name for the component. </param>
		IComponent CreateComponent(Type componentClass, string name);

		/// <summary>Creates a <see cref="T:System.ComponentModel.Design.DesignerTransaction" /> that can encapsulate event sequences to improve performance and enable undo and redo support functionality.</summary>
		/// <returns>A new instance of <see cref="T:System.ComponentModel.Design.DesignerTransaction" />. When you complete the steps in your transaction, you should call <see cref="M:System.ComponentModel.Design.DesignerTransaction.Commit" /> on this object.</returns>
		DesignerTransaction CreateTransaction();

		/// <summary>Creates a <see cref="T:System.ComponentModel.Design.DesignerTransaction" /> that can encapsulate event sequences to improve performance and enable undo and redo support functionality, using the specified transaction description.</summary>
		/// <returns>A new <see cref="T:System.ComponentModel.Design.DesignerTransaction" />. When you have completed the steps in your transaction, you should call <see cref="M:System.ComponentModel.Design.DesignerTransaction.Commit" /> on this object.</returns>
		/// <param name="description">A title or description for the newly created transaction. </param>
		DesignerTransaction CreateTransaction(string description);

		/// <summary>Destroys the specified component and removes it from the designer container.</summary>
		/// <param name="component">The component to destroy. </param>
		void DestroyComponent(IComponent component);

		/// <summary>Gets the designer instance that contains the specified component.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.Design.IDesigner" />, or null if there is no designer for the specified component.</returns>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent" /> to retrieve the designer for. </param>
		IDesigner GetDesigner(IComponent component);

		/// <summary>Gets an instance of the specified, fully qualified type name.</summary>
		/// <returns>The type object for the specified type name, or null if the type cannot be found.</returns>
		/// <param name="typeName">The name of the type to load. </param>
		Type GetType(string typeName);
	}
}
