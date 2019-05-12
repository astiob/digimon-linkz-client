using System;
using System.Collections.Generic;

namespace UnityEngine
{
	internal class GUILayoutGroup : GUILayoutEntry
	{
		public List<GUILayoutEntry> entries = new List<GUILayoutEntry>();

		public bool isVertical = true;

		public bool resetCoords = false;

		public float spacing = 0f;

		public bool sameSize = true;

		public bool isWindow = false;

		public int windowID = -1;

		private int m_Cursor = 0;

		protected int m_StretchableCountX = 100;

		protected int m_StretchableCountY = 100;

		protected bool m_UserSpecifiedWidth = false;

		protected bool m_UserSpecifiedHeight = false;

		protected float m_ChildMinWidth = 100f;

		protected float m_ChildMaxWidth = 100f;

		protected float m_ChildMinHeight = 100f;

		protected float m_ChildMaxHeight = 100f;

		private readonly RectOffset m_Margin = new RectOffset();

		public GUILayoutGroup() : base(0f, 0f, 0f, 0f, GUIStyle.none)
		{
		}

		public GUILayoutGroup(GUIStyle _style, GUILayoutOption[] options) : base(0f, 0f, 0f, 0f, _style)
		{
			if (options != null)
			{
				this.ApplyOptions(options);
			}
			this.m_Margin.left = _style.margin.left;
			this.m_Margin.right = _style.margin.right;
			this.m_Margin.top = _style.margin.top;
			this.m_Margin.bottom = _style.margin.bottom;
		}

		public override RectOffset margin
		{
			get
			{
				return this.m_Margin;
			}
		}

		public override void ApplyOptions(GUILayoutOption[] options)
		{
			if (options != null)
			{
				base.ApplyOptions(options);
				foreach (GUILayoutOption guilayoutOption in options)
				{
					switch (guilayoutOption.type)
					{
					case GUILayoutOption.Type.fixedWidth:
					case GUILayoutOption.Type.minWidth:
					case GUILayoutOption.Type.maxWidth:
						this.m_UserSpecifiedHeight = true;
						break;
					case GUILayoutOption.Type.fixedHeight:
					case GUILayoutOption.Type.minHeight:
					case GUILayoutOption.Type.maxHeight:
						this.m_UserSpecifiedWidth = true;
						break;
					case GUILayoutOption.Type.spacing:
						this.spacing = (float)((int)guilayoutOption.value);
						break;
					}
				}
			}
		}

		protected override void ApplyStyleSettings(GUIStyle style)
		{
			base.ApplyStyleSettings(style);
			RectOffset margin = style.margin;
			this.m_Margin.left = margin.left;
			this.m_Margin.right = margin.right;
			this.m_Margin.top = margin.top;
			this.m_Margin.bottom = margin.bottom;
		}

		public void ResetCursor()
		{
			this.m_Cursor = 0;
		}

		public Rect PeekNext()
		{
			if (this.m_Cursor < this.entries.Count)
			{
				GUILayoutEntry guilayoutEntry = this.entries[this.m_Cursor];
				return guilayoutEntry.rect;
			}
			throw new ArgumentException(string.Concat(new object[]
			{
				"Getting control ",
				this.m_Cursor,
				"'s position in a group with only ",
				this.entries.Count,
				" controls when doing ",
				Event.current.rawType,
				"\nAborting"
			}));
		}

		public GUILayoutEntry GetNext()
		{
			if (this.m_Cursor < this.entries.Count)
			{
				GUILayoutEntry result = this.entries[this.m_Cursor];
				this.m_Cursor++;
				return result;
			}
			throw new ArgumentException(string.Concat(new object[]
			{
				"Getting control ",
				this.m_Cursor,
				"'s position in a group with only ",
				this.entries.Count,
				" controls when doing ",
				Event.current.rawType,
				"\nAborting"
			}));
		}

