using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	/// <summary>Provides the base implementation for the <see cref="T:System.ComponentModel.IComponent" /> interface and enables object sharing between applications.</summary>
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DesignerCategory("Component")]
	[ComVisible(true)]
	public class Component : MarshalByRefObject, IDisposable, IComponent
	{
		private EventHandlerList event_handlers;

		private ISite mySite;

		private object disposedEvent = new object();

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Component" /> class. </summary>
		public Component()
		{
			this.event_handlers = null;
		}

		/// <summary>Occurs when the component is disposed by a call to the <see cref="M:System.ComponentModel.Component.Dispose" /> method. </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler Disposed
		{
			add
			{
				this.Events.AddHandler(this.disposedEvent, value);
			}
			remove
			{
				this.Events.RemoveHandler(this.disposedEvent, value);
			}
		}

		/// <summary>Gets a value indicating whether the component can raise an event.</summary>
		/// <returns>true if the component can raise events; otherwise, false. The default is true.</returns>
		protected virtual bool CanRaiseEvents
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.ComponentModel.ISite" /> of the <see cref="T:System.ComponentModel.Component" />.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISite" /> associated with the <see cref="T:System.ComponentModel.Component" />, or null if the <see cref="T:System.ComponentModel.Component" /> is not encapsulated in an <see cref="T:System.ComponentModel.IContainer" />, the <see cref="T:System.ComponentModel.Component" /> does not have an <see cref="T:System.ComponentModel.ISite" /> associated with it, or the <see cref="T:System.ComponentModel.Component" /> is removed from its <see cref="T:System.ComponentModel.IContainer" />.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual ISite Site
		{
			get
			{
				return this.mySite;
			}
			set
			{
				this.mySite = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.ComponentModel.IContainer" /> that contains the <see cref="T:System.ComponentModel.Component" />.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.IContainer" /> that contains the <see cref="T:System.ComponentModel.Component" />, if any, or null if the <see cref="T:System.ComponentModel.Component" /> is not encapsulated in an <see cref="T:System.ComponentModel.IContainer" />.</returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IContainer Container
		{
			get
			{
				if (this.mySite == null)
				{
					return null;
				}
				return this.mySite.Container;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.ComponentModel.Component" /> is currently in design mode.</summary>
		/// <returns>true if the <see cref="T:System.ComponentModel.Component" /> is in design mode; otherwise, false.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		protected bool DesignMode
		{
			get
			{
				return this.mySite != null && this.mySite.DesignMode;
			}
		}

		/// <summary>Gets the list of event handlers that are attached to this <see cref="T:System.ComponentModel.Component" />.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.EventHandlerList" /> that provides the delegates for this component.</returns>
		protected EventHandlerList Events
		{
			get
			{
				if (this.event_handlers == null)
				{
					this.event_handlers = new EventHandlerList();
				}
				return this.event_handlers;
			}
		}

		/// <summary>Releases unmanaged resources and performs other cleanup operations before the <see cref="T:System.ComponentModel.Component" /> is reclaimed by garbage collection.</summary>
		~Component()
		{
			this.Dispose(false);
		}

		/// <summary>Releases all resources used by the <see cref="T:System.ComponentModel.Component" />.</summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected virtual void Dispose(bool release_all)
		{
			if (release_all)
			{
				if (this.mySite != null && this.mySite.Container != null)
				{
					this.mySite.Container.Remove(this);
				}
				EventHandler eventHandler = (EventHandler)this.Events[this.disposedEvent];
				if (eventHandler != null)
				{
					eventHandler(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>Returns an object that represents a service provided by the <see cref="T:System.ComponentModel.Component" /> or by its <see cref="T:System.ComponentModel.Container" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents a service provided by the <see cref="T:System.ComponentModel.Component" />, or null if the <see cref="T:System.ComponentModel.Component" /> does not provide the specified service.</returns>
		/// <param name="service">A service provided by the <see cref="T:System.ComponentModel.Component" />. </param>
		protected virtual object GetService(Type service)
		{
			if (this.mySite != null)
			{
				return this.mySite.GetService(service);
			}
			return null;
		}

		/// <summary>Returns a <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any. This method should not be overridden.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any, or null if the <see cref="T:System.ComponentModel.Component" /> is unnamed.</returns>
		public override string ToString()
		{
			if (this.mySite == null)
			{
				return base.GetType().ToString();
			}
			return string.Format("{0} [{1}]", this.mySite.Name, base.GetType().ToString());
		}
	}
}
