using System;
using System.Collections.Generic;

namespace Monster
{
	public sealed class MonsterUserDataMng : ClassSingleton<MonsterUserDataMng>
	{
		private List<MonsterData> userMonsterList;

		private string[] colosseumDeckUserMonsterIdList;

		private MonsterData GetMonsterUserData(string userMonsterId)
		{
			MonsterData result = null;
			for (int i = 0; i < this.userMonsterList.Count; i++)
			{
				if (userMonsterId == this.userMonsterList[i].GetMonster().userMonsterId)
				{
					result = this.userMonsterList[i];
					break;
				}
			}
			return result;
		}

		private string[] GetDeckUserMonsterIdList(int deckIndex)
		{
			GameWebAPI.RespDataMN_GetDeckList.MonsterList[] monsterList = DataMng.Instance().RespDataMN_DeckList.deckList[deckIndex].monsterList;
			string[] array = new string[monsterList.Length];
			for (int i = 0; i < monsterList.Length; i++)
			{
				array[i] = monsterList[i].userMonsterId;
			}
			return array;
		}

		private List<string> GetDeckMonsterPathList(List<string> deckUserMonsterIdList)
		{
			List<string> list = new List<string>();
			if (deckUserMonsterIdList != null)
			{
				for (int i = 0; i < deckUserMonsterIdList.Count; i++)
				{
					MonsterData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(deckUserMonsterIdList[i]);
					list.Add(MonsterObject.GetFilePath(userMonster.GetMonsterMaster().Group.modelId));
				}
			}
			return list;
		}

		public void Initialize()
		{
			if (this.userMonsterList == null)
			{
				this.userMonsterList = new List<MonsterData>();
			}
			else
			{
				this.userMonsterList.Clear();
			}
			this.colosseumDeckUserMonsterIdList = null;
		}

		public void SetUserMonsterData(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] responseUserMonsterList)
		{
			for (int i = 0; i < responseUserMonsterList.Length; i++)
			{
				MonsterData item = new MonsterData(responseUserMonsterList[i]);
				this.userMonsterList.Add(item);
			}
		}

