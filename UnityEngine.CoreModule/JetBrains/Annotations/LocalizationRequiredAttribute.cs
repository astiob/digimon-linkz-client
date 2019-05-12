using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class LocalizationRequiredAttribute : Attribute
	{
		public LocalizationRequiredAttribute() : this(true)
		{
		}

		public LocalizationRequiredAttribute(bool required)
		{
			this.Required = required;
		}

		public bool Required { get; private set; }
	}
}
