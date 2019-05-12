using System;
using System.Collections;

namespace System.Linq
{
	internal interface IQueryableEnumerable : IEnumerable, IQueryable
	{
		IEnumerable GetEnumerable();
	}
}
