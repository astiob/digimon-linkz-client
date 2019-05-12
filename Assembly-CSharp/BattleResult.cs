using System;
using System.Collections.Generic;

public class BattleResult : TCPData<BattleResult>
{
	public int ri;

	public int si;

	public int cf;

	public List<int> amis;

	public List<int> ogis;

	public int clearRound;

	public int uniqueRequestId;

	public int requestStatus;
}
