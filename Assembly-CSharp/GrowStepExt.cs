using Master;
using System;

internal static class GrowStepExt
{
	public static int ConverBattleInt(this GrowStep growStep)
	{
		return growStep - GrowStep.CHILD_1;
	}
}
