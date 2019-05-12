using System;

namespace System
{
	/// <summary>Encapsulates a method that has no parameters and returns a value of the type specified by the <paramref name="TResult" /> parameter.</summary>
	/// <returns>The return value of the method that this delegate encapsulates.</returns>
	/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
	public delegate TResult Func<TResult>();
}
