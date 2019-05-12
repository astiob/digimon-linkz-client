using System;

namespace System.ComponentModel
{
	/// <summary>Provides data for the <see cref="E:System.ComponentModel.BackgroundWorker.DoWork" /> event handler.</summary>
	public class DoWorkEventArgs : EventArgs
	{
		private object arg;

		private object result;

		private bool cancel;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DoWorkEventArgs" /> class.</summary>
		/// <param name="argument">Specifies an argument for an asynchronous operation.</param>
		public DoWorkEventArgs(object argument)
		{
			this.arg = argument;
		}

		/// <summary>Gets a value that represents the argument of an asynchronous operation.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing the argument of an asynchronous operation.</returns>
		public object Argument
		{
			get
			{
				return this.arg;
			}
		}

		/// <summary>Gets or sets a value that represents the result of an asynchronous operation.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing the result of an asynchronous operation.</returns>
		public object Result
		{
			get
			{
				return this.result;
			}
			set
			{
				this.result = value;
			}
		}

		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}
	}
}
