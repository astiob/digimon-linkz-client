using System;
using System.Collections.Generic;

namespace Picturebook
{
	public static class MonsterPicturebookData
	{
		private static List<GameWebAPI.RespDataMN_Picturebook.UserCollectionData> userPicturebook;

		public static void Initialize()
		{
			if (MonsterPicturebookData.userPicturebook == null)
			{
				MonsterPicturebookData.userPicturebook = new List<GameWebAPI.RespDataMN_Picturebook.UserCollectionData>();
			}
			else
			{
				MonsterPicturebookData.userPicturebook.Clear();
			}
		}

		public static bool IsReady()
		{
			bool result = false;
			for (int i = 0; i < MonsterPicturebookData.userPicturebook.Count; i++)
			{
				if ("1" == MonsterPicturebookData.userPicturebook[i].collectionStatus)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public static APIRequestTask RequestUserPicturebook()
		{
			GameWebAPI.RequestFA_MN_PicturebookExec requestFA_MN_PicturebookExec = new GameWebAPI.RequestFA_MN_PicturebookExec();
			requestFA_MN_PicturebookExec.SetSendData = delegate(GameWebAPI.MN_Req_Picturebook param)
			{
				param.targetUserId = DataMng.Instance().RespDataCM_Login.playerInfo.userId;
			};
			requestFA_MN_PicturebookExec.OnReceived = delegate(GameWebAPI.RespDataMN_Picturebook response)
			{
				MonsterPicturebookData.userPicturebook.AddRange(response.userCollectionList);
			};
			GameWebAPI.RequestFA_MN_PicturebookExec request = requestFA_MN_PicturebookExec;
			return new APIRequestTask(request, false);
		}

		public static List<GameWebAPI.RespDataMN_Picturebook.UserCollectionData> GetUserPicturebook()
		{
			return MonsterPicturebookData.userPicturebook;
		}

		public static bool ExistPicturebook(string collectionId)
		{
			bool result = false;
			for (int i = 0; i < MonsterPicturebookData.userPicturebook.Count; i++)
			{
				if (collectionId == MonsterPicturebookData.userPicturebook[i].monsterCollectionId)
				{
					result = MonsterPicturebookData.userPicturebook[i].IsHave();
					break;
				}
			}
			return result;
		}

		public static void AddPictureBook(string collectionId)
		{
			Debug.Assert(!MonsterPicturebookData.ExistPicturebook(collectionId), "登録済みのモンスター図鑑情報を追加しようとしました.");
			GameWebAPI.RespDataMN_Picturebook.UserCollectionData userCollectionData = new GameWebAPI.RespDataMN_Picturebook.UserCollectionData
			{
				monsterCollectionId = collectionId
			};
			userCollectionData.SetHaveStatus();
			MonsterPicturebookData.userPicturebook.Add(userCollectionData);
		}
	}
}
