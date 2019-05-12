using System;

namespace System
{
	/// <summary>Represents a method that converts an object from one type to another type.</summary>
	/// <returns>The <paramref name="TOutput" /> that represents the converted <paramref name="TInput" />.</returns>
	/// <param name="input">The object to convert.</param>
	/// <typeparam name="TInput">The type of object that is to be converted.</typeparam>
	/// <typeparam name="TOutput">The type the input object is to be converted to.</typeparam>
	/// <filterpriority>1</filterpriority>
	public delegate TOutput Converter<TInput, TOutput>(TInput input);
}
