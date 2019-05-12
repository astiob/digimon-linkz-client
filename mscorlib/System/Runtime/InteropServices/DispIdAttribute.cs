using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Specifies the COM dispatch identifier (DISPID) of a method, field, or property.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event, Inherited = false)]
	public sealed class DispIdAttribute : Attribute
	{
		private int id;

		/// <summary>Initializes a new instance of the DispIdAttribute class with the specified DISPID.</summary>
		/// <param name="dispId">The DISPID for the member. </param>
		public DispIdAttribute(int dispId)
		{
			this.id = dispId;
		}

		/// <summary>Gets the DISPID for the member.</summary>
		/// <returns>The DISPID for the member.</returns>
		public int Value
		{
			get
			{
				return this.id;
			}
		}
	}
}
