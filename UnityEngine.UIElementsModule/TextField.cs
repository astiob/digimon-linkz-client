using System;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
	public class TextField : VisualContainer
	{
		public Action<string> OnTextChanged;

		public Action OnTextChangeValidated;

		private const string SelectionColorProperty = "selection-color";

		private const string CursorColorProperty = "cursor-color";

		private StyleValue<Color> m_SelectionColor;

		private StyleValue<Color> m_CursorColor;

		private bool m_Multiline;

		private bool m_IsPasswordField;

		internal const int kMaxLengthNone = -1;

		public TextField() : this(-1, false, false, '\0')
		{
		}

		public TextField(int maxLength, bool multiline, bool isPasswordField, char maskChar)
		{
			this.maxLength = maxLength;
			this.multiline = multiline;
			this.isPasswordField = isPasswordField;
			this.maskChar = maskChar;
			if (this.touchScreenTextField)
			{
				this.editor = new TouchScreenTextEditor(this);
			}
			else
			{
				this.doubleClickSelectsWord = true;
				this.tripleClickSelectsLine = true;
				this.editor = new KeyboardTextEditor(this);
			}
			this.editor.style = new GUIStyle(this.editor.style);
			base.focusIndex = 0;
			this.AddManipulator(this.editor);
		}

		public Color selectionColor
		{
			get
			{
				return this.m_SelectionColor.GetSpecifiedValueOrDefault(Color.clear);
			}
		}

		public Color cursorColor
		{
			get
			{
				return this.m_CursorColor.GetSpecifiedValueOrDefault(Color.clear);
			}
		}

		public bool multiline
		{
			get
			{
				return this.m_Multiline;
			}
			set
			{
				this.m_Multiline = value;
				if (!value)
				{
					base.text = base.text.Replace("\n", "");
				}
			}
		}

		public bool isPasswordField
		{
			get
			{
				return this.m_IsPasswordField;
			}
			set
			{
				this.m_IsPasswordField = value;
				if (value)
				{
					this.multiline = false;
				}
			}
		}

		public char maskChar { get; set; }

		public bool doubleClickSelectsWord { get; set; }

		public bool tripleClickSelectsLine { get; set; }

		public int maxLength { get; set; }

		private bool touchScreenTextField
		{
			get
			{
				return TouchScreenKeyboard.isSupported;
			}
		}

		public bool hasFocus
		{
			get
			{
				return base.elementPanel != null && base.elementPanel.focusController.focusedElement == this;
			}
		}

		internal TextEditor editor { get; set; }

		internal void TextFieldChanged()
		{
			if (this.OnTextChanged != null)
			{
				this.OnTextChanged(base.text);
			}
		}

		internal void TextFieldChangeValidated()
		{
			if (this.OnTextChangeValidated != null)
			{
				this.OnTextChangeValidated();
			}
		}

		public override void OnPersistentDataReady()
		{
			base.OnPersistentDataReady();
			string fullHierarchicalPersistenceKey = base.GetFullHierarchicalPersistenceKey();
			base.OverwriteFromPersistedData(this, fullHierarchicalPersistenceKey);
		}

		public override void OnStyleResolved(ICustomStyle style)
		{
			base.OnStyleResolved(style);
			base.effectiveStyle.ApplyCustomProperty("selection-color", ref this.m_SelectionColor);
			base.effectiveStyle.ApplyCustomProperty("cursor-color", ref this.m_CursorColor);
			base.effectiveStyle.WriteToGUIStyle(this.editor.style);
		}

		internal override void DoRepaint(IStylePainter painter)
		{
			if (this.touchScreenTextField)
			{
				TouchScreenTextEditor touchScreenTextEditor = this.editor as TouchScreenTextEditor;
				if (touchScreenTextEditor != null && touchScreenTextEditor.keyboardOnScreen != null)
				{
					base.text = touchScreenTextEditor.keyboardOnScreen.text;
					if (this.editor.maxLength >= 0 && base.text != null && base.text.Length > this.editor.maxLength)
					{
						base.text = base.text.Substring(0, this.editor.maxLength);
					}
					if (touchScreenTextEditor.keyboardOnScreen.done)
					{
						touchScreenTextEditor.keyboardOnScreen = null;
						GUI.changed = true;
					}
				}
				string text = base.text;
				if (touchScreenTextEditor != null && !string.IsNullOrEmpty(touchScreenTextEditor.secureText))
				{
					text = "".PadRight(touchScreenTextEditor.secureText.Length, this.maskChar);
				}
				base.DoRepaint(painter);
				base.text = text;
			}
			else if (this.isPasswordField)
			{
				string text2 = base.text;
				base.text = "".PadRight(base.text.Length, this.maskChar);
				if (!this.hasFocus)
				{
					base.DoRepaint(painter);
				}
				else
				{
					this.DrawWithTextSelectionAndCursor(painter, base.text);
				}
				base.text = text2;
			}
			else if (!this.hasFocus)
			{
				base.DoRepaint(painter);
			}
			else
			{
				this.DrawWithTextSelectionAndCursor(painter, base.text);
			}
		}

		private void DrawWithTextSelectionAndCursor(IStylePainter painter, string newText)
		{
			KeyboardTextEditor keyboardTextEditor = this.editor as KeyboardTextEditor;
			if (keyboardTextEditor != null)
			{
				keyboardTextEditor.PreDrawCursor(newText);
				int cursorIndex = keyboardTextEditor.cursorIndex;
				int selectIndex = keyboardTextEditor.selectIndex;
				Rect localPosition = keyboardTextEditor.localPosition;
				Vector2 scrollOffset = keyboardTextEditor.scrollOffset;
				IStyle style = base.style;
				TextStylePainterParameters defaultTextParameters = painter.GetDefaultTextParameters(this);
				defaultTextParameters.text = " ";
				defaultTextParameters.wordWrapWidth = 0f;
				defaultTextParameters.wordWrap = false;
				float num = painter.ComputeTextHeight(defaultTextParameters);
				float num2 = (!style.wordWrap) ? 0f : base.contentRect.width;
				Input.compositionCursorPos = keyboardTextEditor.graphicalCursorPos - scrollOffset + new Vector2(localPosition.x, localPosition.y + num);
				Color color = (!(this.cursorColor != Color.clear)) ? GUI.skin.settings.cursorColor : this.cursorColor;
				int num3 = (!string.IsNullOrEmpty(Input.compositionString)) ? (cursorIndex + Input.compositionString.Length) : selectIndex;
				painter.DrawBackground(this);
				if (cursorIndex != num3)
				{
					RectStylePainterParameters defaultRectParameters = painter.GetDefaultRectParameters(this);
					defaultRectParameters.color = this.selectionColor;
					defaultRectParameters.borderLeftWidth = 0f;
					defaultRectParameters.borderTopWidth = 0f;
					defaultRectParameters.borderRightWidth = 0f;
					defaultRectParameters.borderBottomWidth = 0f;
					defaultRectParameters.borderTopLeftRadius = 0f;
					defaultRectParameters.borderTopRightRadius = 0f;
					defaultRectParameters.borderBottomRightRadius = 0f;
					defaultRectParameters.borderBottomLeftRadius = 0f;
					int cursorIndex2 = (cursorIndex >= num3) ? num3 : cursorIndex;
					int cursorIndex3 = (cursorIndex <= num3) ? num3 : cursorIndex;
					CursorPositionStylePainterParameters defaultCursorPositionParameters = painter.GetDefaultCursorPositionParameters(this);
					defaultCursorPositionParameters.text = keyboardTextEditor.text;
					defaultCursorPositionParameters.wordWrapWidth = num2;
					defaultCursorPositionParameters.cursorIndex = cursorIndex2;
					Vector2 a = painter.GetCursorPosition(defaultCursorPositionParameters);
					defaultCursorPositionParameters.cursorIndex = cursorIndex3;
					Vector2 a2 = painter.GetCursorPosition(defaultCursorPositionParameters);
					a -= scrollOffset;
					a2 -= scrollOffset;
					if (Mathf.Approximately(a.y, a2.y))
					{
						defaultRectParameters.layout = new Rect(a.x, a.y, a2.x - a.x, num);
						painter.DrawRect(defaultRectParameters);
					}
					else
					{
						defaultRectParameters.layout = new Rect(a.x, a.y, num2 - a.x, num);
						painter.DrawRect(defaultRectParameters);
						float num4 = a2.y - a.y - num;
						if (num4 > 0f)
						{
							defaultRectParameters.layout = new Rect(0f, a.y + num, num2, num4);
							painter.DrawRect(defaultRectParameters);
						}
						defaultRectParameters.layout = new Rect(0f, a2.y, a2.x, num);
						painter.DrawRect(defaultRectParameters);
					}
				}
				painter.DrawBorder(this);
				if (!string.IsNullOrEmpty(keyboardTextEditor.text) && base.contentRect.width > 0f && base.contentRect.height > 0f)
				{
					defaultTextParameters = painter.GetDefaultTextParameters(this);
					defaultTextParameters.layout = new Rect(base.contentRect.x - scrollOffset.x, base.contentRect.y - scrollOffset.y, base.contentRect.width, base.contentRect.height);
					defaultTextParameters.text = keyboardTextEditor.text;
					painter.DrawText(defaultTextParameters);
				}
				if (cursorIndex == num3 && style.font != null)
				{
					CursorPositionStylePainterParameters defaultCursorPositionParameters = painter.GetDefaultCursorPositionParameters(this);
					defaultCursorPositionParameters.text = keyboardTextEditor.text;
					defaultCursorPositionParameters.wordWrapWidth = num2;
					defaultCursorPositionParameters.cursorIndex = cursorIndex;
					Vector2 a3 = painter.GetCursorPosition(defaultCursorPositionParameters);
					a3 -= scrollOffset;
					RectStylePainterParameters painterParams = new RectStylePainterParameters
					{
						layout = new Rect(a3.x, a3.y, 1f, num),
						color = color
					};
					painter.DrawRect(painterParams);
				}
				if (keyboardTextEditor.altCursorPosition != -1)
				{
					CursorPositionStylePainterParameters defaultCursorPositionParameters = painter.GetDefaultCursorPositionParameters(this);
					defaultCursorPositionParameters.text = keyboardTextEditor.text.Substring(0, keyboardTextEditor.altCursorPosition);
					defaultCursorPositionParameters.wordWrapWidth = num2;
					defaultCursorPositionParameters.cursorIndex = keyboardTextEditor.altCursorPosition;
					Vector2 a4 = painter.GetCursorPosition(defaultCursorPositionParameters);
					a4 -= scrollOffset;
					RectStylePainterParameters painterParams2 = new RectStylePainterParameters
					{
						layout = new Rect(a4.x, a4.y, 1f, num),
						color = color
					};
					painter.DrawRect(painterParams2);
				}
				keyboardTextEditor.PostDrawCursor();
			}
		}
	}
}
