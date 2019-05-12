using System;

namespace UniRx.Operators
{
	internal interface ISelect<TR>
	{
		IObservable<TR> CombinePredicate(Func<TR, bool> predicate);
	}
}
