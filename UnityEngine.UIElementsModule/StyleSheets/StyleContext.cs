using System;
using System.Collections.Generic;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
	internal class StyleContext
	{
		private List<RuleMatcher> m_Matchers;

		private VisualElement m_VisualTree;

		private static Dictionary<long, VisualElementStylesData> s_StyleCache = new Dictionary<long, VisualElementStylesData>();

		private static StyleContext.StyleContextHierarchyTraversal s_StyleContextHierarchyTraversal = new StyleContext.StyleContextHierarchyTraversal();

		public StyleContext(VisualElement tree)
		{
			this.m_VisualTree = tree;
			this.m_Matchers = new List<RuleMatcher>(0);
		}

		public float currentPixelsPerPoint { get; set; }

		public void DirtyStyleSheets()
		{
			StyleContext.PropagateDirtyStyleSheets(this.m_VisualTree);
		}

		public void ApplyStyles()
		{
			Debug.Assert(this.m_VisualTree.panel != null);
			StyleContext.s_StyleContextHierarchyTraversal.currentPixelsPerPoint = this.currentPixelsPerPoint;
			StyleContext.s_StyleContextHierarchyTraversal.Traverse(this.m_VisualTree, 0, this.m_Matchers);
			this.m_Matchers.Clear();
		}

		private static void PropagateDirtyStyleSheets(VisualElement element)
		{
			if (element != null)
			{
				if (element.styleSheets != null)
				{
					element.LoadStyleSheetsFromPaths();
				}
				foreach (VisualElement element2 in element.shadow.Children())
				{
					StyleContext.PropagateDirtyStyleSheets(element2);
				}
			}
		}

		public static void ClearStyleCache()
		{
			StyleContext.s_StyleCache.Clear();
		}

		private struct RuleRef
		{
			public StyleComplexSelector selector;

			public StyleSheet sheet;
		}

		private class StyleContextHierarchyTraversal : HierarchyTraversal
		{
			private List<StyleContext.RuleRef> m_MatchedRules = new List<StyleContext.RuleRef>(0);

			private long m_MatchingRulesHash;

			public float currentPixelsPerPoint { get; set; }

			public override bool ShouldSkipElement(VisualElement element)
			{
				return !element.IsDirty(ChangeType.Styles) && !element.IsDirty(ChangeType.StylesPath);
			}

			public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				StyleRule rule = matcher.complexSelector.rule;
				int specificity = matcher.complexSelector.specificity;
				this.m_MatchingRulesHash = (this.m_MatchingRulesHash * 397L ^ (long)rule.GetHashCode());
				this.m_MatchingRulesHash = (this.m_MatchingRulesHash * 397L ^ (long)specificity);
				this.m_MatchedRules.Add(new StyleContext.RuleRef
				{
					selector = matcher.complexSelector,
					sheet = matcher.sheet
				});
				return false;
			}

			public override void OnBeginElementTest(VisualElement element, List<RuleMatcher> ruleMatchers)
			{
				if (element != null && element.styleSheets != null)
				{
					foreach (StyleSheet styleSheet in element.styleSheets)
					{
						StyleComplexSelector[] complexSelectors = styleSheet.complexSelectors;
						int val = ruleMatchers.Count + complexSelectors.Length;
						ruleMatchers.Capacity = Math.Max(ruleMatchers.Capacity, val);
						foreach (StyleComplexSelector complexSelector in complexSelectors)
						{
							ruleMatchers.Add(new RuleMatcher
							{
								sheet = styleSheet,
								complexSelector = complexSelector,
								simpleSelectorIndex = 0,
								depth = int.MaxValue
							});
						}
					}
				}
				this.m_MatchedRules.Clear();
				string fullTypeName = element.fullTypeName;
				long num = (long)fullTypeName.GetHashCode();
				this.m_MatchingRulesHash = (num * 397L ^ (long)this.currentPixelsPerPoint.GetHashCode());
			}

			public override void ProcessMatchedRules(VisualElement element)
			{
				VisualElementStylesData visualElementStylesData;
				if (StyleContext.s_StyleCache.TryGetValue(this.m_MatchingRulesHash, out visualElementStylesData))
				{
					element.SetSharedStyles(visualElementStylesData);
				}
				else
				{
					visualElementStylesData = new VisualElementStylesData(true);
					int i = 0;
					int count = this.m_MatchedRules.Count;
					while (i < count)
					{
						StyleContext.RuleRef ruleRef = this.m_MatchedRules[i];
						StylePropertyID[] propertyIDs = StyleSheetCache.GetPropertyIDs(ruleRef.sheet, ruleRef.selector.ruleIndex);
						visualElementStylesData.ApplyRule(ruleRef.sheet, ruleRef.selector.specificity, ruleRef.selector.rule, propertyIDs);
						i++;
					}
					StyleContext.s_StyleCache[this.m_MatchingRulesHash] = visualElementStylesData;
					element.SetSharedStyles(visualElementStylesData);
				}
			}
		}
	}
}
