using Monster;
using MonsterIcon;
using System;

namespace MonsterIconExtensions
{
	public static class MonsterIconCustomize
	{
		public static void SetMonsterImage(this MonsterIcon self, MonsterClientMaster monsterMaster)
		{
			self.Thumbnail.SetImage(monsterMaster.Simple.iconId, monsterMaster.Group.growStep);
		}

		public static void SetMonsterDetailed(this MonsterIcon self, MonsterClientMaster monsterMaster)
		{
			self.Thumbnail.SetImage(monsterMaster.Simple.iconId, monsterMaster.Group.growStep);
			self.Arousal.SetArousal(monsterMaster.Simple.rare);
		}

		public static void SetUserMonsterDetailed(this MonsterIcon self, MonsterUserData monster, bool isGimmick)
		{
			MonsterClientMaster monsterMaster = monster.GetMonsterMaster();
			self.Thumbnail.SetImage(monsterMaster.Simple.iconId, monsterMaster.Group.growStep);
			self.Arousal.SetArousal(monsterMaster.Simple.rare);
			if (monster.GetMonster().IsLocked)
			{
				self.Lock.SetLock();
			}
			if (isGimmick)
			{
				self.Gimmick.SetGimmickIcon();
			}
		}

		public static void SetColosseumDeckMonsterDetailed(this MonsterIcon self, MonsterUserData monster)
		{
			MonsterClientMaster monsterMaster = monster.GetMonsterMaster();
			self.Thumbnail.SetImage(monsterMaster.Simple.iconId, monsterMaster.Group.growStep);
			self.Arousal.SetArousal(monsterMaster.Simple.rare);
			if (monster.GetMonster().IsLocked)
			{
				self.Lock.SetLock();
			}
		}

		public static void SetBattleMonsterDetailed(this MonsterIcon self, MonsterUserData monster, int playerIndex)
		{
			MonsterClientMaster monsterMaster = monster.GetMonsterMaster();
			self.Thumbnail.SetImage(monsterMaster.Simple.iconId, monsterMaster.Group.growStep);
			self.Arousal.SetArousal(monsterMaster.Simple.rare);
			self.PlayerNo.SetPlayerIndex(playerIndex);
		}

		public static void SetGashaMonsterDetailed(this MonsterIcon self, MonsterClientMaster monsterMaster, bool isNew)
		{
			self.Thumbnail.SetImage(monsterMaster.Simple.iconId, monsterMaster.Group.growStep);
			self.Arousal.SetArousal(monsterMaster.Simple.rare);
			if (isNew)
			{
				self.New.SetNew();
			}
		}

		public static void ClaerDetailed(this MonsterIcon self)
		{
			self.Thumbnail.SetEmptyIcon();
			if (null != self.Lock)
			{
				self.Lock.ClearLock();
			}
			if (null != self.New)
			{
				self.New.ClearNew();
			}
			if (null != self.Message)
			{
				self.Message.ClearMessage();
			}
			if (null != self.Arousal)
			{
				self.Arousal.ClearArousal();
			}
			if (null != self.Medal)
			{
				self.Medal.ClearMedal();
			}
			if (null != self.PlayerNo)
			{
				self.PlayerNo.ClearPlayerNo();
			}
			if (null != self.Gimmick)
			{
				self.Gimmick.ClearGimmickIcon();
			}
		}
	}
}
