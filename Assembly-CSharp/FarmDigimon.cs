using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FarmDigimon : MonoBehaviour
{
	private GameObject digimon;

	private FarmDigimonManager manager;

	private FarmDigimonAI farmDigimonAI;

	private FarmDigimonAction farmDigimonAction;

	public string userMonsterID;

	public DateTime memoryNextTime;

	private bool isInitialized;

	private bool isActive;

	private GameObject friendshipUpEfect;

	private float DigimonSizeY;

	private FarmDigimon.ActionState actionState;

	private Func<IEnumerator>[] stateRoutines;

	private FarmDigimon.CancelState cancelState;

	private Action[] cancelRoutines;

	private IEnumerator hitcheckEnumerator;

	private IEnumerator mainEnumerator;

	private IEnumerator actionEnumerator;

	private IEnumerator actionSubEnumerator;

	private Coroutine actionSubCoroutine;

	private IEnumerator cancelEnumerator;

	private MonsterData monsterData;

	private MonsterData beforeMonsterData;

	public bool IsFriendShipUp = true;

	private GameWebAPI.RespDataMN_Friendship digimonFriendShip;

	[SerializeField]
	private GameObject friendshipUpPref;

	[SerializeField]
	private GameObject TapfaceMark;

	private GameObject faceMark;

	private bool isSetUpBillBoard;

	private List<Material> matList;

	private List<float> valueList;

	private Camera farmCamera;

	private float camAveregeLen = 23f;

	public FarmDigimonAI.ActionID ActionID
	{
		get
		{
			return this.farmDigimonAI.GetActionParam().actionID;
		}
	}

	private Func<IEnumerator> StateRoutine
	{
		get
		{
			return this.stateRoutines[(int)this.actionState];
		}
	}

	private Action CancelRoutine
	{
		get
		{
			return this.cancelRoutines[(int)this.actionState];
		}
	}

	private void Awake()
	{
		this.stateRoutines = new Func<IEnumerator>[]
		{
			null,
			new Func<IEnumerator>(this.WaitAppearance),
			new Func<IEnumerator>(this.Thinking),
			new Func<IEnumerator>(this.Act),
			new Func<IEnumerator>(this.WaitEndAction)
		};
		this.cancelRoutines = new Action[]
		{
			null,
			new Action(this.CancelAppearance),
			new Action(this.CancelThinking),
			new Action(this.CancelAct),
			new Action(this.CancelWait)
		};
		this.manager = base.GetComponentInParent<FarmDigimonManager>();
		this.farmDigimonAI = base.GetComponent<FarmDigimonAI>();
		this.farmDigimonAction = base.GetComponent<FarmDigimonAction>();
	}

	public IEnumerator CreateDigimon(string userMonsterId)
	{
		MonsterData monsterData = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterId, false);
		yield return base.StartCoroutine(this.CreateDigimon(monsterData));
		yield break;
	}

	public IEnumerator CreateDigimon(MonsterData monsterData)
	{
		this.monsterData = monsterData;
		if (monsterData.userMonster != null)
		{
			this.userMonsterID = monsterData.userMonster.userMonsterId;
		}
		this.memoryNextTime = DateTime.MinValue;
		yield return base.StartCoroutine(this.LoadDigimon(monsterData.monsterMG.monsterGroupId));
		this.ChooseAI();
		this.hitcheckEnumerator = this.HitCheckBuild();
		base.StartCoroutine(this.hitcheckEnumerator);
		FarmRoot.Instance.Input.AddTouchEndEvent(new Action<InputControll, bool>(this.OnTouchUp));
		this.InitOutline();
		this.isInitialized = true;
		yield break;
	}

	public void SetActiveDigimon(bool value)
	{
		if (value)
		{
			this.StartAction();
		}
		else
		{
			base.StartCoroutine(this.EndAction());
		}
		if (this.digimon != null)
		{
			this.digimon.SetActive(value);
		}
	}

	public void DeleteDigimon()
	{
		this.isInitialized = false;
		this.isActive = false;
		this.userMonsterID = string.Empty;
		this.actionState = FarmDigimon.ActionState.NONE;
		this.cancelState = FarmDigimon.CancelState.NONE;
		FarmRoot.Instance.Input.RemoveTouchEndEvent(new Action<InputControll, bool>(this.OnTouchUp));
		if (this.hitcheckEnumerator != null)
		{
			base.StopCoroutine(this.hitcheckEnumerator);
			this.hitcheckEnumerator = null;
		}
		if (this.mainEnumerator != null)
		{
			base.StopCoroutine(this.mainEnumerator);
			this.mainEnumerator = null;
		}
		if (this.actionEnumerator != null)
		{
			base.StopCoroutine(this.actionEnumerator);
			this.actionEnumerator = null;
		}
		this.StopSubRoutine();
		this.farmDigimonAI.ClearActionParam();
		this.farmDigimonAction.StopAction();
		if (null != this.digimon)
		{
			UnityEngine.Object.Destroy(this.digimon);
			this.digimon = null;
		}
	}

	private IEnumerator LoadDigimon(string monsterGroupID)
	{
		GameObject resource = AssetDataMng.Instance().LoadObject("Characters/" + monsterGroupID + "/prefab", null, true) as GameObject;
		yield return null;
		this.digimon = UnityEngine.Object.Instantiate<GameObject>(resource);
		CharacterParams param = this.digimon.GetComponent<CharacterParams>();
		param.SetShadowObject();
		this.digimon.SetActive(false);
		yield return null;
		this.digimon.transform.parent = base.transform;
		this.digimon.transform.localPosition = Vector3.zero;
		this.digimon.transform.localRotation = Quaternion.identity;
		this.digimon.transform.localScale = Vector3.one;
		this.digimon.tag = "Farm.Chara";
		resource = null;
		Resources.UnloadUnusedAssets();
		yield break;
	}

	private void ChooseAI()
	{
		this.farmDigimonAI.SetAI_ID(FarmDigimonAI.AI_ID.NORMAL);
	}

	private IEnumerator HitCheckBuild()
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		FarmField farmField = farmRoot.Field;
		List<FarmGrid.Grid> grids = farmField.GetField().grids;
		while (base.gameObject.activeSelf)
		{
			while (!this.digimon.activeSelf)
			{
				yield return new WaitForSeconds(1f);
			}
			while (!this.CheckHitBuild(farmField.Grid, grids))
			{
				yield return new WaitForSeconds(0.5f);
			}
		}
		yield break;
	}

	private bool CheckHitBuild(FarmGrid farmGrid, List<FarmGrid.Grid> grids)
	{
		return false;
	}

	public IEnumerator Boot()
	{
		while (!this.isInitialized)
		{
			yield return null;
		}
		this.actionState = FarmDigimon.ActionState.APPEARANCE;
		this.UpdateOutline();
		this.StartAction();
		yield break;
	}

	private void Update()
	{
		this.UpdateOutline();
	}

	public void StartAction()
	{
		if (!this.isActive)
		{
			this.isActive = true;
			this.mainEnumerator = this.ActionMainRoutine();
			base.StartCoroutine(this.mainEnumerator);
		}
	}

	public IEnumerator EndAction()
	{
		if (this.isActive)
		{
			this.isActive = false;
			if (this.StateRoutine != null && this.actionEnumerator != null)
			{
				base.StopCoroutine(this.actionEnumerator);
				this.actionEnumerator = null;
				this.StopSubRoutine();
			}
			this.cancelState = FarmDigimon.CancelState.START;
			if (this.CancelRoutine != null)
			{
				this.CancelRoutine();
			}
			this.actionState = FarmDigimon.ActionState.NONE;
			base.StopCoroutine(this.mainEnumerator);
			this.mainEnumerator = null;
			while (this.cancelState != FarmDigimon.CancelState.END)
			{
				yield return null;
			}
			this.cancelEnumerator = null;
		}
		yield break;
	}

	public bool IsFinishedEndAction()
	{
		return this.cancelState == FarmDigimon.CancelState.NONE || FarmDigimon.CancelState.END == this.cancelState;
	}

	private IEnumerator ActionMainRoutine()
	{
		if (this.cancelState != FarmDigimon.CancelState.NONE)
		{
			while (this.cancelState != FarmDigimon.CancelState.END)
			{
				yield return new WaitForSeconds(1f);
			}
			this.cancelState = FarmDigimon.CancelState.NONE;
			if (this.actionState == FarmDigimon.ActionState.NONE)
			{
				this.actionState = (this.digimon.activeSelf ? FarmDigimon.ActionState.THINKING : FarmDigimon.ActionState.APPEARANCE);
			}
		}
		while (this.StateRoutine != null)
		{
			this.actionEnumerator = this.StateRoutine();
			yield return base.StartCoroutine(this.actionEnumerator);
		}
		yield break;
	}

	private IEnumerator WaitAppearance()
	{
		this.digimon.SetActive(true);
		this.StartIdleAnimation();
		this.actionSubEnumerator = this.farmDigimonAI.Appearance(this.digimon);
		this.actionSubCoroutine = base.StartCoroutine(this.actionSubEnumerator);
		yield return this.actionSubCoroutine;
		this.actionSubEnumerator = null;
		this.actionSubCoroutine = null;
		this.actionState = ((!this.digimon.activeSelf) ? FarmDigimon.ActionState.NONE : FarmDigimon.ActionState.THINKING);
		this.SetUpBillBoard();
		yield break;
	}

	private void CancelAppearance()
	{
		if (this.digimon.activeSelf)
		{
			this.cancelEnumerator = this.WaitRoutine(this.actionSubCoroutine, delegate
			{
				this.actionSubCoroutine = null;
				this.actionSubEnumerator = null;
				this.StartIdleAnimation();
				this.cancelState = FarmDigimon.CancelState.END;
			});
			base.StartCoroutine(this.cancelEnumerator);
		}
		else
		{
			this.cancelState = FarmDigimon.CancelState.END;
		}
	}

	private IEnumerator Thinking()
	{
		FarmDigimonAI.ActionID actionID = this.farmDigimonAI.ChooseAction();
		while (this.manager.FindAction(actionID))
		{
			yield return new WaitForSeconds(2f);
			actionID = this.farmDigimonAI.ChooseAction();
		}
		this.farmDigimonAI.CreateActionParam(actionID);
		this.actionState = FarmDigimon.ActionState.ACT;
		yield break;
	}

	private void CancelThinking()
	{
		this.ResetAction();
		this.StartIdleAnimation();
		this.cancelState = FarmDigimon.CancelState.END;
	}

	private void ResetAction()
	{
		this.farmDigimonAI.ClearActionParam();
		this.farmDigimonAction.StopAction();
	}

	private IEnumerator Act()
	{
		yield return null;
		this.actionState = FarmDigimon.ActionState.WAIT;
		yield break;
	}

	private void CancelAct()
	{
		this.cancelEnumerator = this.WaitRoutine(this.actionSubCoroutine, delegate
		{
			this.actionSubCoroutine = null;
			this.actionSubEnumerator = null;
			this.ResetAction();
			this.StartIdleAnimation();
			this.cancelState = FarmDigimon.CancelState.END;
		});
		base.StartCoroutine(this.cancelEnumerator);
	}

	private IEnumerator WaitEndAction()
	{
		FarmDigimonAI.ActionParam param = this.farmDigimonAI.GetActionParam();
		switch (param.actionID)
		{
		case FarmDigimonAI.ActionID.MEAT_FARM:
		case FarmDigimonAI.ActionID.STROLL:
		case FarmDigimonAI.ActionID.STROLL_FAST:
		case FarmDigimonAI.ActionID.CONSTRUCTION:
			if (param.pathGridIndexs != null)
			{
				CharacterParams characterParams = this.digimon.GetComponent<CharacterParams>();
				characterParams.PlayAnimation(CharacterAnimationType.move, SkillType.Attack, 0, null, null);
				this.actionSubEnumerator = this.farmDigimonAction.Walk();
				yield return base.StartCoroutine(this.actionSubEnumerator);
				this.actionSubEnumerator = null;
			}
			break;
		case FarmDigimonAI.ActionID.TOUCH_ACTION:
		{
			CharacterParams characterParams2 = this.digimon.GetComponent<CharacterParams>();
			characterParams2.PlayAnimation(CharacterAnimationType.move, SkillType.Attack, 0, null, null);
			this.actionSubEnumerator = this.farmDigimonAction.TouchAction();
			yield return base.StartCoroutine(this.actionSubEnumerator);
			this.actionSubEnumerator = null;
			break;
		}
		default:
		{
			CharacterParams characterParams3 = this.digimon.GetComponent<CharacterParams>();
			characterParams3.PlayRevivalAnimationSmooth();
			break;
		}
		}
		CharacterParams characterParams4 = this.digimon.GetComponent<CharacterParams>();
		characterParams4.PlayRevivalAnimationSmooth();
		yield return new WaitForSeconds(5f);
		this.farmDigimonAI.ClearActionParam();
		this.actionState = FarmDigimon.ActionState.THINKING;
		yield return null;
		yield break;
	}

	private void CancelWait()
	{
		this.StopSubRoutine();
		this.ResetAction();
		this.StartIdleAnimation();
		this.cancelState = FarmDigimon.CancelState.END;
	}

	private void StartIdleAnimation()
	{
		CharacterParams component = this.digimon.GetComponent<CharacterParams>();
		component.PlayIdleAnimation();
	}

	private IEnumerator WaitRoutine(Coroutine routine, Action finished)
	{
		if (routine != null)
		{
			yield return routine;
		}
		finished();
		yield break;
	}

	private void StopSubRoutine()
	{
		if (this.actionSubEnumerator != null)
		{
			base.StopCoroutine(this.actionSubEnumerator);
			this.actionSubCoroutine = null;
			this.actionSubEnumerator = null;
		}
	}

	private void OnTouchUp(InputControll inputControll, bool isDraged)
	{
		if (isDraged || inputControll.rayHitObjectType != InputControll.TouchObjectType.CHARA || inputControll.rayHitColliderObject != this.digimon || this.ActionID == FarmDigimonAI.ActionID.TOUCH_ACTION)
		{
			return;
		}
		base.StartCoroutine(this.TouchAction());
	}

	private IEnumerator TouchAction()
	{
		yield return base.StartCoroutine(this.EndAction());
		this.farmDigimonAI.CreateActionParam(FarmDigimonAI.ActionID.TOUCH_ACTION);
		this.actionState = FarmDigimon.ActionState.ACT;
		this.StartAction();
		if (this.IsFriendShipUp && DateTime.MinValue != this.memoryNextTime)
		{
			if ((this.memoryNextTime - ServerDateTime.Now).TotalSeconds <= 0.0 && this.memoryNextTime != DateTime.MinValue)
			{
				int maxFriendship = CommonSentenceData.MaxFriendshipValue(this.monsterData.monsterMG.growStep);
				int friendshipInt = int.Parse(this.monsterData.userMonster.friendship);
				if (friendshipInt < maxFriendship)
				{
					this.beforeMonsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.monsterData.userMonster.monsterId);
					this.beforeMonsterData.DuplicateUserMonster(this.monsterData.userMonster);
					this.UpFriendStatus();
				}
				else
				{
					this.PopBalloon(false);
				}
			}
			else
			{
				this.PopBalloon(false);
			}
		}
		yield break;
	}

	private void UpFriendStatus()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
		GameWebAPI.RequestMN_UserMonsterFriendship request = new GameWebAPI.RequestMN_UserMonsterFriendship
		{
			SetSendData = delegate(GameWebAPI.MN_Req_FriendshipStatus param)
			{
				param.userMonsterId = int.Parse(this.userMonsterID);
			},
			OnReceived = delegate(GameWebAPI.RespDataMN_Friendship response)
			{
				this.digimonFriendShip = response;
				this.memoryNextTime = ServerDateTime.Now.AddSeconds((double)response.nextTimeSec);
			}
		};
		base.StartCoroutine(request.Run(new Action(this.UpFriendStatusEnd), null, null));
	}

	private void UpFriendStatusEnd()
	{
		RestrictionInput.EndLoad();
		this.FriendshipUpEndPop();
	}

	private void FriendshipUpEndPop()
	{
		if (0 < this.digimonFriendShip.upFriendship)
		{
			this.PopBalloon(true);
			if (this.digimonFriendShip.upStatus != null)
			{
				if (this.digimonFriendShip.userMonster != null)
				{
					DataMng.Instance().SetUserMonster(this.digimonFriendShip.userMonster);
					this.monsterData.DuplicateUserMonster(this.digimonFriendShip.userMonster);
				}
				base.StartCoroutine(this.PopFriendshipUpStatus());
			}
			else
			{
				this.AddDigimonFriendship(this.digimonFriendShip.upFriendship);
			}
			this.PopFriendshipEfect();
		}
		else
		{
			this.PopBalloon(false);
		}
	}

	private void AddDigimonFriendship(int value)
	{
		string friendship = string.Empty;
		if (!string.IsNullOrEmpty(this.monsterData.userMonster.friendship))
		{
			friendship = (this.monsterData.userMonster.friendship.ToInt32() + value).ToString();
		}
		else
		{
			friendship = value.ToString();
		}
		this.monsterData.userMonster.friendship = friendship;
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster = DataMng.Instance().GetUserMonster(this.userMonsterID);
		if (userMonster != null)
		{
			userMonster.friendship = friendship;
		}
	}

	private IEnumerator PopFriendshipUpStatus()
	{
		this.faceMark.GetComponent<FriendshipUPFaceMark>().barrierOn = true;
		yield return new WaitForSeconds(1f);
		CMD_FriendshipStatusUP cd = GUIMain.ShowCommonDialog(null, "CMD_FriendshipStatusUP") as CMD_FriendshipStatusUP;
		cd.SetData(this.monsterData);
		cd.SetChangeData(this.beforeMonsterData);
		yield break;
	}

	private void PopBalloon(bool friendshipUpFlg = false)
	{
		if (this.DigimonSizeY == 0f)
		{
			this.DigimonSizeY = base.gameObject.GetComponentInChildren<CharacterParams>().RootToCenterDistance();
		}
		if (this.faceMark != null)
		{
			UnityEngine.Object.Destroy(this.faceMark);
		}
		this.faceMark = UnityEngine.Object.Instantiate<GameObject>(this.TapfaceMark);
		this.faceMark.transform.parent = Singleton<GUIMain>.Instance.transform;
		this.faceMark.GetComponent<FriendshipUPFaceMark>().farmObject = base.gameObject;
		FriendshipUPFaceMark component = this.faceMark.GetComponent<FriendshipUPFaceMark>();
		component.ChangeIcon(friendshipUpFlg, this.DigimonSizeY);
		if (friendshipUpFlg)
		{
			base.Invoke("PopFriendshipUpCount", 0.8f);
		}
	}

	private void PopFriendshipUpCount()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.friendshipUpPref);
		gameObject.transform.parent = Singleton<GUIMain>.Instance.transform;
		gameObject.GetComponent<FriendshipUP>().farmObject = base.gameObject;
		gameObject.GetComponent<FriendshipUP>().ViewFriendshipStatus(this.digimonFriendShip.upFriendship);
	}

	private void PopFriendshipEfect()
	{
		if (this.friendshipUpEfect != null)
		{
			UnityEngine.Object.Destroy(this.friendshipUpEfect);
		}
		this.friendshipUpEfect = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Cutscenes/NewFX4"));
		this.friendshipUpEfect.transform.parent = base.gameObject.transform.parent.gameObject.transform;
		this.friendshipUpEfect.transform.localPosition = base.gameObject.transform.localPosition;
	}

	private void SetUpBillBoard()
	{
		if (this.digimon != null && this.farmCamera != null && base.gameObject.activeSelf && !this.isSetUpBillBoard)
		{
			CharacterParams component = this.digimon.GetComponent<CharacterParams>();
			component.SetBillBoardCamera(this.farmCamera);
			this.isSetUpBillBoard = true;
		}
	}

	private void InitOutline()
	{
		List<SkinnedMeshRenderer> compoSMR = CommonRender3DRT.GetCompoSMR(base.gameObject);
		this.matList = CommonRender3DRT.GetAllMaterialsInSMRS(compoSMR);
		this.valueList = CommonRender3DRT.GetOutlineWidth(this.matList);
		this.farmCamera = FarmRoot.Instance.Camera;
	}

	private void UpdateOutline()
	{
		if (this.matList != null && this.valueList != null)
		{
			float num = 0.3f;
			float num2 = 0.1f;
			if (this.farmCamera != null)
			{
				num2 = this.farmCamera.orthographicSize;
			}
			num = num * num2 * 0.3f;
			float num3 = Vector3.Distance(base.gameObject.transform.localPosition, this.farmCamera.transform.localPosition);
			num = num / num3 * this.camAveregeLen;
			for (int i = 0; i < this.matList.Count; i++)
			{
				if (this.matList[i] != null)
				{
					float value = this.valueList[i] * num;
					this.matList[i].SetFloat("_OutlineWidth", value);
				}
			}
		}
	}

	private enum ActionState
	{
		NONE,
		APPEARANCE,
		THINKING,
		ACT,
		WAIT
	}

	private enum CancelState
	{
		NONE,
		START,
		END
	}
}
