using System;

namespace UnityEngine.Experimental.UIElements
{
	internal class KeyboardTextEditor : TextEditor
	{
		internal bool m_Changed;

		private bool m_Dragged;

		private bool m_DragToPosition = true;

		private bool m_PostPoneMove;

		private bool m_SelectAllOnMouseUp = true;

		private string m_PreDrawCursorText;

		public KeyboardTextEditor(TextField textField) : base(textField)
		{
		}

		protected override void RegisterCallbacksOnTarget()
		{
			base.RegisterCallbacksOnTarget();
			base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
			base.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), Capture.NoCapture);
			base.target.RegisterCallback<IMGUIEvent>(new EventCallback<IMGUIEvent>(this.OnIMGUIEvent), Capture.NoCapture);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.UnregisterCallbacksFromTarget();
			base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
			base.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.UnregisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), Capture.NoCapture);
			base.target.UnregisterCallback<IMGUIEvent>(new EventCallback<IMGUIEvent>(this.OnIMGUIEvent), Capture.NoCapture);
		}

		private void OnMouseDown(MouseDownEvent evt)
		{
			base.SyncTextEditor();
			this.m_Changed = false;
			base.target.TakeCapture();
			if (!this.m_HasFocus)
			{
				this.m_HasFocus = true;
				base.MoveCursorToPosition_Internal(evt.localMousePosition, evt.shiftKey);
				evt.StopPropagation();
			}
			else
			{
				if (evt.clickCount == 2 && base.doubleClickSelectsWord)
				{
					base.SelectCurrentWord();
					base.DblClickSnap(TextEditor.DblClickSnapping.WORDS);
					base.MouseDragSelectsWholeWords(true);
					this.m_DragToPosition = false;
				}
				else if (evt.clickCount == 3 && base.tripleClickSelectsLine)
				{
					base.SelectCurrentParagraph();
					base.MouseDragSelectsWholeWords(true);
					base.DblClickSnap(TextEditor.DblClickSnapping.PARAGRAPHS);
					this.m_DragToPosition = false;
				}
				else
				{
					base.MoveCursorToPosition_Internal(evt.localMousePosition, evt.shiftKey);
					this.m_SelectAllOnMouseUp = false;
				}
				evt.StopPropagation();
			}
			if (this.m_Changed)
			{
				if (base.maxLength >= 0 && base.text != null && base.text.Length > base.maxLength)
				{
					base.text = base.text.Substring(0, base.maxLength);
				}
				base.textField.text = base.text;
				base.textField.TextFieldChanged();
				evt.StopPropagation();
			}
			base.UpdateScrollOffset();
		}

		private void OnMouseUp(MouseUpEvent evt)
		{
			if (base.target.HasCapture())
			{
				base.SyncTextEditor();
				this.m_Changed = false;
				if (this.m_Dragged && this.m_DragToPosition)
				{
					base.MoveSelectionToAltCursor();
				}
				else if (this.m_PostPoneMove)
				{
					base.MoveCursorToPosition_Internal(evt.localMousePosition, evt.shiftKey);
				}
				else if (this.m_SelectAllOnMouseUp)
				{
					this.m_SelectAllOnMouseUp = false;
				}
				base.MouseDragSelectsWholeWords(false);
				base.target.ReleaseCapture();
				this.m_DragToPosition = true;
				this.m_Dragged = false;
				this.m_PostPoneMove = false;
				evt.StopPropagation();
				if (this.m_Changed)
				{
					if (base.maxLength >= 0 && base.text != null && base.text.Length > base.maxLength)
					{
						base.text = base.text.Substring(0, base.maxLength);
					}
					base.textField.text = base.text;
					base.textField.TextFieldChanged();
					evt.StopPropagation();
				}
				base.UpdateScrollOffset();
			}
		}

		private void OnMouseMove(MouseMoveEvent evt)
		{
			if (base.target.HasCapture())
			{
				base.SyncTextEditor();
				this.m_Changed = false;
				if (!evt.shiftKey && base.hasSelection && this.m_DragToPosition)
				{
					base.MoveAltCursorToPosition(evt.localMousePosition);
				}
				else
				{
					if (evt.shiftKey)
					{
						base.MoveCursorToPosition_Internal(evt.localMousePosition, evt.shiftKey);
					}
					else
					{
						base.SelectToPosition(evt.localMousePosition);
					}
					this.m_DragToPosition = false;
					this.m_SelectAllOnMouseUp = !base.hasSelection;
				}
				this.m_Dragged = true;
				evt.StopPropagation();
				if (this.m_Changed)
				{
					if (base.maxLength >= 0 && base.text != null && base.text.Length > base.maxLength)
					{
						base.text = base.text.Substring(0, base.maxLength);
					}
					base.textField.text = base.text;
					base.textField.TextFieldChanged();
					evt.StopPropagation();
				}
				base.UpdateScrollOffset();
			}
		}

		private void OnKeyDown(KeyDownEvent evt)
		{
			if (base.textField.hasFocus)
			{
				base.SyncTextEditor();
				this.m_Changed = false;
				if (base.HandleKeyEvent(evt.imguiEvent))
				{
					this.m_Changed = true;
					base.textField.text = base.text;
					evt.StopPropagation();
				}
				else
				{
					if (evt.keyCode == KeyCode.Tab || evt.character == '\t')
					{
						return;
					}
					char character = evt.character;
					if (character == '\n' && !this.multiline && !evt.altKey)
					{
						base.textField.TextFieldChangeValidated();
						return;
					}
					Font font = base.textField.editor.style.font;
					if ((font != null && font.HasCharacter(character)) || character == '\n')
					{
						base.Insert(character);
						this.m_Changed = true;
					}
					else if (character == '\0')
					{
						if (!string.IsNullOrEmpty(Input.compositionString))
						{
							base.ReplaceSelection("");
							this.m_Changed = true;
						}
						evt.StopPropagation();
					}
				}
				if (this.m_Changed)
				{
					if (base.maxLength >= 0 && base.text != null && base.text.Length > base.maxLength)
					{
						base.text = base.text.Substring(0, base.maxLength);
					}
					base.textField.text = base.text;
					base.textField.TextFieldChanged();
					evt.StopPropagation();
				}
				base.UpdateScrollOffset();
			}
		}

		private void OnIMGUIEvent(IMGUIEvent evt)
		{
			if (base.textField.hasFocus)
			{
				base.SyncTextEditor();
				this.m_Changed = false;
				EventType type = evt.imguiEvent.type;
				if (type != EventType.ValidateCommand)
				{
					if (type == EventType.ExecuteCommand)
					{
						bool flag = false;
						string text = base.text;
						if (!base.textField.hasFocus)
						{
							return;
						}
						string commandName = evt.imguiEvent.commandName;
						if (commandName != null)
						{
							if (commandName == "OnLostFocus")
							{
								evt.StopPropagation();
								return;
							}
							if (!(commandName == "Cut"))
							{
								if (commandName == "Copy")
								{
									base.Copy();
									evt.StopPropagation();
									return;
								}
								if (!(commandName == "Paste"))
								{
									if (commandName == "SelectAll")
									{
										base.SelectAll();
										evt.StopPropagation();
										return;
									}
									if (commandName == "Delete")
									{
										if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX)
										{
											base.Delete();
										}
										else
										{
											base.Cut();
										}
										flag = true;
									}
								}
								else
								{
									base.Paste();
									flag = true;
								}
							}
							else
							{
								base.Cut();
								flag = true;
							}
						}
						if (flag)
						{
							if (text != base.text)
							{
								this.m_Changed = true;
							}
							evt.StopPropagation();
						}
					}
				}
				else
				{
					string commandName2 = evt.imguiEvent.commandName;
					if (commandName2 != null)
					{
						if (!(commandName2 == "Cut") && !(commandName2 == "Copy"))
						{
							if (!(commandName2 == "Paste"))
							{
								if (!(commandName2 == "SelectAll") && !(commandName2 == "Delete"))
								{
									if (!(commandName2 == "UndoRedoPerformed"))
									{
									}
								}
							}
							else if (!base.CanPaste())
							{
								return;
							}
						}
						else if (!base.hasSelection)
						{
							return;
						}
					}
					evt.StopPropagation();
				}
				if (this.m_Changed)
				{
					if (base.maxLength >= 0 && base.text != null && base.text.Length > base.maxLength)
					{
						base.text = base.text.Substring(0, base.maxLength);
					}
					base.textField.text = base.text;
					base.textField.TextFieldChanged();
					evt.StopPropagation();
				}
				base.UpdateScrollOffset();
			}
		}

		public void PreDrawCursor(string newText)
		{
			base.SyncTextEditor();
			this.m_PreDrawCursorText = base.text;
			int num = base.cursorIndex;
			if (!string.IsNullOrEmpty(Input.compositionString))
			{
				base.text = newText.Substring(0, base.cursorIndex) + Input.compositionString + newText.Substring(base.selectIndex);
				num += Input.compositionString.Length;
			}
			else
			{
				base.text = newText;
			}
			if (base.maxLength >= 0 && base.text != null && base.text.Length > base.maxLength)
			{
				base.text = base.text.Substring(0, base.maxLength);
				num = Math.Min(num, base.maxLength - 1);
			}
			this.graphicalCursorPos = this.style.GetCursorPixelPosition(this.localPosition, new GUIContent(base.text), num);
		}

		public void PostDrawCursor()
		{
			base.text = this.m_PreDrawCursorText;
		}
	}
}
