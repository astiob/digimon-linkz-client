using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Text.RegularExpressions
{
	internal abstract class BaseMachine : IMachine
	{
		protected bool needs_groups_or_captures = true;

		public virtual string Replace(Regex regex, string input, string replacement, int count, int startat)
		{
			ReplacementEvaluator replacementEvaluator = new ReplacementEvaluator(regex, replacement);
			if (regex.RightToLeft)
			{
				return this.RTLReplace(regex, input, new MatchEvaluator(replacementEvaluator.Evaluate), count, startat);
			}
			return this.LTRReplace(regex, input, new BaseMachine.MatchAppendEvaluator(replacementEvaluator.EvaluateAppend), count, startat, replacementEvaluator.NeedsGroupsOrCaptures);
		}

		public virtual string[] Split(Regex regex, string input, int count, int startat)
		{
			ArrayList arrayList = new ArrayList();
			if (count == 0)
			{
				count = int.MaxValue;
			}
			int num = startat;
			Match match = null;
			while (--count > 0)
			{
				if (match != null)
				{
					match = match.NextMatch();
				}
				else
				{
					match = regex.Match(input, num);
				}
				if (!match.Success)
				{
					break;
				}
				if (regex.RightToLeft)
				{
					arrayList.Add(input.Substring(match.Index + match.Length, num - match.Index - match.Length));
				}
				else
				{
					arrayList.Add(input.Substring(num, match.Index - num));
				}
				int count2 = match.Groups.Count;
				for (int i = 1; i < count2; i++)
				{
					Group group = match.Groups[i];
					arrayList.Add(input.Substring(group.Index, group.Length));
				}
				if (regex.RightToLeft)
				{
					num = match.Index;
				}
				else
				{
					num = match.Index + match.Length;
				}
			}
			if (regex.RightToLeft && num >= 0)
			{
				arrayList.Add(input.Substring(0, num));
			}
			if (!regex.RightToLeft && num <= input.Length)
			{
				arrayList.Add(input.Substring(num));
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		public virtual Match Scan(Regex regex, string text, int start, int end)
		{
			throw new NotImplementedException("Scan method must be implemented in derived classes");
		}

		public virtual string Result(string replacement, Match match)
		{
			return ReplacementEvaluator.Evaluate(replacement, match);
		}

		internal string LTRReplace(Regex regex, string input, BaseMachine.MatchAppendEvaluator evaluator, int count, int startat)
		{
			return this.LTRReplace(regex, input, evaluator, count, startat, true);
		}

		internal string LTRReplace(Regex regex, string input, BaseMachine.MatchAppendEvaluator evaluator, int count, int startat, bool needs_groups_or_captures)
		{
			this.needs_groups_or_captures = needs_groups_or_captures;
			Match match = this.Scan(regex, input, startat, input.Length);
			if (!match.Success)
			{
				return input;
			}
			StringBuilder stringBuilder = new StringBuilder(input.Length);
			int num = startat;
			int num2 = count;
			stringBuilder.Append(input, 0, num);
			while (count == -1 || num2-- > 0)
			{
				if (match.Index < num)
				{
					throw new SystemException("how");
				}
				stringBuilder.Append(input, num, match.Index - num);
				evaluator(match, stringBuilder);
				num = match.Index + match.Length;
				match = match.NextMatch();
				if (!match.Success)
				{
					IL_AA:
					stringBuilder.Append(input, num, input.Length - num);
					return stringBuilder.ToString();
				}
			}
			goto IL_AA;
		}

		internal string RTLReplace(Regex regex, string input, MatchEvaluator evaluator, int count, int startat)
		{
			Match match = this.Scan(regex, input, startat, input.Length);
			if (!match.Success)
			{
				return input;
			}
			int num = startat;
			int num2 = count;
			List<string> list = new List<string>();
			list.Add(input.Substring(num));
			while (count == -1 || num2-- > 0)
			{
				if (match.Index + match.Length > num)
				{
					throw new SystemException("how");
				}
				list.Add(input.Substring(match.Index + match.Length, num - match.Index - match.Length));
				list.Add(evaluator(match));
				num = match.Index;
				match = match.NextMatch();
				if (!match.Success)
				{
					IL_BB:
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(input, 0, num);
					int i = list.Count;
					while (i > 0)
					{
						stringBuilder.Append(list[--i]);
					}
					list.Clear();
					return stringBuilder.ToString();
				}
			}
			goto IL_BB;
		}

		internal delegate void MatchAppendEvaluator(Match match, StringBuilder sb);
	}
}
