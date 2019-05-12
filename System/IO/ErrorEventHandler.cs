using System;

namespace System.IO
{
	/// <summary>Represents the method that will handle the <see cref="E:System.IO.FileSystemWatcher.Error" /> event of a <see cref="T:System.IO.FileSystemWatcher" /> object.</summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">An <see cref="T:System.IO.ErrorEventArgs" /> object that contains the event data. </param>
	/// <filterpriority>2</filterpriority>
	public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);
}
