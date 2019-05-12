using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Provides support for root-level designer view technologies.</summary>
	[ComVisible(true)]
	public interface IRootDesigner : IDisposable, IDesigner
	{
		/// <summary>Gets the set of technologies that this designer can support for its display.</summary>
		/// <returns>An array of supported <see cref="T:System.ComponentModel.Design.ViewTechnology" /> values.</returns>
		ViewTechnology[] SupportedTechnologies { get; }

		/// <summary>Gets a view object for the specified view technology.</summary>
		/// <returns>An object that represents the view for this designer.</returns>
		/// <param name="technology">A <see cref="T:System.ComponentModel.Design.ViewTechnology" /> that indicates a particular view technology.</param>
		/// <exception cref="T:System.ArgumentException">The specified view technology is not supported or does not exist. </exception>
		object GetView(ViewTechnology technology);
	}
}