		public Rect GetLast()
		{
			Rect result;
			if (this.m_Cursor == 0)
			{
				Debug.LogError("You cannot call GetLast immediately after beginning a group.");
				result = GUILayoutEntry.kDummyRect;
			}
			else if (this.m_Cursor <= this.entries.Count)
			{
				GUILayoutEntry guilayoutEntry = this.entries[this.m_Cursor - 1];
				result = guilayoutEntry.rect;
			}
			else
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Getting control ",
					this.m_Cursor,
					"'s position in a group with only ",
					this.entries.Count,
					" controls when doing ",
					Event.current.type
				}));
				result = GUILayoutEntry.kDummyRect;
			}
			return result;
		}

		public void Add(GUILayoutEntry e)
		{
			this.entries.Add(e);
		}

		public override void CalcWidth()
		{
			if (this.entries.Count == 0)
			{
				this.maxWidth = (this.minWidth = (float)base.style.padding.horizontal);
			}
			else
			{
				int num = 0;
				int num2 = 0;
				this.m_ChildMinWidth = 0f;
				this.m_ChildMaxWidth = 0f;
				this.m_StretchableCountX = 0;
				bool flag = true;
				if (this.isVertical)
				{
					foreach (GUILayoutEntry guilayoutEntry in this.entries)
					{
						guilayoutEntry.CalcWidth();
						RectOffset margin = guilayoutEntry.margin;
						if (guilayoutEntry.style != GUILayoutUtility.spaceStyle)
						{
							if (!flag)
							{
								num = Mathf.Min(margin.left, num);
								num2 = Mathf.Min(margin.right, num2);
							}
							else
							{
								num = margin.left;
								num2 = margin.right;
								flag = false;
							}
							this.m_ChildMinWidth = Mathf.Max(guilayoutEntry.minWidth + (float)margin.horizontal, this.m_ChildMinWidth);
							this.m_ChildMaxWidth = Mathf.Max(guilayoutEntry.maxWidth + (float)margin.horizontal, this.m_ChildMaxWidth);
						}
						this.m_StretchableCountX += guilayoutEntry.stretchWidth;
					}
					this.m_ChildMinWidth -= (float)(num + num2);
					this.m_ChildMaxWidth -= (float)(num + num2);
				}
				else
				{
					int num3 = 0;
					foreach (GUILayoutEntry guilayoutEntry2 in this.entries)
					{
						guilayoutEntry2.CalcWidth();
						RectOffset margin2 = guilayoutEntry2.margin;
						if (guilayoutEntry2.style != GUILayoutUtility.spaceStyle)
						{
							int num4;
							if (!flag)
							{
								num4 = ((num3 <= margin2.left) ? margin2.left : num3);
							}
							else
							{
								num4 = 0;
								flag = false;
							}
							this.m_ChildMinWidth += guilayoutEntry2.minWidth + this.spacing + (float)num4;
							this.m_ChildMaxWidth += guilayoutEntry2.maxWidth + this.spacing + (float)num4;
							num3 = margin2.right;
							this.m_StretchableCountX += guilayoutEntry2.stretchWidth;
						}
						else
						{
							this.m_ChildMinWidth += guilayoutEntry2.minWidth;
							this.m_ChildMaxWidth += guilayoutEntry2.maxWidth;
							this.m_StretchableCountX += guilayoutEntry2.stretchWidth;
						}
					}
					this.m_ChildMinWidth -= this.spacing;
					this.m_ChildMaxWidth -= this.spacing;
					if (this.entries.Count != 0)
					{
						num = this.entries[0].margin.left;
						num2 = num3;
					}
					else
					{
						num2 = (num = 0);
					}
				}
				float num5;
				float num6;
				if (base.style != GUIStyle.none || this.m_UserSpecifiedWidth)
				{
					num5 = (float)Mathf.Max(base.style.padding.left, num);
					num6 = (float)Mathf.Max(base.style.padding.right, num2);
				}
				else
				{
					this.m_Margin.left = num;
					this.m_Margin.right = num2;
					num6 = (num5 = 0f);
				}
				this.minWidth = Mathf.Max(this.minWidth, this.m_ChildMinWidth + num5 + num6);
				if (this.maxWidth == 0f)
				{
					this.stretchWidth += this.m_StretchableCountX + ((!base.style.stretchWidth) ? 0 : 1);
					this.maxWidth = this.m_ChildMaxWidth + num5 + num6;
				}
				else
				{
					this.stretchWidth = 0;
				}
				this.maxWidth = Mathf.Max(this.maxWidth, this.minWidth);
				if (base.style.fixedWidth != 0f)
				{
					this.maxWidth = (this.minWidth = base.style.fixedWidth);
					this.stretchWidth = 0;
				}
			}
		}

		public override void SetHorizontal(float x, float width)
		{
			base.SetHorizontal(x, width);
			if (this.resetCoords)
			{
				x = 0f;
			}
			RectOffset padding = base.style.padding;
			if (this.isVertical)
			{
				if (base.style != GUIStyle.none)
				{
					foreach (GUILayoutEntry guilayoutEntry in this.entries)
					{
						float num = (float)Mathf.Max(guilayoutEntry.margin.left, padding.left);
						float x2 = x + num;
						float num2 = width - (float)Mathf.Max(guilayoutEntry.margin.right, padding.right) - num;
						if (guilayoutEntry.stretchWidth != 0)
						{
							guilayoutEntry.SetHorizontal(x2, num2);
						}
						else
						{
							guilayoutEntry.SetHorizontal(x2, Mathf.Clamp(num2, guilayoutEntry.minWidth, guilayoutEntry.maxWidth));
						}
					}
				}
				else
				{
					float num3 = x - (float)this.margin.left;
					float num4 = width + (float)this.margin.horizontal;
					foreach (GUILayoutEntry guilayoutEntry2 in this.entries)
					{
						if (guilayoutEntry2.stretchWidth != 0)
						{
							guilayoutEntry2.SetHorizontal(num3 + (float)guilayoutEntry2.margin.left, num4 - (float)guilayoutEntry2.margin.horizontal);
						}
						else
						{
							guilayoutEntry2.SetHorizontal(num3 + (float)guilayoutEntry2.margin.left, Mathf.Clamp(num4 - (float)guilayoutEntry2.margin.horizontal, guilayoutEntry2.minWidth, guilayoutEntry2.maxWidth));
						}
					}
				}
			}
			else
			{
				if (base.style != GUIStyle.none)
				{
					float num5 = (float)padding.left;
					float num6 = (float)padding.right;
					if (this.entries.Count != 0)
					{
						num5 = Mathf.Max(num5, (float)this.entries[0].margin.left);
						num6 = Mathf.Max(num6, (float)this.entries[this.entries.Count - 1].margin.right);
					}
					x += num5;
					width -= num6 + num5;
				}
				float num7 = width - this.spacing * (float)(this.entries.Count - 1);
				float t = 0f;
				if (this.m_ChildMinWidth != this.m_ChildMaxWidth)
				{
					t = Mathf.Clamp((num7 - this.m_ChildMinWidth) / (this.m_ChildMaxWidth - this.m_ChildMinWidth), 0f, 1f);
				}
				float num8 = 0f;
				if (num7 > this.m_ChildMaxWidth)
				{
					if (this.m_StretchableCountX > 0)
					{
						num8 = (num7 - this.m_ChildMaxWidth) / (float)this.m_StretchableCountX;
					}
				}
				int num9 = 0;
				bool flag = true;
				foreach (GUILayoutEntry guilayoutEntry3 in this.entries)
				{
					float num10 = Mathf.Lerp(guilayoutEntry3.minWidth, guilayoutEntry3.maxWidth, t);
					num10 += num8 * (float)guilayoutEntry3.stretchWidth;
					if (guilayoutEntry3.style != GUILayoutUtility.spaceStyle)
					{
						int num11 = guilayoutEntry3.margin.left;
						if (flag)
						{
							num11 = 0;
							flag = false;
						}
						int num12 = (num9 <= num11) ? num11 : num9;
						x += (float)num12;
						num9 = guilayoutEntry3.margin.right;
					}
					guilayoutEntry3.SetHorizontal(Mathf.Round(x), Mathf.Round(num10));
					x += num10 + this.spacing;
				}
			}
		}

		public override void CalcHeight()
		{
			if (this.entries.Count == 0)
			{
				this.maxHeight = (this.minHeight = (float)base.style.padding.vertical);
			}
			else
			{
				int num = 0;
				int num2 = 0;
				this.m_ChildMinHeight = 0f;
				this.m_ChildMaxHeight = 0f;
				this.m_StretchableCountY = 0;
				if (this.isVertical)
				{
					int num3 = 0;
					bool flag = true;
					foreach (GUILayoutEntry guilayoutEntry in this.entries)
					{
						guilayoutEntry.CalcHeight();
						RectOffset margin = guilayoutEntry.margin;
						if (guilayoutEntry.style != GUILayoutUtility.spaceStyle)
						{
							int num4;
							if (!flag)
							{
								num4 = Mathf.Max(num3, margin.top);
							}
							else
							{
								num4 = 0;
								flag = false;
							}
							this.m_ChildMinHeight += guilayoutEntry.minHeight + this.spacing + (float)num4;
							this.m_ChildMaxHeight += guilayoutEntry.maxHeight + this.spacing + (float)num4;
							num3 = margin.bottom;
							this.m_StretchableCountY += guilayoutEntry.stretchHeight;
						}
						else
						{
							this.m_ChildMinHeight += guilayoutEntry.minHeight;
							this.m_ChildMaxHeight += guilayoutEntry.maxHeight;
							this.m_StretchableCountY += guilayoutEntry.stretchHeight;
						}
					}
					this.m_ChildMinHeight -= this.spacing;
					this.m_ChildMaxHeight -= this.spacing;
					if (this.entries.Count != 0)
					{
						num = this.entries[0].margin.top;
						num2 = num3;
					}
					else
					{
						num = (num2 = 0);
					}
				}
				else
				{
					bool flag2 = true;
					foreach (GUILayoutEntry guilayoutEntry2 in this.entries)
					{
						guilayoutEntry2.CalcHeight();
						RectOffset margin2 = guilayoutEntry2.margin;
						if (guilayoutEntry2.style != GUILayoutUtility.spaceStyle)
						{
							if (!flag2)
							{
								num = Mathf.Min(margin2.top, num);
								num2 = Mathf.Min(margin2.bottom, num2);
							}
							else
							{
								num = margin2.top;
								num2 = margin2.bottom;
								flag2 = false;
							}
							this.m_ChildMinHeight = Mathf.Max(guilayoutEntry2.minHeight, this.m_ChildMinHeight);
							this.m_ChildMaxHeight = Mathf.Max(guilayoutEntry2.maxHeight, this.m_ChildMaxHeight);
						}
						this.m_StretchableCountY += guilayoutEntry2.stretchHeight;
					}
				}
				float num5;
				float num6;
				if (base.style != GUIStyle.none || this.m_UserSpecifiedHeight)
				{
					num5 = (float)Mathf.Max(base.style.padding.top, num);
					num6 = (float)Mathf.Max(base.style.padding.bottom, num2);
				}
				else
				{
					this.m_Margin.top = num;
					this.m_Margin.bottom = num2;
					num6 = (num5 = 0f);
				}
				this.minHeight = Mathf.Max(this.minHeight, this.m_ChildMinHeight + num5 + num6);
				if (this.maxHeight == 0f)
				{
					this.stretchHeight += this.m_StretchableCountY + ((!base.style.stretchHeight) ? 0 : 1);
					this.maxHeight = this.m_ChildMaxHeight + num5 + num6;
				}
				else
				{
					this.stretchHeight = 0;
				}
				this.maxHeight = Mathf.Max(this.maxHeight, this.minHeight);
				if (base.style.fixedHeight != 0f)
				{
					this.maxHeight = (this.minHeight = base.style.fixedHeight);
					this.stretchHeight = 0;
				}
			}
		}

		public override void SetVertical(float y, float height)
		{
			base.SetVertical(y, height);
			if (this.entries.Count != 0)
			{
				RectOffset padding = base.style.padding;
				if (this.resetCoords)
				{
					y = 0f;
				}
				if (this.isVertical)
				{
					if (base.style != GUIStyle.none)
					{
						float num = (float)padding.top;
						float num2 = (float)padding.bottom;
						if (this.entries.Count != 0)
						{
							num = Mathf.Max(num, (float)this.entries[0].margin.top);
							num2 = Mathf.Max(num2, (float)this.entries[this.entries.Count - 1].margin.bottom);
						}
						y += num;
						height -= num2 + num;
					}
					float num3 = height - this.spacing * (float)(this.entries.Count - 1);
					float t = 0f;
					if (this.m_ChildMinHeight != this.m_ChildMaxHeight)
					{
						t = Mathf.Clamp((num3 - this.m_ChildMinHeight) / (this.m_ChildMaxHeight - this.m_ChildMinHeight), 0f, 1f);
					}
					float num4 = 0f;
					if (num3 > this.m_ChildMaxHeight)
					{
						if (this.m_StretchableCountY > 0)
						{
							num4 = (num3 - this.m_ChildMaxHeight) / (float)this.m_StretchableCountY;
						}
					}
					int num5 = 0;
					bool flag = true;
					foreach (GUILayoutEntry guilayoutEntry in this.entries)
					{
						float num6 = Mathf.Lerp(guilayoutEntry.minHeight, guilayoutEntry.maxHeight, t);
						num6 += num4 * (float)guilayoutEntry.stretchHeight;
						if (guilayoutEntry.style != GUILayoutUtility.spaceStyle)
						{
							int num7 = guilayoutEntry.margin.top;
							if (flag)
							{
								num7 = 0;
								flag = false;
							}
							int num8 = (num5 <= num7) ? num7 : num5;
							y += (float)num8;
							num5 = guilayoutEntry.margin.bottom;
						}
						guilayoutEntry.SetVertical(Mathf.Round(y), Mathf.Round(num6));
						y += num6 + this.spacing;
					}
				}
				else if (base.style != GUIStyle.none)
				{
					foreach (GUILayoutEntry guilayoutEntry2 in this.entries)
					{
						float num9 = (float)Mathf.Max(guilayoutEntry2.margin.top, padding.top);
						float y2 = y + num9;
						float num10 = height - (float)Mathf.Max(guilayoutEntry2.margin.bottom, padding.bottom) - num9;
						if (guilayoutEntry2.stretchHeight != 0)
						{
							guilayoutEntry2.SetVertical(y2, num10);
						}
						else
						{
							guilayoutEntry2.SetVertical(y2, Mathf.Clamp(num10, guilayoutEntry2.minHeight, guilayoutEntry2.maxHeight));
						}
					}
				}
				else
				{
					float num11 = y - (float)this.margin.top;
					float num12 = height + (float)this.margin.vertical;
					foreach (GUILayoutEntry guilayoutEntry3 in this.entries)
					{
						if (guilayoutEntry3.stretchHeight != 0)
						{
							guilayoutEntry3.SetVertical(num11 + (float)guilayoutEntry3.margin.top, num12 - (float)guilayoutEntry3.margin.vertical);
						}
						else
						{
							guilayoutEntry3.SetVertical(num11 + (float)guilayoutEntry3.margin.top, Mathf.Clamp(num12 - (float)guilayoutEntry3.margin.vertical, guilayoutEntry3.minHeight, guilayoutEntry3.maxHeight));
						}
					}
				}
			}
		}

		public override string ToString()
		{
			string text = "";
			string text2 = "";
			for (int i = 0; i < GUILayoutEntry.indent; i++)
			{
				text2 += " ";
			}
			string text3 = text;
			text = string.Concat(new object[]
			{
				text3,
				base.ToString(),
				" Margins: ",
				this.m_ChildMinHeight,
				" {\n"
			});
			GUILayoutEntry.indent += 4;
			foreach (GUILayoutEntry guilayoutEntry in this.entries)
			{
				text = text + guilayoutEntry.ToString() + "\n";
			}
			text = text + text2 + "}";
			GUILayoutEntry.indent -= 4;
			return text;
		}
	}
}
