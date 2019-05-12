using System;

namespace UniRx.Operators
{
	internal interface IZipLatestObservable
	{
		void Publish(int index);

		void Fail(Exception error);

		void Done(int index);
	}
}
