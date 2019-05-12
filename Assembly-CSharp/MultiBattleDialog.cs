using Master;
using System;
using System.Collections;
using UnityEngine;

public class MultiBattleDialog : MonoBehaviour
{
	private Action failedActionCallback;

	private int nowTime = 10;

	[SerializeField]
	[Header("内容ラベル")]
	private UILabel messageLabel;

	[SerializeField]
	[Header("ボタンのテキストラベル")]
	private UILabel buttonTextLabel;

	[SerializeField]
	[Header("ボタン無し内容ラベル")]
	private UILabel noButtonMessageLabel;

	[SerializeField]
	[Header("ボタン")]
	private UIButton button;

	[Header("スキン")]
	[SerializeField]
	private UIComponentSkinner uiCompornentSkinner;

	[Header("背景オブジェクト")]
	[SerializeField]
	private GameObject[] bg;

	[SerializeField]
	[Header("閉じるローカライズ")]
	private UILabel closeLocalize;

	private int failedLeftTime;

	private Coroutine countDownCoroutine;

	private Coroutine failedCoroutine;

	private bool _isShowBg = true;

	public Action callBackAction { private get; set; }

	public int maxTime { private get; set; }

	public bool isFailed
	{
		get
		{
			return this.failedLeftTime <= 0;
		}
	}

	public int BATTLE_TIMEOUT_TIME
	{
		get
		{
			if (!BattleStateManager.current.onServerConnect)
			{
				return 15;
			}
			if (BattleStateManager.current.battleMode == BattleMode.Multi)
			{
				return ConstValue.MULTI_BATTLE_TIMEOUT_TIME;
			}
			if (BattleStateManager.current.battleMode == BattleMode.PvP)
			{
				return ConstValue.PVP_BATTLE_TIMEOUT_TIME;
			}
			return 15;
		}
	}

	private int OPPONENT_BATTLE_TIMEOUT_TIME
	{
		get
		{
			if (!BattleStateManager.current.onServerConnect)
			{
				return 30;
			}
			if (BattleStateManager.current.battleMode != BattleMode.PvP)
			{
				global::Debug.LogError("ありえない.");
				return 30;
			}
			if (ConstValue.PVP_BATTLE_ENEMY_RECOVER_TIME <= 0)
			{
				global::Debug.LogError("マスターデータがおかしいようです.");
				return 30;
			}
			return ConstValue.PVP_BATTLE_ENEMY_RECOVER_TIME;
		}
	}

	public bool IsBlockNewDialog { get; set; }

	public bool isShowDialog
	{
		get
		{
			return this.uiCompornentSkinner.currentSkin != 0;
		}
	}

	public bool isShowBg
	{
		get
		{
			return this._isShowBg;
		}
		set
		{
			this._isShowBg = value;
			foreach (GameObject go in this.bg)
			{
				NGUITools.SetActiveSelf(go, this._isShowBg);
			}
		}
	}

	private void Awake()
	{
		this.failedLeftTime = this.BATTLE_TIMEOUT_TIME;
		this.closeLocalize.text = StringMaster.GetString("SystemButtonClose");
	}

	public bool IsAlreadyOpen()
	{
		return this.uiCompornentSkinner.currentSkin == 2;
	}

	public void SetSkin(int skinIndex)
	{
		if (skinIndex == 0)
		{
			if (this.countDownCoroutine != null)
			{
				base.StopCoroutine(this.countDownCoroutine);
				this.countDownCoroutine = null;
			}
			if (this.failedCoroutine != null)
			{
				base.StopCoroutine(this.failedCoroutine);
				this.failedCoroutine = null;
			}
		}
		else if (this.maxTime > 0)
		{
			this.StartTimer();
		}
		this.uiCompornentSkinner.SetSkins(skinIndex);
	}

	public void StartFailedTimer(string waitingConnectionFormat, Action aFailedActionCallback, bool isEnemy = false)
	{
		if (this.failedCoroutine != null)
		{
			base.StopCoroutine(this.failedCoroutine);
			this.failedCoroutine = null;
		}
		this.failedActionCallback = aFailedActionCallback;
		if (isEnemy)
		{
			this.failedLeftTime = this.OPPONENT_BATTLE_TIMEOUT_TIME;
		}
		else
		{
			this.failedLeftTime = this.BATTLE_TIMEOUT_TIME;
		}
		string text = string.Format(waitingConnectionFormat, this.failedLeftTime);
		this.messageLabel.text = string.Empty;
		this.noButtonMessageLabel.text = text;
		this.SetSkin(2);
		this.failedCoroutine = base.StartCoroutine(this.RunFailedCountDown(waitingConnectionFormat, isEnemy));
	}

	private IEnumerator RunFailedCountDown(string format, bool isEnemy = false)
	{
		for (;;)
		{
			yield return new WaitForEndOfFrame();
			IEnumerator wait = Util.WaitForRealTime(1f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			if (this.failedLeftTime == 0)
			{
				break;
			}
			this.failedLeftTime--;
			if (isEnemy)
			{
				this.failedLeftTime = Mathf.Clamp(this.failedLeftTime, 0, this.OPPONENT_BATTLE_TIMEOUT_TIME);
			}
			else
			{
				this.failedLeftTime = Mathf.Clamp(this.failedLeftTime, 0, this.BATTLE_TIMEOUT_TIME);
			}
			string message = string.Format(format, this.failedLeftTime);
			this.noButtonMessageLabel.text = message;
		}
		this.failedActionCallback();
		yield break;
		yield break;
	}

	public void SetMessage(string message, string buttonText = "")
	{
		this.messageLabel.text = message;
		this.noButtonMessageLabel.text = message;
		this.buttonTextLabel.text = buttonText;
	}

	private void OnButton()
	{
		if (this.callBackAction != null)
		{
			this.callBackAction();
		}
	}

	private void StartTimer()
	{
		if (this.failedCoroutine != null)
		{
			base.StopCoroutine(this.failedCoroutine);
			this.failedCoroutine = null;
		}
		this.nowTime = this.maxTime;
		if (base.gameObject.activeInHierarchy)
		{
			this.countDownCoroutine = base.StartCoroutine(this.RunCountDown());
		}
	}

	private IEnumerator RunCountDown()
	{
		do
		{
			IEnumerator wait = Util.WaitForRealTime(1f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			this.nowTime--;
		}
		while (this.nowTime >= 0);
		if (this.callBackAction == null)
		{
			global::Debug.LogErrorFormat("{0}ゲームオブジェクトの{1}の{2}変数にコールバック登録お願いします.", new object[]
			{
				base.name,
				base.GetType(),
				"icallBackAction"
			});
		}
		else
		{
			this.callBackAction();
		}
		yield break;
		yield break;
	}

	public enum Skin
	{
		None,
		Button,
		NoButton
	}
}
