using System;
using System.ComponentModel;

namespace System.Net
{
	/// <summary>Provides data for the <see cref="E:System.Net.WebClient.UploadStringCompleted" /> event.</summary>
	public class UploadStringCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
	{
		private string result;

		internal UploadStringCompletedEventArgs(string result, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
			this.result = result;
		}

		/// <summary>Gets the server reply to a string upload operation that is started by calling an <see cref="Overload:System.Net.WebClient.UploadStringAsync" /> method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array that contains the server reply.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.result;
			}
		}
	}
}
