using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Specifies which culture the assembly supports.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCultureAttribute : Attribute
	{
		private string name;

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyCultureAttribute" /> class with the culture supported by the assembly being attributed.</summary>
		/// <param name="culture">The culture supported by the attributed assembly. </param>
		public AssemblyCultureAttribute(string culture)
		{
			this.name = culture;
		}

		/// <summary>Gets the supported culture of the attributed assembly.</summary>
		/// <returns>A string containing the name of the supported culture.</returns>
		public string Culture
		{
			get
			{
				return this.name;
			}
		}
	}
}
