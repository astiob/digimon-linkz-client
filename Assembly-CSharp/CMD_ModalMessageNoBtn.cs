using System;
using UnityEngine;

public sealed class CMD_ModalMessageNoBtn : CMD
{
	[SerializeField]
	private UILabel textLabel;

	public void SetParam(string text)
	{
		this.textLabel.text = text;
	}

	public void SetFontSize(int size)
	{
		this.textLabel.fontSize = size;
	}

	public void AdjustSize()
	{
		int num = this.textLabel.text.Split(new char[]
		{
			'\n'
		}).Length;
		if (num >= 6)
		{
			int num2 = (this.textLabel.fontSize + this.textLabel.spacingY) * (num - 5);
			base.GetComponent<UIWidget>().height += num2;
		}
	}
}
