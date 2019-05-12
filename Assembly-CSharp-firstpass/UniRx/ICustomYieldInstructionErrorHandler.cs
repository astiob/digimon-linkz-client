using System;

namespace UniRx
{
	internal interface ICustomYieldInstructionErrorHandler
	{
		bool HasError { get; }

		Exception Error { get; }

		bool IsReThrowOnError { get; }

		void ForceDisableRethrowOnError();

		void ForceEnableRethrowOnError();
	}
}
