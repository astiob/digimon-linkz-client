using System;

namespace UnityEngine.StyleSheets
{
	[Serializable]
	internal struct StyleSelectorPart
	{
		[SerializeField]
		private string m_Value;

		[SerializeField]
		private StyleSelectorType m_Type;

		internal object tempData;

		public string value
		{
			get
			{
				return this.m_Value;
			}
			internal set
			{
				this.m_Value = value;
			}
		}

		public StyleSelectorType type
		{
			get
			{
				return this.m_Type;
			}
			internal set
			{
				this.m_Type = value;
			}
		}

		public override string ToString()
		{
			return string.Format("[StyleSelectorPart: value={0}, type={1}]", this.value, this.type);
		}

		public static StyleSelectorPart CreateClass(string className)
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.Class,
				m_Value = className
			};
		}

		public static StyleSelectorPart CreateId(string Id)
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.ID,
				m_Value = Id
			};
		}

		public static StyleSelectorPart CreateType(Type t)
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.Type,
				m_Value = t.Name
			};
		}

		public static StyleSelectorPart CreatePredicate(object predicate)
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.Predicate,
				tempData = predicate
			};
		}

		public static StyleSelectorPart CreateWildCard()
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.Wildcard
			};
		}
	}
}
