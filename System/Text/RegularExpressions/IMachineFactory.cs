using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	internal interface IMachineFactory
	{
		IMachine NewInstance();

		IDictionary Mapping { get; set; }

		int GroupCount { get; }

		int Gap { get; set; }

		string[] NamesMapping { get; set; }
	}
}
