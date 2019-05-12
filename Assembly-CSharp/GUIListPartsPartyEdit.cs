using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIListPartsPartyEdit : GUIListPartBS
{
	[SerializeField]
	private PartsPartyMonsInfo monsterInfoUI;

	[SerializeField]
	private GameObject[] infoAnchorList;

	private GameWebAPI.RespDataMN_GetDeckList.DeckList deckList;

	[NonSerialized]
	public int partyNumber;

	[NonSerialized]
	public CMD_PartyEdit partyEdit;

	[NonSerialized]
	public GUISelectPanelPartyEdit selectPanelParty;

	public List<PartsPartyMonsInfo> ppmiList { get; set; }

	public void SetDeck(GameWebAPI.RespDataMN_GetDeckList.DeckList data)
	{
		this.deckList = data;
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		PartsPartyMonsInfo[] array = this.CreateMonsterInfo();
		this.ppmiList = new List<PartsPartyMonsInfo>();
		GameWebAPI.RespDataMN_GetDeckList.MonsterList[] monsterList = this.deckList.monsterList;
		for (int i = 0; i < monsterList.Length; i++)
		{
			array[i].Data = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(monsterList[i].userMonsterId, false);
			array[i].ShowGUI();
			this.ppmiList.Add(array[i]);
		}
	}

	private PartsPartyMonsInfo[] CreateMonsterInfo()
	{
		PartsPartyMonsInfo[] array = new PartsPartyMonsInfo[this.infoAnchorList.Length];
		Transform transform = this.infoAnchorList[0].transform;
		this.monsterInfoUI.transform.localPosition = transform.localPosition;
		array[0] = this.monsterInfoUI;
		array[0].guiListPartsPartyEdit = this;
		for (int i = 1; i < this.infoAnchorList.Length; i++)
		{
			array[i] = UnityEngine.Object.Instantiate<PartsPartyMonsInfo>(this.monsterInfoUI);
			array[i].guiListPartsPartyEdit = this;
			transform = this.infoAnchorList[i].transform;
			array[i].transform.parent = this.monsterInfoUI.transform.parent;
			array[i].transform.localScale = this.monsterInfoUI.transform.localScale;
			array[i].transform.localPosition = transform.localPosition;
			array[i].transform.localRotation = this.monsterInfoUI.transform.localRotation;
			array[i].SetOriginalPos(transform.localPosition);
			array[i].SetLeaderMark(false);
		}
		for (int j = 0; j < this.infoAnchorList.Length; j++)
		{
			UnityEngine.Object.Destroy(this.infoAnchorList[j].gameObject);
			this.infoAnchorList[j] = null;
		}
		this.infoAnchorList = null;
		return array;
	}

	public bool IsChanged()
	{
		GameWebAPI.RespDataMN_GetDeckList.MonsterList[] monsterList = this.deckList.monsterList;
		for (int i = 0; i < monsterList.Length; i++)
		{
			string userMonsterId = monsterList[i].userMonsterId;
			int index = int.Parse(monsterList[i].position) - 1;
			MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterId, false);
			PartsPartyMonsInfo partsPartyMonsInfo = this.ppmiList[index];
			if (partsPartyMonsInfo.Data != monsterDataByUserMonsterID)
			{
				return true;
			}
		}
		return false;
	}

	public List<MonsterData> GetNowMD()
	{
		List<MonsterData> list = new List<MonsterData>();
		for (int i = 0; i < this.ppmiList.Count; i++)
		{
			PartsPartyMonsInfo partsPartyMonsInfo = this.ppmiList[i];
			list.Add(partsPartyMonsInfo.Data);
		}
		return list;
	}

	public int[] GetChanged()
	{
		GameWebAPI.RespDataMN_GetDeckList.MonsterList[] monsterList = this.deckList.monsterList;
		int[] array = new int[3];
		for (int i = 0; i < monsterList.Length; i++)
		{
			int num = int.Parse(monsterList[i].position) - 1;
			PartsPartyMonsInfo partsPartyMonsInfo = this.ppmiList[num];
			if (partsPartyMonsInfo.Data != null)
			{
				array[num] = int.Parse(partsPartyMonsInfo.Data.userMonster.userMonsterId);
			}
			else
			{
				array[num] = 0;
			}
		}
		return array;
	}

	public void OnChanged()
	{
		if (null != this.selectPanelParty && this.selectPanelParty.popupCallback != null)
		{
			this.selectPanelParty.popupCallback();
		}
		for (int i = 0; i < this.ppmiList.Count; i++)
		{
			PartsPartyMonsInfo partsPartyMonsInfo = this.ppmiList[i];
			if (null != partsPartyMonsInfo)
			{
				partsPartyMonsInfo.ShowRare();
			}
		}
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchBegan(touch, pos);
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
		if (flag && this.deckList != null)
		{
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f)
			{
			}
		}
	}

	private void OnClickedBtnSelect()
	{
	}

	protected override void Update()
	{
		base.Update();
		this.ChangeNumber();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public void ChangeNumber()
	{
		if (base.gameObject.transform.position.x > -1f && base.gameObject.transform.position.x < 1f && this.partyEdit.idxNumber != this.partyNumber)
		{
			this.partyEdit.idxNumber = this.partyNumber;
		}
	}

	public void ReloadAllCharacters(bool flg)
	{
		for (int i = 0; i < this.ppmiList.Count; i++)
		{
			if (flg)
			{
				this.ppmiList[i].ShowCharacter();
			}
			else
			{
				this.ppmiList[i].ReleaseCharacter();
			}
		}
	}
}
