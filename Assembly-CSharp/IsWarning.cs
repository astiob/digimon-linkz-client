using Master;
using System;
using UnityEngine;

public class IsWarning : MonoBehaviour
{
	[SerializeField]
	[Header("UIWidget")]
	public UIWidget widget;

	[Header("Allyのタイトルバーのスキナー")]
	[SerializeField]
	private UIComponentSkinner allyTitleBarSkinner;

	[SerializeField]
	[Header("メッセージ")]
	private UILabel messageLocalize;

	public void ApplyWarning(SufferStateProperty.SufferType sufferType, CharacterStateControl characterStateControl = null)
	{
		this.allyTitleBarSkinner.SetSkins((!characterStateControl.isEnemy) ? 0 : 1);
		switch (sufferType)
		{
		case SufferStateProperty.SufferType.Paralysis:
			this.messageLocalize.text = StringMaster.GetString("BattleNotice-10");
			return;
		case SufferStateProperty.SufferType.Sleep:
			this.messageLocalize.text = StringMaster.GetString("BattleNotice-06");
			return;
		case SufferStateProperty.SufferType.Stun:
			this.messageLocalize.text = StringMaster.GetString("BattleNotice-09");
			return;
		default:
			if (sufferType != SufferStateProperty.SufferType.PowerCharge)
			{
				return;
			}
			this.messageLocalize.text = StringMaster.GetString("BattleNotice-07");
			return;
		}
	}
}
