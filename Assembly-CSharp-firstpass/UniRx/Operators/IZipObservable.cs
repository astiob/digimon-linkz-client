using System;

namespace UniRx.Operators
{
	internal interface IZipObservable
	{
		void Dequeue(int index);

		void Fail(Exception error);

		void Done(int index);
	}
}
