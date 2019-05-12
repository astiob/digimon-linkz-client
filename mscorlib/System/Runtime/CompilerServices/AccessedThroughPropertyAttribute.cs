using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	/// <summary>Specifies the name of the property that accesses the attributed field.</summary>
	[AttributeUsage(AttributeTargets.Field)]
	[ComVisible(true)]
	public sealed class AccessedThroughPropertyAttribute : Attribute
	{
		private string name;

		/// <summary>Initializes a new instance of the AccessedThroughPropertyAttribute class with the name of the property used to access the attributed field.</summary>
		/// <param name="propertyName">The name of the property used to access the attributed field. </param>
		public AccessedThroughPropertyAttribute(string propertyName)
		{
			this.name = propertyName;
		}

		/// <summary>Gets the name of the property used to access the attributed field.</summary>
		/// <returns>The name of the property used to access the attributed field.</returns>
		public string PropertyName
		{
			get
			{
				return this.name;
			}
		}
	}
}
