using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DigiChat.Tools
{
	public sealed class ChatTools
	{
		private static int cgi;

		[CompilerGenerated]
		private static Action<string> <>f__mg$cache0;

		[CompilerGenerated]
		private static Action<Dictionary<string, object>> <>f__mg$cache1;

		public static Vector3 GetColliderSize(GameWebAPI.Common_MessageData resp, BoxCollider bc, GameObject tmpTxtOb, out string outStr)
		{
			string txt = resp.message;
			if (resp.ngwordFlg == 1)
			{
				txt = StringMaster.GetString("ChatLog-04");
			}
			else if (BlockManager.instance().blockList != null && resp.type != 3)
			{
				int count = BlockManager.instance().blockList.Where((GameWebAPI.FriendList item) => item.userData.userId == resp.userId).ToList<GameWebAPI.FriendList>().Count;
				if (count > 0)
				{
					txt = StringMaster.GetString("ChatLog-05");
				}
			}
			int num;
			if (resp.type == 4)
			{
				num = 60;
				if (resp.message != null)
				{
					outStr = resp.message;
				}
				else
				{
					outStr = string.Empty;
				}
			}
			else if (resp.type == 3)
			{
				num = ChatTools.CheckCommentSize(txt, 635, tmpTxtOb, out outStr) + 30;
			}
			else
			{
				num = ChatTools.CheckCommentSize(txt, 635, tmpTxtOb, out outStr);
			}
			if (resp.type == 3)
			{
				return new Vector3(bc.size.x, bc.size.y + (float)(num - 30 - 40 - 30), bc.size.z);
			}
			return new Vector3(bc.size.x, bc.size.y + (float)(num - 30), bc.size.z);
		}

		public static string OutputDateCtrl(string compDate)
		{
			string a = DateTime.Parse(ServerDateTime.Now.ToString()).ToString("yyyyMMdd");
			string a2 = DateTime.Parse(ServerDateTime.Now.ToString()).AddDays(-1.0).ToString("yyyyMMdd");
			string b = DateTime.Parse(compDate).ToString("yyyyMMdd");
			string result;
			if (a == b)
			{
				result = DateTime.Parse(compDate).ToString(StringMaster.GetString("ChatLog-08"));
			}
			else if (a2 == b)
			{
				result = DateTime.Parse(compDate).ToString(StringMaster.GetString("ChatLog-07"));
			}
			else
			{
				result = DateTime.Parse(compDate).ToString(StringMaster.GetString("ChatLog-06"));
			}
			return result;
		}

		public static int CheckCommentSize(string txt, int maxTextWidth, GameObject ob, out string ostr)
		{
			int num = 1;
			string text = string.Empty;
			ostr = string.Empty;
			int num2 = 0;
			if (txt != null)
			{
				num2 = txt.Length;
			}
			UILabel component = ob.GetComponent<UILabel>();
			for (int i = 0; i < num2; i++)
			{
				text += txt[i];
				component.text = text;
				if (component.width > maxTextWidth)
				{
					ostr += text;
					if (i != num2 - 1)
					{
						num++;
						ostr += "\n";
					}
					text = string.Empty;
					component.text = string.Empty;
				}
			}
			ostr += text;
			int result;
			if (num > 1)
			{
				result = 30 * num;
			}
			else
			{
				result = 30;
			}
			return result;
		}

		public static void chatGroupMaxJoinDialog()
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
			cmd_ModalMessage.Info = StringMaster.GetString("ChatSearch-05");
		}

		public static void SetSystemMessageTCP()
		{
			TCPUtil instance = Singleton<TCPUtil>.Instance;
			if (ChatTools.<>f__mg$cache0 == null)
			{
				ChatTools.<>f__mg$cache0 = new Action<string>(ChatTools.AfterPrepareTCPServer);
			}
			instance.PrepareTCPServer(ChatTools.<>f__mg$cache0, "chat");
		}

		private static void AfterPrepareTCPServer(string server)
		{
			Singleton<TCPUtil>.Instance.MakeTCPClient();
			TCPUtil instance = Singleton<TCPUtil>.Instance;
			if (ChatTools.<>f__mg$cache1 == null)
			{
				ChatTools.<>f__mg$cache1 = new Action<Dictionary<string, object>>(ChatTools.GetTCPSystemReceponseData);
			}
			instance.SetTCPCallBackMethod(ChatTools.<>f__mg$cache1);
		}

		private static void GetTCPSystemReceponseData(Dictionary<string, object> data)
		{
			foreach (KeyValuePair<string, object> keyValuePair in data)
			{
				Dictionary<object, object> dictionary = (Dictionary<object, object>)keyValuePair.Value;
				foreach (KeyValuePair<object, object> keyValuePair2 in dictionary)
				{
					if (keyValuePair2.Key.ToString() == "cgi")
					{
						ChatTools.cgi = Convert.ToInt32(keyValuePair2.Value);
					}
				}
				if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId == ChatTools.cgi)
				{
					Singleton<TCPUtil>.Instance.TCPDisConnect(false);
				}
			}
		}

		public static void UpdatePrefsHistoryIdList(string diffGroupId, string diffHistoryId)
		{
			string text = string.Empty;
			string arg = string.Empty;
			bool flag = false;
			bool flag2 = false;
			if (ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.prefsLastHistoryList != null && ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.prefsLastHistoryList.Count > 0)
			{
				foreach (FaceChatNotification.UserPrefsHistoryIdList userPrefsHistoryIdList in ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.prefsLastHistoryList)
				{
					if (diffGroupId == userPrefsHistoryIdList.historyData.chatGroupId)
					{
						flag2 = true;
						if (diffHistoryId != userPrefsHistoryIdList.historyData.chatMessageHistoryId)
						{
							flag = true;
							arg = diffHistoryId;
						}
						else
						{
							arg = userPrefsHistoryIdList.historyData.chatMessageHistoryId;
						}
					}
					else
					{
						arg = userPrefsHistoryIdList.historyData.chatMessageHistoryId;
					}
					text += string.Format("{0}:{1},", userPrefsHistoryIdList.historyData.chatGroupId, arg);
				}
				if (!flag2)
				{
					flag = true;
					text += string.Format("{0}:{1},", diffGroupId, diffHistoryId);
				}
				if (flag)
				{
					text = text.Trim(new char[]
					{
						','
					});
					PlayerPrefs.SetString("lastHistoryIds", text);
				}
			}
			else
			{
				text += string.Format("{0}:{1},", diffGroupId, diffHistoryId);
				text = text.Trim(new char[]
				{
					','
				});
				PlayerPrefs.SetString("lastHistoryIds", text);
			}
		}

		public static Rect MakeChatListRectWindow()
		{
			return new Rect
			{
				xMin = -445f,
				xMax = 445f,
				yMin = -240f - GUIMain.VerticalSpaceSize,
				yMax = 255f + GUIMain.VerticalSpaceSize
			};
		}

		public static Rect MakeChatListMemberRectWindow()
		{
			return new Rect
			{
				xMin = -280f,
				xMax = 280f,
				yMin = -342f - GUIMain.VerticalSpaceSize,
				yMax = 250f + GUIMain.VerticalSpaceSize
			};
		}

		public static bool CheckOnFLG(int s)
		{
			bool result = false;
			if (s == 1)
			{
				result = true;
			}
			return result;
		}

		public static int SetMessageType()
		{
			int result;
			if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaster)
			{
				result = 2;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		public static void ChatLoadDisplay(bool disp = false)
		{
			if (disp)
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			}
			else
			{
				RestrictionInput.EndLoad();
			}
		}
	}
}
