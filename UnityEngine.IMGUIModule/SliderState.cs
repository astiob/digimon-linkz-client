using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	internal class SliderState
	{
		public float dragStartPos;

		public float dragStartValue;

		public bool isDragging;

		[RequiredByNativeCode]
		public SliderState()
		{
		}
	}
}
