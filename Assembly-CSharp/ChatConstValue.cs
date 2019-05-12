using System;
using UnityEngine;

public static class ChatConstValue
{
	public const string PREFS_KEY_AGREEMENT_CHAT = "ChatAgreement";

	public const string PREFS_KEY_CHAT_NOTIFICATION = "lastHistoryIds";

	public const string PREFS_KEY_PVP_MOCK_NOTIFICATION = "lastPvPMockTime";

	public const string PREFS_KEY_MULTI_REQ_NOTIFICATION = "lastMultiReqId";

	public const float CHAT_NOTIFICATION_PLATE_HIDE_SEC = 10f;

	public const int CHAT_GROUP_MANY_INVITE_MAX_NUM = 10;

	public const int CHATLOG_MAX_INPUT_NUM = 100;

	public const int CHATLOG_COMMENT_MAX_WIDTH = 635;

	public const int CHATLOG_SYSTEM_COMMENT_MAX_WIDTH = 635;

	public const int CHAT_CHARA_LIMIT_GROUP_NAME = 15;

	public const int CHAT_CHARA_LIMIT_GROUP_COMMENT = 30;

	public const float CHAT_SEND_BUTTON_OPEN_SEC = 3f;

	public const float CHATLOG_REQUEST_WAIT_TIME = 0.3f;

	public const float MULTI_UPDATE_BUTTON_OPEN_SEC = 3f;

	public const int CHAT_LIST_UPDATE_LOCATE_MARGIN = 150;

	public const int CHATLOG_CHAR_WIDTH = 25;

	public const int CHATLOG_HALF_CHAR_WIDTH = 17;

	public const int CHATLOG_CHAR_HEIGHT = 30;

	public const int CHATLOG_SYSTEM_COMMENT_ADJUST_HEIGHT = 40;

	public const int CHATLOG_MULTI_PASSCODE_LINE_COUNT = 2;

	public const int CHATLOG_MULTI_PASSCODE_WIDTH = 300;

	public const int CHAT_OPEN_GROUP_TYPE_INVITE_REQUEST = 1;

	public const int CHAT_OPEN_GROUP_TYPE_USER_REQUEST = 2;

	public const int CHAT_CATEGORY_ID_CAPTURE = 1;

	public const int CHAT_CATEGORY_ID_TALK = 2;

	public const int CHAT_CATEGORY_ID_RELATION = 3;

	public const int CHAT_CATEGORY_ID_OTHER = 4;

	public const int CHAT_MEMBER_LIST_TYPE_FRIEND = 1;

	public const int CHAT_MEMBER_LIST_TYPE_DELEGATE = 2;

	public const int CHAT_MEMBER_LIST_TYPE_MEMBER = 3;

	public const int CHAT_MEMBER_LIST_TYPE_REQUEST = 4;

	public const int CHAT_MEMBER_LIST_TYPE_INVITE = 5;

	public const int CHAT_MESSEGE_TYPE_MEMBER = 1;

	public const int CHAT_MESSEGE_TYPE_OWNER = 2;

	public const int CHAT_MESSEGE_TYPE_SYSTEM = 3;

	public const int CHAT_MESSEGE_TYPE_MULTI = 4;

	public const int OFF_VALUE = 0;

	public const int ON_VALUE = 1;

	public const int LEAVE_GROUP_RESULT_CODE_EXPULSION = 90;

	public const int DIALOG_YES = 0;

	public const int DIALOG_NO = 1;

	public const int APPROVALTYPE_OFF = 1;

	public const int APPROVALTYPE_ON = 2;

	public const int CHAT_RECONNECT_COUNT = 3;

	public const string SPRITE_MEMBER_RELATION_TYPE_SELF = "ListStatus_Waku_Green";

	public const string SPRITE_MEMBER_RELATION_TYPE_BLOCK = "ListStatus_Waku_Red";

	public static readonly string[] SPRITE_GROUP_CATEGORY = new string[]
	{
		string.Empty,
		"ChatCategori_Capture",
		"ChatCategori_Chat",
		"ChatCategori_Interact2",
		"ChatCategori_Interact"
	};

	public const string API_SOCKET_SEND_MESSAGE = "810002";

	public const string API_SOCKET_SEND_EXPULSION = "810003";

	public const string CHAT_MESSEGE_HISTORY_ID_DEFAULT = "0";

	public static readonly string[] WORLD_ID_LIST = new string[]
	{
		"1",
		"3",
		"8"
	};

	public const string PF_CHAT_REQ_LIST = "CMD_ChatReqList";

	public const string PF_CHAT_WINDOW = "CMD_ChatWindow";

	public const string PF_CHAT_MENU = "CMD_ChatMenu";

	public const string PF_CHAT_MENU_MEMBER = "CMD_ChatMenuM";

	public const string PF_CREATE_CHAT_GROUP = "CMD_CreateChatGroup";

	public const string PF_CHAT_MODAL = "CMD_ChatModal";

	public const string PF_CHAT_SORT_MODAL = "CMD_ChatSort";

	public const string PF_CHAT_SEARCH_MODAL = "CMD_ChatSearch";

	public const string PF_CHAT_INVITATION = "CMD_ChatInvitation";

	public const string PF_CHAT_FIXED_PHRASE = "CMD_ChatFixedPhrase";

	public const string PF_AGREEMENT_CHAT = "CMD_AgreementChat";

	public const string PF_MODAL_MESSAGE = "CMD_ModalMessage";

	public const string PF_CONFIRM = "CMD_Confirm";

	public const string PF_PROFILE_FRIEND = "CMD_ProfileFriend";

	public static int CHAT_GROUP_JOIN_MAX_NUM
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.CHAT_GROUP_JOIN_MAX_NUM;
		}
	}

	public static int CHAT_GROUP_MEMBER_MAX_NUM
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.CHAT_GROUP_MEMBER_MAX_NUM;
		}
	}

	public static int CHATLOG_VIEW_LIST_INIT_NUM
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.CHATLOG_VIEW_LIST_INIT_NUM;
		}
	}

	public static int CHATLOG_VIEW_LIST_MAX_NUM
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.CHATLOG_VIEW_LIST_MAX_NUM;
		}
	}

	public static string CHAT_NOTIFICATION_INTERVAL_TIME
	{
		get
		{
			return MasterDataMng.Instance().RespDataMA_CodeM.codeM.CHAT_NOTIFICATION_INTERVAL_TIME;
		}
	}

	public static Color CHAT_DEF_TXT_COLOR
	{
		get
		{
			return new Color(0.227450982f, 0.227450982f, 0.227450982f, 1f);
		}
	}
}
