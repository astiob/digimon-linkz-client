using System;

namespace UnityEngine.Timeline
{
	public interface ITimeControl
	{
		void SetTime(double time);

		void OnControlTimeStart();

		void OnControlTimeStop();
	}
}
