using System;

namespace UnityEngine.Experimental.UIElements
{
	internal class TouchScreenTextEditor : TextEditor
	{
		private string m_SecureText;

		public TouchScreenTextEditor(TextField textField) : base(textField)
		{
			this.secureText = string.Empty;
		}

		public string secureText
		{
			get
			{
				return this.m_SecureText;
			}
			set
			{
				string text = value ?? string.Empty;
				if (text != this.m_SecureText)
				{
					this.m_SecureText = text;
				}
			}
		}

		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseUpDownEvent), Capture.NoCapture);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseUpDownEvent), Capture.NoCapture);
		}

		private void OnMouseUpDownEvent(MouseDownEvent evt)
		{
			base.SyncTextEditor();
			base.textField.TakeCapture();
			this.keyboardOnScreen = TouchScreenKeyboard.Open(string.IsNullOrEmpty(this.secureText) ? base.textField.text : this.secureText, TouchScreenKeyboardType.Default, true, this.multiline, !string.IsNullOrEmpty(this.secureText));
			base.UpdateScrollOffset();
			evt.StopPropagation();
		}
	}
}
