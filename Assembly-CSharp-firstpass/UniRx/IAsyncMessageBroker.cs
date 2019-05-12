using System;

namespace UniRx
{
	public interface IAsyncMessageBroker : IAsyncMessagePublisher, IAsyncMessageReceiver
	{
	}
}
