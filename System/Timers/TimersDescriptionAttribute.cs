using System;
using System.ComponentModel;

namespace System.Timers
{
	/// <summary>Sets the description that visual designers can display when referencing an event, extender, or property.</summary>
	[AttributeUsage(AttributeTargets.All)]
	public class TimersDescriptionAttribute : System.ComponentModel.DescriptionAttribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Timers.TimersDescriptionAttribute" /> class.</summary>
		/// <param name="description">The description to use. </param>
		public TimersDescriptionAttribute(string description) : base(description)
		{
		}

		/// <summary>Gets the description that visual designers can display when referencing an event, extender, or property.</summary>
		/// <returns>The description for the event, extender, or property.</returns>
		public override string Description
		{
			get
			{
				return base.Description;
			}
		}
	}
}
