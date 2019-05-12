using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Represents the method that handles the <see cref="E:System.ComponentModel.Design.IDesignerHost.TransactionClosed" /> and <see cref="E:System.ComponentModel.Design.IDesignerHost.TransactionClosing" /> events of a designer.</summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">A <see cref="T:System.ComponentModel.Design.DesignerTransactionCloseEventArgs" />  that contains the event data. </param>
	[ComVisible(true)]
	public delegate void DesignerTransactionCloseEventHandler(object sender, DesignerTransactionCloseEventArgs e);
}
