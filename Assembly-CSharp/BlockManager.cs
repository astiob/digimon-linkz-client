using System;
using System.Collections.Generic;
using System.Linq;

public class BlockManager
{
	private static BlockManager _instance;

	public static BlockManager instance()
	{
		if (BlockManager._instance == null)
		{
			BlockManager._instance = new BlockManager();
		}
		return BlockManager._instance;
	}

	public List<GameWebAPI.FriendList> blockList { get; private set; }

	public bool enableBlock
	{
		get
		{
			return this.blockList != null && this.blockList.Count < ConstValue.MAX_BLOCK_COUNT;
		}
	}

	public APIRequestTask RequestBlockList(bool requestRetry = true)
	{
		GameWebAPI.RequestBL_BlockList request = new GameWebAPI.RequestBL_BlockList
		{
			OnReceived = delegate(GameWebAPI.RespDataBL_BlockList response)
			{
				this.blockList = new List<GameWebAPI.FriendList>();
				this.blockList.AddRange(response.blockList);
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public bool CheckBlock(string userId)
	{
		return this.blockList.Any((GameWebAPI.FriendList x) => x.userData.userId == userId);
	}
}
