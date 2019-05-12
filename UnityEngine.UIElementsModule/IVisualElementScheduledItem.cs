using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IVisualElementScheduledItem
	{
		VisualElement element { get; }

		bool isActive { get; }

		void Resume();

		void Pause();

		void ExecuteLater(long delayMs);

		IVisualElementScheduledItem StartingIn(long delayMs);

		IVisualElementScheduledItem Every(long intervalMs);

		IVisualElementScheduledItem Until(Func<bool> stopCondition);

		IVisualElementScheduledItem ForDuration(long durationMs);
	}
}
