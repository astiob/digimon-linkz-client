using System;
using System.Collections;

namespace Cutscene.Gasha
{
	public interface IGashaAnimation
	{
		IEnumerator StartAnimation();

		void SkipAnimation();
	}
}
