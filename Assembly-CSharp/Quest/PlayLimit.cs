using Master;
using System;

namespace Quest
{
	public class PlayLimit : ClassSingleton<PlayLimit>
	{
		private GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dngTicket_cache;

		private int dngTicket_UseCount_cache;

		private GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dngPlayLimit_cache;

		private int dngPlayLimit_UseCount_cache;

		public void RecoverPlayLimit(GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dng, Action<GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons> actCallBackRcv)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			GameWebAPI.RequestWD_RecoverPlayLimit request = new GameWebAPI.RequestWD_RecoverPlayLimit
			{
				SetSendData = delegate(GameWebAPI.WD_RecoverPlayLimit param)
				{
					param.worldDungeonId = dng.worldDungeonId;
				},
				OnReceived = delegate(WebAPI.ResponseData res)
				{
					GameWebAPI.RespDataWD_GetDungeonInfo.PlayLimit playLimit = dng.playLimit;
					int num = int.Parse(playLimit.restCount);
					int num2 = int.Parse(playLimit.recoveryCount);
					int num3 = int.Parse(playLimit.maxCount);
					num = num2;
					if (num > num3)
					{
						num = num3;
					}
					playLimit.restCount = num.ToString();
					if (playLimit.recoveryAssetCategoryId == 2)
					{
						int num4 = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
						num4 -= dng.playLimit.recoveryAssetNum;
						DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point = num4;
					}
					else if (playLimit.recoveryAssetCategoryId == 6)
					{
						Singleton<UserDataMng>.Instance.UpdateUserItemNum(playLimit.recoveryAssetValue, -playLimit.recoveryAssetNum);
					}
					else
					{
						Debug.LogError("===================================回数制限DNG: サポートされてないAsssetCategoryID");
					}
					actCallBackRcv(dng);
				}
			};
			AppCoroutine.Start(request.Run(delegate()
			{
				RestrictionInput.EndLoad();
			}, null, null), false);
		}

		public bool PlayLimitCheck(GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dng, Action<int> actCallBackShort, Action<int> actCallBack, int usedCT = 0)
		{
			GameWebAPI.RespDataWD_GetDungeonInfo.PlayLimit playLimit = dng.playLimit;
			if (playLimit != null)
			{
				int num = int.Parse(playLimit.restCount);
				if (num <= 0)
				{
					if (playLimit.recoveryFlg == "1")
					{
						if (playLimit.recoveryAssetCategoryId == 2)
						{
							int useStoneNum = playLimit.recoveryAssetNum;
							int hasStoneNum = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
							CMD_ChangePOP_STONE cd = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE") as CMD_ChangePOP_STONE;
							cd.Title = StringMaster.GetString("QuestPlayLimitNoneTitle");
							cd.OnPushedYesAction = delegate()
							{
								if (hasStoneNum < useStoneNum)
								{
									cd.SetCloseAction(delegate(int idx)
									{
										actCallBackShort(idx);
									});
									cd.ClosePanel(true);
								}
								else
								{
									cd.SetCloseAction(delegate(int idx)
									{
										actCallBack(idx);
									});
									cd.ClosePanel(true);
								}
							};
							cd.Info = string.Format(StringMaster.GetString("QuestPlayLimitNoneInfo"), useStoneNum, num, int.Parse(playLimit.recoveryCount));
							cd.SetDigistone(hasStoneNum, useStoneNum);
						}
						else if (playLimit.recoveryAssetCategoryId == 6)
						{
							GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(playLimit.recoveryAssetValue.ToString());
							int useItemNum = playLimit.recoveryAssetNum;
							int hasItemNum = Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(playLimit.recoveryAssetValue);
							CMD_ChangePOP cd = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP") as CMD_ChangePOP;
							cd.Title = StringMaster.GetString("QuestPlayLimitNoneTitle");
							cd.OnPushedYesAction = delegate()
							{
								if (hasItemNum < useItemNum)
								{
									cd.SetCloseAction(delegate(int idx)
									{
										actCallBackShort(idx);
									});
									cd.ClosePanel(true);
								}
								else
								{
									cd.SetCloseAction(delegate(int idx)
									{
										actCallBack(idx);
									});
									cd.ClosePanel(true);
								}
							};
							cd.Info = string.Format(StringMaster.GetString("QuestPlayLimitNoneInfoItem"), new object[]
							{
								itemM.name,
								useItemNum,
								itemM.unitName,
								num,
								int.Parse(playLimit.recoveryCount)
							});
							cd.SetPoint(hasItemNum, useItemNum);
							if (itemM.img != null && itemM.img.Length > 0)
							{
								cd.SetTextureIcon(itemM.img[0]);
							}
						}
						else
						{
							Debug.LogError("===================================回数制限DNG: サポートされてないAsssetCategoryID");
						}
					}
					else
					{
						CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
						cmd_ModalMessage.Title = StringMaster.GetString("QuestPlayLimitTitle");
						cmd_ModalMessage.Info = StringMaster.GetString("QuestPlayLimitZeroInfo");
					}
					return false;
				}
				if (usedCT > 0)
				{
					this.SetPlayLimitNumCont(dng, usedCT);
				}
			}
			return true;
		}

		public void ClearTicketNumCont()
		{
			this.dngTicket_cache = null;
		}

		public void SetTicketNumCont(GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dng, int useCount)
		{
			this.dngTicket_cache = dng;
			this.dngTicket_UseCount_cache = useCount;
		}

		public void UseTicketNumCont()
		{
			if (this.dngTicket_cache != null)
			{
				int num = int.Parse(this.dngTicket_cache.dungeonTicketNum);
				num -= this.dngTicket_UseCount_cache;
				if (num < 0)
				{
					num = 0;
				}
				this.dngTicket_cache.dungeonTicketNum = num.ToString();
				this.dngTicket_cache = null;
			}
		}

		public void ClearPlayLimitNumCont()
		{
			this.dngPlayLimit_cache = null;
		}

		public void SetPlayLimitNumCont(GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dng, int useCount)
		{
			this.dngPlayLimit_cache = dng;
			this.dngPlayLimit_UseCount_cache = useCount;
		}

		public void UsePlayLimitNumCont()
		{
			if (this.dngPlayLimit_cache != null && this.dngPlayLimit_cache.playLimit != null)
			{
				GameWebAPI.RespDataWD_GetDungeonInfo.PlayLimit playLimit = this.dngPlayLimit_cache.playLimit;
				int num = int.Parse(playLimit.restCount);
				num -= this.dngPlayLimit_UseCount_cache;
				if (num < 0)
				{
					num = 0;
				}
				playLimit.restCount = num.ToString();
				this.dngPlayLimit_cache = null;
			}
		}
	}
}
