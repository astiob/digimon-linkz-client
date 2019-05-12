using System;

namespace Firebase.Platform
{
	internal interface ITokenChangeListener
	{
		void OnTokenChange(string token);

		void OnTokenChange();
	}
}
