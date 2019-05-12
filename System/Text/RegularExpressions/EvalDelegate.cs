using System;

namespace System.Text.RegularExpressions
{
	internal delegate bool EvalDelegate(RxInterpreter interp, int strpos, ref int strpos_result);
}
