using DigiChat.Tools;
using System;
using UnityEngine;

public class GUISelectChatLogPanel : GUISelectPanelBSPartsUD
{
	private float panelUpdateTime;

	[SerializeField]
	private GameObject goBaseParts;

	private float hsize;

	private float startPartH;

	private float ypos;

	private float allListHight;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
		this.panelUpdateTime += Time.deltaTime;
		if (this.panelUpdateTime > 1f && base.gameObject.transform.localPosition.y < base.minLocate - 150f && !CMD_ChatWindow.instance.isGetChatLogListMax)
		{
			CMD_ChatWindow.instance.setChatMessageHistory(false);
			this.panelUpdateTime = 0f;
		}
	}

	public void AllBuild(GameWebAPI.RespData_ChatNewMessageHistoryLogic data)
	{
		CMD_ChatWindow.instance.nowPartsCount = data.result.Length;
		ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupLastHistoryId = data.result[data.result.Length - 1].chatMessageHistoryId;
		if (base.selectCollider != null)
		{
			bool flag = true;
			this.allListHight = 0f;
			foreach (GameWebAPI.RespData_ChatNewMessageHistoryLogic.Result result2 in data.result)
			{
				BoxCollider component = this.goBaseParts.GetComponent<BoxCollider>();
				string text;
				Vector3 colliderSize = ChatTools.GetColliderSize(result2, component, CMD_ChatWindow.instance.goBaseTXT, out text);
				this.allListHight += colliderSize.y;
				if (flag)
				{
					if (result2.type == 3)
					{
						this.startPartH = colliderSize.y + 40f;
					}
					else
					{
						this.startPartH = colliderSize.y;
					}
					CMD_ChatWindow.instance.nowLastMessageId = result2.chatMessageHistoryId;
					flag = false;
				}
			}
			base.InitBuild();
			this.hsize = this.allListHight + this.verticalBorder * 2f - this.verticalMargin;
			this.ypos = this.hsize / 2f - this.startPartH / 2f;
			foreach (GameWebAPI.RespData_ChatNewMessageHistoryLogic.Result respDataChatMessageDataResult in data.result)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListChatLogParts component2 = gameObject.GetComponent<GUIListChatLogParts>();
				if (component2 != null)
				{
					component2.SetOriginalPos(new Vector3(0f, this.ypos, -5f));
					component2.RespDataChatMessageDataResult = respDataChatMessageDataResult;
				}
				this.ypos -= component2.listColliderHeight;
			}
			base.height = this.hsize;
			base.InitMinMaxLocation(-1, 0f);
		}
	}

	public void LogPartsBuild(GameWebAPI.Common_MessageData data)
	{
		CMD_ChatWindow.instance.nowPartsCount++;
		ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupLastHistoryId = data.chatMessageHistoryId.ToString();
		if (base.selectCollider != null)
		{
			GameObject gameObject = base.AddBuildPart();
			GUIListChatLogParts component = gameObject.GetComponent<GUIListChatLogParts>();
			if (component != null)
			{
				component.SetOriginalPos(new Vector3(0f, this.ypos, -5f));
				component.RespDataChatMessageDataResult = data;
				this.allListHight += component.listColliderHeight;
			}
			this.hsize = this.allListHight + this.verticalBorder * 2f - this.verticalMargin;
			this.ypos = this.hsize / 2f - this.startPartH / 2f;
			foreach (GUIListPartBS guilistPartBS in this.partObjs)
			{
				guilistPartBS.gameObject.transform.localPosition = new Vector3(guilistPartBS.gameObject.transform.localPosition.x, this.ypos, guilistPartBS.gameObject.transform.localPosition.z);
				BoxCollider component2 = guilistPartBS.gameObject.GetComponent<BoxCollider>();
				this.ypos -= component2.size.y;
			}
			base.height = this.hsize;
			base.initEffectFlg = true;
			base.InitMinMaxLocation(-1, 0f);
		}
	}

	public void PastListBuild(GameWebAPI.RespData_ChatNewMessageHistoryLogic data)
	{
		int num = data.result.Length;
		float num2 = 0f;
		CMD_ChatWindow.instance.nowPartsCount = CMD_ChatWindow.instance.nowPartsCount + data.result.Length;
		if (CMD_ChatWindow.instance.nowPartsCount >= ChatConstValue.CHATLOG_VIEW_LIST_MAX_NUM)
		{
			CMD_ChatWindow.instance.isGetChatLogListMax = true;
		}
		if (base.selectCollider != null)
		{
			int num3 = num - 1;
			while (0 <= num3)
			{
				GameObject gameObject = base.InsertBuildPart(0);
				GUIListChatLogParts component = gameObject.GetComponent<GUIListChatLogParts>();
				component.RespDataChatMessageDataResult = data.result[num3];
				this.allListHight += component.listColliderHeight;
				if (num3 == 0)
				{
					if (data.result[num3].type == 3)
					{
						this.startPartH = component.listColliderHeight + 40f;
					}
					else
					{
						this.startPartH = component.listColliderHeight;
					}
					CMD_ChatWindow.instance.nowLastMessageId = data.result[num3].chatMessageHistoryId;
				}
				num3--;
			}
			this.hsize = this.allListHight + this.verticalBorder * 2f - this.verticalMargin;
			this.ypos = this.hsize / 2f - this.startPartH / 2f;
			int num4 = 0;
			foreach (GUIListPartBS guilistPartBS in this.partObjs)
			{
				guilistPartBS.gameObject.transform.localPosition = new Vector3(guilistPartBS.gameObject.transform.localPosition.x, this.ypos, -5f);
				BoxCollider component2 = guilistPartBS.gameObject.GetComponent<BoxCollider>();
				this.ypos -= component2.size.y;
				if (num < ChatConstValue.CHATLOG_VIEW_LIST_INIT_NUM && num4 == num - 1)
				{
					num2 = this.ypos;
				}
				else if (num == ChatConstValue.CHATLOG_VIEW_LIST_INIT_NUM && num4 == ChatConstValue.CHATLOG_VIEW_LIST_INIT_NUM - 1)
				{
					num2 = this.ypos;
				}
				if (num4 == ChatConstValue.CHATLOG_VIEW_LIST_INIT_NUM)
				{
					num2 -= component2.size.y;
				}
				num4++;
			}
			base.height = this.hsize;
			base.initMaxLocation = false;
			base.initEffectFlg = true;
			this.selectLoc = num2 * -1f;
			base.InitMinMaxLocation(-1, 0f);
		}
	}
}