		public void UpdateUserMonsterData(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] responseUserMonsterList)
		{
			for (int i = 0; i < responseUserMonsterList.Length; i++)
			{
				this.UpdateUserMonsterData(responseUserMonsterList[i]);
			}
		}

		public void UpdateUserMonsterData(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList responseUserMonster)
		{
			MonsterData monsterUserData = this.GetMonsterUserData(responseUserMonster.userMonsterId);
			Debug.Assert(null != monsterUserData, "該当情報が無い userMonsterId (" + responseUserMonster.userMonsterId + ")");
			monsterUserData.SetMonster(responseUserMonster);
		}

		public void AddUserMonsterData(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] responseUserMonsterList)
		{
			for (int i = 0; i < responseUserMonsterList.Length; i++)
			{
				this.AddUserMonsterData(responseUserMonsterList[i]);
			}
		}

		public void AddUserMonsterData(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList responseUserMonster)
		{
			Debug.Assert(null == this.GetMonsterUserData(responseUserMonster.userMonsterId), "既に一覧に登録されています. userMonsterId (" + responseUserMonster.userMonsterId + ")");
			MonsterData item = new MonsterData(responseUserMonster);
			this.userMonsterList.Add(item);
		}

		[Obsolete("Please refer to the Summary.")]
		public void RefreshUserMonsterData(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] responseUserMonsterList)
		{
			for (int i = 0; i < responseUserMonsterList.Length; i++)
			{
				MonsterData monsterData = this.GetMonsterUserData(responseUserMonsterList[i].userMonsterId);
				if (monsterData != null)
				{
					monsterData.SetMonster(responseUserMonsterList[i]);
				}
				else
				{
					monsterData = new MonsterData(responseUserMonsterList[i]);
					this.userMonsterList.Add(monsterData);
				}
			}
		}

		public void DeleteUserMonsterData(string[] userMonsterIdList)
		{
			for (int i = 0; i < userMonsterIdList.Length; i++)
			{
				this.DeleteUserMonsterData(userMonsterIdList[i]);
			}
		}

		public void DeleteUserMonsterData(string userMonsterId)
		{
			MonsterData monsterUserData = this.GetMonsterUserData(userMonsterId);
			Debug.Assert(null != monsterUserData, "該当情報が無い userMonsterId (" + userMonsterId + ")");
			this.userMonsterList.Remove(monsterUserData);
		}

		public MonsterData GetUserMonster(int userMonsterId)
		{
			return this.GetUserMonster(string.Format("{0}", userMonsterId));
		}

		public MonsterData GetUserMonster(string userMonsterId)
		{
			return this.GetMonsterUserData(userMonsterId);
		}

		public List<MonsterData> GetMonsterList(string monsterId)
		{
			List<MonsterData> list = new List<MonsterData>();
			for (int i = 0; i < this.userMonsterList.Count; i++)
			{
				if (monsterId == this.userMonsterList[i].GetMonster().monsterId)
				{
					list.Add(this.userMonsterList[i]);
				}
			}
			return list;
		}

		public MonsterData GetOldestMonster()
		{
			MonsterData result = null;
			int num = -1;
			for (int i = 0; i < this.userMonsterList.Count; i++)
			{
				int num2 = int.Parse(this.userMonsterList[i].GetMonster().userMonsterId);
				if (num == -1 || num > num2)
				{
					num = num2;
					result = this.userMonsterList[i];
				}
			}
			return result;
		}

		public int GetMonsterNum()
		{
			return this.userMonsterList.Count;
		}

		public List<string> GetDeckUserMonsterIdList()
		{
			List<string> list = new List<string>();
			GameWebAPI.RespDataMN_GetDeckList.DeckList[] deckList = DataMng.Instance().RespDataMN_DeckList.deckList;
			for (int i = 0; i < deckList.Length; i++)
			{
				list.AddRange(this.GetDeckUserMonsterIdList(i));
			}
			return list;
		}

		public List<string> GetFavoriteDeckUserMonsterIdList()
		{
			List<string> list = new List<string>();
			GameWebAPI.RespDataMN_GetDeckList.DeckList[] deckList = DataMng.Instance().RespDataMN_DeckList.deckList;
			string favoriteDeckNum = DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum;
			for (int i = 0; i < deckList.Length; i++)
			{
				if (favoriteDeckNum == deckList[i].deckNum)
				{
					list.AddRange(this.GetDeckUserMonsterIdList(i));
					break;
				}
			}
			return list;
		}

		public List<string> GetDeckMonsterPathList(bool favour)
		{
			List<string> deckUserMonsterIdList;
			if (favour)
			{
				deckUserMonsterIdList = this.GetFavoriteDeckUserMonsterIdList();
			}
			else
			{
				deckUserMonsterIdList = this.GetDeckUserMonsterIdList();
			}
			return this.GetDeckMonsterPathList(deckUserMonsterIdList);
		}

		public void RefreshMonsterSlot()
		{
			for (int i = 0; i < this.userMonsterList.Count; i++)
			{
				MonsterChipEquipData chipEquip = this.userMonsterList[i].GetChipEquip();
				if (chipEquip != null)
				{
					chipEquip.SetChipEquip(this.userMonsterList[i].GetMonster().userMonsterId);
				}
			}
		}

		public void RefreshMonsterSlot(int[] userMonsterIdList)
		{
			if (userMonsterIdList == null)
			{
				return;
			}
			for (int i = 0; i < userMonsterIdList.Length; i++)
			{
				string text = userMonsterIdList[i].ToString();
				MonsterData monsterUserData = this.GetMonsterUserData(text);
				if (monsterUserData != null)
				{
					MonsterChipEquipData chipEquip = monsterUserData.GetChipEquip();
					if (chipEquip != null)
					{
						chipEquip.SetChipEquip(text);
					}
				}
			}
		}

		public List<MonsterData> GetDeckUserMonsterList()
		{
			List<string> deckUserMonsterIdList = this.GetDeckUserMonsterIdList();
			List<MonsterData> list = new List<MonsterData>();
			for (int i = 0; i < deckUserMonsterIdList.Count; i++)
			{
				MonsterData userMonster = this.GetUserMonster(deckUserMonsterIdList[i]);
				if (userMonster != null)
				{
					list.Add(userMonster);
				}
			}
			return list;
		}

		public bool ExistColosseumDeckData()
		{
			return null != this.colosseumDeckUserMonsterIdList;
		}

		public bool FindMonsterColosseumDeck(string userMonsterId)
		{
			bool result = false;
			for (int i = 0; i < this.colosseumDeckUserMonsterIdList.Length; i++)
			{
				if (!string.IsNullOrEmpty(this.colosseumDeckUserMonsterIdList[i]) && userMonsterId == this.colosseumDeckUserMonsterIdList[i])
				{
					result = true;
				}
			}
			return result;
		}

		public void SetColosseumDeckUserMonster(string[] userMonsteridList)
		{
			this.colosseumDeckUserMonsterIdList = userMonsteridList;
		}

		public string[] GetColosseumDeckUserMonsterIdList()
		{
			return this.colosseumDeckUserMonsterIdList;
		}

		public List<MonsterData> GetColosseumDeckUserMonsterList()
		{
			List<MonsterData> list = new List<MonsterData>();
			if (this.colosseumDeckUserMonsterIdList != null)
			{
				for (int i = 0; i < this.colosseumDeckUserMonsterIdList.Length; i++)
				{
					if (this.colosseumDeckUserMonsterIdList[i] != null)
					{
						MonsterData monsterUserData = this.GetMonsterUserData(this.colosseumDeckUserMonsterIdList[i]);
						list.Add(monsterUserData);
					}
				}
			}
			return list;
		}

		public static bool AnyHighGrowStepMonster(List<MonsterData> monsterUserDataList)
		{
			bool result = false;
			for (int i = 0; i < monsterUserDataList.Count; i++)
			{
				if (MonsterGrowStepData.IsGrowStepHigh(monsterUserDataList[i].GetMonsterMaster().Group.growStep))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public static bool AnyChipEquipMonster(List<MonsterData> monsterUserDataList)
		{
			bool result = false;
			for (int i = 0; i < monsterUserDataList.Count; i++)
			{
				if (monsterUserDataList[i].GetChipEquip().IsAttachedChip())
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public List<MonsterData> GetUserMonsterList()
		{
			return this.userMonsterList;
		}
	}
}
