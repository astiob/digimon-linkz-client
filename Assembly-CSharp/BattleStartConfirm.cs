using System;
using System.Collections.Generic;

public class BattleStartConfirm : TCPData<BattleStartConfirm>
{
	public int ri;

	public List<BattleStartConfirm.userData> umis;

	public int mpid;

	public int userDungeonTicketId;

	public class userData
	{
		public int ui;

		public List<int> umi;
	}
}
