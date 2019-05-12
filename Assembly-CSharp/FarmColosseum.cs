using Facility;
using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarmColosseum : MonoBehaviour
{
	public bool EnableTouch = true;

	private ConstructionName constructionName;

	[SerializeField]
	private GameObject[] colosseums;

	[SerializeField]
	private GameObject signalPresent;

	private GameObject instantSignalPresent;

	private List<FarmColosseum.Schedule> scheduleList = new List<FarmColosseum.Schedule>();

	public void Awake()
	{
		base.gameObject.tag = "Farm.Other";
		FarmRoot.Instance.Input.AddTouchEndEvent(new Action<InputControll, bool>(this.OnTouchUp));
		this.instantSignalPresent = UnityEngine.Object.Instantiate<GameObject>(this.signalPresent);
	}

	private void OnDestroy()
	{
		if (this.instantSignalPresent != null)
		{
			UnityEngine.Object.Destroy(this.instantSignalPresent);
			this.instantSignalPresent = null;
		}
	}

	private void Update()
	{
		bool flag = this.IsOpen() && DataMng.Instance().IsReleaseColosseum;
		if (this.colosseums[0].activeSelf != flag)
		{
			this.colosseums[0].SetActive(flag);
		}
		if (this.colosseums[1].activeSelf != !flag)
		{
			this.colosseums[1].SetActive(!flag);
		}
		if (this.constructionName == null && FarmRoot.Instance.farmUI != null)
		{
			GameObject gameObject = GUIManager.LoadCommonGUI("Farm/ConstructionName", FarmRoot.Instance.farmUI.gameObject);
			gameObject.name = "FacilityNamePlate_Colosseum";
			this.constructionName = gameObject.GetComponent<ConstructionName>();
			this.constructionName.farmObject = base.gameObject;
			if (flag && DataMng.Instance().RespData_ColosseumInfo.openAllDay > 0)
			{
				this.CreateSignalColosseumEvent();
			}
		}
		GameWebAPI.RespDataCL_GetColosseumReward respData_ColosseumReward = DataMng.Instance().RespData_ColosseumReward;
		bool flag2 = respData_ColosseumReward != null && respData_ColosseumReward.rewardList != null;
		Vector3 position = FarmRoot.Instance.Camera.WorldToScreenPoint(base.gameObject.transform.position);
		Vector3 position2 = GUIManager.gUICamera.ScreenToWorldPoint(position);
		position2.z = 0f;
		this.instantSignalPresent.transform.position = position2;
		this.instantSignalPresent.transform.localPosition += new Vector3(0f, 20f, 0f);
		if (this.instantSignalPresent.activeSelf != flag2)
		{
			this.instantSignalPresent.SetActive(flag2);
		}
	}

	private void OnTouchUp(InputControll inputControll, bool isDraged)
	{
		if (!this.EnableTouch || isDraged || inputControll.rayHitObjectType != InputControll.TouchObjectType.OTHER || inputControll.rayHitColliderObject != base.gameObject || FarmRoot.Instance.IsVisitFriendFarm)
		{
			return;
		}
		base.StartCoroutine(this.TouchUp());
	}

	private IEnumerator TouchUp()
	{
		GUIMain.BarrierON(null);
		SoundMng.Instance().TryPlaySE("SEInternal/Farm/se_203", 0f, false, true, null, -1);
		Animator animator = base.GetComponent<Animator>();
		if (null != animator && null == animator.runtimeAnimatorController)
		{
			animator.runtimeAnimatorController = FarmDataManager.FacilityAnimator;
			animator.enabled = true;
		}
		yield return base.StartCoroutine(FarmObjectAnimation.PlayAnimation(base.gameObject, FacilityAnimationID.SELECT));
		GUIMain.BarrierOFF();
		FarmColosseum.ShowPvPTop();
		yield break;
	}

	public static void ShowPvPTop()
	{
		if (DataMng.Instance().IsReleaseColosseum && DataMng.Instance().RespData_ColosseumInfo.colosseumId > 0)
		{
			GUIMain.ShowCommonDialog(null, "CMD_PvPTop");
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ColosseumTitle");
			if (DataMng.Instance().IsReleaseColosseum)
			{
				cmd_ModalMessage.Info = StringMaster.GetString("ColosseumCloseTime");
			}
			else
			{
				cmd_ModalMessage.Info = StringMaster.GetString("ColosseumLimit");
			}
		}
	}

	public void StartOpenAnimation(Action callback)
	{
		base.StartCoroutine(this.OpenAnimation(callback));
	}

	private IEnumerator OpenAnimation(Action callback)
	{
		GUICameraControll farmCamera = FarmRoot.Instance.Camera.GetComponent<GUICameraControll>();
		Vector3 position = base.transform.position;
		position.y -= FarmRoot.Instance.gameObject.transform.localPosition.y;
		yield return base.StartCoroutine(farmCamera.MoveCameraToLookAtPoint(position, 0.2f));
		bool isEffectEnd = false;
		EffectAnimatorObserver effect = FarmRoot.Instance.GetBuildCompleteEffect(base.transform);
		if (null != effect)
		{
			EffectAnimatorEventTime eventTime = effect.GetComponent<EffectAnimatorEventTime>();
			eventTime.SetEvent(0, delegate
			{
				isEffectEnd = true;
			});
			effect.Play();
		}
		while (!isEffectEnd)
		{
			yield return null;
		}
		if (callback != null)
		{
			callback();
		}
		yield break;
	}

	private bool IsOpen()
	{
		GameWebAPI.RespData_ColosseumInfoLogic respData_ColosseumInfo = DataMng.Instance().RespData_ColosseumInfo;
		if (respData_ColosseumInfo == null || respData_ColosseumInfo.colosseumId == 0)
		{
			this.scheduleList.Clear();
			return false;
		}
		if (DataMng.Instance().RespData_ColosseumInfo.openAllDay > 0)
		{
			return true;
		}
		if (this.scheduleList.Count<FarmColosseum.Schedule>() == 0)
		{
			GameWebAPI.RespDataMA_ColosseumTimeScheduleM respDataMA_ColosseumTimeScheduleMaster = MasterDataMng.Instance().RespDataMA_ColosseumTimeScheduleMaster;
			if (respDataMA_ColosseumTimeScheduleMaster == null)
			{
				return false;
			}
			string b = respData_ColosseumInfo.colosseumId.ToString();
			foreach (GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule colosseumTimeSchedule in respDataMA_ColosseumTimeScheduleMaster.colosseumTimeScheduleM)
			{
				if (colosseumTimeSchedule.colosseumId == b)
				{
					FarmColosseum.Schedule item = new FarmColosseum.Schedule
					{
						start = DateTime.Parse(colosseumTimeSchedule.startHour),
						end = DateTime.Parse(colosseumTimeSchedule.endHour)
					};
					this.scheduleList.Add(item);
				}
			}
		}
		foreach (FarmColosseum.Schedule schedule in this.scheduleList)
		{
			if (schedule.start < ServerDateTime.Now && ServerDateTime.Now < schedule.end)
			{
				return true;
			}
		}
		return false;
	}

	public void CreateSignalColosseumEvent()
	{
		if (null != this.constructionName)
		{
			SignalColosseumEvent[] componentsInChildren = this.constructionName.GetComponentsInChildren<SignalColosseumEvent>(true);
			if (componentsInChildren != null && 0 < componentsInChildren.Length)
			{
				componentsInChildren[0].SetDisplay(true);
			}
			else
			{
				GameObject gameObject = GUIManager.LoadCommonGUI("Farm/SignalColosseumEvent", this.constructionName.gameObject);
				if (gameObject != null)
				{
					gameObject.name = "SignalColosseumEvent";
					SignalColosseumEvent component = gameObject.GetComponent<SignalColosseumEvent>();
					component.SetNamePlate(this.constructionName);
					component.SetDisplay(true);
				}
			}
		}
	}

	private struct Schedule
	{
		public DateTime start;

		public DateTime end;
	}
}
