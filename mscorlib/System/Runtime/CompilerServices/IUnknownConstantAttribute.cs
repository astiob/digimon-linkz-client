using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	/// <summary>Indicates that the default value for the attributed field or parameter is an instance of <see cref="T:System.Runtime.InteropServices.UnknownWrapper" />, where the <see cref="P:System.Runtime.InteropServices.UnknownWrapper.WrappedObject" /> is null. This class cannot be inherited. </summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public sealed class IUnknownConstantAttribute : CustomConstantAttribute
	{
		/// <summary>Gets the IUnknown constant stored in this attribute.</summary>
		/// <returns>The IUnknown constant stored in this attribute. Only null is allowed for an IUnknown constant value.</returns>
		public override object Value
		{
			get
			{
				return null;
			}
		}
	}
}
