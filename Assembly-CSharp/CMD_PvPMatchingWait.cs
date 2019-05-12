using Colosseum;
using Master;
using Monster;
using MultiBattle.Tools;
using PvP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMD_PvPMatchingWait : CMD
{
	public static CMD_PvPMatchingWait instance;

	private bool isOwner;

	private int randomSeed;

	private string myUserId;

	private string mockBattleEnemyUserCode;

	private Vector3 v3DPos;

	private MultiBattleData.PvPUserData[] pvpUserDatas;

	private string[] mySelectIndexIdList = new string[3];

	private string worldDungeonId;

	private int isMockBattle;

	private bool isRetry;

	private bool isPrepareBattle;

	private bool isReadyBattle;

	private bool isShowAlert;

	private int selectPartyRetryCount;

	private GameWebAPI.ColosseumUserStatus colosseumUserStatus;

	private IEnumerator pvpEnemyOnlineCheck;

	private bool IsPvPOnlineCheck;

	private List<int> TCPSendUserIdList = new List<int>();

	private bool pvpDataRetry;

	private float pausedTime;

	[Header("取り消しボタンコライダー")]
	[SerializeField]
	private BoxCollider coCancelBtn;

	[Header("取り消しボタンスプライト")]
	[SerializeField]
	private UISprite spCancelBtn;

	[SerializeField]
	[Header("取り消しボタンラベル")]
	private UILabel lbCancelBtn;

	[Header("モンスター表示")]
	[SerializeField]
	private PartsMatchingWaitMonsInfo monsInfo;

	[Header("マッチング中アニメオブジェクト")]
	[SerializeField]
	private GameObject goMatchingNowAnim;

	[SerializeField]
	[Header("マッチング完了アニメオブジェクト")]
	private GameObject goMatchingEndAnim;

	[SerializeField]
	[Header("マッチング完了エフェクト")]
	private Animator MatchingEffectAnimator;

	[SerializeField]
	private UITexture background;

	[Header("転送エフェクトの乗算色(アルファは０固定)")]
	[SerializeField]
	private Color renderTextureColor;

	[Header("キャラの勝利アニメを見せる時間（秒）")]
	[SerializeField]
	private float winAnimationWait;

	[Header("キャラが消えてから情報が出るまでの時間（秒）")]
	[SerializeField]
	private float transferWait;

	[Header("３対選択画面")]
	[SerializeField]
	private PVPPartySelect3 partySelect;

	[Header("演出チェック用")]
	[SerializeField]
	private bool debugAnimation;

	private Coroutine matchingFinishAnimation;

	private PvPVersusInfo6Icon versusInfo;

	private List<MonsterData> enemyMonsterDataList = new List<MonsterData>();

	private bool myMonsterDataSendCheck;

	private bool enemyMonsterDataGetCheck;

	private bool myPartyDataSendCheck;

	public int deckNum { get; set; }

	protected override void Awake()
	{
		base.Awake();
		CMD_PvPMatchingWait.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		Screen.sleepTimeout = -1;
		this.v3DPos = new Vector3(-1000f, 4000f, 0f);
		this.myUserId = DataMng.Instance().RespDataCM_Login.playerInfo.userId;
		this.mockBattleEnemyUserCode = ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode;
		this.isMockBattle = ((!(this.mockBattleEnemyUserCode == "0")) ? 1 : 0);
		this.lbCancelBtn.text = StringMaster.GetString("ColosseumCancel");
		base.SetOpendAction(delegate(int action)
		{
			base.StartCoroutine(this.InitColosseumStatus());
		});
		base.Show(f, sizeX, sizeY, aT);
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.returnVal == -1)
		{
			this.returnVal = 0;
			this.OnClickCancel();
		}
		else
		{
			MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			if (CMD_PartyEdit.instance != null)
			{
				CMD_PartyEdit.instance.ReloadAllCharacters(true);
				CMD_PartyEdit.instance.DispClips();
			}
			if (CMD_PvPMatchingWait.instance != null)
			{
				base.ClosePanel(animation);
			}
		}
	}

	protected override void OnDestroy()
	{
		Screen.sleepTimeout = -2;
		base.OnDestroy();
	}

	private void SetCancelBtn(bool isEnabled)
	{
		this.coCancelBtn.enabled = isEnabled;
		GUIManager.ExtBackKeyReady = isEnabled;
		if (isEnabled)
		{
			this.spCancelBtn.spriteName = "Common02_Btn_SupportRed";
			this.spCancelBtn.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			this.lbCancelBtn.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
		else
		{
			this.spCancelBtn.spriteName = "Common02_Btn_SupportWhite";
			this.spCancelBtn.color = new Color32(100, 100, 100, byte.MaxValue);
			this.lbCancelBtn.color = ConstValue.DEACTIVE_BUTTON_LABEL;
		}
	}

	private IEnumerator InitColosseumStatus()
	{
		bool isOpen = false;
		if (this.isMockBattle == 0)
		{
			GameWebAPI.RespDataMA_ColosseumM.Colosseum[] colosseumM = MasterDataMng.Instance().RespDataMA_ColosseumMaster.colosseumM.Where((GameWebAPI.RespDataMA_ColosseumM.Colosseum data) => DateTime.Parse(data.openTime) < ServerDateTime.Now && ServerDateTime.Now < DateTime.Parse(data.closeTime)).ToArray<GameWebAPI.RespDataMA_ColosseumM.Colosseum>();
			if (colosseumM.Length > 0)
			{
				isOpen = true;
				this.worldDungeonId = colosseumM[0].worldDungeonId;
				global::Debug.Log("通常戦での処理を開始します");
				this.colosseumUserStatus = CMD_PvPTop.Instance.ColosseumUserStatus;
			}
		}
		else
		{
			isOpen = true;
			this.worldDungeonId = "10001";
			global::Debug.Log("模擬戦での処理を開始します");
			GameWebAPI.ColosseumUserStatusLogic colosseumUserStatusLogic = new GameWebAPI.ColosseumUserStatusLogic();
			colosseumUserStatusLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumUserStatusLogic param)
			{
				param.target = "me";
				param.isMockBattle = 1;
			};
			colosseumUserStatusLogic.OnReceived = delegate(GameWebAPI.RespData_ColosseumUserStatusLogic resData)
			{
				PvPUtility.SetPvPTopNoticeCode(resData);
				this.colosseumUserStatus = resData.userStatus;
			};
			GameWebAPI.ColosseumUserStatusLogic request = colosseumUserStatusLogic;
			yield return base.StartCoroutine(request.Run(new Action(RestrictionInput.EndLoad), null, null));
		}
		if (isOpen)
		{
			List<MonsterData> monsterDeckList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
			string[] idList = new string[6];
			for (int i = 0; i < 6; i++)
			{
				if (monsterDeckList[i] != null)
				{
					idList[i] = monsterDeckList[i].GetMonster().userMonsterId;
				}
				else
				{
					idList[i] = "0";
				}
			}
			GameWebAPI.ColosseumDeckEditLogic request2 = new GameWebAPI.ColosseumDeckEditLogic
			{
				SetSendData = delegate(GameWebAPI.ReqData_ColosseumDeckEditLogic param)
				{
					param.deckNum = this.deckNum;
					param.isMockBattle = this.isMockBattle;
					param.userMonsterIdList = idList;
				},
				OnReceived = delegate(GameWebAPI.RespData_ColosseumDeckEditLogic response)
				{
					GameWebAPI.RespData_ColosseumDeckEditLogic data = response;
				}
			};
			yield return base.StartCoroutine(request2.RunOneTime(delegate()
			{
				RestrictionInput.EndLoad();
				this.AfterColosseumDeckEdit(data);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
				this.SetCancelBtn(true);
			}, null));
		}
		else
		{
			this.DispErrorModal(StringMaster.GetString("ColosseumCloseTime"), StringMaster.GetString("ColosseumGoTop"), false);
		}
		if (CMD_PartyEdit.instance != null)
		{
			CMD_PartyEdit.instance.ReloadAllCharacters(false);
		}
		this.CharacterShow();
		yield break;
	}

	private void AfterColosseumDeckEdit(GameWebAPI.RespData_ColosseumDeckEditLogic data)
	{
		if (data.resultCode == 1)
		{
			Singleton<TCPUtil>.Instance.PrepareTCPServer(new Action<string>(this.AfterPrepareTCPServer), "pvp");
		}
		else
		{
			this.DispErrorModal(StringMaster.GetString("AlertNetworkErrorTitle"), StringMaster.GetString("AlertNetworkErrorInfo"), false);
		}
	}

	public Vector3 Get3DPos()
	{
		return this.v3DPos;
	}

	private void CharacterShow()
	{
		List<MonsterData> colosseumDeckUserMonsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
		List<MonsterData> list = new List<MonsterData>();
		for (int i = 0; i < 3; i++)
		{
			MonsterData item = colosseumDeckUserMonsterList[UnityEngine.Random.Range(0, colosseumDeckUserMonsterList.Count)];
			list.Add(item);
			colosseumDeckUserMonsterList.Remove(item);
		}
		this.monsInfo.mdList = list;
		this.monsInfo.ShowGUI();
	}

	private void AfterPrepareTCPServer(string server)
	{
		this.SetCancelBtn(false);
		Singleton<TCPUtil>.Instance.MakeTCPClient();
		Singleton<TCPUtil>.Instance.SetAfterConnectTCPMethod(new Action<bool>(this.AfterConnectTCP));
		Singleton<TCPUtil>.Instance.SetTCPCallBackMethod(new Action<Dictionary<string, object>>(this.GetTCPReceponseData));
		Singleton<TCPUtil>.Instance.SetOnExitCallBackMethod(new Action(this.OnExitTCP));
		Singleton<TCPUtil>.Instance.SetExceptionMethod(new Action<short, string>(this.OnExceptionTCP));
		Singleton<TCPUtil>.Instance.SetRequestExceptionMethod(new Action<string, string>(this.OnRequestExceptionTCP));
		Singleton<TCPUtil>.Instance.ConnectTCPServerAsync(DataMng.Instance().RespDataCM_Login.playerInfo.userId);
	}

	private void AfterConnectTCP(bool result)
	{
		if (result)
		{
			this.SendTCPPvPMatching(true);
		}
		else
		{
			this.DispErrorModal(StringMaster.GetString("AlertNetworkErrorTitle"), StringMaster.GetString("AlertNetworkErrorInfo"), false);
		}
	}

	private void SendTCPPvPMatching(bool isRequest)
	{
		int act;
		if (isRequest)
		{
			global::Debug.Log("マッチング開始");
			if (this.isMockBattle == 1 && CMD_ChatTop.instance != null)
			{
				act = 2;
			}
			else
			{
				act = 1;
			}
		}
		else
		{
			global::Debug.Log("マッチング中断");
			act = 0;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		PvPMatching value = new PvPMatching
		{
			act = act,
			targetUserCode = this.mockBattleEnemyUserCode,
			isMockBattle = this.isMockBattle,
			uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
		};
		dictionary.Add("080106", value);
		Singleton<TCPUtil>.Instance.SendTCPRequest(dictionary, "activityList");
	}

	private void SendTCPPvPBattleStart()
	{
		int entranceType = 1;
		if (CMD_PvPTop.Instance != null && CMD_PvPTop.Instance.isExtraBattle)
		{
			entranceType = 2;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		PvPBattleStart value = new PvPBattleStart
		{
			isMockBattle = this.isMockBattle,
			entranceType = entranceType,
			uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
		};
		dictionary.Add("080108", value);
		Singleton<TCPUtil>.Instance.SendTCPRequest(dictionary, "activityList");
	}

	private void GetTCPReceponseData(Dictionary<string, object> arg)
	{
		if (arg.ContainsKey("080106"))
		{
			foreach (KeyValuePair<string, object> keyValuePair in arg)
			{
				Dictionary<object, object> dictionary = (Dictionary<object, object>)keyValuePair.Value;
				foreach (KeyValuePair<object, object> keyValuePair2 in dictionary)
				{
					if (keyValuePair2.Key.ToString() == "resultCode")
					{
						string text = keyValuePair2.Value.ToString();
						if (text == null)
						{
							goto IL_291;
						}
						if (CMD_PvPMatchingWait.<>f__switch$map34 == null)
						{
							CMD_PvPMatchingWait.<>f__switch$map34 = new Dictionary<string, int>(9)
							{
								{
									"0",
									0
								},
								{
									"4",
									0
								},
								{
									"1",
									1
								},
								{
									"2",
									2
								},
								{
									"3",
									3
								},
								{
									"5",
									4
								},
								{
									"6",
									5
								},
								{
									"7",
									6
								},
								{
									"93",
									7
								}
							};
						}
						int num;
						if (!CMD_PvPMatchingWait.<>f__switch$map34.TryGetValue(text, out num))
						{
							goto IL_291;
						}
						switch (num)
						{
						case 0:
							global::Debug.Log("マッチング待ち状態になりました >>" + keyValuePair2.Value.ToString());
							this.isOwner = true;
							this.SetCancelBtn(true);
							break;
						case 1:
							global::Debug.Log("マッチングしました");
							base.StartCoroutine(this.PrepareGoToBattleScene());
							break;
						case 2:
							global::Debug.Log("未エントリーです");
							this.DispErrorModal(StringMaster.GetString("ColosseumNotEntry"), StringMaster.GetString("ColosseumGoTop"), false);
							break;
						case 3:
							global::Debug.Log("マッチング取り下げました\u3000リトライ ? >>>" + this.isRetry);
							if (this.isRetry)
							{
								this.isRetry = false;
								this.SendTCPPvPMatching(true);
							}
							else
							{
								Singleton<TCPUtil>.Instance.TCPDisConnect(false);
								this.ClosePanel(true);
							}
							break;
						case 4:
							global::Debug.Log("既にマッチング済みです。");
							this.RetryMatching();
							break;
						case 5:
							global::Debug.Log("開催中のコロシアムがない");
							this.DispErrorModal(StringMaster.GetString("ColosseumCloseTime"), StringMaster.GetString("ColosseumGoTop"), false);
							break;
						case 6:
							global::Debug.Log("相手にブラックリスト登録されている");
							Singleton<TCPUtil>.Instance.TCPDisConnect(false);
							this.SetCancelBtn(true);
							break;
						case 7:
							global::Debug.Log("相手が依頼を取り下げ済み");
							this.DispErrorModal(StringMaster.GetString("ColosseumWithdraw"), StringMaster.GetString("ColosseumSelect"), true);
							break;
						default:
							goto IL_291;
						}
						continue;
						IL_291:
						global::Debug.Log("例外: " + keyValuePair2.Key.ToString() + " " + keyValuePair2.Value.ToString());
						this.DispErrorModal(keyValuePair2.Value.ToString());
					}
					else
					{
						global::Debug.Log("例外: " + keyValuePair2.Key.ToString() + " " + keyValuePair2.Value.ToString());
						this.DispErrorModal(keyValuePair2.Value.ToString());
					}
				}
			}
		}
		else if (arg.ContainsKey("080108"))
		{
			if (!this.isReadyBattle)
			{
				this.OnRecievedBattleStart(arg);
			}
		}
		else if (arg.ContainsKey("enemyData"))
		{
			this.OnRecievedBattlePvPEnemyData(arg);
		}
		else if (arg.ContainsKey("enemyDataReceive"))
		{
			this.OnRecievedEnemySendData(arg["enemyDataReceive"]);
		}
		else if (arg.ContainsKey("080110"))
		{
			this.OnRecievedOnlineCheck(arg["080110"]);
		}
	}

	private void OnRecievedEnemySendData(object messageObj)
	{
		this.myMonsterDataSendCheck = true;
		if (!this.isOwner)
		{
			this.randomSeed = MultiTools.GetValueByKey<int>(messageObj, "randomSeed");
		}
	}

	private void OnRecievedBattleStart(Dictionary<string, object> packet)
	{
		foreach (KeyValuePair<string, object> keyValuePair in packet)
		{
			Dictionary<object, object> dictionary = keyValuePair.Value as Dictionary<object, object>;
			if (dictionary != null)
			{
				foreach (KeyValuePair<object, object> keyValuePair2 in dictionary)
				{
					if (keyValuePair2.Key.ToString() == "resultCode")
					{
						string text = keyValuePair2.Value.ToString();
						if (text == null)
						{
							goto IL_13D;
						}
						if (CMD_PvPMatchingWait.<>f__switch$map35 == null)
						{
							CMD_PvPMatchingWait.<>f__switch$map35 = new Dictionary<string, int>(3)
							{
								{
									"1",
									0
								},
								{
									"3",
									0
								},
								{
									"2",
									1
								}
							};
						}
						int num;
						if (!CMD_PvPMatchingWait.<>f__switch$map35.TryGetValue(text, out num))
						{
							goto IL_13D;
						}
						if (num != 0)
						{
							if (num != 1)
							{
								goto IL_13D;
							}
							global::Debug.Log("TCP：相手 - バトル開始確認受信");
						}
						else
						{
							global::Debug.Log("TCP：自分 - バトル開始確認受信 - " + keyValuePair2.Value.ToString());
							this.isReadyBattle = true;
							if (this.isMockBattle == 0 && CMD_PvPTop.Instance != null)
							{
								Singleton<UserDataMng>.Instance.ConsumeUserStamina(CMD_PvPTop.Instance.NeedStamina);
							}
						}
						continue;
						IL_13D:
						global::Debug.Log("例外: " + keyValuePair2.Key.ToString() + " " + keyValuePair2.Value.ToString());
						this.DispErrorModal(keyValuePair2.Value.ToString());
					}
					else
					{
						global::Debug.Log("例外: " + keyValuePair2.Key.ToString() + " " + keyValuePair2.Value.ToString());
						this.DispErrorModal(keyValuePair2.Value.ToString());
					}
				}
			}
		}
	}

	private void OnRecievedBattlePvPEnemyData(Dictionary<string, object> packet)
	{
		foreach (KeyValuePair<string, object> keyValuePair in packet)
		{
			Dictionary<object, object> dictionary = keyValuePair.Value as Dictionary<object, object>;
			if (dictionary != null)
			{
				foreach (KeyValuePair<object, object> keyValuePair2 in dictionary)
				{
					Dictionary<object, object> dictionary2 = keyValuePair2.Value as Dictionary<object, object>;
					if (dictionary2 != null)
					{
						foreach (KeyValuePair<object, object> keyValuePair3 in dictionary2)
						{
							if (keyValuePair3.Key.Equals("indexId"))
							{
								List<object> list = keyValuePair3.Value as List<object>;
								List<string> list2 = new List<string>();
								for (int i = 0; i < list.Count; i++)
								{
									list2.Add(list[i] as string);
								}
								GameWebAPI.Common_MonsterData[] array = new GameWebAPI.Common_MonsterData[3];
								for (int j = 0; j < array.Length; j++)
								{
									array[j] = this.pvpUserDatas[1].monsterData[int.Parse(list2[j])];
								}
								this.pvpUserDatas[1].monsterData = array;
								if (this.pvpUserDatas[1].monsterData.Length == 3)
								{
									this.enemyMonsterDataGetCheck = true;
									this.SendPvPEnemyDataReceive();
								}
							}
						}
					}
				}
			}
		}
	}

	private void OnRecievedOnlineCheck(object messageObj)
	{
		int valueByKey = MultiTools.GetValueByKey<int>(messageObj, "resultCode");
		global::Debug.LogFormat("[対戦相手のオンラインステータスチェック（080110）]result_code:{0}", new object[]
		{
			valueByKey
		});
		if (valueByKey == 0)
		{
			this.IsPvPOnlineCheck = true;
		}
		else if (valueByKey == 1)
		{
			this.IsPvPOnlineCheck = true;
		}
	}

	private IEnumerator PrepareGoToBattleScene()
	{
		this.isPrepareBattle = true;
		this.SetCancelBtn(false);
		this.pvpUserDatas = new MultiBattleData.PvPUserData[2];
		this.SetMyData();
		bool isMatchSuccess = true;
		yield return base.StartCoroutine(this.SetEnemyData(delegate(bool result)
		{
			isMatchSuccess = result;
		}));
		if (!isMatchSuccess)
		{
			this.RetryMatching();
			yield break;
		}
		bool isPrepareSuccess = true;
		yield return base.StartCoroutine(this.GetEnemyPrepareStateus(delegate(bool result)
		{
			isPrepareSuccess = result;
		}));
		if (!isPrepareSuccess)
		{
			this.RetryMatching();
			yield break;
		}
		while (!this.isReadyBattle)
		{
			yield return base.StartCoroutine(Util.WaitForRealTime(1f));
			if (!this.isShowAlert)
			{
				this.SendTCPPvPBattleStart();
			}
		}
		this.monsInfo.SetAnimation(CharacterAnimationType.win);
		ClassSingleton<MultiBattleData>.Instance.MaxAttackTime = ConstValue.PVP_MAX_ATTACK_TIME;
		ClassSingleton<MultiBattleData>.Instance.HurryUpAttackTime = ConstValue.PVP_HURRYUP_ATTACK_TIME;
		ClassSingleton<MultiBattleData>.Instance.MaxRoundNum = ConstValue.PVP_MAX_ROUND_NUM;
		ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId = this.myUserId;
		ClassSingleton<MultiBattleData>.Instance.PvPUserDatas = this.pvpUserDatas;
		MultiBattleData.PvPFieldData field = new MultiBattleData.PvPFieldData();
		field.worldDungeonId = this.worldDungeonId;
		ClassSingleton<MultiBattleData>.Instance.PvPField = field;
		ClassSingleton<MultiBattleData>.Instance.RandomSeed = this.randomSeed;
		ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StopGetHistoryIdList();
		if (this.matchingFinishAnimation == null)
		{
			this.matchingFinishAnimation = base.StartCoroutine(this.StartMatchingFinishAnimation());
		}
		yield break;
	}

	private void RetryMatching()
	{
		this.isRetry = true;
		this.isPrepareBattle = false;
		this.SendTCPPvPMatching(false);
	}

	private void SetMyData()
	{
		MultiBattleData.PvPUserData pvPUserData = new MultiBattleData.PvPUserData();
		pvPUserData.userStatus = this.colosseumUserStatus;
		pvPUserData.monsterData = new GameWebAPI.Common_MonsterData[6];
		pvPUserData.isOwner = this.isOwner;
		List<MonsterData> colosseumDeckUserMonsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
		for (int i = 0; i < colosseumDeckUserMonsterList.Count; i++)
		{
			GameWebAPI.Common_MonsterData common_MonsterData = new GameWebAPI.Common_MonsterData(colosseumDeckUserMonsterList[i]);
			pvPUserData.monsterData[i] = common_MonsterData;
		}
		this.pvpUserDatas[0] = pvPUserData;
	}

	private IEnumerator SetEnemyData(Action<bool> callback)
	{
		MultiBattleData.PvPUserData pvpEnemyData = new MultiBattleData.PvPUserData();
		pvpEnemyData.monsterData = new GameWebAPI.Common_MonsterData[6];
		GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode resultCode_GetColosseumUserStatus = GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode.SUCCESS;
		GameWebAPI.RespData_ColosseumDeckInfoLogic.ResultCode resultCode_GetColosseumDeckInfo = GameWebAPI.RespData_ColosseumDeckInfoLogic.ResultCode.SUCCESS;
		this.TCPSendUserIdList.Clear();
		GameWebAPI.ColosseumUserStatusLogic usRequest = new GameWebAPI.ColosseumUserStatusLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ColosseumUserStatusLogic param)
			{
				param.target = "battle";
				param.isMockBattle = this.isMockBattle;
			},
			OnReceived = delegate(GameWebAPI.RespData_ColosseumUserStatusLogic response)
			{
				PvPUtility.SetPvPTopNoticeCode(response);
				pvpEnemyData.userStatus = response.userStatus;
				resultCode_GetColosseumUserStatus = response.GetResultCodeEnum;
				this.TCPSendUserIdList.Add(int.Parse(response.userStatus.userId));
			}
		};
		yield return base.StartCoroutine(usRequest.Run(null, null, null));
		if (resultCode_GetColosseumUserStatus != GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode.SUCCESS)
		{
			global::Debug.LogFormat("対戦相手情報が不正のため、マッチング待機状態に遷移します {0}", new object[]
			{
				resultCode_GetColosseumUserStatus
			});
			callback(false);
			yield break;
		}
		GameWebAPI.ColosseumDeckInfoLogic diRequest = new GameWebAPI.ColosseumDeckInfoLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ColosseumDeckInfoLogic param)
			{
				param.isMockBattle = this.isMockBattle;
			},
			OnReceived = delegate(GameWebAPI.RespData_ColosseumDeckInfoLogic response)
			{
				pvpEnemyData.monsterData = response.partyMonsters;
				resultCode_GetColosseumDeckInfo = response.GetResultCodeEnum;
			}
		};
		yield return base.StartCoroutine(diRequest.Run(null, null, null));
		if (resultCode_GetColosseumDeckInfo != GameWebAPI.RespData_ColosseumDeckInfoLogic.ResultCode.SUCCESS)
		{
			global::Debug.LogFormat("対戦相手のデッキ情報が不正のため、マッチング待機状態に遷移します {0}", new object[]
			{
				resultCode_GetColosseumDeckInfo
			});
			callback(false);
			yield break;
		}
		yield return base.StartCoroutine(this.GetEnemyChip(pvpEnemyData));
		for (int i = 0; i < pvpEnemyData.monsterData.Length; i++)
		{
			MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(pvpEnemyData.monsterData[i].monsterId);
			this.enemyMonsterDataList.Add(monsterData);
		}
		this.pvpUserDatas[1] = pvpEnemyData;
		global::Debug.Log("対戦相手の情報取得に成功しました。対戦相手は " + pvpEnemyData.userStatus.nickname + " です");
		yield break;
	}

	private IEnumerator GetEnemyChip(MultiBattleData.PvPUserData pvpEnemyData)
	{
		GameWebAPI.RespDataCS_MonsterSlotInfoListLogic chipResponse = null;
		GameWebAPI.MonsterSlotInfoListLogic chipRequest = new GameWebAPI.MonsterSlotInfoListLogic
		{
			SetSendData = delegate(GameWebAPI.ReqDataCS_MonsterSlotInfoListLogic param)
			{
				param.userMonsterId = new int[]
				{
					pvpEnemyData.monsterData[0].userMonsterId.ToInt32(),
					pvpEnemyData.monsterData[1].userMonsterId.ToInt32(),
					pvpEnemyData.monsterData[2].userMonsterId.ToInt32(),
					pvpEnemyData.monsterData[3].userMonsterId.ToInt32(),
					pvpEnemyData.monsterData[4].userMonsterId.ToInt32(),
					pvpEnemyData.monsterData[5].userMonsterId.ToInt32()
				};
			},
			OnReceived = delegate(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic response)
			{
				chipResponse = response;
			}
		};
		yield return base.StartCoroutine(chipRequest.Run(null, null, null));
		if (chipResponse != null && chipResponse.slotInfo != null)
		{
			List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip> enemyUserChipIdList = new List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip>();
			for (int i = 0; i < chipResponse.slotInfo.Length; i++)
			{
				if (chipResponse.slotInfo[i] != null && chipResponse.slotInfo[i].equip != null)
				{
					enemyUserChipIdList.AddRange(chipResponse.slotInfo[i].equip);
				}
			}
			if (0 < enemyUserChipIdList.Count)
			{
				int[] userChipIdList = new int[enemyUserChipIdList.Count];
				for (int j = 0; j < enemyUserChipIdList.Count; j++)
				{
					userChipIdList[j] = enemyUserChipIdList[j].userChipId;
				}
				GameWebAPI.RespDataCS_ChipListLogic chipInfoResponse = null;
				GameWebAPI.ReqDataCS_ChipListLogic chipInfoRequest = new GameWebAPI.ReqDataCS_ChipListLogic
				{
					SetSendData = delegate(GameWebAPI.SendDataCS_ChipListLogic param)
					{
						param.userChipId = userChipIdList;
					},
					OnReceived = delegate(GameWebAPI.RespDataCS_ChipListLogic response)
					{
						chipInfoResponse = response;
					}
				};
				yield return base.StartCoroutine(chipInfoRequest.Run(null, null, null));
				if (chipInfoResponse != null && chipInfoResponse.userChipList != null)
				{
					for (int k = 0; k < pvpEnemyData.monsterData.Length; k++)
					{
						int id = pvpEnemyData.monsterData[k].userMonsterId.ToInt32();
						int[] idList = chipInfoResponse.userChipList.Where((GameWebAPI.RespDataCS_ChipListLogic.UserChipList x) => x.userMonsterId == id).Select((GameWebAPI.RespDataCS_ChipListLogic.UserChipList x) => x.chipId).ToArray<int>();
						if (0 < idList.Length)
						{
							pvpEnemyData.monsterData[k].chipIdList = idList;
						}
					}
				}
			}
		}
		yield break;
	}

	private IEnumerator GetEnemyPrepareStateus(Action<bool> callback)
	{
		int MAX_CNT = 10;
		for (int cnt = 0; cnt < MAX_CNT; cnt++)
		{
			GameWebAPI.RespData_ColosseumPrepareStatusLogic.PrepareStatusCode resultCode = GameWebAPI.RespData_ColosseumPrepareStatusLogic.PrepareStatusCode.READY;
			GameWebAPI.ColosseumPrepareStatusLogic request = new GameWebAPI.ColosseumPrepareStatusLogic
			{
				SetSendData = delegate(GameWebAPI.ReqData_ColosseumPrepareStatusLogic param)
				{
					param.isMockBattle = this.isMockBattle;
				},
				OnReceived = delegate(GameWebAPI.RespData_ColosseumPrepareStatusLogic response)
				{
					resultCode = response.GetResultCodeEnum;
				}
			};
			yield return base.StartCoroutine(request.RunOneTime(null, null, null));
			switch (resultCode)
			{
			case GameWebAPI.RespData_ColosseumPrepareStatusLogic.PrepareStatusCode.READY:
				global::Debug.Log("相手の準備が完了しています");
				callback(true);
				yield break;
			case GameWebAPI.RespData_ColosseumPrepareStatusLogic.PrepareStatusCode.NOT_READY:
				if (cnt == MAX_CNT - 1)
				{
					global::Debug.Log("相手の準備があまりに遅いためこのマッチングを破棄します");
					callback(false);
					yield break;
				}
				global::Debug.Log("相手の準備が未完了のためしばらく待ちます");
				break;
			case GameWebAPI.RespData_ColosseumPrepareStatusLogic.PrepareStatusCode.CANT_START:
				global::Debug.Log("相手がバトルを開始できない状態のためこのマッチングを破棄します");
				callback(false);
				yield break;
			}
			yield return base.StartCoroutine(Util.WaitForRealTime(1f));
		}
		yield break;
	}

	public void OnClickCancel()
	{
		MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		if (Singleton<TCPUtil>.Instance.CheckTCPConnection())
		{
			this.SendTCPPvPMatching(false);
		}
		else
		{
			this.ClosePanel(true);
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus)
		{
			if (this.isPrepareBattle)
			{
				this.isOwner = false;
				Singleton<TCPUtil>.Instance.TCPReConnection(0);
			}
			else
			{
				this.DispErrorModal(StringMaster.GetString("MultiRecruit-13"), StringMaster.GetString("ColosseumNetworkError"), false);
			}
			int num = (int)(Time.realtimeSinceStartup - this.pausedTime);
			if (num >= ConstValue.MULTI_BATTLE_TIMEOUT_TIME)
			{
				global::Debug.LogErrorFormat("{0}秒経ったので負け.", new object[]
				{
					num
				});
				BattlePvPFunction.isAlreadyLoseBeforeBattle = true;
			}
		}
		else
		{
			this.pausedTime = Time.realtimeSinceStartup;
		}
	}

	private void DispErrorModal(string title, string info, bool isClosePartyEdit = false)
	{
		AlertManager.ShowModalMessage(delegate(int modal)
		{
			if (isClosePartyEdit)
			{
				this.SetCloseAction(delegate(int i)
				{
				});
			}
			Singleton<TCPUtil>.Instance.TCPDisConnect(false);
			this.ClosePanel(true);
		}, title, info, AlertManager.ButtonActionType.Close, false);
	}

	private void DispErrorModal(string errorCode)
	{
		if (!this.isShowAlert)
		{
			this.isShowAlert = true;
			base.StartCoroutine(this.ShowAlertDialog(errorCode, new Action<string>(this.ShowAlertDialog)));
		}
	}

	private IEnumerator ShowAlertDialog(string errorCode, Action<string> onFinished)
	{
		if (Singleton<TCPUtil>.Instance.CheckTCPConnection())
		{
			MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			GameWebAPI.RespData_ColosseumUserStatusLogic responseStatus = null;
			APIRequestTask task = PvPUtility.RequestColosseumUserStatus(PvPUtility.RequestUserStatusType.MY, delegate(GameWebAPI.RespData_ColosseumUserStatusLogic res)
			{
				PvPUtility.SetPvPTopNoticeCode(res);
				responseStatus = res;
			});
			yield return base.StartCoroutine(task.Run(null, null, null));
			switch (responseStatus.GetResultCodeEnum)
			{
			case GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode.BATTLE_INTERRUPTION:
			case GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode.BATTLE_INTERRUPTION_WIN:
			{
				GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleResult result = PvPUtility.GetBattleResult(responseStatus.GetResultCodeEnum);
				task = PvPUtility.RequestColosseumBattleEnd(result, GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleMode.NORMAL_BATTLE);
				task.Add(PvPUtility.RequestColosseumUserStatus(PvPUtility.RequestUserStatusType.MY, delegate(GameWebAPI.RespData_ColosseumUserStatusLogic res)
				{
					PvPUtility.SetPvPTopNoticeCode(res);
				}));
				yield return base.StartCoroutine(task.Run(null, null, null));
				Singleton<TCPUtil>.Instance.TCPDisConnect(false);
				goto IL_188;
			}
			}
			this.SendTCPPvPMatching(false);
			this.isRetry = false;
			while (Singleton<TCPUtil>.Instance.CheckTCPConnection())
			{
				yield return null;
			}
			IL_188:
			MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		}
		if (onFinished != null)
		{
			onFinished(errorCode);
		}
		yield break;
	}

	private void ShowAlertDialog(string errorCode)
	{
		AlertManager.ShowAlertDialog(delegate(int modal)
		{
			if (modal == 3)
			{
				GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			}
			else
			{
				this.ClosePanel(true);
			}
		}, errorCode);
	}

	public void OnExitTCP()
	{
		global::Debug.Log("TCP exit");
	}

	public void OnExceptionTCP(short errorCode, string errorMessage)
	{
		global::Debug.Log(errorCode);
		global::Debug.Log(errorMessage);
		if (!this.isPrepareBattle)
		{
			MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			this.DispErrorModal(StringMaster.GetString("MultiRecruit-13"), StringMaster.GetString("ColosseumNetworkError"), false);
		}
	}

	private void OnRequestExceptionTCP(string resultCode, string resultMessage)
	{
		this.DispErrorModal("E-AL07");
	}

	private IEnumerator StartMatchingFinishAnimation()
	{
		PvPMatchingFinishAnimation animationController = new PvPMatchingFinishAnimation(this.goMatchingNowAnim, this.goMatchingEndAnim, this.monsInfo, this.winAnimationWait, this.renderTextureColor);
		yield return base.StartCoroutine(animationController.StartMatchingEffect());
		WaitForSeconds waitTime = new WaitForSeconds(this.transferWait);
		yield return waitTime;
		if (null == this.versusInfo)
		{
			this.versusInfo = PvPVersusInfo6Icon.CreateInstance(Singleton<GUIMain>.Instance.transform);
			this.versusInfo.SetTitle(this.isMockBattle);
			this.versusInfo.SetUserInfo(ClassSingleton<MultiBattleData>.Instance.PvPUserDatas);
			this.versusInfo.SetActionAnimationEnd(delegate
			{
				base.StartCoroutine(this.OnFinishedMatchingAnimation(animationController));
			});
		}
		yield break;
	}

	private IEnumerator OnFinishedMatchingAnimation(PvPMatchingFinishAnimation animationController)
	{
		if (this.partySelect == null)
		{
			this.partySelect = PVPPartySelect3.CreateInstance(Singleton<GUIMain>.Instance.transform);
			List<MonsterData> allMdList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
			this.partySelect.SetData(this);
			this.partySelect.SetMonsterData(allMdList, this.enemyMonsterDataList);
			this.partySelect.gameObject.SetActive(true);
		}
		else
		{
			List<MonsterData> allMdList2 = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
			this.partySelect.Init();
			this.partySelect.SetData(this);
			this.partySelect.SetMonsterData(allMdList2, this.enemyMonsterDataList);
			this.partySelect.gameObject.SetActive(true);
		}
		yield return base.StartCoroutine(this.SelectBattleParty());
		while (!this.pvpDataRetry)
		{
			yield return null;
			if (this.pvpEnemyOnlineCheck != null)
			{
				GameWebAPI.Common_MonsterData[] pvpEnemyListData = new GameWebAPI.Common_MonsterData[3];
				for (int i = 0; i < pvpEnemyListData.Length; i++)
				{
					pvpEnemyListData[i] = this.pvpUserDatas[1].monsterData[i];
				}
				this.pvpUserDatas[1].monsterData = pvpEnemyListData;
				this.pvpDataRetry = true;
				MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			}
		}
		yield return base.StartCoroutine(animationController.DeleteTransferEffect());
		this.versusInfo.SetBackground(this.background);
		this.versusInfo.gameObject.SetActive(true);
		this.versusInfo.AnimaObjectActiveSet(false);
		base.SetCloseAction(new Action<int>(this.ChangeScene));
		this.useCMDAnim = false;
		while (this.isShowAlert)
		{
			yield return null;
		}
		if (null != CMD_PvPTop.Instance)
		{
			CMD_PvPTop.Instance.IsToBattle = true;
		}
		GUIManager.CloseAllCommonDialog(null);
		this.partySelect.gameObject.SetActive(false);
		yield break;
	}

	private IEnumerator SelectBattleParty()
	{
		this.versusInfo.gameObject.SetActive(false);
		this.spCancelBtn.gameObject.SetActive(false);
		bool waitSendData = false;
		bool retry = false;
		this.pvpEnemyOnlineCheck = null;
		float timer = 0f;
		for (;;)
		{
			if (this.myPartyDataSendCheck)
			{
				if (this.myMonsterDataSendCheck && this.enemyMonsterDataGetCheck)
				{
					break;
				}
				timer += Time.deltaTime * 1f;
				if (timer >= (float)MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_SELECT_DATA_RETRY_TIME)
				{
					goto Block_4;
				}
				yield return null;
			}
			else
			{
				yield return null;
			}
		}
		waitSendData = false;
		retry = false;
		this.pvpDataRetry = true;
		goto IL_1D9;
		Block_4:
		if (this.selectPartyRetryCount >= MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_SELECT_DATA_RETRY_COUNT)
		{
			this.pvpDataRetry = true;
			waitSendData = true;
			retry = false;
		}
		else
		{
			this.pvpDataRetry = false;
			retry = true;
			this.selectPartyRetryCount++;
			if (!this.myMonsterDataSendCheck)
			{
				this.myPartyDataSendCheck = false;
				this.TCPPvPEnemyData();
			}
			base.StartCoroutine(this.SelectBattleParty());
		}
		IL_1D9:
		if (retry)
		{
			yield break;
		}
		if (waitSendData)
		{
			this.pvpEnemyOnlineCheck = this.PvPOnlineCheck();
		}
		yield break;
	}

	private IEnumerator PvPOnlineCheck()
	{
		IEnumerator judge = this.SendPvPOnlineCheck();
		while (judge.MoveNext())
		{
			object obj = judge.Current;
			yield return obj;
		}
		yield break;
	}

	public IEnumerator SendPvPOnlineCheck()
	{
		float sendingWaitingTerm = 2f;
		this.IsPvPOnlineCheck = false;
		Dictionary<string, object> data = new Dictionary<string, object>();
		PvPOnlineCheck message = new PvPOnlineCheck
		{
			uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
		};
		data.Add("080110", message);
		int checkCount = 0;
		for (;;)
		{
			checkCount++;
			if (this.IsPvPOnlineCheck || Singleton<TCPUtil>.Instance == null)
			{
				break;
			}
			if (checkCount > MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_SELECT_DATA_RETRY_COUNT)
			{
				goto Block_2;
			}
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			IEnumerator wait = Util.WaitForRealTime(sendingWaitingTerm);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
		}
		yield break;
		Block_2:
		yield break;
		yield break;
	}

	public void SendPvPEnemyData(string[] indexIdList)
	{
		List<MonsterData> colosseumDeckUserMonsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
		GameWebAPI.Common_MonsterData[] array = new GameWebAPI.Common_MonsterData[3];
		ColosseumData.LastUseMonsterList = new MonsterData[3];
		this.mySelectIndexIdList = indexIdList;
		for (int i = 0; i < array.Length; i++)
		{
			MonsterData monsterData = colosseumDeckUserMonsterList[int.Parse(this.mySelectIndexIdList[i])];
			ColosseumData.LastUseMonsterList[i] = monsterData;
			GameWebAPI.Common_MonsterData common_MonsterData = new GameWebAPI.Common_MonsterData(monsterData);
			array[i] = common_MonsterData;
		}
		this.pvpUserDatas[0].monsterData = array;
		this.TCPPvPEnemyData();
		this.myPartyDataSendCheck = true;
		MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
	}

	private void TCPPvPEnemyData()
	{
		int[] array = new int[this.mySelectIndexIdList.Length];
		for (int i = 0; i < this.mySelectIndexIdList.Length; i++)
		{
			array[i] = int.Parse(this.mySelectIndexIdList[i]);
		}
		PvPEnemyData message = new PvPEnemyData
		{
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.PvPEnemyData, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			indexId = array
		};
		Singleton<TCPUtil>.Instance.SendMessageForTarget(TCPMessageType.PvPEnemyData, message, this.TCPSendUserIdList, "enemyData");
		this.myPartyDataSendCheck = true;
	}

	private void SendPvPEnemyDataReceive()
	{
		PvPEnemyDataReceive pvPEnemyDataReceive = new PvPEnemyDataReceive
		{
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.PvPEnemyDataReceive, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			resultCode = "1",
			randomSeed = ((int)DateTime.Now.Ticks & 65535)
		};
		Singleton<TCPUtil>.Instance.SendMessageForTarget(TCPMessageType.PvPEnemyDataReceive, pvPEnemyDataReceive, this.TCPSendUserIdList, "enemyDataReceive");
		if (this.isOwner)
		{
			this.randomSeed = pvPEnemyDataReceive.randomSeed;
		}
	}

	private void ChangeScene(int noop)
	{
		global::Debug.Log("バトルシーンのロードを開始します");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		RestrictionInput.SetDisplayObject(this.versusInfo.gameObject);
		this.versusInfo = null;
		BattleStateManager.StartPvP(0.5f, 0.5f, true, null);
	}
}
