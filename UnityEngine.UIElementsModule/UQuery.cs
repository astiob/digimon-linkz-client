using System;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
	public static class UQuery
	{
		private static UQuery.FirstQueryMatcher s_First = new UQuery.FirstQueryMatcher();

		private static UQuery.LastQueryMatcher s_Last = new UQuery.LastQueryMatcher();

		private static UQuery.IndexQueryMatcher s_Index = new UQuery.IndexQueryMatcher();

		internal interface IVisualPredicateWrapper
		{
			bool Predicate(object e);
		}

		internal class IsOfType<T> : UQuery.IVisualPredicateWrapper where T : VisualElement
		{
			public static UQuery.IsOfType<T> s_Instance = new UQuery.IsOfType<T>();

			public bool Predicate(object e)
			{
				return e is T;
			}
		}

		internal class PredicateWrapper<T> : UQuery.IVisualPredicateWrapper where T : VisualElement
		{
			private Func<T, bool> predicate;

			public PredicateWrapper(Func<T, bool> p)
			{
				this.predicate = p;
			}

			public bool Predicate(object e)
			{
				T t = e as T;
				return t != null && this.predicate(t);
			}
		}

		private abstract class UQueryMatcher : HierarchyTraversal
		{
			public override bool ShouldSkipElement(VisualElement element)
			{
				return false;
			}

			protected override bool MatchSelectorPart(VisualElement element, StyleSelector selector, StyleSelectorPart part)
			{
				bool result;
				if (part.type == StyleSelectorType.Predicate)
				{
					UQuery.IVisualPredicateWrapper visualPredicateWrapper = part.tempData as UQuery.IVisualPredicateWrapper;
					result = (visualPredicateWrapper != null && visualPredicateWrapper.Predicate(element));
				}
				else
				{
					result = base.MatchSelectorPart(element, selector, part);
				}
				return result;
			}

			public virtual void Run(VisualElement root, List<RuleMatcher> ruleMatchers)
			{
				base.Traverse(root, 0, ruleMatchers);
			}
		}

		private abstract class SingleQueryMatcher : UQuery.UQueryMatcher
		{
			public VisualElement match { get; set; }

			public override void Run(VisualElement root, List<RuleMatcher> ruleMatchers)
			{
				this.match = null;
				base.Run(root, ruleMatchers);
			}
		}

		private class FirstQueryMatcher : UQuery.SingleQueryMatcher
		{
			public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				if (base.match == null)
				{
					base.match = element;
				}
				return true;
			}
		}

		private class LastQueryMatcher : UQuery.SingleQueryMatcher
		{
			public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				base.match = element;
				return false;
			}
		}

		private class IndexQueryMatcher : UQuery.SingleQueryMatcher
		{
			private int matchCount = -1;

			private int _matchIndex;

			public int matchIndex
			{
				get
				{
					return this._matchIndex;
				}
				set
				{
					this.matchCount = -1;
					this._matchIndex = value;
				}
			}

			public override void Run(VisualElement root, List<RuleMatcher> ruleMatchers)
			{
				this.matchCount = -1;
				base.Run(root, ruleMatchers);
			}

			public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				this.matchCount++;
				if (this.matchCount == this._matchIndex)
				{
					base.match = element;
				}
				return this.matchCount >= this._matchIndex;
			}
		}

		public struct QueryBuilder<T> where T : VisualElement
		{
			private List<StyleSelector> m_StyleSelectors;

			private List<StyleSelectorPart> m_Parts;

			private VisualElement m_Element;

			private List<RuleMatcher> m_Matchers;

			private StyleSelectorRelationship m_Relationship;

			private int pseudoStatesMask;

			private int negatedPseudoStatesMask;

			public QueryBuilder(VisualElement visualElement)
			{
				this = default(UQuery.QueryBuilder<T>);
				this.m_Element = visualElement;
				this.m_Parts = null;
				this.m_StyleSelectors = null;
				this.m_Relationship = StyleSelectorRelationship.None;
				this.m_Matchers = new List<RuleMatcher>();
				this.pseudoStatesMask = (this.negatedPseudoStatesMask = 0);
			}

			private List<StyleSelector> styleSelectors
			{
				get
				{
					List<StyleSelector> result;
					if ((result = this.m_StyleSelectors) == null)
					{
						result = (this.m_StyleSelectors = new List<StyleSelector>());
					}
					return result;
				}
			}

			private List<StyleSelectorPart> parts
			{
				get
				{
					List<StyleSelectorPart> result;
					if ((result = this.m_Parts) == null)
					{
						result = (this.m_Parts = new List<StyleSelectorPart>());
					}
					return result;
				}
			}

			public UQuery.QueryBuilder<T> Class(string classname)
			{
				this.AddClass(classname);
				return this;
			}

			public UQuery.QueryBuilder<T> Name(string id)
			{
				this.AddName(id);
				return this;
			}

			public UQuery.QueryBuilder<T2> Descendents<T2>(string name = null, params string[] classNames) where T2 : VisualElement
			{
				this.FinishCurrentSelector();
				this.AddType<T2>();
				this.AddName(name);
				this.AddClasses(classNames);
				return this.AddRelationship<T2>(StyleSelectorRelationship.Descendent);
			}

			public UQuery.QueryBuilder<T2> Descendents<T2>(string name = null, string classname = null) where T2 : VisualElement
			{
				this.FinishCurrentSelector();
				this.AddType<T2>();
				this.AddName(name);
				this.AddClass(classname);
				return this.AddRelationship<T2>(StyleSelectorRelationship.Descendent);
			}

			public UQuery.QueryBuilder<T2> Children<T2>(string name = null, params string[] classes) where T2 : VisualElement
			{
				this.FinishCurrentSelector();
				this.AddType<T2>();
				this.AddName(name);
				this.AddClasses(classes);
				return this.AddRelationship<T2>(StyleSelectorRelationship.Child);
			}

			public UQuery.QueryBuilder<T2> Children<T2>(string name = null, string className = null) where T2 : VisualElement
			{
				this.FinishCurrentSelector();
				this.AddType<T2>();
				this.AddName(name);
				this.AddClass(className);
				return this.AddRelationship<T2>(StyleSelectorRelationship.Child);
			}

			public UQuery.QueryBuilder<T2> OfType<T2>(string name = null, params string[] classes) where T2 : VisualElement
			{
				this.AddType<T2>();
				this.AddName(name);
				this.AddClasses(classes);
				return this.AddRelationship<T2>(StyleSelectorRelationship.None);
			}

			public UQuery.QueryBuilder<T2> OfType<T2>(string name = null, string className = null) where T2 : VisualElement
			{
				this.AddType<T2>();
				this.AddName(name);
				this.AddClass(className);
				return this.AddRelationship<T2>(StyleSelectorRelationship.None);
			}

			public UQuery.QueryBuilder<T> Where(Func<T, bool> selectorPredicate)
			{
				this.parts.Add(StyleSelectorPart.CreatePredicate(new UQuery.PredicateWrapper<T>(selectorPredicate)));
				return this;
			}

			private void AddClass(string c)
			{
				if (c != null)
				{
					this.parts.Add(StyleSelectorPart.CreateClass(c));
				}
			}

			private void AddClasses(params string[] classes)
			{
				if (classes != null)
				{
					for (int i = 0; i < classes.Length; i++)
					{
						this.AddClass(classes[i]);
					}
				}
			}

			private void AddName(string id)
			{
				if (id != null)
				{
					this.parts.Add(StyleSelectorPart.CreateId(id));
				}
			}

			private void AddType<T2>() where T2 : VisualElement
			{
				if (typeof(T2) != typeof(VisualElement))
				{
					this.parts.Add(StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T2>.s_Instance));
				}
			}

			private UQuery.QueryBuilder<T> AddPseudoState(PseudoStates s)
			{
				this.pseudoStatesMask |= (int)s;
				return this;
			}

			private UQuery.QueryBuilder<T> AddNegativePseudoState(PseudoStates s)
			{
				this.negatedPseudoStatesMask |= (int)s;
				return this;
			}

			public UQuery.QueryBuilder<T> Active()
			{
				return this.AddPseudoState(PseudoStates.Active);
			}

			public UQuery.QueryBuilder<T> NotActive()
			{
				return this.AddNegativePseudoState(PseudoStates.Active);
			}

			public UQuery.QueryBuilder<T> Visible()
			{
				return this.AddNegativePseudoState(PseudoStates.Invisible);
			}

			public UQuery.QueryBuilder<T> NotVisible()
			{
				return this.AddPseudoState(PseudoStates.Invisible);
			}

			public UQuery.QueryBuilder<T> Hovered()
			{
				return this.AddPseudoState(PseudoStates.Hover);
			}

			public UQuery.QueryBuilder<T> NotHovered()
			{
				return this.AddNegativePseudoState(PseudoStates.Hover);
			}

			public UQuery.QueryBuilder<T> Checked()
			{
				return this.AddPseudoState(PseudoStates.Checked);
			}

			public UQuery.QueryBuilder<T> NotChecked()
			{
				return this.AddNegativePseudoState(PseudoStates.Checked);
			}

			public UQuery.QueryBuilder<T> Selected()
			{
				return this.AddPseudoState(PseudoStates.Selected);
			}

			public UQuery.QueryBuilder<T> NotSelected()
			{
				return this.AddNegativePseudoState(PseudoStates.Selected);
			}

			public UQuery.QueryBuilder<T> Enabled()
			{
				return this.AddNegativePseudoState(PseudoStates.Disabled);
			}

			public UQuery.QueryBuilder<T> NotEnabled()
			{
				return this.AddPseudoState(PseudoStates.Disabled);
			}

			public UQuery.QueryBuilder<T> Focused()
			{
				return this.AddPseudoState(PseudoStates.Focus);
			}

			public UQuery.QueryBuilder<T> NotFocused()
			{
				return this.AddNegativePseudoState(PseudoStates.Focus);
			}

			private UQuery.QueryBuilder<T2> AddRelationship<T2>(StyleSelectorRelationship relationship) where T2 : VisualElement
			{
				return new UQuery.QueryBuilder<T2>(this.m_Element)
				{
					m_Matchers = this.m_Matchers,
					m_Parts = this.m_Parts,
					m_StyleSelectors = this.m_StyleSelectors,
					m_Relationship = relationship,
					pseudoStatesMask = this.pseudoStatesMask,
					negatedPseudoStatesMask = this.negatedPseudoStatesMask
				};
			}

			private void AddPseudoStatesRuleIfNecessasy()
			{
				if (this.pseudoStatesMask != 0 || this.negatedPseudoStatesMask != 0)
				{
					this.parts.Add(new StyleSelectorPart
					{
						type = StyleSelectorType.PseudoClass
					});
				}
			}

			private void FinishSelector()
			{
				this.FinishCurrentSelector();
				if (this.styleSelectors.Count > 0)
				{
					StyleComplexSelector styleComplexSelector = new StyleComplexSelector();
					styleComplexSelector.selectors = this.styleSelectors.ToArray();
					this.styleSelectors.Clear();
					this.m_Matchers.Add(new RuleMatcher
					{
						complexSelector = styleComplexSelector,
						simpleSelectorIndex = 0,
						depth = int.MaxValue
					});
				}
			}

			private bool CurrentSelectorEmpty()
			{
				return this.parts.Count == 0 && this.m_Relationship == StyleSelectorRelationship.None && this.pseudoStatesMask == 0 && this.negatedPseudoStatesMask == 0;
			}

			private void FinishCurrentSelector()
			{
				if (!this.CurrentSelectorEmpty())
				{
					StyleSelector styleSelector = new StyleSelector();
					styleSelector.previousRelationship = this.m_Relationship;
					this.AddPseudoStatesRuleIfNecessasy();
					styleSelector.parts = this.m_Parts.ToArray();
					styleSelector.pseudoStateMask = this.pseudoStatesMask;
					styleSelector.negatedPseudoStateMask = this.negatedPseudoStatesMask;
					this.styleSelectors.Add(styleSelector);
					this.m_Parts.Clear();
					this.pseudoStatesMask = (this.negatedPseudoStatesMask = 0);
				}
			}

			public UQuery.QueryState<T> Build()
			{
				this.FinishSelector();
				return new UQuery.QueryState<T>(this.m_Element, this.m_Matchers);
			}

			public static implicit operator T(UQuery.QueryBuilder<T> s)
			{
				return s.First();
			}

			public T First()
			{
				return this.Build().First();
			}

			public T Last()
			{
				return this.Build().Last();
			}

			public List<T> ToList()
			{
				return this.Build().ToList();
			}

			public void ToList(List<T> results)
			{
				this.Build().ToList(results);
			}

			public T AtIndex(int index)
			{
				return this.Build().AtIndex(index);
			}

			public void ForEach<T2>(List<T2> result, Func<T, T2> funcCall)
			{
				this.Build().ForEach<T2>(result, funcCall);
			}

			public List<T2> ForEach<T2>(Func<T, T2> funcCall)
			{
				return this.Build().ForEach<T2>(funcCall);
			}

			public void ForEach(Action<T> funcCall)
			{
				this.Build().ForEach(funcCall);
			}
		}

		public struct QueryState<T> where T : VisualElement
		{
			private readonly VisualElement m_Element;

			private readonly List<RuleMatcher> m_Matchers;

			private static readonly UQuery.QueryState<T>.ListQueryMatcher s_List = new UQuery.QueryState<T>.ListQueryMatcher();

			private static UQuery.QueryState<T>.ActionQueryMatcher s_Action = new UQuery.QueryState<T>.ActionQueryMatcher();

			internal QueryState(VisualElement element, List<RuleMatcher> matchers)
			{
				this.m_Element = element;
				this.m_Matchers = matchers;
			}

			public UQuery.QueryState<T> RebuildOn(VisualElement element)
			{
				return new UQuery.QueryState<T>(element, this.m_Matchers);
			}

			public T First()
			{
				UQuery.s_First.Run(this.m_Element, this.m_Matchers);
				T result = UQuery.s_First.match as T;
				UQuery.s_First.match = null;
				return result;
			}

			public T Last()
			{
				UQuery.s_Last.Run(this.m_Element, this.m_Matchers);
				T result = UQuery.s_Last.match as T;
				UQuery.s_Last.match = null;
				return result;
			}

			public void ToList(List<T> results)
			{
				UQuery.QueryState<T>.s_List.matches = results;
				UQuery.QueryState<T>.s_List.Run(this.m_Element, this.m_Matchers);
				UQuery.QueryState<T>.s_List.Reset();
			}

			public List<T> ToList()
			{
				List<T> list = new List<T>();
				this.ToList(list);
				return list;
			}

			public T AtIndex(int index)
			{
				UQuery.s_Index.matchIndex = index;
				UQuery.s_Index.Run(this.m_Element, this.m_Matchers);
				T result = UQuery.s_Index.match as T;
				UQuery.s_Index.match = null;
				return result;
			}

			public void ForEach(Action<T> funcCall)
			{
				UQuery.QueryState<T>.s_Action.callBack = funcCall;
				UQuery.QueryState<T>.s_Action.Run(this.m_Element, this.m_Matchers);
				UQuery.QueryState<T>.s_Action.callBack = null;
			}

			public void ForEach<T2>(List<T2> result, Func<T, T2> funcCall)
			{
				UQuery.QueryState<T>.DelegateQueryMatcher<T2> s_Instance = UQuery.QueryState<T>.DelegateQueryMatcher<T2>.s_Instance;
				s_Instance.callBack = funcCall;
				s_Instance.result = result;
				s_Instance.Run(this.m_Element, this.m_Matchers);
				s_Instance.callBack = null;
				s_Instance.result = null;
			}

			public List<T2> ForEach<T2>(Func<T, T2> funcCall)
			{
				List<T2> result = new List<T2>();
				this.ForEach<T2>(result, funcCall);
				return result;
			}

			private class ListQueryMatcher : UQuery.UQueryMatcher
			{
				public List<T> matches { get; set; }

				public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
				{
					this.matches.Add(element as T);
					return false;
				}

				public void Reset()
				{
					this.matches = null;
				}
			}

			private class ActionQueryMatcher : UQuery.UQueryMatcher
			{
				internal Action<T> callBack { get; set; }

				public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
				{
					T t = element as T;
					if (t != null)
					{
						this.callBack(t);
					}
					return false;
				}
			}

			private class DelegateQueryMatcher<TReturnType> : UQuery.UQueryMatcher
			{
				public static UQuery.QueryState<T>.DelegateQueryMatcher<TReturnType> s_Instance = new UQuery.QueryState<T>.DelegateQueryMatcher<TReturnType>();

				public Func<T, TReturnType> callBack { get; set; }

				public List<TReturnType> result { get; set; }

				public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
				{
					T t = element as T;
					if (t != null)
					{
						this.result.Add(this.callBack(t));
					}
					return false;
				}
			}
		}
	}
}
