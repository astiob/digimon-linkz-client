using System;

namespace System.Net
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Net.WebClient.UploadFileCompleted" /> event of a <see cref="T:System.Net.WebClient" />.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A <see cref="T:System.Net.UploadFileCompletedEventArgs" /> that contains event data.</param>
	public delegate void UploadFileCompletedEventHandler(object sender, UploadFileCompletedEventArgs e);
}
