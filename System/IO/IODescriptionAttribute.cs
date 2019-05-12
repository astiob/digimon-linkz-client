using System;
using System.ComponentModel;

namespace System.IO
{
	/// <summary>Sets the description visual designers can display when referencing an event, extender, or property.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.All)]
	public class IODescriptionAttribute : System.ComponentModel.DescriptionAttribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.IO.IODescriptionAttribute" /> class.</summary>
		/// <param name="description">The description to use. </param>
		public IODescriptionAttribute(string description) : base(description)
		{
		}

		/// <summary>Gets the description.</summary>
		/// <returns>The description for the event, extender, or property.</returns>
		/// <filterpriority>2</filterpriority>
		public override string Description
		{
			get
			{
				return base.DescriptionValue;
			}
		}
	}
}
