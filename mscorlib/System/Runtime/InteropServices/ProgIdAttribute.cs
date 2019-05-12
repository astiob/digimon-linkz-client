using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Allows the user to specify the ProgID of a class.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class ProgIdAttribute : Attribute
	{
		private string pid;

		/// <summary>Initializes a new instance of the ProgIdAttribute with the specified ProgID.</summary>
		/// <param name="progId">The ProgID to be assigned to the class. </param>
		public ProgIdAttribute(string progId)
		{
			this.pid = progId;
		}

		/// <summary>Gets the ProgID of the class.</summary>
		/// <returns>The ProgID of the class.</returns>
		public string Value
		{
			get
			{
				return this.pid;
			}
		}
	}
}
