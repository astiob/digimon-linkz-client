using System;
using UnityEngine;

public class FaceMission : GUICollider
{
	public const string tapFlgFile = "MissionView2";

	private int counter;

	private readonly int maxCounter = 10;

	private float rotateZ = 7f;

	private readonly float time = 0.01f;

	public string lastClickTime = string.Empty;

	public bool dayChangeflg;

	[SerializeField]
	private GameObject floppy;

	[SerializeField]
	private UISprite buttonSprite;

	[SerializeField]
	private BoxCollider myCollider;

	[SerializeField]
	private GameObject effectBeginner;

	protected override void Awake()
	{
		ClassSingleton<FaceMissionAccessor>.Instance.faceMission = this;
		base.Awake();
	}

	public void SetBadge(bool showParticle = true)
	{
		GameWebAPI.RespDataMP_MyPage respDataMP_MyPage = DataMng.Instance().RespDataMP_MyPage;
		if (respDataMP_MyPage == null || respDataMP_MyPage.userNewsCountList == null)
		{
			return;
		}
		int num = respDataMP_MyPage.userNewsCountList.missionNewCount;
		int num2 = respDataMP_MyPage.userNewsCountList.missionRewardCount;
		int beginnerMissionNewCount = respDataMP_MyPage.userNewsCountList.beginnerMissionNewCount;
		int beginnerMissionRewardCount = respDataMP_MyPage.userNewsCountList.beginnerMissionRewardCount;
		num += beginnerMissionNewCount;
		num2 += beginnerMissionRewardCount;
		if (0 < num || this.dayChangeflg || 0 < num2)
		{
			this.SetActiveExclamationMark(true);
		}
		if (showParticle)
		{
			this.ShowParticleMissionIcon(beginnerMissionNewCount, beginnerMissionRewardCount);
		}
	}

	public void SetParticleMissionIcon()
	{
		GameWebAPI.RespDataMP_MyPage respDataMP_MyPage = DataMng.Instance().RespDataMP_MyPage;
		if (respDataMP_MyPage != null && respDataMP_MyPage.userNewsCountList != null)
		{
			int beginnerMissionNewCount = respDataMP_MyPage.userNewsCountList.beginnerMissionNewCount;
			int beginnerMissionRewardCount = respDataMP_MyPage.userNewsCountList.beginnerMissionRewardCount;
			this.ShowParticleMissionIcon(beginnerMissionNewCount, beginnerMissionRewardCount);
		}
	}

	private void ShowParticleMissionIcon(int beginnerMissionNewCount, int beginnerMissionRewardCount)
	{
		bool active = beginnerMissionNewCount > 0 || beginnerMissionRewardCount > 0;
		this.effectBeginner.SetActive(active);
	}

	public void ResetBadge()
	{
		APIRequestTask task = DataMng.Instance().RequestMyPageData(true);
		base.StartCoroutine(task.Run(delegate
		{
			this.SetBadge(true);
		}, null, null));
	}

	public void EnableCollider(bool isEnable)
	{
		this.myCollider.enabled = isEnable;
	}

	private void SetActiveExclamationMark(bool active)
	{
		this.buttonSprite.gameObject.SetActive(active);
		if (!active)
		{
			this.floppy.transform.localRotation = Quaternion.identity;
			this.RunITween("DoNothing", true);
		}
		else
		{
			this.PlayAnim();
		}
	}

	private void DoNothing()
	{
	}

	private void PlayAnim()
	{
		this.RunITween("OnComplete", false);
	}

	private void RunITween(string compFunc, bool isStop = false)
	{
		iTween.RotateTo(this.floppy, iTween.Hash(new object[]
		{
			"z",
			(!isStop) ? this.rotateZ : 0f,
			"time",
			(!isStop) ? this.time : 3f,
			"islocal",
			true,
			"oncomplete",
			compFunc,
			"oncompletetarget",
			base.gameObject
		}));
	}

	private void OnComplete()
	{
		this.counter++;
		this.rotateZ *= -1f;
		string compFunc = "OnComplete";
		if (this.counter >= this.maxCounter)
		{
			compFunc = "OnCompleteStop";
		}
		this.RunITween(compFunc, false);
	}

	private void OnCompleteStop()
	{
		this.counter = 0;
		this.floppy.transform.localRotation = Quaternion.identity;
		this.RunITween("OnComplete", true);
	}

	public void MissionTapCheck()
	{
		this.lastClickTime = PlayerPrefs.GetString("MissionView2", string.Empty);
		string a = ServerDateTime.Now.ToString("MM/dd/yyyy");
		if (a != this.lastClickTime)
		{
			this.dayChangeflg = true;
		}
	}

	public void MissionTapSave()
	{
		PlayerPrefs.SetString("MissionView2", ServerDateTime.Now.ToString("MM/dd/yyyy"));
		this.dayChangeflg = false;
	}
}
