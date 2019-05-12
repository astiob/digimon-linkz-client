using System;

namespace UniRx.Operators
{
	public delegate TR CombineLatestFunc<T1, T2, T3, TR>(T1 arg1, T2 arg2, T3 arg3);
}
