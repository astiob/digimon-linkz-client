using System;

namespace System.Net.NetworkInformation
{
	/// <summary>The exception that is thrown when an error occurs while retrieving network information.</summary>
	[Serializable]
	public class NetworkInformationException : Exception
	{
		private int error_code;

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.NetworkInformation.NetworkInformationException" /> class.</summary>
		public NetworkInformationException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.NetworkInformation.NetworkInformationException" /> class with the specified error code.</summary>
		/// <param name="errorCode">A Win32 error code. </param>
		public NetworkInformationException(int errorCode)
		{
			this.error_code = errorCode;
		}

		/// <summary>Gets the Win32 error code for this exception.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that contains the Win32 error code.</returns>
		public int ErrorCode
		{
			get
			{
				return this.error_code;
			}
		}
	}
}
