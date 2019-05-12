using System;

namespace System.Threading
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Windows.Forms.Application.ThreadException" /> event of an <see cref="T:System.Windows.Forms.Application" />.</summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">A <see cref="T:System.Threading.ThreadExceptionEventArgs" /> that contains the event data. </param>
	/// <filterpriority>2</filterpriority>
	public delegate void ThreadExceptionEventHandler(object sender, ThreadExceptionEventArgs e);
}
