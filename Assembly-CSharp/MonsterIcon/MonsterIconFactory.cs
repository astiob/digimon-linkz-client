using System;
using UnityEngine;

namespace MonsterIcon
{
	public static class MonsterIconFactory
	{
		public const int PARTS_SIMPLE = 0;

		public const int PARTS_IMAGE = 0;

		public const int PARTS_MESSAGE = 1;

		public const int PARTS_NEW_ICON = 2;

		public const int PARTS_LOCK_ICON = 4;

		public const int PARTS_AROUSAL = 8;

		public const int PARTS_MEDAL = 16;

		public const int PARTS_PLAYER_NO = 32;

		public const int PARTS_GIMMICK_ICON = 64;

		private static MonsterIconPartsPool partsPool;

		private static GameObject LoadPrefab(string path)
		{
			GameObject original = AssetDataMng.Instance().LoadObject(path, null, true) as GameObject;
			return UnityEngine.Object.Instantiate<GameObject>(original);
		}

		private static GameObject GetThumbnailGameObject()
		{
			if (null == MonsterIconFactory.partsPool.Thumbnail)
			{
				MonsterIconFactory.partsPool.SetThumbnail(MonsterIconFactory.LoadPrefab("UI/Common/MonsterIcon/MonsterThumbnail"));
			}
			return MonsterIconFactory.partsPool.Thumbnail;
		}

		private static GameObject GetMessageGameObject()
		{
			if (null == MonsterIconFactory.partsPool.Message)
			{
				MonsterIconFactory.partsPool.SetMessage(MonsterIconFactory.LoadPrefab("UI/Common/MonsterIcon/MonsterIconText"));
			}
			return MonsterIconFactory.partsPool.Message;
		}

		private static GameObject GetNewIconGameObject()
		{
			if (null == MonsterIconFactory.partsPool.NewIcon)
			{
				MonsterIconFactory.partsPool.SetNewIcon(MonsterIconFactory.LoadPrefab("UI/Common/MonsterIcon/MonsterIconNew"));
			}
			return MonsterIconFactory.partsPool.NewIcon;
		}

		private static GameObject GetLockIconGameObject()
		{
			if (null == MonsterIconFactory.partsPool.LockIcon)
			{
				MonsterIconFactory.partsPool.SetLockIcon(MonsterIconFactory.LoadPrefab("UI/Common/MonsterIcon/MonsterIconLock"));
			}
			return MonsterIconFactory.partsPool.LockIcon;
		}

		private static GameObject GetArousalGameObject()
		{
			if (null == MonsterIconFactory.partsPool.Arousal)
			{
				MonsterIconFactory.partsPool.SetArousal(MonsterIconFactory.LoadPrefab("UI/Common/MonsterIcon/MonsterIconArousal"));
			}
			return MonsterIconFactory.partsPool.Arousal;
		}

		private static GameObject GetMedalGameObject()
		{
			if (null == MonsterIconFactory.partsPool.Medal)
			{
				MonsterIconFactory.partsPool.SetMedal(MonsterIconFactory.LoadPrefab("UI/Common/MonsterIcon/MonsterIconMedal"));
			}
			return MonsterIconFactory.partsPool.Medal;
		}

		private static GameObject GetPlayerNoGameObject()
		{
			if (null == MonsterIconFactory.partsPool.PlayerNo)
			{
				MonsterIconFactory.partsPool.SetPlayerNo(MonsterIconFactory.LoadPrefab("UI/Common/MonsterIcon/MonsterIconPlayerNo"));
			}
			return MonsterIconFactory.partsPool.PlayerNo;
		}

		private static GameObject GetGimmickIconGameObject()
		{
			if (null == MonsterIconFactory.partsPool.GimmickIcon)
			{
				MonsterIconFactory.partsPool.SetGimmickIcon(MonsterIconFactory.LoadPrefab("UI/Common/MonsterIcon/MonsterIconGimmick"));
			}
			return MonsterIconFactory.partsPool.GimmickIcon;
		}

		public static void SetPartsPool(MonsterIconPartsPool pool)
		{
			MonsterIconFactory.partsPool = pool;
		}

		public static MonsterIcon CreateIcon(int partsFlag)
		{
			MonsterIcon monsterIcon = new MonsterIcon();
			monsterIcon.SetThumbnail(MonsterIconFactory.GetThumbnailGameObject());
			if (0 < (1 & partsFlag))
			{
				monsterIcon.SetMessage(MonsterIconFactory.GetMessageGameObject());
			}
			if (0 < (2 & partsFlag))
			{
				monsterIcon.SetNewIcon(MonsterIconFactory.GetNewIconGameObject());
			}
			if (0 < (4 & partsFlag))
			{
				monsterIcon.SetLockIcon(MonsterIconFactory.GetLockIconGameObject());
			}
			if (0 < (8 & partsFlag))
			{
				monsterIcon.SetArousal(MonsterIconFactory.GetArousalGameObject());
			}
			if (0 < (16 & partsFlag))
			{
				monsterIcon.SetMedal(MonsterIconFactory.GetMedalGameObject());
			}
			if (0 < (32 & partsFlag))
			{
				monsterIcon.SetPlayerNo(MonsterIconFactory.GetPlayerNoGameObject());
			}
			if (0 < (64 & partsFlag))
			{
				monsterIcon.SetGimmickIcon(MonsterIconFactory.GetGimmickIconGameObject());
			}
			return monsterIcon;
		}

		public static MonsterIcon Copy(MonsterIcon icon)
		{
			MonsterIcon result = new MonsterIcon();
			icon.Copy(ref result);
			return result;
		}
	}
}
