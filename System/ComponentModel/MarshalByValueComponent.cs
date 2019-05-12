using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	/// <summary>Implements <see cref="T:System.ComponentModel.IComponent" /> and provides the base implementation for remotable components that are marshaled by value (a copy of the serialized object is passed).</summary>
	[DesignerCategory("Component")]
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.ComponentDocumentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.ComponentModel.Design.IRootDesigner))]
	[TypeConverter(typeof(ComponentConverter))]
	public class MarshalByValueComponent : IDisposable, IServiceProvider, IComponent
	{
		private EventHandlerList eventList;

		private ISite mySite;

		private object disposedEvent = new object();

		/// <summary>Adds an event handler to listen to the <see cref="E:System.ComponentModel.MarshalByValueComponent.Disposed" /> event on the component.</summary>
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

		/// <summary>Releases all resources used by the <see cref="T:System.ComponentModel.MarshalByValueComponent" />.</summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.MarshalByValueComponent" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		[MonoTODO]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
		}

		~MarshalByValueComponent()
		{
			this.Dispose(false);
		}

		/// <summary>Gets the implementer of the <see cref="T:System.IServiceProvider" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the implementer of the <see cref="T:System.IServiceProvider" />.</returns>
		/// <param name="service">A <see cref="T:System.Type" /> that represents the type of service you want. </param>
		public virtual object GetService(Type service)
		{
			if (this.mySite != null)
			{
				return this.mySite.GetService(service);
			}
			return null;
		}

		/// <summary>Gets the container for the component.</summary>
		/// <returns>An object implementing the <see cref="T:System.ComponentModel.IContainer" /> interface that represents the component's container, or null if the component does not have a site.</returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IContainer Container
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

		/// <summary>Gets a value indicating whether the component is currently in design mode.</summary>
		/// <returns>true if the component is in design mode; otherwise, false.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual bool DesignMode
		{
			get
			{
				return this.mySite != null && this.mySite.DesignMode;
			}
		}

		/// <summary>Gets or sets the site of the component.</summary>
		/// <returns>An object implementing the <see cref="T:System.ComponentModel.ISite" /> interface that represents the site of the component.</returns>
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

		/// <summary>Returns a <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any. This method should not be overridden.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any.null if the <see cref="T:System.ComponentModel.Component" /> is unnamed.</returns>
		public override string ToString()
		{
			if (this.mySite == null)
			{
				return base.GetType().ToString();
			}
			return string.Format("{0} [{1}]", this.mySite.Name, base.GetType().ToString());
		}

		/// <summary>Gets the list of event handlers that are attached to this component.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.EventHandlerList" /> that provides the delegates for this component.</returns>
		protected EventHandlerList Events
		{
			get
			{
				if (this.eventList == null)
				{
					this.eventList = new EventHandlerList();
				}
				return this.eventList;
			}
		}
	}
}
