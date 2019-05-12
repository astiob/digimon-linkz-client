using System;
using System.ComponentModel;
using System.IO;

namespace System.Net
{
	/// <summary>Provides data for the <see cref="E:System.Net.WebClient.OpenWriteCompleted" /> event.</summary>
	public class OpenWriteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
	{
		private Stream result;

		internal OpenWriteCompletedEventArgs(Stream result, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
			this.result = result;
		}

		/// <summary>Gets a writable stream that is used to send data to a server.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> where you can write data to be uploaded.</returns>
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
