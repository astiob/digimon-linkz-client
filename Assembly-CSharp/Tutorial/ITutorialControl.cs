using System;
using System.Collections.Generic;

namespace Tutorial
{
	public interface ITutorialControl
	{
		void SetTutorialParameter(Dictionary<string, Action> passTutorialAction);
	}
}
