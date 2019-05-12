using System;
using System.ComponentModel;
using System.IO;

namespace System.Net
{
	/// <summary>Provides data for the <see cref="E:System.Net.WebClient.OpenReadCompleted" /> event.</summary>
	public class OpenReadCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
	{
		private Stream result;

		internal OpenReadCompletedEventArgs(Stream result, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
			this.result = result;
		}

		/// <summary>Gets a readable stream that contains data downloaded by a <see cref="Overload:System.Net.WebClient.DownloadDataAsync" /> method.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> that contains the downloaded data.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public Stream Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.result;
			}
		}
	}
}
