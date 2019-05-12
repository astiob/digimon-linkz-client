using System;
using System.Collections.Generic;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
	internal abstract class HierarchyTraversal
	{
		public abstract bool ShouldSkipElement(VisualElement element);

		public abstract bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element);

		public virtual void OnBeginElementTest(VisualElement element, List<RuleMatcher> ruleMatchers)
		{
		}

		public void BeginElementTest(VisualElement element, List<RuleMatcher> ruleMatchers)
		{
			this.OnBeginElementTest(element, ruleMatchers);
		}

		public virtual void ProcessMatchedRules(VisualElement element)
		{
		}

		internal void Traverse(VisualElement element, int depth, List<RuleMatcher> ruleMatchers)
		{
			if (!this.ShouldSkipElement(element))
			{
				int count = ruleMatchers.Count;
				this.BeginElementTest(element, ruleMatchers);
				int count2 = ruleMatchers.Count;
				for (int i = 0; i < count2; i++)
				{
					RuleMatcher matcher = ruleMatchers[i];
					if (matcher.depth >= depth && this.Match(element, ref matcher))
					{
						StyleSelector[] selectors = matcher.complexSelector.selectors;
						int num = matcher.simpleSelectorIndex + 1;
						int num2 = selectors.Length;
						if (num < num2)
						{
							RuleMatcher item = new RuleMatcher
							{
								complexSelector = matcher.complexSelector,
								depth = ((selectors[num].previousRelationship != StyleSelectorRelationship.Child) ? int.MaxValue : (depth + 1)),
								simpleSelectorIndex = num,
								sheet = matcher.sheet
							};
							ruleMatchers.Add(item);
						}
						else if (this.OnRuleMatchedElement(matcher, element))
						{
							return;
						}
					}
				}
				this.ProcessMatchedRules(element);
				this.Recurse(element, depth, ruleMatchers);
				if (ruleMatchers.Count > count)
				{
					ruleMatchers.RemoveRange(count, ruleMatchers.Count - count);
				}
			}
		}

		protected virtual void Recurse(VisualElement element, int depth, List<RuleMatcher> ruleMatchers)
		{
			for (int i = 0; i < element.shadow.childCount; i++)
			{
				VisualElement element2 = element.shadow[i];
				this.Traverse(element2, depth + 1, ruleMatchers);
			}
		}

		protected virtual bool MatchSelectorPart(VisualElement element, StyleSelector selector, StyleSelectorPart part)
		{
			bool flag = true;
			switch (part.type)
			{
			case StyleSelectorType.Wildcard:
				return flag;
			case StyleSelectorType.Type:
				return element.typeName == part.value;
			case StyleSelectorType.Class:
				return element.ClassListContains(part.value);
			case StyleSelectorType.PseudoClass:
			{
				int pseudoStates = (int)element.pseudoStates;
				flag = ((selector.pseudoStateMask & pseudoStates) == selector.pseudoStateMask);
				return flag & (selector.negatedPseudoStateMask & ~pseudoStates) == selector.negatedPseudoStateMask;
			}
			case StyleSelectorType.ID:
				return element.name == part.value;
			}
			flag = false;
			return flag;
		}

		public virtual bool Match(VisualElement element, ref RuleMatcher matcher)
		{
			bool flag = true;
			StyleSelector styleSelector = matcher.complexSelector.selectors[matcher.simpleSelectorIndex];
			int num = styleSelector.parts.Length;
			int num2 = 0;
			while (num2 < num && flag)
			{
				flag = this.MatchSelectorPart(element, styleSelector, styleSelector.parts[num2]);
				num2++;
			}
			return flag;
		}
	}
}
