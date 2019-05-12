using System;

namespace System.ComponentModel
{
	/// <summary>Specifies the default event for a component.</summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DefaultEventAttribute : Attribute
	{
		private string eventName;

		/// <summary>Specifies the default value for the <see cref="T:System.ComponentModel.DefaultEventAttribute" />, which is null. This static field is read-only.</summary>
		public static readonly DefaultEventAttribute Default = new DefaultEventAttribute(null);

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DefaultEventAttribute" /> class.</summary>
		/// <param name="name">The name of the default event for the component this attribute is bound to. </param>
		public DefaultEventAttribute(string name)
		{
			this.eventName = name;
		}

		/// <summary>Gets the name of the default event for the component this attribute is bound to.</summary>
		/// <returns>The name of the default event for the component this attribute is bound to. The default value is null.</returns>
		public string Name
		{
			get
			{
				return this.eventName;
			}
		}

		/// <summary>Returns whether the value of the given object is equal to the current <see cref="T:System.ComponentModel.DefaultEventAttribute" />.</summary>
		/// <returns>true if the value of the given object is equal to that of the current; otherwise, false.</returns>
		/// <param name="obj">The object to test the value equality of. </param>
		public override bool Equals(object o)
		{
			return o is DefaultEventAttribute && ((DefaultEventAttribute)o).eventName == this.eventName;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
