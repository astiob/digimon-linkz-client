using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Provides data for the <see cref="E:System.ComponentModel.Design.IDesignerHost.TransactionClosed" /> and <see cref="E:System.ComponentModel.Design.IDesignerHost.TransactionClosing" /> events.</summary>
	[ComVisible(true)]
	public class DesignerTransactionCloseEventArgs : EventArgs
	{
		private bool commit;

		private bool last_transaction;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.DesignerTransactionCloseEventArgs" /> class. </summary>
		/// <param name="commit">A value indicating whether the transaction was committed.</param>
		/// <param name="lastTransaction">true if this is the last transaction to close; otherwise, false.</param>
		public DesignerTransactionCloseEventArgs(bool commit, bool lastTransaction)
		{
			this.commit = commit;
			this.last_transaction = lastTransaction;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.DesignerTransactionCloseEventArgs" /> class, using the specified value that indicates whether the designer called <see cref="M:System.ComponentModel.Design.DesignerTransaction.Commit" /> on the transaction.</summary>
		/// <param name="commit">A value indicating whether the transaction was committed.</param>
		[Obsolete("Use another constructor that indicates lastTransaction")]
		public DesignerTransactionCloseEventArgs(bool commit)
		{
			this.commit = commit;
		}

		/// <summary>Gets a value indicating whether this is the last transaction to close.</summary>
		/// <returns>true, if this is the last transaction to close; otherwise, false. </returns>
		public bool LastTransaction
		{
			get
			{
				return this.last_transaction;
			}
		}

		/// <summary>Indicates whether the designer called <see cref="M:System.ComponentModel.Design.DesignerTransaction.Commit" /> on the transaction.</summary>
		/// <returns>true if the designer called <see cref="M:System.ComponentModel.Design.DesignerTransaction.Commit" /> on the transaction; otherwise, false.</returns>
		public bool TransactionCommitted
		{
			get
			{
				return this.commit;
			}
		}
	}
}
