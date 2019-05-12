using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	/// <summary>The <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure is an object representation of a token that represents symbolic information.</summary>
	[ComVisible(true)]
	public struct SymbolToken
	{
		private int _val;

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure when given a value.</summary>
		/// <param name="val">The value to be used for the token. </param>
		public SymbolToken(int val)
		{
			this._val = val;
		}

		/// <summary>Determines whether <paramref name="obj" /> is an instance of <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> and is equal to this instance.</summary>
		/// <returns>true if <paramref name="obj" /> is an instance of <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> and is equal to this instance; otherwise, false.</returns>
		/// <param name="obj">The object to check. </param>
		public override bool Equals(object obj)
		{
			return obj is SymbolToken && ((SymbolToken)obj).GetToken() == this._val;
		}

		/// <summary>Determines whether <paramref name="obj" /> is equal to this instance.</summary>
		/// <returns>true if <paramref name="obj" /> is equal to this instance; otherwise, false.</returns>
		/// <param name="obj">The <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> to check.</param>
		public bool Equals(SymbolToken obj)
		{
			return obj.GetToken() == this._val;
		}

		/// <summary>Generates the hash code for the current token.</summary>
		/// <returns>The hash code for the current token.</returns>
		public override int GetHashCode()
		{
			return this._val.GetHashCode();
		}

		/// <summary>Gets the value of the current token.</summary>
		/// <returns>The value of the current token.</returns>
		public int GetToken()
		{
			return this._val;
		}

		/// <summary>Returns a value indicating whether two <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> objects are equal.</summary>
		/// <returns>true if <paramref name="a" /> and <paramref name="b" /> are equal; otherwise, false.</returns>
		/// <param name="a">A <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure.</param>
		/// <param name="b">A <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure.</param>
		public static bool operator ==(SymbolToken a, SymbolToken b)
		{
			return a.Equals(b);
		}

		/// <summary>Returns a value indicating whether two <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> objects are not equal.</summary>
		/// <returns>true if <paramref name="a" /> and <paramref name="b" /> are not equal; otherwise, false.</returns>
		/// <param name="a">A <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure.</param>
		/// <param name="b">A <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure.</param>
		public static bool operator !=(SymbolToken a, SymbolToken b)
		{
			return !a.Equals(b);
		}
	}
}
