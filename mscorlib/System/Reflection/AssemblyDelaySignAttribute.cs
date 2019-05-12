using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Specifies that the assembly is not fully signed when created.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyDelaySignAttribute : Attribute
	{
		private bool delay;

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyDelaySignAttribute" /> class.</summary>
		/// <param name="delaySign">true if the feature this attribute represents is activated; otherwise, false. </param>
		public AssemblyDelaySignAttribute(bool delaySign)
		{
			this.delay = delaySign;
		}

		/// <summary>Gets a value indicating the state of the attribute.</summary>
		/// <returns>true if this assembly has been built as delay-signed; otherwise, false.</returns>
		public bool DelaySign
		{
			get
			{
				return this.delay;
			}
		}
	}
}
