using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	[Serializable]
	internal class VisualElementAsset : IUxmlAttributes
	{
		[SerializeField]
		private string m_Name;

		[SerializeField]
		private int m_Id;

		[SerializeField]
		private int m_ParentId;

		[SerializeField]
		private int m_RuleIndex;

		[SerializeField]
		private string m_Text;

		[SerializeField]
		private PickingMode m_PickingMode;

		[SerializeField]
		private string m_FullTypeName;

		[SerializeField]
		private string[] m_Classes;

		[SerializeField]
		private List<string> m_Stylesheets;

		[SerializeField]
		private List<string> m_Properties;

		public VisualElementAsset(string fullTypeName)
		{
			this.m_FullTypeName = fullTypeName;
		}

		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}

		public int id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
			}
		}

		public int parentId
		{
			get
			{
				return this.m_ParentId;
			}
			set
			{
				this.m_ParentId = value;
			}
		}

		public int ruleIndex
		{
			get
			{
				return this.m_RuleIndex;
			}
			set
			{
				this.m_RuleIndex = value;
			}
		}

		public string text
		{
			get
			{
				return this.m_Text;
			}
			set
			{
				this.m_Text = value;
			}
		}

		public PickingMode pickingMode
		{
			get
			{
				return this.m_PickingMode;
			}
			set
			{
				this.m_PickingMode = value;
			}
		}

		public string fullTypeName
		{
			get
			{
				return this.m_FullTypeName;
			}
			set
			{
				this.m_FullTypeName = value;
			}
		}

		public string[] classes
		{
			get
			{
				return this.m_Classes;
			}
			set
			{
				this.m_Classes = value;
			}
		}

		public List<string> stylesheets
		{
			get
			{
				return (this.m_Stylesheets != null) ? this.m_Stylesheets : (this.m_Stylesheets = new List<string>());
			}
			set
			{
				this.m_Stylesheets = value;
			}
		}

		public VisualElement Create(CreationContext ctx)
		{
			Func<IUxmlAttributes, CreationContext, VisualElement> func;
			VisualElement result;
			if (!Factories.TryGetValue(this.fullTypeName, out func))
			{
				Debug.LogErrorFormat("Visual Element Type '{0}' has no factory method.", new object[]
				{
					this.fullTypeName
				});
				result = new Label(string.Format("Unknown type: '{0}'", this.fullTypeName));
			}
			else if (func == null)
			{
				Debug.LogErrorFormat("Visual Element Type '{0}' has a null factory method.", new object[]
				{
					this.fullTypeName
				});
				result = new Label(string.Format("Type with no factory method: '{0}'", this.fullTypeName));
			}
			else
			{
				VisualElement visualElement = func(this, ctx);
				if (visualElement == null)
				{
					Debug.LogErrorFormat("The factory of Visual Element Type '{0}' has returned a null object", new object[]
					{
						this.fullTypeName
					});
					result = new Label(string.Format("The factory of Visual Element Type '{0}' has returned a null object", this.fullTypeName));
				}
				else
				{
					visualElement.name = this.name;
					if (this.classes != null)
					{
						for (int i = 0; i < this.classes.Length; i++)
						{
							visualElement.AddToClassList(this.classes[i]);
						}
					}
					if (this.stylesheets != null)
					{
						for (int j = 0; j < this.stylesheets.Count; j++)
						{
							visualElement.AddStyleSheetPath(this.stylesheets[j]);
						}
					}
					if (!string.IsNullOrEmpty(this.text))
					{
						visualElement.text = this.text;
					}
					visualElement.pickingMode = this.pickingMode;
					result = visualElement;
				}
			}
			return result;
		}

		public void AddProperty(string propertyName, string propertyValue)
		{
			if (this.m_Properties == null)
			{
				this.m_Properties = new List<string>();
			}
			this.m_Properties.Add(propertyName);
			this.m_Properties.Add(propertyValue);
		}

		public string GetPropertyString(string propertyName)
		{
			string result;
			if (this.m_Properties == null)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < this.m_Properties.Count - 1; i += 2)
				{
					if (this.m_Properties[i] == propertyName)
					{
						return this.m_Properties[i + 1];
					}
				}
				result = null;
			}
			return result;
		}

		public int GetPropertyInt(string propertyName, int defaultValue)
		{
			string propertyString = this.GetPropertyString(propertyName);
			int num;
			int result;
			if (propertyString == null || !int.TryParse(propertyString, out num))
			{
				result = defaultValue;
			}
			else
			{
				result = num;
			}
			return result;
		}

		public bool GetPropertyBool(string propertyName, bool defaultValue)
		{
			string propertyString = this.GetPropertyString(propertyName);
			bool flag;
			bool result;
			if (propertyString == null || !bool.TryParse(propertyString, out flag))
			{
				result = defaultValue;
			}
			else
			{
				result = flag;
			}
			return result;
		}

		public Color GetPropertyColor(string propertyName, Color defaultValue)
		{
			string propertyString = this.GetPropertyString(propertyName);
			Color32 c;
			Color result;
			if (propertyString == null || !ColorUtility.DoTryParseHtmlColor(propertyString, out c))
			{
				result = defaultValue;
			}
			else
			{
				result = c;
			}
			return result;
		}

		public long GetPropertyLong(string propertyName, long defaultValue)
		{
			string propertyString = this.GetPropertyString(propertyName);
			long num;
			long result;
			if (propertyString == null || !long.TryParse(propertyString, out num))
			{
				result = defaultValue;
			}
			else
			{
				result = num;
			}
			return result;
		}

		public float GetPropertyFloat(string propertyName, float def)
		{
			string propertyString = this.GetPropertyString(propertyName);
			float num;
			float result;
			if (propertyString == null || !float.TryParse(propertyString, out num))
			{
				result = def;
			}
			else
			{
				result = num;
			}
			return result;
		}

		public T GetPropertyEnum<T>(string propertyName, T def)
		{
			string propertyString = this.GetPropertyString(propertyName);
			T result;
			if (propertyString == null || !Enum.IsDefined(typeof(T), propertyString))
			{
				result = def;
			}
			else
			{
				T t = (T)((object)Enum.Parse(typeof(T), propertyString));
				result = t;
			}
			return result;
		}
	}
}
