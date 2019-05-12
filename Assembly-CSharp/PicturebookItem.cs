using MonsterPicturebook;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PicturebookItem : GUIListPartBS
{
	public static readonly int ITEM_COUNT_PER_LINE = 8;

	public static readonly int ICON_TEXTURE_CX = 128;

	public static readonly int ICON_TEXTURE_CY = 128;

	private static List<PicturebookItem.TextureData> _monsterTextureDataList = new List<PicturebookItem.TextureData>();

	private List<PicturebookMonsterIcon> _monsterIconList;

	private List<PicturebookMonster> _monsterDataList;

	public static void MakeAllMonsterTex(CMD_Picturebook cmd, int allMonsterCount)
	{
		for (int i = 0; i < allMonsterCount; i++)
		{
			PicturebookMonster monsterData = cmd.GetMonsterData(i);
			if (monsterData == null)
			{
				break;
			}
			string iconId = monsterData.monsterMaster.Simple.iconId;
			string monsterIconPathByIconId = GUIMonsterIcon.GetMonsterIconPathByIconId(iconId);
			string resourcePath = GUIMonsterIcon.InternalGetMonsterIconPathByIconId(iconId);
			PicturebookItem.TextureData textureData = new PicturebookItem.TextureData();
			textureData._monsterTexture = new Texture2D(PicturebookItem.ICON_TEXTURE_CX, PicturebookItem.ICON_TEXTURE_CY);
			textureData._monsterAlphaTexture = new Texture2D(PicturebookItem.ICON_TEXTURE_CX, PicturebookItem.ICON_TEXTURE_CY);
			PicturebookMonsterIcon.SetTextureMonsterParts(ref textureData, resourcePath, monsterIconPathByIconId);
			PicturebookItem._monsterTextureDataList.Add(textureData);
		}
	}

	public static void ClearMonsterTex()
	{
		PicturebookItem._monsterTextureDataList.Clear();
	}

	protected override void Awake()
	{
		base.Awake();
		this._monsterIconList = new List<PicturebookMonsterIcon>();
		this._monsterDataList = new List<PicturebookMonster>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			this._monsterIconList.Add(base.transform.GetChild(i).GetComponent<PicturebookMonsterIcon>());
		}
	}

	public override void SetData()
	{
		CMD_Picturebook cmd_Picturebook = (CMD_Picturebook)base.GetInstanceCMD();
		this._monsterDataList.Clear();
		for (int i = 0; i < this._monsterIconList.Count; i++)
		{
			int listId = base.IDX * PicturebookItem.ITEM_COUNT_PER_LINE + i;
			PicturebookMonster monsterData = cmd_Picturebook.GetMonsterData(listId);
			this._monsterDataList.Add(monsterData);
		}
	}

	public override void InitParts()
	{
		this.RefreshParts();
	}

	public override void RefreshParts()
	{
		CMD_Picturebook @object = (CMD_Picturebook)base.GetInstanceCMD();
		for (int i = 0; i < this._monsterIconList.Count; i++)
		{
			if (this._monsterDataList[i] != null)
			{
				if (!this._monsterIconList[i].gameObject.activeSelf)
				{
					this._monsterIconList[i].gameObject.SetActive(true);
				}
				int listIndex = base.IDX * PicturebookItem.ITEM_COUNT_PER_LINE + i;
				this.SetIconMonsterData(listIndex, this._monsterIconList[i], this._monsterDataList[i], new Action<PicturebookMonster>(@object.PressMIconShort));
			}
			else if (this._monsterIconList[i].gameObject.activeSelf)
			{
				this._monsterIconList[i].gameObject.SetActive(false);
			}
		}
	}

	private void SetIconMonsterData(int listIndex, PicturebookMonsterIcon monsterIcon, PicturebookMonster monsterData, Action<PicturebookMonster> actionShortPress)
	{
		if (PicturebookItem._monsterTextureDataList.Count > listIndex)
		{
			monsterIcon.SetMonsterIcon(PicturebookItem._monsterTextureDataList[listIndex], monsterData.monsterMaster.Group.growStep, monsterData.isUnknown);
			monsterIcon.SetMonsterData(monsterData);
			if (monsterData.isUnknown)
			{
				monsterIcon.SetTouchAct_S(null);
			}
			else
			{
				monsterIcon.SetTouchAct_S(delegate(PicturebookMonster noop)
				{
					actionShortPress(monsterData);
				});
			}
		}
	}

	public class TextureData
	{
		public Texture2D _monsterTexture;

		public Texture2D _monsterAlphaTexture;

		public bool _isMainTexture;
	}
}
