using Master;
using System;
using UnityEngine;

public class CMD_PartsFriendIDSearch : CMD
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel buttonLabel;

	[SerializeField]
	private UISprite buttonSprite;

	[SerializeField]
	private BoxCollider buttonCollider;

	[SerializeField]
	private UIInput ngINPUT;

	[SerializeField]
	private UILabel adviceLabel;

	public static string InputValue { get; private set; }

	public static string InputSaveValue { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		CMD_PartsFriendIDSearch.InputValue = string.Empty;
		this.titleLabel.text = StringMaster.GetString("FriendSearch-01");
		this.buttonLabel.text = StringMaster.GetString("FriendSearch-03");
		this.adviceLabel.text = StringMaster.GetString("FriendSearch-02");
		this.EnableButton(false);
		if (!string.IsNullOrEmpty(CMD_PartsFriendIDSearch.InputSaveValue))
		{
			this.ngINPUT.value = CMD_PartsFriendIDSearch.InputSaveValue;
		}
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

	public void OnChangedValue()
	{
		if (this.ngINPUT != null)
		{
			this.ngINPUT.value = this.ngINPUT.value.Replace("-", string.Empty);
			this.ngINPUT.value = this.ngINPUT.value.Replace("\n", string.Empty).Replace("\r", string.Empty);
			this.ngINPUT.label.text = this.ngINPUT.value;
			CMD_PartsFriendIDSearch.InputValue = this.ngINPUT.value;
			if (string.IsNullOrEmpty(this.ngINPUT.value) || this.ngINPUT.value.Length != 8)
			{
				this.EnableButton(false);
			}
			else
			{
				this.EnableButton(true);
			}
		}
	}

	private void EnableButton(bool IsEnable)
	{
		this.buttonLabel.color = ((!IsEnable) ? Color.gray : Color.white);
		this.buttonSprite.spriteName = ((!IsEnable) ? "Common02_Btn_BaseG" : "Common02_Btn_BaseON1");
		this.buttonCollider.enabled = IsEnable;
	}

	public static void SaveInputId()
	{
		CMD_PartsFriendIDSearch.InputSaveValue = CMD_PartsFriendIDSearch.InputValue;
	}

	public static void DeleteInputId()
	{
		CMD_PartsFriendIDSearch.InputSaveValue = string.Empty;
	}
}
