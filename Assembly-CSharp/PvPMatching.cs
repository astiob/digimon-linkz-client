using System;

public class PvPMatching : TCPData<PvPMatching>
{
	public int act;

	public string targetUserCode;

	public int isMockBattle;

	public int uniqueRequestId;
}
