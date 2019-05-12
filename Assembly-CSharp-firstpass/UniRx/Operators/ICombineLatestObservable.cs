using System;

namespace UniRx.Operators
{
	internal interface ICombineLatestObservable
	{
		void Publish(int index);

		void Fail(Exception error);

		void Done(int index);
	}
}
