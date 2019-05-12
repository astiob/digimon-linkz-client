using System;

namespace System
{
	/// <summary>Encapsulates a method that has one parameter and returns a value of the type specified by the <paramref name="TResult" /> parameter.</summary>
	/// <returns>The return value of the method that this delegate encapsulates.</returns>
	/// <param name="arg">The parameter of the method that this delegate encapsulates.</param>
	/// <typeparam name="T">The type of the parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
	public delegate TResult Func<T, TResult>(T arg1);
}
