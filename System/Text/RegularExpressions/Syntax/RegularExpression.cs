using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class RegularExpression : Group
	{
		private int group_count;

		public RegularExpression()
		{
			this.group_count = 0;
		}

		public int GroupCount
		{
			get
			{
				return this.group_count;
			}
			set
			{
				this.group_count = value;
			}
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			int min;
			int max;
			this.GetWidth(out min, out max);
			cmp.EmitInfo(this.group_count, min, max);
			AnchorInfo anchorInfo = this.GetAnchorInfo(reverse);
			LinkRef linkRef = cmp.NewLink();
			cmp.EmitAnchor(reverse, anchorInfo.Offset, linkRef);
			if (anchorInfo.IsPosition)
			{
				cmp.EmitPosition(anchorInfo.Position);
			}
			else if (anchorInfo.IsSubstring)
			{
				cmp.EmitString(anchorInfo.Substring, anchorInfo.IgnoreCase, reverse);
			}
			cmp.EmitTrue();
			cmp.ResolveLink(linkRef);
			base.Compile(cmp, reverse);
			cmp.EmitTrue();
		}
	}
}
