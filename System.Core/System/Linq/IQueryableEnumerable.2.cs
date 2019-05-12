using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
	internal interface IQueryableEnumerable<TElement> : IEnumerable, IOrderedQueryable, IQueryable, IQueryableEnumerable, IQueryable<TElement>, IEnumerable<TElement>, IOrderedQueryable<TElement>
	{
	}
}
