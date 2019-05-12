using System;

namespace System.ComponentModel.Design
{
	/// <summary>Provides a way to group a series of design-time actions to improve performance and enable most types of changes to be undone.</summary>
	public abstract class DesignerTransaction : IDisposable
	{
		private string description;

		private bool committed;

		private bool canceled;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.DesignerTransaction" /> class with no description.</summary>
		protected DesignerTransaction() : this(string.Empty)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.DesignerTransaction" /> class using the specified transaction description.</summary>
		/// <param name="description">A description for this transaction. </param>
		protected DesignerTransaction(string description)
		{
			this.description = description;
			this.committed = false;
			this.canceled = false;
		}

		/// <summary>Releases all resources used by the <see cref="T:System.ComponentModel.Design.DesignerTransaction" />. </summary>
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Design.DesignerTransaction" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected virtual void Dispose(bool disposing)
		{
			this.Cancel();
			if (disposing)
			{
				GC.SuppressFinalize(true);
			}
		}

		/// <summary>Raises the Cancel event.</summary>
		protected abstract void OnCancel();

		/// <summary>Performs the actual work of committing a transaction.</summary>
		protected abstract void OnCommit();

		/// <summary>Cancels the transaction and attempts to roll back the changes made by the events of the transaction.</summary>
		public void Cancel()
		{
			if (!this.Canceled && !this.Committed)
			{
				this.canceled = true;
				this.OnCancel();
			}
		}

		/// <summary>Commits this transaction.</summary>
		public void Commit()
		{
			if (!this.Canceled && !this.Committed)
			{
				this.committed = true;
				this.OnCommit();
			}
		}

		/// <summary>Gets a value indicating whether the transaction was canceled.</summary>
		/// <returns>true if the transaction was canceled; otherwise, false.</returns>
		public bool Canceled
		{
			get
			{
				return this.canceled;
			}
		}

		/// <summary>Gets a value indicating whether the transaction was committed.</summary>
		/// <returns>true if the transaction was committed; otherwise, false.</returns>
		public bool Committed
		{
			get
			{
				return this.committed;
			}
		}

		/// <summary>Gets a description for the transaction.</summary>
		/// <returns>A description for the transaction.</returns>
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		/// <summary>Releases the resources associated with this object. This override commits this transaction if it was not already committed.</summary>
		~DesignerTransaction()
		{
			this.Dispose(false);
		}
	}
}
