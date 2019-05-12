using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
	/// <summary>Represents the result of a sorting operation.</summary>
	/// <typeparam name="T">The type of the content of the data source.</typeparam>
	public interface IOrderedQueryable<T> : IEnumerable, IOrderedQueryable, IQueryable, IQueryable<T>, IEnumerable<T>
	{
	}
}
