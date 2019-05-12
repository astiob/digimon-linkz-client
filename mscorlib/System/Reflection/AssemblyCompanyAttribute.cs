using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Defines a company name custom attribute for an assembly manifest.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCompanyAttribute : Attribute
	{
		private string name;

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyCompanyAttribute" /> class.</summary>
		/// <param name="company">The company name information. </param>
		public AssemblyCompanyAttribute(string company)
		{
			this.name = company;
		}

		/// <summary>Gets company name information.</summary>
		/// <returns>A string containing the company name.</returns>
		public string Company
		{
			get
			{
				return this.name;
			}
		}
	}
}
