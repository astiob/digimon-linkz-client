using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Specifies the build configuration, such as retail or debug, for an assembly.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyConfigurationAttribute : Attribute
	{
		private string name;

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyConfigurationAttribute" /> class.</summary>
		/// <param name="configuration">The assembly configuration. </param>
		public AssemblyConfigurationAttribute(string configuration)
		{
			this.name = configuration;
		}

		/// <summary>Gets assembly configuration information.</summary>
		/// <returns>A string containing the assembly configuration information.</returns>
		public string Configuration
		{
			get
			{
				return this.name;
			}
		}
	}
}
