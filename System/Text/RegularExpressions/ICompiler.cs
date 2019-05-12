using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	internal interface ICompiler
	{
		void Reset();

		IMachineFactory GetMachineFactory();

		void EmitFalse();

		void EmitTrue();

		void EmitCharacter(char c, bool negate, bool ignore, bool reverse);

		void EmitCategory(Category cat, bool negate, bool reverse);

		void EmitNotCategory(Category cat, bool negate, bool reverse);

		void EmitRange(char lo, char hi, bool negate, bool ignore, bool reverse);

		void EmitSet(char lo, BitArray set, bool negate, bool ignore, bool reverse);

		void EmitString(string str, bool ignore, bool reverse);

		void EmitPosition(Position pos);

		void EmitOpen(int gid);

		void EmitClose(int gid);

		void EmitBalanceStart(int gid, int balance, bool capture, LinkRef tail);

		void EmitBalance();

		void EmitReference(int gid, bool ignore, bool reverse);

		void EmitIfDefined(int gid, LinkRef tail);

		void EmitSub(LinkRef tail);

		void EmitTest(LinkRef yes, LinkRef tail);

		void EmitBranch(LinkRef next);

		void EmitJump(LinkRef target);

		void EmitRepeat(int min, int max, bool lazy, LinkRef until);

		void EmitUntil(LinkRef repeat);

		void EmitIn(LinkRef tail);

		void EmitInfo(int count, int min, int max);

		void EmitFastRepeat(int min, int max, bool lazy, LinkRef tail);

		void EmitAnchor(bool reverse, int offset, LinkRef tail);

		void EmitBranchEnd();

		void EmitAlternationEnd();

		LinkRef NewLink();

		void ResolveLink(LinkRef link);
	}
}
