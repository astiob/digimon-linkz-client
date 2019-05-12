using Master;
using System;
using System.Collections;
using UnityEngine;

public sealed class UserStamina : MonoBehaviour
{
	private const int RECOVERY_TIME = 180;

	[SerializeField]
	private UILabel point;

	[SerializeField]
	private UILabel recoverTime;

	[SerializeField]
	private UIProgressBar gauge;

	[SerializeField]
	private UIProgressBar overPointGauge;

	private int countDown;

	private Coroutine countDownCoroutine;

	public void SetMode(UserStamina.Mode mode)
	{
	}

	public void RefreshParams()
	{
		this.SetRecoverTime();
		this.SetStaminaDetails();
		if (0 < this.countDown && this.countDownCoroutine == null)
		{
			this.countDownCoroutine = AppCoroutine.Start(this.UpdateRecoverTime(), true);
		}
	}

	private void OnDisable()
	{
		if (this.countDownCoroutine != null)
		{
		}
	}

	private void OnDestroy()
	{
		if (this.countDownCoroutine != null)
		{
			AppCoroutine.Stop(this.countDownCoroutine, true);
			this.countDownCoroutine = null;
		}
	}

	private IEnumerator UpdateRecoverTime()
	{
		while (0 < this.countDown)
		{
			this.SetRecoverTime();
			this.SetStaminaDetails();
			yield return new WaitForSeconds(1f);
		}
		this.countDownCoroutine = null;
		yield break;
	}

	private void SetRecoverTime()
	{
		TimeSpan timeSpan = ServerDateTime.Now - Singleton<UserDataMng>.Instance.playerStaminaBaseTime;
		this.countDown = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.recovery - (int)timeSpan.TotalSeconds;
		if (0 < this.countDown)
		{
			this.recoverTime.text = TimeUtility.ToStaminaRecoveryTime(this.countDown);
		}
		else
		{
			this.recoverTime.text = StringMaster.GetString("StaminaFull");
			this.countDown = 0;
		}
	}

	private void SetStaminaDetails()
	{
		int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.staminaMax);
		int num2 = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina;
		if (num2 > num)
		{
			this.gauge.value = 1f;
			this.overPointGauge.value = (float)(num2 - num) / (float)num;
		}
		else
		{
			int num3 = this.countDown / 180;
			if (0 < this.countDown % 180)
			{
				num3++;
			}
			num2 = num - num3;
			this.gauge.value = Mathf.Clamp01((float)num2 / (float)num);
			this.overPointGauge.value = 0f;
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina = num2;
		}
		if (num2 > num)
		{
			this.point.text = string.Format(StringMaster.GetString("StaminaOverPoint"), num2, num);
		}
		else
		{
			this.point.text = string.Format(StringMaster.GetString("SystemFraction"), num2, num);
		}
	}

	public enum Mode
	{
		QUEST
	}
}
