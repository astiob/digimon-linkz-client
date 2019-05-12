using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	/// <summary>Specifies the network resource access that is granted to code.</summary>
	[ComVisible(true)]
	[Serializable]
	public class CodeConnectAccess
	{
		/// <summary>Contains the string value that represents the scheme wildcard.</summary>
		public static readonly string AnyScheme = "*";

		/// <summary>Contains the value used to represent the default port.</summary>
		public static readonly int DefaultPort = -3;

		/// <summary>Contains the value used to represent the port value in the URI where code originated.</summary>
		public static readonly int OriginPort = -4;

		/// <summary>Contains the value used to represent the scheme in the URL where the code originated.</summary>
		public static readonly string OriginScheme = "$origin";

		private string _scheme;

		private int _port;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.CodeConnectAccess" /> class. </summary>
		/// <param name="allowScheme">The URI scheme represented by the current instance.</param>
		/// <param name="allowPort">The port represented by the current instance.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="allowScheme" /> is null.-or-<paramref name="allowScheme" /> is an empty string ("").-or-<paramref name="allowScheme" /> contains characters that are not permitted in schemes.-or-<paramref name="allowPort" /> is less than 0.-or-<paramref name="allowPort" /> is greater than 65,535.</exception>
		[MonoTODO("(2.0) validations incomplete")]
		public CodeConnectAccess(string allowScheme, int allowPort)
		{
			if (allowScheme == null || allowScheme.Length == 0)
			{
				throw new ArgumentOutOfRangeException("allowScheme");
			}
			if (allowPort < 0 || allowPort > 65535)
			{
				throw new ArgumentOutOfRangeException("allowPort");
			}
			this._scheme = allowScheme;
			this._port = allowPort;
		}

		/// <summary>Gets the port represented by the current instance.</summary>
		/// <returns>A <see cref="T:System.Int32" /> value that identifies a computer port used in conjunction with the <see cref="P:System.Security.Policy.CodeConnectAccess.Scheme" /> property.</returns>
		public int Port
		{
			get
			{
				return this._port;
			}
		}

		/// <summary>Gets the URI scheme represented by the current instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that identifies a URI scheme, converted to lowercase.</returns>
		public string Scheme
		{
			get
			{
				return this._scheme;
			}
		}

		/// <summary>Returns a value indicating whether two <see cref="T:System.Security.Policy.CodeConnectAccess" /> objects represent the same scheme and port.</summary>
		/// <returns>true if the two objects represent the same scheme and port; otherwise, false.</returns>
		/// <param name="o">The object to compare to the current <see cref="T:System.Security.Policy.CodeConnectAccess" /> object.</param>
		public override bool Equals(object o)
		{
			CodeConnectAccess codeConnectAccess = o as CodeConnectAccess;
			return codeConnectAccess != null && this._scheme == codeConnectAccess._scheme && this._port == codeConnectAccess._port;
		}

		public override int GetHashCode()
		{
			return this._scheme.GetHashCode() ^ this._port;
		}

		/// <summary>Returns a <see cref="T:System.Security.Policy.CodeConnectAccess" /> instance that represents access to the specified port using any scheme.</summary>
		/// <returns>A <see cref="T:System.Security.Policy.CodeConnectAccess" /> instance for the specified port.</returns>
		/// <param name="allowPort">The port represented by the returned instance.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="allowPort" /> is less than 0.-or-<paramref name="allowPort" /> is greater than 65,535.</exception>
		public static CodeConnectAccess CreateAnySchemeAccess(int allowPort)
		{
			return new CodeConnectAccess(CodeConnectAccess.AnyScheme, allowPort);
		}

		/// <summary>Returns a <see cref="T:System.Security.Policy.CodeConnectAccess" /> instance that represents access to the specified port using the code's scheme of origin.</summary>
		/// <returns>A <see cref="T:System.Security.Policy.CodeConnectAccess" /> instance for the specified port.</returns>
		/// <param name="allowPort">The port represented by the returned instance.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="allowPort" /> is less than 0.-or-<paramref name="allowPort" /> is greater than 65,535.</exception>
		public static CodeConnectAccess CreateOriginSchemeAccess(int allowPort)
		{
			return new CodeConnectAccess(CodeConnectAccess.OriginScheme, allowPort);
		}
	}
}
