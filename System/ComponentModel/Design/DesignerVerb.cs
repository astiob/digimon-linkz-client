using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Represents a verb that can be invoked from a designer.</summary>
	[ComVisible(true)]
	public class DesignerVerb : MenuCommand
	{
		private string text;

		private string description;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.DesignerVerb" /> class.</summary>
		/// <param name="text">The text of the menu command that is shown to the user. </param>
		/// <param name="handler">The event handler that performs the actions of the verb. </param>
		public DesignerVerb(string text, EventHandler handler) : this(text, handler, StandardCommands.VerbFirst)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.DesignerVerb" /> class.</summary>
		/// <param name="text">The text of the menu command that is shown to the user. </param>
		/// <param name="handler">The event handler that performs the actions of the verb. </param>
		/// <param name="startCommandID">The starting command ID for this verb. By default, the designer architecture sets aside a range of command IDs for verbs. You can override this by providing a custom command ID. </param>
		public DesignerVerb(string text, EventHandler handler, CommandID startCommandID) : base(handler, startCommandID)
		{
			this.text = text;
		}

		/// <summary>Gets the text description for the verb command on the menu.</summary>
		/// <returns>A description for the verb command.</returns>
		public string Text
		{
			get
			{
				return this.text;
			}
		}

		/// <summary>Gets or sets the description of the menu item for the verb.</summary>
		/// <returns>A string describing the menu item. </returns>
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		/// <summary>Overrides <see cref="M:System.Object.ToString" />.</summary>
		/// <returns>The verb's text, or an empty string ("") if the text field is empty.</returns>
		public override string ToString()
		{
			return this.text + " : " + base.ToString();
		}
	}
}
