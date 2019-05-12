using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class PartyEditPartyMember : MonoBehaviour
{
	[SerializeField]
	private GameObject listRootParent;

	[SerializeField]
	private GameObject listParts;

	private GUISelectPanelPartyEdit listRoot;

	private void InitializeList(CMD_PartyEdit cmdPartyEdit)
	{
		GameObject gameObject = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelPartyEdit", base.gameObject);
		this.listRoot = gameObject.GetComponent<GUISelectPanelPartyEdit>();
		this.listRoot.partyEdit = cmdPartyEdit;
		if (null != this.listRootParent)
		{
			gameObject.transform.parent = this.listRootParent.transform;
		}
		Vector3 localPosition = this.listParts.transform.localPosition;
		gameObject.transform.localPosition.y = localPosition.y;
		GUICollider component = gameObject.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		this.listRoot.selectParts = this.listParts;
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -525f + localPosition.x;
		listWindowViewRect.xMax = 525f + localPosition.x;
		listWindowViewRect.yMin = -240f;
		listWindowViewRect.yMax = 240f;
		this.listRoot.ListWindowViewRect = listWindowViewRect;
	}

	private void SetMonsterList()
	{
		GameWebAPI.RespDataMN_GetDeckList.DeckList[] deckList = DataMng.Instance().RespDataMN_DeckList.deckList;
		string selectDeckNum = DataMng.Instance().RespDataMN_DeckList.selectDeckNum;
		int selectedIndexRev = selectDeckNum.ToInt32() - 1;
		this.listRoot.initLocation = true;
		this.listRoot.AllBuild(deckList.ToList<GameWebAPI.RespDataMN_GetDeckList.DeckList>());
		this.listRoot.SetSelectedIndexRev(selectedIndexRev);
		this.listParts.SetActive(false);
	}

	private void SetUserDeckMonsterId(string position, string userMonsterId, GameWebAPI.RespDataMN_GetDeckList.MonsterList[] monsterList)
	{
		for (int i = 0; i < monsterList.Length; i++)
		{
			if (monsterList[i].position == position)
			{
				monsterList[i].userMonsterId = userMonsterId;
				break;
			}
		}
	}

	private void UpdateUserDeckMonsterId(int[] userMonsterIdList, GameWebAPI.RespDataMN_GetDeckList.MonsterList[] deckMonsterList)
	{
		for (int i = 0; i < userMonsterIdList.Length; i++)
		{
			string position = string.Format("{0}", i + 1);
			string userMonsterId = string.Format("{0}", userMonsterIdList[i]);
			this.SetUserDeckMonsterId(position, userMonsterId, deckMonsterList);
		}
	}

	private void UpdateUserDeck(int selectDeckNo, int favoriteDeckNo, int[][] deckList)
	{
		GameWebAPI.RespDataMN_GetDeckList.DeckList[] deckList2 = DataMng.Instance().RespDataMN_DeckList.deckList;
		for (int i = 0; i < deckList.Length; i++)
		{
			this.UpdateUserDeckMonsterId(deckList[i], deckList2[i].monsterList);
		}
		DataMng.Instance().RespDataMN_DeckList.selectDeckNum = selectDeckNo.ToString();
		DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum = favoriteDeckNo.ToString();
	}

	private int ConvertPartyIndex(int partyNo)
	{
		return this.listRoot.fastSetPartObjs.Count - (partyNo + 1);
	}

	public void SetView(CMD_PartyEdit cmdPartyEdit)
	{
		this.InitializeList(cmdPartyEdit);
		this.SetMonsterList();
	}

	public void SetChangeMonsterEvent(Action onChangeMonster)
	{
		this.listRoot.popupCallback = onChangeMonster;
	}

	public void HideParts()
	{
		for (int i = 0; i < this.listRoot.fastSetPartObjs.Count; i++)
		{
			this.listRoot.fastSetPartObjs[i].HideClipObjects();
		}
	}

	public bool IsDirty()
	{
		bool result = false;
		for (int i = 0; i < this.listRoot.fastSetPartObjs.Count; i++)
		{
			if (this.listRoot.fastSetPartObjs[i].IsChanged())
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public int GetPartyCount()
	{
		return this.listRoot.fastSetPartObjs.Count;
	}

	public PartsPartyMonsInfo GetLeaderMonsterInfo(int partyNo)
	{
		PartsPartyMonsInfo result = null;
		if (this.listRoot.fastSetPartObjs != null)
		{
			int index = this.ConvertPartyIndex(partyNo);
			if (this.listRoot.fastSetPartObjs[index].ppmiList != null)
			{
				result = this.listRoot.fastSetPartObjs[index].ppmiList[0];
			}
		}
		return result;
	}

	public void SetStatusPage(int partyNo, int pageNo)
	{
		if (this.listRoot.fastSetPartObjs != null)
		{
			int index = this.ConvertPartyIndex(partyNo);
			List<PartsPartyMonsInfo> ppmiList = this.listRoot.fastSetPartObjs[index].ppmiList;
			if (ppmiList != null)
			{
				for (int i = 0; i < ppmiList.Count; i++)
				{
					ppmiList[i].SetStatusPage(pageNo);
				}
			}
		}
	}

	public int[] GetUserMonsterIdList(int partyNo)
	{
		int[] array = null;
		if (this.listRoot.fastSetPartObjs != null)
		{
			int num = this.ConvertPartyIndex(partyNo);
			if (0 <= num && this.listRoot.fastSetPartObjs.Count > num && null != this.listRoot.fastSetPartObjs[num])
			{
				array = this.listRoot.fastSetPartObjs[num].GetChanged();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == 0)
					{
						array = null;
						break;
					}
				}
			}
		}
		return array;
	}

	public List<MonsterData> GetMonsterDataList(int partyNo)
	{
		List<MonsterData> result = new List<MonsterData>();
		if (this.listRoot.fastSetPartObjs != null)
		{
			int num = this.ConvertPartyIndex(partyNo);
			if (0 <= num && this.listRoot.fastSetPartObjs.Count > num && null != this.listRoot.fastSetPartObjs[num])
			{
				result = this.listRoot.fastSetPartObjs[num].GetNowMD();
			}
		}
		return result;
	}

	public List<string> GetCharaModelPathList(int partyNo)
	{
		List<string> list = new List<string>();
		if (this.listRoot.fastSetPartObjs != null && this.listRoot.fastSetPartObjs.Count > partyNo)
		{
			List<MonsterData> nowMD = this.listRoot.fastSetPartObjs[partyNo].GetNowMD();
			for (int i = 0; i < nowMD.Count; i++)
			{
				string monsterCharaPathByMonsterGroupId = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(nowMD[i].monsterM.monsterGroupId);
				list.Add(monsterCharaPathByMonsterGroupId);
			}
		}
		return list;
	}

	public void RefreshMonsterInfo(bool flag)
	{
		this.listRoot.ReloadAllCharacters(flag);
	}

	public APIRequestTask RequestSaveUserDeck(int selectDeckNo, int favoriteDeckNo)
	{
		APIRequestTask result = null;
		int count = this.listRoot.fastSetPartObjs.Count;
		int[][] deckList = new int[count][];
		for (int i = 0; i < count; i++)
		{
			deckList[i] = this.GetUserMonsterIdList(i);
			if (deckList[i] == null)
			{
				deckList = null;
				break;
			}
		}
		if (deckList != null)
		{
			GameWebAPI.RequestMN_DeckEdit request = new GameWebAPI.RequestMN_DeckEdit
			{
				SetSendData = delegate(GameWebAPI.MN_Req_EditDeckList param)
				{
					param.deckData = deckList;
					param.selectDeckNum = selectDeckNo;
					param.favoriteDeckNum = favoriteDeckNo;
				},
				OnReceived = delegate(GameWebAPI.RespDataMN_EditDeckList response)
				{
					this.UpdateUserDeck(selectDeckNo, favoriteDeckNo, deckList);
				}
			};
			result = new APIRequestTask(request, true);
		}
		return result;
	}

	public bool IsScroll()
	{
		bool result = false;
		if (0f < this.listRoot.GetScrollSpeed() || !this.listRoot.IsStopRev())
		{
			result = true;
		}
		return result;
	}
}
