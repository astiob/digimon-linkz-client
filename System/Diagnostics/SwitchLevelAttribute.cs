using System;

namespace System.Diagnostics
{
	/// <summary>Identifies the level type for a switch. </summary>
	/// <filterpriority>1</filterpriority>
	[MonoLimitation("This attribute is not considered in trace support.")]
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SwitchLevelAttribute : Attribute
	{
		private Type type;

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.SwitchLevelAttribute" /> class, specifying the type that determines whether a trace should be written.</summary>
		/// <param name="switchLevelType">The <see cref="T:System.Type" /> that determines whether a trace should be written.</param>
		public SwitchLevelAttribute(Type switchLevelType)
		{
			if (switchLevelType == null)
			{
				throw new ArgumentNullException("switchLevelType");
			}
			this.type = switchLevelType;
		}

		/// <summary>Gets or sets the type that determines whether a trace should be written.</summary>
		/// <returns>The <see cref="T:System.Type" /> that determines whether a trace should be written.</returns>
		/// <exception cref="T:System.ArgumentNullException">The set operation failed because the value is null.</exception>
		/// <filterpriority>2</filterpriority>
		public Type SwitchLevelType
		{
			get
			{
				return this.type;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.type = value;
			}
		}
	}
}
