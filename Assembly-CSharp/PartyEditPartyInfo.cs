using Master;
using System;
using UnityEngine;

public sealed class PartyEditPartyInfo : MonoBehaviour
{
	[SerializeField]
	private UILabel partyNo;

	[SerializeField]
	private MonsterLeaderSkill monsterLeaderSkill;

	[SerializeField]
	private GameObject statusChangeButton;

	[SerializeField]
	private UISprite nameUnderLine;

	[SerializeField]
	private UISprite infoUnderLine;

	[SerializeField]
	private PartyEditPartyInfo.ViewInfo partyEditViewInfo;

	public void SetView(CMD_PartyEdit.MODE_TYPE type)
	{
		if (type == CMD_PartyEdit.MODE_TYPE.EDIT)
		{
			Transform transform = this.statusChangeButton.transform;
			transform.localPosition = new Vector3(this.partyEditViewInfo.statusChangeButton.x, this.partyEditViewInfo.statusChangeButton.y, transform.localPosition.z);
			this.nameUnderLine.width = this.partyEditViewInfo.nameUnderLine;
			this.infoUnderLine.width = this.partyEditViewInfo.infoUnderLine;
		}
	}

	public void SetPartyInfo(int viewPartyNo, MonsterData monsterData)
	{
		this.partyNo.text = string.Format(StringMaster.GetString("PartyNumber"), viewPartyNo);
		this.monsterLeaderSkill.SetSkill(monsterData);
	}

	[Serializable]
	private sealed class ViewInfo
	{
		public Vector2 statusChangeButton;

		[SerializeField]
		public int nameUnderLine;

		[SerializeField]
		public int infoUnderLine;
	}
}
