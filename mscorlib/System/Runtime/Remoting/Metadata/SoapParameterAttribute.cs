using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	/// <summary>Customizes SOAP generation and processing for a parameter. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	[ComVisible(true)]
	public sealed class SoapParameterAttribute : SoapAttribute
	{
	}
}
