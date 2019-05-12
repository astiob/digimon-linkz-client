using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Provides data for the <see cref="E:System.AppDomain.TypeResolve" />, <see cref="E:System.AppDomain.ResourceResolve" />, and <see cref="E:System.AppDomain.AssemblyResolve" /> events.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public class ResolveEventArgs : EventArgs
	{
		private string m_Name;

		/// <summary>Initializes a new instance of the <see cref="T:System.ResolveEventArgs" /> class.</summary>
		/// <param name="name">The name of an item to resolve. </param>
		public ResolveEventArgs(string name)
		{
			this.m_Name = name;
		}

		/// <summary>Gets the name of the item to resolve.</summary>
		/// <returns>The name of the item to resolve.</returns>
		/// <filterpriority>2</filterpriority>
		public string Name
		{
			get
			{
				return this.m_Name;
			}
		}
	}
}
