using Master;
using System;
using System.Collections;
using UnityEngine;

public sealed class RemainingTurn : MonoBehaviour
{
	[SerializeField]
	[Header("真ん中か右下かType")]
	private RemainingTurn.Type myType;

	[SerializeField]
	[Header("あと(右下用)")]
	private UILabel remainingLabel;

	[Header("ターン(右下用/真ん中メッセージ)")]
	[SerializeField]
	private UILabel turnLabel;

	[SerializeField]
	[Header("あなたのターンです(真ん中メッセージ)")]
	private UILabel yourTurnLabel;

	[SerializeField]
	[Header("あなたのターンまで残り(真ん中メッセージ)")]
	private UILabel yourRemaingTurnLabel;

	[Header("自分/仲間/敵メッセージ(右下用)")]
	[SerializeField]
	private UILabel[] messageLabels;

	[Header("コンテンツ")]
	[SerializeField]
	private GameObject contains;

	[Header("自分/仲間/敵コンテンツ(真ん中ターン表示)")]
	[SerializeField]
	private Animator centerAnimator;

	private void Awake()
	{
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		this.turnLabel.text = StringMaster.GetString("BattleNotice-02");
		if (this.myType == RemainingTurn.Type.RightDown)
		{
			this.remainingLabel.text = StringMaster.GetString("BattleNotice-14");
		}
		else if (this.myType == RemainingTurn.Type.Middle)
		{
			this.yourTurnLabel.text = StringMaster.GetString("BattleNotice-16");
			this.yourRemaingTurnLabel.text = StringMaster.GetString("BattleNotice-15");
		}
	}

	public void SetEnable(bool isActive)
	{
		NGUITools.SetActiveSelf(this.contains, isActive);
	}

	public void SetLabel(int num, RemainingTurn.MiddleType middleType)
	{
		RemainingTurn.Type type = this.myType;
		if (type != RemainingTurn.Type.RightDown)
		{
			if (type == RemainingTurn.Type.Middle)
			{
				if (middleType == RemainingTurn.MiddleType.You)
				{
					this.centerAnimator.SetTrigger("0");
				}
				else if (middleType == RemainingTurn.MiddleType.Others)
				{
					this.centerAnimator.SetTrigger(num.ToString());
				}
				else if (middleType == RemainingTurn.MiddleType.Enemy)
				{
					base.StartCoroutine(this.PlayDelayAnim(num));
				}
			}
		}
		else
		{
			this.messageLabels[0].text = num.ToString();
		}
	}

	private IEnumerator PlayDelayAnim(int num)
	{
		float time = 1.11f;
		IEnumerator wait = Util.WaitForRealTime(time);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		this.centerAnimator.SetTrigger(num.ToString());
		yield break;
	}

	private enum Type
	{
		RightDown,
		Middle
	}

	public enum MiddleType
	{
		None,
		You,
		Others,
		Enemy
	}
}
