using System;

namespace UniRx
{
	public interface IMessageBroker : IMessagePublisher, IMessageReceiver
	{
	}
}
