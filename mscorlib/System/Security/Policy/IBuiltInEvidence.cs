using System;

namespace System.Security.Policy
{
	internal interface IBuiltInEvidence
	{
		int GetRequiredSize(bool verbose);

		int InitFromBuffer(char[] buffer, int position);

		int OutputToBuffer(char[] buffer, int position, bool verbose);
	}
}
