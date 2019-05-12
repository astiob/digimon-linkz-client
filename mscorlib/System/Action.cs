using System;

namespace System
{
	/// <summary>Encapsulates a method that takes a single parameter and does not return a value.</summary>
	/// <param name="obj">The parameter of the method that this delegate encapsulates.</param>
	/// <typeparam name="T">The type of the parameter of the method that this delegate encapsulates.</typeparam>
	/// <filterpriority>1</filterpriority>
	public delegate void Action<T>(T obj);
}
