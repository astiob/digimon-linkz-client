using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Provides a callback mechanism that can create an instance of a service on demand.</summary>
	/// <returns>The service specified by <paramref name="serviceType" />, or null if the service could not be created. </returns>
	/// <param name="container">The service container that requested the creation of the service. </param>
	/// <param name="serviceType">The type of service to create. </param>
	[ComVisible(true)]
	public delegate object ServiceCreatorCallback(IServiceContainer container, Type serviceType);
}
