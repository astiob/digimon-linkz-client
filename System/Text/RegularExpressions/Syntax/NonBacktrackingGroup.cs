using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class NonBacktrackingGroup : Group
	{
		public override void Compile(ICompiler cmp, bool reverse)
		{
			LinkRef linkRef = cmp.NewLink();
			cmp.EmitSub(linkRef);
			base.Compile(cmp, reverse);
			cmp.EmitTrue();
			cmp.ResolveLink(linkRef);
		}

		public override bool IsComplex()
		{
			return true;
		}
	}
}
