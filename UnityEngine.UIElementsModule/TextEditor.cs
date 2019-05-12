using System;

namespace UnityEngine.Experimental.UIElements
{
	public class TextEditor : TextEditor, IManipulator
	{
		private VisualElement m_Target;

		protected TextEditor(TextField textField)
		{
			this.textField = textField;
			this.SyncTextEditor();
		}

		public int maxLength { get; set; }

		public char maskChar { get; set; }

		public bool doubleClickSelectsWord { get; set; }

		public bool tripleClickSelectsLine { get; set; }

		protected TextField textField { get; set; }

		internal override Rect localPosition
		{
			get
			{
				return new Rect(0f, 0f, base.position.width, base.position.height);
			}
		}

		protected virtual void RegisterCallbacksOnTarget()
		{
			this.target.RegisterCallback<FocusEvent>(new EventCallback<FocusEvent>(this.OnFocus), Capture.NoCapture);
			this.target.RegisterCallback<BlurEvent>(new EventCallback<BlurEvent>(this.OnBlur), Capture.NoCapture);
		}

		protected virtual void UnregisterCallbacksFromTarget()
		{
			this.target.UnregisterCallback<FocusEvent>(new EventCallback<FocusEvent>(this.OnFocus), Capture.NoCapture);
			this.target.UnregisterCallback<BlurEvent>(new EventCallback<BlurEvent>(this.OnBlur), Capture.NoCapture);
		}

		private void OnFocus(FocusEvent evt)
		{
			base.OnFocus();
		}

		private void OnBlur(BlurEvent evt)
		{
			base.OnLostFocus();
		}

		public VisualElement target
		{
			get
			{
				return this.m_Target;
			}
			set
			{
				if (this.target != null)
				{
					this.UnregisterCallbacksFromTarget();
				}
				this.m_Target = value;
				if (this.target != null)
				{
					this.RegisterCallbacksOnTarget();
				}
			}
		}

		protected void SyncTextEditor()
		{
			string text = this.textField.text;
			if (this.maxLength >= 0 && text != null && text.Length > this.maxLength)
			{
				text = text.Substring(0, this.maxLength);
			}
			base.text = text;
			base.SaveBackup();
			base.position = this.textField.layout;
			this.maxLength = this.textField.maxLength;
			this.multiline = this.textField.multiline;
			this.isPasswordField = this.textField.isPasswordField;
			this.maskChar = this.textField.maskChar;
			this.doubleClickSelectsWord = this.textField.doubleClickSelectsWord;
			this.tripleClickSelectsLine = this.textField.tripleClickSelectsLine;
			base.DetectFocusChange();
		}

		internal override void OnDetectFocusChange()
		{
			if (this.m_HasFocus && !this.textField.hasFocus)
			{
				base.OnFocus();
			}
			if (!this.m_HasFocus && this.textField.hasFocus)
			{
				base.OnLostFocus();
			}
		}
	}
}
