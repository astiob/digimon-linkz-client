using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
	{
		public NotifyPropertyChangedInvocatorAttribute()
		{
		}

		public NotifyPropertyChangedInvocatorAttribute(string parameterName)
		{
			this.ParameterName = parameterName;
		}

		public string ParameterName { get; private set; }
	}
}
