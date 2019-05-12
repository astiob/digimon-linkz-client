using System;

namespace System.Net
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Net.WebClient.OpenReadCompleted" /> event of a <see cref="T:System.Net.WebClient" />.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A <see cref="T:System.Net.OpenReadCompletedEventArgs" /> containing event data.</param>
	public delegate void OpenReadCompletedEventHandler(object sender, OpenReadCompletedEventArgs e);
}
