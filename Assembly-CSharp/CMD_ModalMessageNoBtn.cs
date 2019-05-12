using System;
using UnityEngine;

public class CMD_ModalMessageNoBtn : CMD
{
	[SerializeField]
	private UILabel textLabel;

	public void SetParam(string text)
	{
		this.textLabel.text = text;
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
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
