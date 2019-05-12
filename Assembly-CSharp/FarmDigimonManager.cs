using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public sealed class FarmDigimonManager : MonoBehaviour
{
	private const int MAX_DIGIMON_NUM = 3;

	private FarmDigimon[] farmDigimons;

	private string lastFavoriteDeckNum = string.Empty;

	private string[] lastMonstreIDs = new string[]
	{
		"-1",
		"-1",
		"-1"
	};

	public static GameWebAPI.RespDataMN_FriendTimeCheck.FriendshipTime[] FriendTimeList { get; set; }

	private void Awake()
	{
		this.farmDigimons = base.GetComponentsInChildren<FarmDigimon>(true);
	}

	public void CreateDigimonGameObject()
	{
		string[] favoriteDeckDigimon = this.GetFavoriteDeckDigimon();
		for (int i = 0; i < favoriteDeckDigimon.Length; i++)
		{
			this.farmDigimons[i].DeleteDigimon();
			if (!string.IsNullOrEmpty(favoriteDeckDigimon[i]))
			{
				base.StartCoroutine(this.farmDigimons[i].CreateDigimon(favoriteDeckDigimon[i]));
			}
		}
		this.GetLastMonsterId(favoriteDeckDigimon, false);
		this.lastFavoriteDeckNum = DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum;
		this.SetFriendshipUpFlg();
	}

	private void SetFriendshipUpFlg()
	{
		if (FarmDigimonManager.FriendTimeList != null)
		{
			for (int i = 0; i < this.farmDigimons.Length; i++)
			{
				for (int j = 0; j < this.farmDigimons.Length; j++)
				{
					if (this.farmDigimons[i].userMonsterID == FarmDigimonManager.FriendTimeList[j].userMonsterId)
					{
						this.farmDigimons[i].memoryNextTime = ServerDateTime.Now.AddSeconds((double)FarmDigimonManager.FriendTimeList[j].nextTimeSec);
						break;
					}
				}
			}
		}
	}

	private void GetLastMonsterId(string[] userMonsterIds, bool isEvolvePage = false)
	{
		for (int i = 0; i < userMonsterIds.Length; i++)
		{
			if (!string.IsNullOrEmpty(userMonsterIds[i]))
			{
				MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterIds[i], isEvolvePage);
				this.lastMonstreIDs[i] = monsterDataByUserMonsterID.monsterM.monsterId;
			}
		}
	}

	private bool IsSameDigimon(string monsterId, string userMonsterId, bool isEvolvePage = false)
	{
		MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterId, isEvolvePage);
		return monsterDataByUserMonsterID.monsterM.monsterId == monsterId;
	}

	public void RefreshDigimonGameObject(bool isEvolvePage = false, Action actCallBack = null)
	{
		base.StartCoroutine(this.EnumeratorRefreshDigimonGameObject(isEvolvePage, actCallBack));
	}

	private IEnumerator EnumeratorRefreshDigimonGameObject(bool isEvolvePage = false, Action actCallBack = null)
	{
		if (this.lastFavoriteDeckNum != DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum)
		{
			this.CreateDigimonGameObject();
			this.AppaearanceDigimon(actCallBack);
			this.lastFavoriteDeckNum = DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum;
			string[] favoriteDeckDigimon = this.GetFavoriteDeckDigimon();
			this.GetLastMonsterId(favoriteDeckDigimon, isEvolvePage);
			this.SetFriendshipUpFlg();
		}
		else if (null != MonsterDataMng.Instance())
		{
			string[] userMonstreIDs = this.GetFavoriteDeckDigimon();
			bool changed = false;
			for (int i = 0; i < userMonstreIDs.Length; i++)
			{
				if (!this.IsSameDigimon(this.lastMonstreIDs[i], userMonstreIDs[i], isEvolvePage))
				{
					this.farmDigimons[i].DeleteDigimon();
					if (!string.IsNullOrEmpty(userMonstreIDs[i]))
					{
						yield return base.StartCoroutine(this.farmDigimons[i].CreateDigimon(userMonstreIDs[i]));
					}
					changed = true;
				}
				else
				{
					this.farmDigimons[i].userMonsterID = userMonstreIDs[i];
				}
			}
			if (changed)
			{
				this.AppaearanceDigimon(actCallBack);
				this.GetLastMonsterId(userMonstreIDs, false);
			}
			else if (actCallBack != null)
			{
				actCallBack();
			}
			this.SetFriendshipUpFlg();
		}
		yield break;
	}

	public void CreateFriendDigimonGameObject(int[] ids, Action actCallBack = null)
	{
		base.StartCoroutine(this.EnumeratorCreateFriendDigimonGameObject(ids, actCallBack));
	}

	private IEnumerator EnumeratorCreateFriendDigimonGameObject(int[] ids, Action actCallBack = null)
	{
		for (int i = 0; i < ids.Length; i++)
		{
			this.farmDigimons[i].DeleteDigimon();
			MonsterData monsterData = new MonsterData(ids[i].ToString());
			yield return base.StartCoroutine(this.farmDigimons[i].CreateDigimon(monsterData));
		}
		this.lastMonstreIDs = new string[]
		{
			"-1",
			"-1",
			"-1"
		};
		this.AppaearanceDigimon(actCallBack);
		yield break;
	}

	private string[] GetFavoriteDeckDigimon()
	{
		int num = -1;
		if (int.TryParse(DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum, out num))
		{
			num--;
			GameWebAPI.RespDataMN_GetDeckList.DeckList deckList = DataMng.Instance().RespDataMN_DeckList.deckList[num];
			if (deckList != null && deckList.monsterList != null)
			{
				return deckList.monsterList.Select((GameWebAPI.RespDataMN_GetDeckList.MonsterList x) => x.userMonsterId).ToArray<string>();
			}
		}
		return new string[3];
	}

	public void AppaearanceDigimon(Action actCallBack = null)
	{
		for (int i = 0; i < this.farmDigimons.Length; i++)
		{
			base.StartCoroutine(this.farmDigimons[i].Boot());
		}
		if (actCallBack != null)
		{
			actCallBack();
		}
	}

	public bool FindAction(FarmDigimonAI.ActionID action)
	{
		for (int i = 0; i < this.farmDigimons.Length; i++)
		{
			if (this.farmDigimons[i].ActionID == action)
			{
				return true;
			}
		}
		return false;
	}

	public void SetActiveDigimon(bool value)
	{
		for (int i = 0; i < this.farmDigimons.Length; i++)
		{
			this.farmDigimons[i].SetActiveDigimon(value);
		}
	}

	public void SetFriendFarmMode(bool isFriendFarmMode)
	{
		for (int i = 0; i < this.farmDigimons.Length; i++)
		{
			this.farmDigimons[i].IsFriendShipUp = !isFriendFarmMode;
		}
	}
}
