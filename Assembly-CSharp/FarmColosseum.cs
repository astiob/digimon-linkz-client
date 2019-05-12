using Facility;
using Master;
using System;
using System.Collections;
using UnityEngine;

public class FarmColosseum : MonoBehaviour
{
	public bool EnableTouch = true;

	private ConstructionName constructionName;

	[SerializeField]
	private GameObject[] colosseums;

	private float timer;

	private ColosseumUtil colosseumUtil;

	public void Awake()
	{
		base.gameObject.tag = "Farm.Other";
		FarmRoot.Instance.Input.AddTouchEndEvent(new Action<InputControll, bool>(this.OnTouchUp));
		this.colosseumUtil = new ColosseumUtil();
	}

	private void Update()
	{
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			this.timer = 1f;
			bool flag = this.colosseumUtil.isOpen && DataMng.Instance().IsReleaseColosseum;
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
				if (this.colosseumUtil.isOpenAllDay)
				{
					this.CreateSignalColosseumEvent();
				}
			}
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
			GUIMain.ShowCommonDialog(null, "CMD_PvPTop", null);
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
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
}
