using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Provides a text description for an assembly.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyDescriptionAttribute : Attribute
	{
		private string name;

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyDescriptionAttribute" /> class.</summary>
		/// <param name="description">The assembly description. </param>
		public AssemblyDescriptionAttribute(string description)
		{
			this.name = description;
		}

		/// <summary>Gets assembly description information.</summary>
		/// <returns>A string containing the assembly description.</returns>
		public string Description
		{
			get
			{
				return this.name;
			}
		}
	}
}
