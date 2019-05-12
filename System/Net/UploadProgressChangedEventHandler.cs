using System;

namespace System.Net
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Net.WebClient.UploadProgressChanged" /> event of a <see cref="T:System.Net.WebClient" />.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A <see cref="T:System.Net.UploadProgressChangedEventArgs" /> containing event data.</param>
	public delegate void UploadProgressChangedEventHandler(object sender, UploadProgressChangedEventArgs e);
}
