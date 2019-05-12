using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Provides data for the <see cref="E:System.AppDomain.AssemblyLoad" /> event.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public class AssemblyLoadEventArgs : EventArgs
	{
		private Assembly m_loadedAssembly;

		/// <summary>Initializes a new instance of the <see cref="T:System.AssemblyLoadEventArgs" /> class using the specified <see cref="T:System.Reflection.Assembly" />.</summary>
		/// <param name="loadedAssembly">An instance that represents the currently loaded assembly. </param>
		public AssemblyLoadEventArgs(Assembly loadedAssembly)
		{
			this.m_loadedAssembly = loadedAssembly;
		}

		/// <summary>Gets an <see cref="T:System.Reflection.Assembly" /> that represents the currently loaded assembly.</summary>
		/// <returns>An instance of <see cref="T:System.Reflection.Assembly" /> that represents the currently loaded assembly.</returns>
		/// <filterpriority>2</filterpriority>
		public Assembly LoadedAssembly
		{
			get
			{
				return this.m_loadedAssembly;
			}
		}
	}
}
