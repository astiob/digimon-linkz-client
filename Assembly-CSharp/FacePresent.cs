using System;
using UnityEngine;

public class FacePresent : GUICollider
{
	protected GameWebAPI.RespDataMP_MyPage mypageData;

	public GameObject BadgeObject;

	[SerializeField]
	private GameObject presentIcon;

	private int counter;

	private readonly int maxCounter = 10;

	private float rotateZ = 7f;

	private readonly float time = 0.01f;

	[SerializeField]
	private BoxCollider myCollider;

	protected override void Awake()
	{
		ClassSingleton<FacePresentAccessor>.Instance.facePresent = this;
		base.Awake();
	}

	public void SetBadgeOnly()
	{
		this.mypageData = DataMng.Instance().RespDataMP_MyPage;
		if (this.mypageData != null && this.mypageData.userNewsCountList != null)
		{
			if (this.mypageData.userNewsCountList.prize != null && this.mypageData.userNewsCountList.prize != "0")
			{
				this.BadgeObject.SetActive(true);
				this.PlayAnim();
			}
			else
			{
				this.BadgeObject.SetActive(false);
				this.RunITween("DoNothing", true);
				this.presentIcon.transform.localRotation = Quaternion.identity;
			}
		}
	}

	public void SetBadgeAndLoad()
	{
		APIRequestTask task = DataMng.Instance().RequestMyPageData(true);
		base.StartCoroutine(task.Run(new Action(this.SetBadgeOnly), null, null));
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
		iTween.RotateTo(this.presentIcon, iTween.Hash(new object[]
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
		this.presentIcon.transform.localRotation = Quaternion.identity;
		this.RunITween("OnComplete", true);
	}

	public void EnableCollider(bool isEnable)
	{
		this.myCollider.enabled = isEnable;
	}
}
