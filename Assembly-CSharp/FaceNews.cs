using System;
using UnityEngine;

public class FaceNews : GUICollider
{
	public GameObject BadgeObject;

	[SerializeField]
	private GameObject newsIcon;

	private int counter;

	private readonly int maxCounter = 10;

	private float rotateZ = 7f;

	private readonly float time = 0.01f;

	[SerializeField]
	private BoxCollider myCollider;

	protected override void Awake()
	{
		ClassSingleton<FaceNewsAccessor>.Instance.faceNews = this;
		base.Awake();
	}

	public void SetBadgeOnly()
	{
		GameWebAPI.RespDataIN_InfoList respDataIN_InfoList = DataMng.Instance().RespDataIN_InfoList;
		if (respDataIN_InfoList != null)
		{
			foreach (GameWebAPI.RespDataIN_InfoList.InfoList infoList2 in respDataIN_InfoList.infoList)
			{
				if (infoList2.confirmationFlg == 0)
				{
					this.BadgeObject.SetActive(true);
					this.PlayAnim();
					return;
				}
			}
		}
		this.BadgeObject.SetActive(false);
		this.RunITween("DoNothing", true);
		this.newsIcon.transform.localRotation = Quaternion.identity;
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
		iTween.RotateTo(this.newsIcon, iTween.Hash(new object[]
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
		this.newsIcon.transform.localRotation = Quaternion.identity;
		this.RunITween("OnComplete", true);
	}

	public void EnableCollider(bool isEnable)
	{
		this.myCollider.enabled = isEnable;
	}
}
