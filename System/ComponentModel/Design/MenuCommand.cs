using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Represents a Windows menu or toolbar command item.</summary>
	[ComVisible(true)]
	public class MenuCommand
	{
		private EventHandler handler;

		private CommandID command;

		private bool ischecked;

		private bool enabled = true;

		private bool issupported = true;

		private bool visible = true;

		private Hashtable properties;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.MenuCommand" /> class.</summary>
		/// <param name="handler">The event to raise when the user selects the menu item or toolbar button. </param>
		/// <param name="command">The unique command ID that links this menu command to the environment's menu. </param>
		public MenuCommand(EventHandler handler, CommandID command)
		{
			this.handler = handler;
			this.command = command;
		}

		/// <summary>Occurs when the menu command changes.</summary>
		public event EventHandler CommandChanged;

		/// <summary>Gets or sets a value indicating whether this menu item is checked.</summary>
		/// <returns>true if the item is checked; otherwise, false.</returns>
		public virtual bool Checked
		{
			get
			{
				return this.ischecked;
			}
			set
			{
				if (this.ischecked != value)
				{
					this.ischecked = value;
					this.OnCommandChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets the <see cref="T:System.ComponentModel.Design.CommandID" /> associated with this menu command.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.Design.CommandID" /> associated with the menu command.</returns>
		public virtual CommandID CommandID
		{
			get
			{
				return this.command;
			}
		}

		/// <summary>Gets a value indicating whether this menu item is available.</summary>
		/// <returns>true if the item is enabled; otherwise, false.</returns>
		public virtual bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (this.enabled != value)
				{
					this.enabled = value;
					this.OnCommandChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets the OLE command status code for this menu item.</summary>
		/// <returns>An integer containing a mixture of status flags that reflect the state of this menu item.</returns>
		[MonoTODO]
		public virtual int OleStatus
		{
			get
			{
				return 3;
			}
		}

		/// <summary>Gets the public properties associated with the <see cref="T:System.ComponentModel.Design.MenuCommand" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionary" /> containing the public properties of the <see cref="T:System.ComponentModel.Design.MenuCommand" />. </returns>
		public virtual IDictionary Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new Hashtable();
				}
				return this.properties;
			}
		}

		/// <summary>Gets or sets a value indicating whether this menu item is supported.</summary>
		/// <returns>true if the item is supported, which is the default; otherwise, false.</returns>
		public virtual bool Supported
		{
			get
			{
				return this.issupported;
			}
			set
			{
				this.issupported = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether this menu item is visible.</summary>
		/// <returns>true if the item is visible; otherwise, false.</returns>
		public virtual bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				this.visible = value;
			}
		}

		/// <summary>Invokes the command.</summary>
		public virtual void Invoke()
		{
			if (this.handler != null)
			{
				this.handler(this, EventArgs.Empty);
			}
		}

		/// <summary>Invokes the command with the given parameter.</summary>
		/// <param name="arg">An optional argument for use by the command.</param>
		public virtual void Invoke(object arg)
		{
			this.Invoke();
		}

		/// <summary>Raises the <see cref="E:System.ComponentModel.Design.MenuCommand.CommandChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		protected virtual void OnCommandChanged(EventArgs e)
		{
			if (this.CommandChanged != null)
			{
				this.CommandChanged(this, e);
			}
		}

		/// <summary>Returns a string representation of this menu command.</summary>
		/// <returns>A string containing the value of the <see cref="P:System.ComponentModel.Design.MenuCommand.CommandID" /> property appended with the names of any flags that are set, separated by pipe bars (|). These flag properties include <see cref="P:System.ComponentModel.Design.MenuCommand.Checked" />, <see cref="P:System.ComponentModel.Design.MenuCommand.Enabled" />, <see cref="P:System.ComponentModel.Design.MenuCommand.Supported" />, and <see cref="P:System.ComponentModel.Design.MenuCommand.Visible" />.</returns>
		public override string ToString()
		{
			string text = string.Empty;
			if (this.command != null)
			{
				text = this.command.ToString();
			}
			text += " : ";
			if (this.Supported)
			{
				text += "Supported";
			}
			if (this.Enabled)
			{
				text += "|Enabled";
			}
			if (this.Visible)
			{
				text += "|Visible";
			}
			if (this.Checked)
			{
				text += "|Checked";
			}
			return text;
		}
	}
}
