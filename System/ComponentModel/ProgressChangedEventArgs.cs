using System;

namespace System.ComponentModel
{
	/// <summary>Provides data for the <see cref="E:System.ComponentModel.BackgroundWorker.ProgressChanged" /> event.</summary>
	public class ProgressChangedEventArgs : EventArgs
	{
		private int progress;

		private object state;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ProgressChangedEventArgs" /> class.</summary>
		/// <param name="progressPercentage">The percentage of an asynchronous task that has been completed.</param>
		/// <param name="userState">A unique user state.</param>
		public ProgressChangedEventArgs(int progressPercentage, object userState)
		{
			this.progress = progressPercentage;
			this.state = userState;
		}

		/// <summary>Gets the asynchronous task progress percentage.</summary>
		/// <returns>A percentage value indicating the asynchronous task progress.</returns>
		public int ProgressPercentage
		{
			get
			{
				return this.progress;
			}
		}

		/// <summary>Gets a unique user state.</summary>
		/// <returns>A unique <see cref="T:System.Object" /> indicating the user state.</returns>
		public object UserState
		{
			get
			{
				return this.state;
			}
		}
	}
}
