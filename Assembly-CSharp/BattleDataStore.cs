﻿using BattleStateMachineInternal;
using JsonFx.Json;
using Master;
using Monster;
using Quest;
using Save;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public sealed class BattleDataStore : ClassSingleton<BattleDataStore>
{
	private List<string> cachedJsonForSave;

	public const string saveFileName = "SAVE_DT.binary";

	public const string subSaveFileName = "SUB_SAVE_DT.binary";

	public bool IsBattleRecoverable;

	private bool isRecoverSuccessWebAPI;

	private PersistentFile save = new PersistentFile(false);

	private bool isProgressSave;

	private bool isSubTurn;

	private string[] cachedJsonArr;

	private Type[] typeNameBlackList = new Type[]
	{
		typeof(CameraParams),
		typeof(TweenCameraFunction),
		typeof(UnityObjectPooler<SpawnPointParams>),
		typeof(UnityObjectPooler<CharacterParams>),
		typeof(UnityObjectPooler<CharacterDatas>),
		typeof(UnityObjectPooler<SkillStatus>),
		typeof(UnityObjectPooler<HitEffectParams[]>),
		typeof(List<CharacterStateControl>),
		typeof(UnityObjectPooler<CameraParams>),
		typeof(BattleCameraSwitcher),
		typeof(GameObject),
		typeof(HitEffectParams),
		typeof(HitEffectParams[]),
		typeof(AlwaysEffectParams[]),
		typeof(LayerMask),
		typeof(Transform[]),
		typeof(Transform),
		typeof(TweenCameraTargetFunction),
		typeof(int[]),
		typeof(List<FraudDataLog>),
		typeof(List<HitEffectParams>)
	};

	private string[] variableNameBlackList = new string[]
	{
		"isShowMenuWindow",
		"isShowRetireWindow",
		"isShowRevivalWindow",
		"isShowContinueWindow",
		"isContinueFlag",
		"isShowCharacterDescription",
		"isShowHelp",
		"isInvocationEffectPlaying",
		"isPossibleShowDescription",
		"isSlowMotion",
		"isShowSpecificTrade",
		"isShowInitialIntroduction",
		"isShowShop",
		"beforeConfirmDigiStoneNumber",
		"currentDeadCharacters",
		"isMustLoad",
		"calledBattleStartAction",
		"isBattleRetired",
		"leaderCharacter",
		"leaderEnemyCharacter",
		"useInheritanceSkillPlayers",
		"extraEffectStatus",
		"currentSelectCharacterState"
	};

	public TaskBase RequestWorldStartDataLogic()
	{
		NormalTask normalTask = new NormalTask(delegate()
		{
			this.IsBattleRecoverable = false;
			this.isRecoverSuccessWebAPI = false;
			ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart = null;
			return null;
		});
		GameWebAPI.WorldStartDataLogic request = new GameWebAPI.WorldStartDataLogic
		{
			OnReceived = delegate(GameWebAPI.ReceiveQuestResume response)
			{
				if (response.startId != null)
				{
					this.isRecoverSuccessWebAPI = true;
					ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart = response;
					string text = string.Empty;
					DkLog.W("response.userDungeonTicketId : " + response.userDungeonTicketId, false);
					if (string.IsNullOrEmpty(response.userDungeonTicketId))
					{
						string @string = PlayerPrefs.GetString("userDungeonTicketId", string.Empty);
						if (!string.IsNullOrEmpty(@string))
						{
							text = @string;
							DkLog.W("ticketid : " + text, false);
						}
						PlayerPrefs.SetString("userDungeonTicketId", string.Empty);
					}
					else
					{
						text = response.userDungeonTicketId;
					}
					DkLog.W("SetRetryData(response.worldDungeonId, ticketid) : " + text, false);
					this.SetRetryData(response.worldDungeonId, text);
				}
			}
		};
		normalTask.Add(new APIRequestTask(request, true));
		normalTask.Add(new NormalTask(delegate()
		{
			if (this.CheckSaveFile())
			{
				return this.CheckJsonBySaveData();
			}
			this.ValidateForPopup(false);
			return null;
		}));
		return normalTask;
	}

	private void SetRetryData(string worldDungeonId, string userDungeonTicketId)
	{
		string selectDeckNum = DataMng.Instance().RespDataMN_DeckList.selectDeckNum;
		DataMng.Instance().GetResultUtilData().last_dng_req = new GameWebAPI.WD_Req_DngStart
		{
			dungeonId = worldDungeonId,
			deckNum = selectDeckNum.ToString(),
			userDungeonTicketId = userDungeonTicketId
		};
	}

	private IEnumerator RequestWorldStartDataDelete()
	{
		GameWebAPI.WorldResultLogic worldResultLogic = new GameWebAPI.WorldResultLogic();
		worldResultLogic.SetSendData = delegate(GameWebAPI.WD_Req_DngResult param)
		{
			param.startId = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart.startId;
			param.dungeonId = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart.worldDungeonId;
			param.clear = 0;
			param.aliveInfo = new int[3];
			GameWebAPI.RespDataWD_DungeonStart.DungeonFloor[] dungeonFloor = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart.dungeonFloor;
			if (dungeonFloor != null)
			{
				param.enemyAliveInfo = new int[dungeonFloor.Length][];
				for (int i = 0; i < dungeonFloor.Length; i++)
				{
					int num = dungeonFloor[i].enemy.Length;
					param.enemyAliveInfo[i] = new int[num];
				}
			}
		};
		GameWebAPI.WorldResultLogic request = worldResultLogic;
		return request.Run(null, delegate(Exception nop)
		{
			global::Debug.LogError("RequestWorldStartDataDelete is failed.");
		}, null);
	}

	public void OpenBattleRecoverConfirm(Action onBattleRecover, Action onCancel)
	{
		this.IsBattleRecoverable = false;
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(delegate(int i)
		{
			if (i == 0)
			{
				AppCoroutine.Start(this.GoBattle(), false);
				if (onBattleRecover != null)
				{
					onBattleRecover();
				}
			}
			else if (i == 1)
			{
				this.GoFarm(onCancel);
			}
			else
			{
				global::Debug.LogError("ダイアログ選択でありえない選択がされた");
				this.GoFarm(onCancel);
			}
		}, "CMD_Confirm", null) as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("BattleComebackTitle");
		cmd_Confirm.Info = StringMaster.GetString("BattleComebackInfo");
	}

	private IEnumerator GoBattle()
	{
		GameWebAPI.WD_Req_DngStart sendData = DataMng.Instance().GetResultUtilData().last_dng_req;
		if (!ClassSingleton<QuestData>.Instance.ExistEvent(sendData.dungeonId))
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			while (!AssetDataCacheMng.Instance().IsCacheAllReadyType(AssetDataCacheMng.CACHE_TYPE.BATTLE_COMMON))
			{
				yield return null;
			}
			this.SetCacheChipIconResources();
			RestrictionInput.EndLoad();
			BattleStateManager.StartSingle(0.5f, 0.5f, true, null);
		}
		else
		{
			QuestStart.StartEventStage(DataMng.Instance().GetResultUtilData().last_dng_req);
		}
		yield break;
	}

	private void SetCacheChipIconResources()
	{
		string selectDeckNum = DataMng.Instance().RespDataMN_DeckList.selectDeckNum;
		GameWebAPI.RespDataMN_GetDeckList.DeckList deckList = null;
		GameWebAPI.RespDataMN_GetDeckList.DeckList[] deckList2 = DataMng.Instance().RespDataMN_DeckList.deckList;
		for (int i = 0; i < deckList2.Length; i++)
		{
			if (selectDeckNum == deckList2[i].deckNum)
			{
				deckList = deckList2[i];
				break;
			}
		}
		int[] array = null;
		for (int j = 0; j < deckList.monsterList.Length; j++)
		{
			MonsterData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(deckList.monsterList[j].userMonsterId);
			MonsterChipEquipData chipEquip = userMonster.GetChipEquip();
			array = chipEquip.GetChipIdList();
		}
		for (int k = 0; k < array.Length; k++)
		{
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(array[k]);
			string iconPath = chipMainData.GetIconPath();
			if (!AssetDataMng.Instance().IsAssetBundleData(iconPath))
			{
				UnityEngine.Object resource = AssetDataMng.Instance().LoadObject(iconPath, null, true);
				AssetDataCacheMng.Instance().AddCache(iconPath, AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY, resource);
			}
		}
	}

	private void GoFarm(Action closeAction)
	{
		ClassSingleton<BattleDataStore>.Instance.DeleteForSystem();
		AppCoroutine.Start(this.DeleteAndStopAction(closeAction), false);
	}

	private IEnumerator DeleteAndStopAction(Action closeAction)
	{
		yield return AppCoroutine.Start(this.RequestWorldStartDataDelete(), false);
		if (closeAction != null)
		{
			closeAction();
		}
		yield break;
	}

	private IEnumerator LoadFromBinaryFile(string path, Action<string> callback)
	{
		if (!this.isProgressSave)
		{
			this.isProgressSave = true;
			IEnumerator wait = this.save.Read(path, delegate(bool isSuccess, string data)
			{
				this.isProgressSave = false;
				if (isSuccess)
				{
					callback(data);
				}
				else
				{
					callback(string.Empty);
					global::Debug.LogError("save.Readの失敗.");
				}
			});
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
		}
		yield break;
	}

	private IEnumerator SaveToBinaryFile(string jsonStr, string path)
	{
		if (!this.isProgressSave)
		{
			this.isProgressSave = true;
			yield return AppCoroutine.Start(this.save.Write(path, jsonStr, delegate(bool isSuccess)
			{
				this.isProgressSave = false;
				if (isSuccess)
				{
					string jsonFilePath = this.GetJsonFilePath(!this.isSubTurn);
					File.Delete(jsonFilePath);
					this.isSubTurn = !this.isSubTurn;
				}
				else
				{
					global::Debug.LogError("SaveToBinaryFile failed.");
				}
			}), false);
		}
		yield break;
	}

	private void SaveForSystem()
	{
		if (this.cachedJsonForSave == null)
		{
			return;
		}
		string jsonStr = JsonWriter.Serialize(this.cachedJsonForSave.ToArray());
		string jsonFilePath = this.GetJsonFilePath(this.isSubTurn);
		try
		{
			AppCoroutine.Start(this.SaveToBinaryFile(jsonStr, jsonFilePath), false);
		}
		catch
		{
		}
	}

	public void DeleteForSystem()
	{
		string jsonFilePath = this.GetJsonFilePath(false);
		if (File.Exists(jsonFilePath))
		{
			File.Delete(jsonFilePath);
			this.cachedJsonForSave = null;
		}
		jsonFilePath = this.GetJsonFilePath(true);
		if (File.Exists(jsonFilePath))
		{
			File.Delete(jsonFilePath);
			this.cachedJsonForSave = null;
		}
	}

	private bool CheckSaveFile()
	{
		string jsonFilePath = this.GetJsonFilePath(false);
		bool result = false;
		if (File.Exists(jsonFilePath))
		{
			this.isSubTurn = false;
			result = true;
		}
		else
		{
			jsonFilePath = this.GetJsonFilePath(true);
			if (File.Exists(jsonFilePath))
			{
				this.isSubTurn = true;
				result = true;
			}
		}
		return result;
	}

	public IEnumerator CheckRecoverForBattle(Action<bool> callback)
	{
		bool res = false;
		if (this.CheckSaveFile())
		{
			global::Debug.Log("ファイル自体がある.");
			IEnumerator wait = this.CacheJsonBySaveDataForBattle(delegate(bool callbackRes)
			{
				global::Debug.Log("CheckRecoverForBattleのcallback.");
				res = callbackRes;
			});
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
		}
		else
		{
			global::Debug.Log("ファイル自体がない.");
			res = false;
		}
		if (!res)
		{
			global::Debug.Log("キャッシュすることに失敗してたらファイル消しておく.");
			this.DeleteForSystem();
		}
		global::Debug.LogFormat("最終的なres{0}.", new object[]
		{
			res
		});
		callback(res);
		yield return null;
		yield break;
	}

	private IEnumerator CacheJsonBySaveDataForBattle(Action<bool> callback)
	{
		global::Debug.Log("CacheJsonBySaveDataForBattle.");
		bool res = true;
		string path = this.GetJsonFilePath(this.isSubTurn);
		if (string.IsNullOrEmpty(VersionManager.version))
		{
			global::Debug.LogError("VersionManager.version is null.");
			callback(false);
			yield break;
		}
		string tmpJsonStr = string.Empty;
		IEnumerator wait = this.LoadFromBinaryFile(path, delegate(string resJson)
		{
			global::Debug.Log("resJson.");
			tmpJsonStr = resJson;
		});
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		if (string.IsNullOrEmpty(tmpJsonStr))
		{
			global::Debug.LogError("tmpJsonStr file is bad.");
			callback(false);
			yield break;
		}
		try
		{
			string[] array = JsonReader.Deserialize<string[]>(tmpJsonStr);
			if (array.Length < 3)
			{
				global::Debug.LogError("jsonObj format error");
				res = false;
			}
			if (res && VersionManager.version == array[0])
			{
				GameWebAPI.RespDataWD_DungeonStart respDataWD_DungeonStart = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart;
				string b = string.Empty;
				string b2 = string.Empty;
				if (respDataWD_DungeonStart == null)
				{
					global::Debug.LogError("dungeon data is null");
					res = false;
				}
				else
				{
					b = respDataWD_DungeonStart.startId;
					b2 = respDataWD_DungeonStart.worldDungeonId;
				}
				VersatileDataStore versatileDataStore = JsonReader.Deserialize<VersatileDataStore>(array[1]);
				string startId = versatileDataStore.startId;
				string worldDungeonId = versatileDataStore.worldDungeonId;
				if (versatileDataStore.randomSeed != 0)
				{
					UnityEngine.Random.InitState(versatileDataStore.randomSeed);
				}
				else
				{
					UnityEngine.Random.state = JsonUtility.FromJson<UnityEngine.Random.State>(versatileDataStore.randomState);
				}
				if (res && startId == b && worldDungeonId == b2)
				{
					this.cachedJsonArr = new string[array.Length - 1];
					int num = 2;
					Array.Copy(array, num, this.cachedJsonArr, 0, array.Length - num);
					callback(res);
					yield break;
				}
				callback(false);
				yield break;
			}
			else
			{
				res = false;
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogErrorFormat("Error:{0}", new object[]
			{
				ex.Message
			});
			res = false;
		}
		global::Debug.LogFormat("最終的なres:{0}", new object[]
		{
			res
		});
		callback(res);
		yield break;
	}

	private string CalculateMD5(string str)
	{
		string result;
		using (MD5 md = MD5.Create())
		{
			byte[] value = md.ComputeHash(new UTF8Encoding().GetBytes(str));
			result = BitConverter.ToString(value).ToLower().Replace("-", string.Empty);
		}
		return result;
	}

	private IEnumerator CheckJsonBySaveData()
	{
		string path = this.GetJsonFilePath(this.isSubTurn);
		if (string.IsNullOrEmpty(VersionManager.version))
		{
			global::Debug.LogError("VersionManager.version is null.");
		}
		yield return AppCoroutine.Start(this.LoadFromBinaryFile(path, delegate(string resJson)
		{
			bool isValidFileExist = this.ValidateAndCacheJson(resJson);
			this.ValidateForPopup(isValidFileExist);
		}), false);
		yield break;
	}

	private void ValidateForPopup(bool isValidFileExist)
	{
		if (!this.isRecoverSuccessWebAPI && isValidFileExist)
		{
			this.DeleteForSystem();
			this.IsBattleRecoverable = false;
		}
		else if (this.isRecoverSuccessWebAPI && !isValidFileExist)
		{
			this.IsBattleRecoverable = true;
		}
		else if (this.isRecoverSuccessWebAPI && isValidFileExist)
		{
			this.IsBattleRecoverable = true;
		}
		else
		{
			this.DeleteForSystem();
		}
	}

	private bool ValidateAndCacheJson(string resJson)
	{
		bool flag = true;
		try
		{
			string[] array = JsonReader.Deserialize<string[]>(resJson);
			if (array.Length < 3)
			{
				global::Debug.LogError("jsonObj format error");
				flag = false;
			}
			if (flag && VersionManager.version == array[0])
			{
				GameWebAPI.RespDataWD_DungeonStart respDataWD_DungeonStart = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart;
				string b = string.Empty;
				string b2 = string.Empty;
				if (respDataWD_DungeonStart == null)
				{
					global::Debug.LogWarning("dungeon data is null");
					flag = false;
				}
				else
				{
					b = respDataWD_DungeonStart.startId;
					b2 = respDataWD_DungeonStart.worldDungeonId;
				}
				VersatileDataStore versatileDataStore = JsonReader.Deserialize<VersatileDataStore>(array[1]);
				string startId = versatileDataStore.startId;
				string worldDungeonId = versatileDataStore.worldDungeonId;
				if (flag && startId == b && worldDungeonId == b2)
				{
					string[] array2 = new string[array.Length - 1];
					int num = 2;
					Array.Copy(array, num, array2, 0, array.Length - num);
					string str = string.Concat(array2);
					string a = this.CalculateMD5(str);
					if (a == versatileDataStore.hashCode)
					{
						this.cachedJsonArr = array2;
					}
					else
					{
						global::Debug.LogError("MD5違う.");
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogErrorFormat("Error:{0}", new object[]
			{
				ex.Message
			});
			flag = false;
		}
		return flag;
	}

	public string GetJsonFilePath(bool isSub = false)
	{
		return Application.persistentDataPath + "/" + ((!isSub) ? "SAVE_DT.binary" : "SUB_SAVE_DT.binary");
	}

	public void Load(BattleStateData battleStateData)
	{
		string propInfoName = string.Empty;
		Type propInfoType = null;
		try
		{
			PropertyInfo[] propertyInfos = this.GetPropertyInfos(typeof(BattleStateData), false);
			int num = 0;
			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				propInfoName = propertyInfo.Name;
				propInfoType = propertyInfo.PropertyType;
				if (!this.typeNameBlackList.Any((Type c) => c == propInfoType))
				{
					if (!this.variableNameBlackList.Any((string c) => c == propInfoName))
					{
						string text = this.cachedJsonArr[num];
						if (propInfoType != typeof(CharacterStateControl))
						{
							if (propInfoType == typeof(CharacterStateControl[]))
							{
								if (propInfoName == "playerCharacters")
								{
									string[] array2 = JsonReader.Deserialize(text, typeof(string[])) as string[];
									int num2 = 0;
									foreach (string json in array2)
									{
										this.ManageCharacterStateControlForJsonToValue(json, battleStateData.playerCharacters[num2], battleStateData);
										num2++;
									}
								}
								else
								{
									if (propInfoName == "enemies")
									{
										goto IL_279;
									}
									global::Debug.LogErrorFormat("ありえない変数名:{0}です。", new object[]
									{
										propInfoName
									});
									goto IL_279;
								}
							}
							else if (propInfoType == typeof(CharacterStateControl[][]))
							{
								string[][] array4 = JsonReader.Deserialize(text, typeof(string[][])) as string[][];
								int num3 = 0;
								foreach (string array6 in array4)
								{
									int num4 = 0;
									foreach (string json2 in array6)
									{
										this.ManageCharacterStateControlForJsonToValue(json2, battleStateData.preloadEnemies[num3][num4], battleStateData);
										num4++;
									}
									num3++;
								}
							}
							else if (propInfoType == typeof(List<ItemDropResult>))
							{
								this.ManageItemDropResultForJsonToValue(text, battleStateData);
							}
							else if (!(propertyInfo.Name == "currentDigiStoneNumber"))
							{
								object value = JsonReader.Deserialize(text, propInfoType);
								propertyInfo.SetValue(battleStateData, value, null);
							}
						}
						num++;
					}
				}
				IL_279:;
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogErrorFormat("Error:{0}, 変数名:{1}, Type:{2}", new object[]
			{
				ex.Message,
				propInfoName,
				propInfoType
			});
		}
		DkLog.W(string.Format("チケットID {0} : {1}", battleStateData.ticketId, DataMng.Instance().GetResultUtilData().last_dng_req.userDungeonTicketId), false);
		if (!string.IsNullOrEmpty(battleStateData.ticketId))
		{
			DataMng.Instance().GetResultUtilData().last_dng_req.userDungeonTicketId = battleStateData.ticketId;
		}
	}

	private PropertyInfo[] GetPropertyInfos(Type type, bool isOnlyGet = false)
	{
		if (isOnlyGet)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		}
		return type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where((PropertyInfo p) => p.CanRead && p.CanWrite).ToArray<PropertyInfo>();
	}

	public void Save(BattleStateData battleStateData)
	{
		battleStateData.ticketId = DataMng.Instance().GetResultUtilData().last_dng_req.userDungeonTicketId;
		PropertyInfo[] propertyInfos = this.GetPropertyInfos(typeof(BattleStateData), false);
		List<string> list = new List<string>();
		string propInfoName = string.Empty;
		Type propInfoType = null;
		foreach (PropertyInfo propertyInfo in propertyInfos)
		{
			propInfoName = propertyInfo.Name;
			propInfoType = propertyInfo.PropertyType;
			if (!this.typeNameBlackList.Any((Type c) => c == propInfoType))
			{
				if (!this.variableNameBlackList.Any((string c) => c == propInfoName))
				{
					if (propertyInfo.PropertyType == typeof(CharacterStateControl))
					{
						if (propInfoName == "leaderCharacter")
						{
						}
					}
					else if (propInfoType == typeof(CharacterStateControl[]))
					{
						if (propInfoName == "playerCharacters")
						{
							List<string> list2 = new List<string>();
							CharacterStateControl[] array2 = propertyInfo.GetValue(battleStateData, null) as CharacterStateControl[];
							foreach (CharacterStateControl csc in array2)
							{
								string item = this.ManageCharacterStateControlForValueToJson(csc);
								list2.Add(item);
							}
							string item2 = JsonWriter.Serialize(list2.ToArray());
							list.Add(item2);
						}
						else if (!(propInfoName == "enemies"))
						{
							global::Debug.LogFormat("ありえない変数名:{0}です。", new object[]
							{
								propInfoName
							});
						}
					}
					else if (propInfoType == typeof(CharacterStateControl[][]))
					{
						List<string[]> list3 = new List<string[]>();
						CharacterStateControl[][] array4 = propertyInfo.GetValue(battleStateData, null) as CharacterStateControl[][];
						foreach (CharacterStateControl array6 in array4)
						{
							List<string> list4 = new List<string>();
							foreach (CharacterStateControl csc2 in array6)
							{
								string item3 = this.ManageCharacterStateControlForValueToJson(csc2);
								list4.Add(item3);
							}
							list3.Add(list4.ToArray());
						}
						string item4 = JsonWriter.Serialize(list3.ToArray());
						list.Add(item4);
					}
					else if (propInfoType == typeof(List<ItemDropResult>))
					{
						List<ItemDropResult> idrs = propertyInfo.GetValue(battleStateData, null) as List<ItemDropResult>;
						string item5 = this.ManageItemDropResultForValueToJson(idrs);
						list.Add(item5);
					}
					else
					{
						object value = propertyInfo.GetValue(battleStateData, null);
						string item6 = JsonWriter.Serialize(value);
						list.Add(item6);
					}
				}
			}
		}
		this.AppendStandardInfo(list);
		this.cachedJsonForSave = list;
		this.SaveForSystem();
	}

	private void AppendStandardInfo(List<string> jsonObjs)
	{
		GameWebAPI.RespDataWD_DungeonStart respDataWD_DungeonStart = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart;
		string str = string.Concat(jsonObjs.ToArray());
		string hashCode = this.CalculateMD5(str);
		VersatileDataStore versatileDataStore = new VersatileDataStore
		{
			worldDungeonId = respDataWD_DungeonStart.worldDungeonId,
			startId = respDataWD_DungeonStart.startId,
			hashCode = hashCode,
			randomState = JsonUtility.ToJson(UnityEngine.Random.state)
		};
		UnityEngine.Random.state = JsonUtility.FromJson<UnityEngine.Random.State>(versatileDataStore.randomState);
		string item = JsonWriter.Serialize(versatileDataStore);
		jsonObjs.Insert(0, item);
		jsonObjs.Insert(0, VersionManager.version);
	}

	private string ManageCharacterStateControlForValueToJson(CharacterStateControl csc)
	{
		CharacterStateControlStore value = new CharacterStateControlStore
		{
			hp = csc.hp,
			ap = csc.ap,
			isSelectSkill = csc.isSelectSkill,
			hate = csc.hate,
			previousHate = csc.previousHate,
			isLeader = csc.isLeader,
			skillOrder = csc.skillOrder,
			myIndex = csc.myIndex,
			isEnemy = csc.isEnemy,
			chipIds = csc.chipIds,
			isEscape = csc.isEscape,
			skillUseCounts = csc.skillUseCounts,
			currentSufferState = csc.currentSufferState.Get(),
			chipEffectCount = csc.GetChipEffectCountToString(),
			potencyChipIdList = csc.GetPotencyChipIdListToString()
		};
		return JsonWriter.Serialize(value);
	}

	private void ManageCharacterStateControlForJsonToValue(string json, CharacterStateControl characterStateControl, BattleStateData battleStateData)
	{
		CharacterStateControlStore savedCSC = JsonReader.Deserialize(json, typeof(CharacterStateControlStore)) as CharacterStateControlStore;
		characterStateControl.SetCharacterState(savedCSC, battleStateData);
	}

	private string ManageItemDropResultForValueToJson(List<ItemDropResult> idrs)
	{
		List<string> list = new List<string>();
		foreach (ItemDropResult itemDropResult in idrs)
		{
			ItemDropResultStore value = new ItemDropResultStore
			{
				isDropped = itemDropResult.isDropped,
				dropBoxType = itemDropResult.dropBoxType,
				isRare = itemDropResult.isRare
			};
			string item = JsonWriter.Serialize(value);
			list.Add(item);
		}
		return JsonWriter.Serialize(list.ToArray());
	}

	private void ManageItemDropResultForJsonToValue(string json, BattleStateData battleStateData)
	{
		string[] array = JsonReader.Deserialize(json, typeof(string[])) as string[];
		List<ItemDropResult> list = new List<ItemDropResult>();
		foreach (string value in array)
		{
			ItemDropResultStore itemDropResultStore = JsonReader.Deserialize(value, typeof(ItemDropResultStore)) as ItemDropResultStore;
			ItemDropResult item = new ItemDropResult(itemDropResultStore.dropBoxType);
			list.Add(item);
		}
		battleStateData.itemDropResults = list;
	}
}
