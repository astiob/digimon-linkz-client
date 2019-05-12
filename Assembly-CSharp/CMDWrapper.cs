using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CMDWrapper : CMD
{
	private Action<int> dialogCloseEvent;

	private float dialogSizeX;

	private float dialogSizeY;

	private float dialogShowTime;

	private List<Action> publicDialogCloseStartEvent;

	private List<Action> publicDialogClosedEvent;

	protected GameObject parentDialogGameObject;

	private void OnClosedEvent(int selectButtonIndex)
	{
		if (this.publicDialogClosedEvent != null)
		{
			for (int i = 0; i < this.publicDialogClosedEvent.Count; i++)
			{
				Action action = this.publicDialogClosedEvent[i];
				if (action != null)
				{
					action();
				}
				this.publicDialogClosedEvent[i] = null;
			}
			this.publicDialogClosedEvent.Clear();
		}
		this.OnClosedDialog();
		this.dialogCloseEvent(selectButtonIndex);
	}

	protected abstract void OnShowDialog();

	protected abstract void OnOpenedDialog();

	protected abstract bool OnCloseStartDialog();

	protected abstract void OnClosedDialog();

	public override void Show(Action<int> closeEvent, float sizeX, float sizeY, float showTime)
	{
		this.dialogCloseEvent = closeEvent;
		this.dialogSizeX = sizeX;
		this.dialogSizeY = sizeY;
		this.dialogShowTime = showTime;
	}

	public void Show()
	{
		this.OnShowDialog();
		base.Show(new Action<int>(this.OnClosedEvent), this.dialogSizeX, this.dialogSizeY, this.dialogShowTime);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		this.OnOpenedDialog();
	}

	protected static DialogType LoadPrefab<DialogType>(string fileName) where DialogType : CMDWrapper
	{
		return GUIMain.ShowCommonDialog(null, fileName) as DialogType;
	}

	public void SetCloseStartAction(Action closeStartAction)
	{
		if (this.publicDialogCloseStartEvent == null)
		{
			this.publicDialogCloseStartEvent = new List<Action>();
		}
		this.publicDialogCloseStartEvent.Add(closeStartAction);
	}

	public void SetClosedAction(Action closedAction)
	{
		if (this.publicDialogClosedEvent == null)
		{
			this.publicDialogClosedEvent = new List<Action>();
		}
		this.publicDialogClosedEvent.Add(closedAction);
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.publicDialogCloseStartEvent != null)
		{
			for (int i = 0; i < this.publicDialogCloseStartEvent.Count; i++)
			{
				Action action = this.publicDialogCloseStartEvent[i];
				if (action != null)
				{
					action();
				}
				this.publicDialogCloseStartEvent[i] = null;
			}
			this.publicDialogCloseStartEvent.Clear();
		}
		if (this.OnCloseStartDialog())
		{
			base.ClosePanel(animation);
		}
	}
}
